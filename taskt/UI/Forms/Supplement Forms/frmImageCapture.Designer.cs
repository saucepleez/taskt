namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmImageCapture
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImageCapture));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlMouseContainer = new System.Windows.Forms.Panel();
            this.uiAccept = new taskt.UI.CustomControls.UIPictureButton();
            this.uiClose = new taskt.UI.CustomControls.UIPictureButton();
            this.tabTestMode = new System.Windows.Forms.TabControl();
            this.tabFingerPrintImage = new System.Windows.Forms.TabPage();
            this.pbTaggedImage = new System.Windows.Forms.PictureBox();
            this.tabSearchResult = new System.Windows.Forms.TabPage();
            this.pbSearchResult = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlMouseContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiAccept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiClose)).BeginInit();
            this.tabTestMode.SuspendLayout();
            this.tabFingerPrintImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTaggedImage)).BeginInit();
            this.tabSearchResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSearchResult)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(843, 363);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // pnlMouseContainer
            // 
            this.pnlMouseContainer.Controls.Add(this.uiAccept);
            this.pnlMouseContainer.Controls.Add(this.uiClose);
            this.pnlMouseContainer.Location = new System.Drawing.Point(12, 12);
            this.pnlMouseContainer.Name = "pnlMouseContainer";
            this.pnlMouseContainer.Size = new System.Drawing.Size(105, 55);
            this.pnlMouseContainer.TabIndex = 3;
            this.pnlMouseContainer.MouseEnter += new System.EventHandler(this.pnlMouseContainer_MouseEnter);
            // 
            // uiAccept
            // 
            this.uiAccept.BackColor = System.Drawing.Color.Transparent;
            this.uiAccept.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiAccept.DisplayText = "Accept";
            this.uiAccept.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiAccept.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiAccept.Image = global::taskt.Properties.Resources.action_bar_save;
            this.uiAccept.IsMouseOver = false;
            this.uiAccept.Location = new System.Drawing.Point(3, 3);
            this.uiAccept.Name = "uiAccept";
            this.uiAccept.Size = new System.Drawing.Size(48, 48);
            this.uiAccept.TabIndex = 1;
            this.uiAccept.TabStop = false;
            this.uiAccept.Click += new System.EventHandler(this.uiAccept_Click);
            // 
            // uiClose
            // 
            this.uiClose.BackColor = System.Drawing.Color.Transparent;
            this.uiClose.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiClose.DisplayText = "Close";
            this.uiClose.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiClose.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiClose.Image = global::taskt.Properties.Resources.command_error;
            this.uiClose.IsMouseOver = false;
            this.uiClose.Location = new System.Drawing.Point(53, 3);
            this.uiClose.Name = "uiClose";
            this.uiClose.Size = new System.Drawing.Size(48, 48);
            this.uiClose.TabIndex = 2;
            this.uiClose.TabStop = false;
            this.uiClose.Click += new System.EventHandler(this.uiClose_Click);
            // 
            // tabTestMode
            // 
            this.tabTestMode.Controls.Add(this.tabFingerPrintImage);
            this.tabTestMode.Controls.Add(this.tabSearchResult);
            this.tabTestMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTestMode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTestMode.Location = new System.Drawing.Point(0, 0);
            this.tabTestMode.Name = "tabTestMode";
            this.tabTestMode.SelectedIndex = 0;
            this.tabTestMode.Size = new System.Drawing.Size(843, 363);
            this.tabTestMode.TabIndex = 4;
            // 
            // tabFingerPrintImage
            // 
            this.tabFingerPrintImage.Controls.Add(this.pbTaggedImage);
            this.tabFingerPrintImage.Location = new System.Drawing.Point(4, 24);
            this.tabFingerPrintImage.Name = "tabFingerPrintImage";
            this.tabFingerPrintImage.Padding = new System.Windows.Forms.Padding(3);
            this.tabFingerPrintImage.Size = new System.Drawing.Size(835, 335);
            this.tabFingerPrintImage.TabIndex = 0;
            this.tabFingerPrintImage.Text = "Tagged Image";
            this.tabFingerPrintImage.UseVisualStyleBackColor = true;
            // 
            // pbTaggedImage
            // 
            this.pbTaggedImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbTaggedImage.Location = new System.Drawing.Point(3, 3);
            this.pbTaggedImage.Name = "pbTaggedImage";
            this.pbTaggedImage.Size = new System.Drawing.Size(829, 329);
            this.pbTaggedImage.TabIndex = 0;
            this.pbTaggedImage.TabStop = false;
            // 
            // tabSearchResult
            // 
            this.tabSearchResult.Controls.Add(this.pbSearchResult);
            this.tabSearchResult.Location = new System.Drawing.Point(4, 24);
            this.tabSearchResult.Name = "tabSearchResult";
            this.tabSearchResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabSearchResult.Size = new System.Drawing.Size(835, 335);
            this.tabSearchResult.TabIndex = 1;
            this.tabSearchResult.Text = "Search Result";
            this.tabSearchResult.UseVisualStyleBackColor = true;
            // 
            // pbSearchResult
            // 
            this.pbSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbSearchResult.Location = new System.Drawing.Point(3, 3);
            this.pbSearchResult.Name = "pbSearchResult";
            this.pbSearchResult.Size = new System.Drawing.Size(829, 329);
            this.pbSearchResult.TabIndex = 1;
            this.pbSearchResult.TabStop = false;
            // 
            // frmImageCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 363);
            this.Controls.Add(this.tabTestMode);
            this.Controls.Add(this.pnlMouseContainer);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmImageCapture";
            this.Text = "Capture Image";
            this.Load += new System.EventHandler(this.frmImageCapture_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmImageCapture_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlMouseContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiAccept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiClose)).EndInit();
            this.tabTestMode.ResumeLayout(false);
            this.tabFingerPrintImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbTaggedImage)).EndInit();
            this.tabSearchResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbSearchResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CustomControls.UIPictureButton uiAccept;
        private CustomControls.UIPictureButton uiClose;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlMouseContainer;
        private System.Windows.Forms.TabControl tabTestMode;
        private System.Windows.Forms.TabPage tabFingerPrintImage;
        private System.Windows.Forms.TabPage tabSearchResult;
        public System.Windows.Forms.PictureBox pbTaggedImage;
        public System.Windows.Forms.PictureBox pbSearchResult;
    }
}