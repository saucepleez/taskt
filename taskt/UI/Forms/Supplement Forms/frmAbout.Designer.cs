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
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblBuildDate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.BackColor = System.Drawing.Color.Transparent;
            this.lblProjectName.Font = new System.Drawing.Font("Segoe UI", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblProjectName.Location = new System.Drawing.Point(1, 2);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(102, 47);
            this.lblProjectName.TabIndex = 1;
            this.lblProjectName.Text = "taskt";
            // 
            // lblAppVersion
            // 
            this.lblAppVersion.AutoSize = true;
            this.lblAppVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblAppVersion.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppVersion.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblAppVersion.Location = new System.Drawing.Point(8, 46);
            this.lblAppVersion.Name = "lblAppVersion";
            this.lblAppVersion.Size = new System.Drawing.Size(61, 21);
            this.lblAppVersion.TabIndex = 2;
            this.lblAppVersion.Text = "v. 0.0.0";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.BackColor = System.Drawing.Color.Transparent;
            this.lblCopyright.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCopyright.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblCopyright.Location = new System.Drawing.Point(8, 84);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(168, 21);
            this.lblCopyright.TabIndex = 4;
            this.lblCopyright.Text = "(c)2018 Jason Bayldon";
            // 
            // lblBuildDate
            // 
            this.lblBuildDate.AutoSize = true;
            this.lblBuildDate.BackColor = System.Drawing.Color.Transparent;
            this.lblBuildDate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuildDate.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblBuildDate.Location = new System.Drawing.Point(8, 65);
            this.lblBuildDate.Name = "lblBuildDate";
            this.lblBuildDate.Size = new System.Drawing.Size(94, 21);
            this.lblBuildDate.TabIndex = 5;
            this.lblBuildDate.Text = "Build Date:";
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 110;
            this.ClientSize = new System.Drawing.Size(452, 116);
            this.Controls.Add(this.lblBuildDate);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblAppVersion);
            this.Controls.Add(this.lblProjectName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAbout";
            this.Text = "About";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.Label lblAppVersion;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Label lblBuildDate;
    }
}