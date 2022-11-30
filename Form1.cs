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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

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

        private void butNew_Click(object sender, EventArgs e)
        {
            vdFramedControl1.BaseControl.ActiveDocument.New();
            vdFramedControl1.BaseControl.ActiveDocument.ShowUCSAxis = false; 
            butCircle.Enabled = button1.Enabled = button2.Enabled = button3.Enabled = butPolyline.Enabled = true;
            vdFramedControl1.Select();
            this.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            vdFramedControl1.SetLayoutStyle(vdControls.vdFramedControl.LayoutStyle.CommandLine, true);
            vdFramedControl1.SetLayoutStyle(vdControls.vdFramedControl.LayoutStyle.LayoutTab, false);
            // vdFramedControl1.UnLoadMenu();
            vdFramedControl1.BaseControl.ActiveDocument.ShowUCSAxis = false;
   string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\RequiredFiles\\";
            vdFramedControl1.BaseControl.ActiveDocument.SupportPath = path;
        }

        private void butPolyline_Click(object sender, EventArgs e)
        {
            butPolyline.Enabled = false;
             
            VectorDraw.Professional.vdFigures.vdPolyline onepoly = new VectorDraw.Professional.vdFigures.vdPolyline(); 
            onepoly.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            onepoly.setDocumentDefaults();

            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(-10, -10));
            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(40, -10));
            onepoly.VertexList.Add(new VectorDraw.Geometry.Vertex(40, 20, 0));
            onepoly.VertexList.Add(new VectorDraw.Geometry.Vertex(-10, 20, 0));
            onepoly.ToolTip = "Destination";
            onepoly.Flag = VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE;
            onepoly.PenColor.ColorIndex = 200; 
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(onepoly);
             
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.ZoomWindow(new VectorDraw.Geometry.gPoint(-100.0, -40.0), new VectorDraw.Geometry.gPoint(140.0, 90.0));
          
            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true); 

        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false; 

            Routing(); 
        }

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
        private bool IsOver2SideHeight(VectorDraw.Geometry.Box ObsCenter, VectorDraw.Geometry.Box TargetCenter, VectorDraw.Professional.vdFigures.vdCircle ObserverCenter, out double length)
        {
            double val = 0.0;
            //Changed to Target's height Even inside range
            if ((ObsCenter.MidPoint.y - (ObsCenter.Height / 2)) <= TargetCenter.MidPoint.y && (ObsCenter.MidPoint.y + (ObsCenter.Height / 2)) >= TargetCenter.MidPoint.y && ObserverCenter.Center.x > ObsCenter.MidPoint.x)
            {
                //Total would be 3 path
                //Add extra ObsServer temp 
                DrawRoute(ObserverCenter.Center.x, ObserverCenter.Center.y, ObserverCenter.Center.x, (ObserverCenter.Center.y + (ObsCenter.Height)));
                val += Math.Abs(Math.Abs(ObserverCenter.Center.y) - Math.Abs((ObserverCenter.Center.y + (ObsCenter.Height))));
                DrawRoute(ObserverCenter.Center.x, (ObserverCenter.Center.y + ObsCenter.Height), TargetCenter.MidPoint.x, (ObserverCenter.Center.y + ObsCenter.Height));
                val += Math.Abs(Math.Abs(ObserverCenter.Center.x) - Math.Abs(TargetCenter.MidPoint.x));
                DrawRoute(TargetCenter.MidPoint.x, (ObserverCenter.Center.y + ObsCenter.Height), TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
                val += Math.Abs(Math.Abs((ObserverCenter.Center.y + ObsCenter.Height)) - Math.Abs(TargetCenter.MidPoint.y));
                length = val;
                return true;
            }
            length = val;
            return false;
        }

        private bool IsOver2SideWeight(VectorDraw.Geometry.Box ObsCenter, VectorDraw.Geometry.Box TargetCenter, VectorDraw.Professional.vdFigures.vdCircle ObserverCenter, out double length)
        {
            double val = 0.0;
            //Changed to Target's width Even inside range
            if ((ObsCenter.MidPoint.x - (ObsCenter.Width / 2)) <= TargetCenter.MidPoint.x && (ObsCenter.MidPoint.x + (ObsCenter.Width / 2)) >= TargetCenter.MidPoint.x && ObserverCenter.Center.y > ObsCenter.MidPoint.y)
            {
                //Total would be 3 path
                //Add extra ObsServer temp 
                DrawRoute(ObserverCenter.Center.x, ObserverCenter.Center.y, (ObserverCenter.Center.x+ ObsCenter.Width), ObserverCenter.Center.y);
                val += Math.Abs(Math.Abs(ObserverCenter.Center.x) - Math.Abs((ObserverCenter.Center.x + ObsCenter.Width)));
                DrawRoute((ObserverCenter.Center.x + ObsCenter.Width), ObserverCenter.Center.y, (ObserverCenter.Center.x + ObsCenter.Width), TargetCenter.MidPoint.y);
                val += Math.Abs(Math.Abs(ObserverCenter.Center.y) - Math.Abs(TargetCenter.MidPoint.y));
                DrawRoute((ObserverCenter.Center.x + ObsCenter.Width), TargetCenter.MidPoint.y, TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
                val += Math.Abs(Math.Abs((ObserverCenter.Center.x + ObsCenter.Width)) - Math.Abs(TargetCenter.MidPoint.x));
                length = val;
                return true;
            }
            length = val;
            return false;
        }
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
                var TargetCenter = Tar.VertexList.GetBox();// Tar.VertexList.Id;
                var ObsCenter = Obs.VertexList.GetBox();// Tar.VertexList.Id;

                //Avoid height Obstacle
                if ((ObsCenter.MidPoint.y - (ObsCenter.Height / 2)) <= ObserverCenter.Center.y && (ObsCenter.MidPoint.y + (ObsCenter.Height / 2)) >= ObserverCenter.Center.y)
                {
                    if (!IsOver2SideHeight(ObsCenter, TargetCenter, ObserverCenter, out double length))
                    {
                        PathLength = GoHorizontal(TargetCenter, ObserverCenter);
                    }
                    else
                        PathLength = length;
                       
                }
                //Avoid Widht Obstacle
                else if ((ObsCenter.MidPoint.x - (ObsCenter.Width / 2)) <= ObserverCenter.Center.x && (ObsCenter.MidPoint.x + (ObsCenter.Width / 2)) >= ObserverCenter.Center.x)
                {
                    if (!IsOver2SideWeight(ObsCenter, TargetCenter, ObserverCenter, out double length))
                        PathLength=GoVertical(TargetCenter, ObserverCenter);
                    else
                        PathLength = length;
                }
                else
                {
                    //Make Shortern Priority
                    if ((TargetCenter.MidPoint.y - ObserverCenter.Center.y) < (TargetCenter.MidPoint.x - ObserverCenter.Center.x))
                    {
                        if ((ObsCenter.MidPoint.y - (ObsCenter.Height / 2)) <= TargetCenter.MidPoint.y && (ObsCenter.MidPoint.y + (ObsCenter.Height / 2)) >= TargetCenter.MidPoint.y)
                            PathLength=GoVertical(TargetCenter, ObserverCenter);
                        else
                            PathLength=GoHorizontal(TargetCenter, ObserverCenter);
                    }
                    else
                    {
                        if ((ObsCenter.MidPoint.x - (ObsCenter.Width / 2)) <= TargetCenter.MidPoint.x && (ObsCenter.MidPoint.x + (ObsCenter.Width / 2)) >= TargetCenter.MidPoint.x)

                            PathLength=GoHorizontal(TargetCenter, ObserverCenter);
                        else
                            PathLength=GoVertical(TargetCenter, ObserverCenter);

                    }
                }
                TextInsert(ObserverCenter.Center.x, ObserverCenter.Center.y, "Instrument" + i.ToString());
                dtInstrument.Rows.Add(new object[] {
                i , "Instrument" + i.ToString(),  Convert.ToInt32(PathLength).ToString()
                }) ;
            }
        }
        private double GoVertical(VectorDraw.Geometry.Box TargetCenter, VectorDraw.Professional.vdFigures.vdCircle ObserverCenter)
        {
            double val = 0.0;
            DrawRoute(ObserverCenter.Center.x, ObserverCenter.Center.y, TargetCenter.MidPoint.x, ObserverCenter.Center.y);
            val += Math.Abs(Math.Abs(ObserverCenter.Center.x) - Math.Abs(TargetCenter.MidPoint.x));
            DrawRoute(TargetCenter.MidPoint.x, ObserverCenter.Center.y, TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
            val += Math.Abs(Math.Abs(ObserverCenter.Center.y) - Math.Abs(TargetCenter.MidPoint.y));
            return val;
        }
        private double GoHorizontal(VectorDraw.Geometry.Box TargetCenter, VectorDraw.Professional.vdFigures.vdCircle ObserverCenter)
        {
            double val = 0.0;
            DrawRoute(ObserverCenter.Center.x, ObserverCenter.Center.y, ObserverCenter.Center.x, TargetCenter.MidPoint.y);
            val += Math.Abs(Math.Abs(ObserverCenter.Center.y) - Math.Abs(TargetCenter.MidPoint.y));
            DrawRoute(ObserverCenter.Center.x, TargetCenter.MidPoint.y, TargetCenter.MidPoint.x, TargetCenter.MidPoint.y);
            val += Math.Abs(Math.Abs(ObserverCenter.Center.x) - Math.Abs(TargetCenter.MidPoint.x));
            return val;
        }
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
