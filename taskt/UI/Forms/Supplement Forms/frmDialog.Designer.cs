namespace taskt.UI.Forms.Supplemental
{
    partial class frmDialog
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
            taskt.Core.Theme theme1 = new taskt.Core.Theme();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDialog));
            this.autoCloseTimer = new System.Windows.Forms.Timer(this.components);
            this.lblAutoClose = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlControlContainer = new taskt.UI.CustomControls.UIPanel();
            this.uiBtnOk = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlControlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // autoCloseTimer
            // 
            this.autoCloseTimer.Tick += new System.EventHandler(this.autoCloseTimer_Tick);
            // 
            // lblAutoClose
            // 
            this.lblAutoClose.AutoSize = true;
            this.lblAutoClose.BackColor = System.Drawing.Color.Transparent;
            this.lblAutoClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAutoClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoClose.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblAutoClose.Location = new System.Drawing.Point(3, 0);
            this.lblAutoClose.Name = "lblAutoClose";
            this.lblAutoClose.Size = new System.Drawing.Size(662, 21);
            this.lblAutoClose.TabIndex = 19;
            this.lblAutoClose.Text = "auto close label";
            this.lblAutoClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAutoClose.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnlControlContainer, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblAutoClose, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtMessage, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(668, 344);
            this.tableLayoutPanel1.TabIndex = 20;
            // 
            // pnlControlContainer
            // 
            this.pnlControlContainer.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlControlContainer.Controls.Add(this.uiBtnOk);
            this.pnlControlContainer.Controls.Add(this.uiBtnCancel);
            this.pnlControlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlControlContainer.Location = new System.Drawing.Point(0, 287);
            this.pnlControlContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlControlContainer.Name = "pnlControlContainer";
            this.pnlControlContainer.Size = new System.Drawing.Size(668, 57);
            this.pnlControlContainer.TabIndex = 19;
            theme1.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(50)))), ((int)(((byte)(178)))));
            theme1.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(136)))), ((int)(((byte)(204)))));
            this.pnlControlContainer.Theme = theme1;
            // 
            // uiBtnOk
            // 
            this.uiBtnOk.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOk.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOk.DisplayText = "Ok";
            this.uiBtnOk.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOk.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOk.Image = global::taskt.Properties.Resources.various_ok_button;
            this.uiBtnOk.IsMouseOver = false;
            this.uiBtnOk.Location = new System.Drawing.Point(1, 3);
            this.uiBtnOk.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(88, 45);
            this.uiBtnOk.TabIndex = 16;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Text = "Ok";
            this.uiBtnOk.Click += new System.EventHandler(this.uiBtnOk_Click);
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
            this.uiBtnCancel.Location = new System.Drawing.Point(95, 3);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(88, 45);
            this.uiBtnCancel.TabIndex = 17;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtMessage.Location = new System.Drawing.Point(3, 24);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(662, 260);
            this.txtMessage.TabIndex = 20;
            this.txtMessage.Text = "";
            this.txtMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);
            // 
            // frmDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(668, 344);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDialog";
            this.Text = "Dialog";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmDialog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDialog_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnlControlContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private taskt.UI.CustomControls.UIPictureButton uiBtnOk;
        private taskt.UI.CustomControls.UIPictureButton uiBtnCancel;
        private System.Windows.Forms.Timer autoCloseTimer;
        private System.Windows.Forms.Label lblAutoClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private taskt.UI.CustomControls.UIPanel pnlControlContainer;
        private System.Windows.Forms.RichTextBox txtMessage;
    }
}