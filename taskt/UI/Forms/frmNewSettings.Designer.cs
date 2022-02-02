namespace taskt.UI.Forms
{
    partial class frmNewSettings
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Debug");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Folder");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Script Metric");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Start Up");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Application", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Engine");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Keyword");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Parser");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Variable");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Automation Engine", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Editor");
            this.tableLayoutBase = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.uiCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnOpen = new taskt.UI.CustomControls.UIPictureButton();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.tvSettingsMenu = new System.Windows.Forms.TreeView();
            this.flowLayoutSettings = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutBase.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.tableLayoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutBase
            // 
            this.tableLayoutBase.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutBase.ColumnCount = 1;
            this.tableLayoutBase.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutBase.Controls.Add(this.panelHeader, 0, 0);
            this.tableLayoutBase.Controls.Add(this.panelFooter, 0, 2);
            this.tableLayoutBase.Controls.Add(this.tableLayoutMain, 0, 1);
            this.tableLayoutBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutBase.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutBase.Name = "tableLayoutBase";
            this.tableLayoutBase.RowCount = 3;
            this.tableLayoutBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutBase.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutBase.Size = new System.Drawing.Size(743, 450);
            this.tableLayoutBase.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Controls.Add(this.lblMainLogo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(743, 75);
            this.panelHeader.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 26);
            this.label1.TabIndex = 16;
            this.label1.Text = "Manage settings used by the application";
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(3, 0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(125, 45);
            this.lblMainLogo.TabIndex = 15;
            this.lblMainLogo.Text = "settings";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.uiCancel);
            this.panelFooter.Controls.Add(this.uiBtnOpen);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(0, 395);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(743, 55);
            this.panelFooter.TabIndex = 18;
            // 
            // uiCancel
            // 
            this.uiCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiCancel.DisplayText = "Cancel";
            this.uiCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.uiCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiCancel.Image = global::taskt.Properties.Resources.various_cancel_button;
            this.uiCancel.IsMouseOver = false;
            this.uiCancel.Location = new System.Drawing.Point(80, 0);
            this.uiCancel.Name = "uiCancel";
            this.uiCancel.Size = new System.Drawing.Size(48, 44);
            this.uiCancel.TabIndex = 17;
            this.uiCancel.TabStop = false;
            this.uiCancel.Text = "Cancel";
            // 
            // uiBtnOpen
            // 
            this.uiBtnOpen.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOpen.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOpen.DisplayText = "Ok";
            this.uiBtnOpen.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOpen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOpen.Image = global::taskt.Properties.Resources.various_ok_button;
            this.uiBtnOpen.IsMouseOver = false;
            this.uiBtnOpen.Location = new System.Drawing.Point(14, 0);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(48, 44);
            this.uiBtnOpen.TabIndex = 16;
            this.uiBtnOpen.TabStop = false;
            this.uiBtnOpen.Text = "Ok";
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.ColumnCount = 2;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Controls.Add(this.tvSettingsMenu, 0, 0);
            this.tableLayoutMain.Controls.Add(this.flowLayoutSettings, 1, 0);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(3, 78);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 1;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Size = new System.Drawing.Size(737, 314);
            this.tableLayoutMain.TabIndex = 19;
            // 
            // tvSettingsMenu
            // 
            this.tvSettingsMenu.BackColor = System.Drawing.Color.DimGray;
            this.tvSettingsMenu.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvSettingsMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSettingsMenu.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.tvSettingsMenu.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tvSettingsMenu.LineColor = System.Drawing.Color.White;
            this.tvSettingsMenu.Location = new System.Drawing.Point(3, 3);
            this.tvSettingsMenu.Name = "tvSettingsMenu";
            treeNode1.Name = "nodeDebug";
            treeNode1.Text = "Debug";
            treeNode2.Name = "nodeFolder";
            treeNode2.Text = "Folder";
            treeNode3.Name = "nodeMetric";
            treeNode3.Text = "Script Metric";
            treeNode4.Name = "nodeStartUp";
            treeNode4.Text = "Start Up";
            treeNode5.Name = "nodeApplication";
            treeNode5.Text = "Application";
            treeNode6.Name = "nodeEngine";
            treeNode6.Text = "Engine";
            treeNode7.Name = "nodeKeyword";
            treeNode7.Text = "Keyword";
            treeNode8.Name = "nodeParser";
            treeNode8.Text = "Parser";
            treeNode9.Name = "nodeVariable";
            treeNode9.Text = "Variable";
            treeNode10.Name = "nodeAutomationEngine";
            treeNode10.Text = "Automation Engine";
            treeNode11.Name = "nodeEditor";
            treeNode11.Text = "Editor";
            this.tvSettingsMenu.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode10,
            treeNode11});
            this.tvSettingsMenu.Size = new System.Drawing.Size(194, 308);
            this.tvSettingsMenu.TabIndex = 0;
            this.tvSettingsMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSettingsMenu_AfterSelect);
            // 
            // flowLayoutSettings
            // 
            this.flowLayoutSettings.AutoScroll = true;
            this.flowLayoutSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.flowLayoutSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutSettings.Location = new System.Drawing.Point(203, 3);
            this.flowLayoutSettings.Name = "flowLayoutSettings";
            this.flowLayoutSettings.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.flowLayoutSettings.Size = new System.Drawing.Size(531, 308);
            this.flowLayoutSettings.TabIndex = 1;
            // 
            // frmNewSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 450);
            this.Controls.Add(this.tableLayoutBase);
            this.Name = "frmNewSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmNewSettings_Load);
            this.tableLayoutBase.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).EndInit();
            this.tableLayoutMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutBase;
        private System.Windows.Forms.Label lblMainLogo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelFooter;
        private CustomControls.UIPictureButton uiCancel;
        private CustomControls.UIPictureButton uiBtnOpen;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private System.Windows.Forms.TreeView tvSettingsMenu;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutSettings;
    }
}