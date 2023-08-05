namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmJSONPathHelper
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.uiBtnAdd = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.splitBody = new System.Windows.Forms.SplitContainer();
            this.tableRawJSON = new System.Windows.Forms.TableLayoutPanel();
            this.panalRawJSONButtons = new System.Windows.Forms.Panel();
            this.picClear = new System.Windows.Forms.PictureBox();
            this.btnParse = new System.Windows.Forms.Button();
            this.picOpenFromURL = new System.Windows.Forms.PictureBox();
            this.picOpenFromFile = new System.Windows.Forms.PictureBox();
            this.txtRawJSON = new System.Windows.Forms.TextBox();
            this.tableJSONParse = new System.Windows.Forms.TableLayoutPanel();
            this.splitJsonParse = new System.Windows.Forms.SplitContainer();
            this.tvJSON = new System.Windows.Forms.TreeView();
            this.txtJSONPathResult = new System.Windows.Forms.TextBox();
            this.panelJSONPath = new System.Windows.Forms.Panel();
            this.txtJSONPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.myToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitBody)).BeginInit();
            this.splitBody.Panel1.SuspendLayout();
            this.splitBody.Panel2.SuspendLayout();
            this.splitBody.SuspendLayout();
            this.tableRawJSON.SuspendLayout();
            this.panalRawJSONButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOpenFromURL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOpenFromFile)).BeginInit();
            this.tableJSONParse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitJsonParse)).BeginInit();
            this.splitJsonParse.Panel1.SuspendLayout();
            this.splitJsonParse.Panel2.SuspendLayout();
            this.splitJsonParse.SuspendLayout();
            this.panelJSONPath.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelFooter, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelHeader, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.splitBody, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(711, 389);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.lblMessage);
            this.panelFooter.Controls.Add(this.uiBtnAdd);
            this.panelFooter.Controls.Add(this.uiBtnCancel);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(0, 332);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(711, 57);
            this.panelFooter.TabIndex = 0;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(323, 18);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(65, 20);
            this.lblMessage.TabIndex = 20;
            this.lblMessage.Text = "Copied!!";
            // 
            // uiBtnAdd
            // 
            this.uiBtnAdd.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnAdd.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnAdd.DisplayText = "Ok";
            this.uiBtnAdd.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnAdd.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnAdd.Image = global::taskt.Properties.Resources.various_ok_button;
            this.uiBtnAdd.IsMouseOver = false;
            this.uiBtnAdd.Location = new System.Drawing.Point(15, 3);
            this.uiBtnAdd.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.uiBtnAdd.Name = "uiBtnAdd";
            this.uiBtnAdd.Size = new System.Drawing.Size(88, 49);
            this.uiBtnAdd.TabIndex = 18;
            this.uiBtnAdd.TabStop = false;
            this.uiBtnAdd.Text = "Ok";
            this.uiBtnAdd.Click += new System.EventHandler(this.uiBtnAdd_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = global::taskt.Properties.Resources.various_cancel_button;
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(115, 3);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(88, 49);
            this.uiBtnCancel.TabIndex = 19;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(711, 40);
            this.panelHeader.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "JSONPath Helper";
            // 
            // splitBody
            // 
            this.splitBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.splitBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBody.Location = new System.Drawing.Point(3, 43);
            this.splitBody.Name = "splitBody";
            // 
            // splitBody.Panel1
            // 
            this.splitBody.Panel1.Controls.Add(this.tableRawJSON);
            // 
            // splitBody.Panel2
            // 
            this.splitBody.Panel2.Controls.Add(this.tableJSONParse);
            this.splitBody.Size = new System.Drawing.Size(705, 286);
            this.splitBody.SplitterDistance = 235;
            this.splitBody.TabIndex = 2;
            // 
            // tableRawJSON
            // 
            this.tableRawJSON.ColumnCount = 1;
            this.tableRawJSON.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableRawJSON.Controls.Add(this.panalRawJSONButtons, 0, 0);
            this.tableRawJSON.Controls.Add(this.txtRawJSON, 0, 1);
            this.tableRawJSON.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableRawJSON.Location = new System.Drawing.Point(0, 0);
            this.tableRawJSON.Name = "tableRawJSON";
            this.tableRawJSON.RowCount = 2;
            this.tableRawJSON.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableRawJSON.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableRawJSON.Size = new System.Drawing.Size(235, 286);
            this.tableRawJSON.TabIndex = 0;
            // 
            // panalRawJSONButtons
            // 
            this.panalRawJSONButtons.Controls.Add(this.picClear);
            this.panalRawJSONButtons.Controls.Add(this.btnParse);
            this.panalRawJSONButtons.Controls.Add(this.picOpenFromURL);
            this.panalRawJSONButtons.Controls.Add(this.picOpenFromFile);
            this.panalRawJSONButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panalRawJSONButtons.Location = new System.Drawing.Point(0, 0);
            this.panalRawJSONButtons.Margin = new System.Windows.Forms.Padding(0);
            this.panalRawJSONButtons.Name = "panalRawJSONButtons";
            this.panalRawJSONButtons.Size = new System.Drawing.Size(235, 40);
            this.panalRawJSONButtons.TabIndex = 0;
            // 
            // picClear
            // 
            this.picClear.BackgroundImage = global::taskt.Properties.Resources.command_error;
            this.picClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picClear.Location = new System.Drawing.Point(94, 2);
            this.picClear.Name = "picClear";
            this.picClear.Size = new System.Drawing.Size(38, 38);
            this.picClear.TabIndex = 3;
            this.picClear.TabStop = false;
            this.myToolTip.SetToolTip(this.picClear, "Clear JSON");
            this.picClear.Click += new System.EventHandler(this.picClear_Click);
            // 
            // btnParse
            // 
            this.btnParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParse.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnParse.Location = new System.Drawing.Point(157, 3);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(75, 34);
            this.btnParse.TabIndex = 1;
            this.btnParse.Text = "Pars&e";
            this.myToolTip.SetToolTip(this.btnParse, "Double-Click on the text box below to Parse");
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
            // 
            // picOpenFromURL
            // 
            this.picOpenFromURL.BackgroundImage = global::taskt.Properties.Resources.command_web;
            this.picOpenFromURL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picOpenFromURL.Location = new System.Drawing.Point(50, 3);
            this.picOpenFromURL.Name = "picOpenFromURL";
            this.picOpenFromURL.Size = new System.Drawing.Size(38, 38);
            this.picOpenFromURL.TabIndex = 2;
            this.picOpenFromURL.TabStop = false;
            this.myToolTip.SetToolTip(this.picOpenFromURL, "Open From URL");
            this.picOpenFromURL.Click += new System.EventHandler(this.picOpenFromURL_Click);
            // 
            // picOpenFromFile
            // 
            this.picOpenFromFile.BackgroundImage = global::taskt.Properties.Resources.command_files;
            this.picOpenFromFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picOpenFromFile.Location = new System.Drawing.Point(6, 3);
            this.picOpenFromFile.Name = "picOpenFromFile";
            this.picOpenFromFile.Size = new System.Drawing.Size(38, 38);
            this.picOpenFromFile.TabIndex = 1;
            this.picOpenFromFile.TabStop = false;
            this.myToolTip.SetToolTip(this.picOpenFromFile, "Open From File");
            this.picOpenFromFile.Click += new System.EventHandler(this.picOpenFromFile_Click);
            // 
            // txtRawJSON
            // 
            this.txtRawJSON.AllowDrop = true;
            this.txtRawJSON.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRawJSON.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.txtRawJSON.Location = new System.Drawing.Point(3, 43);
            this.txtRawJSON.Multiline = true;
            this.txtRawJSON.Name = "txtRawJSON";
            this.txtRawJSON.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRawJSON.Size = new System.Drawing.Size(229, 240);
            this.txtRawJSON.TabIndex = 1;
            this.myToolTip.SetToolTip(this.txtRawJSON, "Drag & Drop JSON file here\r\nDouble-Click to Parse JSON");
            this.txtRawJSON.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtRawJSON_DragDrop);
            this.txtRawJSON.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtRawJSON_DragEnter);
            this.txtRawJSON.DoubleClick += new System.EventHandler(this.txtRawJSON_DoubleClick);
            // 
            // tableJSONParse
            // 
            this.tableJSONParse.ColumnCount = 1;
            this.tableJSONParse.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableJSONParse.Controls.Add(this.splitJsonParse, 0, 0);
            this.tableJSONParse.Controls.Add(this.panelJSONPath, 0, 1);
            this.tableJSONParse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableJSONParse.Location = new System.Drawing.Point(0, 0);
            this.tableJSONParse.Margin = new System.Windows.Forms.Padding(0);
            this.tableJSONParse.Name = "tableJSONParse";
            this.tableJSONParse.RowCount = 2;
            this.tableJSONParse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableJSONParse.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableJSONParse.Size = new System.Drawing.Size(466, 286);
            this.tableJSONParse.TabIndex = 0;
            // 
            // splitJsonParse
            // 
            this.splitJsonParse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitJsonParse.Location = new System.Drawing.Point(0, 0);
            this.splitJsonParse.Margin = new System.Windows.Forms.Padding(0);
            this.splitJsonParse.Name = "splitJsonParse";
            // 
            // splitJsonParse.Panel1
            // 
            this.splitJsonParse.Panel1.Controls.Add(this.tvJSON);
            // 
            // splitJsonParse.Panel2
            // 
            this.splitJsonParse.Panel2.Controls.Add(this.txtJSONPathResult);
            this.splitJsonParse.Size = new System.Drawing.Size(466, 246);
            this.splitJsonParse.SplitterDistance = 320;
            this.splitJsonParse.TabIndex = 0;
            // 
            // tvJSON
            // 
            this.tvJSON.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvJSON.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.tvJSON.Location = new System.Drawing.Point(0, 0);
            this.tvJSON.Name = "tvJSON";
            this.tvJSON.Size = new System.Drawing.Size(320, 246);
            this.tvJSON.TabIndex = 0;
            this.tvJSON.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvJSON_NodeMouseClick);
            // 
            // txtJSONPathResult
            // 
            this.txtJSONPathResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJSONPathResult.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.txtJSONPathResult.Location = new System.Drawing.Point(0, 0);
            this.txtJSONPathResult.Multiline = true;
            this.txtJSONPathResult.Name = "txtJSONPathResult";
            this.txtJSONPathResult.ReadOnly = true;
            this.txtJSONPathResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJSONPathResult.Size = new System.Drawing.Size(142, 246);
            this.txtJSONPathResult.TabIndex = 0;
            this.myToolTip.SetToolTip(this.txtJSONPathResult, "Double-Click to copy Value in Clipboard");
            this.txtJSONPathResult.DoubleClick += new System.EventHandler(this.txtJSONPathResult_DoubleClick);
            // 
            // panelJSONPath
            // 
            this.panelJSONPath.Controls.Add(this.txtJSONPath);
            this.panelJSONPath.Controls.Add(this.label2);
            this.panelJSONPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelJSONPath.Location = new System.Drawing.Point(0, 246);
            this.panelJSONPath.Margin = new System.Windows.Forms.Padding(0);
            this.panelJSONPath.Name = "panelJSONPath";
            this.panelJSONPath.Size = new System.Drawing.Size(466, 40);
            this.panelJSONPath.TabIndex = 1;
            // 
            // txtJSONPath
            // 
            this.txtJSONPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJSONPath.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtJSONPath.Location = new System.Drawing.Point(80, 7);
            this.txtJSONPath.Name = "txtJSONPath";
            this.txtJSONPath.ReadOnly = true;
            this.txtJSONPath.Size = new System.Drawing.Size(379, 27);
            this.txtJSONPath.TabIndex = 22;
            this.myToolTip.SetToolTip(this.txtJSONPath, "Double-Click to copy JSONPath in Clipboard");
            this.txtJSONPath.DoubleClick += new System.EventHandler(this.txtJSONPath_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 20);
            this.label2.TabIndex = 21;
            this.label2.Text = "JSON&Path:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "JSON (*.json)|*.json|All FIles (*.*)|*.*";
            // 
            // frmJSONPathHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 389);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmJSONPathHelper";
            this.Text = "JSONPath Helper";
            this.Load += new System.EventHandler(this.frmJSONPathHelper_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.splitBody.Panel1.ResumeLayout(false);
            this.splitBody.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBody)).EndInit();
            this.splitBody.ResumeLayout(false);
            this.tableRawJSON.ResumeLayout(false);
            this.tableRawJSON.PerformLayout();
            this.panalRawJSONButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picClear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOpenFromURL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOpenFromFile)).EndInit();
            this.tableJSONParse.ResumeLayout(false);
            this.splitJsonParse.Panel1.ResumeLayout(false);
            this.splitJsonParse.Panel2.ResumeLayout(false);
            this.splitJsonParse.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitJsonParse)).EndInit();
            this.splitJsonParse.ResumeLayout(false);
            this.panelJSONPath.ResumeLayout(false);
            this.panelJSONPath.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelFooter;
        private CustomControls.UIPictureButton uiBtnAdd;
        private CustomControls.UIPictureButton uiBtnCancel;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitBody;
        private System.Windows.Forms.TableLayoutPanel tableRawJSON;
        private System.Windows.Forms.TextBox txtRawJSON;
        private System.Windows.Forms.Panel panalRawJSONButtons;
        private System.Windows.Forms.Button btnParse;
        private System.Windows.Forms.PictureBox picOpenFromURL;
        private System.Windows.Forms.PictureBox picOpenFromFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TableLayoutPanel tableJSONParse;
        private System.Windows.Forms.SplitContainer splitJsonParse;
        private System.Windows.Forms.TreeView tvJSON;
        private System.Windows.Forms.TextBox txtJSONPathResult;
        private System.Windows.Forms.Panel panelJSONPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtJSONPath;
        private System.Windows.Forms.ToolTip myToolTip;
        private System.Windows.Forms.PictureBox picClear;
    }
}