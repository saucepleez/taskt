namespace taskt.UI.Forms.Supplemental
{
    partial class frmScriptSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScriptSelector));
            this.cboSelectFile = new System.Windows.Forms.ComboBox();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnOpen = new taskt.UI.CustomControls.UIPictureButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.SuspendLayout();
            // 
            // cboSelectFile
            // 
            this.cboSelectFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSelectFile.FormattingEnabled = true;
            this.cboSelectFile.Location = new System.Drawing.Point(12, 65);
            this.cboSelectFile.Name = "cboSelectFile";
            this.cboSelectFile.Size = new System.Drawing.Size(633, 28);
            this.cboSelectFile.TabIndex = 0;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMainLogo.Location = new System.Drawing.Point(46, 22);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(164, 37);
            this.lblMainLogo.TabIndex = 4;
            this.lblMainLogo.Text = "Open File";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::taskt.Properties.Resources.logo;
            this.pictureBox2.Location = new System.Drawing.Point(12, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(63, 101);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(48, 48);
            this.uiBtnCancel.TabIndex = 13;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // uiBtnOpen
            // 
            this.uiBtnOpen.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOpen.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOpen.DisplayText = "Ok";
            this.uiBtnOpen.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOpen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOpen.Image")));
            this.uiBtnOpen.IsMouseOver = false;
            this.uiBtnOpen.Location = new System.Drawing.Point(12, 101);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(48, 48);
            this.uiBtnOpen.TabIndex = 12;
            this.uiBtnOpen.TabStop = false;
            this.uiBtnOpen.Click += new System.EventHandler(this.uiBtnOpen_Click);
            // 
            // frmScriptSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.BackgroundChangeIndex = 96;
            this.ClientSize = new System.Drawing.Size(659, 154);
            this.Controls.Add(this.uiBtnCancel);
            this.Controls.Add(this.uiBtnOpen);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lblMainLogo);
            this.Controls.Add(this.cboSelectFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmScriptSelector";
            this.Text = "Please select a script";
            this.Load += new System.EventHandler(this.frmScriptSelector_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ComboBox cboSelectFile;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblMainLogo;
        private taskt.UI.CustomControls.UIPictureButton uiBtnCancel;
        private taskt.UI.CustomControls.UIPictureButton uiBtnOpen;
    }
}