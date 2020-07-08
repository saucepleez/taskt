namespace taskt.UI.Forms
{
    partial class frmScriptVariables
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScriptVariables));
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.uiBtnCancel = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.uiBtnOK = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.tlpVariables = new System.Windows.Forms.TableLayoutPanel();
            this.tvScriptVariables = new taskt.UI.CustomControls.CustomUIControls.UITreeView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDefineName = new System.Windows.Forms.Label();
            this.uiBtnNew = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.pnlBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).BeginInit();
            this.tlpVariables.SuspendLayout();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(-3, -2);
            this.lblMainLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(173, 54);
            this.lblMainLogo.TabIndex = 7;
            this.lblMainLogo.Text = "variables";
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
            this.uiBtnCancel.Location = new System.Drawing.Point(73, 1);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(64, 59);
            this.uiBtnCancel.TabIndex = 15;
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
            this.uiBtnOK.Location = new System.Drawing.Point(4, 1);
            this.uiBtnOK.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnOK.Name = "uiBtnOK";
            this.uiBtnOK.Size = new System.Drawing.Size(64, 59);
            this.uiBtnOK.TabIndex = 14;
            this.uiBtnOK.TabStop = false;
            this.uiBtnOK.Text = "Ok";
            this.uiBtnOK.Click += new System.EventHandler(this.uiBtnOK_Click);
            // 
            // tlpVariables
            // 
            this.tlpVariables.BackColor = System.Drawing.Color.Transparent;
            this.tlpVariables.ColumnCount = 1;
            this.tlpVariables.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpVariables.Controls.Add(this.tvScriptVariables, 0, 1);
            this.tlpVariables.Controls.Add(this.pnlTop, 0, 0);
            this.tlpVariables.Controls.Add(this.pnlBottom, 0, 2);
            this.tlpVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpVariables.Location = new System.Drawing.Point(0, 0);
            this.tlpVariables.Margin = new System.Windows.Forms.Padding(4);
            this.tlpVariables.Name = "tlpVariables";
            this.tlpVariables.RowCount = 3;
            this.tlpVariables.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 119F));
            this.tlpVariables.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpVariables.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tlpVariables.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tlpVariables.Size = new System.Drawing.Size(809, 513);
            this.tlpVariables.TabIndex = 17;
            // 
            // tvScriptVariables
            // 
            this.tvScriptVariables.BackColor = System.Drawing.Color.DimGray;
            this.tvScriptVariables.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvScriptVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvScriptVariables.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvScriptVariables.ForeColor = System.Drawing.Color.White;
            this.tvScriptVariables.Location = new System.Drawing.Point(4, 123);
            this.tvScriptVariables.Margin = new System.Windows.Forms.Padding(4);
            this.tvScriptVariables.Name = "tvScriptVariables";
            this.tvScriptVariables.ShowLines = false;
            this.tvScriptVariables.Size = new System.Drawing.Size(801, 323);
            this.tvScriptVariables.TabIndex = 18;
            this.tvScriptVariables.DoubleClick += new System.EventHandler(this.tvScriptVariables_DoubleClick);
            this.tvScriptVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvScriptVariables_KeyDown);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.pnlTop.Controls.Add(this.lblDefineName);
            this.pnlTop.Controls.Add(this.uiBtnNew);
            this.pnlTop.Controls.Add(this.lblMainLogo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(809, 119);
            this.pnlTop.TabIndex = 18;
            // 
            // lblDefineName
            // 
            this.lblDefineName.AutoSize = true;
            this.lblDefineName.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineName.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineName.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.lblDefineName.Location = new System.Drawing.Point(73, 60);
            this.lblDefineName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineName.Name = "lblDefineName";
            this.lblDefineName.Size = new System.Drawing.Size(346, 50);
            this.lblDefineName.TabIndex = 16;
            this.lblDefineName.Text = "Double-Click to edit existing variables\r\nPress \'DEL\' key to delete existing varia" +
    "bles";
            // 
            // uiBtnNew
            // 
            this.uiBtnNew.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnNew.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnNew.DisplayText = "Add";
            this.uiBtnNew.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnNew.Image = global::taskt.Properties.Resources.action_bar_new;
            this.uiBtnNew.IsMouseOver = false;
            this.uiBtnNew.Location = new System.Drawing.Point(7, 55);
            this.uiBtnNew.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnNew.Name = "uiBtnNew";
            this.uiBtnNew.Size = new System.Drawing.Size(64, 60);
            this.uiBtnNew.TabIndex = 13;
            this.uiBtnNew.TabStop = false;
            this.uiBtnNew.Text = "Add";
            this.uiBtnNew.Click += new System.EventHandler(this.uiBtnNew_Click);
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.Transparent;
            this.pnlBottom.Controls.Add(this.uiBtnOK);
            this.pnlBottom.Controls.Add(this.uiBtnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(0, 450);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(809, 63);
            this.pnlBottom.TabIndex = 19;
            this.pnlBottom.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBottom_Paint);
            // 
            // frmScriptVariables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(809, 513);
            this.Controls.Add(this.tlpVariables);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmScriptVariables";
            this.Text = "Variables";
            this.Load += new System.EventHandler(this.frmScriptVariables_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).EndInit();
            this.tlpVariables.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblMainLogo;
        private taskt.UI.CustomControls.CustomUIControls.UIPictureButton uiBtnCancel;
        private taskt.UI.CustomControls.CustomUIControls.UIPictureButton uiBtnOK;
        private System.Windows.Forms.TableLayoutPanel tlpVariables;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private CustomControls.CustomUIControls.UITreeView tvScriptVariables;
        private CustomControls.CustomUIControls.UIPictureButton uiBtnNew;
        private System.Windows.Forms.Label lblDefineName;
    }
}