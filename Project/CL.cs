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
    public partial class CL : Form
    {
        static string DataSourceCableList = Entity.staticCache.DataSourceCableList;

        public CL()
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
            if (!File.Exists(DataSourceCableList))
            {
                MessageBox.Show("Please make and configure a setting to initialize dbsource file having path " + DataSourceCableList + "." + Environment.NewLine + "Source File have been put at Project's Datasource Folder.");
                return;
            }
            dtSource = new DataTable();
            dtSource.Columns.Add("colClassification1");
            dtSource.Columns.Add("colClassification2");
            dtSource.Columns.Add("colClassification3");
        
            List<CableList> result;
            using (TextReader fileReader = File.OpenText(DataSourceCableList))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = false;
                csv.Read();
                result = csv.GetRecords<CableList>().ToList();

            }
            int i = 0;
            foreach (CableList cbe in result)
            {
           
                    dtSource.Rows.Add(new object[] { cbe.명칭.Trim(), cbe.분류.Trim(), cbe.외경.Trim() });
               
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
            File.Delete(DataSourceCableList);
            using (var writer = new StreamWriter(DataSourceCableList))
            using (var csvWriter = new CsvWriter(writer))
            { 
                List<CableList> lst = new List<CableList>();
                CableList ie = new CableList();

                foreach (DataRow dr in dtSource.Rows)
                {
                    ie = new CableList();
                    ie.명칭 = dr["colClassification1"].ToString().Trim();
                    ie.분류 = dr["colClassification2"].ToString().Trim();
                    ie.외경 = dr["colClassification3"].ToString().Trim();
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

                if (dataGridView1.SelectionMode == System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect && dataGridView1.SelectedCells.Count == 3)
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
    public class CableList
    {
        public string 명칭 { get; set; }
        public string 분류 { get; set; }
        public string 외경 { get; set; }

    }
}
