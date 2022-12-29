namespace VFD1.Project
{
    partial class ProjectInfo
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
            this.sidepanel = new System.Windows.Forms.Panel();
            this.btc = new System.Windows.Forms.Button();
            this.btcd = new System.Windows.Forms.Button();
            this.btinst = new System.Windows.Forms.Button();
            this.btproject = new System.Windows.Forms.Button();
            this.mainpanel1 = new System.Windows.Forms.Panel();
            this.sidepanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sidepanel
            // 
            this.sidepanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sidepanel.Controls.Add(this.btc);
            this.sidepanel.Controls.Add(this.btcd);
            this.sidepanel.Controls.Add(this.btinst);
            this.sidepanel.Controls.Add(this.btproject);
            this.sidepanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.sidepanel.Location = new System.Drawing.Point(0, 0);
            this.sidepanel.Margin = new System.Windows.Forms.Padding(2);
            this.sidepanel.Name = "sidepanel";
            this.sidepanel.Size = new System.Drawing.Size(98, 732);
            this.sidepanel.TabIndex = 6;
            // 
            // btc
            // 
            this.btc.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btc.Location = new System.Drawing.Point(4, 78);
            this.btc.Margin = new System.Windows.Forms.Padding(2);
            this.btc.Name = "btc";
            this.btc.Size = new System.Drawing.Size(87, 32);
            this.btc.TabIndex = 8;
            this.btc.Text = "Cable";
            this.btc.UseVisualStyleBackColor = false;
            // 
            // btcd
            // 
            this.btcd.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btcd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btcd.Location = new System.Drawing.Point(4, 115);
            this.btcd.Margin = new System.Windows.Forms.Padding(2);
            this.btcd.Name = "btcd";
            this.btcd.Size = new System.Drawing.Size(87, 32);
            this.btcd.TabIndex = 3;
            this.btcd.Text = "Cable Duct";
            this.btcd.UseVisualStyleBackColor = false;
            // 
            // btinst
            // 
            this.btinst.BackColor = System.Drawing.Color.White;
            this.btinst.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btinst.Location = new System.Drawing.Point(4, 42);
            this.btinst.Margin = new System.Windows.Forms.Padding(2);
            this.btinst.Name = "btinst";
            this.btinst.Size = new System.Drawing.Size(87, 32);
            this.btinst.TabIndex = 1;
            this.btinst.Text = "Instrument";
            this.btinst.UseVisualStyleBackColor = false;
            this.btinst.Click += new System.EventHandler(this.btinst_Click);
            // 
            // btproject
            // 
            this.btproject.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btproject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btproject.Location = new System.Drawing.Point(4, 6);
            this.btproject.Margin = new System.Windows.Forms.Padding(2);
            this.btproject.Name = "btproject";
            this.btproject.Size = new System.Drawing.Size(87, 32);
            this.btproject.TabIndex = 0;
            this.btproject.Text = "Project";
            this.btproject.UseVisualStyleBackColor = false;
            this.btproject.Click += new System.EventHandler(this.btproject_Click);
            // 
            // mainpanel1
            // 
            this.mainpanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainpanel1.Location = new System.Drawing.Point(98, 0);
            this.mainpanel1.Margin = new System.Windows.Forms.Padding(2);
            this.mainpanel1.Name = "mainpanel1";
            this.mainpanel1.Size = new System.Drawing.Size(1100, 732);
            this.mainpanel1.TabIndex = 7;
            // 
            // ProjectInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1198, 732);
            this.Controls.Add(this.mainpanel1);
            this.Controls.Add(this.sidepanel);
            this.Name = "ProjectInfo";
            this.Text = "ProjectInfo";
            this.Load += new System.EventHandler(this.ProjectInfo_Load);
            this.sidepanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel sidepanel;
        private System.Windows.Forms.Button btc;
        private System.Windows.Forms.Button btcd;
        private System.Windows.Forms.Button btinst;
        private System.Windows.Forms.Button btproject;
        private System.Windows.Forms.Panel mainpanel1;
    }
}