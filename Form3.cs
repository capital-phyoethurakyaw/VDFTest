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
            //openFileDialog1.RestoreDirectory = true; 
            //openFileDialog1.Filter = "DWG files (*.dwg)|*.dwg*";
            //openFileDialog1.FilterIndex = 2;
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{  var file = openFileDialog1.FileName;

            object ret = vdFramedControl1.BaseControl.ActiveDocument.GetOpenFileNameDlg(0, "", 0);
            if (ret == null) return;
            string fname = (string)ret;
            bool success = vdFramedControl1.BaseControl.ActiveDocument.Open(fname);
            if (success)
            {
                vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
                BindLayer(vdFramedControl1);
            }
            //}
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

        private void Display() //VectorDraw.Professional.vdPrimaries.vdLayer lyr, bool IsRemoved = false
        {
            //*** We will be in a big polygonal Obstacle to build route. So, this big one should be neglected 



            //As usual
            List<VectorDraw.Professional.vdFigures.vdCircle> LstObserverObjects;
            List<VectorDraw.Professional.vdFigures.vdPolyline> TargetObjects;

            //Only Line, Insert, Polyline include in Obstacle Layer     
            List<VectorDraw.Professional.vdFigures.vdLine> ObstacleLineObjects;   
            List<VectorDraw.Professional.vdFigures.vdInsert> ObstacleVdInsertObjects;

            //Test
            //List<VectorDraw.Professional.vdPrimaries.vdBlock> vdBlocks = new List<VectorDraw.Professional.vdPrimaries.vdBlock>();
          
            var intr = lyrInstrument;

            //Each selected List 
            foreach (var itm in lstInstrument.Items)
            {
                LstObserverObjects      = new List<VectorDraw.Professional.vdFigures.vdCircle>();
                TargetObjects           = new List<VectorDraw.Professional.vdFigures.vdPolyline>(); 
                ObstacleLineObjects     = new List<VectorDraw.Professional.vdFigures.vdLine>();
                ObstacleVdInsertObjects = new List<VectorDraw.Professional.vdFigures.vdInsert>();

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
                            LstObserverObjects.Add(cirCle);
                        }
                    }
                    else if (v is VectorDraw.Professional.vdFigures.vdPolyline target)
                    {
                        if (target.Layer.Name == lyrDestination[0].Name.ToString())  // Only One Destination lyr
                        {
                            TargetObjects.Add(target);
                        }
                    }
                    //else if (v.GetType().ToString().ToLower().Contains("block")) // Check Blocks in Obstale
                    else if ((v is VectorDraw.Professional.vdFigures.vdLine blkType1))
                    {
                        if (blkType1.Layer.Name == lyrObstacle[0].Name.ToString())//|| blkType1.Layer.Name != lyrObstacle[0].Name.ToString())
                        {
                            ObstacleLineObjects.Add(blkType1);
                        }

                    }
                    else if ((v is VectorDraw.Professional.vdFigures.vdInsert blkType2))
                    {
                        if (blkType2.Layer.Name == lyrObstacle[0].Name.ToString())//|| blkType2.Layer.Name != lyrObstacle[0].Name.ToString())
                        {
                            ObstacleVdInsertObjects.Add(blkType2);
                        }
                    }


                }

                //Star Collect btw points
                foreach (var lst in LstObserverObjects)
                {
                    SettingRoute(TargetObjects[0], lst, ObstacleLineObjects, ObstacleVdInsertObjects);

                    // SettingRoute(TargetObjects[0].VertexList.GetBox().MidPoint, lst.Center, ObstacleLineObjects, ObstacleVdInsertObjects);
                }
                //ObstacleLineObjects[0].get
                //  var inr = ObstacleVdInsertObjects[0];
                //// var d= inr.Block; 
                //var d=  inr.BoundingBox.GetPoints().GetBox().MidPoint;

            }
            
            //    vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
            //    vdFramedControl1.BaseControl.Redraw();

            //}

        }
        //Colect Only inside Points
        private void SettingRoute(VectorDraw.Professional.vdFigures.vdPolyline TPoint, VectorDraw.Professional.vdFigures.vdCircle IPoint, List<VectorDraw.Professional.vdFigures.vdLine> ObsLine, List<VectorDraw.Professional.vdFigures.vdInsert> ObsInsert)
        {
            List<VectorDraw.Professional.vdFigures.vdLine> ObsLineIn = new List<VectorDraw.Professional.vdFigures.vdLine>(); 
            foreach (var ol in ObsLine)
            {
               if ( ol.getStartPoint().x  > TPoint.VertexList.GetBox().MidPoint.x && ol.getEndPoint().x < IPoint.Center.x  && ol.getStartPoint().y > TPoint.VertexList.GetBox().MidPoint.y && ol.getEndPoint().y < IPoint.Center.y)
                {
                    ObsLineIn.Add(ol);
                }
            }
            //foreach (var oi in ObsInsert)
            //{
            //    var insss = oi.GetGripPoints();
            //    var sss = oi.InsertionPoint;
            //    var dddd= oi.BoundingBox.GetPoints().GetBox().MidPoint;
            //    //if (oi.poin.x > TPoint.x && ol.getEndPoint().x < IPoint.x && ol.getStartPoint().y > TPoint.y && ol.getEndPoint().y < IPoint.y)
            //    //{
            //    //    ObsLineIn.Add(ol);
            //    //}
            //}
            if (ObsLineIn.Count > 0)
            StartRoute(ObsLineIn, TPoint , IPoint);
        }
        private void StartRoute(List<VectorDraw.Professional.vdFigures.vdLine> lstObstacles, VectorDraw.Professional.vdFigures.vdPolyline Tar, VectorDraw.Professional.vdFigures.vdCircle Observer)
        {

            // int i = 0;

            List<VectorDraw.Professional.vdFigures.vdCircle> LstObserver = new List<VectorDraw.Professional.vdFigures.vdCircle>();
            LstObserver.Add(Observer);
            foreach (var ObserverCenter in LstObserver)
            {
                //i++;
                double PathLength = 0.0;
                //Get Center of Destination Target and Obstacle.
                var TargetCenter = Tar.VertexList.GetBox();
                bool Isvertical = true;
                foreach (var lstObstacle in lstObstacles)
                {
                    var ObsCenter = VectorDraw.Geometry.gPoint.MidPoint(lstObstacle.StartPoint, lstObstacle.EndPoint);// lstObstacle[0].mid; // Obs.VertexList.GetBox();

                    double heightobs = 0;
                    double widthobs = 0;
                    if (lstObstacle.StartPoint.x == lstObstacle.EndPoint.x)
                    {
                        heightobs = 0;
                    }
                    else
                    {
                        widthobs = lstObstacle.Length();
                    }
                    if (lstObstacle.StartPoint.y == lstObstacle.EndPoint.y)
                    {
                        widthobs = 0;
                    }
                    else
                    {
                        heightobs = lstObstacle.Length();
                    }

                    //Avoid height Obstacle
                    // Check Obstacle height is inside area of Instrument height
                    if ((ObsCenter.y - (heightobs / 2)) <= ObserverCenter.Center.y && (ObsCenter.y + (heightobs / 2)) >= ObserverCenter.Center.y)
                    {
                        // if (!IsOver2SideHeight(ObsCenter, TargetCenter, ObserverCenter, out double length))// Draw if 3 paths, if not draw 2 paths.
                        //  {
                        Isvertical = false;
                        //  PathLength = GoHorizontal(TargetCenter, ObserverCenter);//draw 2 paths.
                        //}
                        //else
                        //    PathLength = length;
                    }
                    //Avoid Widht Obstacle
                    // Check Obstacle width is inside area of Instrument width
                    else if ((ObsCenter.x - (widthobs / 2)) <= ObserverCenter.Center.x && (ObsCenter.x + (widthobs / 2)) >= ObserverCenter.Center.x)
                    {
                        // Isvertical = true;
                        // if (!IsOver2SideWidth(ObsCenter, TargetCenter, ObserverCenter, out double length))// Draw if 3 paths, if not draw 2 paths.
                        // PathLength = GoVertical(TargetCenter, ObserverCenter);//draw 2 paths.
                        //else
                        //    PathLength = length;
                    }
                    else
                    {
                        // It is okay to go vertical first or horizontal first. 
                        //But, Make Shortern Priority  
                        //eg if vertical path is shorter => Vertical path > horizontal
                        if ((TargetCenter.MidPoint.y - ObserverCenter.Center.y) < (TargetCenter.MidPoint.x - ObserverCenter.Center.x))
                        {
                            //In second route, Check and avoid not to pass through to an obstacle . 
                            //if exist, change route
                            if ((ObsCenter.y - (heightobs / 2)) <= TargetCenter.MidPoint.y && (ObsCenter.y + (heightobs / 2)) >= TargetCenter.MidPoint.y)
                            {
                                //Isvertical = true;  //PathLength = GoVertical(TargetCenter, ObserverCenter);
                            }
                            else
                            {
                                Isvertical = false; // PathLength = GoHorizontal(TargetCenter, ObserverCenter);
                            }
                        }
                        else
                        {
                            //In second route, Check and avoid not to pass through to an obstacle . 
                            //if exist, change route
                            if ((ObsCenter.x - (widthobs / 2)) <= TargetCenter.MidPoint.x && (ObsCenter.x + (widthobs / 2)) >= TargetCenter.MidPoint.x)

                                Isvertical = false;  // PathLength = GoHorizontal(TargetCenter, ObserverCenter);
                            //else
                            //    PathLength = GoVertical(TargetCenter, ObserverCenter);
                        }
                    }
                    if (!Isvertical)
                        break;
                }
                if (!Isvertical)
                    PathLength = GoHorizontal(TargetCenter, ObserverCenter);
                else
                    PathLength = GoVertical(TargetCenter, ObserverCenter);

                // Set instrument label and put at dtInstrument table 
                //TextInsert(ObserverCenter.Center.x, ObserverCenter.Center.y, "Instrument" + i.ToString());
                //dtInstrument.Rows.Add(new object[] {
                //i , "Instrument" + i.ToString(),  Convert.ToInt32(PathLength).ToString()
                //});
            }
        }
        private void lstInstrument_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnRouting_Click(object sender, EventArgs e)
        {
            //Not included in Obstacle Layer
            //var con = vdFramedControl1.BaseControl.ActiveDocument.Blocks;
            //foreach (vdBlock blk in con)
            //{
            //   if (blk.Document.ActiveLayer.Name.ToLower().Contains("obstacle"))
            //    { 
            //    } 
            //}


            //foreach (vdFigure figure in vdFramedControl1.BaseControl.ActiveDocument.Model.Entities)
            //{
            //    if (figure is  vdInsert)
            //    {
            //        var insert = (vdInsert)figure;
            //        vdEntities entities = insert.Explode();
            //        // Console.WriteLine(figure.GetType().ToString() + " " + this.vdScrollableControl1.BaseControl.ActiveDocument.Model.Entities.Count + " " + entities.Count);
            //       // findEntity(entities, list);
            //    }
            //      Console.WriteLine(figure.GetType().ToString());
            //}
            Display();
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
    }
}
