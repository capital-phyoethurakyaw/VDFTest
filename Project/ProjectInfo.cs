using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VFD1.Project
{
    public partial class ProjectInfo : Form
    {
        public ProjectInfo()
        {
            InitializeComponent();
        }

        private void ProjectInfo_Load(object sender, EventArgs e)
        {
            loadform(new PI());
        }

        private void btproject_Click(object sender, EventArgs e)
        {
            loadform(new PI());
        }

        public void loadform(object Form)
        {
            if (this.mainpanel1.Controls.Count > 0)
                this.mainpanel1.Controls.RemoveAt(0);
            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.mainpanel1.Controls.Add(f);
            this.mainpanel1.Tag = f;
            f.Show();
        }

        private void btinst_Click(object sender, EventArgs e)
        {
            loadform(new IS());
        }

        private void btc_Click(object sender, EventArgs e)
        {
            loadform(new CL());
        }

        private void btcd_Click(object sender, EventArgs e)
        {
            loadform(new CDL());
        }
    }
}
