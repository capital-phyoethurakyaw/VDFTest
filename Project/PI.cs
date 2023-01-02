//using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;

namespace VFD1.Project
{
    public partial class PI : Form
    {
        static string DataSourceComboSetting = Entity.staticCache.DataSourceComboSetting;
        static string DataSourceProjectList = Entity.staticCache.DataSourceProjectList;
        public PI()
        {
            InitializeComponent(); 
        }

        private void PI_Load(object sender, EventArgs e)
        {
            SetSetting();
        }
                DataTable dtNation = new DataTable();   
                DataTable dtSite    = new DataTable();  
                DataTable dtLine    = new DataTable();  
                DataTable dtPhase    = new DataTable(); 
                DataTable dtScope   = new DataTable();  
                DataTable dtConPhase = new DataTable(); 
                DataTable dtDeli1 = new DataTable();    
                DataTable dtDeli2 = new DataTable();
        private void SetSetting()
        {
            if (!File.Exists(DataSourceComboSetting) || !File.Exists(DataSourceProjectList))
            {
                MessageBox.Show("Please make and configure a setting to initialize dbsource file having path " + DataSourceComboSetting + " & " + DataSourceProjectList + "." + Environment.NewLine + "Source File have been put at Project's Datasource Folder.");
                return;
            }
            dtNation.Columns.Add("ID"); dtNation.Columns.Add("Value");
            dtSite.Columns.Add("ID"); dtSite.Columns.Add("Value");
            dtLine.Columns.Add("ID"); dtLine.Columns.Add("Value");
            dtPhase.Columns.Add("ID"); dtPhase.Columns.Add("Value");
            dtScope.Columns.Add("ID"); dtScope.Columns.Add("Value");
            dtConPhase.Columns.Add("ID"); dtConPhase.Columns.Add("Value");
            dtDeli1.Columns.Add("ID"); dtDeli1.Columns.Add("Value");
            dtDeli2.Columns.Add("ID"); dtDeli2.Columns.Add("Value");


            MakeSettingCombo();
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

        private void MakeSettingCombo()
        { 
            List<ComboSetting> result; 
            using (TextReader fileReader = File.OpenText(DataSourceComboSetting))
            {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = false;
                csv.Read();
                result = csv.GetRecords<ComboSetting>().ToList();
            }
            int i = 0;
            foreach (ComboSetting cbe in result)
            {
                i++;
                //if (i != 0)
                //{
                if (!String.IsNullOrEmpty(cbe.Nation.Trim()))
                    {
                        
                        dtNation.Rows.Add(new object[] { i.ToString(), cbe.Nation.Trim() });
                    }
                    if (!String.IsNullOrEmpty(cbe.Nation.Trim()))
                    {
                        
                        dtSite.Rows.Add(new object[] { i.ToString(), cbe.Site.Trim() });
                    }
                    if (!String.IsNullOrEmpty(cbe.Line.Trim()))
                    {
                        
                        dtLine.Rows.Add(new object[] { i.ToString(), cbe.Line.Trim() });
                    }
                    if (!String.IsNullOrEmpty(cbe.Phase.Trim()))
                    {
                        
                        dtPhase.Rows.Add(new object[] { i.ToString(), cbe.Phase.Trim() });
                    }
                    if (!String.IsNullOrEmpty(cbe.Scope.Trim()))
                    {
                        
                        dtScope.Rows.Add(new object[] { i.ToString(), cbe.Scope.Trim() });
                    }
                    if (!String.IsNullOrEmpty(cbe.ConstuctionPhase.Trim()))
                    {
                        
                        dtConPhase.Rows.Add(new object[] { i.ToString(), cbe.ConstuctionPhase.Trim() });
                    }
                    if (!String.IsNullOrEmpty(cbe.Deliverable1.Trim()))
                    {
                        
                        dtDeli1.Rows.Add(new object[] { i.ToString(), cbe.Deliverable1.Trim() });
                    }
                    if (!String.IsNullOrEmpty(cbe.Deliverable2.Trim()))
                    {
                        
                        dtDeli2.Rows.Add(new object[] { i.ToString(), cbe.Deliverable2.Trim() });
                    }
                //}
               

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

                List<Entity.ProjectInfoEntity> result;
                using (TextReader fileReader = File.OpenText(DataSourceProjectList))
                {
                    var csv = new CsvReader(fileReader);
                    csv.Configuration.HasHeaderRecord = false;
                    csv.Read();
                    result = csv.GetRecords<Entity.ProjectInfoEntity>().ToList();
                }
                File.Delete(DataSourceProjectList);
                var fs = new StreamWriter(DataSourceProjectList);
                var csvWriter = new CsvWriter(fs);  
                result.Add(pet);

                csvWriter.WriteRecords( result); 
                csvWriter.Flush();
                fs.Flush();
                fs.Close();
                MessageBox.Show("Project information was saved successfully!");
                ResetValues();
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
    public class ComboSetting
    {
        //Nation	Site	Line	Phase	Scope	ConstuctionPhase	Deliverable1	Deliverable2

        public string Nation { get; set; }
        public string Site { get; set; }
        public string Line { get; set; }
        public string Phase { get; set; }
        public string Scope { get; set; }
        public string ConstuctionPhase { get; set; }
        public string Deliverable1 { get; set; }
        public string Deliverable2 { get; set; }
    }
}
