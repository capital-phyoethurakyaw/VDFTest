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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
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
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(1253, 441);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(195, 52);
            this.button1.TabIndex = 45;
            this.button1.Text = "Build Route";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(1253, 499);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(195, 52);
            this.button2.TabIndex = 46;
            this.button2.Text = "Generate Dimension";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(1253, 246);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(195, 52);
            this.button3.TabIndex = 47;
            this.button3.Text = "Add Obstacle";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 736);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.vdFramedControl1);
            this.Controls.Add(this.butNew);
            this.Controls.Add(this.butPolyline);
            this.Controls.Add(this.butCircle);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button butPolyline;
        private System.Windows.Forms.Button butCircle;
        private vdControls.vdFramedControl vdFramedControl1;
        private System.Windows.Forms.Button butNew;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

