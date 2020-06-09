namespace taskt.UI.Forms.Supplement_Forms
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
            this.lblThankYou = new System.Windows.Forms.Label();
            this.lblSpecialThanks = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.BackColor = System.Drawing.Color.Transparent;
            this.lblProjectName.Font = new System.Drawing.Font("Segoe UI Light", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblProjectName.Location = new System.Drawing.Point(1, 0);
            this.lblProjectName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(110, 60);
            this.lblProjectName.TabIndex = 1;
            this.lblProjectName.Text = "taskt";
            // 
            // lblAppVersion
            // 
            this.lblAppVersion.AutoSize = true;
            this.lblAppVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblAppVersion.Font = new System.Drawing.Font("Segoe UI Semilight", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAppVersion.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblAppVersion.Location = new System.Drawing.Point(4, 53);
            this.lblAppVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAppVersion.Name = "lblAppVersion";
            this.lblAppVersion.Size = new System.Drawing.Size(128, 50);
            this.lblAppVersion.TabIndex = 2;
            this.lblAppVersion.Text = "v. 0.0.0";
            // 
            // lblBuildDate
            // 
            this.lblBuildDate.AutoSize = true;
            this.lblBuildDate.BackColor = System.Drawing.Color.Transparent;
            this.lblBuildDate.Font = new System.Drawing.Font("Segoe UI Semilight", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuildDate.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblBuildDate.Location = new System.Drawing.Point(4, 101);
            this.lblBuildDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBuildDate.Name = "lblBuildDate";
            this.lblBuildDate.Size = new System.Drawing.Size(193, 50);
            this.lblBuildDate.TabIndex = 5;
            this.lblBuildDate.Text = "Build Date:";
            // 
            // lblThankYou
            // 
            this.lblThankYou.AutoSize = true;
            this.lblThankYou.BackColor = System.Drawing.Color.Transparent;
            this.lblThankYou.Font = new System.Drawing.Font("Segoe UI Semibold", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThankYou.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblThankYou.Location = new System.Drawing.Point(5, 150);
            this.lblThankYou.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThankYou.Name = "lblThankYou";
            this.lblThankYou.Size = new System.Drawing.Size(225, 60);
            this.lblThankYou.TabIndex = 6;
            this.lblThankYou.Text = "thank you";
            // 
            // lblSpecialThanks
            // 
            this.lblSpecialThanks.BackColor = System.Drawing.Color.Transparent;
            this.lblSpecialThanks.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpecialThanks.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSpecialThanks.Location = new System.Drawing.Point(11, 207);
            this.lblSpecialThanks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSpecialThanks.Name = "lblSpecialThanks";
            this.lblSpecialThanks.Size = new System.Drawing.Size(611, 59);
            this.lblSpecialThanks.TabIndex = 7;
            this.lblSpecialThanks.Text = "to all of the community members and projects that make this software possible.\r\n";
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 272);
            this.Controls.Add(this.lblProjectName);
            this.Controls.Add(this.lblAppVersion);
            this.Controls.Add(this.lblSpecialThanks);
            this.Controls.Add(this.lblBuildDate);
            this.Controls.Add(this.lblThankYou);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
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
        private System.Windows.Forms.Label lblThankYou;
        private System.Windows.Forms.Label lblSpecialThanks;
    }
}