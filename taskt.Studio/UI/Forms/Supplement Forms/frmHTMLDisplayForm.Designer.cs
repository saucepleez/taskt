namespace taskt.UI.Forms.Supplement_Forms
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
            this.webBrowserHTML = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowserHTML
            // 
            this.webBrowserHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserHTML.Location = new System.Drawing.Point(0, 0);
            this.webBrowserHTML.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserHTML.Name = "webBrowserHTML";
            this.webBrowserHTML.Size = new System.Drawing.Size(1262, 801);
            this.webBrowserHTML.TabIndex = 0;
            // 
            // frmHTMLDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1262, 801);
            this.Controls.Add(this.webBrowserHTML);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmHTMLDisplayForm";
            this.Text = "taskt - input window";
            this.Load += new System.EventHandler(this.frmHTMLDisplayForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowserHTML;
    }
}