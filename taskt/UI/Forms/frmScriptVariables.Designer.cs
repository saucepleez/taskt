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
            this.uiBtnCancel = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnOK = new taskt.UI.CustomControls.UIPictureButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tvScriptVariables = new taskt.UI.CustomControls.UITreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDefineName = new System.Windows.Forms.Label();
            this.uiBtnNew = new taskt.UI.CustomControls.UIPictureButton();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(-2, -2);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(138, 45);
            this.lblMainLogo.TabIndex = 0;
            this.lblMainLogo.Text = "variables";
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
            this.uiBtnCancel.Location = new System.Drawing.Point(55, 1);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(48, 44);
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
            this.uiBtnOK.Image = global::taskt.Properties.Resources.various_ok_button;
            this.uiBtnOK.IsMouseOver = false;
            this.uiBtnOK.Location = new System.Drawing.Point(3, 1);
            this.uiBtnOK.Name = "uiBtnOK";
            this.uiBtnOK.Size = new System.Drawing.Size(48, 44);
            this.uiBtnOK.TabIndex = 14;
            this.uiBtnOK.TabStop = false;
            this.uiBtnOK.Text = "Ok";
            this.uiBtnOK.Click += new System.EventHandler(this.uiBtnOK_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tvScriptVariables, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(607, 385);
            this.tableLayoutPanel1.TabIndex = 17;
            // 
            // tvScriptVariables
            // 
            this.tvScriptVariables.BackColor = System.Drawing.Color.DimGray;
            this.tvScriptVariables.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvScriptVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvScriptVariables.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvScriptVariables.ForeColor = System.Drawing.Color.White;
            this.tvScriptVariables.Location = new System.Drawing.Point(3, 93);
            this.tvScriptVariables.Name = "tvScriptVariables";
            this.tvScriptVariables.ShowLines = false;
            this.tvScriptVariables.Size = new System.Drawing.Size(601, 242);
            this.tvScriptVariables.TabIndex = 1;
            this.tvScriptVariables.DoubleClick += new System.EventHandler(this.tvScriptVariables_DoubleClick);
            this.tvScriptVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tvScriptVariables_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel1.Controls.Add(this.lblDefineName);
            this.panel1.Controls.Add(this.uiBtnNew);
            this.panel1.Controls.Add(this.lblMainLogo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(607, 90);
            this.panel1.TabIndex = 0;
            // 
            // lblDefineName
            // 
            this.lblDefineName.AutoSize = true;
            this.lblDefineName.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineName.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineName.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.lblDefineName.Location = new System.Drawing.Point(55, 45);
            this.lblDefineName.Name = "lblDefineName";
            this.lblDefineName.Size = new System.Drawing.Size(294, 40);
            this.lblDefineName.TabIndex = 1;
            this.lblDefineName.Text = "Enter or Double-Click to edit existing variables\r\nPress \'DEL\' key to delete exist" +
    "ing variables";
            // 
            // uiBtnNew
            // 
            this.uiBtnNew.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnNew.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnNew.DisplayText = "Add";
            this.uiBtnNew.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.uiBtnNew.Image = global::taskt.Properties.Resources.action_bar_add_variable;
            this.uiBtnNew.IsMouseOver = false;
            this.uiBtnNew.Location = new System.Drawing.Point(5, 42);
            this.uiBtnNew.Name = "uiBtnNew";
            this.uiBtnNew.Size = new System.Drawing.Size(48, 45);
            this.uiBtnNew.TabIndex = 13;
            this.uiBtnNew.TabStop = false;
            this.uiBtnNew.Text = "Add";
            this.uiBtnNew.Click += new System.EventHandler(this.uiBtnNew_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.uiBtnOK);
            this.panel2.Controls.Add(this.uiBtnCancel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 338);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(607, 47);
            this.panel2.TabIndex = 2;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // frmScriptVariables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(607, 385);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmScriptVariables";
            this.Text = "Variables";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmScriptVariables_FormClosed);
            this.Load += new System.EventHandler(this.frmScriptVariables_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnNew)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblMainLogo;
        private taskt.UI.CustomControls.UIPictureButton uiBtnCancel;
        private taskt.UI.CustomControls.UIPictureButton uiBtnOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private CustomControls.UITreeView tvScriptVariables;
        private CustomControls.UIPictureButton uiBtnNew;
        private System.Windows.Forms.Label lblDefineName;
    }
}