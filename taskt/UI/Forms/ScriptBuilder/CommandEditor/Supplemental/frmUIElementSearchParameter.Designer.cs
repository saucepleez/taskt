namespace taskt.UI.Forms.ScriptBuilder.CommandEditor.Supplemental
{
    partial class frmUIElementSearchParameter
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.uiBtnOK = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.dgvSearchParameters = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchParameters)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelFooter, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvSearchParameters, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(451, 385);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.uiBtnOK);
            this.panelFooter.Controls.Add(this.uiBtnCancel);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFooter.Location = new System.Drawing.Point(0, 328);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(0);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(451, 57);
            this.panelFooter.TabIndex = 0;
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
            this.uiBtnOK.Location = new System.Drawing.Point(6, 5);
            this.uiBtnOK.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.uiBtnOK.Name = "uiBtnOK";
            this.uiBtnOK.Size = new System.Drawing.Size(88, 49);
            this.uiBtnOK.TabIndex = 20;
            this.uiBtnOK.TabStop = false;
            this.uiBtnOK.Text = "Ok";
            this.uiBtnOK.Click += new System.EventHandler(this.uiBtnOK_Click);
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
            // dgvSearchParameters
            // 
            this.dgvSearchParameters.AllowUserToAddRows = false;
            this.dgvSearchParameters.AllowUserToDeleteRows = false;
            this.dgvSearchParameters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvSearchParameters.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSearchParameters.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSearchParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSearchParameters.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSearchParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSearchParameters.Location = new System.Drawing.Point(3, 3);
            this.dgvSearchParameters.Name = "dgvSearchParameters";
            this.dgvSearchParameters.RowTemplate.Height = 21;
            this.dgvSearchParameters.Size = new System.Drawing.Size(445, 322);
            this.dgvSearchParameters.TabIndex = 1;
            // 
            // frmUIElementSearchParameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 385);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmUIElementSearchParameter";
            this.Text = "UIElement Search Parameter Input";
            this.Load += new System.EventHandler(this.frmUIElementSearchParameter_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchParameters)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelFooter;
        private CustomControls.UIPictureButton uiBtnOK;
        private CustomControls.UIPictureButton uiBtnCancel;
        private System.Windows.Forms.DataGridView dgvSearchParameters;
    }
}