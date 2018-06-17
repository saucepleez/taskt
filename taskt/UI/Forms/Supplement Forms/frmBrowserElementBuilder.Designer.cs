namespace taskt.UI.Forms.Supplemental
{
    partial class frmBrowserElementBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBrowserElementBuilder));
            this.cboIEWindow = new System.Windows.Forms.ComboBox();
            this.dgvSearchParameters = new System.Windows.Forms.DataGridView();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.uiBtnRefresh = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnOK = new taskt.UI.CustomControls.UIPictureButton();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchParameters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).BeginInit();
            this.SuspendLayout();
            // 
            // cboIEWindow
            // 
            this.cboIEWindow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIEWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboIEWindow.FormattingEnabled = true;
            this.cboIEWindow.Location = new System.Drawing.Point(12, 69);
            this.cboIEWindow.Name = "cboIEWindow";
            this.cboIEWindow.Size = new System.Drawing.Size(398, 28);
            this.cboIEWindow.TabIndex = 0;
            this.cboIEWindow.SelectedIndexChanged += new System.EventHandler(this.cboIEWindow_SelectedIndexChanged);
            this.cboIEWindow.SelectionChangeCommitted += new System.EventHandler(this.cboIEWindow_SelectionChangeCommitted);
            // 
            // dgvSearchParameters
            // 
            this.dgvSearchParameters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSearchParameters.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSearchParameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearchParameters.Location = new System.Drawing.Point(12, 132);
            this.dgvSearchParameters.Name = "dgvSearchParameters";
            this.dgvSearchParameters.Size = new System.Drawing.Size(516, 180);
            this.dgvSearchParameters.TabIndex = 2;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMainLogo.Location = new System.Drawing.Point(12, 19);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(337, 29);
            this.lblMainLogo.TabIndex = 5;
            this.lblMainLogo.Text = "Please select an IE Window";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(12, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 29);
            this.label1.TabIndex = 6;
            this.label1.Text = "Element Search Parameters";
            // 
            // uiBtnRefresh
            // 
            this.uiBtnRefresh.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRefresh.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRefresh.DisplayText = "Refresh";
            this.uiBtnRefresh.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiBtnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnRefresh.Image")));
            this.uiBtnRefresh.IsMouseOver = false;
            this.uiBtnRefresh.Location = new System.Drawing.Point(416, 58);
            this.uiBtnRefresh.Name = "uiBtnRefresh";
            this.uiBtnRefresh.Size = new System.Drawing.Size(48, 48);
            this.uiBtnRefresh.TabIndex = 9;
            this.uiBtnRefresh.TabStop = false;
            this.uiBtnRefresh.Click += new System.EventHandler(this.uiBtnRefresh_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(63, 318);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(48, 48);
            this.uiBtnCancel.TabIndex = 17;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // uiBtnOK
            // 
            this.uiBtnOK.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOK.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOK.DisplayText = "Ok";
            this.uiBtnOK.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOK.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOK.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOK.Image")));
            this.uiBtnOK.IsMouseOver = false;
            this.uiBtnOK.Location = new System.Drawing.Point(12, 318);
            this.uiBtnOK.Name = "uiBtnOK";
            this.uiBtnOK.Size = new System.Drawing.Size(48, 48);
            this.uiBtnOK.TabIndex = 16;
            this.uiBtnOK.TabStop = false;
            this.uiBtnOK.Click += new System.EventHandler(this.uiBtnOK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(14, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(296, 18);
            this.label3.TabIndex = 19;
            this.label3.Text = "Select a window name, then click to capture";
            // 
            // frmBrowserElementBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 205;
            this.ClientSize = new System.Drawing.Size(615, 372);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.uiBtnCancel);
            this.Controls.Add(this.uiBtnOK);
            this.Controls.Add(this.uiBtnRefresh);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMainLogo);
            this.Controls.Add(this.dgvSearchParameters);
            this.Controls.Add(this.cboIEWindow);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBrowserElementBuilder";
            this.Text = "Web Element Builder";
            this.Load += new System.EventHandler(this.frmBrowserElementBuilder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchParameters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboIEWindow;
        private System.Windows.Forms.Label lblMainLogo;
        private System.Windows.Forms.Label label1;
        private taskt.UI.CustomControls.UIPictureButton uiBtnRefresh;
        private taskt.UI.CustomControls.UIPictureButton uiBtnCancel;
        private taskt.UI.CustomControls.UIPictureButton uiBtnOK;
        public System.Windows.Forms.DataGridView dgvSearchParameters;
        private System.Windows.Forms.Label label3;
    }
}