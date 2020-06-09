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
            this.pbMainImage = new System.Windows.Forms.PictureBox();
            this.pnlMouseContainer = new System.Windows.Forms.Panel();
            this.uiAccept = new taskt.UI.CustomControls.UIPictureButton();
            this.uiClose = new taskt.UI.CustomControls.UIPictureButton();
            this.tabTestMode = new System.Windows.Forms.TabControl();
            this.tabFingerPrintImage = new System.Windows.Forms.TabPage();
            this.pbTaggedImage = new System.Windows.Forms.PictureBox();
            this.tabSearchResult = new System.Windows.Forms.TabPage();
            this.pbSearchResult = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbMainImage)).BeginInit();
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
            // pbMainImage
            // 
            this.pbMainImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbMainImage.Location = new System.Drawing.Point(0, 0);
            this.pbMainImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbMainImage.Name = "pbMainImage";
            this.pbMainImage.Size = new System.Drawing.Size(1124, 447);
            this.pbMainImage.TabIndex = 0;
            this.pbMainImage.TabStop = false;
            this.pbMainImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbMainImage_MouseDown);
            this.pbMainImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbMainImage_MouseMove);
            // 
            // pnlMouseContainer
            // 
            this.pnlMouseContainer.Controls.Add(this.uiAccept);
            this.pnlMouseContainer.Controls.Add(this.uiClose);
            this.pnlMouseContainer.Location = new System.Drawing.Point(16, 15);
            this.pnlMouseContainer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlMouseContainer.Name = "pnlMouseContainer";
            this.pnlMouseContainer.Size = new System.Drawing.Size(140, 68);
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
            this.uiAccept.Location = new System.Drawing.Point(4, 4);
            this.uiAccept.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uiAccept.Name = "uiAccept";
            this.uiAccept.Size = new System.Drawing.Size(64, 59);
            this.uiAccept.TabIndex = 1;
            this.uiAccept.TabStop = false;
            this.uiAccept.Text = "Accept";
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
            this.uiClose.Location = new System.Drawing.Point(71, 4);
            this.uiClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uiClose.Name = "uiClose";
            this.uiClose.Size = new System.Drawing.Size(64, 59);
            this.uiClose.TabIndex = 2;
            this.uiClose.TabStop = false;
            this.uiClose.Text = "Close";
            this.uiClose.Click += new System.EventHandler(this.uiClose_Click);
            // 
            // tabTestMode
            // 
            this.tabTestMode.Controls.Add(this.tabFingerPrintImage);
            this.tabTestMode.Controls.Add(this.tabSearchResult);
            this.tabTestMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTestMode.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabTestMode.Location = new System.Drawing.Point(0, 0);
            this.tabTestMode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabTestMode.Name = "tabTestMode";
            this.tabTestMode.SelectedIndex = 0;
            this.tabTestMode.Size = new System.Drawing.Size(1124, 447);
            this.tabTestMode.TabIndex = 4;
            // 
            // tabFingerPrintImage
            // 
            this.tabFingerPrintImage.Controls.Add(this.pbTaggedImage);
            this.tabFingerPrintImage.Location = new System.Drawing.Point(4, 29);
            this.tabFingerPrintImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabFingerPrintImage.Name = "tabFingerPrintImage";
            this.tabFingerPrintImage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabFingerPrintImage.Size = new System.Drawing.Size(1116, 414);
            this.tabFingerPrintImage.TabIndex = 0;
            this.tabFingerPrintImage.Text = "Tagged Image";
            this.tabFingerPrintImage.UseVisualStyleBackColor = true;
            // 
            // pbTaggedImage
            // 
            this.pbTaggedImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbTaggedImage.Location = new System.Drawing.Point(4, 4);
            this.pbTaggedImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbTaggedImage.Name = "pbTaggedImage";
            this.pbTaggedImage.Size = new System.Drawing.Size(1108, 406);
            this.pbTaggedImage.TabIndex = 0;
            this.pbTaggedImage.TabStop = false;
            // 
            // tabSearchResult
            // 
            this.tabSearchResult.Controls.Add(this.pbSearchResult);
            this.tabSearchResult.Location = new System.Drawing.Point(4, 29);
            this.tabSearchResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabSearchResult.Name = "tabSearchResult";
            this.tabSearchResult.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabSearchResult.Size = new System.Drawing.Size(1116, 414);
            this.tabSearchResult.TabIndex = 1;
            this.tabSearchResult.Text = "Search Result";
            this.tabSearchResult.UseVisualStyleBackColor = true;
            // 
            // pbSearchResult
            // 
            this.pbSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbSearchResult.Location = new System.Drawing.Point(4, 4);
            this.pbSearchResult.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbSearchResult.Name = "pbSearchResult";
            this.pbSearchResult.Size = new System.Drawing.Size(1108, 406);
            this.pbSearchResult.TabIndex = 1;
            this.pbSearchResult.TabStop = false;
            // 
            // frmImageCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1124, 447);
            this.Controls.Add(this.tabTestMode);
            this.Controls.Add(this.pnlMouseContainer);
            this.Controls.Add(this.pbMainImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmImageCapture";
            this.Text = "Capture Image";
            this.Load += new System.EventHandler(this.frmImageCapture_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmImageCapture_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbMainImage)).EndInit();
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
        public System.Windows.Forms.PictureBox pbMainImage;
        private System.Windows.Forms.Panel pnlMouseContainer;
        private System.Windows.Forms.TabControl tabTestMode;
        private System.Windows.Forms.TabPage tabFingerPrintImage;
        private System.Windows.Forms.TabPage tabSearchResult;
        public System.Windows.Forms.PictureBox pbTaggedImage;
        public System.Windows.Forms.PictureBox pbSearchResult;
    }
}