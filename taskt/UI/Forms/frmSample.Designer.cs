
namespace taskt.UI.Forms
{
    partial class frmSample
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSample));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tvSamples = new taskt.UI.CustomControls.UITreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnNew = new taskt.UI.CustomControls.UIPictureButton();
            this.btnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.btnImport = new taskt.UI.CustomControls.UIPictureButton();
            this.btnOpen = new taskt.UI.CustomControls.UIPictureButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMain = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.picClear = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.picSearch = new System.Windows.Forms.PictureBox();
            this.txtSearchBox = new System.Windows.Forms.TextBox();
            this.tvContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tvContestSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearFilterTvContextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.rootContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rootContextSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearFilterRootContextMenuStrop = new System.Windows.Forms.ToolStripMenuItem();
            this.myToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOpen)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSearch)).BeginInit();
            this.tvContextMenuStrip.SuspendLayout();
            this.rootContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(591, 361);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tvSamples);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 103);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(585, 200);
            this.panel1.TabIndex = 17;
            // 
            // tvSamples
            // 
            this.tvSamples.BackColor = System.Drawing.Color.DimGray;
            this.tvSamples.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSamples.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.tvSamples.ForeColor = System.Drawing.Color.White;
            this.tvSamples.Location = new System.Drawing.Point(0, 0);
            this.tvSamples.Margin = new System.Windows.Forms.Padding(8);
            this.tvSamples.Name = "tvSamples";
            this.tvSamples.Size = new System.Drawing.Size(585, 200);
            this.tvSamples.TabIndex = 0;
            this.tvSamples.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvSamples_NodeMouseClick);
            this.tvSamples.DoubleClick += new System.EventHandler(this.tvSamples_DoubleClick);
            this.tvSamples.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvSamples_KeyDown);
            this.tvSamples.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvSamples_MouseClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnNew);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnImport);
            this.panel2.Controls.Add(this.btnOpen);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 306);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(591, 55);
            this.panel2.TabIndex = 0;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.Transparent;
            this.btnNew.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnNew.DisplayText = "New";
            this.btnNew.DisplayTextBrush = System.Drawing.Color.White;
            this.btnNew.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnNew.Image = global::taskt.Properties.Resources.taskt_logo_alt;
            this.btnNew.IsMouseOver = false;
            this.btnNew.Location = new System.Drawing.Point(111, 4);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(48, 48);
            this.btnNew.TabIndex = 3;
            this.btnNew.TabStop = false;
            this.btnNew.Text = "New";
            this.myToolTip.SetToolTip(this.btnNew, "Open Sample/Templete in New taskt");
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnCancel.DisplayText = "Close";
            this.btnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Image = global::taskt.Properties.Resources.various_cancel_button;
            this.btnCancel.IsMouseOver = false;
            this.btnCancel.Location = new System.Drawing.Point(200, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(48, 48);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.Transparent;
            this.btnImport.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnImport.DisplayText = "Import";
            this.btnImport.DisplayTextBrush = System.Drawing.Color.White;
            this.btnImport.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnImport.Image = global::taskt.Properties.Resources.action_bar_import;
            this.btnImport.IsMouseOver = false;
            this.btnImport.Location = new System.Drawing.Point(57, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(48, 48);
            this.btnImport.TabIndex = 1;
            this.btnImport.TabStop = false;
            this.btnImport.Text = "Import";
            this.myToolTip.SetToolTip(this.btnImport, "Import Sample/Templete");
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.Color.Transparent;
            this.btnOpen.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnOpen.DisplayText = "Open";
            this.btnOpen.DisplayTextBrush = System.Drawing.Color.White;
            this.btnOpen.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnOpen.Image = global::taskt.Properties.Resources.action_bar_open;
            this.btnOpen.IsMouseOver = false;
            this.btnOpen.Location = new System.Drawing.Point(3, 4);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(48, 48);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.TabStop = false;
            this.btnOpen.Text = "Open";
            this.myToolTip.SetToolTip(this.btnOpen, "Open Sample/Templete");
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.lblMain);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(591, 65);
            this.panel3.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(252, 20);
            this.label1.TabIndex = 17;
            this.label1.Text = "Open or Import sample script files.";
            // 
            // lblMain
            // 
            this.lblMain.AutoSize = true;
            this.lblMain.BackColor = System.Drawing.Color.Transparent;
            this.lblMain.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMain.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMain.Location = new System.Drawing.Point(3, 0);
            this.lblMain.Name = "lblMain";
            this.lblMain.Size = new System.Drawing.Size(274, 45);
            this.lblMain.TabIndex = 16;
            this.lblMain.Text = "Sample / Template";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel4.Controls.Add(this.picClear);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.picSearch);
            this.panel4.Controls.Add(this.txtSearchBox);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 65);
            this.panel4.Margin = new System.Windows.Forms.Padding(0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(591, 35);
            this.panel4.TabIndex = 1;
            // 
            // picClear
            // 
            this.picClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picClear.Image = global::taskt.Properties.Resources.command_error;
            this.picClear.Location = new System.Drawing.Point(544, 6);
            this.picClear.Name = "picClear";
            this.picClear.Size = new System.Drawing.Size(24, 24);
            this.picClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picClear.TabIndex = 19;
            this.picClear.TabStop = false;
            this.myToolTip.SetToolTip(this.picClear, "Clear Filter");
            this.picClear.Click += new System.EventHandler(this.picClear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Filter:";
            // 
            // picSearch
            // 
            this.picSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picSearch.Image = global::taskt.Properties.Resources.command_search;
            this.picSearch.Location = new System.Drawing.Point(508, 3);
            this.picSearch.Name = "picSearch";
            this.picSearch.Size = new System.Drawing.Size(30, 30);
            this.picSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSearch.TabIndex = 1;
            this.picSearch.TabStop = false;
            this.myToolTip.SetToolTip(this.picSearch, "Search");
            this.picSearch.Click += new System.EventHandler(this.picSearch_Click);
            // 
            // txtSearchBox
            // 
            this.txtSearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchBox.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSearchBox.Location = new System.Drawing.Point(57, 3);
            this.txtSearchBox.Name = "txtSearchBox";
            this.txtSearchBox.Size = new System.Drawing.Size(444, 29);
            this.txtSearchBox.TabIndex = 1;
            this.myToolTip.SetToolTip(this.txtSearchBox, "Filter Keyword");
            this.txtSearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchBox_KeyDown);
            // 
            // tvContextMenuStrip
            // 
            this.tvContextMenuStrip.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.tvContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.importToolStripMenuItem,
            this.newWindowToolStripMenuItem,
            this.tvContestSep1,
            this.clearFilterTvContextMenuStrip});
            this.tvContextMenuStrip.Name = "tvContextMenuStrip";
            this.tvContextMenuStrip.Size = new System.Drawing.Size(178, 114);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.ToolTipText = "Open Templete/Sample";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_import;
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.importToolStripMenuItem.Text = "&Import";
            this.importToolStripMenuItem.ToolTipText = "Import Templete/Sample";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // newWindowToolStripMenuItem
            // 
            this.newWindowToolStripMenuItem.Image = global::taskt.Properties.Resources.taskt_logo_alt;
            this.newWindowToolStripMenuItem.Name = "newWindowToolStripMenuItem";
            this.newWindowToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.newWindowToolStripMenuItem.Text = "&New Window";
            this.newWindowToolStripMenuItem.ToolTipText = "Open Templete/Sample in New taskt";
            this.newWindowToolStripMenuItem.Click += new System.EventHandler(this.newWindowToolStripMenuItem_Click);
            // 
            // tvContestSep1
            // 
            this.tvContestSep1.Name = "tvContestSep1";
            this.tvContestSep1.Size = new System.Drawing.Size(174, 6);
            // 
            // clearFilterTvContextMenuStrip
            // 
            this.clearFilterTvContextMenuStrip.Enabled = false;
            this.clearFilterTvContextMenuStrip.Name = "clearFilterTvContextMenuStrip";
            this.clearFilterTvContextMenuStrip.Size = new System.Drawing.Size(177, 26);
            this.clearFilterTvContextMenuStrip.Text = "Clear Filter (&L)";
            this.clearFilterTvContextMenuStrip.Click += new System.EventHandler(this.clearFilterTvContextMenuStrip_Click);
            // 
            // rootContextMenuStrip
            // 
            this.rootContextMenuStrip.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rootContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.expandToolStripMenuItem,
            this.collapseToolStripMenuItem,
            this.rootContextSep1,
            this.clearFilterRootContextMenuStrop});
            this.rootContextMenuStrip.Name = "rootContextMenuStrip";
            this.rootContextMenuStrip.Size = new System.Drawing.Size(178, 88);
            // 
            // expandToolStripMenuItem
            // 
            this.expandToolStripMenuItem.Name = "expandToolStripMenuItem";
            this.expandToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.expandToolStripMenuItem.Text = "&Expand";
            this.expandToolStripMenuItem.Click += new System.EventHandler(this.expandToolStripMenuItem_Click);
            // 
            // collapseToolStripMenuItem
            // 
            this.collapseToolStripMenuItem.Name = "collapseToolStripMenuItem";
            this.collapseToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.collapseToolStripMenuItem.Text = "&Collapse";
            this.collapseToolStripMenuItem.Click += new System.EventHandler(this.collapseToolStripMenuItem_Click);
            // 
            // rootContextSep1
            // 
            this.rootContextSep1.Name = "rootContextSep1";
            this.rootContextSep1.Size = new System.Drawing.Size(174, 6);
            // 
            // clearFilterRootContextMenuStrop
            // 
            this.clearFilterRootContextMenuStrop.Enabled = false;
            this.clearFilterRootContextMenuStrop.Name = "clearFilterRootContextMenuStrop";
            this.clearFilterRootContextMenuStrop.Size = new System.Drawing.Size(177, 26);
            this.clearFilterRootContextMenuStrop.Text = "Clear Filter (&L)";
            this.clearFilterRootContextMenuStrop.Click += new System.EventHandler(this.clearFilterRootContextMenuStrop_Click);
            // 
            // frmSample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmSample";
            this.Text = "Sample Templete";
            this.Load += new System.EventHandler(this.frmSample_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSample_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnNew)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOpen)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSearch)).EndInit();
            this.tvContextMenuStrip.ResumeLayout(false);
            this.rootContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private CustomControls.UIPictureButton btnImport;
        private CustomControls.UIPictureButton btnOpen;
        private CustomControls.UIPictureButton btnCancel;
        private CustomControls.UITreeView tvSamples;
        private System.Windows.Forms.ContextMenuStrip tvContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblMain;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtSearchBox;
        private System.Windows.Forms.PictureBox picSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picClear;
        private System.Windows.Forms.ContextMenuStrip rootContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem;
        private CustomControls.UIPictureButton btnNew;
        private System.Windows.Forms.ToolStripMenuItem newWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator tvContestSep1;
        private System.Windows.Forms.ToolStripMenuItem clearFilterTvContextMenuStrip;
        private System.Windows.Forms.ToolStripSeparator rootContextSep1;
        private System.Windows.Forms.ToolStripMenuItem clearFilterRootContextMenuStrop;
        private System.Windows.Forms.ToolTip myToolTip;
    }
}