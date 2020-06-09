namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmThickAppElementRecorder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmThickAppElementRecorder));
            this.tlpControls = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pbRefresh = new System.Windows.Forms.PictureBox();
            this.cboWindowTitle = new System.Windows.Forms.ComboBox();
            this.pbRecord = new System.Windows.Forms.PictureBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.chkStopOnClick = new System.Windows.Forms.CheckBox();
            this.lblSubHeader = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tlpControls.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRecord)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpControls
            // 
            this.tlpControls.ColumnCount = 1;
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpControls.Controls.Add(this.pnlHeader, 0, 0);
            this.tlpControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpControls.Location = new System.Drawing.Point(0, 0);
            this.tlpControls.Name = "tlpControls";
            this.tlpControls.RowCount = 1;
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpControls.Size = new System.Drawing.Size(735, 186);
            this.tlpControls.TabIndex = 1;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlHeader.Controls.Add(this.pbRefresh);
            this.pnlHeader.Controls.Add(this.cboWindowTitle);
            this.pnlHeader.Controls.Add(this.pbRecord);
            this.pnlHeader.Controls.Add(this.lblDescription);
            this.pnlHeader.Controls.Add(this.chkStopOnClick);
            this.pnlHeader.Controls.Add(this.lblSubHeader);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(735, 190);
            this.pnlHeader.TabIndex = 1;
            // 
            // pbRefresh
            // 
            this.pbRefresh.Image = global::taskt.Properties.Resources.command_startloop;
            this.pbRefresh.Location = new System.Drawing.Point(14, 123);
            this.pbRefresh.Name = "pbRefresh";
            this.pbRefresh.Size = new System.Drawing.Size(32, 32);
            this.pbRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRefresh.TabIndex = 19;
            this.pbRefresh.TabStop = false;
            this.pbRefresh.Click += new System.EventHandler(this.pbRefresh_Click);
            // 
            // cboWindowTitle
            // 
            this.cboWindowTitle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboWindowTitle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWindowTitle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboWindowTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboWindowTitle.FormattingEnabled = true;
            this.cboWindowTitle.Location = new System.Drawing.Point(52, 127);
            this.cboWindowTitle.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cboWindowTitle.Name = "cboWindowTitle";
            this.cboWindowTitle.Size = new System.Drawing.Size(348, 28);
            this.cboWindowTitle.TabIndex = 3;
            // 
            // pbRecord
            // 
            this.pbRecord.Image = global::taskt.Properties.Resources.various_record_button;
            this.pbRecord.Location = new System.Drawing.Point(409, 123);
            this.pbRecord.Name = "pbRecord";
            this.pbRecord.Size = new System.Drawing.Size(32, 32);
            this.pbRecord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRecord.TabIndex = 4;
            this.pbRecord.TabStop = false;
            this.pbRecord.Click += new System.EventHandler(this.pbRecord_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.ForeColor = System.Drawing.Color.White;
            this.lblDescription.Location = new System.Drawing.Point(10, 69);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(778, 53);
            this.lblDescription.TabIndex = 16;
            this.lblDescription.Text = "Instructions: Select the target window name from the drop-down list and click the" +
    " record button.  Once recording has started, click the element in the target app" +
    "lication that you want to capture.\r\n ";
            // 
            // chkStopOnClick
            // 
            this.chkStopOnClick.AutoSize = true;
            this.chkStopOnClick.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStopOnClick.ForeColor = System.Drawing.Color.White;
            this.chkStopOnClick.Location = new System.Drawing.Point(18, 161);
            this.chkStopOnClick.Name = "chkStopOnClick";
            this.chkStopOnClick.Size = new System.Drawing.Size(195, 21);
            this.chkStopOnClick.TabIndex = 20;
            this.chkStopOnClick.Text = "Stop Recording on First Click";
            this.chkStopOnClick.UseVisualStyleBackColor = true;
            // 
            // lblSubHeader
            // 
            this.lblSubHeader.AutoSize = true;
            this.lblSubHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubHeader.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.lblSubHeader.Location = new System.Drawing.Point(9, 43);
            this.lblSubHeader.Name = "lblSubHeader";
            this.lblSubHeader.Size = new System.Drawing.Size(299, 21);
            this.lblSubHeader.TabIndex = 15;
            this.lblSubHeader.Text = "Capture Elements from Thick Applications";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeader.Location = new System.Drawing.Point(7, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(273, 37);
            this.lblHeader.TabIndex = 14;
            this.lblHeader.Text = "element recorder";
            // 
            // frmThickAppElementRecorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.BackgroundChangeIndex = 265;
            this.ClientSize = new System.Drawing.Size(735, 186);
            this.Controls.Add(this.tlpControls);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmThickAppElementRecorder";
            this.Text = "element recorder";
            this.Load += new System.EventHandler(this.frmThickAppElementRecorder_Load);
            this.tlpControls.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRecord)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlpControls;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.PictureBox pbRecord;
        private System.Windows.Forms.Label lblSubHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.PictureBox pbRefresh;
        public System.Windows.Forms.ComboBox cboWindowTitle;
        private System.Windows.Forms.CheckBox chkStopOnClick;
    }
}