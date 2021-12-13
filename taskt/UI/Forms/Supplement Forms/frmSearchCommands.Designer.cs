
namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmSearchCommands
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchCommands));
            this.baseTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.searchTab = new taskt.UI.CustomControls.UITabControl();
            this.tabSearch = new System.Windows.Forms.TabPage();
            this.btnSearchKeywordVariable = new System.Windows.Forms.Button();
            this.btnSearchNext = new System.Windows.Forms.Button();
            this.btnSearchSearch = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkSearchBackToTop = new System.Windows.Forms.CheckBox();
            this.chkSearchCaseSensitive = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSearchTargetIsInstance = new System.Windows.Forms.CheckBox();
            this.cmbSearchInstance = new System.Windows.Forms.ComboBox();
            this.chkSearchTargetIsDisplayText = new System.Windows.Forms.CheckBox();
            this.chkSearchTargetIsComment = new System.Windows.Forms.CheckBox();
            this.chkSearchTargetIsName = new System.Windows.Forms.CheckBox();
            this.chkSearchTargetIsParameter = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSearchKeyword = new System.Windows.Forms.TextBox();
            this.tabReplace = new System.Windows.Forms.TabPage();
            this.btnReplaceReplaceVariable = new System.Windows.Forms.Button();
            this.btnReplaceSearchVariable = new System.Windows.Forms.Button();
            this.btnReplaceReplaceAll = new System.Windows.Forms.Button();
            this.btnReplaceReplace = new System.Windows.Forms.Button();
            this.btnReplaceSearch = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkReplaceBackToTop = new System.Windows.Forms.CheckBox();
            this.chkReplaceCaseSensitive = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbReplaceInstance = new System.Windows.Forms.ComboBox();
            this.radioTargetIsComment = new System.Windows.Forms.RadioButton();
            this.radioTargetIsInstance = new System.Windows.Forms.RadioButton();
            this.radioTargetIsAll = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtReplaceReplace = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtReplaceSearch = new System.Windows.Forms.TextBox();
            this.footarTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.footerButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClose = new taskt.UI.CustomControls.UIPictureButton();
            this.btnClearAndClose = new taskt.UI.CustomControls.UIPictureButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.barOpacity = new System.Windows.Forms.TrackBar();
            this.baseTableLayout.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.searchTab.SuspendLayout();
            this.tabSearch.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabReplace.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.footarTableLayout.SuspendLayout();
            this.footerButtonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClearAndClose)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // baseTableLayout
            // 
            this.baseTableLayout.BackColor = System.Drawing.Color.Transparent;
            this.baseTableLayout.ColumnCount = 1;
            this.baseTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.baseTableLayout.Controls.Add(this.pnlHeader, 0, 0);
            this.baseTableLayout.Controls.Add(this.searchTab, 0, 1);
            this.baseTableLayout.Controls.Add(this.footarTableLayout, 0, 2);
            this.baseTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.baseTableLayout.Location = new System.Drawing.Point(0, 0);
            this.baseTableLayout.Name = "baseTableLayout";
            this.baseTableLayout.RowCount = 3;
            this.baseTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.baseTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.baseTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.baseTableLayout.Size = new System.Drawing.Size(605, 401);
            this.baseTableLayout.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(3, 3);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(599, 59);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Light", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblHeader.Location = new System.Drawing.Point(6, 6);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(385, 45);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Search Replace Commands";
            // 
            // searchTab
            // 
            this.searchTab.Controls.Add(this.tabSearch);
            this.searchTab.Controls.Add(this.tabReplace);
            this.searchTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchTab.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.searchTab.Location = new System.Drawing.Point(3, 68);
            this.searchTab.Name = "searchTab";
            this.searchTab.SelectedIndex = 0;
            this.searchTab.Size = new System.Drawing.Size(599, 275);
            this.searchTab.TabIndex = 0;
            // 
            // tabSearch
            // 
            this.tabSearch.Controls.Add(this.btnSearchKeywordVariable);
            this.tabSearch.Controls.Add(this.btnSearchNext);
            this.tabSearch.Controls.Add(this.btnSearchSearch);
            this.tabSearch.Controls.Add(this.groupBox2);
            this.tabSearch.Controls.Add(this.groupBox1);
            this.tabSearch.Controls.Add(this.label9);
            this.tabSearch.Controls.Add(this.txtSearchKeyword);
            this.tabSearch.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.tabSearch.Location = new System.Drawing.Point(4, 30);
            this.tabSearch.Name = "tabSearch";
            this.tabSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabSearch.Size = new System.Drawing.Size(591, 241);
            this.tabSearch.TabIndex = 0;
            this.tabSearch.Text = "Search";
            this.tabSearch.UseVisualStyleBackColor = true;
            // 
            // btnSearchKeywordVariable
            // 
            this.btnSearchKeywordVariable.BackgroundImage = global::taskt.Properties.Resources.action_bar_variable;
            this.btnSearchKeywordVariable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearchKeywordVariable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchKeywordVariable.Location = new System.Drawing.Point(441, 16);
            this.btnSearchKeywordVariable.Name = "btnSearchKeywordVariable";
            this.btnSearchKeywordVariable.Size = new System.Drawing.Size(24, 24);
            this.btnSearchKeywordVariable.TabIndex = 2;
            this.btnSearchKeywordVariable.UseVisualStyleBackColor = true;
            this.btnSearchKeywordVariable.Click += new System.EventHandler(this.btnSearchKeywordVariable_Click);
            // 
            // btnSearchNext
            // 
            this.btnSearchNext.Location = new System.Drawing.Point(480, 64);
            this.btnSearchNext.Name = "btnSearchNext";
            this.btnSearchNext.Size = new System.Drawing.Size(93, 43);
            this.btnSearchNext.TabIndex = 4;
            this.btnSearchNext.Text = "&Next";
            this.btnSearchNext.UseVisualStyleBackColor = true;
            this.btnSearchNext.Click += new System.EventHandler(this.btnSearchNext_Click);
            // 
            // btnSearchSearch
            // 
            this.btnSearchSearch.Location = new System.Drawing.Point(480, 15);
            this.btnSearchSearch.Name = "btnSearchSearch";
            this.btnSearchSearch.Size = new System.Drawing.Size(93, 43);
            this.btnSearchSearch.TabIndex = 3;
            this.btnSearchSearch.Text = "&Search";
            this.btnSearchSearch.UseVisualStyleBackColor = true;
            this.btnSearchSearch.Click += new System.EventHandler(this.btnSearchSearch_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkSearchBackToTop);
            this.groupBox2.Controls.Add(this.chkSearchCaseSensitive);
            this.groupBox2.Location = new System.Drawing.Point(348, 140);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(180, 95);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Method";
            // 
            // chkSearchBackToTop
            // 
            this.chkSearchBackToTop.AutoSize = true;
            this.chkSearchBackToTop.Checked = true;
            this.chkSearchBackToTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSearchBackToTop.Location = new System.Drawing.Point(15, 59);
            this.chkSearchBackToTop.Name = "chkSearchBackToTop";
            this.chkSearchBackToTop.Size = new System.Drawing.Size(108, 25);
            this.chkSearchBackToTop.TabIndex = 1;
            this.chkSearchBackToTop.Text = "&Back To Top";
            this.chkSearchBackToTop.UseVisualStyleBackColor = true;
            // 
            // chkSearchCaseSensitive
            // 
            this.chkSearchCaseSensitive.AutoSize = true;
            this.chkSearchCaseSensitive.Location = new System.Drawing.Point(15, 28);
            this.chkSearchCaseSensitive.Name = "chkSearchCaseSensitive";
            this.chkSearchCaseSensitive.Size = new System.Drawing.Size(150, 25);
            this.chkSearchCaseSensitive.TabIndex = 0;
            this.chkSearchCaseSensitive.Text = "Case Sensitive (&E)";
            this.chkSearchCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSearchTargetIsInstance);
            this.groupBox1.Controls.Add(this.cmbSearchInstance);
            this.groupBox1.Controls.Add(this.chkSearchTargetIsDisplayText);
            this.groupBox1.Controls.Add(this.chkSearchTargetIsComment);
            this.groupBox1.Controls.Add(this.chkSearchTargetIsName);
            this.groupBox1.Controls.Add(this.chkSearchTargetIsParameter);
            this.groupBox1.Location = new System.Drawing.Point(10, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 185);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Target";
            // 
            // chkSearchTargetIsInstance
            // 
            this.chkSearchTargetIsInstance.AutoSize = true;
            this.chkSearchTargetIsInstance.Location = new System.Drawing.Point(15, 152);
            this.chkSearchTargetIsInstance.Name = "chkSearchTargetIsInstance";
            this.chkSearchTargetIsInstance.Size = new System.Drawing.Size(132, 25);
            this.chkSearchTargetIsInstance.TabIndex = 4;
            this.chkSearchTargetIsInstance.Text = "&Instance Name";
            this.chkSearchTargetIsInstance.UseVisualStyleBackColor = true;
            this.chkSearchTargetIsInstance.CheckedChanged += new System.EventHandler(this.chkSearchTargetIsInstance_CheckedChanged);
            // 
            // cmbSearchInstance
            // 
            this.cmbSearchInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchInstance.FormattingEnabled = true;
            this.cmbSearchInstance.Items.AddRange(new object[] {
            "DataBase",
            "DataTable",
            "Dictionary",
            "Excel",
            "IE",
            "JSON",
            "List",
            "NLG",
            "StopWatch",
            "Web Browser",
            "Word"});
            this.cmbSearchInstance.Location = new System.Drawing.Point(150, 150);
            this.cmbSearchInstance.Name = "cmbSearchInstance";
            this.cmbSearchInstance.Size = new System.Drawing.Size(173, 29);
            this.cmbSearchInstance.TabIndex = 5;
            // 
            // chkSearchTargetIsDisplayText
            // 
            this.chkSearchTargetIsDisplayText.AutoSize = true;
            this.chkSearchTargetIsDisplayText.Checked = true;
            this.chkSearchTargetIsDisplayText.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSearchTargetIsDisplayText.Location = new System.Drawing.Point(15, 121);
            this.chkSearchTargetIsDisplayText.Name = "chkSearchTargetIsDisplayText";
            this.chkSearchTargetIsDisplayText.Size = new System.Drawing.Size(110, 25);
            this.chkSearchTargetIsDisplayText.TabIndex = 3;
            this.chkSearchTargetIsDisplayText.Text = "&Display Text";
            this.chkSearchTargetIsDisplayText.UseVisualStyleBackColor = true;
            // 
            // chkSearchTargetIsComment
            // 
            this.chkSearchTargetIsComment.AutoSize = true;
            this.chkSearchTargetIsComment.Location = new System.Drawing.Point(15, 90);
            this.chkSearchTargetIsComment.Name = "chkSearchTargetIsComment";
            this.chkSearchTargetIsComment.Size = new System.Drawing.Size(98, 25);
            this.chkSearchTargetIsComment.TabIndex = 2;
            this.chkSearchTargetIsComment.Text = "&Comment";
            this.chkSearchTargetIsComment.UseVisualStyleBackColor = true;
            // 
            // chkSearchTargetIsName
            // 
            this.chkSearchTargetIsName.AutoSize = true;
            this.chkSearchTargetIsName.Location = new System.Drawing.Point(15, 59);
            this.chkSearchTargetIsName.Name = "chkSearchTargetIsName";
            this.chkSearchTargetIsName.Size = new System.Drawing.Size(148, 25);
            this.chkSearchTargetIsName.TabIndex = 1;
            this.chkSearchTargetIsName.Text = "Command &Name";
            this.chkSearchTargetIsName.UseVisualStyleBackColor = true;
            // 
            // chkSearchTargetIsParameter
            // 
            this.chkSearchTargetIsParameter.AutoSize = true;
            this.chkSearchTargetIsParameter.Checked = true;
            this.chkSearchTargetIsParameter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSearchTargetIsParameter.Location = new System.Drawing.Point(15, 28);
            this.chkSearchTargetIsParameter.Name = "chkSearchTargetIsParameter";
            this.chkSearchTargetIsParameter.Size = new System.Drawing.Size(192, 25);
            this.chkSearchTargetIsParameter.TabIndex = 0;
            this.chkSearchTargetIsParameter.Text = "&Parameters / Properties";
            this.chkSearchTargetIsParameter.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.SteelBlue;
            this.label9.Location = new System.Drawing.Point(6, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 21);
            this.label9.TabIndex = 0;
            this.label9.Text = "Search &Keyword :";
            // 
            // txtSearchKeyword
            // 
            this.txtSearchKeyword.Location = new System.Drawing.Point(148, 15);
            this.txtSearchKeyword.Name = "txtSearchKeyword";
            this.txtSearchKeyword.Size = new System.Drawing.Size(287, 29);
            this.txtSearchKeyword.TabIndex = 1;
            this.txtSearchKeyword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearchKeyword_KeyUp);
            // 
            // tabReplace
            // 
            this.tabReplace.Controls.Add(this.btnReplaceReplaceVariable);
            this.tabReplace.Controls.Add(this.btnReplaceSearchVariable);
            this.tabReplace.Controls.Add(this.btnReplaceReplaceAll);
            this.tabReplace.Controls.Add(this.btnReplaceReplace);
            this.tabReplace.Controls.Add(this.btnReplaceSearch);
            this.tabReplace.Controls.Add(this.groupBox4);
            this.tabReplace.Controls.Add(this.groupBox3);
            this.tabReplace.Controls.Add(this.label3);
            this.tabReplace.Controls.Add(this.txtReplaceReplace);
            this.tabReplace.Controls.Add(this.label2);
            this.tabReplace.Controls.Add(this.txtReplaceSearch);
            this.tabReplace.Location = new System.Drawing.Point(4, 30);
            this.tabReplace.Name = "tabReplace";
            this.tabReplace.Padding = new System.Windows.Forms.Padding(3);
            this.tabReplace.Size = new System.Drawing.Size(591, 241);
            this.tabReplace.TabIndex = 1;
            this.tabReplace.Text = "Replace";
            this.tabReplace.UseVisualStyleBackColor = true;
            // 
            // btnReplaceReplaceVariable
            // 
            this.btnReplaceReplaceVariable.BackgroundImage = global::taskt.Properties.Resources.action_bar_variable;
            this.btnReplaceReplaceVariable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReplaceReplaceVariable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReplaceReplaceVariable.Location = new System.Drawing.Point(441, 53);
            this.btnReplaceReplaceVariable.Name = "btnReplaceReplaceVariable";
            this.btnReplaceReplaceVariable.Size = new System.Drawing.Size(24, 24);
            this.btnReplaceReplaceVariable.TabIndex = 5;
            this.btnReplaceReplaceVariable.UseVisualStyleBackColor = true;
            this.btnReplaceReplaceVariable.Click += new System.EventHandler(this.btnReplaceReplaceVariable_Click);
            // 
            // btnReplaceSearchVariable
            // 
            this.btnReplaceSearchVariable.BackgroundImage = global::taskt.Properties.Resources.action_bar_variable;
            this.btnReplaceSearchVariable.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReplaceSearchVariable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReplaceSearchVariable.Location = new System.Drawing.Point(441, 16);
            this.btnReplaceSearchVariable.Name = "btnReplaceSearchVariable";
            this.btnReplaceSearchVariable.Size = new System.Drawing.Size(24, 24);
            this.btnReplaceSearchVariable.TabIndex = 2;
            this.btnReplaceSearchVariable.UseVisualStyleBackColor = true;
            this.btnReplaceSearchVariable.Click += new System.EventHandler(this.btnReplaceSearchVariable_Click);
            // 
            // btnReplaceReplaceAll
            // 
            this.btnReplaceReplaceAll.Location = new System.Drawing.Point(480, 116);
            this.btnReplaceReplaceAll.Name = "btnReplaceReplaceAll";
            this.btnReplaceReplaceAll.Size = new System.Drawing.Size(105, 43);
            this.btnReplaceReplaceAll.TabIndex = 8;
            this.btnReplaceReplaceAll.Text = "Replace &All";
            this.btnReplaceReplaceAll.UseVisualStyleBackColor = true;
            this.btnReplaceReplaceAll.Click += new System.EventHandler(this.btnReplaceReplaceAll_Click);
            // 
            // btnReplaceReplace
            // 
            this.btnReplaceReplace.Location = new System.Drawing.Point(480, 64);
            this.btnReplaceReplace.Name = "btnReplaceReplace";
            this.btnReplaceReplace.Size = new System.Drawing.Size(93, 43);
            this.btnReplaceReplace.TabIndex = 7;
            this.btnReplaceReplace.Text = "&Replace";
            this.btnReplaceReplace.UseVisualStyleBackColor = true;
            this.btnReplaceReplace.Click += new System.EventHandler(this.btnReplaceReplace_Click);
            // 
            // btnReplaceSearch
            // 
            this.btnReplaceSearch.Location = new System.Drawing.Point(480, 15);
            this.btnReplaceSearch.Name = "btnReplaceSearch";
            this.btnReplaceSearch.Size = new System.Drawing.Size(93, 43);
            this.btnReplaceSearch.TabIndex = 6;
            this.btnReplaceSearch.Text = "&Search";
            this.btnReplaceSearch.UseVisualStyleBackColor = true;
            this.btnReplaceSearch.Click += new System.EventHandler(this.btnReplaceSearch_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkReplaceBackToTop);
            this.groupBox4.Controls.Add(this.chkReplaceCaseSensitive);
            this.groupBox4.Location = new System.Drawing.Point(355, 147);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(169, 88);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Method";
            // 
            // chkReplaceBackToTop
            // 
            this.chkReplaceBackToTop.AutoSize = true;
            this.chkReplaceBackToTop.Checked = true;
            this.chkReplaceBackToTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReplaceBackToTop.Location = new System.Drawing.Point(6, 57);
            this.chkReplaceBackToTop.Name = "chkReplaceBackToTop";
            this.chkReplaceBackToTop.Size = new System.Drawing.Size(108, 25);
            this.chkReplaceBackToTop.TabIndex = 1;
            this.chkReplaceBackToTop.Text = "&Back To Top";
            this.chkReplaceBackToTop.UseVisualStyleBackColor = true;
            // 
            // chkReplaceCaseSensitive
            // 
            this.chkReplaceCaseSensitive.AutoSize = true;
            this.chkReplaceCaseSensitive.Location = new System.Drawing.Point(6, 29);
            this.chkReplaceCaseSensitive.Name = "chkReplaceCaseSensitive";
            this.chkReplaceCaseSensitive.Size = new System.Drawing.Size(150, 25);
            this.chkReplaceCaseSensitive.TabIndex = 0;
            this.chkReplaceCaseSensitive.Text = "Case Sensitive (&E)";
            this.chkReplaceCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbReplaceInstance);
            this.groupBox3.Controls.Add(this.radioTargetIsComment);
            this.groupBox3.Controls.Add(this.radioTargetIsInstance);
            this.groupBox3.Controls.Add(this.radioTargetIsAll);
            this.groupBox3.Location = new System.Drawing.Point(10, 88);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(339, 147);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Target";
            // 
            // cmbReplaceInstance
            // 
            this.cmbReplaceInstance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReplaceInstance.FormattingEnabled = true;
            this.cmbReplaceInstance.Items.AddRange(new object[] {
            "DataBase",
            "DataTable",
            "Dictionary",
            "Excel",
            "IE",
            "JSON",
            "List",
            "NLG",
            "StopWatch",
            "Web Browser",
            "Word"});
            this.cmbReplaceInstance.Location = new System.Drawing.Point(150, 59);
            this.cmbReplaceInstance.Name = "cmbReplaceInstance";
            this.cmbReplaceInstance.Size = new System.Drawing.Size(173, 29);
            this.cmbReplaceInstance.TabIndex = 2;
            // 
            // radioTargetIsComment
            // 
            this.radioTargetIsComment.AutoSize = true;
            this.radioTargetIsComment.Location = new System.Drawing.Point(13, 90);
            this.radioTargetIsComment.Name = "radioTargetIsComment";
            this.radioTargetIsComment.Size = new System.Drawing.Size(97, 25);
            this.radioTargetIsComment.TabIndex = 3;
            this.radioTargetIsComment.Text = "&Comment";
            this.radioTargetIsComment.UseVisualStyleBackColor = true;
            // 
            // radioTargetIsInstance
            // 
            this.radioTargetIsInstance.AutoSize = true;
            this.radioTargetIsInstance.Location = new System.Drawing.Point(13, 59);
            this.radioTargetIsInstance.Name = "radioTargetIsInstance";
            this.radioTargetIsInstance.Size = new System.Drawing.Size(131, 25);
            this.radioTargetIsInstance.TabIndex = 1;
            this.radioTargetIsInstance.Text = "&Instance Name";
            this.radioTargetIsInstance.UseVisualStyleBackColor = true;
            this.radioTargetIsInstance.CheckedChanged += new System.EventHandler(this.radioTargetIsInstance_CheckedChanged);
            // 
            // radioTargetIsAll
            // 
            this.radioTargetIsAll.AutoSize = true;
            this.radioTargetIsAll.Checked = true;
            this.radioTargetIsAll.Location = new System.Drawing.Point(13, 28);
            this.radioTargetIsAll.Name = "radioTargetIsAll";
            this.radioTargetIsAll.Size = new System.Drawing.Size(213, 25);
            this.radioTargetIsAll.TabIndex = 0;
            this.radioTargetIsAll.TabStop = true;
            this.radioTargetIsAll.Text = "All &Parameters / Properties";
            this.radioTargetIsAll.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(6, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "&Replace Text :";
            // 
            // txtReplaceReplace
            // 
            this.txtReplaceReplace.Location = new System.Drawing.Point(148, 50);
            this.txtReplaceReplace.Name = "txtReplaceReplace";
            this.txtReplaceReplace.Size = new System.Drawing.Size(287, 29);
            this.txtReplaceReplace.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.SteelBlue;
            this.label2.Location = new System.Drawing.Point(6, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "Search &Keyword :";
            // 
            // txtReplaceSearch
            // 
            this.txtReplaceSearch.Location = new System.Drawing.Point(148, 15);
            this.txtReplaceSearch.Name = "txtReplaceSearch";
            this.txtReplaceSearch.Size = new System.Drawing.Size(287, 29);
            this.txtReplaceSearch.TabIndex = 1;
            // 
            // footarTableLayout
            // 
            this.footarTableLayout.ColumnCount = 2;
            this.footarTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.footarTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.footarTableLayout.Controls.Add(this.footerButtonsPanel, 0, 0);
            this.footarTableLayout.Controls.Add(this.panel1, 1, 0);
            this.footarTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.footarTableLayout.Location = new System.Drawing.Point(0, 346);
            this.footarTableLayout.Margin = new System.Windows.Forms.Padding(0);
            this.footarTableLayout.Name = "footarTableLayout";
            this.footarTableLayout.RowCount = 1;
            this.footarTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.footarTableLayout.Size = new System.Drawing.Size(605, 55);
            this.footarTableLayout.TabIndex = 2;
            // 
            // footerButtonsPanel
            // 
            this.footerButtonsPanel.Controls.Add(this.btnClose);
            this.footerButtonsPanel.Controls.Add(this.btnClearAndClose);
            this.footerButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.footerButtonsPanel.Location = new System.Drawing.Point(0, 0);
            this.footerButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.footerButtonsPanel.Name = "footerButtonsPanel";
            this.footerButtonsPanel.Size = new System.Drawing.Size(300, 55);
            this.footerButtonsPanel.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnClose.DisplayText = "Close";
            this.btnClose.DisplayTextBrush = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnClose.Image = global::taskt.Properties.Resources.various_ok_button;
            this.btnClose.IsMouseOver = false;
            this.btnClose.Location = new System.Drawing.Point(3, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(123, 48);
            this.btnClose.TabIndex = 3;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnClearAndClose
            // 
            this.btnClearAndClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClearAndClose.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnClearAndClose.DisplayText = "Clear Result & Close";
            this.btnClearAndClose.DisplayTextBrush = System.Drawing.Color.White;
            this.btnClearAndClose.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnClearAndClose.Image = global::taskt.Properties.Resources.various_cancel_button;
            this.btnClearAndClose.IsMouseOver = false;
            this.btnClearAndClose.Location = new System.Drawing.Point(132, 3);
            this.btnClearAndClose.Name = "btnClearAndClose";
            this.btnClearAndClose.Size = new System.Drawing.Size(123, 48);
            this.btnClearAndClose.TabIndex = 4;
            this.btnClearAndClose.TabStop = false;
            this.btnClearAndClose.Text = "Clear Result & Close";
            this.btnClearAndClose.Click += new System.EventHandler(this.btnClearAndClose_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.barOpacity);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(300, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(305, 55);
            this.panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Opacity";
            // 
            // barOpacity
            // 
            this.barOpacity.LargeChange = 10;
            this.barOpacity.Location = new System.Drawing.Point(85, 6);
            this.barOpacity.Maximum = 100;
            this.barOpacity.Minimum = 20;
            this.barOpacity.Name = "barOpacity";
            this.barOpacity.Size = new System.Drawing.Size(213, 45);
            this.barOpacity.SmallChange = 5;
            this.barOpacity.TabIndex = 0;
            this.barOpacity.TickFrequency = 5;
            this.barOpacity.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.barOpacity.Value = 100;
            this.barOpacity.Scroll += new System.EventHandler(this.barOpacity_Scroll);
            // 
            // frmSearchCommands
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 401);
            this.Controls.Add(this.baseTableLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmSearchCommands";
            this.Text = "Search Replace Commands";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.frmSearchCommands_Activated);
            this.Deactivate += new System.EventHandler(this.frmSearchCommands_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSearchCommands_FormClosing);
            this.Load += new System.EventHandler(this.frmSearchCommands_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmSearchCommands_KeyDown);
            this.baseTableLayout.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.searchTab.ResumeLayout(false);
            this.tabSearch.ResumeLayout(false);
            this.tabSearch.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabReplace.ResumeLayout(false);
            this.tabReplace.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.footarTableLayout.ResumeLayout(false);
            this.footerButtonsPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClearAndClose)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.barOpacity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel baseTableLayout;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private CustomControls.UITabControl searchTab;
        private System.Windows.Forms.TabPage tabSearch;
        private System.Windows.Forms.TabPage tabReplace;
        private System.Windows.Forms.TextBox txtSearchKeyword;
        private System.Windows.Forms.Button btnSearchNext;
        private System.Windows.Forms.Button btnSearchSearch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkSearchBackToTop;
        private System.Windows.Forms.CheckBox chkSearchCaseSensitive;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkSearchTargetIsComment;
        private System.Windows.Forms.CheckBox chkSearchTargetIsName;
        private System.Windows.Forms.CheckBox chkSearchTargetIsParameter;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TableLayoutPanel footarTableLayout;
        private System.Windows.Forms.FlowLayoutPanel footerButtonsPanel;
        private CustomControls.UIPictureButton btnClose;
        private CustomControls.UIPictureButton btnClearAndClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar barOpacity;
        private System.Windows.Forms.CheckBox chkSearchTargetIsDisplayText;
        private System.Windows.Forms.Button btnReplaceReplaceAll;
        private System.Windows.Forms.Button btnReplaceReplace;
        private System.Windows.Forms.Button btnReplaceSearch;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox chkReplaceBackToTop;
        private System.Windows.Forms.CheckBox chkReplaceCaseSensitive;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cmbReplaceInstance;
        private System.Windows.Forms.RadioButton radioTargetIsComment;
        private System.Windows.Forms.RadioButton radioTargetIsInstance;
        private System.Windows.Forms.RadioButton radioTargetIsAll;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtReplaceReplace;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtReplaceSearch;
        private System.Windows.Forms.CheckBox chkSearchTargetIsInstance;
        private System.Windows.Forms.ComboBox cmbSearchInstance;
        private System.Windows.Forms.Button btnSearchKeywordVariable;
        private System.Windows.Forms.Button btnReplaceReplaceVariable;
        private System.Windows.Forms.Button btnReplaceSearchVariable;
    }
}