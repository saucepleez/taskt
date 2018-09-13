namespace taskt.UI.Forms.Supplemental
{
    partial class frmCodeBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCodeBuilder));
            this.tlpBuilder = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.uiBtnSample = new taskt.UI.CustomControls.UIPictureButton();
            this.uiBtnSave = new taskt.UI.CustomControls.UIPictureButton();
            this.chkRunAfterCompile = new System.Windows.Forms.CheckBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.uiBtnCompile = new taskt.UI.CustomControls.UIPictureButton();
            this.lstCompilerResults = new System.Windows.Forms.ListBox();
            this.rtbCode = new System.Windows.Forms.RichTextBox();
            this.tlpBuilder.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCompile)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpBuilder
            // 
            this.tlpBuilder.BackColor = System.Drawing.Color.DimGray;
            this.tlpBuilder.ColumnCount = 1;
            this.tlpBuilder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.5F));
            this.tlpBuilder.Controls.Add(this.panel1, 0, 0);
            this.tlpBuilder.Controls.Add(this.lstCompilerResults, 0, 2);
            this.tlpBuilder.Controls.Add(this.rtbCode, 0, 1);
            this.tlpBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBuilder.Location = new System.Drawing.Point(0, 0);
            this.tlpBuilder.Name = "tlpBuilder";
            this.tlpBuilder.RowCount = 3;
            this.tlpBuilder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.80672F));
            this.tlpBuilder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.19328F));
            this.tlpBuilder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.tlpBuilder.Size = new System.Drawing.Size(827, 451);
            this.tlpBuilder.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.uiBtnSample);
            this.panel1.Controls.Add(this.uiBtnSave);
            this.panel1.Controls.Add(this.chkRunAfterCompile);
            this.panel1.Controls.Add(this.lblHeader);
            this.panel1.Controls.Add(this.uiBtnCompile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(821, 54);
            this.panel1.TabIndex = 2;
            // 
            // uiBtnSample
            // 
            this.uiBtnSample.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSample.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSample.DisplayText = "Sample";
            this.uiBtnSample.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnSample.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiBtnSample.Image = global::taskt.Properties.Resources.action_bar_new;
            this.uiBtnSample.IsMouseOver = false;
            this.uiBtnSample.Location = new System.Drawing.Point(218, 3);
            this.uiBtnSample.Name = "uiBtnSample";
            this.uiBtnSample.Size = new System.Drawing.Size(52, 48);
            this.uiBtnSample.TabIndex = 19;
            this.uiBtnSample.TabStop = false;
            this.uiBtnSample.Click += new System.EventHandler(this.uiBtnSample_Click);
            // 
            // uiBtnSave
            // 
            this.uiBtnSave.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSave.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSave.DisplayText = "Save";
            this.uiBtnSave.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnSave.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiBtnSave.Image = global::taskt.Properties.Resources.action_bar_save;
            this.uiBtnSave.IsMouseOver = false;
            this.uiBtnSave.Location = new System.Drawing.Point(334, 3);
            this.uiBtnSave.Name = "uiBtnSave";
            this.uiBtnSave.Size = new System.Drawing.Size(52, 48);
            this.uiBtnSave.TabIndex = 18;
            this.uiBtnSave.TabStop = false;
            this.uiBtnSave.Click += new System.EventHandler(this.uiBtnSave_Click);
            // 
            // chkRunAfterCompile
            // 
            this.chkRunAfterCompile.AutoSize = true;
            this.chkRunAfterCompile.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRunAfterCompile.ForeColor = System.Drawing.Color.White;
            this.chkRunAfterCompile.Location = new System.Drawing.Point(389, 3);
            this.chkRunAfterCompile.Name = "chkRunAfterCompile";
            this.chkRunAfterCompile.Size = new System.Drawing.Size(223, 21);
            this.chkRunAfterCompile.TabIndex = 16;
            this.chkRunAfterCompile.Text = "Run App after Successful Compile";
            this.chkRunAfterCompile.UseVisualStyleBackColor = true;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeader.Location = new System.Drawing.Point(3, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(203, 37);
            this.lblHeader.TabIndex = 15;
            this.lblHeader.Text = "code builder";
            // 
            // uiBtnCompile
            // 
            this.uiBtnCompile.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCompile.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCompile.DisplayText = "Compile";
            this.uiBtnCompile.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCompile.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiBtnCompile.Image = global::taskt.Properties.Resources.action_bar_run;
            this.uiBtnCompile.IsMouseOver = false;
            this.uiBtnCompile.Location = new System.Drawing.Point(276, 3);
            this.uiBtnCompile.Name = "uiBtnCompile";
            this.uiBtnCompile.Size = new System.Drawing.Size(52, 48);
            this.uiBtnCompile.TabIndex = 1;
            this.uiBtnCompile.TabStop = false;
            this.uiBtnCompile.Click += new System.EventHandler(this.utBtnCompile_Click);
            // 
            // lstCompilerResults
            // 
            this.lstCompilerResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCompilerResults.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstCompilerResults.FormattingEnabled = true;
            this.lstCompilerResults.ItemHeight = 15;
            this.lstCompilerResults.Location = new System.Drawing.Point(3, 360);
            this.lstCompilerResults.Name = "lstCompilerResults";
            this.lstCompilerResults.Size = new System.Drawing.Size(821, 88);
            this.lstCompilerResults.TabIndex = 3;
            // 
            // rtbCode
            // 
            this.rtbCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCode.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbCode.Location = new System.Drawing.Point(3, 63);
            this.rtbCode.Name = "rtbCode";
            this.rtbCode.Size = new System.Drawing.Size(821, 291);
            this.rtbCode.TabIndex = 4;
            this.rtbCode.Text = "";
            this.rtbCode.TextChanged += new System.EventHandler(this.rtbCode_TextChanged);
            // 
            // frmCodeBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 451);
            this.Controls.Add(this.tlpBuilder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCodeBuilder";
            this.Text = "Code Builder";
            this.Load += new System.EventHandler(this.frmCodeBuilder_Load);
            this.tlpBuilder.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCompile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpBuilder;
        private System.Windows.Forms.Panel panel1;
        private CustomControls.UIPictureButton uiBtnCompile;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.ListBox lstCompilerResults;
        private System.Windows.Forms.CheckBox chkRunAfterCompile;
        private CustomControls.UIPictureButton uiBtnSave;
        public System.Windows.Forms.RichTextBox rtbCode;
        private CustomControls.UIPictureButton uiBtnSample;
    }
}