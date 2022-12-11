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
            this.button3 = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllLayerInstruments)).BeginInit();
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
            this.vdFramedControl1.Size = new System.Drawing.Size(1124, 680);
            this.vdFramedControl1.TabIndex = 44;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btnImport, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button3, 2, 0);
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
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(217, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(102, 31);
            this.button3.TabIndex = 50;
            this.button3.Text = "Info";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(110, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 31);
            this.button2.TabIndex = 49;
            this.button2.Text = "Setting";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.ItemHeight = 13;
            this.comboBox1.Location = new System.Drawing.Point(1145, 75);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(208, 21);
            this.comboBox1.TabIndex = 46;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1359, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Input Layer";
            // 
            // btnRouting
            // 
            this.btnRouting.Location = new System.Drawing.Point(1145, 116);
            this.btnRouting.Name = "btnRouting";
            this.btnRouting.Size = new System.Drawing.Size(101, 34);
            this.btnRouting.TabIndex = 51;
            this.btnRouting.Text = "Routing";
            this.btnRouting.UseVisualStyleBackColor = true;
            this.btnRouting.Click += new System.EventHandler(this.btnRouting_Click);
            // 
            // btnDimension
            // 
            this.btnDimension.Location = new System.Drawing.Point(1252, 116);
            this.btnDimension.Name = "btnDimension";
            this.btnDimension.Size = new System.Drawing.Size(101, 34);
            this.btnDimension.TabIndex = 52;
            this.btnDimension.Text = "Dimension";
            this.btnDimension.UseVisualStyleBackColor = true;
            // 
            // dgvAllLayerInstruments
            // 
            this.dgvAllLayerInstruments.AllowUserToDeleteRows = false;
            this.dgvAllLayerInstruments.AllowUserToResizeColumns = false;
            this.dgvAllLayerInstruments.AllowUserToResizeRows = false;
            this.dgvAllLayerInstruments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAllLayerInstruments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.collayer});
            this.dgvAllLayerInstruments.Location = new System.Drawing.Point(1145, 170);
            this.dgvAllLayerInstruments.Name = "dgvAllLayerInstruments";
            this.dgvAllLayerInstruments.Size = new System.Drawing.Size(316, 233);
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
            this.lstInstrument.Location = new System.Drawing.Point(1142, 470);
            this.lstInstrument.Name = "lstInstrument";
            this.lstInstrument.Size = new System.Drawing.Size(319, 121);
            this.lstInstrument.TabIndex = 54;
            this.lstInstrument.SelectedIndexChanged += new System.EventHandler(this.lstInstrument_SelectedIndexChanged);
            // 
            // btnIncre
            // 
            this.btnIncre.BackColor = System.Drawing.Color.Transparent;
            this.btnIncre.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIncre.ForeColor = System.Drawing.Color.Red;
            this.btnIncre.Location = new System.Drawing.Point(1144, 413);
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
            this.btnDecre.Location = new System.Drawing.Point(1184, 413);
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
            this.cboDesti.Location = new System.Drawing.Point(1253, 626);
            this.cboDesti.Name = "cboDesti";
            this.cboDesti.Size = new System.Drawing.Size(211, 21);
            this.cboDesti.TabIndex = 59;
            // 
            // cboObs
            // 
            this.cboObs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboObs.FormattingEnabled = true;
            this.cboObs.ItemHeight = 13;
            this.cboObs.Location = new System.Drawing.Point(1252, 667);
            this.cboObs.Name = "cboObs";
            this.cboObs.Size = new System.Drawing.Size(211, 21);
            this.cboObs.TabIndex = 60;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1142, 454);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "Instrument";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1142, 626);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "Destination";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1142, 667);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 63;
            this.label4.Text = "Obstacle";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 736);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboObs);
            this.Controls.Add(this.cboDesti);
            this.Controls.Add(this.btnDecre);
            this.Controls.Add(this.btnIncre);
            this.Controls.Add(this.lstInstrument);
            this.Controls.Add(this.dgvAllLayerInstruments);
            this.Controls.Add(this.btnDimension);
            this.Controls.Add(this.btnRouting);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.vdFramedControl1);
            this.Name = "Form3";
            this.Text = "Form3";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAllLayerInstruments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private vdControls.vdFramedControl vdFramedControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button button3;
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
    }
}