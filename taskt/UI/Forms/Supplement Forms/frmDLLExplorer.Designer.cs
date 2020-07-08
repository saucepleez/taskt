namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmDLLExplorer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDLLExplorer));
            this.lstClasses = new System.Windows.Forms.ListBox();
            this.lstMethods = new System.Windows.Forms.ListBox();
            this.lstParameters = new System.Windows.Forms.ListBox();
            this.lblLoadDLL = new System.Windows.Forms.Label();
            this.lblClasses = new System.Windows.Forms.Label();
            this.lblMethods = new System.Windows.Forms.Label();
            this.lblParameters = new System.Windows.Forms.Label();
            this.upbLoadDLL = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.uiBtnOk = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            ((System.ComponentModel.ISupportInitialize)(this.upbLoadDLL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            this.SuspendLayout();
            // 
            // lstClasses
            // 
            this.lstClasses.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstClasses.FormattingEnabled = true;
            this.lstClasses.ItemHeight = 21;
            this.lstClasses.Location = new System.Drawing.Point(12, 116);
            this.lstClasses.Name = "lstClasses";
            this.lstClasses.Size = new System.Drawing.Size(517, 88);
            this.lstClasses.TabIndex = 0;
            this.lstClasses.SelectedIndexChanged += new System.EventHandler(this.lstClasses_SelectedIndexChanged);
            // 
            // lstMethods
            // 
            this.lstMethods.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstMethods.FormattingEnabled = true;
            this.lstMethods.ItemHeight = 21;
            this.lstMethods.Location = new System.Drawing.Point(12, 242);
            this.lstMethods.Name = "lstMethods";
            this.lstMethods.Size = new System.Drawing.Size(517, 88);
            this.lstMethods.TabIndex = 1;
            this.lstMethods.SelectedIndexChanged += new System.EventHandler(this.lstMethods_SelectedIndexChanged);
            // 
            // lstParameters
            // 
            this.lstParameters.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstParameters.FormattingEnabled = true;
            this.lstParameters.ItemHeight = 21;
            this.lstParameters.Location = new System.Drawing.Point(12, 368);
            this.lstParameters.Name = "lstParameters";
            this.lstParameters.Size = new System.Drawing.Size(517, 88);
            this.lstParameters.TabIndex = 2;
            // 
            // lblLoadDLL
            // 
            this.lblLoadDLL.AutoSize = true;
            this.lblLoadDLL.BackColor = System.Drawing.Color.Transparent;
            this.lblLoadDLL.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoadDLL.ForeColor = System.Drawing.Color.White;
            this.lblLoadDLL.Location = new System.Drawing.Point(7, 9);
            this.lblLoadDLL.Name = "lblLoadDLL";
            this.lblLoadDLL.Size = new System.Drawing.Size(424, 25);
            this.lblLoadDLL.TabIndex = 3;
            this.lblLoadDLL.Text = "Load a DLL and explore methods and parameters";
            // 
            // lblClasses
            // 
            this.lblClasses.AutoSize = true;
            this.lblClasses.BackColor = System.Drawing.Color.Transparent;
            this.lblClasses.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClasses.ForeColor = System.Drawing.Color.White;
            this.lblClasses.Location = new System.Drawing.Point(7, 88);
            this.lblClasses.Name = "lblClasses";
            this.lblClasses.Size = new System.Drawing.Size(73, 25);
            this.lblClasses.TabIndex = 4;
            this.lblClasses.Text = "Classes";
            // 
            // lblMethods
            // 
            this.lblMethods.AutoSize = true;
            this.lblMethods.BackColor = System.Drawing.Color.Transparent;
            this.lblMethods.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMethods.ForeColor = System.Drawing.Color.White;
            this.lblMethods.Location = new System.Drawing.Point(7, 214);
            this.lblMethods.Name = "lblMethods";
            this.lblMethods.Size = new System.Drawing.Size(90, 25);
            this.lblMethods.TabIndex = 5;
            this.lblMethods.Text = "Methods:";
            // 
            // lblParameters
            // 
            this.lblParameters.AutoSize = true;
            this.lblParameters.BackColor = System.Drawing.Color.Transparent;
            this.lblParameters.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParameters.ForeColor = System.Drawing.Color.White;
            this.lblParameters.Location = new System.Drawing.Point(12, 340);
            this.lblParameters.Name = "lblParameters";
            this.lblParameters.Size = new System.Drawing.Size(110, 25);
            this.lblParameters.TabIndex = 6;
            this.lblParameters.Text = "Parameters:";
            // 
            // upbLoadDLL
            // 
            this.upbLoadDLL.BackColor = System.Drawing.Color.Transparent;
            this.upbLoadDLL.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.upbLoadDLL.DisplayText = "Load DLL";
            this.upbLoadDLL.DisplayTextBrush = System.Drawing.Color.White;
            this.upbLoadDLL.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.upbLoadDLL.Image = global::taskt.Properties.Resources.command_run_code;
            this.upbLoadDLL.IsMouseOver = false;
            this.upbLoadDLL.Location = new System.Drawing.Point(12, 37);
            this.upbLoadDLL.Name = "upbLoadDLL";
            this.upbLoadDLL.Size = new System.Drawing.Size(55, 48);
            this.upbLoadDLL.TabIndex = 8;
            this.upbLoadDLL.TabStop = false;
            this.upbLoadDLL.Text = "Load DLL";
            this.upbLoadDLL.Click += new System.EventHandler(this.upbLoadDLL_Click);
            // 
            // uiBtnOk
            // 
            this.uiBtnOk.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOk.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOk.DisplayText = "Ok";
            this.uiBtnOk.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOk.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOk.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOk.Image")));
            this.uiBtnOk.IsMouseOver = false;
            this.uiBtnOk.Location = new System.Drawing.Point(11, 464);
            this.uiBtnOk.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(48, 49);
            this.uiBtnOk.TabIndex = 17;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Click += new System.EventHandler(this.uiBtnOk_Click);
            // 
            // frmDLLExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 247;
            this.ClientSize = new System.Drawing.Size(543, 519);
            this.Controls.Add(this.uiBtnOk);
            this.Controls.Add(this.upbLoadDLL);
            this.Controls.Add(this.lblParameters);
            this.Controls.Add(this.lblMethods);
            this.Controls.Add(this.lblClasses);
            this.Controls.Add(this.lblLoadDLL);
            this.Controls.Add(this.lstParameters);
            this.Controls.Add(this.lstMethods);
            this.Controls.Add(this.lstClasses);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDLLExplorer";
            this.Text = "DLL Explorer";
            ((System.ComponentModel.ISupportInitialize)(this.upbLoadDLL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblLoadDLL;
        private System.Windows.Forms.Label lblClasses;
        private System.Windows.Forms.Label lblMethods;
        private System.Windows.Forms.Label lblParameters;
        private CustomControls.CustomUIControls.UIPictureButton upbLoadDLL;
        private CustomControls.CustomUIControls.UIPictureButton uiBtnOk;
        public System.Windows.Forms.ListBox lstClasses;
        public System.Windows.Forms.ListBox lstMethods;
        public System.Windows.Forms.ListBox lstParameters;
    }
}