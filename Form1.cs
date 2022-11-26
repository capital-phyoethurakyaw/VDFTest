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
                //We will create a vdCircle object and add it to the Active Layout which is the basic Model Layout always existing in a Document.
                VectorDraw.Professional.vdFigures.vdCircle onecircle = new VectorDraw.Professional.vdFigures.vdCircle();
                //We set the document where the circle is going to be added.This is important for the vdCircle in order to obtain initial properties with setDocumentDefaults.
                onecircle.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
                onecircle.setDocumentDefaults();

                //The two previus steps are important if a vdFigure object is going to be added to a document.
                //Now we will change some properties of the circle.
                onecircle.Center = new VectorDraw.Geometry.gPoint(30, 30);
                onecircle.Radius = 5;
                onecircle.PenColor.SystemColor = Color.BurlyWood;
                onecircle.Label = "This is a vdCircle object.";
                //This line Type is the same indipending from the zoom.Also the LineTypeScale has no effect,See next object for other linetypes.
                onecircle.LineType = vdFramedControl1.BaseControl.ActiveDocument.LineTypes.DPIDashDotDot;

                //Now we will add this object to the Entities collection of the Model Layout(ActiveLayout).
                vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(onecircle);

                //Zoom in order to see the object.
                vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.ZoomWindow(new VectorDraw.Geometry.gPoint(-100.0, -40.0), new VectorDraw.Geometry.gPoint(140.0, 90.0));
                //Redraw the document to see the above changes.
                vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
        }

        private void butNew_Click(object sender, EventArgs e)
        {
            vdFramedControl1.BaseControl.ActiveDocument.New();
            vdFramedControl1.BaseControl.ActiveDocument.ShowUCSAxis = false; 
            butCircle.Enabled = true; 
            butPolyline.Enabled = true;
            this.Refresh();
        } 
        private void Form1_Load(object sender, EventArgs e)
        {
            vdFramedControl1.SetLayoutStyle(vdControls.vdFramedControl.LayoutStyle.CommandLine, false);
            vdFramedControl1.SetLayoutStyle(vdControls.vdFramedControl.LayoutStyle.LayoutTab, false);
            vdFramedControl1.UnLoadMenu();
            vdFramedControl1.BaseControl.ActiveDocument.ShowUCSAxis = false;

            //We add the "RequiredFiles" folder to the support path. This addition is made in order for our samples to get the required files from this extra folder used in distribution of our sample files.
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\RequiredFiles\\";
            vdFramedControl1.BaseControl.ActiveDocument.SupportPath = path;
        }

        private void butPolyline_Click(object sender, EventArgs e)
        {
            butPolyline.Enabled = false;

            //We will create a vdPolyline object and add it to the Active Layout which is the basic Model Layout always existing in a Document.
            VectorDraw.Professional.vdFigures.vdPolyline onepoly = new VectorDraw.Professional.vdFigures.vdPolyline();
            //We set the document where the polyline is going to be added.This is important for the vdPolyline in order to obtain initial properties with setDocumentDefaults.
            onepoly.SetUnRegisterDocument(vdFramedControl1.BaseControl.ActiveDocument);
            onepoly.setDocumentDefaults();
             
            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(-10, -10));
            onepoly.VertexList.Add(new VectorDraw.Geometry.gPoint(40, -10)); 
            onepoly.VertexList.Add(new VectorDraw.Geometry.Vertex(40, 20, 0));
            onepoly.VertexList.Add(new VectorDraw.Geometry.Vertex(-10, 20, 0));
         
            onepoly.Flag = VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE;
            onepoly.PenColor.ColorIndex = 200;



            //Now we will add this object to the Entities collection of the Model Layout(ActiveLayout).
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(onepoly);

            //Zoom in order to see the object.
            vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.ZoomWindow(new VectorDraw.Geometry.gPoint(-100.0, -40.0), new VectorDraw.Geometry.gPoint(140.0, 90.0));
            //Redraw the document to see the above changes.
            vdFramedControl1.BaseControl.ActiveDocument.Redraw(true);
        }
    }
}
