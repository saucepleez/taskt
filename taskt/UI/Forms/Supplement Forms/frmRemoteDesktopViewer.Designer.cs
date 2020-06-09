namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmRemoteDesktopViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRemoteDesktopViewer));
            this.pnlCover = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.tmrLoginFailure = new System.Windows.Forms.Timer(this.components);
            this.axRDP = new AxMSTSCLib.AxMsRdpClient6NotSafeForScripting();
            this.pnlCover.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axRDP)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCover
            // 
            this.pnlCover.BackColor = System.Drawing.Color.DimGray;
            this.pnlCover.Controls.Add(this.lblLogo);
            this.pnlCover.Location = new System.Drawing.Point(12, 12);
            this.pnlCover.Name = "pnlCover";
            this.pnlCover.Size = new System.Drawing.Size(200, 100);
            this.pnlCover.TabIndex = 1;
            this.pnlCover.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnlCover_MouseDoubleClick);
            // 
            // lblLogo
            // 
            this.lblLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(743, 377);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(515, 86);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "taskt vm window";
            // 
            // tmrLoginFailure
            // 
            this.tmrLoginFailure.Interval = 15000;
            this.tmrLoginFailure.Tick += new System.EventHandler(this.tmrLoginFailure_Tick);
            // 
            // axRDP
            // 
            this.axRDP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axRDP.Enabled = true;
            this.axRDP.Location = new System.Drawing.Point(0, 0);
            this.axRDP.Name = "axRDP";
            this.axRDP.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axRDP.OcxState")));
            this.axRDP.Size = new System.Drawing.Size(1904, 1041);
            this.axRDP.TabIndex = 2;
            // 
            // frmRemoteDesktopViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.pnlCover);
            this.Controls.Add(this.axRDP);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRemoteDesktopViewer";
            this.Text = "Remote Desktop";
            this.Load += new System.EventHandler(this.frmRemoteDesktopViewer_Load);
            this.pnlCover.ResumeLayout(false);
            this.pnlCover.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axRDP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlCover;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Timer tmrLoginFailure;
        private AxMSTSCLib.AxMsRdpClient6NotSafeForScripting axRDP;
    }
}