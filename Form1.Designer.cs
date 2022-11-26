namespace VFD1
{
    partial class Form1
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
            this.butPolyline = new System.Windows.Forms.Button();
            this.butCircle = new System.Windows.Forms.Button();
            this.vdFramedControl1 = new vdControls.vdFramedControl();
            this.butNew = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // butPolyline
            // 
            this.butPolyline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butPolyline.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.butPolyline.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.butPolyline.ForeColor = System.Drawing.Color.White;
            this.butPolyline.Location = new System.Drawing.Point(1253, 130);
            this.butPolyline.Name = "butPolyline";
            this.butPolyline.Size = new System.Drawing.Size(195, 52);
            this.butPolyline.TabIndex = 30;
            this.butPolyline.Text = "Add Destination";
            this.butPolyline.UseVisualStyleBackColor = false;
            this.butPolyline.Click += new System.EventHandler(this.butPolyline_Click);
            // 
            // butCircle
            // 
            this.butCircle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butCircle.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.butCircle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.butCircle.ForeColor = System.Drawing.Color.White;
            this.butCircle.Location = new System.Drawing.Point(1253, 188);
            this.butCircle.Name = "butCircle";
            this.butCircle.Size = new System.Drawing.Size(195, 52);
            this.butCircle.TabIndex = 24;
            this.butCircle.Text = "Add Instrument";
            this.butCircle.UseVisualStyleBackColor = false;
            this.butCircle.Click += new System.EventHandler(this.butCircle_Click);
            // 
            // vdFramedControl1
            // 
            this.vdFramedControl1.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.vdFramedControl1.DisplayPolarCoord = false;
            this.vdFramedControl1.HistoryLines = ((uint)(3u));
            this.vdFramedControl1.LoadCommandstxt = true;
            this.vdFramedControl1.LoadMenutxt = true;
            this.vdFramedControl1.Location = new System.Drawing.Point(12, 11);
            this.vdFramedControl1.Name = "vdFramedControl1";
            this.vdFramedControl1.PropertyGridWidth = ((uint)(300u));
            this.vdFramedControl1.Size = new System.Drawing.Size(1207, 656);
            this.vdFramedControl1.TabIndex = 43;
            // 
            // butNew
            // 
            this.butNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butNew.Location = new System.Drawing.Point(1253, 11);
            this.butNew.Name = "butNew";
            this.butNew.Size = new System.Drawing.Size(195, 30);
            this.butNew.TabIndex = 44;
            this.butNew.Text = "New Document";
            this.butNew.UseVisualStyleBackColor = true;
            this.butNew.Click += new System.EventHandler(this.butNew_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 736);
            this.Controls.Add(this.butNew);
            this.Controls.Add(this.vdFramedControl1);
            this.Controls.Add(this.butPolyline);
            this.Controls.Add(this.butCircle);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button butPolyline;
        private System.Windows.Forms.Button butCircle;
        private vdControls.vdFramedControl vdFramedControl1;
        private System.Windows.Forms.Button butNew;
    }
}

