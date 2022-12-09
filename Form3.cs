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
             lyrInstrument  = new List<VectorDraw.Professional.vdPrimaries.vdLayer>();
             lyrDestination = new List<VectorDraw.Professional.vdPrimaries.vdLayer>();
             lyrObstacle    = new List<VectorDraw.Professional.vdPrimaries.vdLayer>();


            List<VectorDraw.Professional.vdFigures.vdCircle> LstObserver = new List<VectorDraw.Professional.vdFigures.vdCircle>();
            List<VectorDraw.Professional.vdFigures.vdPolyline> Target = new List<VectorDraw.Professional.vdFigures.vdPolyline>();
            List <VectorDraw.Professional.vdFigures.vdPolyline> Obstacle = new List<VectorDraw.Professional.vdFigures.vdPolyline>();
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
                        dtLyrIns.Rows.Add(new object[] { lyrCircle.Name});
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

        }

        private void btnDecre_Click(object sender, EventArgs e)
        {

        }
    }
}
