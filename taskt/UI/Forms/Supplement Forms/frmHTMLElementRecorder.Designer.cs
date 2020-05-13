using System.Drawing;

namespace taskt.UI.Forms.Supplemental
{
    partial class frmHTMLElementRecorder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHTMLElementRecorder));
            this.tlpControls = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pbBack = new System.Windows.Forms.PictureBox();
            this.pbForward = new System.Windows.Forms.PictureBox();
            this.pgGo = new System.Windows.Forms.PictureBox();
            this.tbURL = new System.Windows.Forms.TextBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.pbRefresh = new System.Windows.Forms.PictureBox();
            this.pbRecord = new System.Windows.Forms.PictureBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.chkStopOnClick = new System.Windows.Forms.CheckBox();
            this.lblSubHeader = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tlpControls.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForward)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgGo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRecord)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpControls
            // 
            this.tlpControls.ColumnCount = 1;
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpControls.Controls.Add(this.pnlHeader, 0, 0);
            this.tlpControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpControls.Location = new System.Drawing.Point(0, 0);
            this.tlpControls.Margin = new System.Windows.Forms.Padding(4);
            this.tlpControls.Name = "tlpControls";
            this.tlpControls.RowCount = 1;
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 689F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 689F));
            this.tlpControls.Size = new System.Drawing.Size(1403, 689);
            this.tlpControls.TabIndex = 1;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlHeader.Controls.Add(this.pbBack);
            this.pnlHeader.Controls.Add(this.pbForward);
            this.pnlHeader.Controls.Add(this.pgGo);
            this.pnlHeader.Controls.Add(this.tbURL);
            this.pnlHeader.Controls.Add(this.lblURL);
            this.pnlHeader.Controls.Add(this.webBrowser1);
            this.pnlHeader.Controls.Add(this.pbRefresh);
            this.pnlHeader.Controls.Add(this.pbRecord);
            this.pnlHeader.Controls.Add(this.lblDescription);
            this.pnlHeader.Controls.Add(this.chkStopOnClick);
            this.pnlHeader.Controls.Add(this.lblSubHeader);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1403, 689);
            this.pnlHeader.TabIndex = 1;
            // 
            // pbBack
            // 
            this.pbBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbBack.Image = ((System.Drawing.Image)(resources.GetObject("pbBack.Image")));
            this.pbBack.Location = new System.Drawing.Point(1197, 119);
            this.pbBack.Name = "pbBack";
            this.pbBack.Size = new System.Drawing.Size(43, 39);
            this.pbBack.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbBack.TabIndex = 26;
            this.pbBack.TabStop = false;
            this.pbBack.Click += new System.EventHandler(this.pbBack_Click);
            // 
            // pbForward
            // 
            this.pbForward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbForward.Image = global::taskt.Properties.Resources.links_header;
            this.pbForward.Location = new System.Drawing.Point(1246, 119);
            this.pbForward.Name = "pbForward";
            this.pbForward.Size = new System.Drawing.Size(43, 39);
            this.pbForward.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbForward.TabIndex = 25;
            this.pbForward.TabStop = false;
            this.pbForward.Click += new System.EventHandler(this.pbForward_Click);
            // 
            // pgGo
            // 
            this.pgGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pgGo.Image = global::taskt.Properties.Resources.command_start_process;
            this.pgGo.Location = new System.Drawing.Point(1146, 119);
            this.pgGo.Name = "pgGo";
            this.pgGo.Size = new System.Drawing.Size(43, 39);
            this.pgGo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pgGo.TabIndex = 24;
            this.pgGo.TabStop = false;
            this.pgGo.Click += new System.EventHandler(this.pbGo_Click);
            // 
            // tbURL
            // 
            this.tbURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbURL.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.tbURL.Location = new System.Drawing.Point(119, 118);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(1021, 34);
            this.tbURL.TabIndex = 23;
            this.tbURL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbURL_KeyDown);
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblURL.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.lblURL.Location = new System.Drawing.Point(12, 121);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(101, 28);
            this.lblURL.TabIndex = 22;
            this.lblURL.Text = "Enter URL:";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(0, 195);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(1403, 494);
            this.webBrowser1.TabIndex = 21;
            this.webBrowser1.Url = new System.Uri("https://www.google.com/", System.UriKind.Absolute);
            // 
            // pbRefresh
            // 
            this.pbRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbRefresh.Image = global::taskt.Properties.Resources.command_startloop;
            this.pbRefresh.Location = new System.Drawing.Point(1296, 119);
            this.pbRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.pbRefresh.Name = "pbRefresh";
            this.pbRefresh.Size = new System.Drawing.Size(43, 39);
            this.pbRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRefresh.TabIndex = 19;
            this.pbRefresh.TabStop = false;
            this.pbRefresh.Click += new System.EventHandler(this.pbRefresh_Click);
            // 
            // pbRecord
            // 
            this.pbRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbRecord.Image = global::taskt.Properties.Resources.various_record_button;
            this.pbRecord.Location = new System.Drawing.Point(1347, 119);
            this.pbRecord.Margin = new System.Windows.Forms.Padding(4);
            this.pbRecord.Name = "pbRecord";
            this.pbRecord.Size = new System.Drawing.Size(43, 39);
            this.pbRecord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRecord.TabIndex = 4;
            this.pbRecord.TabStop = false;
            this.pbRecord.Click += new System.EventHandler(this.pbRecord_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.ForeColor = System.Drawing.Color.White;
            this.lblDescription.Location = new System.Drawing.Point(13, 85);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(1294, 36);
            this.lblDescription.TabIndex = 16;
            this.lblDescription.Text = "Instructions: navigate to the target URL and click the record button.  Once recor" +
    "ding has started, click the element that you want to capture.\r\n ";
            // 
            // chkStopOnClick
            // 
            this.chkStopOnClick.AutoSize = true;
            this.chkStopOnClick.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStopOnClick.ForeColor = System.Drawing.Color.White;
            this.chkStopOnClick.Location = new System.Drawing.Point(18, 161);
            this.chkStopOnClick.Margin = new System.Windows.Forms.Padding(4);
            this.chkStopOnClick.Name = "chkStopOnClick";
            this.chkStopOnClick.Size = new System.Drawing.Size(249, 27);
            this.chkStopOnClick.TabIndex = 20;
            this.chkStopOnClick.Text = "Stop Recording on First Click";
            this.chkStopOnClick.UseVisualStyleBackColor = true;
            // 
            // lblSubHeader
            // 
            this.lblSubHeader.AutoSize = true;
            this.lblSubHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubHeader.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.lblSubHeader.Location = new System.Drawing.Point(12, 53);
            this.lblSubHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSubHeader.Name = "lblSubHeader";
            this.lblSubHeader.Size = new System.Drawing.Size(455, 28);
            this.lblSubHeader.TabIndex = 15;
            this.lblSubHeader.Text = "Capture HTML Elements from Browser Applications";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeader.Location = new System.Drawing.Point(9, 11);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(461, 46);
            this.lblHeader.TabIndex = 14;
            this.lblHeader.Text = "HTML element recorder";
            // 
            // frmHTMLElementRecorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.BackgroundChangeIndex = 265;
            this.ClientSize = new System.Drawing.Size(1403, 689);
            this.Controls.Add(this.tlpControls);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmHTMLElementRecorder";
            this.Text = "HTML element recorder";
            this.Load += new System.EventHandler(this.frmHTMLElementRecorder_Load);
            this.tlpControls.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbForward)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pgGo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRecord)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlpControls;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.PictureBox pgGo;
        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.PictureBox pbRefresh;
        private System.Windows.Forms.PictureBox pbRecord;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.CheckBox chkStopOnClick;
        private System.Windows.Forms.Label lblSubHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.PictureBox pbBack;
        private System.Windows.Forms.PictureBox pbForward;
    }
}