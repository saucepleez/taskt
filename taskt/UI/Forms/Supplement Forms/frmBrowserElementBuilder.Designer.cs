namespace taskt.UI.Forms.Supplement_Forms
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
            this.lblParameterHeader = new System.Windows.Forms.Label();
            this.uiBtnRefresh = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnOK = new taskt.UI.CustomControls.UIPictureButton();
            this.lblDirections = new System.Windows.Forms.Label();
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
            this.cboIEWindow.Location = new System.Drawing.Point(16, 85);
            this.cboIEWindow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboIEWindow.Name = "cboIEWindow";
            this.cboIEWindow.Size = new System.Drawing.Size(529, 33);
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
            this.dgvSearchParameters.Location = new System.Drawing.Point(16, 162);
            this.dgvSearchParameters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvSearchParameters.Name = "dgvSearchParameters";
            this.dgvSearchParameters.RowHeadersWidth = 51;
            this.dgvSearchParameters.Size = new System.Drawing.Size(688, 222);
            this.dgvSearchParameters.TabIndex = 2;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMainLogo.Location = new System.Drawing.Point(16, 23);
            this.lblMainLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(410, 36);
            this.lblMainLogo.TabIndex = 5;
            this.lblMainLogo.Text = "Please select an IE Window";
            // 
            // lblParameterHeader
            // 
            this.lblParameterHeader.AutoSize = true;
            this.lblParameterHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblParameterHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParameterHeader.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblParameterHeader.Location = new System.Drawing.Point(16, 126);
            this.lblParameterHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParameterHeader.Name = "lblParameterHeader";
            this.lblParameterHeader.Size = new System.Drawing.Size(407, 36);
            this.lblParameterHeader.TabIndex = 6;
            this.lblParameterHeader.Text = "Element Search Parameters";
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
            this.uiBtnRefresh.Location = new System.Drawing.Point(555, 71);
            this.uiBtnRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uiBtnRefresh.Name = "uiBtnRefresh";
            this.uiBtnRefresh.Size = new System.Drawing.Size(64, 59);
            this.uiBtnRefresh.TabIndex = 9;
            this.uiBtnRefresh.TabStop = false;
            this.uiBtnRefresh.Text = "Refresh";
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
            this.uiBtnCancel.Location = new System.Drawing.Point(84, 391);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(64, 59);
            this.uiBtnCancel.TabIndex = 17;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
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
            this.uiBtnOK.Location = new System.Drawing.Point(16, 391);
            this.uiBtnOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uiBtnOK.Name = "uiBtnOK";
            this.uiBtnOK.Size = new System.Drawing.Size(64, 59);
            this.uiBtnOK.TabIndex = 16;
            this.uiBtnOK.TabStop = false;
            this.uiBtnOK.Text = "Ok";
            this.uiBtnOK.Click += new System.EventHandler(this.uiBtnOK_Click);
            // 
            // lblDirections
            // 
            this.lblDirections.AutoSize = true;
            this.lblDirections.BackColor = System.Drawing.Color.Transparent;
            this.lblDirections.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDirections.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblDirections.Location = new System.Drawing.Point(19, 58);
            this.lblDirections.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDirections.Name = "lblDirections";
            this.lblDirections.Size = new System.Drawing.Size(377, 24);
            this.lblDirections.TabIndex = 19;
            this.lblDirections.Text = "Select a window name, then click to capture";
            // 
            // frmBrowserElementBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 205;
            this.ClientSize = new System.Drawing.Size(820, 458);
            this.Controls.Add(this.lblDirections);
            this.Controls.Add(this.uiBtnCancel);
            this.Controls.Add(this.uiBtnOK);
            this.Controls.Add(this.uiBtnRefresh);
            this.Controls.Add(this.lblParameterHeader);
            this.Controls.Add(this.lblMainLogo);
            this.Controls.Add(this.dgvSearchParameters);
            this.Controls.Add(this.cboIEWindow);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.Label lblParameterHeader;
        private taskt.UI.CustomControls.UIPictureButton uiBtnRefresh;
        private taskt.UI.CustomControls.UIPictureButton uiBtnCancel;
        private taskt.UI.CustomControls.UIPictureButton uiBtnOK;
        public System.Windows.Forms.DataGridView dgvSearchParameters;
        private System.Windows.Forms.Label lblDirections;
    }
}