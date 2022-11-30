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
    public partial class Form2 : Form
    {
        DataTable dtins = new DataTable();
        public Form2(DataTable dt= null)
        {
            InitializeComponent();
            dtins = dt;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dtins;
            
        }
    }
}
