using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace VFD1.Project
{
    public partial class PI : Form
    {
        static string DataSource = Entity.staticCache.DataSource;
        public PI()
        {
            InitializeComponent(); 
        }

        private void PI_Load(object sender, EventArgs e)
        {
            SetSetting();
        }
        private void SetSetting()
        {
            if ( !File.Exists(DataSource))
            {
                MessageBox.Show("Please make and configure a setting to initialize dbsource file having path " + DataSource +"." + Environment.NewLine + "Source File have been put at Project's Datasource Folder.");
                return;
            }
            using (XLWorkbook workBook = new XLWorkbook(DataSource))
            {
                var comboList = workBook.Worksheet(1);
                DataTable dtNation = new DataTable(); dtNation.Columns.Add("ID"); dtNation.Columns.Add("Value");
                DataTable dtSite = new DataTable(); dtSite.Columns.Add("ID"); dtSite.Columns.Add("Value");
                DataTable dtLine = new DataTable(); dtLine.Columns.Add("ID"); dtLine.Columns.Add("Value");
                DataTable dtPhase = new DataTable(); dtPhase.Columns.Add("ID"); dtPhase.Columns.Add("Value");
                DataTable dtScope = new DataTable(); dtScope.Columns.Add("ID"); dtScope.Columns.Add("Value");
                DataTable dtConPhase = new DataTable(); dtConPhase.Columns.Add("ID"); dtConPhase.Columns.Add("Value");
                DataTable dtDeli1 = new DataTable(); dtDeli1.Columns.Add("ID"); dtDeli1.Columns.Add("Value");
                DataTable dtDeli2 = new DataTable(); dtDeli2.Columns.Add("ID"); dtDeli2.Columns.Add("Value");
                //Loop through the Worksheet rows.

                foreach (IXLColumn col in comboList.Columns())
                {
                    bool firstRow = true;
                    int i = 0;
                    foreach (IXLCell cell in col.Cells())
                    {
                        if (!firstRow)
                        {
                            i++;//Nation	Site	Line	Phase	Scope	ConstuctionPhase	Deliverable1	Deliverable2

                            if (col.FirstCell().Value.ToString() == "Nation" && !string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                                dtNation.Rows.Add(new object[] { i.ToString(), cell.Value.ToString() });
                            else if (col.FirstCell().Value.ToString() == "Site" && !string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                                dtSite.Rows.Add(new object[] { i.ToString(), cell.Value.ToString() });
                            else if (col.FirstCell().Value.ToString() == "Line" && !string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                                dtLine.Rows.Add(new object[] { i.ToString(), cell.Value.ToString() });
                            else if (col.FirstCell().Value.ToString() == "Phase" && !string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                                dtPhase.Rows.Add(new object[] { i.ToString(), cell.Value.ToString() });
                            else if (col.FirstCell().Value.ToString() == "Scope" && !string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                                dtScope.Rows.Add(new object[] { i.ToString(), cell.Value.ToString() });
                            else if (col.FirstCell().Value.ToString() == "ConstuctionPhase" && !string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                                dtConPhase.Rows.Add(new object[] { i.ToString(), cell.Value.ToString() });
                            else if (col.FirstCell().Value.ToString() == "Deliverable1" && !string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                                dtDeli1.Rows.Add(new object[] { i.ToString(), cell.Value.ToString() });
                            else if (col.FirstCell().Value.ToString() == "Deliverable2" && !string.IsNullOrEmpty(cell.Value.ToString().Trim()))
                                dtDeli2.Rows.Add(new object[] { i.ToString(), cell.Value.ToString() });
                        }
                        firstRow = false;
                    }


                }
                cboNation.ValueMember = "ID";
                cboNation.DisplayMember = "Value";
                cboNation.DataSource = dtNation;

                cboSite.ValueMember = "ID";
                cboSite.DisplayMember = "Value";
                cboSite.DataSource = dtSite;

                cboLine.ValueMember = "ID";
                cboLine.DisplayMember = "Value";
                cboLine.DataSource = dtLine;

                cboPhrase.ValueMember = "ID";
                cboPhrase.DisplayMember = "Value";
                cboPhrase.DataSource = dtPhase;

                cboScope.ValueMember = "ID";
                cboScope.DisplayMember = "Value";
                cboScope.DataSource = dtScope;

                cboConPhrase.ValueMember = "ID";
                cboConPhrase.DisplayMember = "Value";
                cboConPhrase.DataSource = dtConPhase;

                cboDeli1.ValueMember = "ID";
                cboDeli1.DisplayMember = "Value";
                cboDeli1.DataSource = dtDeli1;

                cboDeli2.ValueMember = "ID";
                cboDeli2.DisplayMember = "Value";
                cboDeli2.DataSource = dtDeli2;
            }
        }
        private bool ErrorCheck()
        {
            var ctrl = GetAllControls(this);

            foreach (var ctrlTxt in ctrl)
            {
                if (ctrlTxt is TextBox)
                {
                    if (string.IsNullOrEmpty((ctrlTxt as TextBox).Text.TrimEnd()))
                    {
                        ctrlTxt.Focus();
                        return true;
                    }
                }
                if (ctrlTxt is ComboBox)
                {
                    if (string.IsNullOrEmpty((ctrlTxt as ComboBox).Text.TrimEnd()))
                    {
                        ctrlTxt.Focus();
                        return true;
                    }
                }
                //ctrlTxt.isClosing = true;
            }
            return false;
        }
        public IEnumerable<Control> GetAllControls(Control root)
        {
            foreach (Control control in root.Controls)
            {
                foreach (Control child in GetAllControls(control))
                {
                    yield return child;
                }
            }
            yield return root;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ErrorCheck())
            {
                MessageBox.Show("Fill in the blank.");
                return;
            }
            Entity.ProjectInfoEntity pet = new Entity.ProjectInfoEntity()
            {
                Nation = cboNation.Text,
                Site = cboSite.Text,
                ProjectTitle = txtProTitle.Text,
                ProjectSubTitle = txtProSubTitle.Text,
                Line = cboLine.Text,
                Phase = cboPhrase.Text,
                Scope = cboScope.Text,
                ConstrutionPhase = cboConPhrase.Text,
                RevNo = txtRevNo.Text,
                Deliverable1 = cboDeli1.Text,
                Deliverable2 = cboDeli2.Text,
                Assign1 = txtAssign1.Text,
                Assign2 = txtAssign2.Text,
                Password = txtPwd.Text

            };
            try
            {
                using (XLWorkbook workBook = new XLWorkbook(DataSource))
                {
                    var pjlist = workBook.Worksheet(2);// PjLst 
                    var lastRow = pjlist.RowsUsed().Count() + 1;
                    pjlist.Cell(lastRow, 1).Value = lastRow - 1;
                    pjlist.Cell(lastRow, 2).Value = pet.Nation;
                    pjlist.Cell(lastRow, 3).Value = pet.Site;
                    pjlist.Cell(lastRow, 4).Value = pet.ProjectTitle;
                    pjlist.Cell(lastRow, 5).Value = pet.ProjectSubTitle;
                    pjlist.Cell(lastRow, 6).Value = pet.Line;
                    pjlist.Cell(lastRow, 7).Value = pet.Phase;
                    pjlist.Cell(lastRow, 8).Value = pet.Scope;
                    pjlist.Cell(lastRow, 9).Value = pet.ConstrutionPhase;
                    pjlist.Cell(lastRow, 10).Value = pet.RevNo;
                    pjlist.Cell(lastRow, 11).Value = pet.Deliverable1;
                    pjlist.Cell(lastRow, 12).Value = pet.Deliverable2;
                    pjlist.Cell(lastRow, 13).Value = pet.Assign1;
                    pjlist.Cell(lastRow, 14).Value = pet.Assign2;
                    pjlist.Cell(lastRow, 15).Value = pet.Password;

                    workBook.Save();
                    MessageBox.Show("Project information was saved successfully!");
                    ResetValues();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ResetValues()
        {
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is TextBox)
                    (ctrl as TextBox).Text = ""; 
            }
        }
    }
}
