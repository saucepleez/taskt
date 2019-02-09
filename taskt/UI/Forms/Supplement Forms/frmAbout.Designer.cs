namespace taskt.UI.Forms.Supplemental
{
    partial class frmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.lblProjectName = new System.Windows.Forms.Label();
            this.lblAppVersion = new System.Windows.Forms.Label();
            this.lblBuildDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.BackColor = System.Drawing.Color.Transparent;
            this.lblProjectName.Font = new System.Drawing.Font("Segoe UI Light", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblProjectName.Location = new System.Drawing.Point(1, 0);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(87, 47);
            this.lblProjectName.TabIndex = 1;
            this.lblProjectName.Text = "taskt";
            // 
            // lblAppVersion
            // 
            this.lblAppVersion.AutoSize = true;
            this.lblAppVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblAppVersion.Font = new System.Drawing.Font("Segoe UI Semilight", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppVersion.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblAppVersion.Location = new System.Drawing.Point(3, 43);
            this.lblAppVersion.Name = "lblAppVersion";
            this.lblAppVersion.Size = new System.Drawing.Size(100, 40);
            this.lblAppVersion.TabIndex = 2;
            this.lblAppVersion.Text = "v. 0.0.0";
            // 
            // lblBuildDate
            // 
            this.lblBuildDate.AutoSize = true;
            this.lblBuildDate.BackColor = System.Drawing.Color.Transparent;
            this.lblBuildDate.Font = new System.Drawing.Font("Segoe UI Semilight", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuildDate.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblBuildDate.Location = new System.Drawing.Point(3, 82);
            this.lblBuildDate.Name = "lblBuildDate";
            this.lblBuildDate.Size = new System.Drawing.Size(151, 40);
            this.lblBuildDate.TabIndex = 5;
            this.lblBuildDate.Text = "Build Date:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(4, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 47);
            this.label1.TabIndex = 6;
            this.label1.Text = "thank you";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(8, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(458, 48);
            this.label2.TabIndex = 7;
            this.label2.Text = "to all of the community members and projects that make this software possible.\r\n";
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 221);
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.lblAppVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblBuildDate);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAbout";
            this.Text = "about";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblAppVersion;
        private System.Windows.Forms.Label lblBuildDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}