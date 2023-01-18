using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VFD1
{
    public partial class Form5 : Form
    {
        static DataTable DataSourceInstrumentType = Entity.staticCache.DataSourceInstrumentType();
       // public DataTable dtInstrument;
        public Form5(DataTable dt= null)
        {
            InitializeComponent();
            //dtInstrument = Form3.dtInstrument;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            if (Form4.dtInstrument != null)
            {
                dataGridView1.AutoGenerateColumns = false;
                
                InstrumentType.Name = "Name";
                InstrumentType.DataSource = DataSourceInstrumentType;
                InstrumentType.DisplayMember = "Name";
                InstrumentType.ValueMember = "Key";
            
                dataGridView1.DataSource = Form4.dtInstrument;
             
            }
        }
    }
}
