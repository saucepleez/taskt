﻿namespace taskt.UI.Forms.ScriptBuilder.CommandEditor
{
    partial class frmCommandEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCommandEditor));
            this.cboSelectedCommand = new System.Windows.Forms.ComboBox();
            this.flw_InputVariables = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.uiBtnAdd = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.uiButtonVariable = new taskt.UI.CustomControls.UIPictureButton();
            this.btnHelpThisCommand = new taskt.UI.CustomControls.UIPictureButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiButtonVariable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHelpThisCommand)).BeginInit();
            this.SuspendLayout();
            // 
            // cboSelectedCommand
            // 
            this.cboSelectedCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSelectedCommand.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboSelectedCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectedCommand.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboSelectedCommand.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSelectedCommand.FormattingEnabled = true;
            this.cboSelectedCommand.Location = new System.Drawing.Point(6, 5);
            this.cboSelectedCommand.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cboSelectedCommand.Name = "cboSelectedCommand";
            this.cboSelectedCommand.Size = new System.Drawing.Size(619, 28);
            this.cboSelectedCommand.TabIndex = 2;
            this.cboSelectedCommand.SelectionChangeCommitted += new System.EventHandler(this.cboSelectedCommand_SelectionChangeCommitted);
            this.cboSelectedCommand.Click += new System.EventHandler(this.cboSelectedCommand_Click);
            // 
            // flw_InputVariables
            // 
            this.flw_InputVariables.AutoScroll = true;
            this.flw_InputVariables.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.flw_InputVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flw_InputVariables.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flw_InputVariables.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flw_InputVariables.Location = new System.Drawing.Point(5, 38);
            this.flw_InputVariables.Margin = new System.Windows.Forms.Padding(5);
            this.flw_InputVariables.Name = "flw_InputVariables";
            this.flw_InputVariables.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.flw_InputVariables.Size = new System.Drawing.Size(621, 487);
            this.flw_InputVariables.TabIndex = 3;
            this.flw_InputVariables.WrapContents = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.cboSelectedCommand, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flw_InputVariables, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(631, 587);
            this.tableLayoutPanel1.TabIndex = 17;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.uiBtnAdd);
            this.flowLayoutPanel1.Controls.Add(this.uiBtnCancel);
            this.flowLayoutPanel1.Controls.Add(this.uiButtonVariable);
            this.flowLayoutPanel1.Controls.Add(this.btnHelpThisCommand);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 530);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(631, 57);
            this.flowLayoutPanel1.TabIndex = 4;
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
            this.uiBtnAdd.TabIndex = 14;
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
            this.uiBtnCancel.TabIndex = 15;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // uiButtonVariable
            // 
            this.uiButtonVariable.BackColor = System.Drawing.Color.Transparent;
            this.uiButtonVariable.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiButtonVariable.DisplayText = "Variables";
            this.uiButtonVariable.DisplayTextBrush = System.Drawing.Color.White;
            this.uiButtonVariable.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiButtonVariable.Image = global::taskt.Properties.Resources.action_bar_variable;
            this.uiButtonVariable.IsMouseOver = false;
            this.uiButtonVariable.Location = new System.Drawing.Point(232, 5);
            this.uiButtonVariable.Margin = new System.Windows.Forms.Padding(32, 5, 6, 5);
            this.uiButtonVariable.Name = "uiButtonVariable";
            this.uiButtonVariable.Size = new System.Drawing.Size(58, 49);
            this.uiButtonVariable.TabIndex = 16;
            this.uiButtonVariable.TabStop = false;
            this.uiButtonVariable.Text = "Variables";
            this.uiButtonVariable.Click += new System.EventHandler(this.uiButtonVariable_Click);
            // 
            // btnHelpThisCommand
            // 
            this.btnHelpThisCommand.BackColor = System.Drawing.Color.Transparent;
            this.btnHelpThisCommand.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnHelpThisCommand.DisplayText = "Help Command";
            this.btnHelpThisCommand.DisplayTextBrush = System.Drawing.Color.White;
            this.btnHelpThisCommand.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnHelpThisCommand.Image = global::taskt.Properties.Resources.web_help;
            this.btnHelpThisCommand.IsMouseOver = false;
            this.btnHelpThisCommand.Location = new System.Drawing.Point(299, 5);
            this.btnHelpThisCommand.Margin = new System.Windows.Forms.Padding(3, 5, 6, 5);
            this.btnHelpThisCommand.Name = "btnHelpThisCommand";
            this.btnHelpThisCommand.Size = new System.Drawing.Size(88, 49);
            this.btnHelpThisCommand.TabIndex = 17;
            this.btnHelpThisCommand.TabStop = false;
            this.btnHelpThisCommand.Text = "Help Command";
            this.btnHelpThisCommand.Click += new System.EventHandler(this.btnHelpThisCommand_Click);
            // 
            // frmCommandEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(631, 587);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "frmCommandEditor";
            this.Text = "Add New Command";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCommandEditor_FormClosed);
            this.Load += new System.EventHandler(this.frmNewCommand_Load);
            this.Shown += new System.EventHandler(this.frmCommandEditor_Shown);
            this.Resize += new System.EventHandler(this.frmCommandEditor_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiButtonVariable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnHelpThisCommand)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cboSelectedCommand;
        private taskt.UI.CustomControls.UIPictureButton uiBtnCancel;
        private taskt.UI.CustomControls.UIPictureButton uiBtnAdd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        public System.Windows.Forms.FlowLayoutPanel flw_InputVariables;
        private CustomControls.UIPictureButton uiButtonVariable;
        private CustomControls.UIPictureButton btnHelpThisCommand;
    }
}