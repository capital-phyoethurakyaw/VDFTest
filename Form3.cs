using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VectorDraw.Geometry;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdPrimaries;

namespace VFD1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Import();
         
            //}
        }
        private void Import(bool IsDrivenCode= false)
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
                        var selectedLyr = lyrInstrument.Where(x => x.Name.Equals(newlyrIns) ).ToList();
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
                LstObserverObjects      = new List<VectorDraw.Professional.vdFigures.vdCircle>();
                TargetObjects           = new List<VectorDraw.Professional.vdFigures.vdPolyline>(); 
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
                            if (LstObserverObjects.Where(c=> c.Center == cirCle.Center).ToList().Count == 0 )
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
        private void SettingRoute(VectorDraw.Professional.vdFigures.vdPolyline TPoint, VectorDraw.Professional.vdFigures.vdCircle IPoint, List<VectorDraw.Professional.vdFigures.vdPolyline> ObsPoly )
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
            StartRoute(ObsPolyIn, TPoint , IPoint);
        }
        
        private bool IsOver2SideHeight(VectorDraw.Geometry.Box ObsCenter, VectorDraw.Geometry.Box TargetCenter, VectorDraw.Professional.vdFigures.vdCircle ObserverCenter,  out double length,bool OnlyCheck = true)
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
        private  int counter = 0;
        private static DataTable dtInstrument;
        private static string DocPath= "";
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
                            PathLength =  GoHorizontal(TargetCenter, ObserverCenter);//draw 2 paths. 
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
                    } ;

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
            text.InsertionPoint = new VectorDraw.Geometry.gPoint(x - 40, y - 40);
            text.TextString = txt;
            text.Height =45.0;
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(text);
            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
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
            Form2 frm = new Form2(dtInstrument);
            frm.WindowState = FormWindowState.Normal;
            frm.ShowDialog();
        }
    }
}
