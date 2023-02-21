using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;
namespace VFD1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        static string DataSourceCableDuctSchedule = Entity.staticCache.DataSourceCableDuctSchedule;
        string blockVsInsert = "P4_FAB_ARCH_1F$0$Form Plate_ V_ Shared - Form Plate_LFC_ SS275_7T_1500 x 1500-2081056-_EXPORT_ 1F XREF";
        private void btnImport_Click(object sender, EventArgs e)
        {
            Import();

            //}
        }
        private void Import(bool IsDrivenCode = false)
        {
            lyrInstrument = new List<VectorDraw.Professional.vdPrimaries.vdLayer>();
            lyrDestination = new List<VectorDraw.Professional.vdPrimaries.vdLayer>();
            lyrObstacle = new List<VectorDraw.Professional.vdPrimaries.vdLayer>();
            string fname;
            if (!IsDrivenCode)
            {
                object ret = vdFramedControl1.BaseControl.ActiveDocument.GetOpenFileNameDlg(0, "", 0);
                if (ret == null) return;

                DocPath = ret as string;
                fname = (string)ret;
            }
            else
                fname = DocPath;
            bool success = vdFramedControl1.BaseControl.ActiveDocument.Open(fname);
            if (success)
            {
                vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
                BindLayer(vdFramedControl1);
            }
        }
        List<VectorDraw.Professional.vdPrimaries.vdLayer> lyrInstrument;
        List<VectorDraw.Professional.vdPrimaries.vdLayer> lyrDestination;
        List<VectorDraw.Professional.vdPrimaries.vdLayer> lyrObstacle;
        List<Instrument> instruments = new List<Instrument>();
        List<vdPolyline> obstacles = new List<vdPolyline>();
        List<vdCircle> instrument = new List<vdCircle>();
        gPoints gridPoints = new gPoints();
        vdPolyline boundary;
        vdPolyline offsetBoundary;
        Destination destination;
        List<string> blkvdInsertCollection = new List<string>() { };
        double sx = 0;
        double sy = 0;

        private void BindLayer(vdControls.vdFramedControl vfc)
        {
            //VectorDraw.Professional.vdPrimaries.vdBlock obtBlock
            var flayers = vdFramedControl1.BaseControl.ActiveDocument.Layers;
            lyrInstrument = new List<VectorDraw.Professional.vdPrimaries.vdLayer>();
            lyrDestination = new List<VectorDraw.Professional.vdPrimaries.vdLayer>();
            lyrObstacle = new List<VectorDraw.Professional.vdPrimaries.vdLayer>();


            List<VectorDraw.Professional.vdFigures.vdCircle> LstObserver = new List<VectorDraw.Professional.vdFigures.vdCircle>();
            List<VectorDraw.Professional.vdFigures.vdPolyline> Target = new List<VectorDraw.Professional.vdFigures.vdPolyline>();
            List<VectorDraw.Professional.vdFigures.vdPolyline> Obstacle = new List<VectorDraw.Professional.vdFigures.vdPolyline>();
            var allControl = flayers;   // vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.ArrayItems;
            DataTable dtLyrIns = new DataTable();
            dtLyrIns.Columns.Add("colLayer");

            
            foreach (var v in allControl)
            {
                if (v is VectorDraw.Professional.vdPrimaries.vdLayer lyrCircle)
                {
                    if (lyrCircle.Name.ToLower().Contains("instrument"))
                    {
                        lyrInstrument.Add(lyrCircle);
                        dtLyrIns.Rows.Add(new object[] { lyrCircle.Name });
                    }
                    else if (lyrCircle.Name.ToLower().Contains("destination"))
                    {
                        lyrDestination.Add(lyrCircle);
                    }
                    else if (lyrCircle.Name.ToLower().Contains("obstacle"))
                    {
                        lyrObstacle.Add(lyrCircle);
                    }
                }
            }

            //lstInstrument.DataSource = lyrInstrument;
            DataView dv = new DataView(dtLyrIns);
            dv.Sort = "colLayer asc";
            dgvAllLayerInstruments.DataSource = dv;
            cboDesti.DataSource = lyrDestination;
            cboObs.DataSource = lyrObstacle;

            var usedBlk = vdFramedControl1.BaseControl.ActiveDocument.Blocks.GetNotDeletedItems();
            foreach (var ub in usedBlk)
            {
                if (ub is vdBlock vb)
                    blkvdInsertCollection.Add((vb).Name.ToString().Replace("vdInsert ", "").TrimStart());
            }

        }
        private bool Check_1C_2A(vdEntities ve)
        {
            foreach (var lines in ve)
            {
                if (!(lines is vdCircle) && !(lines is vdAttribDef))
                    return false;
            }
            return true;
        }
        private void TryDetect()
        {
            var dtCircles = SettingTable();
            int obsOccurence = 0;
            foreach (vdFigure f in vdFramedControl1.BaseControl.ActiveDocument.Model.Entities)
            {
                if (f != null && f.Layer.Name.ToLower().ToString().Contains("0"))/// == cboObs.SelectedItem.ToString())
                {
                    try
                    {
                        if (f is vdInsert vi && blkvdInsertCollection.Contains(f.ToString().Replace("vdInsert ", "")))
                        {
                            var ent = vi.Explode();
                            if (ent != null && Check_1C_2A(ent))
                            {
                                obsOccurence++;
                                int entityOccurence = 0;
                                var tagName = f.ToString().Replace("vdInsert ", "");
                                var t1 = "";
                                var t2 = "";
                                foreach (var e in ent)
                                {
                                    entityOccurence++;

                                    if (e is vdAttribDef va && va.TextString == "T1")
                                        t1 = va.ValueString;
                                    if (e is vdAttribDef vb && vb.TextString == "T2")
                                        t2 = vb.ValueString;
                                }
                                if (obsOccurence == 1)
                                {
                                    lst_circleBlk.Items.Add(
                                     "Instrument_No" + "     " + "Block Name" + "      " + "T1" + "     " + "T2"
                                      );
                                    lst_circleBlk.Items.Add(Environment.NewLine);
                                }
                                lst_circleBlk.Items.Add(
                                      "Instrument_" + obsOccurence + "      " + tagName + "                " + t1 + "     " + t2
                                       );
                            }
                            else
                            {
                                MessageBox.Show("One of the VdInsert of VdBlock should be comprised of VdLines");
                                return;
                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
        }
        private void btnIncre_Click(object sender, EventArgs e)
        {
            var dgr = dgvAllLayerInstruments.CurrentRow;

            if (dgr == null)
                MessageBox.Show("Please select a layer to interact");
            else
            {
                string newlyrIns = dgr.Cells[0].Value.ToString();
                if (!String.IsNullOrEmpty(newlyrIns))
                {
                    if (!lstInstrument.Items.Contains(newlyrIns))
                    {
                        lstInstrument.Items.Add(newlyrIns);
                        lstInstrument.Update();
                        var selectedLyr = lyrInstrument.Where(x => x.Name.Equals(newlyrIns)).ToList();
                        //Display(selectedLyr[0]);
                    }
                }
            }
        }

        private void btnDecre_Click(object sender, EventArgs e)
        {
            if (lstInstrument.Items.Count > 0)
            {
                var selectedLyr = lyrInstrument.Where(x => x.Name.Equals(lstInstrument.Items[0].ToString())).ToList();
                //Display(selectedLyr[0], true);
                lstInstrument.Items.RemoveAt(0);

            }
            lstInstrument.Refresh();
        }
        private static List<Point> lstPoint; // to remove same coincidence objects
        private void Display() //VectorDraw.Professional.vdPrimaries.vdLayer lyr, bool IsRemoved = false
        {
            lstPoint = new List<Point>();
            //*** We will be in a big polygonal Obstacle to build route. So, this big one should be neglected 


            dtInstrument = new DataTable();
            dtInstrument.Columns.Add("No");
            dtInstrument.Columns.Add("Instrument");
            dtInstrument.Columns.Add("Dimension");
            //As usual
            List<VectorDraw.Professional.vdFigures.vdCircle> LstObserverObjects;
            List<VectorDraw.Professional.vdFigures.vdPolyline> TargetObjects;
            List<VectorDraw.Professional.vdFigures.vdPolyline> ObstaclePolyObjects;
            //Only Line, Insert, Polyline include in Obstacle Layer     
            //List<VectorDraw.Professional.vdFigures.vdLine> ObstacleLineObjects;   
            //List<VectorDraw.Professional.vdFigures.vdInsert> ObstacleVdInsertObjects;

            //Test
            //List<VectorDraw.Professional.vdPrimaries.vdBlock> vdBlocks = new List<VectorDraw.Professional.vdPrimaries.vdBlock>();

            var intr = lyrInstrument;

            //Each selected List 
            foreach (var itm in lstInstrument.Items)
            {
                LstObserverObjects = new List<VectorDraw.Professional.vdFigures.vdCircle>();
                TargetObjects = new List<VectorDraw.Professional.vdFigures.vdPolyline>();
                ObstaclePolyObjects = new List<VectorDraw.Professional.vdFigures.vdPolyline>();


                //Removed from last version
                //ObstacleLineObjects = new List<VectorDraw.Professional.vdFigures.vdLine>();
                //ObstacleVdInsertObjects = new List<VectorDraw.Professional.vdFigures.vdInsert>();

                var currentInstument = lyrInstrument.Where(x => x.Name.Equals(itm.ToString())).ToList()[0];
                var allControl = vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.ArrayItems;
                //Each Control
                foreach (var v in allControl)
                {


                    //if (v is VectorDraw.Professional.vdPrimaries.vdBlock fr ) //
                    //{
                    //   
                    //}
                    if (v is VectorDraw.Professional.vdFigures.vdCircle cirCle)
                    {
                        if (cirCle.Layer.Name.ToLower().Trim() == itm.ToString().ToLower().Trim()) // Much Instrlyer
                        {
                            if (LstObserverObjects.Where(c => c.Center == cirCle.Center).ToList().Count == 0)
                            {
                                LstObserverObjects.Add(cirCle);
                            }
                        }
                    }
                    else if (v is VectorDraw.Professional.vdFigures.vdPolyline target)
                    {

                        if (target.Layer.Name.ToLower().Trim() == lyrDestination[0].Name.ToLower().ToString())  // Only One Destination lyr
                        {
                            TargetObjects.Add(target);
                        }
                        else if (target.Layer.Name.ToLower().ToString() == lyrObstacle[0].Name.ToLower().ToString())
                        {
                            ObstaclePolyObjects.Add(target);
                        }
                    }

                }

                //Star Collect btw points
                foreach (var lst in LstObserverObjects)
                {
                    try
                    {


                        SettingRoute(TargetObjects[0], lst, ObstaclePolyObjects);
                    }
                    catch (Exception ex)
                    {
                        var msg = "";
                        if (ex.StackTrace.Contains("bound"))
                        {
                            msg = "***Probably Destination polyline should be a closed polygon like obstacle polygons.";
                        }
                        MessageBox.Show(ex.Message + Environment.NewLine + msg);
                        return;
                    }

                }

            }

        }
        //Colect Only inside Points  //List<VectorDraw.Professional.vdFigures.vdLine> ObsLine, List<VectorDraw.Professional.vdFigures.vdInsert> ObsInsert
        private void SettingRoute(VectorDraw.Professional.vdFigures.vdPolyline TPoint, VectorDraw.Professional.vdFigures.vdCircle IPoint, List<VectorDraw.Professional.vdFigures.vdPolyline> ObsPoly)
        {

            List<VectorDraw.Professional.vdFigures.vdPolyline> ObsPolyIn = new List<VectorDraw.Professional.vdFigures.vdPolyline>();
            foreach (var op in ObsPoly)
            {

                //if ( (op.VertexList.GetBox().MidPoint.x - (op.VertexList.GetBox().Width/2)) > TPoint.VertexList.GetBox().MidPoint.x && (op.VertexList.GetBox().MidPoint.x + (op.VertexList.GetBox().Width / 2)) < IPoint.Center.x  && ((op.VertexList.GetBox().MidPoint.y - (op.VertexList.GetBox().Height/2)) > TPoint.VertexList.GetBox().MidPoint.y )&& (op.VertexList.GetBox().MidPoint.y + (op.VertexList.GetBox().Height / 2)) < IPoint.Center.y)
                // {
                ObsPolyIn.Add(op);
                //}
            }
            if (ObsPolyIn.Count > 0)
                StartRoute(ObsPolyIn, TPoint, IPoint);
        }

        private bool IsOver2SideHeight(VectorDraw.Geometry.Box ObsCenter, VectorDraw.Geometry.Box TargetCenter, VectorDraw.Professional.vdFigures.vdCircle ObserverCenter, out double length, bool OnlyCheck = true)
        {
            double val = 0.0;
            //Changed to Target's height Even inside range
            if ((ObsCenter.MidPoint.y - (ObsCenter.Height / 2)) <= TargetCenter.MidPoint.y && (ObsCenter.MidPoint.y + (ObsCenter.Height / 2)) >= TargetCenter.MidPoint.y && ObserverCenter.Center.x > ObsCenter.MidPoint.x)
            {
                //Total would be 3 path 
                // Get vector distance between points 
                //eg. To get magnitude value points  (x1,y1), (x2,y2)
                //    |(x1-x2)| + |(y1-y2)|  
                //   val        +       val

                if (!OnlyCheck)
                {
                    DrawRoute(ObserverCenter.Center.x, ObserverCenter.Center.y, ObserverCenter.Center.x, (ObserverCenter.Center.y + (ObsCenter.Height)));
                    val += Math.Abs(ObserverCenter.Center.y - (ObserverCenter.Center.y + ObsCenter.Height));
                    DrawRoute(ObserverCenter.Center.x, (ObserverCenter.Center.y + ObsCenter.Height), TargetCenter.MidPoint.x, (ObserverCenter.Center.y + ObsCenter.Height));
                    val += Math.Abs(ObserverCenter.Center.x - TargetCenter.MidPoint.x);
                    DrawRoute(TargetCenter.MidPoint.x, (ObserverCenter.Center.y + ObsCenter.Height), TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
                    val += Math.Abs((ObserverCenter.Center.y + ObsCenter.Height) - TargetCenter.MidPoint.y);
                }
                length = val;
                return true;
            }
            length = val;
            return false;
        }

        private bool IsOver2SideWidth(VectorDraw.Geometry.Box ObsCenter, VectorDraw.Geometry.Box TargetCenter, VectorDraw.Professional.vdFigures.vdCircle ObserverCenter, out double length, bool OnlyCheck = true)
        {
            double val = 0.0;
            //Changed to Target's width Even inside range
            if ((ObsCenter.MidPoint.x - (ObsCenter.Width / 2)) <= TargetCenter.MidPoint.x && (ObsCenter.MidPoint.x + (ObsCenter.Width / 2)) >= TargetCenter.MidPoint.x && ObserverCenter.Center.y > ObsCenter.MidPoint.y)
            {
                //Total would be 3 path 
                // Get vector distance between points 
                //eg. To get magnitude value points  (x1,y1), (x2,y2)
                //    |(x1-x2)| + |(y1-y2)|  
                //   val        +       val
                if (!OnlyCheck)
                {
                    DrawRoute(ObserverCenter.Center.x, ObserverCenter.Center.y, (ObserverCenter.Center.x + ObsCenter.Width), ObserverCenter.Center.y);
                    val += Math.Abs(ObserverCenter.Center.x - (ObserverCenter.Center.x + ObsCenter.Width));
                    DrawRoute((ObserverCenter.Center.x + ObsCenter.Width), ObserverCenter.Center.y, (ObserverCenter.Center.x + ObsCenter.Width), TargetCenter.MidPoint.y);
                    val += Math.Abs(ObserverCenter.Center.y - TargetCenter.MidPoint.y);
                    DrawRoute((ObserverCenter.Center.x + ObsCenter.Width), TargetCenter.MidPoint.y, TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
                    val += Math.Abs((ObserverCenter.Center.x + ObsCenter.Width) - TargetCenter.MidPoint.x);
                }
                length = val;
                return true;
            }
            length = val;
            return false;
        }
        private int counter = 0;
        public static DataTable dtInstrument;
        private static DataTable dtSchedule;
        private static DataTable dtSegmentation;

        private static string DocPath = "";
        private void StartRoute(List<VectorDraw.Professional.vdFigures.vdPolyline> lstObstacles, VectorDraw.Professional.vdFigures.vdPolyline Tar, VectorDraw.Professional.vdFigures.vdCircle Observer)
        {

            // int i = counter;

            List<VectorDraw.Professional.vdFigures.vdCircle> LstObserver = new List<VectorDraw.Professional.vdFigures.vdCircle>();
            LstObserver.Add(Observer);
            foreach (var ObserverCenter in LstObserver)
            {
                counter++;
                double PathLength = 0.0;
                bool IsDrawnPath = false;
                int PathFlg = 0;  //    1 =>IsOver2SideHeight :  2 =>IsOver2SideWidth : 3=>GoHorizontal : 4=> GoVertical

                var TargetCenter = Tar.VertexList.GetBox();
                foreach (var lstObstacle in lstObstacles)
                {
                    //Start
                    //Get Center of Destination Target and Obstacle.

                    var ObsCenter = lstObstacle.VertexList.GetBox();

                    //Avoid height Obstacle
                    // Check Obstacle height is inside area of Instrument height
                    if ((ObsCenter.MidPoint.y - (ObsCenter.Height / 2)) <= ObserverCenter.Center.y && (ObsCenter.MidPoint.y + (ObsCenter.Height / 2)) >= ObserverCenter.Center.y)
                    {
                        if (!IsOver2SideHeight(ObsCenter, TargetCenter, ObserverCenter, out double length, false))// Draw if 3 paths, if not draw 2 paths.
                        {
                            PathLength = GoHorizontal(TargetCenter, ObserverCenter);//draw 2 paths. 
                        }
                        else
                            PathLength = length;
                        IsDrawnPath = true;
                    }

                }

                //Avoid Widht Obstacle
                // Check Obstacle width is inside area of Instrument width

                //else  
                if (!IsDrawnPath)
                    foreach (var lstObstacle in lstObstacles)
                    {
                        //Start
                        //Get Center of Destination Target and Obstacle.

                        var ObsCenter = lstObstacle.VertexList.GetBox();
                        //else
                        if ((ObsCenter.MidPoint.x - (ObsCenter.Width / 2)) <= ObserverCenter.Center.x && (ObsCenter.MidPoint.x + (ObsCenter.Width / 2)) >= ObserverCenter.Center.x)
                        {
                            if (!IsOver2SideWidth(ObsCenter, TargetCenter, ObserverCenter, out double length, false))// Draw if 3 paths, if not draw 2 paths.
                                PathLength = GoVertical(TargetCenter, ObserverCenter);//draw 2 paths.

                            else
                                PathLength = length;
                            IsDrawnPath = true;
                        }
                    }
                if (!IsDrawnPath)
                    foreach (var lstObstacle in lstObstacles)
                    {
                        //Start
                        //Get Center of Destination Target and Obstacle.

                        var ObsCenter = lstObstacle.VertexList.GetBox();
                        // It is okay to go vertical first or horizontal first. 
                        //But, Make Shortern Priority  
                        //eg if vertical path is shorter => Vertical path > horizontal
                        if ((TargetCenter.MidPoint.y - ObserverCenter.Center.y) < (TargetCenter.MidPoint.x - ObserverCenter.Center.x))
                        {
                            //In second route, Check and avoid not to pass through to an obstacle . 
                            //if exist, change route
                            if ((ObsCenter.MidPoint.y - (ObsCenter.Height / 2)) <= TargetCenter.MidPoint.y && (ObsCenter.MidPoint.y + (ObsCenter.Height / 2)) >= TargetCenter.MidPoint.y)
                                PathLength = GoVertical(TargetCenter, ObserverCenter);
                            else
                                PathLength = GoHorizontal(TargetCenter, ObserverCenter);
                        }
                        else if ((TargetCenter.MidPoint.y - ObserverCenter.Center.y) == (TargetCenter.MidPoint.x - ObserverCenter.Center.x))
                        {
                            if ((ObsCenter.MidPoint.y - (ObsCenter.Height / 2)) <= TargetCenter.MidPoint.y && (ObsCenter.MidPoint.y + (ObsCenter.Height / 2)) >= TargetCenter.MidPoint.y)
                                PathLength = GoHorizontal(TargetCenter, ObserverCenter);
                            else
                                PathLength = GoVertical(TargetCenter, ObserverCenter);
                        }
                        else
                        {
                            //In second route, Check and avoid not to pass through to an obstacle . 
                            //if exist, change route
                            if ((ObsCenter.MidPoint.x - (ObsCenter.Width / 2)) <= TargetCenter.MidPoint.x && (ObsCenter.MidPoint.x + (ObsCenter.Width / 2)) >= TargetCenter.MidPoint.x)
                                PathLength = GoHorizontal(TargetCenter, ObserverCenter);
                            else
                                PathLength = GoVertical(TargetCenter, ObserverCenter);

                        }
                    };

                //Set instrument label and put at dtInstrument table
                TextInsert(ObserverCenter.Center.x, ObserverCenter.Center.y, "Instrument " + counter.ToString());
                dtInstrument.Rows.Add(new object[] {
                counter , "Instrument" + counter.ToString(),  Convert.ToInt32(PathLength).ToString()
                });
            }
        }
        private void lstInstrument_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void TextInsert(double x, double y, string txt)
        {
            VectorDraw.Professional.vdFigures.vdText text = new VectorDraw.Professional.vdFigures.vdText();
            text.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            text.setDocumentDefaults();
            text.InsertionPoint = new VectorDraw.Geometry.gPoint(x - 400, y - 400);
            text.TextString = txt;
            text.Height = 450.0;
            text.PenColor = new vdColor(Color.Red);
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(text);
            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
        }
        public static bool contains(Vertexes _pts, gPoint pt)
        {
            bool isIn = false;

            int NumberOfPoints = _pts.Count;
            if (true)
            {
                int i, j = 0;
                for (i = 0, j = NumberOfPoints - 1; i < NumberOfPoints; j = i++)
                {
                    if (
                    (
                    ((_pts[i].y <= pt.y) && (pt.y <= _pts[j].y)) || ((_pts[j].y <= pt.y) && (pt.y <= _pts[i].y))
                    ) &&
                    (pt.x <= (_pts[j].x - _pts[i].x) * (pt.y - _pts[i].y) / (_pts[j].y - _pts[i].y) + _pts[i].x)
                    )
                    {
                        isIn = !isIn;
                        if (isIn)
                            return isIn;

                    }
                }
            }
            return isIn;
        }

        public void GetEntities()
        {
            try
            {
                obstacles = new List<vdPolyline>();
                instruments = new List<Instrument>();
                instrument = new List<vdCircle>();
                foreach (vdFigure f in vdFramedControl1.BaseControl.ActiveDocument.Model.Entities)
                {

                    if (f.Layer.Name == lstInstrument.Items[0].ToString())
                    //  if ( lstInstrument.Items.Contains(f.Layer.Name))
                    {
                        if ((f is vdCircle))
                        {
                            vdCircle circle = (vdCircle)f;
                            this.instrument.Add(circle);

                            Instrument instrument = new Instrument(circle);
                            this.instruments.Add(instrument);
                        }
                    }
                    else if (f.Layer.Name == cboObs.SelectedItem.ToString())
                    {
                        if ((f is vdPolyline))
                        {
                            vdPolyline poly = (vdPolyline)f;
                            this.obstacles.Add(poly);
                        }
                    }
                    else if (f.Layer.Name == "Boundary")
                    {
                        if ((f is vdPolyline))
                        {
                            vdPolyline poly = (vdPolyline)f;
                            this.boundary = poly;
                            vdCurves c = poly.getOffsetCurve(1000); ;
                            this.offsetBoundary = new vdPolyline(c.Document, c[0].GetGripPoints());
                        }
                    }
                    else if (f.Layer.Name == cboDesti.SelectedItem.ToString())
                    {
                        if (f is vdPolyline)
                        {
                            vdPolyline poly = (vdPolyline)f;
                            this.destination = new Destination(poly);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "***Possibly it should choose correct destination, instrument or obstacles");
            }
        }
        public void makeGrid(bool IsAlternative=false)
        {
            try
            {
                if (offsetBoundary == null)
                {
                    MessageBox.Show("Cant Detect boundary or no exist that kind of boundary layer." + Environment.NewLine + "Please make sure boundary layer exist.");
                    return;
                }
                Box box = this.offsetBoundary.BoundingBox;

                sx = box.Left;
                sy = box.Bottom;
                //make gap
                int wGap = (1000 * 2) * int.Parse(txtHTolerance.Text);
                int hGap = (1000 * 2) * int.Parse(txtHTolerance.Text);
                int wc = (int)box.Width / (wGap) + 1;
                int hc = (int)box.Height / (hGap) + 1;
                int pCount = wc * hc;
                //Make AdjencyMatrix Setting by Setting Max Value
                double[,] adjMatrix = new double[pCount, pCount];
                for (int i = 0; i < pCount; i++)
                {
                    for (int j = 0; j < pCount; j++)
                    {
                        adjMatrix[i, j] = double.MaxValue;
                    }
                }
                for (int i = 0; i < pCount - 1; i++)
                {
                    if ((i / wc) == ((i + 1) / wc))
                    {
                        adjMatrix[i, i + 1] = wGap;
                        adjMatrix[i + 1, i] = wGap;
                    }
                    if (i + wc < pCount)
                    {
                        adjMatrix[i, i + wc] = hGap;
                        adjMatrix[i + wc, i] = hGap;
                    }
                }

                for (int i = 0; i < hc; i++)
                {
                    for (int j = 0; j < wc; j++)
                    {
                        int index = i * wc + j; // left to right vertically
                        gPoint p = new gPoint(sx + j * wGap, sy + i * hGap);
                        gridPoints.Add(p);
                        bool isIn = contains(this.boundary.VertexList, p);
                        bool isIn2 = false;
                        foreach (vdPolyline obstacle in this.obstacles)
                        {
                            isIn2 = contains(obstacle.VertexList, p);
                            if (isIn2)
                                break;
                        }
                        //if somethings existed in restricted location, make that index as disturbant point
                        if (!isIn || isIn2)
                        {
                            if ((index - wc) > 0)
                            {
                                adjMatrix[index - wc, index] = double.MaxValue;
                                adjMatrix[index, index - wc] = double.MaxValue;
                            }
                            if ((index - 1) > 0 && ((index - 1) / wc == index / wc))
                            {
                                adjMatrix[index - 1, index] = double.MaxValue;
                                adjMatrix[index, index - 1] = double.MaxValue;
                            }
                            if ((index + wc) < pCount)
                            {
                                adjMatrix[index + wc, index] = double.MaxValue;
                                adjMatrix[index, index + wc] = double.MaxValue;
                            }
                            if ((index + 1) < pCount && ((index + 1) / wc == index / wc))
                            {
                                adjMatrix[index + 1, index] = double.MaxValue;
                                adjMatrix[index, index + 1] = double.MaxValue;
                            }
                        }


                    }
                }
                double minDistance = double.MaxValue;
                int count = 0;
                //Get Nearest Location of Destination
                foreach (gPoint gridPoint in this.gridPoints)
                {
                    double dis = this.destination.center.Distance2D(gridPoint);
                    if (minDistance > dis)
                    {
                        minDistance = dis;
                        destination.gridPoint = gridPoint;
                        destination.gridIndex = count;
                    }
                    count++;
                }

                foreach (Instrument ins in this.instruments)
                {
                    minDistance = double.MaxValue;
                    count = 0;
                    foreach (gPoint gridPoint in this.gridPoints)
                    {
                        double dis = ins.centerPoint.Distance2D(gridPoint);
                        if (minDistance > dis)
                        {
                            minDistance = dis;
                            ins.distance = dis;
                            ins.gridPoint = gridPoint;
                            ins.gridIndex = count;
                        }
                        count++;
                    }
                }

                double[] result;
                foreach (Instrument ins in this.instruments)
                {
                    //   ins.distanceFromDestination = this.destination.center.Distance2D(ins.centerPoint);
                    //   Console.WriteLine("gridIndex   "+ins.gridIndex);
                    result = Dijkstra.analysis(ins.gridIndex, destination.gridIndex, adjMatrix);
                    if (result != null) // it might seen an error  // ***PTK 
                        ins.distanceFromDestination = result[result.Length - 1];

                }

                this.instruments = this.instruments.OrderBy(x => x.distanceFromDestination).ToList();
                txtMaxLength.Text = (Convert.ToInt32(this.instruments.Max(x => x.distanceFromDestination) / 1000)).ToString();
                // this.instruments.Reverse();
                dtInstrument = new DataTable();
                dtInstrument.Columns.Add("Id");
                dtInstrument.Columns.Add("No");
                dtInstrument.Columns.Add("Instrument");
                dtInstrument.Columns.Add("Dimension");
                //dtInstrument.Columns.Add("SquareArea");//
                dtInstrument.Columns.Add("InstrumentType");//InstrumentType
                int counterText = 0;
                foreach (Instrument ins in this.instruments)
                {

                    counterText++;
                    result = Dijkstra.analysis(ins.gridIndex, destination.gridIndex, adjMatrix);
                    gPoints ps = new gPoints();
                    try
                    {
                        for (int i = 0; i < result.Length - 1; i++)
                        {
                            gPoint gp = gridPoints[(int)result[i]];
                            ps.Add(gp);
                        }

                    }
                    catch
                    {

                    }
                    try
                    {
                        for (int i = 0; i < result.Length - 2; i++)
                        {
                            adjMatrix[(int)result[i], (int)result[i + 1]] = adjMatrix[(int)result[i], (int)result[i + 1]] * 0.1;
                            adjMatrix[(int)result[i + 1], (int)result[i]] = adjMatrix[(int)result[i + 1], (int)result[i]] * 0.1;
                        }
                    }
                    catch
                    {

                    }

                    vdPolyline line = new vdPolyline(this.vdFramedControl1.BaseControl.ActiveDocument, ps);
                    line.PenColor = new vdColor(Color.Yellow);
                    vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(line);
                    line.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
                    line.setDocumentDefaults();
                    //var ids = line.Id;

                    //Texting / Dimensioning
                    var ctxt = counterText.ToString();
                    TextInsert(ins.centerPoint.x, ins.centerPoint.y, ("Instrument_" + counterText.ToString() + ((Convert.ToInt32(ins.circle.Radius) == 1500) ? "  Analyzer" : "")));
                    if (Convert.ToInt32(ins.distanceFromDestination) == Convert.ToInt32(this.instruments.Max(x => x.distanceFromDestination).ToString()))
                        lblMaxLengthInstrument.Text = "Instrument_" + counterText.ToString();
                    dtInstrument.Rows.Add(new object[] {
                 line.Id , counterText , ("Instrument_" + counterText.ToString() ),  Convert.ToInt32(ins.distanceFromDestination/1000).ToString(), "0_121"
                });
                    //Classify instrument type as Analyzer   // Assume All Instruments with radius 1500(1.5m) is attached with Analyzer that required separated powering Line. 
                //    if (Convert.ToInt32(ins.circle.Radius) == 1500)
                //    {
                //        counterText++;
                //        // Draw new Power tray duct
                //        var oat = line.getOffsetCurve((Convert.ToInt32(txtHTolerance.Text) * 100)).Items;
                //        ps = new gPoints();
                //        int j = 0;
                //        foreach (vdFigure lne in (oat[0] as vdPolyline).Explode().ArrayItems)
                //        {
                //            if (j == 0)
                //            {
                //                ps.Add((lne as vdLine).getStartPoint());
                //            }
                //            ps.Add((lne as vdLine).getEndPoint());
                //            j++;
                //        }
                //        line = new vdPolyline(this.vdFramedControl1.BaseControl.ActiveDocument, ps);

                //        vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(line);
                //        line.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
                //        line.PenColor.ByLayer = false;
                //        line.PenColor.ByBlock = false;
                //        line.PenColor.ByColorIndex = true;
                //        line.PenColor = new vdColor(Color.Yellow);
                //        dtInstrument.Rows.Add(new object[] {
                // line.Id , counterText ,  ("Instrument_" + ctxt + "  Analyzer"),  Convert.ToInt32(ins.distanceFromDestination/1000).ToString(),""
                //});
                //    }

                }
                this.vdFramedControl1.BaseControl.Redraw();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "Error at Grid Algorithm");
            }
        }
        private void btnRouting_Click(object sender, EventArgs e)
        {
            Reset();
            Import(true);
            Display();
        }
        private void Reset()
        {
            counter = 0;
        }
        /// <summary>
        /// Draw for verticle line on XY axis. 
        /// </summary>
        private double GoVertical(VectorDraw.Geometry.Box TargetCenter, VectorDraw.Professional.vdFigures.vdCircle ObserverCenter)
        {
            // Get vector distance between points 
            //eg. To get magnitude value points  (x1,y1), (x2,y2)
            //    |(x1-x2)| + |(y1-y2)|
            //   val        +       val
            double val = 0.0;
            DrawRoute(ObserverCenter.Center.x, ObserverCenter.Center.y, TargetCenter.MidPoint.x, ObserverCenter.Center.y);
            val += Math.Abs(ObserverCenter.Center.x - TargetCenter.MidPoint.x);
            DrawRoute(TargetCenter.MidPoint.x, ObserverCenter.Center.y, TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
            val += Math.Abs(ObserverCenter.Center.y - TargetCenter.MidPoint.y);
            return val;
        }
        /// <summary>
        /// Draw for horizontal line on XY axis. 
        /// </summary>
        private double GoHorizontal(VectorDraw.Geometry.Box TargetCenter, VectorDraw.Professional.vdFigures.vdCircle ObserverCenter)
        {
            // Get vector distance between points 
            //eg. To get magnitude value points  (x1,y1), (x2,y2)
            //    |(x1-x2)| + |(y1-y2)|
            //   val        +       val
            double val = 0.0;
            DrawRoute(ObserverCenter.Center.x, ObserverCenter.Center.y, ObserverCenter.Center.x, TargetCenter.MidPoint.y);
            val += Math.Abs(ObserverCenter.Center.y - TargetCenter.MidPoint.y);
            DrawRoute(ObserverCenter.Center.x, TargetCenter.MidPoint.y, TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
            val += Math.Abs(ObserverCenter.Center.x - TargetCenter.MidPoint.x);
            return val;
        }
        private void DrawRoute(double x1, double y1, double x2, double y2)
        {
            VectorDraw.Professional.vdFigures.vdLine oneline = new VectorDraw.Professional.vdFigures.vdLine();
            oneline.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            oneline.setDocumentDefaults();
            oneline.StartPoint = new VectorDraw.Geometry.gPoint(x1, y1);
            oneline.EndPoint = new VectorDraw.Geometry.gPoint(x2, y2);
            oneline.PenColor.ColorIndex = 3;
            oneline.PenWidth = 0.4;
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(oneline);
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.ZoomWindow(new VectorDraw.Geometry.gPoint(-100.0, -40.0), new VectorDraw.Geometry.gPoint(140.0, 90.0));

            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
        }

        private void btnDimension_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            Form2 frm = new Form2();
            frm.WindowState = FormWindowState.Normal;
            frm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            Project.ProjectInfo frm = new Project.ProjectInfo();
            frm.WindowState = FormWindowState.Normal;
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //GetEntities();
            //makeGrid();
            // makeRelocation();
            //Explode();
            // AddBlockItems();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            TryDetect();
        }
        private void Explode()
        {

            //BlockTypeConverter d = new BlockTypeConverter();
            ////var fs = d.ConvertTo(typeof(vdPolyline));
            //var pri = vdFramedControl1.BaseControl.ActiveDocument.Blocks.GetNotDeletedItems();
            //foreach (var blk in pri)
            //{
            //    var bk = (blk as vdBlock);
            //    if (bk.Name == blockVsInsert)
            //    {

            //        //   bk.Entities.RemoveAll();
            //        var arrblk = bk.Entities;//.ArrayItems.ArrayItems;
            //        int i = 0;
            //        foreach (var vfg in arrblk.ArrayItems.ArrayItems)
            //        {
            //            if (vfg is vdLine vi)
            //            {
            //                // arrblk.RemoveAt(i);  
            //                i++;
            //            }
            //        }
            //    }
            //}
            //vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
            //SettingSource();
            //RefreshUpdate();
            //DrawPolyline();
        }
        private DataTable SettingTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("BK");
            dt.Columns.Add("FG");
            dt.Columns.Add("SP", typeof(gPoint));
            dt.Columns.Add("EP", typeof(gPoint));
            dt.Columns.Add("GEO");
            return dt;
        }
        DataTable dtEntities;
        private void RefreshUpdate()
        {
            foreach (vdFigure f in vdFramedControl1.BaseControl.ActiveDocument.Model.Entities)
            {
                if (f != null && f.Layer.Name.ToLower().ToString().Contains("destination") && f is vdPolyline vi)
                {
                    //if (vi.Flag == VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE)
                    //{
                    vi.Explode().RemoveAll();
                    vi.Explode().EraseAll();
                    vi.Deleted = true;
                    vi.Explode().RemoveAll();
                    //}
                    //break;
                }
            }
            vdFramedControl1.BaseControl.Redraw();
            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
            vdFramedControl1.BaseControl.ReFresh();
        }
        private void SettingSource()
        {
            dtEntities = SettingTable();
            int obsOccurence = 0;
            foreach (vdFigure f in vdFramedControl1.BaseControl.ActiveDocument.Model.Entities)
            {
                try
                {

                    if (f != null && f.Layer.Name.ToLower().ToString().Contains("destination") && f is vdPolyline vi)
                    {

                        var ent = vi.Explode();
                        if (ent != null && CheckAllOnlyLines(ent) && vi.Flag == VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE)
                        {
                            OCentriod = vi.BoundingBox.MidPoint;
                            obsOccurence++;
                            int entityOccurence = 0;

                            foreach (var e in ent)
                            {
                                entityOccurence++;
                                if (e is vdLine vl)
                                {
                                    var s = vl.Layer.PenColor.ColorIndex;
                                    dtEntities.Rows.Add(new object[] { obsOccurence, entityOccurence, vl.StartPoint, vl.EndPoint, vi.BoundingBox.MidPoint });
                                    vi.Invalidate();
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("One of the VdInsert of VdBlock should be comprised of VdLines");
                    return;
                }

            }
        }
        private void DrawPolyline()
        {
            var dest = vdFramedControl1.BaseControl.ActiveDocument.Layers.FindName("Destination");
            vdFramedControl1.BaseControl.ActiveDocument.SetActiveLayer(dest);
            int completeOnepolygon = 0;
            VectorDraw.Professional.vdFigures.vdPolyline onepoly = new VectorDraw.Professional.vdFigures.vdPolyline();
            //Desti
            //butCircle_Click( new  gPoint( Centroids[0].X, Centroids[0].Y));
            gPoint NewCentroid = new gPoint(OCentriod.x - Centroids[0].X, OCentriod.y - Centroids[0].Y);
            foreach (DataRow dr in dtEntities.Rows)
            {
                completeOnepolygon++;
                if (completeOnepolygon <= 4)
                {
                    var pt = new gPoint(((dr["SP"] as gPoint).x - NewCentroid.x), (dr["SP"] as gPoint).y - NewCentroid.y);
                    onepoly.VertexList.Add(pt);

                    onepoly.PenColor.ColorIndex = 7;
                }
                if (completeOnepolygon == 4)
                {
                    vdFramedControl1.BaseControl.ActiveDocument.SetActiveLayer(dest);
                    onepoly.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
                    onepoly.setDocumentDefaults();
                    onepoly.Flag = VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE;
                    vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(onepoly);
                    vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
                    onepoly = new VectorDraw.Professional.vdFigures.vdPolyline();

                    if (completeOnepolygon == 4)
                        completeOnepolygon = 0;

                }

            }
            // RefreshUpdate();
        }
        private void AddBlockItems()
        {
            //We create a block object and initialize it's default properties.
            VectorDraw.Professional.vdPrimaries.vdBlock blk = new
           VectorDraw.Professional.vdPrimaries.vdBlock();
            blk.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            blk.setDocumentDefaults();
            //We add some entities to the block.
            VectorDraw.Professional.vdFigures.vdPolyline poly = new
           VectorDraw.Professional.vdFigures.vdPolyline();
            poly.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            poly.setDocumentDefaults();
            poly.VertexList.Add(new VectorDraw.Geometry.gPoint());
            poly.VertexList.Add(new VectorDraw.Geometry.gPoint(1.0, 0.0));
            poly.VertexList.Add(new VectorDraw.Geometry.gPoint(2.0, 1.0));
            poly.VertexList.Add(new VectorDraw.Geometry.gPoint(4.0, -1.0));
            poly.VertexList.Add(new VectorDraw.Geometry.gPoint(6.0, 1.0));
            poly.VertexList.Add(new VectorDraw.Geometry.gPoint(8.0, -1.0));
            poly.VertexList.Add(new VectorDraw.Geometry.gPoint(10.0, 1.0));
            poly.VertexList.Add(new VectorDraw.Geometry.gPoint(11.0, 0.0));
            poly.VertexList.Add(new VectorDraw.Geometry.gPoint(12.0, 0.0));
            blk.Entities.AddItem(poly);
            blk.Origin = new VectorDraw.Geometry.gPoint(6.0, 0.0);
            blk.Name = "CustomBlock123456";
            VectorDraw.Professional.vdFigures.vdAttribDef attribdef = new
           VectorDraw.Professional.vdFigures.vdAttribDef();
            attribdef.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            attribdef.setDocumentDefaults();
            attribdef.InsertionPoint = new VectorDraw.Geometry.gPoint(5.0, 1.2);
            //Name of the attribute used to be found when using the block.
            attribdef.TagString = "resistance1234567";
            //Default value used when inserted the block from the block's dialog.
            attribdef.ValueString = "1W";
            blk.Entities.AddItem(attribdef);
            //And then we add this block to the document's blocks collection
            vdFramedControl1.BaseControl.ActiveDocument.Blocks.AddItem(blk);
            //We will also add a block from a precreated file.
            string path = Application.ExecutablePath.Substring(0,
           Application.ExecutablePath.LastIndexOf('\\')) + "\\..\\..\\vdblk.vdml";
            string path1 = "";
            VectorDraw.Professional.vdPrimaries.vdBlock blk2;
            if (vdFramedControl1.BaseControl.ActiveDocument.FindFile(path, out path1))
            {
                blk2 = vdFramedControl1.BaseControl.ActiveDocument.Blocks.AddFromFile(path,
               false);
                //We check if a block with name CustomBlock2 already exists and if not
                //  we change the name of the block to CustomBlock2.
                if (vdFramedControl1.BaseControl.ActiveDocument.Blocks.FindName("CustomBlock2") == null) blk2.Name = "CustomBlock2";
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            vdFramedControl1.SetLayoutStyle(vdControls.vdFramedControl.LayoutStyle.CommandLine, true);
            vdFramedControl1.SetLayoutStyle(vdControls.vdFramedControl.LayoutStyle.LayoutTab, false);
            vdFramedControl1.UnLoadMenu();
            //var f = vdFramedControl1.BaseControl.ActiveDocument.GetUsedBlocks();
            //vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.lunits.UType = VectorDraw.Geometry.LUnits.LUnitType.lu_Architectural;
            //vdFramedControl1.BaseControl.ActiveDocument.OnActionDraw += ActiveDocument_OnActionDraw;
            //vdFramedControl1.BaseControl.ActiveDocument.OnAfterAddItem += ActiveDocument_OnAfterAddItem;
        }

        private void ActiveDocument_OnAfterAddItem(object obj)
        {

            vdBlock line = obj as vdBlock;
            if (line == null)
            {
                return;
            }
            else
            { //Draw a rectangle around the vdLine that was previously added
                VectorDraw.Professional.vdFigures.vdRect onerect = new
               VectorDraw.Professional.vdFigures.vdRect();
                //We set the document where the rect is going to be added.This is important for the
                // vdRect in order to obtain initial properties with setDocumentDefaults.
                onerect.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
                onerect.setDocumentDefaults();
                // on
                //The two previus steps are important if a vdFigure object is going to be added to a
                // document.
                //Now we will change some properties of the rect.
                onerect.InsertionPoint = line.BoundingBox().MidPoint;
                onerect.Width = 200;
                onerect.Height = 200;
                onerect.PenColor.ColorIndex = 34;
                ////Now we will add this object to the Entities collection of the Model
                //Layout(ActiveLayout).
                vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(onerect);
            }
        }

        private void ActiveDocument_OnActionDraw(object sender, object action, bool isHideMode, ref bool cancel)
        {
            if (!(action is VectorDraw.Actions.ActionGetRefPoint)) return; //if the action is not an
                                                                           // "input point" then exit
            VectorDraw.Actions.BaseAction act = action as VectorDraw.Actions.BaseAction;
            VectorDraw.Geometry.gPoint refpoint = act.ReferencePoint; //This is the base point
            VectorDraw.Geometry.gPoint currentpoint = act.OrthoPoint; //This is the cursor position
            VectorDraw.Professional.vdFigures.vdCircle circle = new
           VectorDraw.Professional.vdFigures.vdCircle();
            circle.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            circle.setDocumentDefaults();
            circle.Center = VectorDraw.Geometry.gPoint.MidPoint(refpoint, currentpoint);
            circle.Radius = circle.Center.Distance3D(refpoint);
            circle.Draw(act.Render);

        }

        private List<PointF> Seeds = new List<PointF>();
        private List<PointF> Centroids = new List<PointF>();
        private List<PointData> Points = new List<PointData>();
        private gPoint OCentriod;
        private void Preparepoint()
        {
            Points.Clear();
            foreach (var lst in instruments)
                Points.Add(new PointData(new System.Drawing.Point(Convert.ToInt32(lst.centerPoint.x), Convert.ToInt32(lst.centerPoint.y)), 0));
            Centroids.Clear();

            //Get Cluster
            int num_clusters = 1;
            if (Points.Count < num_clusters) return;

            // Reset the data.
            // Pick random centroids.
            Centroids = new List<PointF>();
            Points.Randomize();
            for (int i = 0; i < num_clusters; i++)
                Centroids.Add(Points[i].Location);
            foreach (PointData point_data in Points)
                point_data.ClusterNum = 0;
            RefreshDoc();
        }
        private void RefreshDoc()
        {
            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
        }
        private bool CheckAllOnlyLines(vdEntities ve)
        {
            foreach (var lines in ve)
            {
                if (!(lines is vdLine))
                    return false;
            }
            return true;
        }
        private void UpdateSolution()
        {
            // Find new centroids.
            int num_clusters = Centroids.Count;
            PointF[] new_centers = new PointF[num_clusters];
            int[] num_points = new int[num_clusters];
            foreach (PointData point in Points)
            {
                double best_dist =
                    Distance(point.Location, Centroids[0]);
                int best_cluster = 0;
                for (int i = 1; i < num_clusters; i++)
                {
                    double test_dist =
                        Distance(point.Location, Centroids[i]);
                    if (test_dist < best_dist)
                    {
                        best_dist = test_dist;
                        best_cluster = i;
                    }
                }
                point.ClusterNum = best_cluster;
                new_centers[best_cluster].X += point.Location.X;
                new_centers[best_cluster].Y += point.Location.Y;
                num_points[best_cluster]++;
            }

            // Calculate the new centroids.
            List<PointF> new_centroids = new List<PointF>();
            for (int i = 0; i < num_clusters; i++)
            {
                new_centroids.Add(new PointF(
                    new_centers[i].X / num_points[i],
                    new_centers[i].Y / num_points[i]));
            }

            Centroids = new_centroids;
        }
        private double Distance(PointF point1, PointF point2)
        {
            float dx = point1.X - point2.X;
            float dy = point1.Y - point2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private void button3_Click(object sender, EventArgs e)
        {


            GetEntities();
            //makeGrid();
            //Kmean
            Preparepoint();
            RefreshDoc();
            UpdateSolution();
            SettingSource();
            RefreshUpdate();
            DrawPolyline();
            RefreshDoc();
            //Dijkstra
            GetEntities();
            makeGrid(true);

        }

        private void butCircle_Click(gPoint pt)
        {
            VectorDraw.Professional.vdFigures.vdCircle onecircle = new VectorDraw.Professional.vdFigures.vdCircle();
            onecircle.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            onecircle.setDocumentDefaults();
            onecircle.Center = pt;
            onecircle.Radius = 5;
            onecircle.PenColor.ColorIndex = 50;
            onecircle.LineType = vdFramedControl1.BaseControl.ActiveDocument.LineTypes.DPIDash;

            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(onecircle);

            // vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.ZoomWindow(new VectorDraw.Geometry.gPoint(-100.0, -40.0), new VectorDraw.Geometry.gPoint(140.0, 90.0));

            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
        }
        void vdSource_vdMouseDown(MouseEventArgs e, ref bool cancel)
        { //begin a drag drop to the Source BaseCotnrol

        }

        private void vdFramedControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            { 
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            dtSchedule = new DataTable();
            dtSchedule.Columns.Add("TagNo");
            dtSchedule.Columns.Add("System");
            dtSchedule.Columns.Add("CableSpecification");
            dtSchedule.Columns.Add("From");
            dtSchedule.Columns.Add("To");
            dtSchedule.Columns.Add("SignalType");
            dtSchedule.Columns.Add("CableLength(m)");
            dtSchedule.Columns.Add("Remark");
            var set = vdFramedControl1.BaseControl.ActiveDocument.Selections;
            foreach (var slc in set)
            {
                var lstpos = ((VectorDraw.Professional.vdCollections.vdSelection)slc).ListPosition;
                foreach (var lst in ((VectorDraw.Generics.vdArray<VectorDraw.Professional.vdPrimaries.vdFigure>)lstpos).ArrayItems)
                    if (lst is vdPolyline vpl && vpl.Layer.Name == "Destination")
                    {
                        foreach (DataRow dr in dtInstrument.Rows)
                        {
                            if (dr["Id"].ToString() == vpl.Id.ToString())
                            {
                                dtSchedule.Rows.Add(new object[]
                            {
                                dr["No"].ToString() ,
                                "",
                                "",
                                dr["Instrument"].ToString() ,
                                "I/O Room-1",
                                "",
                                dr["Dimension"].ToString() ,
                                dr["Instrument"].ToString().Contains("Analyzer")? ("Power AC 220"): "" ,

                            });
                            }
                        }
                    }
            }

           if ( dtSchedule.Rows.Count == 0 )
            {
                MessageBox.Show("Please select some cable duct routes to export.");
                return;
            }
            if (!Directory.Exists(DataSourceCableDuctSchedule))
            {
                Directory.CreateDirectory(DataSourceCableDuctSchedule);
            }

            using (var writer = new StreamWriter(DataSourceCableDuctSchedule + GetFileName()))
            using (var csvWriter = new CsvWriter(writer))
            {
                List<CableSchedule> lst = new List<CableSchedule>();
                CableSchedule ie;

                foreach (DataRow dr in dtSchedule.Rows)
                {
                    ie = new CableSchedule();
                    ie.TagNo = dr["TagNo"].ToString().Trim();
                    ie.System = dr["System"].ToString().Trim();
                    ie.CableSpecification = dr["CableSpecification"].ToString().Trim();
                    ie.From = dr["From"].ToString().Trim();
                    ie.To = dr["To"].ToString().Trim();
                    ie.SignalType = dr["SignalType"].ToString().Trim();
                    ie.CableLength_m = dr["CableLength(m)"].ToString().Trim();
                    ie.Remark = dr["Remark"].ToString().Trim(); 
                    lst.Add(ie); 
                }
                csvWriter.WriteRecords(lst);
                csvWriter.Flush();
                writer.Flush();
                writer.Close();
            }
            System.Diagnostics.Process.Start(DataSourceCableDuctSchedule);

        }
        private string GetFileName()
        {
            return  "CableSchedule_"+DateTime.Now.ToString("yyyymmddHHmmss").Replace(" ", "").Replace(":", "").Replace("-", "").Replace("/", "")+".csv";
        }
        public static DataTable dtOptimalResult;
        private void button5_Click(object sender, EventArgs e)
        {

           
            Segmentaion();

        }

        private void Segmentaion()
        {
            dtSegmentation = new DataTable();
            dtSegmentation.Columns.Add("PK");
            dtSegmentation.Columns.Add("Instrument");
            dtSegmentation.Columns.Add("Id");
            dtSegmentation.Columns.Add("SEQ");
            dtSegmentation.Columns.Add("SP");//typeof(gPoint)
            dtSegmentation.Columns.Add("EP");
            dtSegmentation.Columns.Add("Length(m)");
            dtSegmentation.Columns.Add("SquareArea");
            dtSegmentation.Columns.Add("UsedFlg");
            dtSegmentation.Columns.Add("Remark");
            int pk = 0;
            foreach (DataRow dr in dtInstrument.Rows) //Get All Full Paths
            {

                if (!dr["Instrument"].ToString().Contains("Analyzer")) // Skipped Anylyzer Logic
                {

                    var id = Convert.ToInt32(dr["Id"].ToString());
                    var ent = vdFramedControl1.BaseControl.ActiveDocument.Model.Entities.FindFromId(id).Explode();
                    int seq = 0;
                    foreach (vdFigure vf in ent)
                    {
                        pk++; seq++;
                        if (vf is vdLine vl)
                        {
                            dtSegmentation.Rows.Add(new object[] {
                            pk,
                            dr["Instrument"].ToString(),
                            id,
                            seq,
                            vl.StartPoint.ToString(),
                            vl.EndPoint.ToString(),
                            dr["Dimension"].ToString(),
                            dr["InstrumentType"].ToString().Split('_').Last().Split('_').Last(),
                            "0",
                            ""
                            });
                        }
                    }
                }
            }
            DataTable dt_segment = new DataTable();
            dt_segment.Columns.Add("SG");
            dt_segment.Columns.Add("PK");
            dt_segment.Columns.Add("Id");
            dt_segment.Columns.Add("SP");
            dt_segment.Columns.Add("EP");
            dt_segment.Columns.Add("OptimalDuctSize");
            dt_segment.Columns.Add("Length");
            dt_segment.Columns.Add("SquareArea");
            dt_segment.Columns.Add("TotalCable");
            int segmt_q = 0;
            int grpCount = 0;
            int pk_q = 0;
            bool IsNewSegmt = false;
            string ep_Last = "";
            foreach (DataRow dr in dtSegmentation.Rows)
            {
                if (dr["UsedFlg"].ToString() == "0")
                {
                    var sp_p = dr["SP"].ToString(); var ep_p = dr["EP"].ToString();
                    var dr_p = dtSegmentation.Select("SP = '" + sp_p + "' and EP = '" + ep_p + "' and UsedFlg = '0'");

                    if (dr_p != null && dr_p.CopyToDataTable().Rows.Count > 0)
                    {
                        if (grpCount != dr_p.Count() || segmt_q == 0 || IsNewSegmt || ep_Last != sp_p)
                            segmt_q++;
                        var dt_p = dr_p.CopyToDataTable();
                        foreach (DataRow dr_q in dt_p.Rows)
                        {
                            pk_q++;
                            var sp_q = dr_q["SP"].ToString(); var ep_q = dr_q["EP"].ToString(); var id_q = dr_q["Id"].ToString(); var prk_q = dr_q["PK"].ToString();
                            dtSegmentation.Rows[Convert.ToInt32(prk_q) - 1]["UsedFlg"] = "1";
                            dtSegmentation.AcceptChanges();
                            dt_segment.Rows.Add(segmt_q, pk_q, id_q, sp_q, ep_q, "", "", "", "");
                            ep_Last = ep_q;
                        }
                        if (ep_p == destination.gridPoint.ToString())
                            IsNewSegmt = true;
                        else
                            IsNewSegmt = false;
                        grpCount = dr_p.Count();
                    }
                    dtSegmentation.AcceptChanges();
                }
            }

            foreach (DataRow dr in dt_segment.Rows)
            {
                var id = dr["Id"].ToString();
                dr["SquareArea"] = dtInstrument.Select(" id = '" + id + "'").CopyToDataTable().Rows[0]["InstrumentType"].ToString().Split('_').Last();

            }
            var dt_Type = dt_segment.AsEnumerable().GroupBy(r => new { Col1 = r["SG"], Col2 = r["Id"] }).Select(g => g.OrderBy(r => r["SG"]).First()).CopyToDataTable();
            foreach (DataRow dr in dt_segment.Rows)
            {
                var SG = dr["SG"].ToString();
                var totalTypes = dt_Type.Select(" SG = '" + SG + "'").CopyToDataTable();
                int totalCables = 0;
                double totalAreas = 0.0;
                foreach (DataRow dr_t in totalTypes.Rows)
                {
                    totalCables++;
                    totalAreas += Convert.ToDouble(dr_t["SquareArea"].ToString());
                }
                dr["OptimalDuctSize"] = Convert.ToInt32((totalAreas /  (Convert.ToDouble(txtDuctOptimize.Text)/100)).ToString()); // y= 0.2 x
                dr["TotalCable"] = totalCables.ToString();
            }

            var dt_poly = dt_segment.AsEnumerable().GroupBy(r => new { Col1 = r["SG"], Col2 = r["SP"], Col3 = r["EP"] }).Select(g => g.OrderBy(r => r["SG"]).First()).CopyToDataTable();

           
            foreach (DataRow drs in dt_segment.Rows)
            {
                double lengthSegmt = 0.0;
                var sg = drs["SG"].ToString();
                var polys = dt_poly.Select(" SG = '" + sg + "'").CopyToDataTable();
                foreach (DataRow dr in polys.Rows)
                {
                    lengthSegmt += Gp(dr["SP"].ToString()).Distance2D(Gp(dr["EP"].ToString()));
                }
                drs["Length"] = Convert.ToInt32( lengthSegmt/1000 );
                drs["SG"] = "Segment_" + drs["SG"].ToString();
            }
            dtOptimalResult = dt_segment.AsEnumerable().GroupBy(r => new { Col1 = r["SG"] }).Select(g => g.OrderBy(r => r["SG"]).First()).CopyToDataTable();
            foreach (DataRow dr in dtOptimalResult.Rows)
            {
                TextInsert( Gp(dr["SP"].ToString()).x, Gp(dr["EP"].ToString()).y, dr["SG"].ToString());
            }
             
            Form6 pro = new Form6();
            pro.ShowDialog();
        }
        
        private gPoint Gp(string pts)
        { 
            return new gPoint( Convert.ToDouble(pts.Split(',')[0].ToString()) , Convert.ToDouble(pts.Split(',')[1].ToString()), Convert.ToDouble(pts.Split(',')[2].ToString())); 
        }
        private void button6_Click(object sender, EventArgs e)
        { 
            Form5 pro = new Form5();
            pro.ShowDialog();  
        }

      
    }
    public class CableSchedule
    {
        public string TagNo { get; set; }
        public string System { get; set; }
        public string CableSpecification { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string SignalType { get; set; }
        public string CableLength_m { get; set; }
        public string Remark { get; set; }

    }
}
