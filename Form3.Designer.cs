namespace VFD1
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.vdFramedControl1 = new vdControls.vdFramedControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnImport = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRouting = new System.Windows.Forms.Button();
            this.btnDimension = new System.Windows.Forms.Button();
            this.dgvAllLayerInstruments = new System.Windows.Forms.DataGridView();
            this.collayer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lstInstrument = new System.Windows.Forms.ListBox();
            this.btnIncre = new System.Windows.Forms.Button();
            this.btnDecre = new System.Windows.Forms.Button();
            this.cboDesti = new System.Windows.Forms.ComboBox();
            this.cboObs = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtHTolerance = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMaxLength = new System.Windows.Forms.TextBox();
            this.txtDuctOptimize = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtVTolerenace = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblMaxLengthInstrument = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.lst_circleBlk = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllLayerInstruments)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // vdFramedControl1
            // 
            this.vdFramedControl1.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.vdFramedControl1.DisplayPolarCoord = false;
            this.vdFramedControl1.HistoryLines = ((uint)(3u));
            this.vdFramedControl1.LoadCommandstxt = true;
            this.vdFramedControl1.LoadMenutxt = true;
            this.vdFramedControl1.Location = new System.Drawing.Point(12, 12);
            this.vdFramedControl1.Name = "vdFramedControl1";
            this.vdFramedControl1.PropertyGridWidth = ((uint)(300u));
            this.vdFramedControl1.Size = new System.Drawing.Size(1114, 548);
            this.vdFramedControl1.TabIndex = 44;
            this.vdFramedControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.vdFramedControl1_MouseDown);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btnImport, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1142, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(322, 37);
            this.tableLayoutPanel1.TabIndex = 45;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(3, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(101, 31);
            this.btnImport.TabIndex = 48;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(110, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 31);
            this.button2.TabIndex = 49;
            this.button2.Text = "Project";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.ItemHeight = 13;
            this.comboBox1.Location = new System.Drawing.Point(3, 30);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(208, 21);
            this.comboBox1.TabIndex = 46;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(217, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Input Layer";
            // 
            // btnRouting
            // 
            this.btnRouting.Location = new System.Drawing.Point(3, 71);
            this.btnRouting.Name = "btnRouting";
            this.btnRouting.Size = new System.Drawing.Size(73, 34);
            this.btnRouting.TabIndex = 51;
            this.btnRouting.Text = "Routing";
            this.btnRouting.UseVisualStyleBackColor = true;
            this.btnRouting.Click += new System.EventHandler(this.btnRouting_Click);
            // 
            // btnDimension
            // 
            this.btnDimension.Location = new System.Drawing.Point(82, 71);
            this.btnDimension.Name = "btnDimension";
            this.btnDimension.Size = new System.Drawing.Size(71, 34);
            this.btnDimension.TabIndex = 52;
            this.btnDimension.Text = "Dimension";
            this.btnDimension.UseVisualStyleBackColor = true;
            this.btnDimension.Click += new System.EventHandler(this.btnDimension_Click);
            // 
            // dgvAllLayerInstruments
            // 
            this.dgvAllLayerInstruments.AllowUserToDeleteRows = false;
            this.dgvAllLayerInstruments.AllowUserToResizeColumns = false;
            this.dgvAllLayerInstruments.AllowUserToResizeRows = false;
            this.dgvAllLayerInstruments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllLayerInstruments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.collayer});
            this.dgvAllLayerInstruments.Location = new System.Drawing.Point(3, 111);
            this.dgvAllLayerInstruments.Name = "dgvAllLayerInstruments";
            this.dgvAllLayerInstruments.Size = new System.Drawing.Size(316, 169);
            this.dgvAllLayerInstruments.TabIndex = 53;
            // 
            // collayer
            // 
            this.collayer.DataPropertyName = "colLayer";
            this.collayer.HeaderText = "Layer";
            this.collayer.Name = "collayer";
            this.collayer.ReadOnly = true;
            this.collayer.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.collayer.Width = 300;
            // 
            // lstInstrument
            // 
            this.lstInstrument.FormattingEnabled = true;
            this.lstInstrument.Location = new System.Drawing.Point(0, 347);
            this.lstInstrument.Name = "lstInstrument";
            this.lstInstrument.Size = new System.Drawing.Size(319, 69);
            this.lstInstrument.TabIndex = 54;
            this.lstInstrument.SelectedIndexChanged += new System.EventHandler(this.lstInstrument_SelectedIndexChanged);
            // 
            // btnIncre
            // 
            this.btnIncre.BackColor = System.Drawing.Color.Transparent;
            this.btnIncre.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIncre.ForeColor = System.Drawing.Color.Red;
            this.btnIncre.Location = new System.Drawing.Point(2, 290);
            this.btnIncre.Name = "btnIncre";
            this.btnIncre.Size = new System.Drawing.Size(34, 34);
            this.btnIncre.TabIndex = 55;
            this.btnIncre.Text = "+";
            this.btnIncre.UseVisualStyleBackColor = false;
            this.btnIncre.Click += new System.EventHandler(this.btnIncre_Click);
            // 
            // btnDecre
            // 
            this.btnDecre.BackColor = System.Drawing.Color.Transparent;
            this.btnDecre.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDecre.ForeColor = System.Drawing.Color.Red;
            this.btnDecre.Location = new System.Drawing.Point(42, 290);
            this.btnDecre.Name = "btnDecre";
            this.btnDecre.Size = new System.Drawing.Size(34, 34);
            this.btnDecre.TabIndex = 56;
            this.btnDecre.Text = "-";
            this.btnDecre.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDecre.UseVisualStyleBackColor = false;
            this.btnDecre.Click += new System.EventHandler(this.btnDecre_Click);
            // 
            // cboDesti
            // 
            this.cboDesti.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDesti.FormattingEnabled = true;
            this.cboDesti.ItemHeight = 13;
            this.cboDesti.Location = new System.Drawing.Point(108, 431);
            this.cboDesti.Name = "cboDesti";
            this.cboDesti.Size = new System.Drawing.Size(211, 21);
            this.cboDesti.TabIndex = 59;
            // 
            // cboObs
            // 
            this.cboObs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboObs.FormattingEnabled = true;
            this.cboObs.ItemHeight = 13;
            this.cboObs.Location = new System.Drawing.Point(107, 460);
            this.cboObs.Name = "cboObs";
            this.cboObs.Size = new System.Drawing.Size(211, 21);
            this.cboObs.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 331);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "Instrument";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-3, 431);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "Destination";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-3, 460);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 63;
            this.label4.Text = "Obstacle";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(240, 512);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(78, 37);
            this.button3.TabIndex = 67;
            this.button3.Text = "Relocate";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(159, 71);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 34);
            this.button1.TabIndex = 66;
            this.button1.Text = "Explode";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtHTolerance
            // 
            this.txtHTolerance.Location = new System.Drawing.Point(-3, 564);
            this.txtHTolerance.Name = "txtHTolerance";
            this.txtHTolerance.Size = new System.Drawing.Size(76, 20);
            this.txtHTolerance.TabIndex = 71;
            this.txtHTolerance.Text = "5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-3, 548);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 73;
            this.label5.Text = "Horizontal Tol";
            // 
            // txtMaxLength
            // 
            this.txtMaxLength.Location = new System.Drawing.Point(-3, 520);
            this.txtMaxLength.Name = "txtMaxLength";
            this.txtMaxLength.ReadOnly = true;
            this.txtMaxLength.Size = new System.Drawing.Size(76, 20);
            this.txtMaxLength.TabIndex = 75;
            this.txtMaxLength.Text = "0";
            // 
            // txtDuctOptimize
            // 
            this.txtDuctOptimize.Location = new System.Drawing.Point(-3, 619);
            this.txtDuctOptimize.Name = "txtDuctOptimize";
            this.txtDuctOptimize.Size = new System.Drawing.Size(76, 20);
            this.txtDuctOptimize.TabIndex = 76;
            this.txtDuctOptimize.Text = "20";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-6, 603);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 77;
            this.label7.Text = "%  Cable Duct";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(104, 548);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 79;
            this.label6.Text = "Vertical Tol";
            // 
            // txtVTolerenace
            // 
            this.txtVTolerenace.Location = new System.Drawing.Point(107, 564);
            this.txtVTolerenace.Name = "txtVTolerenace";
            this.txtVTolerenace.ReadOnly = true;
            this.txtVTolerenace.Size = new System.Drawing.Size(76, 20);
            this.txtVTolerenace.TabIndex = 78;
            this.txtVTolerenace.Text = "5";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(-5, 501);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 80;
            this.label8.Text = "Max Length";
            // 
            // lblMaxLengthInstrument
            // 
            this.lblMaxLengthInstrument.AutoSize = true;
            this.lblMaxLengthInstrument.Location = new System.Drawing.Point(79, 520);
            this.lblMaxLengthInstrument.Name = "lblMaxLengthInstrument";
            this.lblMaxLengthInstrument.Size = new System.Drawing.Size(0, 13);
            this.lblMaxLengthInstrument.TabIndex = 81;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(244, 681);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(78, 37);
            this.button4.TabIndex = 82;
            this.button4.Text = "Export";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(244, 610);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(78, 37);
            this.button5.TabIndex = 83;
            this.button5.Text = "Optimize";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(107, 610);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(82, 33);
            this.button6.TabIndex = 84;
            this.button6.Text = "Setting";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.btnRouting);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.btnDimension);
            this.panel1.Controls.Add(this.lblMaxLengthInstrument);
            this.panel1.Controls.Add(this.dgvAllLayerInstruments);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.lstInstrument);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnIncre);
            this.panel1.Controls.Add(this.txtVTolerenace);
            this.panel1.Controls.Add(this.btnDecre);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.cboDesti);
            this.panel1.Controls.Add(this.txtDuctOptimize);
            this.panel1.Controls.Add(this.cboObs);
            this.panel1.Controls.Add(this.txtMaxLength);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtHTolerance);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(1132, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 739);
            this.panel1.TabIndex = 85;
            this.panel1.Visible = false;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(321, 609);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(118, 39);
            this.button7.TabIndex = 86;
            this.button7.Text = "Detect CircleBlock";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // lst_circleBlk
            // 
            this.lst_circleBlk.FormattingEnabled = true;
            this.lst_circleBlk.Location = new System.Drawing.Point(461, 609);
            this.lst_circleBlk.Name = "lst_circleBlk";
            this.lst_circleBlk.Size = new System.Drawing.Size(565, 173);
            this.lst_circleBlk.TabIndex = 87;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 806);
            this.Controls.Add(this.lst_circleBlk);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.vdFramedControl1);
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form3";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form3_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllLayerInstruments)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private vdControls.vdFramedControl vdFramedControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRouting;
        private System.Windows.Forms.Button btnDimension;
        private System.Windows.Forms.DataGridView dgvAllLayerInstruments;
        private System.Windows.Forms.ListBox lstInstrument;
        private System.Windows.Forms.Button btnIncre;
        private System.Windows.Forms.Button btnDecre;
        private System.Windows.Forms.ComboBox cboDesti;
        private System.Windows.Forms.ComboBox cboObs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn collayer;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtHTolerance;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMaxLength;
        private System.Windows.Forms.TextBox txtDuctOptimize;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtVTolerenace;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblMaxLengthInstrument;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ListBox lst_circleBlk;
    }
}