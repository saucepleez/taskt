namespace taskt.UI.Forms
{
    partial class frmScheduleManagement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScheduleManagement));
            this.txtAppPath = new System.Windows.Forms.TextBox();
            this.cboSelectedScript = new System.Windows.Forms.ComboBox();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.lblScheduledScripts = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblScriptName = new System.Windows.Forms.Label();
            this.dgvScheduledTasks = new System.Windows.Forms.DataGridView();
            this.colTaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskLastRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskLastResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskNextRunTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colChangeTaskStatus = new System.Windows.Forms.DataGridViewButtonColumn();
            this.bgwGetSchedulingInfo = new System.ComponentModel.BackgroundWorker();
            this.tmrGetSchedulingInfo = new System.Windows.Forms.Timer(this.components);
            this.uiBtnShowScheduleManager = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnOk = new taskt.UI.CustomControls.UIPictureButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboRecurType = new System.Windows.Forms.ComboBox();
            this.lblRecurrence = new System.Windows.Forms.Label();
            this.txtRecurCount = new System.Windows.Forms.TextBox();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.rdoDoNotEnd = new System.Windows.Forms.RadioButton();
            this.rdoEndByDate = new System.Windows.Forms.RadioButton();
            this.dtEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtStartTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScheduledTasks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnShowScheduleManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAppPath
            // 
            this.txtAppPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAppPath.Location = new System.Drawing.Point(9, 240);
            this.txtAppPath.Name = "txtAppPath";
            this.txtAppPath.Size = new System.Drawing.Size(513, 24);
            this.txtAppPath.TabIndex = 0;
            // 
            // cboSelectedScript
            // 
            this.cboSelectedScript.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectedScript.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSelectedScript.FormattingEnabled = true;
            this.cboSelectedScript.Location = new System.Drawing.Point(10, 286);
            this.cboSelectedScript.Name = "cboSelectedScript";
            this.cboSelectedScript.Size = new System.Drawing.Size(334, 26);
            this.cboSelectedScript.TabIndex = 2;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(1, 4);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(152, 37);
            this.lblMainLogo.TabIndex = 4;
            this.lblMainLogo.Text = "schedule";
            // 
            // lblScheduledScripts
            // 
            this.lblScheduledScripts.AutoSize = true;
            this.lblScheduledScripts.BackColor = System.Drawing.Color.Transparent;
            this.lblScheduledScripts.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScheduledScripts.ForeColor = System.Drawing.Color.AliceBlue;
            this.lblScheduledScripts.Location = new System.Drawing.Point(6, 36);
            this.lblScheduledScripts.Name = "lblScheduledScripts";
            this.lblScheduledScripts.Size = new System.Drawing.Size(202, 21);
            this.lblScheduledScripts.TabIndex = 6;
            this.lblScheduledScripts.Text = "Currently Scheduled Scripts";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.AliceBlue;
            this.label3.Location = new System.Drawing.Point(7, 221);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "taskt.exe location (executing assembly)";
            // 
            // lblScriptName
            // 
            this.lblScriptName.AutoSize = true;
            this.lblScriptName.BackColor = System.Drawing.Color.Transparent;
            this.lblScriptName.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScriptName.ForeColor = System.Drawing.Color.White;
            this.lblScriptName.Location = new System.Drawing.Point(6, 267);
            this.lblScriptName.Name = "lblScriptName";
            this.lblScriptName.Size = new System.Drawing.Size(80, 17);
            this.lblScriptName.TabIndex = 9;
            this.lblScriptName.Text = "Script Name";
            // 
            // dgvScheduledTasks
            // 
            this.dgvScheduledTasks.AllowUserToAddRows = false;
            this.dgvScheduledTasks.AllowUserToDeleteRows = false;
            this.dgvScheduledTasks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvScheduledTasks.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvScheduledTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScheduledTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTaskName,
            this.colTaskLastRun,
            this.colTaskLastResult,
            this.colTaskNextRunTime,
            this.colTaskState,
            this.colChangeTaskStatus});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvScheduledTasks.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvScheduledTasks.Location = new System.Drawing.Point(9, 59);
            this.dgvScheduledTasks.Name = "dgvScheduledTasks";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvScheduledTasks.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvScheduledTasks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvScheduledTasks.Size = new System.Drawing.Size(612, 124);
            this.dgvScheduledTasks.TabIndex = 11;
            this.dgvScheduledTasks.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvScheduledTasks_CellContentClick);
            // 
            // colTaskName
            // 
            this.colTaskName.HeaderText = "Task Name";
            this.colTaskName.Name = "colTaskName";
            this.colTaskName.ReadOnly = true;
            this.colTaskName.Width = 95;
            // 
            // colTaskLastRun
            // 
            this.colTaskLastRun.HeaderText = "Last Run";
            this.colTaskLastRun.Name = "colTaskLastRun";
            this.colTaskLastRun.ReadOnly = true;
            this.colTaskLastRun.Width = 81;
            // 
            // colTaskLastResult
            // 
            this.colTaskLastResult.HeaderText = "Last Result";
            this.colTaskLastResult.Name = "colTaskLastResult";
            this.colTaskLastResult.ReadOnly = true;
            this.colTaskLastResult.Width = 93;
            // 
            // colTaskNextRunTime
            // 
            this.colTaskNextRunTime.HeaderText = "Next Run";
            this.colTaskNextRunTime.Name = "colTaskNextRunTime";
            this.colTaskNextRunTime.ReadOnly = true;
            this.colTaskNextRunTime.Width = 83;
            // 
            // colTaskState
            // 
            this.colTaskState.HeaderText = "Active";
            this.colTaskState.Name = "colTaskState";
            this.colTaskState.ReadOnly = true;
            this.colTaskState.Width = 63;
            // 
            // colChangeTaskStatus
            // 
            this.colChangeTaskStatus.HeaderText = "Update";
            this.colChangeTaskStatus.Name = "colChangeTaskStatus";
            this.colChangeTaskStatus.ReadOnly = true;
            this.colChangeTaskStatus.Width = 53;
            // 
            // bgwGetSchedulingInfo
            // 
            this.bgwGetSchedulingInfo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwGetSchedulingInfo_DoWork);
            this.bgwGetSchedulingInfo.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwGetSchedulingInfo_RunWorkerCompleted);
            // 
            // tmrGetSchedulingInfo
            // 
            this.tmrGetSchedulingInfo.Enabled = true;
            this.tmrGetSchedulingInfo.Interval = 1000;
            this.tmrGetSchedulingInfo.Tick += new System.EventHandler(this.tmrGetSchedulingInfo_Tick);
            // 
            // uiBtnShowScheduleManager
            // 
            this.uiBtnShowScheduleManager.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnShowScheduleManager.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnShowScheduleManager.DisplayText = "Launch Task Scheduler";
            this.uiBtnShowScheduleManager.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnShowScheduleManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnShowScheduleManager.Image = global::taskt.Properties.Resources.command_files;
            this.uiBtnShowScheduleManager.IsMouseOver = false;
            this.uiBtnShowScheduleManager.Location = new System.Drawing.Point(469, 4);
            this.uiBtnShowScheduleManager.Name = "uiBtnShowScheduleManager";
            this.uiBtnShowScheduleManager.Size = new System.Drawing.Size(152, 52);
            this.uiBtnShowScheduleManager.TabIndex = 14;
            this.uiBtnShowScheduleManager.TabStop = false;
            this.uiBtnShowScheduleManager.Click += new System.EventHandler(this.uiBtnShowScheduleManager_Click);
            // 
            // uiBtnOk
            // 
            this.uiBtnOk.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOk.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOk.DisplayText = "Add";
            this.uiBtnOk.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnOk.Image = global::taskt.Properties.Resources.logo;
            this.uiBtnOk.IsMouseOver = false;
            this.uiBtnOk.Location = new System.Drawing.Point(7, 423);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(55, 48);
            this.uiBtnOk.TabIndex = 1;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Click += new System.EventHandler(this.uiBtnOk_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.cboRecurType);
            this.panel1.Controls.Add(this.lblRecurrence);
            this.panel1.Controls.Add(this.txtRecurCount);
            this.panel1.Controls.Add(this.lblStartDate);
            this.panel1.Controls.Add(this.rdoDoNotEnd);
            this.panel1.Controls.Add(this.rdoEndByDate);
            this.panel1.Controls.Add(this.dtEndTime);
            this.panel1.Controls.Add(this.dtStartTime);
            this.panel1.Location = new System.Drawing.Point(8, 318);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 106);
            this.panel1.TabIndex = 15;
            // 
            // cboRecurType
            // 
            this.cboRecurType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRecurType.FormattingEnabled = true;
            this.cboRecurType.Items.AddRange(new object[] {
            "Minutes",
            "Hours",
            "Days"});
            this.cboRecurType.Location = new System.Drawing.Point(215, 19);
            this.cboRecurType.Name = "cboRecurType";
            this.cboRecurType.Size = new System.Drawing.Size(158, 23);
            this.cboRecurType.TabIndex = 7;
            // 
            // lblRecurrence
            // 
            this.lblRecurrence.AutoSize = true;
            this.lblRecurrence.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecurrence.ForeColor = System.Drawing.Color.White;
            this.lblRecurrence.Location = new System.Drawing.Point(176, 0);
            this.lblRecurrence.Name = "lblRecurrence";
            this.lblRecurrence.Size = new System.Drawing.Size(73, 17);
            this.lblRecurrence.TabIndex = 6;
            this.lblRecurrence.Text = "Recurrence";
            // 
            // txtRecurCount
            // 
            this.txtRecurCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRecurCount.Location = new System.Drawing.Point(179, 19);
            this.txtRecurCount.Name = "txtRecurCount";
            this.txtRecurCount.Size = new System.Drawing.Size(32, 21);
            this.txtRecurCount.TabIndex = 5;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartDate.ForeColor = System.Drawing.Color.White;
            this.lblStartDate.Location = new System.Drawing.Point(2, 0);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(66, 17);
            this.lblStartDate.TabIndex = 4;
            this.lblStartDate.Text = "Start Date";
            // 
            // rdoDoNotEnd
            // 
            this.rdoDoNotEnd.AutoSize = true;
            this.rdoDoNotEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoDoNotEnd.ForeColor = System.Drawing.Color.White;
            this.rdoDoNotEnd.Location = new System.Drawing.Point(5, 85);
            this.rdoDoNotEnd.Name = "rdoDoNotEnd";
            this.rdoDoNotEnd.Size = new System.Drawing.Size(95, 20);
            this.rdoDoNotEnd.TabIndex = 3;
            this.rdoDoNotEnd.TabStop = true;
            this.rdoDoNotEnd.Text = "Do Not End";
            this.rdoDoNotEnd.UseVisualStyleBackColor = true;
            // 
            // rdoEndByDate
            // 
            this.rdoEndByDate.AutoSize = true;
            this.rdoEndByDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoEndByDate.ForeColor = System.Drawing.Color.White;
            this.rdoEndByDate.Location = new System.Drawing.Point(5, 42);
            this.rdoEndByDate.Name = "rdoEndByDate";
            this.rdoEndByDate.Size = new System.Drawing.Size(101, 20);
            this.rdoEndByDate.TabIndex = 2;
            this.rdoEndByDate.TabStop = true;
            this.rdoEndByDate.Text = "End By Date";
            this.rdoEndByDate.UseVisualStyleBackColor = true;
            // 
            // dtEndTime
            // 
            this.dtEndTime.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtEndTime.CustomFormat = "MM/dd/yyyy hh:mm:ss";
            this.dtEndTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEndTime.Location = new System.Drawing.Point(5, 62);
            this.dtEndTime.Name = "dtEndTime";
            this.dtEndTime.Size = new System.Drawing.Size(155, 23);
            this.dtEndTime.TabIndex = 1;
            // 
            // dtStartTime
            // 
            this.dtStartTime.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtStartTime.CustomFormat = "MM/dd/yyyy hh:mm:ss";
            this.dtStartTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStartTime.Location = new System.Drawing.Point(5, 19);
            this.dtStartTime.Name = "dtStartTime";
            this.dtStartTime.Size = new System.Drawing.Size(155, 23);
            this.dtStartTime.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.AliceBlue;
            this.label2.Location = new System.Drawing.Point(6, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 21);
            this.label2.TabIndex = 16;
            this.label2.Text = "Add New Schedule";
            // 
            // frmScheduleManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 268;
            this.ClientSize = new System.Drawing.Size(628, 474);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.uiBtnShowScheduleManager);
            this.Controls.Add(this.dgvScheduledTasks);
            this.Controls.Add(this.lblScriptName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblScheduledScripts);
            this.Controls.Add(this.lblMainLogo);
            this.Controls.Add(this.uiBtnOk);
            this.Controls.Add(this.cboSelectedScript);
            this.Controls.Add(this.txtAppPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmScheduleManagement";
            this.Text = "Schedule";
            this.Load += new System.EventHandler(this.frmScheduleManagement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScheduledTasks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnShowScheduleManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAppPath;
        private taskt.UI.CustomControls.UIPictureButton uiBtnOk;
        private System.Windows.Forms.ComboBox cboSelectedScript;
        private System.Windows.Forms.Label lblMainLogo;
        private System.Windows.Forms.Label lblScheduledScripts;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblScriptName;
        private System.Windows.Forms.DataGridView dgvScheduledTasks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskLastRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskLastResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskNextRunTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskState;
        private System.Windows.Forms.DataGridViewButtonColumn colChangeTaskStatus;
        private System.ComponentModel.BackgroundWorker bgwGetSchedulingInfo;
        private System.Windows.Forms.Timer tmrGetSchedulingInfo;
        private taskt.UI.CustomControls.UIPictureButton uiBtnShowScheduleManager;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtStartTime;
        private System.Windows.Forms.RadioButton rdoEndByDate;
        private System.Windows.Forms.DateTimePicker dtEndTime;
        private System.Windows.Forms.RadioButton rdoDoNotEnd;
        private System.Windows.Forms.ComboBox cboRecurType;
        private System.Windows.Forms.Label lblRecurrence;
        private System.Windows.Forms.TextBox txtRecurCount;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label label2;
    }
}