//using ClosedXML.Excel;
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

namespace VFD1.Project
{
    public partial class CDL : Form
    {
        static string DataSourceCableDuctList = Entity.staticCache.DataSourceCableDuctList;

        public CDL()
        {
            InitializeComponent();
            this.Load += IS_Load;
        }
        DataTable dtSource;
        private void IS_Load(object sender, EventArgs e)
        {
            BindGrid();
        }
        private void BindGrid()
        {
            if (!File.Exists(DataSourceCableDuctList))
            {
                MessageBox.Show("Please make and configure a setting to initialize dbsource file having path " + DataSourceCableDuctList + "." + Environment.NewLine + "Source File have been put at Project's Datasource Folder.");
                return;
            }
            dtSource = new DataTable();
            dtSource.Columns.Add("colClassification1");
            dtSource.Columns.Add("colClassification2");
            dtSource.Columns.Add("colClassification3");
            dtSource.Columns.Add("colClassification4");
            List<CableDuctList> result;
            using (TextReader fileReader = File.OpenText(DataSourceCableDuctList))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = false;
                csv.Read();
                result = csv.GetRecords<CableDuctList>().ToList(); 
            } 
            foreach (CableDuctList cbe in result)
            {
                dtSource.Rows.Add(new object[] { cbe.Category.Trim(), cbe.Type.Trim(), cbe.Width.Trim(), cbe.Height.Trim() });
            } 
            dataGridView1.DataSource = dtSource; 
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Update();
                SaveChanges();

                MessageBox.Show("Your changes have been successfully saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SaveChanges()
        { 
            File.Delete(DataSourceCableDuctList);
            using (var writer = new StreamWriter(DataSourceCableDuctList))
            using (var csvWriter = new CsvWriter(writer))
            { 
                List<CableDuctList> lst = new List<CableDuctList>();
                CableDuctList ie = new CableDuctList();

                foreach (DataRow dr in dtSource.Rows)
                {
                    ie = new CableDuctList();
                    ie.Category = dr["colClassification1"].ToString().Trim();
                    ie.Type = dr["colClassification2"].ToString().Trim();
                    ie.Width = dr["colClassification3"].ToString().Trim();
                    ie.Height = dr["colClassification3"].ToString().Trim();
                    lst.Add(ie); 
                }
                csvWriter.WriteRecords(lst);
                csvWriter.Flush();
                writer.Flush();
                writer.Close();
            }
            BindGrid();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Update();

                if (dataGridView1.SelectionMode == System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect && dataGridView1.SelectedCells.Count == 4)
                {
                    DeleteSelectedRow();
                    MessageBox.Show("Deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DeleteSelectedRow()
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove(row);
                dataGridView1.Update();
            }

            SaveChanges();


        }
    }
    public class CableDuctList
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        //Category	Type	Width	Height

    }
}
