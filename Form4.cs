 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vdControls;
using VectorDraw.Geometry;
using VectorDraw.Professional.Constants;
using VectorDraw.Professional.vdCollections;
using VectorDraw.Professional.vdFigures;
using VectorDraw.Professional.vdObjects;
using VectorDraw.Professional.vdPrimaries;

namespace VFD1
{
    public partial class Form4 : Form
    {
        List<Instrument> instruments = new List<Instrument>();
        List<vdPolyline> obstacles = new List<vdPolyline>();
        List<vdCircle> instrument = new List<vdCircle>();
        List<string> blkvdInsertCollection = new List<string>() { };
        gPoints gridPoints = new gPoints();
        vdPolyline boundary;
        vdPolyline offsetBoundary;
        Destination destination;
        double sx = 0;
        double sy = 0;

        public Form4()
        {
            InitializeComponent();
            // AddLayersItem();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //Setting s2 = new Setting();
            //s2.ShowDialog();
            //s2 = null;
            //this.Show();
        }


        //toolstrip 
        //How to import or open the file
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string fname;
            string DocPath;

            object ret = vdsc.BaseControl.ActiveDocument.GetOpenFileNameDlg(0, "", 0);
            if (ret == null) return;

            DocPath = ret as string;
            fname = (string)ret;

            bool success = vdsc.BaseControl.ActiveDocument.Open(fname);
            if (!success) return;

            foreach (vdLayer layer in vdsc.BaseControl.ActiveDocument.Layers)
            {
                dgvInstLayers.Rows.Add(layer.Name);
                cboObs.Items.Add(layer.Name);
                cboDesti.Items.Add(layer.Name);
            } 
            vdsc.BaseControl.ActiveDocument.Redraw(true);

            var usedBlk = vdsc.BaseControl.ActiveDocument.Blocks.GetNotDeletedItems();
            foreach(var ub in usedBlk)
            {
                if (ub is vdBlock vb)
                blkvdInsertCollection.Add((vb).Name.ToString().Replace("vdInsert ","").TrimStart());
            }
            Explode();
        }
        private void Explode()
        {
            SettingSource();
            RefreshUpdate();
            DrawPolyline();
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
            var prim = vdsc.BaseControl.ActiveDocument.Blocks.GetNotDeletedItems();
            foreach (var blk in prim)
            {
                if ((blk as vdBlock).Entities != null)
                {
                    (blk as vdBlock).Entities.RemoveAll();
                }
            }
            //this.Update();
            //this.UpdateBounds();
            //vdsc.BaseControl.ActiveDocument.Update();
            //vdsc.BaseControl.ActiveDocument.ActiveLayOut.ZoomAll();
            vdsc.BaseControl.ActiveDocument.Redraw(true);
            //vdFramedControl1.BaseControl.ActiveDocument.ActiveLayOut.ZoomExtents();

        }
        private void SettingSource()
        {
            dtEntities = SettingTable();
            int obsOccurence = 0;
            foreach (vdFigure f in vdsc.BaseControl.ActiveDocument.Model.Entities)
            {


                if (f != null)
                {
                    if (f.Layer.Name.ToLower().ToString().Contains("obstacle"))/// == cboObs.SelectedItem.ToString())
                    {
                        try
                        {

                            if (f is vdInsert vi && blkvdInsertCollection.Contains(f.ToString().Replace("vdInsert ", "")))
                            {

                                var ent = vi.Explode();
                                if (ent != null)
                                {
                                    ////********Check Only Lines that can make polygonal figures with continuous closed lines inside a certain block sample 
                                    ////********That means no matter what kinds of polygonal shaped lines, or double dash-like polygon and other kind of figure that comprised of vdLine.
                                    if (CheckAllOnlyLines(ent))
                                    {
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
                                        MessageBox.Show("One of the VdInsert of VdBlock should be comprised of VdLines");
                                        return;
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

            }
        }
        private bool CheckAllOnlyLines(vdEntities ve)
        {
            foreach(var lines in ve)
            {
                if (!(lines is vdLine))
                    return false;
            }
            return true;
        }
        private void DrawPolyline()
        {
            var obsLayer = vdsc.BaseControl.ActiveDocument.Layers.FindName("Obstacle");
            vdsc.BaseControl.ActiveDocument.SetActiveLayer(obsLayer);
            //vdFramedControl1.BaseControl.ActiveDocument.SetActiveLayer(new vdLayer()); // Make currentlayer Obstable assure
            int completeOnepolygon = 0;
            VectorDraw.Professional.vdFigures.vdPolyline onepoly = new VectorDraw.Professional.vdFigures.vdPolyline();
            foreach (DataRow dr in dtEntities.Rows)
            {
                completeOnepolygon++;
                if (completeOnepolygon <= 4)
                {
                    onepoly.VertexList.Add(dr["SP"] as gPoint);
                    onepoly.PenColor.ColorIndex = 7;

                }
                if (completeOnepolygon == 4)
                {
                    vdsc.BaseControl.ActiveDocument.SetActiveLayer(obsLayer);
                    onepoly.SetUnRegisterDocument(vdsc.BaseControl.ActiveDocument);
                    onepoly.setDocumentDefaults();
                    onepoly.Flag = VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE;
                    vdsc.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(onepoly);
                    vdsc.BaseControl.ActiveDocument.Redraw(true);
                    onepoly = new VectorDraw.Professional.vdFigures.vdPolyline();
                    if (completeOnepolygon == 4)
                        completeOnepolygon = 0;
                }

            }
        }
        public void GetEntities()
        {
            try
            {
                foreach (vdFigure f in vdsc.BaseControl.ActiveDocument.Model.Entities)
                {

                    if (f.Layer.Name == lstboxInstLayer.Items[0].ToString())
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        //Additional node condition
        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            //this.Hide();
            //Project pro = new Project();
            //pro.ShowDialog();
            //pro = null;
            //this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void vdScrollableControl1_Load(object sender, EventArgs e)
        {

        }

        public void makeGrid()
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

                int wGap = 2000;
                int hGap = 2000;
                int wc = (int)box.Width / wGap + 1;
                int hc = (int)box.Height / hGap + 1;
                int pCount = wc * hc;



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
                        int index = i * wc + j;
                        gPoint p = new gPoint(sx + j * wGap, sy + i * hGap);
                        gridPoints.Add(p);
                        bool isIn = contains(this.boundary.VertexList, p);
                        bool isIn2 = false;
                        foreach (vdPolyline obstacle in this.obstacles)
                        {
                            isIn2 = contains(obstacle.VertexList, p);
                            if (isIn2) break;
                        }

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



                /*
                for (int i = 0; i < pCount; i++)
                {
                    gPoint gp1 = gridPoints[i];
                    vdCircle c = new vdCircle(this.vdsc.BaseControl.ActiveDocument, gp1, 100);
                    vdsc.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(c);
                    c.SetUnRegisterDocument(vdsc.BaseControl.ActiveDocument);
                    c.setDocumentDefaults();
                }
                */

                for (int i = 0; i < pCount - 1; i++)
                {
                    gPoint gp1 = gridPoints[i];
                    for (int j = i + 1; j < pCount; j++)
                    {
                        gPoint gp2 = gridPoints[j];
                        /*
                        if (adjMatrix[i, j] < 100000000)
                        {
                            vdLine line = new vdLine(this.vdsc.BaseControl.ActiveDocument, gp1, gp2);
                            vdsc.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(line);
                            line.SetUnRegisterDocument(vdsc.BaseControl.ActiveDocument);
                            line.setDocumentDefaults();
                        }
                        */
                    }
                }


                double minDistance = double.MaxValue;
                int count = 0;
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


                foreach (Instrument ins in this.instruments)
                {
                    //   ins.distanceFromDestination = this.destination.center.Distance2D(ins.centerPoint);
                    //   Console.WriteLine("gridIndex   "+ins.gridIndex);
                    double[] result = Dijkstra.analysis(ins.gridIndex, destination.gridIndex, adjMatrix);
                    if (result != null) // it might seen an error  // ***PTK 
                    ins.distanceFromDestination = result[result.Length - 1];

                }

                this.instruments = this.instruments.OrderBy(x => x.distanceFromDestination).ToList();
                // this.instruments.Reverse();

                foreach (Instrument ins in this.instruments)
                {
                    double[] result = Dijkstra.analysis(ins.gridIndex, destination.gridIndex, adjMatrix);
                    gPoints ps = new gPoints();
                    try
                    {
                        for (int i = 0; i < result.Length - 1; i++)
                        {
                            gPoint gp = gridPoints[(int)result[i]];
                            ps.Add(gp);
                        }
                    }
                    catch { }
                    try
                    {
                        for (int i = 0; i < result.Length - 2; i++)
                        {
                            adjMatrix[(int)result[i], (int)result[i + 1]] = adjMatrix[(int)result[i], (int)result[i + 1]] * 0.1;
                            adjMatrix[(int)result[i + 1], (int)result[i]] = adjMatrix[(int)result[i + 1], (int)result[i]] * 0.1;
                        }
                    }
                    catch { }

                    vdPolyline line = new vdPolyline(this.vdsc.BaseControl.ActiveDocument, ps);
                    line.PenColor = new vdColor(Color.Yellow);
                    vdsc.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(line);
                    line.SetUnRegisterDocument(vdsc.BaseControl.ActiveDocument);
                    line.setDocumentDefaults();

                }
                this.vdsc.BaseControl.Redraw();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "Error at Grid Algorithm");
            }
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
                    }
                }
            }
            return isIn;
        }

        public static bool contains2(Vertexes _pts, gPoint pt)
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
                    ((_pts[i].y < pt.y) && (pt.y < _pts[j].y)) || ((_pts[j].y < pt.y) && (pt.y < _pts[i].y))
                    ) &&
                    (pt.x < (_pts[j].x - _pts[i].x) * (pt.y - _pts[i].y) / (_pts[j].y - _pts[i].y) + _pts[i].x)
                    )
                    {
                        isIn = !isIn;
                    }
                }
            }
            return isIn;
        }

        public static void setColorPath(vdPolyline polyline, Color color, VdConstLineWeight vdConstLineWeight)
        {
            polyline.PenColor = new VectorDraw.Professional.vdObjects.vdColor(color);
            polyline.LineWeight = vdConstLineWeight;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            GetEntities();
            makeGrid();
        }

        private void labelObst_Click(object sender, EventArgs e)
        {

        }

        private void labelDesti_Click(object sender, EventArgs e)
        {

        }

        private void dgvInstLayers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void butAddLayer_Click(object sender, EventArgs e)
        {
            var dgr = dgvInstLayers.CurrentRow;

            if (dgr == null)
                MessageBox.Show("Please select a layer to interact");

            if (lstboxInstLayer.Items.Count >= 1)
            {
                MessageBox.Show("Please add only one layer for the instrument. The system will be updated to add multiple layers.");
            }

            else
            {
                string newlyrIns = dgr.Cells[0].Value.ToString();
                if (!String.IsNullOrEmpty(newlyrIns))
                {
                    if (!lstboxInstLayer.Items.Contains(newlyrIns))
                    {
                        lstboxInstLayer.Items.Add(newlyrIns);
                        lstboxInstLayer.Update();
                    }
                }
            }
        }

        private void butRemoveLayer_Click(object sender, EventArgs e)
        {
            var lst = lstboxInstLayer.SelectedItem;

            if (lst == null)
                MessageBox.Show("Please select a layer to remove");
            else
            {
                lstboxInstLayer.Items.Remove(lst);
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            vdsc.BaseControl.ActiveDocument.New();
            this.Refresh();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        { 

            this.Hide();
            Project.ProjectInfo pro = new Project.ProjectInfo();
            pro.ShowDialog();
            pro = null;
            this.Show();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
          
         
        }
    }
}
