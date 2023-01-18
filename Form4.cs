
using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        static string DataSourceCableDuctSchedule = Entity.staticCache.DataSourceCableDuctSchedule;

        List<Instrument> instruments = new List<Instrument>();
        List<vdPolyline> obstacles = new List<vdPolyline>();
        List<vdCircle> instrument = new List<vdCircle>();
        List<string> blkvdInsertCollection = new List<string>() { };
        gPoints gridPoints = new gPoints();
        vdPolyline boundary;
        vdPolyline offsetBoundary;
        Destination destination;
        private List<PointF> Seeds = new List<PointF>();
        private List<PointF> Centroids = new List<PointF>();
        private List<PointData> Points = new List<PointData>();
        private gPoint OCentriod;
        double sx = 0;
        double sy = 0;
        private int counter = 0;
        public static DataTable dtInstrument;
        private static DataTable dtSchedule;
        private static DataTable dtSegmentation;
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

        string DocPath;
        //toolstrip 
        //How to import or open the file
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string fname;
            

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
        private void RefreshUpdateDestination()
        {
            foreach (vdFigure f in vdsc.BaseControl.ActiveDocument.Model.Entities)
            {
                //
                if (f != null && f.Layer.Name.ToLower().ToString().Contains("destination") && f is vdPolyline vi)
                { 
                    vi.Explode().RemoveAll();
                    vi.Explode().EraseAll();
                    vi.Deleted = true;
                    vi.Explode().RemoveAll(); 
                }
            }
            foreach (vdFigure f in vdsc.BaseControl.ActiveDocument.Model.Entities)
            {
                if (f != null && f.Layer.Name.ToLower().ToString().Contains("obstacle") && f is vdPolyline vi && vi.Flag != VectorDraw.Professional.Constants.VdConstPlineFlag.PlFlagCLOSE)
                {
                    vi.Explode().RemoveAll();
                    vi.Explode().EraseAll();
                    vi.Deleted = true;
                    vi.Explode().RemoveAll();
                }
            }
            vdsc.BaseControl.Redraw();
            vdsc.BaseControl.ActiveDocument.Redraw(true);
            vdsc.BaseControl.ReFresh();
        }
        private void Initialize()
        {

            instruments = new List<Instrument>();
            obstacles = new List<vdPolyline>();
            instrument = new List<vdCircle>();
            blkvdInsertCollection = new List<string>() { };
            gridPoints = new gPoints();
              boundary = new vdPolyline();
              offsetBoundary= new vdPolyline();
              destination= null;
        }
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
            vdsc.BaseControl.ActiveDocument.Redraw(true); 

        }
        private void SettingSourceDestination()
        {
            dtEntities = SettingTable();
            int obsOccurence = 0;
            foreach (vdFigure f in vdsc.BaseControl.ActiveDocument.Model.Entities)
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
        private void DrawPolylineDestination()
        {
            if (OCentriod == null || Centroids.Count == 0)
            {
                return;
            }
            var dest = vdsc.BaseControl.ActiveDocument.Layers.FindName("Destination");
            vdsc.BaseControl.ActiveDocument.SetActiveLayer(dest);
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
                    vdsc.BaseControl.ActiveDocument.SetActiveLayer(dest);
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
            // RefreshUpdate();
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

        public void makeGrid(bool IsAlternative =false)
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

                int wGap = (1000 * 2) * int.Parse(txtHTolerance.Text);
                int hGap = (1000 * 2) * int.Parse(txtHTolerance.Text);
                int wc = (int)box.Width / (wGap) + 1;
                int hc = (int)box.Height / (hGap) + 1;
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
                    //var ids = line.Id;

                    if (IsAlternative)
                    {
                        //Texting / Dimensioning
                        var ctxt = counterText.ToString();
                        TextInsert(ins.centerPoint.x, ins.centerPoint.y, ("Instrument_" + counterText.ToString()));//analyzer
                        if (Convert.ToInt32(ins.distanceFromDestination) == Convert.ToInt32(this.instruments.Max(x => x.distanceFromDestination).ToString().Contains("E")? "10000000000" : this.instruments.Max(x => x.distanceFromDestination).ToString()))
                            lblMaxLengthInstrument.Text = "Instrument_" + counterText.ToString();
                        dtInstrument.Rows.Add(new object[] {
                 line.Id , counterText , ("Instrument_" + counterText.ToString() ),  Convert.ToInt32(ins.distanceFromDestination/1000).ToString(), "0_121"
                });
                    }

                }
                this.vdsc.BaseControl.Redraw();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "Error at Grid Algorithm. " + Environment.NewLine + "Close and Try with Relocate action.");
            }
        }
        private void TextInsert(double x, double y, string txt,bool isSegment= false)
        {
            VectorDraw.Professional.vdFigures.vdText text = new VectorDraw.Professional.vdFigures.vdText();
            text.SetUnRegisterDocument(vdsc.BaseControl.ActiveDocument);
            text.setDocumentDefaults();
            text.InsertionPoint = new VectorDraw.Geometry.gPoint(x - 400, y - 400);
            text.TextString = txt;
            text.Height = 450.0;
            if (isSegment)
                text.PenColor = new vdColor(Color.Yellow);
                else
            text.PenColor = new vdColor(Color.Red);
            vdsc.BaseControl.ActiveDocument.ActiveLayOut.Entities.AddItem(text);
            vdsc.BaseControl.ActiveDocument.Redraw(true);
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
            vdsc.BaseControl.ActiveDocument.Redraw(true);
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            GetEntities();
            //Kmean
            //Preparepoint();
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
        private double Distance(PointF point1, PointF point2)
        {
            float dx = point1.X - point2.X;
            float dy = point1.Y - point2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
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
        private void button3_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(DocPath))
                return;
            Initialize();
            GetEntities(); //
            //Kmean
            Preparepoint();//
            RefreshDoc();//
            UpdateSolution();//
            SettingSourceDestination();//
            RefreshUpdateDestination();//
            DrawPolylineDestination();
            RefreshDoc();//
            //Dijkstra
            //GetEntities();
            makeGrid(true);//
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form5 pro = new Form5();
            pro.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Segmentaion();
        }
        private void Segmentaion()
        {
            if (dtInstrument == null)
                return;
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
                    var ent = vdsc.BaseControl.ActiveDocument.Model.Entities.FindFromId(id).Explode();
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
                dr["OptimalDuctSize"] = Convert.ToInt32((totalAreas / (Convert.ToDouble(txtDuctOptimize.Text) / 100)).ToString()); // y= 0.2 x
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
                drs["Length"] = Convert.ToInt32(lengthSegmt / 1000);
                drs["SG"] = "Segment_" + drs["SG"].ToString();
            }
            dtOptimalResult = dt_segment.AsEnumerable().GroupBy(r => new { Col1 = r["SG"] }).Select(g => g.OrderBy(r => r["SG"]).First()).CopyToDataTable();
            foreach (DataRow dr in dtOptimalResult.Rows)
            {
                TextInsert(Gp(dr["SP"].ToString()).x, Gp(dr["EP"].ToString()).y, dr["SG"].ToString(), true);
            }

            Form6 pro = new Form6();
            pro.ShowDialog();
        }
        public static DataTable dtOptimalResult;
        private gPoint Gp(string pts)
        {
            return new gPoint(Convert.ToDouble(pts.Split(',')[0].ToString()), Convert.ToDouble(pts.Split(',')[1].ToString()), Convert.ToDouble(pts.Split(',')[2].ToString()));
        }
        private string GetFileName()
        {
            return "CableSchedule_" + DateTime.Now.ToString("yyyymmddHHmmss").Replace(" ", "").Replace(":", "").Replace("-", "").Replace("/", "") + ".csv";
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
            var set = vdsc.BaseControl.ActiveDocument.Selections;
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

            if (dtSchedule.Rows.Count == 0)
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
    }
}
