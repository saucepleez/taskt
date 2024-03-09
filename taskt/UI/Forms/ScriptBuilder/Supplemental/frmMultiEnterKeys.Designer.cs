namespace taskt.UI.Forms.ScriptBuilder.Supplemental
{
    partial class frmMultiEnterKeys
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMultiEnterKeys));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.uiBtnAdd = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.flowBody = new System.Windows.Forms.FlowLayoutPanel();
            this.flowWindowName = new System.Windows.Forms.FlowLayoutPanel();
            this.lblWindowName = new System.Windows.Forms.Label();
            this.lnkWindoNameVariable = new taskt.UI.CustomControls.CommandItemControl();
            this.lnkWindowNameUpToDate = new taskt.UI.CustomControls.CommandItemControl();
            this.cmbWindowName = new System.Windows.Forms.ComboBox();
            this.txtWaitTime = new System.Windows.Forms.TextBox();
            this.lnkWaitTimeVariable = new taskt.UI.CustomControls.CommandItemControl();
            this.lblWaitTimeAfter = new System.Windows.Forms.Label();
            this.lnkKeyStrokesVariable = new taskt.UI.CustomControls.CommandItemControl();
            this.txtTextToSend = new System.Windows.Forms.TextBox();
            this.lblTextToSend = new System.Windows.Forms.Label();
            this.cmbSearchMethod = new System.Windows.Forms.ComboBox();
            this.lblCompareMethod = new System.Windows.Forms.Label();
            this.flowCompareMethod = new System.Windows.Forms.FlowLayoutPanel();
            this.flowTextToSend = new System.Windows.Forms.FlowLayoutPanel();
            this.flowWaitTimeAfter = new System.Windows.Forms.FlowLayoutPanel();
            this.lnkCompareMethodVariable = new taskt.UI.CustomControls.CommandItemControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.flowBody.SuspendLayout();
            this.flowWindowName.SuspendLayout();
            this.flowCompareMethod.SuspendLayout();
            this.flowTextToSend.SuspendLayout();
            this.flowWaitTimeAfter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelFooter, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelHeader, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelBody, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(562, 507);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.uiBtnAdd);
            this.panelFooter.Controls.Add(this.uiBtnCancel);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(0, 430);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(562, 57);
            this.panelFooter.TabIndex = 0;
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
            this.uiBtnAdd.TabIndex = 20;
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
            this.uiBtnCancel.TabIndex = 21;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.lblFormTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(562, 40);
            this.panelHeader.TabIndex = 1;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.lblFormTitle.ForeColor = System.Drawing.Color.White;
            this.lblFormTitle.Location = new System.Drawing.Point(3, 5);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(163, 30);
            this.lblFormTitle.TabIndex = 2;
            this.lblFormTitle.Text = "Multi EnterKeys";
            // 
            // panelBody
            // 
            this.panelBody.AutoScroll = true;
            this.panelBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.panelBody.Controls.Add(this.flowBody);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(3, 43);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(556, 384);
            this.panelBody.TabIndex = 2;
            // 
            // flowBody
            // 
            this.flowBody.AutoScroll = true;
            this.flowBody.AutoSize = true;
            this.flowBody.BackColor = System.Drawing.Color.Transparent;
            this.flowBody.Controls.Add(this.flowWindowName);
            this.flowBody.Controls.Add(this.flowCompareMethod);
            this.flowBody.Controls.Add(this.flowTextToSend);
            this.flowBody.Controls.Add(this.flowWaitTimeAfter);
            this.flowBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowBody.Location = new System.Drawing.Point(0, 0);
            this.flowBody.Margin = new System.Windows.Forms.Padding(0);
            this.flowBody.Name = "flowBody";
            this.flowBody.Size = new System.Drawing.Size(556, 384);
            this.flowBody.TabIndex = 14;
            // 
            // flowWindowName
            // 
            this.flowWindowName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowWindowName.AutoSize = true;
            this.flowWindowName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.flowWindowName.Controls.Add(this.lblWindowName);
            this.flowWindowName.Controls.Add(this.lnkWindoNameVariable);
            this.flowWindowName.Controls.Add(this.lnkWindowNameUpToDate);
            this.flowWindowName.Controls.Add(this.cmbWindowName);
            this.flowWindowName.Location = new System.Drawing.Point(3, 3);
            this.flowWindowName.Name = "flowWindowName";
            this.flowWindowName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.flowWindowName.Size = new System.Drawing.Size(527, 105);
            this.flowWindowName.TabIndex = 13;
            // 
            // lblWindowName
            // 
            this.lblWindowName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWindowName.BackColor = System.Drawing.Color.Transparent;
            this.flowWindowName.SetFlowBreak(this.lblWindowName, true);
            this.lblWindowName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblWindowName.ForeColor = System.Drawing.Color.White;
            this.lblWindowName.Location = new System.Drawing.Point(3, 0);
            this.lblWindowName.Name = "lblWindowName";
            this.lblWindowName.Size = new System.Drawing.Size(114, 21);
            this.lblWindowName.TabIndex = 13;
            this.lblWindowName.Text = "Window Name";
            // 
            // lnkWindoNameVariable
            // 
            this.lnkWindoNameVariable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkWindoNameVariable.CommandDisplay = "Insert Variable";
            this.lnkWindoNameVariable.CommandImage = ((System.Drawing.Image)(resources.GetObject("lnkWindoNameVariable.CommandImage")));
            this.lnkWindoNameVariable.DataSource = null;
            this.lnkWindoNameVariable.DrawIcon = global::taskt.Properties.Resources.taskt_variable_helper;
            this.flowWindowName.SetFlowBreak(this.lnkWindoNameVariable, true);
            this.lnkWindoNameVariable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkWindoNameVariable.ForeColor = System.Drawing.Color.White;
            this.lnkWindoNameVariable.FunctionalDescription = null;
            this.lnkWindoNameVariable.HelperType = taskt.Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper;
            this.lnkWindoNameVariable.ImplementationDescription = null;
            this.lnkWindoNameVariable.Location = new System.Drawing.Point(0, 21);
            this.lnkWindoNameVariable.Margin = new System.Windows.Forms.Padding(0);
            this.lnkWindoNameVariable.Name = "lnkWindoNameVariable";
            this.lnkWindoNameVariable.Size = new System.Drawing.Size(105, 17);
            this.lnkWindoNameVariable.TabIndex = 14;
            this.lnkWindoNameVariable.Click += new System.EventHandler(this.lnkWindoNameVariable_Click);
            // 
            // lnkWindowNameUpToDate
            // 
            this.lnkWindowNameUpToDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkWindowNameUpToDate.CommandDisplay = "Up-to-date";
            this.lnkWindowNameUpToDate.CommandImage = ((System.Drawing.Image)(resources.GetObject("lnkWindowNameUpToDate.CommandImage")));
            this.lnkWindowNameUpToDate.DataSource = null;
            this.lnkWindowNameUpToDate.DrawIcon = global::taskt.Properties.Resources.taskt_command_helper;
            this.flowWindowName.SetFlowBreak(this.lnkWindowNameUpToDate, true);
            this.lnkWindowNameUpToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkWindowNameUpToDate.ForeColor = System.Drawing.Color.White;
            this.lnkWindowNameUpToDate.FunctionalDescription = null;
            this.lnkWindowNameUpToDate.HelperType = taskt.Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper;
            this.lnkWindowNameUpToDate.ImplementationDescription = null;
            this.lnkWindowNameUpToDate.Location = new System.Drawing.Point(0, 38);
            this.lnkWindowNameUpToDate.Margin = new System.Windows.Forms.Padding(0);
            this.lnkWindowNameUpToDate.Name = "lnkWindowNameUpToDate";
            this.lnkWindowNameUpToDate.Size = new System.Drawing.Size(85, 17);
            this.lnkWindowNameUpToDate.TabIndex = 16;
            this.lnkWindowNameUpToDate.Click += new System.EventHandler(this.lnkWindowNameUpToDate_Click);
            // 
            // cmbWindowName
            // 
            this.cmbWindowName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowWindowName.SetFlowBreak(this.cmbWindowName, true);
            this.cmbWindowName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbWindowName.FormattingEnabled = true;
            this.cmbWindowName.Location = new System.Drawing.Point(3, 58);
            this.cmbWindowName.Name = "cmbWindowName";
            this.cmbWindowName.Size = new System.Drawing.Size(521, 28);
            this.cmbWindowName.TabIndex = 15;
            this.cmbWindowName.Click += new System.EventHandler(this.cmbWindowName_Click);
            this.cmbWindowName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbWindowName_KeyUp);
            // 
            // txtWaitTime
            // 
            this.txtWaitTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWaitTime.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtWaitTime.Location = new System.Drawing.Point(3, 49);
            this.txtWaitTime.Name = "txtWaitTime";
            this.txtWaitTime.Size = new System.Drawing.Size(521, 27);
            this.txtWaitTime.TabIndex = 10;
            this.txtWaitTime.Text = "500";
            // 
            // lnkWaitTimeVariable
            // 
            this.lnkWaitTimeVariable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkWaitTimeVariable.CommandDisplay = "Insert Variable";
            this.lnkWaitTimeVariable.CommandImage = ((System.Drawing.Image)(resources.GetObject("lnkWaitTimeVariable.CommandImage")));
            this.lnkWaitTimeVariable.DataSource = null;
            this.lnkWaitTimeVariable.DrawIcon = global::taskt.Properties.Resources.taskt_variable_helper;
            this.lnkWaitTimeVariable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkWaitTimeVariable.ForeColor = System.Drawing.Color.White;
            this.lnkWaitTimeVariable.FunctionalDescription = null;
            this.lnkWaitTimeVariable.HelperType = taskt.Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper;
            this.lnkWaitTimeVariable.ImplementationDescription = null;
            this.lnkWaitTimeVariable.Location = new System.Drawing.Point(0, 29);
            this.lnkWaitTimeVariable.Margin = new System.Windows.Forms.Padding(0);
            this.lnkWaitTimeVariable.Name = "lnkWaitTimeVariable";
            this.lnkWaitTimeVariable.Size = new System.Drawing.Size(105, 17);
            this.lnkWaitTimeVariable.TabIndex = 9;
            this.lnkWaitTimeVariable.Click += new System.EventHandler(this.lnkWaitTimeVariable_Click);
            // 
            // lblWaitTimeAfter
            // 
            this.lblWaitTimeAfter.AutoSize = true;
            this.lblWaitTimeAfter.BackColor = System.Drawing.Color.Transparent;
            this.flowWaitTimeAfter.SetFlowBreak(this.lblWaitTimeAfter, true);
            this.lblWaitTimeAfter.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblWaitTimeAfter.ForeColor = System.Drawing.Color.White;
            this.lblWaitTimeAfter.Location = new System.Drawing.Point(3, 8);
            this.lblWaitTimeAfter.Name = "lblWaitTimeAfter";
            this.lblWaitTimeAfter.Size = new System.Drawing.Size(323, 21);
            this.lblWaitTimeAfter.TabIndex = 8;
            this.lblWaitTimeAfter.Text = "Waiting time after Keystrokes (Default is 500)";
            // 
            // lnkKeyStrokesVariable
            // 
            this.lnkKeyStrokesVariable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkKeyStrokesVariable.CommandDisplay = "Insert Variable";
            this.lnkKeyStrokesVariable.CommandImage = ((System.Drawing.Image)(resources.GetObject("lnkKeyStrokesVariable.CommandImage")));
            this.lnkKeyStrokesVariable.DataSource = null;
            this.lnkKeyStrokesVariable.DrawIcon = global::taskt.Properties.Resources.taskt_variable_helper;
            this.flowTextToSend.SetFlowBreak(this.lnkKeyStrokesVariable, true);
            this.lnkKeyStrokesVariable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkKeyStrokesVariable.ForeColor = System.Drawing.Color.White;
            this.lnkKeyStrokesVariable.FunctionalDescription = null;
            this.lnkKeyStrokesVariable.HelperType = taskt.Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper;
            this.lnkKeyStrokesVariable.ImplementationDescription = null;
            this.lnkKeyStrokesVariable.Location = new System.Drawing.Point(0, 29);
            this.lnkKeyStrokesVariable.Margin = new System.Windows.Forms.Padding(0);
            this.lnkKeyStrokesVariable.Name = "lnkKeyStrokesVariable";
            this.lnkKeyStrokesVariable.Size = new System.Drawing.Size(105, 17);
            this.lnkKeyStrokesVariable.TabIndex = 7;
            this.lnkKeyStrokesVariable.Click += new System.EventHandler(this.lnkKeyStrokesVariable_Click);
            // 
            // txtTextToSend
            // 
            this.txtTextToSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTextToSend.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtTextToSend.Location = new System.Drawing.Point(3, 49);
            this.txtTextToSend.Multiline = true;
            this.txtTextToSend.Name = "txtTextToSend";
            this.txtTextToSend.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTextToSend.Size = new System.Drawing.Size(521, 130);
            this.txtTextToSend.TabIndex = 6;
            // 
            // lblTextToSend
            // 
            this.lblTextToSend.AutoSize = true;
            this.lblTextToSend.BackColor = System.Drawing.Color.Transparent;
            this.flowTextToSend.SetFlowBreak(this.lblTextToSend, true);
            this.lblTextToSend.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTextToSend.ForeColor = System.Drawing.Color.White;
            this.lblTextToSend.Location = new System.Drawing.Point(3, 8);
            this.lblTextToSend.Name = "lblTextToSend";
            this.lblTextToSend.Size = new System.Drawing.Size(311, 21);
            this.lblTextToSend.TabIndex = 5;
            this.lblTextToSend.Text = "Text of Keys to Send. (Separate by NewLine)";
            // 
            // cmbSearchMethod
            // 
            this.cmbSearchMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowCompareMethod.SetFlowBreak(this.cmbSearchMethod, true);
            this.cmbSearchMethod.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbSearchMethod.FormattingEnabled = true;
            this.cmbSearchMethod.Location = new System.Drawing.Point(3, 49);
            this.cmbSearchMethod.Name = "cmbSearchMethod";
            this.cmbSearchMethod.Size = new System.Drawing.Size(521, 28);
            this.cmbSearchMethod.TabIndex = 4;
            // 
            // lblCompareMethod
            // 
            this.lblCompareMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCompareMethod.AutoSize = true;
            this.lblCompareMethod.BackColor = System.Drawing.Color.Transparent;
            this.flowCompareMethod.SetFlowBreak(this.lblCompareMethod, true);
            this.lblCompareMethod.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblCompareMethod.ForeColor = System.Drawing.Color.White;
            this.lblCompareMethod.Location = new System.Drawing.Point(3, 8);
            this.lblCompareMethod.Name = "lblCompareMethod";
            this.lblCompareMethod.Size = new System.Drawing.Size(338, 21);
            this.lblCompareMethod.TabIndex = 3;
            this.lblCompareMethod.Text = "Window Compare Method (Default is Contains)";
            // 
            // flowCompareMethod
            // 
            this.flowCompareMethod.AutoSize = true;
            this.flowCompareMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.flowCompareMethod.Controls.Add(this.lblCompareMethod);
            this.flowCompareMethod.Controls.Add(this.lnkCompareMethodVariable);
            this.flowCompareMethod.Controls.Add(this.cmbSearchMethod);
            this.flowCompareMethod.Location = new System.Drawing.Point(3, 114);
            this.flowCompareMethod.Name = "flowCompareMethod";
            this.flowCompareMethod.Padding = new System.Windows.Forms.Padding(0, 8, 0, 16);
            this.flowCompareMethod.Size = new System.Drawing.Size(527, 96);
            this.flowCompareMethod.TabIndex = 14;
            // 
            // flowTextToSend
            // 
            this.flowTextToSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowTextToSend.AutoSize = true;
            this.flowTextToSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(80)))), ((int)(((byte)(59)))));
            this.flowTextToSend.Controls.Add(this.lblTextToSend);
            this.flowTextToSend.Controls.Add(this.lnkKeyStrokesVariable);
            this.flowTextToSend.Controls.Add(this.txtTextToSend);
            this.flowTextToSend.Location = new System.Drawing.Point(3, 216);
            this.flowTextToSend.Name = "flowTextToSend";
            this.flowTextToSend.Padding = new System.Windows.Forms.Padding(0, 8, 0, 16);
            this.flowTextToSend.Size = new System.Drawing.Size(527, 198);
            this.flowTextToSend.TabIndex = 15;
            // 
            // flowWaitTimeAfter
            // 
            this.flowWaitTimeAfter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowWaitTimeAfter.AutoSize = true;
            this.flowWaitTimeAfter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(80)))));
            this.flowWaitTimeAfter.Controls.Add(this.lblWaitTimeAfter);
            this.flowWaitTimeAfter.Controls.Add(this.lnkWaitTimeVariable);
            this.flowWaitTimeAfter.Controls.Add(this.txtWaitTime);
            this.flowWaitTimeAfter.Location = new System.Drawing.Point(3, 420);
            this.flowWaitTimeAfter.Name = "flowWaitTimeAfter";
            this.flowWaitTimeAfter.Padding = new System.Windows.Forms.Padding(0, 8, 0, 16);
            this.flowWaitTimeAfter.Size = new System.Drawing.Size(527, 95);
            this.flowWaitTimeAfter.TabIndex = 16;
            // 
            // lnkCompareMethodVariable
            // 
            this.lnkCompareMethodVariable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkCompareMethodVariable.CommandDisplay = "Insert Variable";
            this.lnkCompareMethodVariable.CommandImage = ((System.Drawing.Image)(resources.GetObject("lnkCompareMethodVariable.CommandImage")));
            this.lnkCompareMethodVariable.DataSource = null;
            this.lnkCompareMethodVariable.DrawIcon = global::taskt.Properties.Resources.taskt_variable_helper;
            this.flowCompareMethod.SetFlowBreak(this.lnkCompareMethodVariable, true);
            this.lnkCompareMethodVariable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCompareMethodVariable.ForeColor = System.Drawing.Color.White;
            this.lnkCompareMethodVariable.FunctionalDescription = null;
            this.lnkCompareMethodVariable.HelperType = taskt.Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper;
            this.lnkCompareMethodVariable.ImplementationDescription = null;
            this.lnkCompareMethodVariable.Location = new System.Drawing.Point(0, 29);
            this.lnkCompareMethodVariable.Margin = new System.Windows.Forms.Padding(0);
            this.lnkCompareMethodVariable.Name = "lnkCompareMethodVariable";
            this.lnkCompareMethodVariable.Size = new System.Drawing.Size(105, 17);
            this.lnkCompareMethodVariable.TabIndex = 15;
            this.lnkCompareMethodVariable.Click += new System.EventHandler(this.lnkCompareMethodVariable_Click);
            // 
            // frmMultiEnterKeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 507);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMultiEnterKeys";
            this.Text = "Multi EnterKeys";
            this.Load += new System.EventHandler(this.frmMultiSendKeystrokes_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.flowBody.ResumeLayout(false);
            this.flowBody.PerformLayout();
            this.flowWindowName.ResumeLayout(false);
            this.flowCompareMethod.ResumeLayout(false);
            this.flowCompareMethod.PerformLayout();
            this.flowTextToSend.ResumeLayout(false);
            this.flowTextToSend.PerformLayout();
            this.flowWaitTimeAfter.ResumeLayout(false);
            this.flowWaitTimeAfter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelFooter;
        private CustomControls.UIPictureButton uiBtnAdd;
        private CustomControls.UIPictureButton uiBtnCancel;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.FlowLayoutPanel flowBody;
        private System.Windows.Forms.FlowLayoutPanel flowWindowName;
        private System.Windows.Forms.Label lblWindowName;
        private CustomControls.CommandItemControl lnkWindoNameVariable;
        private CustomControls.CommandItemControl lnkWindowNameUpToDate;
        private System.Windows.Forms.ComboBox cmbWindowName;
        private System.Windows.Forms.TextBox txtWaitTime;
        private CustomControls.CommandItemControl lnkWaitTimeVariable;
        private System.Windows.Forms.Label lblWaitTimeAfter;
        private CustomControls.CommandItemControl lnkKeyStrokesVariable;
        private System.Windows.Forms.TextBox txtTextToSend;
        private System.Windows.Forms.Label lblTextToSend;
        private System.Windows.Forms.ComboBox cmbSearchMethod;
        private System.Windows.Forms.Label lblCompareMethod;
        private System.Windows.Forms.FlowLayoutPanel flowCompareMethod;
        private System.Windows.Forms.FlowLayoutPanel flowTextToSend;
        private System.Windows.Forms.FlowLayoutPanel flowWaitTimeAfter;
        private CustomControls.CommandItemControl lnkCompareMethodVariable;
    }
}