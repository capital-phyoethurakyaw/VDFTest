//using ClosedXML.Excel;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VFD1.Project
{
    public partial class IS : Form
    {
        static string DataSourceInstrumentList = Entity.staticCache.DataSourceInstrumentList;

        public IS()
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
            if (!File.Exists(DataSourceInstrumentList))
            {
                MessageBox.Show("Please make and configure a setting to initialize dbsource file having path " + DataSourceInstrumentList + "." + Environment.NewLine + "Source File have been put at Project's Datasource Folder.");
                return;
            }
            dtSource = new DataTable();
            dtSource.Columns.Add("colClassification1");
            dtSource.Columns.Add("colClassification2");
            dtSource.Columns.Add("colClassification3"); 
            List<InstrumentList> result;
            using (TextReader fileReader = File.OpenText(DataSourceInstrumentList))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = false;
                csv.Read();
                result = csv.GetRecords<InstrumentList>().ToList();
               
            }
            int i = 0;
            foreach (InstrumentList cbe in result)
            { 
                    dtSource.Rows.Add(new object[] { cbe.Classification_1.Trim(), cbe.Classification_2.Trim(), cbe.Classification_3.Trim() });
           
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
            File.Delete(DataSourceInstrumentList);
            using (var writer = new StreamWriter(DataSourceInstrumentList))
            using (var csvWriter = new CsvWriter(writer))
            {
                List<InstrumentList> lst = new List<InstrumentList>();
                InstrumentList ie = new InstrumentList();
                 
                foreach (DataRow dr in dtSource.Rows)
                {
                    ie = new InstrumentList();
                    ie.Classification_1 = dr["colClassification1"].ToString().Trim();
                    ie.Classification_2 = dr["colClassification2"].ToString().Trim();
                    ie.Classification_3 = dr["colClassification3"].ToString().Trim();
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
                if (dataGridView1.SelectionMode == System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect && dataGridView1.SelectedCells.Count ==3)

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
            if (dataGridView1.SelectedRows.Count > 0 )
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.Remove(row);
                    dataGridView1.Update();
                }
            }

            SaveChanges();


        }
    }
    public class InstrumentList
    {
        public string Classification_1 { get; set; }
        public string Classification_2 { get; set; }
        public string Classification_3 { get; set; }

    }
}
