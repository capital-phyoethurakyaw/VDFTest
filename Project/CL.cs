using ClosedXML.Excel;
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
        static string DataSource = Entity.staticCache.DataSource;

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
            if (!File.Exists(DataSource))
            {
                MessageBox.Show("Please make and configure a setting to initialize dbsource file having path " + DataSource + "." + Environment.NewLine + "Source File have been put at Project's Datasource Folder.");
                return;
            }
            dtSource = new DataTable();
            dtSource.Columns.Add("colClassification1");
            dtSource.Columns.Add("colClassification2");
            dtSource.Columns.Add("colClassification3");
            using (XLWorkbook workBook = new XLWorkbook(DataSource))
            {
                var insList = workBook.Worksheet(4);// inslst 
                bool firstRow = true;
                foreach (IXLRow row in insList.Rows())
                {
                    if (!firstRow)
                    {
                        var val1 = row.Cell(1).Value.ToString();
                        var val2 = row.Cell(2).Value.ToString();
                        var val3 = row.Cell(3).Value.ToString();
                        if (!string.IsNullOrEmpty(val1.TrimEnd()) || !string.IsNullOrEmpty(val2.TrimEnd()) || !string.IsNullOrEmpty(val3.TrimEnd()))
                            dtSource.Rows.Add(new object[] { val1, val2, val3 });
                    }
                    firstRow = false;
                }
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
            using (XLWorkbook workBook = new XLWorkbook(DataSource))
            {
                var insList = workBook.Worksheet(4);// inslst  

                //DeleteAllFirst
                bool First = true;
                foreach (IXLRow row in insList.Rows())
                {
                    if (!First)
                        row.Delete();
                    First = false;
                }

                int i = 0;
                foreach (DataRow dr in dtSource.Rows)
                {
                    i++;
                    insList.Cell(i + 1, 1).Value = dr["colClassification1"].ToString();
                    insList.Cell(i + 1, 2).Value = dr["colClassification2"].ToString();
                    insList.Cell(i + 1, 3).Value = dr["colClassification3"].ToString();
                }
                workBook.Save();
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
}
