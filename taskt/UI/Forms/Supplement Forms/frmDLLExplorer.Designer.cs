namespace taskt.UI.Forms.Supplemental
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.uiPictureButton1 = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnOk = new taskt.UI.CustomControls.UIPictureButton();
            ((System.ComponentModel.ISupportInitialize)(this.uiPictureButton1)).BeginInit();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(424, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Load a DLL and explore methods and parameters";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Classes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(7, 214);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Methods:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(12, 340);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 25);
            this.label4.TabIndex = 6;
            this.label4.Text = "Parameters:";
            // 
            // uiPictureButton1
            // 
            this.uiPictureButton1.BackColor = System.Drawing.Color.Transparent;
            this.uiPictureButton1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiPictureButton1.DisplayText = "Load DLL";
            this.uiPictureButton1.DisplayTextBrush = System.Drawing.Color.White;
            this.uiPictureButton1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiPictureButton1.Image = global::taskt.Properties.Resources.command_run_code;
            this.uiPictureButton1.IsMouseOver = false;
            this.uiPictureButton1.Location = new System.Drawing.Point(12, 37);
            this.uiPictureButton1.Name = "uiPictureButton1";
            this.uiPictureButton1.Size = new System.Drawing.Size(55, 48);
            this.uiPictureButton1.TabIndex = 8;
            this.uiPictureButton1.TabStop = false;
            this.uiPictureButton1.Click += new System.EventHandler(this.uiPictureButton1_Click);
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
            this.Controls.Add(this.uiPictureButton1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstParameters);
            this.Controls.Add(this.lstMethods);
            this.Controls.Add(this.lstClasses);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDLLExplorer";
            this.Text = "DLL Explorer";
            ((System.ComponentModel.ISupportInitialize)(this.uiPictureButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private CustomControls.UIPictureButton uiPictureButton1;
        private CustomControls.UIPictureButton uiBtnOk;
        public System.Windows.Forms.ListBox lstClasses;
        public System.Windows.Forms.ListBox lstMethods;
        public System.Windows.Forms.ListBox lstParameters;
    }
}