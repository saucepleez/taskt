namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmGUIInspect
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
            this.panelTitle = new System.Windows.Forms.Panel();
            this.btnReload = new System.Windows.Forms.Button();
            this.cmbWindowList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelXPath = new System.Windows.Forms.Panel();
            this.chkUseAutomationIdAttr = new System.Windows.Forms.CheckBox();
            this.chkUseNameAttr = new System.Windows.Forms.CheckBox();
            this.txtXPath = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvElements = new System.Windows.Forms.TreeView();
            this.txtElementInformation = new System.Windows.Forms.TextBox();
            this.panelFooter = new System.Windows.Forms.FlowLayoutPanel();
            this.uiBtnAdd = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.panelFooterContainer = new System.Windows.Forms.Panel();
            this.lblMessage = new System.Windows.Forms.Label();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.chkElementReload = new System.Windows.Forms.CheckBox();
            this.chkXPathRelative = new System.Windows.Forms.CheckBox();
            this.chkShowInTree = new System.Windows.Forms.CheckBox();
            this.timerLabelShowTime = new System.Windows.Forms.Timer(this.components);
            this.myToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.timerElementReload = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.panelXPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.panelFooterContainer.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelXPath, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelFooter, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.panelMenu, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(569, 398);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelTitle
            // 
            this.panelTitle.Controls.Add(this.btnReload);
            this.panelTitle.Controls.Add(this.cmbWindowList);
            this.panelTitle.Controls.Add(this.label1);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTitle.Location = new System.Drawing.Point(0, 0);
            this.panelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(569, 40);
            this.panelTitle.TabIndex = 0;
            // 
            // btnReload
            // 
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.BackgroundImage = global::taskt.Properties.Resources.action_bar_restart;
            this.btnReload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReload.Location = new System.Drawing.Point(527, 6);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(30, 30);
            this.btnReload.TabIndex = 3;
            this.myToolTip.SetToolTip(this.btnReload, "up-to-date");
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // cmbWindowList
            // 
            this.cmbWindowList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbWindowList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWindowList.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbWindowList.FormattingEnabled = true;
            this.cmbWindowList.Location = new System.Drawing.Point(178, 9);
            this.cmbWindowList.Name = "cmbWindowList";
            this.cmbWindowList.Size = new System.Drawing.Size(344, 23);
            this.cmbWindowList.TabIndex = 2;
            this.cmbWindowList.SelectedValueChanged += new System.EventHandler(this.cmbWindowList_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "GUI Inspect Tool";
            // 
            // panelXPath
            // 
            this.panelXPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.panelXPath.Controls.Add(this.chkUseAutomationIdAttr);
            this.panelXPath.Controls.Add(this.chkUseNameAttr);
            this.panelXPath.Controls.Add(this.txtXPath);
            this.panelXPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelXPath.Location = new System.Drawing.Point(0, 261);
            this.panelXPath.Margin = new System.Windows.Forms.Padding(0);
            this.panelXPath.Name = "panelXPath";
            this.panelXPath.Size = new System.Drawing.Size(569, 80);
            this.panelXPath.TabIndex = 1;
            // 
            // chkUseAutomationIdAttr
            // 
            this.chkUseAutomationIdAttr.AutoSize = true;
            this.chkUseAutomationIdAttr.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkUseAutomationIdAttr.ForeColor = System.Drawing.Color.White;
            this.chkUseAutomationIdAttr.Location = new System.Drawing.Point(203, 47);
            this.chkUseAutomationIdAttr.Name = "chkUseAutomationIdAttr";
            this.chkUseAutomationIdAttr.Size = new System.Drawing.Size(199, 23);
            this.chkUseAutomationIdAttr.TabIndex = 2;
            this.chkUseAutomationIdAttr.Text = "Use &AutomationId Attribute";
            this.chkUseAutomationIdAttr.UseVisualStyleBackColor = true;
            this.chkUseAutomationIdAttr.CheckedChanged += new System.EventHandler(this.chkUseAutomationIdAttr_CheckedChanged);
            // 
            // chkUseNameAttr
            // 
            this.chkUseNameAttr.AutoSize = true;
            this.chkUseNameAttr.Checked = true;
            this.chkUseNameAttr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseNameAttr.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkUseNameAttr.ForeColor = System.Drawing.Color.White;
            this.chkUseNameAttr.Location = new System.Drawing.Point(31, 47);
            this.chkUseNameAttr.Name = "chkUseNameAttr";
            this.chkUseNameAttr.Size = new System.Drawing.Size(150, 23);
            this.chkUseNameAttr.TabIndex = 1;
            this.chkUseNameAttr.Text = "Use &Name Attribute";
            this.chkUseNameAttr.UseVisualStyleBackColor = true;
            this.chkUseNameAttr.CheckedChanged += new System.EventHandler(this.chkUseNameAttr_CheckedChanged);
            // 
            // txtXPath
            // 
            this.txtXPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtXPath.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtXPath.Location = new System.Drawing.Point(4, 9);
            this.txtXPath.Name = "txtXPath";
            this.txtXPath.ReadOnly = true;
            this.txtXPath.Size = new System.Drawing.Size(551, 29);
            this.txtXPath.TabIndex = 0;
            this.myToolTip.SetToolTip(this.txtXPath, "Double-Click to copy XPath in Clipboard");
            this.txtXPath.DoubleClick += new System.EventHandler(this.txtXPath_DoubleClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 73);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvElements);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtElementInformation);
            this.splitContainer1.Size = new System.Drawing.Size(563, 185);
            this.splitContainer1.SplitterDistance = 224;
            this.splitContainer1.TabIndex = 2;
            // 
            // tvElements
            // 
            this.tvElements.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvElements.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tvElements.Location = new System.Drawing.Point(0, 0);
            this.tvElements.Name = "tvElements";
            this.tvElements.Size = new System.Drawing.Size(224, 185);
            this.tvElements.TabIndex = 0;
            this.tvElements.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvElements_AfterCheck);
            this.tvElements.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvElements_NodeMouseClick);
            // 
            // txtElementInformation
            // 
            this.txtElementInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtElementInformation.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtElementInformation.Location = new System.Drawing.Point(0, 0);
            this.txtElementInformation.Multiline = true;
            this.txtElementInformation.Name = "txtElementInformation";
            this.txtElementInformation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtElementInformation.Size = new System.Drawing.Size(335, 185);
            this.txtElementInformation.TabIndex = 0;
            this.myToolTip.SetToolTip(this.txtElementInformation, "Double-Click to copy Element Result in Clipboard");
            this.txtElementInformation.DoubleClick += new System.EventHandler(this.txtElementInformation_DoubleClick);
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.uiBtnAdd);
            this.panelFooter.Controls.Add(this.uiBtnCancel);
            this.panelFooter.Controls.Add(this.panelFooterContainer);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(0, 341);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(569, 57);
            this.panelFooter.TabIndex = 3;
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
            this.uiBtnAdd.Location = new System.Drawing.Point(6, 5);
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
            this.uiBtnCancel.Location = new System.Drawing.Point(106, 5);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(88, 49);
            this.uiBtnCancel.TabIndex = 19;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // panelFooterContainer
            // 
            this.panelFooterContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFooterContainer.Controls.Add(this.lblMessage);
            this.panelFooterContainer.Location = new System.Drawing.Point(203, 3);
            this.panelFooterContainer.Name = "panelFooterContainer";
            this.panelFooterContainer.Size = new System.Drawing.Size(363, 51);
            this.panelFooterContainer.TabIndex = 20;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(126, 11);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(65, 20);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Copied!!";
            this.lblMessage.Visible = false;
            // 
            // panelMenu
            // 
            this.panelMenu.Controls.Add(this.chkElementReload);
            this.panelMenu.Controls.Add(this.chkXPathRelative);
            this.panelMenu.Controls.Add(this.chkShowInTree);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMenu.Location = new System.Drawing.Point(0, 40);
            this.panelMenu.Margin = new System.Windows.Forms.Padding(0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(569, 30);
            this.panelMenu.TabIndex = 4;
            // 
            // chkElementReload
            // 
            this.chkElementReload.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkElementReload.AutoSize = true;
            this.chkElementReload.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkElementReload.Location = new System.Drawing.Point(420, 5);
            this.chkElementReload.Name = "chkElementReload";
            this.chkElementReload.Size = new System.Drawing.Size(104, 25);
            this.chkElementReload.TabIndex = 2;
            this.chkElementReload.Text = "Auto Reload (&5s)";
            this.chkElementReload.UseVisualStyleBackColor = true;
            this.chkElementReload.CheckedChanged += new System.EventHandler(this.chkElementReload_CheckedChanged);
            // 
            // chkXPathRelative
            // 
            this.chkXPathRelative.AutoSize = true;
            this.chkXPathRelative.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkXPathRelative.ForeColor = System.Drawing.Color.White;
            this.chkXPathRelative.Location = new System.Drawing.Point(92, 7);
            this.chkXPathRelative.Name = "chkXPathRelative";
            this.chkXPathRelative.Size = new System.Drawing.Size(275, 23);
            this.chkXPathRelative.TabIndex = 1;
            this.chkXPathRelative.Text = "XPath is &Relative to the checked element";
            this.chkXPathRelative.UseVisualStyleBackColor = true;
            this.chkXPathRelative.Visible = false;
            this.chkXPathRelative.CheckedChanged += new System.EventHandler(this.chkXPathRelative_CheckedChanged);
            // 
            // chkShowInTree
            // 
            this.chkShowInTree.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkShowInTree.AutoSize = true;
            this.chkShowInTree.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkShowInTree.Location = new System.Drawing.Point(8, 5);
            this.chkShowInTree.Name = "chkShowInTree";
            this.chkShowInTree.Size = new System.Drawing.Size(78, 25);
            this.chkShowInTree.TabIndex = 0;
            this.chkShowInTree.Text = "Hide &Check";
            this.chkShowInTree.UseVisualStyleBackColor = true;
            this.chkShowInTree.CheckedChanged += new System.EventHandler(this.chkShowInTree_CheckedChanged);
            // 
            // timerLabelShowTime
            // 
            this.timerLabelShowTime.Interval = 2000;
            this.timerLabelShowTime.Tick += new System.EventHandler(this.timerLabelShowTime_Tick);
            // 
            // timerElementReload
            // 
            this.timerElementReload.Interval = 5000;
            this.timerElementReload.Tick += new System.EventHandler(this.timerElementReload_Tick);
            // 
            // frmGUIInspect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 398);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmGUIInspect";
            this.Text = "GUI Inspect Tool";
            this.Load += new System.EventHandler(this.frmGUIInspect_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.panelXPath.ResumeLayout(false);
            this.panelXPath.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.panelFooterContainer.ResumeLayout(false);
            this.panelFooterContainer.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            this.panelMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelXPath;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.FlowLayoutPanel panelFooter;
        private CustomControls.UIPictureButton uiBtnAdd;
        private CustomControls.UIPictureButton uiBtnCancel;
        private System.Windows.Forms.TextBox txtXPath;
        private System.Windows.Forms.ComboBox cmbWindowList;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.TreeView tvElements;
        private System.Windows.Forms.TextBox txtElementInformation;
        private System.Windows.Forms.Panel panelFooterContainer;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Timer timerLabelShowTime;
        private System.Windows.Forms.ToolTip myToolTip;
        private System.Windows.Forms.CheckBox chkUseAutomationIdAttr;
        private System.Windows.Forms.CheckBox chkUseNameAttr;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.CheckBox chkShowInTree;
        private System.Windows.Forms.CheckBox chkXPathRelative;
        private System.Windows.Forms.Timer timerElementReload;
        private System.Windows.Forms.CheckBox chkElementReload;
    }
}