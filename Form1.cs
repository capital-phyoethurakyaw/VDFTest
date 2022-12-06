using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VFD1
{

    //Let Us Assume Destination and Obstacle fixed//
    public partial class Form1 : Form
    {
        /// <summary>
        /// InitializComponent
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Draw a circle instrument entity with a point specified at (30, 30) with radius 5 as default in a two-dimensional plane.
        /// </summary>
        private void butCircle_Click(object sender, EventArgs e)
        { 
            butCircle.Enabled = false; 
            VectorDraw.Professional.vdFigures.vdCircle onecircle = new VectorDraw.Professional.vdFigures.vdCircle(); 
            onecircle.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            onecircle.setDocumentDefaults(); 
            onecircle.Center = new VectorDraw.Geometry.gPoint(30, 30);
            onecircle.Radius = 5;
            onecircle.PenColor.ColorIndex = 50;
            onecircle.ToolTip = "Instrument"; 
            onecircle.LineType = vdFramedControl1.BaseControl.ActiveDocument.LineTypes.DPIDash;
             
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(onecircle);
             
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.ZoomWindow(new VectorDraw.Geometry.gPoint(-100.0, -40.0), new VectorDraw.Geometry.gPoint(140.0, 90.0));
          
            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
        }
        /// <summary>
        /// Get a new document in a two-dimensional plane and make all action available.
        /// </summary>
        private void butNew_Click(object sender, EventArgs e)
        {
            vdFramedControl1.BaseControl.ActiveDocument.New();
            vdFramedControl1.BaseControl.ActiveDocument.ShowUCSAxis = false; 
            butCircle.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = butPolyline.Enabled = true;
            vdFramedControl1.Select();
            this.Refresh();
        }
        /// <summary>
        /// Show command line for operation and XY axis.
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            vdFramedControl1.SetLayoutStyle(vdControls.vdFramedControl.LayoutStyle.CommandLine, true);
            vdFramedControl1.BaseControl.ActiveDocument.ShowUCSAxis = true;
        }
        /// <summary>
        /// Draw poly lines with 4 sides at (-10, -10) ,(40, -10) ,(40, 20 ),(-10, 20 ) as default on XY axis.
        /// And we will assume this polygon as a reference destination by observer instrument. 
        /// </summary>
        private void butPolyline_Click(object sender, EventArgs e)
        {
            butPolyline.Enabled = false;
            VectorDraw.Professional.vdFigures.vdPolyline onepoly = new VectorDraw.Professional.vdFigures.vdPolyline(); 
            onepoly.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            onepoly.setDocumentDefaults();
            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(-10, -10));
            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(40, -10));
            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(40, 20 ));
            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(-10, 20 ));
            onepoly.ToolTip = "Destination";
            onepoly.Flag = VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE;
            onepoly.PenColor.ColorIndex = 200; 
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(onepoly);
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.ZoomWindow(new VectorDraw.Geometry.gPoint(-100.0, -40.0), new VectorDraw.Geometry.gPoint(140.0, 90.0));
            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true); 

        }
        /// <summary>
        /// Calculate route setting for 4 observer instruments.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false; 

            Routing(); 
        }
        /// <summary>
        /// Draw poly lines with 4 sides at (5, 40) ,(30, 40) ,(30, 60 ),(5, 60 ) as default on XY axis.
        /// And we will assume this polygon as an obstacle object of observer instrument. 
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false; 
            VectorDraw.Professional.vdFigures.vdPolyline onepoly = new VectorDraw.Professional.vdFigures.vdPolyline(); 
            onepoly.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            onepoly.setDocumentDefaults();

            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(5, 40));
            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(30, 40));
            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(30, 60 ));
            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(5, 60 ));
            onepoly.ToolTip = "Obstacle";
            onepoly.Flag = VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE;
            onepoly.PenColor.ColorIndex = 100; 
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(onepoly); 
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.ZoomWindow(new VectorDraw.Geometry.gPoint(-100.0, -40.0), new VectorDraw.Geometry.gPoint(140.0, 90.0));
           
            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true); 
        }
        /// <summary>
        /// 2 Functions
        /// Find and Make all objects with respective labels.
        /// Check whether the specified 4 instruments, 1 obstacle and 1 destination objects are completed or not.
        /// </summary>
        private void Routing()
        {
            List<VectorDraw.Professional.vdFigures.vdCircle> LstObserver = new List<VectorDraw.Professional.vdFigures.vdCircle>();
            VectorDraw.Professional.vdFigures.vdPolyline Target = new VectorDraw.Professional.vdFigures.vdPolyline();
            VectorDraw.Professional.vdFigures.vdPolyline Obstacle = new VectorDraw.Professional.vdFigures.vdPolyline();
            //Find Target and observer  
            var allControl = vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.ArrayItems;
            foreach (var v in allControl)
            {
                if (v is VectorDraw.Professional.vdFigures.vdCircle cirCle)
                {
                    if (cirCle.PenColor.ColorIndex == 50)
                    LstObserver.Add(cirCle);
                }
                else if (v is VectorDraw.Professional.vdFigures.vdPolyline polyLine)
                {
                    if (polyLine.PenColor.ColorIndex == 200)
                    {
                        Target = polyLine;
                        TextInsert(polyLine.VertexList.GetBox().MidPoint.x, polyLine.VertexList.GetBox().MidPoint.y, "Destination");
                    }
                    else if (polyLine.PenColor.ColorIndex == 100)
                    {
                        Obstacle = polyLine;
                        TextInsert(polyLine.VertexList.GetBox().MidPoint.x, polyLine.VertexList.GetBox().MidPoint.y, "Obstacle");

                    }

                }
            }

            if (LstObserver.Count == 4 && Target.Document != null && Obstacle.Document != null)
            {
                SetRoute(LstObserver, Target, Obstacle);
            }
            else
            {
                MessageBox.Show("Please make setting with 1 Destination, 1 Obstacle and 4 Instruments. If Error Repeat, get new Document", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// Check 2 or 3 path 
        /// return false if 2 paths for vertically
        /// draw and return true if 3 paths for vertically
        /// </summary>
        private bool IsOver2SideHeight(VectorDraw.Geometry.Box ObsCenter, VectorDraw.Geometry.Box TargetCenter, VectorDraw.Professional.vdFigures.vdCircle ObserverCenter, out double length)
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
                DrawRoute(ObserverCenter.Center.x, ObserverCenter.Center.y, ObserverCenter.Center.x, (ObserverCenter.Center.y + (ObsCenter.Height)));
                val += Math.Abs( ObserverCenter.Center.y -   ( ObserverCenter.Center.y  +  ObsCenter.Height  ));
                DrawRoute(ObserverCenter.Center.x, (ObserverCenter.Center.y + ObsCenter.Height), TargetCenter.MidPoint.x, (ObserverCenter.Center.y + ObsCenter.Height));
                val += Math.Abs( ObserverCenter.Center.x  -   TargetCenter.MidPoint.x );
                DrawRoute(TargetCenter.MidPoint.x, (ObserverCenter.Center.y + ObsCenter.Height), TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
                val += Math.Abs( (ObserverCenter.Center.y + ObsCenter.Height) -   TargetCenter.MidPoint.y );
                length = val;
                return true;
            }
            length = val;
            return false;
        }
        /// <summary>
        /// Check 2 or 3 path 
        /// return false if 2 paths for horizontally
        /// draw and return true if 3 paths for horizontally
        /// </summary>
        private bool IsOver2SideWidth(VectorDraw.Geometry.Box ObsCenter, VectorDraw.Geometry.Box TargetCenter, VectorDraw.Professional.vdFigures.vdCircle ObserverCenter, out double length)
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
                DrawRoute(ObserverCenter.Center.x, ObserverCenter.Center.y, (ObserverCenter.Center.x+ ObsCenter.Width), ObserverCenter.Center.y);
                val += Math.Abs( ObserverCenter.Center.x  -   ( ObserverCenter.Center.x + ObsCenter.Width ));
                DrawRoute((ObserverCenter.Center.x + ObsCenter.Width), ObserverCenter.Center.y, (ObserverCenter.Center.x + ObsCenter.Width), TargetCenter.MidPoint.y);
                val += Math.Abs( ObserverCenter.Center.y  -  TargetCenter.MidPoint.y) ;
                DrawRoute((ObserverCenter.Center.x + ObsCenter.Width), TargetCenter.MidPoint.y, TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
                val += Math.Abs( ( ObserverCenter.Center.x  + ObsCenter.Width)  -   TargetCenter.MidPoint.x );
                length = val;
                return true;
            }
            length = val;
            return false;
        }
        /// <summary>
        /// Draw text for Destination and Obstacle according to x,y values on XY axis. 
        /// </summary>
        private void TextInsert(double x, double y, string txt)
        {
            VectorDraw.Professional.vdFigures.vdText text = new VectorDraw.Professional.vdFigures.vdText();
            text.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            text.setDocumentDefaults();
            text.InsertionPoint = new VectorDraw.Geometry.gPoint(x-6, y-6);
            text.TextString = txt;
            text.Height = 2.0;
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(text);
            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
            //We set as active layout the first addede layo
        }
        /// <summary>
        /// Draw text for instrument according to x,y values and set route on XY axis. 
        /// </summary>
        private void SetRoute(List<VectorDraw.Professional.vdFigures.vdCircle> LstObserver, VectorDraw.Professional.vdFigures.vdPolyline Tar, VectorDraw.Professional.vdFigures.vdPolyline Obs)
        {
            dtInstrument = new DataTable();
            dtInstrument.Columns.Add("No");
            dtInstrument.Columns.Add("Instrument");
            dtInstrument.Columns.Add("Dimension");

            //Target Vs Obstacle 
            int i = 0;
            foreach (var ObserverCenter in LstObserver)
            {
                i++;
                double PathLength = 0.0; 
                //Get Center of Destination Target and Obstacle.
                var TargetCenter = Tar.VertexList.GetBox(); 
                var ObsCenter = Obs.VertexList.GetBox(); 

                //Avoid height Obstacle
                // Check Obstacle height is inside area of Instrument height
                if ((ObsCenter.MidPoint.y - (ObsCenter.Height / 2)) <= ObserverCenter.Center.y && (ObsCenter.MidPoint.y + (ObsCenter.Height / 2)) >= ObserverCenter.Center.y)
                {
                    if (!IsOver2SideHeight(ObsCenter, TargetCenter, ObserverCenter, out double length))// Draw if 3 paths, if not draw 2 paths.
                    {
                        PathLength = GoHorizontal(TargetCenter, ObserverCenter);//draw 2 paths.
                    }
                    else
                        PathLength = length; 
                }
                //Avoid Widht Obstacle
                // Check Obstacle width is inside area of Instrument width
                else if ((ObsCenter.MidPoint.x - (ObsCenter.Width / 2)) <= ObserverCenter.Center.x && (ObsCenter.MidPoint.x + (ObsCenter.Width / 2)) >= ObserverCenter.Center.x)
                { 
                    if (!IsOver2SideWidth(ObsCenter, TargetCenter, ObserverCenter, out double length))// Draw if 3 paths, if not draw 2 paths.
                        PathLength=GoVertical(TargetCenter, ObserverCenter);//draw 2 paths.
                    else
                        PathLength = length;
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
                        if ((ObsCenter.MidPoint.y - (ObsCenter.Height / 2)) <= TargetCenter.MidPoint.y && (ObsCenter.MidPoint.y + (ObsCenter.Height / 2)) >= TargetCenter.MidPoint.y)
                            PathLength = GoVertical(TargetCenter, ObserverCenter);
                        else
                            PathLength = GoHorizontal(TargetCenter, ObserverCenter);
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
                }
                TextInsert(ObserverCenter.Center.x, ObserverCenter.Center.y, "Instrument" + i.ToString());
                dtInstrument.Rows.Add(new object[] {
                i , "Instrument" + i.ToString(),  Convert.ToInt32(PathLength).ToString()
                }) ;
            }
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
            val += Math.Abs( ObserverCenter.Center.x  -  TargetCenter.MidPoint.x );
            DrawRoute(TargetCenter.MidPoint.x, ObserverCenter.Center.y, TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
            val += Math.Abs( ObserverCenter.Center.y  - TargetCenter.MidPoint.y );
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
            val += Math.Abs( ObserverCenter.Center.y  -  TargetCenter.MidPoint.y );
            DrawRoute(ObserverCenter.Center.x, TargetCenter.MidPoint.y, TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
            val += Math.Abs( ObserverCenter.Center.x  -  TargetCenter.MidPoint.x );
            return val;
        }
        /// <summary>
        /// Draw a line with points (x1,y1), (x2,y2)  on XY axis.
        /// </summary>
        private void DrawRoute( double x1, double y1, double x2, double y2)
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
        /// <summary>
        /// Show dimensions of 4 routed path.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            Form2 frm = new Form2(dtInstrument);
            frm.WindowState = FormWindowState.Normal; 
            frm.ShowDialog();
        }

        private static DataTable dtInstrument;
        
    }
   
}
