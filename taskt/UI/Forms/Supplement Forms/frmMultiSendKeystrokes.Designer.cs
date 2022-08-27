namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmMultiSendKeystrokes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMultiSendKeystrokes));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.uiBtnAdd = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.lnkWindowNameUpToDate = new taskt.UI.CustomControls.CommandItemControl();
            this.cmbWindowName = new System.Windows.Forms.ComboBox();
            this.txtWaitTime = new System.Windows.Forms.TextBox();
            this.lnkWaitTimeVariable = new taskt.UI.CustomControls.CommandItemControl();
            this.label5 = new System.Windows.Forms.Label();
            this.lnkKeyStrokesVariable = new taskt.UI.CustomControls.CommandItemControl();
            this.txtTextToSend = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbSearchMethod = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lnkWindoNameVariable = new taskt.UI.CustomControls.CommandItemControl();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.panelBody.SuspendLayout();
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
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(562, 507);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.uiBtnAdd);
            this.panelFooter.Controls.Add(this.uiBtnCancel);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(0, 450);
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
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(562, 40);
            this.panelHeader.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Emoji", 16F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "Multi SendKeystrokes";
            // 
            // panelBody
            // 
            this.panelBody.AutoScroll = true;
            this.panelBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.panelBody.Controls.Add(this.lnkWindowNameUpToDate);
            this.panelBody.Controls.Add(this.cmbWindowName);
            this.panelBody.Controls.Add(this.txtWaitTime);
            this.panelBody.Controls.Add(this.lnkWaitTimeVariable);
            this.panelBody.Controls.Add(this.label5);
            this.panelBody.Controls.Add(this.lnkKeyStrokesVariable);
            this.panelBody.Controls.Add(this.txtTextToSend);
            this.panelBody.Controls.Add(this.label4);
            this.panelBody.Controls.Add(this.cmbSearchMethod);
            this.panelBody.Controls.Add(this.label3);
            this.panelBody.Controls.Add(this.lnkWindoNameVariable);
            this.panelBody.Controls.Add(this.label2);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(3, 43);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(556, 404);
            this.panelBody.TabIndex = 2;
            // 
            // lnkWindowNameUpToDate
            // 
            this.lnkWindowNameUpToDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkWindowNameUpToDate.CommandDisplay = "Up-to-date";
            this.lnkWindowNameUpToDate.CommandImage = ((System.Drawing.Image)(resources.GetObject("lnkWindowNameUpToDate.CommandImage")));
            this.lnkWindowNameUpToDate.DataSource = null;
            this.lnkWindowNameUpToDate.DrawIcon = global::taskt.Properties.Resources.taskt_command_helper;
            this.lnkWindowNameUpToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkWindowNameUpToDate.ForeColor = System.Drawing.Color.White;
            this.lnkWindowNameUpToDate.FunctionalDescription = null;
            this.lnkWindowNameUpToDate.HelperType = taskt.Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper;
            this.lnkWindowNameUpToDate.ImplementationDescription = null;
            this.lnkWindowNameUpToDate.Location = new System.Drawing.Point(6, 50);
            this.lnkWindowNameUpToDate.Margin = new System.Windows.Forms.Padding(0);
            this.lnkWindowNameUpToDate.Name = "lnkWindowNameUpToDate";
            this.lnkWindowNameUpToDate.Size = new System.Drawing.Size(528, 18);
            this.lnkWindowNameUpToDate.TabIndex = 12;
            this.lnkWindowNameUpToDate.Click += new System.EventHandler(this.lnkWindowNameUpToDate_Click);
            // 
            // cmbWindowName
            // 
            this.cmbWindowName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbWindowName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbWindowName.FormattingEnabled = true;
            this.cmbWindowName.Location = new System.Drawing.Point(9, 69);
            this.cmbWindowName.Name = "cmbWindowName";
            this.cmbWindowName.Size = new System.Drawing.Size(521, 28);
            this.cmbWindowName.TabIndex = 11;
            this.cmbWindowName.Click += new System.EventHandler(this.cmbWindowName_Click);
            this.cmbWindowName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cmbWindowName_KeyUp);
            // 
            // txtWaitTime
            // 
            this.txtWaitTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWaitTime.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtWaitTime.Location = new System.Drawing.Point(9, 409);
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
            this.lnkWaitTimeVariable.Location = new System.Drawing.Point(5, 388);
            this.lnkWaitTimeVariable.Margin = new System.Windows.Forms.Padding(0);
            this.lnkWaitTimeVariable.Name = "lnkWaitTimeVariable";
            this.lnkWaitTimeVariable.Size = new System.Drawing.Size(528, 18);
            this.lnkWaitTimeVariable.TabIndex = 9;
            this.lnkWaitTimeVariable.Click += new System.EventHandler(this.lnkWaitTimeVariable_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(3, 367);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(323, 21);
            this.label5.TabIndex = 8;
            this.label5.Text = "Waiting time after Keystrokes (Default is 500)";
            // 
            // lnkKeyStrokesVariable
            // 
            this.lnkKeyStrokesVariable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkKeyStrokesVariable.CommandDisplay = "Insert Variable";
            this.lnkKeyStrokesVariable.CommandImage = ((System.Drawing.Image)(resources.GetObject("lnkKeyStrokesVariable.CommandImage")));
            this.lnkKeyStrokesVariable.DataSource = null;
            this.lnkKeyStrokesVariable.DrawIcon = global::taskt.Properties.Resources.taskt_variable_helper;
            this.lnkKeyStrokesVariable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkKeyStrokesVariable.ForeColor = System.Drawing.Color.White;
            this.lnkKeyStrokesVariable.FunctionalDescription = null;
            this.lnkKeyStrokesVariable.HelperType = taskt.Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper;
            this.lnkKeyStrokesVariable.ImplementationDescription = null;
            this.lnkKeyStrokesVariable.Location = new System.Drawing.Point(7, 200);
            this.lnkKeyStrokesVariable.Margin = new System.Windows.Forms.Padding(0);
            this.lnkKeyStrokesVariable.Name = "lnkKeyStrokesVariable";
            this.lnkKeyStrokesVariable.Size = new System.Drawing.Size(528, 18);
            this.lnkKeyStrokesVariable.TabIndex = 7;
            this.lnkKeyStrokesVariable.Click += new System.EventHandler(this.lnkKeyStrokesVariable_Click);
            // 
            // txtTextToSend
            // 
            this.txtTextToSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTextToSend.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtTextToSend.Location = new System.Drawing.Point(9, 221);
            this.txtTextToSend.Multiline = true;
            this.txtTextToSend.Name = "txtTextToSend";
            this.txtTextToSend.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTextToSend.Size = new System.Drawing.Size(521, 130);
            this.txtTextToSend.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(254, 21);
            this.label4.TabIndex = 5;
            this.label4.Text = "Send to Text (Separate by NewLine)";
            // 
            // cmbSearchMethod
            // 
            this.cmbSearchMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSearchMethod.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbSearchMethod.FormattingEnabled = true;
            this.cmbSearchMethod.Location = new System.Drawing.Point(9, 133);
            this.cmbSearchMethod.Name = "cmbSearchMethod";
            this.cmbSearchMethod.Size = new System.Drawing.Size(521, 28);
            this.cmbSearchMethod.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(321, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "Window Search Method (Default is Contains)";
            // 
            // lnkWindoNameVariable
            // 
            this.lnkWindoNameVariable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkWindoNameVariable.CommandDisplay = "Insert Variable";
            this.lnkWindoNameVariable.CommandImage = ((System.Drawing.Image)(resources.GetObject("lnkWindoNameVariable.CommandImage")));
            this.lnkWindoNameVariable.DataSource = null;
            this.lnkWindoNameVariable.DrawIcon = global::taskt.Properties.Resources.taskt_variable_helper;
            this.lnkWindoNameVariable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkWindoNameVariable.ForeColor = System.Drawing.Color.White;
            this.lnkWindoNameVariable.FunctionalDescription = null;
            this.lnkWindoNameVariable.HelperType = taskt.Core.Automation.Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper;
            this.lnkWindoNameVariable.ImplementationDescription = null;
            this.lnkWindoNameVariable.Location = new System.Drawing.Point(5, 30);
            this.lnkWindoNameVariable.Margin = new System.Windows.Forms.Padding(0);
            this.lnkWindoNameVariable.Name = "lnkWindoNameVariable";
            this.lnkWindoNameVariable.Size = new System.Drawing.Size(528, 18);
            this.lnkWindoNameVariable.TabIndex = 1;
            this.lnkWindoNameVariable.Click += new System.EventHandler(this.lnkWindoNameVariable_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "Window Name";
            // 
            // frmMultiSendKeystrokes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 507);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMultiSendKeystrokes";
            this.Text = "Multi SendKeystrokes";
            this.Load += new System.EventHandler(this.frmMultiSendKeystrokes_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelFooter;
        private CustomControls.UIPictureButton uiBtnAdd;
        private CustomControls.UIPictureButton uiBtnCancel;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.Label label2;
        private CustomControls.CommandItemControl lnkKeyStrokesVariable;
        private System.Windows.Forms.TextBox txtTextToSend;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbSearchMethod;
        private System.Windows.Forms.Label label3;
        private CustomControls.CommandItemControl lnkWindoNameVariable;
        private System.Windows.Forms.TextBox txtWaitTime;
        private CustomControls.CommandItemControl lnkWaitTimeVariable;
        private System.Windows.Forms.Label label5;
        private CustomControls.CommandItemControl lnkWindowNameUpToDate;
        private System.Windows.Forms.ComboBox cmbWindowName;
    }
}