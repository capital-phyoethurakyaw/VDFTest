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
        /// <summary>
        /// Initilize component.
        /// </summary>
        public Form2(DataTable dt= null)
        {
            InitializeComponent();
            dtins = dt;
        }
        /// <summary>
        /// Make data binding for 4 routed paths at form load
        /// </summary>
        private void Form2_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dtins;
            
        }
    }
}
