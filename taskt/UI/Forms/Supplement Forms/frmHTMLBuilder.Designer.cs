namespace taskt.UI.Forms.Supplemental
{
    partial class frmHTMLBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHTMLBuilder));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flwAcceptIcons = new System.Windows.Forms.FlowLayoutPanel();
            this.rtbHTML = new System.Windows.Forms.RichTextBox();
            this.webBrowserHTML = new System.Windows.Forms.WebBrowser();
            this.uiBtnOK = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.flwAcceptIcons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.webBrowserHTML, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flwAcceptIcons, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.rtbHTML, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1620, 697);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flwAcceptIcons
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.flwAcceptIcons, 2);
            this.flwAcceptIcons.Controls.Add(this.uiBtnOK);
            this.flwAcceptIcons.Controls.Add(this.uiBtnCancel);
            this.flwAcceptIcons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flwAcceptIcons.Location = new System.Drawing.Point(3, 640);
            this.flwAcceptIcons.Name = "flwAcceptIcons";
            this.flwAcceptIcons.Size = new System.Drawing.Size(1614, 54);
            this.flwAcceptIcons.TabIndex = 2;
            // 
            // rtbHTML
            // 
            this.rtbHTML.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHTML.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbHTML.Location = new System.Drawing.Point(3, 3);
            this.rtbHTML.Name = "rtbHTML";
            this.rtbHTML.Size = new System.Drawing.Size(804, 631);
            this.rtbHTML.TabIndex = 3;
            this.rtbHTML.Text = "";
            this.rtbHTML.TextChanged += new System.EventHandler(this.rtbHTML_TextChanged);
            // 
            // webBrowserHTML
            // 
            this.webBrowserHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserHTML.Location = new System.Drawing.Point(813, 3);
            this.webBrowserHTML.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserHTML.Name = "webBrowserHTML";
            this.webBrowserHTML.Size = new System.Drawing.Size(804, 631);
            this.webBrowserHTML.TabIndex = 0;
            // 
            // uiBtnOK
            // 
            this.uiBtnOK.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOK.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOK.DisplayText = "Save";
            this.uiBtnOK.DisplayTextBrush = System.Drawing.Color.SteelBlue;
            this.uiBtnOK.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOK.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOK.Image")));
            this.uiBtnOK.IsMouseOver = false;
            this.uiBtnOK.Location = new System.Drawing.Point(3, 3);
            this.uiBtnOK.Name = "uiBtnOK";
            this.uiBtnOK.Size = new System.Drawing.Size(48, 48);
            this.uiBtnOK.TabIndex = 18;
            this.uiBtnOK.TabStop = false;
            this.uiBtnOK.Click += new System.EventHandler(this.uiBtnOK_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Close";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.SteelBlue;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(57, 3);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(48, 48);
            this.uiBtnCancel.TabIndex = 19;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // frmHTMLBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1620, 697);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmHTMLBuilder";
            this.Text = "taskt HTML Builder";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flwAcceptIcons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.WebBrowser webBrowserHTML;
        private System.Windows.Forms.FlowLayoutPanel flwAcceptIcons;
        private CustomControls.UIPictureButton uiBtnOK;
        private CustomControls.UIPictureButton uiBtnCancel;
        public System.Windows.Forms.RichTextBox rtbHTML;
    }
}