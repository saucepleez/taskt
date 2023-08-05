namespace taskt.UI.Forms
{
    partial class frmScriptVariables
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScriptVariables));
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnOK = new taskt.UI.CustomControls.UIPictureButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tvScriptVariables = new taskt.UI.CustomControls.UITreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDefineName = new System.Windows.Forms.Label();
            this.uiBtnNew = new taskt.UI.CustomControls.UIPictureButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.picAdd = new System.Windows.Forms.PictureBox();
            this.picClear = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.picSearch = new System.Windows.Forms.PictureBox();
            this.txtSearchBox = new System.Windows.Forms.TextBox();
            this.editVariableContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editVariableSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearFilterEditVariableContextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.rootVariableContestMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rootVariableSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.clearFilterRootVariableContextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.myToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSearch)).BeginInit();
            this.editVariableContextMenuStrip.SuspendLayout();
            this.rootVariableContestMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(-2, -2);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(138, 45);
            this.lblMainLogo.TabIndex = 0;
            this.lblMainLogo.Text = "variables";
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
            this.uiBtnCancel.Location = new System.Drawing.Point(55, 1);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(48, 44);
            this.uiBtnCancel.TabIndex = 15;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // uiBtnOK
            // 
            this.uiBtnOK.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOK.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOK.DisplayText = "Ok";
            this.uiBtnOK.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOK.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOK.Image = global::taskt.Properties.Resources.various_ok_button;
            this.uiBtnOK.IsMouseOver = false;
            this.uiBtnOK.Location = new System.Drawing.Point(3, 1);
            this.uiBtnOK.Name = "uiBtnOK";
            this.uiBtnOK.Size = new System.Drawing.Size(48, 44);
            this.uiBtnOK.TabIndex = 14;
            this.uiBtnOK.TabStop = false;
            this.uiBtnOK.Text = "Ok";
            this.uiBtnOK.Click += new System.EventHandler(this.uiBtnOK_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tvScriptVariables, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(607, 421);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // tvScriptVariables
            // 
            this.tvScriptVariables.BackColor = System.Drawing.Color.DimGray;
            this.tvScriptVariables.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvScriptVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvScriptVariables.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvScriptVariables.ForeColor = System.Drawing.Color.White;
            this.tvScriptVariables.Location = new System.Drawing.Point(3, 128);
            this.tvScriptVariables.Name = "tvScriptVariables";
            this.tvScriptVariables.ShowLines = false;
            this.tvScriptVariables.Size = new System.Drawing.Size(601, 243);
            this.tvScriptVariables.TabIndex = 3;
            this.tvScriptVariables.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvScriptVariables_NodeMouseClick);
            this.tvScriptVariables.DoubleClick += new System.EventHandler(this.tvScriptVariables_DoubleClick);
            this.tvScriptVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvScriptVariables_KeyDown);
            this.tvScriptVariables.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvScriptVariables_MouseClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.lblDefineName);
            this.panel1.Controls.Add(this.uiBtnNew);
            this.panel1.Controls.Add(this.lblMainLogo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(607, 90);
            this.panel1.TabIndex = 0;
            // 
            // lblDefineName
            // 
            this.lblDefineName.AutoSize = true;
            this.lblDefineName.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineName.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lblDefineName.ForeColor = System.Drawing.Color.White;
            this.lblDefineName.Location = new System.Drawing.Point(86, 43);
            this.lblDefineName.Name = "lblDefineName";
            this.lblDefineName.Size = new System.Drawing.Size(494, 40);
            this.lblDefineName.TabIndex = 1;
            this.lblDefineName.Text = "Enter or Double-Click to edit existing variables.\r\nPress \'DEL\' key to delete exis" +
    "ting variables. Available right click menu too.";
            // 
            // uiBtnNew
            // 
            this.uiBtnNew.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnNew.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnNew.DisplayText = "Add";
            this.uiBtnNew.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnNew.Image = global::taskt.Properties.Resources.action_bar_add_variable;
            this.uiBtnNew.IsMouseOver = false;
            this.uiBtnNew.Location = new System.Drawing.Point(5, 42);
            this.uiBtnNew.Name = "uiBtnNew";
            this.uiBtnNew.Size = new System.Drawing.Size(75, 45);
            this.uiBtnNew.TabIndex = 13;
            this.uiBtnNew.TabStop = false;
            this.uiBtnNew.Text = "Add";
            this.myToolTip.SetToolTip(this.uiBtnNew, "Add Variable");
            this.uiBtnNew.Click += new System.EventHandler(this.uiBtnNew_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.uiBtnOK);
            this.panel2.Controls.Add(this.uiBtnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 374);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(607, 47);
            this.panel2.TabIndex = 3;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel3.Controls.Add(this.picAdd);
            this.panel3.Controls.Add(this.picClear);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.picSearch);
            this.panel3.Controls.Add(this.txtSearchBox);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.panel3.Location = new System.Drawing.Point(0, 90);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(607, 35);
            this.panel3.TabIndex = 2;
            // 
            // picAdd
            // 
            this.picAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picAdd.Image = global::taskt.Properties.Resources.action_bar_add_variable;
            this.picAdd.Location = new System.Drawing.Point(574, 2);
            this.picAdd.Name = "picAdd";
            this.picAdd.Size = new System.Drawing.Size(30, 30);
            this.picAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picAdd.TabIndex = 24;
            this.picAdd.TabStop = false;
            this.myToolTip.SetToolTip(this.picAdd, "Add Variable");
            this.picAdd.Click += new System.EventHandler(this.picAdd_Click);
            // 
            // picClear
            // 
            this.picClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picClear.Image = global::taskt.Properties.Resources.command_error;
            this.picClear.Location = new System.Drawing.Point(544, 6);
            this.picClear.Name = "picClear";
            this.picClear.Size = new System.Drawing.Size(24, 24);
            this.picClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picClear.TabIndex = 23;
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
            this.label2.Location = new System.Drawing.Point(1, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "&Filter:";
            // 
            // picSearch
            // 
            this.picSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picSearch.Image = global::taskt.Properties.Resources.command_search;
            this.picSearch.Location = new System.Drawing.Point(506, 3);
            this.picSearch.Name = "picSearch";
            this.picSearch.Size = new System.Drawing.Size(30, 30);
            this.picSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSearch.TabIndex = 21;
            this.picSearch.TabStop = false;
            this.myToolTip.SetToolTip(this.picSearch, "Search");
            this.picSearch.Click += new System.EventHandler(this.picSearch_Click);
            // 
            // txtSearchBox
            // 
            this.txtSearchBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchBox.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtSearchBox.Location = new System.Drawing.Point(55, 3);
            this.txtSearchBox.Name = "txtSearchBox";
            this.txtSearchBox.Size = new System.Drawing.Size(444, 29);
            this.txtSearchBox.TabIndex = 1;
            this.myToolTip.SetToolTip(this.txtSearchBox, "Filter Keyword");
            this.txtSearchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchBox_KeyDown);
            // 
            // editVariableContextMenuStrip
            // 
            this.editVariableContextMenuStrip.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            this.editVariableContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.editVariableSep1,
            this.clearFilterEditVariableContextMenuStrip});
            this.editVariableContextMenuStrip.Name = "editVariableContextMenuStrip";
            this.editVariableContextMenuStrip.Size = new System.Drawing.Size(178, 88);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.editToolStripMenuItem.Text = "&Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.removeToolStripMenuItem.Text = "&Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // editVariableSep1
            // 
            this.editVariableSep1.Name = "editVariableSep1";
            this.editVariableSep1.Size = new System.Drawing.Size(174, 6);
            // 
            // clearFilterEditVariableContextMenuStrip
            // 
            this.clearFilterEditVariableContextMenuStrip.Enabled = false;
            this.clearFilterEditVariableContextMenuStrip.Name = "clearFilterEditVariableContextMenuStrip";
            this.clearFilterEditVariableContextMenuStrip.Size = new System.Drawing.Size(177, 26);
            this.clearFilterEditVariableContextMenuStrip.Text = "Clear Filter (&L)";
            this.clearFilterEditVariableContextMenuStrip.Click += new System.EventHandler(this.clearFilterEditVariableContextMenuStrip_Click);
            // 
            // rootVariableContestMenuStrip
            // 
            this.rootVariableContestMenuStrip.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rootVariableContestMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.expandToolStripMenuItem,
            this.collapseToolStripMenuItem,
            this.rootVariableSep1,
            this.clearFilterRootVariableContextMenuStrip});
            this.rootVariableContestMenuStrip.Name = "rootVariableContestMenuStrip";
            this.rootVariableContestMenuStrip.Size = new System.Drawing.Size(178, 114);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Image = global::taskt.Properties.Resources.action_bar_add_variable;
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.addToolStripMenuItem.Size = new System.Drawing.Size(177, 26);
            this.addToolStripMenuItem.Text = "&Add";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
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
            // rootVariableSep1
            // 
            this.rootVariableSep1.Name = "rootVariableSep1";
            this.rootVariableSep1.Size = new System.Drawing.Size(174, 6);
            // 
            // clearFilterRootVariableContextMenuStrip
            // 
            this.clearFilterRootVariableContextMenuStrip.Enabled = false;
            this.clearFilterRootVariableContextMenuStrip.Name = "clearFilterRootVariableContextMenuStrip";
            this.clearFilterRootVariableContextMenuStrip.Size = new System.Drawing.Size(177, 26);
            this.clearFilterRootVariableContextMenuStrip.Text = "Clear Filter (&L)";
            this.clearFilterRootVariableContextMenuStrip.Click += new System.EventHandler(this.clearFilterRootVariableContextMenuStrip_Click);
            // 
            // frmScriptVariables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(607, 421);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmScriptVariables";
            this.Text = "Variables";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmScriptVariables_FormClosed);
            this.Load += new System.EventHandler(this.frmScriptVariables_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmScriptVariables_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSearch)).EndInit();
            this.editVariableContextMenuStrip.ResumeLayout(false);
            this.rootVariableContestMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblMainLogo;
        private taskt.UI.CustomControls.UIPictureButton uiBtnCancel;
        private taskt.UI.CustomControls.UIPictureButton uiBtnOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private CustomControls.UITreeView tvScriptVariables;
        private CustomControls.UIPictureButton uiBtnNew;
        private System.Windows.Forms.Label lblDefineName;
        private System.Windows.Forms.ContextMenuStrip editVariableContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox picClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox picSearch;
        private System.Windows.Forms.TextBox txtSearchBox;
        private System.Windows.Forms.PictureBox picAdd;
        private System.Windows.Forms.ContextMenuStrip rootVariableContestMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator editVariableSep1;
        private System.Windows.Forms.ToolStripMenuItem clearFilterEditVariableContextMenuStrip;
        private System.Windows.Forms.ToolStripSeparator rootVariableSep1;
        private System.Windows.Forms.ToolStripMenuItem clearFilterRootVariableContextMenuStrip;
        private System.Windows.Forms.ToolTip myToolTip;
    }
}