namespace taskt.UI.Forms.ScriptEngine.Supplemental
{
    partial class frmHTMLDisplayForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHTMLDisplayForm));
            this.webBrowserHTML = new Microsoft.Web.WebView2.WinForms.WebView2();
            ((System.ComponentModel.ISupportInitialize)(this.webBrowserHTML)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowserHTML
            // 
            this.webBrowserHTML.AllowExternalDrop = true;
            this.webBrowserHTML.CreationProperties = null;
            this.webBrowserHTML.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webBrowserHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserHTML.Location = new System.Drawing.Point(0, 0);
            this.webBrowserHTML.Margin = new System.Windows.Forms.Padding(4);
            this.webBrowserHTML.Name = "webBrowserHTML";
            this.webBrowserHTML.Size = new System.Drawing.Size(1045, 701);
            this.webBrowserHTML.TabIndex = 0;
            this.webBrowserHTML.ZoomFactor = 1D;
            this.webBrowserHTML.NavigationStarting += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs>(this.webBrowserHTML_NavigationStarting);
            this.webBrowserHTML.NavigationCompleted += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs>(this.webBrowserHTML_NavigationCompleted);
            // 
            // frmHTMLDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1045, 701);
            this.Controls.Add(this.webBrowserHTML);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmHTMLDisplayForm";
            this.Text = "taskt - input window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmHTMLDisplayForm_FormClosing);
            this.Load += new System.EventHandler(this.frmHTMLDisplayForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmHTMLDisplayForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.webBrowserHTML)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webBrowserHTML;
    }
}