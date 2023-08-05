
namespace taskt.UI.Forms
{
    partial class frmScriptInformations
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScriptInformations));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uiBtnOpen = new taskt.UI.CustomControls.UIPictureButton();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtLastRun = new System.Windows.Forms.Label();
            this.lblLastRun = new System.Windows.Forms.Label();
            this.txtRunTimes = new System.Windows.Forms.Label();
            this.lblRunTimes = new System.Windows.Forms.Label();
            this.txtTasktVersion = new System.Windows.Forms.Label();
            this.lblTastkVersion = new System.Windows.Forms.Label();
            this.lblScriptDescription = new System.Windows.Forms.Label();
            this.txtScriptDescription = new System.Windows.Forms.TextBox();
            this.lblScriptVersion = new System.Windows.Forms.Label();
            this.txtScriptVersion = new System.Windows.Forms.TextBox();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.txtScriptAuthor = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.uiBtnOpen, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblMainLogo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(441, 354);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // uiBtnOpen
            // 
            this.uiBtnOpen.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOpen.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOpen.DisplayText = "Ok";
            this.uiBtnOpen.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOpen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOpen.Image = global::taskt.Properties.Resources.various_ok_button;
            this.uiBtnOpen.IsMouseOver = false;
            this.uiBtnOpen.Location = new System.Drawing.Point(3, 302);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(48, 44);
            this.uiBtnOpen.TabIndex = 17;
            this.uiBtnOpen.TabStop = false;
            this.uiBtnOpen.Text = "Ok";
            this.uiBtnOpen.Click += new System.EventHandler(this.uiBtnOpen_Click);
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(3, 0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(278, 45);
            this.lblMainLogo.TabIndex = 16;
            this.lblMainLogo.Text = "Script Informations";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.txtLastRun);
            this.panel1.Controls.Add(this.lblLastRun);
            this.panel1.Controls.Add(this.txtRunTimes);
            this.panel1.Controls.Add(this.lblRunTimes);
            this.panel1.Controls.Add(this.txtTasktVersion);
            this.panel1.Controls.Add(this.lblTastkVersion);
            this.panel1.Controls.Add(this.lblScriptDescription);
            this.panel1.Controls.Add(this.txtScriptDescription);
            this.panel1.Controls.Add(this.lblScriptVersion);
            this.panel1.Controls.Add(this.txtScriptVersion);
            this.panel1.Controls.Add(this.lblAuthor);
            this.panel1.Controls.Add(this.txtScriptAuthor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(435, 238);
            this.panel1.TabIndex = 18;
            // 
            // txtLastRun
            // 
            this.txtLastRun.AutoSize = true;
            this.txtLastRun.BackColor = System.Drawing.Color.Transparent;
            this.txtLastRun.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtLastRun.ForeColor = System.Drawing.Color.SlateGray;
            this.txtLastRun.Location = new System.Drawing.Point(9, 326);
            this.txtLastRun.Name = "txtLastRun";
            this.txtLastRun.Size = new System.Drawing.Size(62, 21);
            this.txtLastRun.TabIndex = 20;
            this.txtLastRun.Text = "last run";
            // 
            // lblLastRun
            // 
            this.lblLastRun.AutoSize = true;
            this.lblLastRun.BackColor = System.Drawing.Color.Transparent;
            this.lblLastRun.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastRun.ForeColor = System.Drawing.Color.SlateGray;
            this.lblLastRun.Location = new System.Drawing.Point(9, 309);
            this.lblLastRun.Name = "lblLastRun";
            this.lblLastRun.Size = new System.Drawing.Size(110, 17);
            this.lblLastRun.TabIndex = 19;
            this.lblLastRun.Text = "Last run date time";
            // 
            // txtRunTimes
            // 
            this.txtRunTimes.AutoSize = true;
            this.txtRunTimes.BackColor = System.Drawing.Color.Transparent;
            this.txtRunTimes.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtRunTimes.ForeColor = System.Drawing.Color.SlateGray;
            this.txtRunTimes.Location = new System.Drawing.Point(9, 282);
            this.txtRunTimes.Name = "txtRunTimes";
            this.txtRunTimes.Size = new System.Drawing.Size(76, 21);
            this.txtRunTimes.TabIndex = 18;
            this.txtRunTimes.Text = "run times";
            // 
            // lblRunTimes
            // 
            this.lblRunTimes.AutoSize = true;
            this.lblRunTimes.BackColor = System.Drawing.Color.Transparent;
            this.lblRunTimes.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRunTimes.ForeColor = System.Drawing.Color.SlateGray;
            this.lblRunTimes.Location = new System.Drawing.Point(9, 265);
            this.lblRunTimes.Name = "lblRunTimes";
            this.lblRunTimes.Size = new System.Drawing.Size(169, 17);
            this.lblRunTimes.TabIndex = 17;
            this.lblRunTimes.Text = "Number of srcript run time(s)";
            // 
            // txtTasktVersion
            // 
            this.txtTasktVersion.AutoSize = true;
            this.txtTasktVersion.BackColor = System.Drawing.Color.Transparent;
            this.txtTasktVersion.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtTasktVersion.ForeColor = System.Drawing.Color.SlateGray;
            this.txtTasktVersion.Location = new System.Drawing.Point(9, 238);
            this.txtTasktVersion.Name = "txtTasktVersion";
            this.txtTasktVersion.Size = new System.Drawing.Size(98, 21);
            this.txtTasktVersion.TabIndex = 16;
            this.txtTasktVersion.Text = "taskt version";
            // 
            // lblTastkVersion
            // 
            this.lblTastkVersion.AutoSize = true;
            this.lblTastkVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblTastkVersion.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTastkVersion.ForeColor = System.Drawing.Color.SlateGray;
            this.lblTastkVersion.Location = new System.Drawing.Point(9, 221);
            this.lblTastkVersion.Name = "lblTastkVersion";
            this.lblTastkVersion.Size = new System.Drawing.Size(78, 17);
            this.lblTastkVersion.TabIndex = 15;
            this.lblTastkVersion.Text = "taskt Version";
            // 
            // lblScriptDescription
            // 
            this.lblScriptDescription.AutoSize = true;
            this.lblScriptDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblScriptDescription.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScriptDescription.ForeColor = System.Drawing.Color.SlateGray;
            this.lblScriptDescription.Location = new System.Drawing.Point(9, 109);
            this.lblScriptDescription.Name = "lblScriptDescription";
            this.lblScriptDescription.Size = new System.Drawing.Size(105, 17);
            this.lblScriptDescription.TabIndex = 13;
            this.lblScriptDescription.Text = "Script Description";
            // 
            // txtScriptDescription
            // 
            this.txtScriptDescription.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScriptDescription.Location = new System.Drawing.Point(12, 126);
            this.txtScriptDescription.Multiline = true;
            this.txtScriptDescription.Name = "txtScriptDescription";
            this.txtScriptDescription.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtScriptDescription.Size = new System.Drawing.Size(401, 85);
            this.txtScriptDescription.TabIndex = 14;
            // 
            // lblScriptVersion
            // 
            this.lblScriptVersion.AutoSize = true;
            this.lblScriptVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblScriptVersion.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScriptVersion.ForeColor = System.Drawing.Color.SlateGray;
            this.lblScriptVersion.Location = new System.Drawing.Point(9, 60);
            this.lblScriptVersion.Name = "lblScriptVersion";
            this.lblScriptVersion.Size = new System.Drawing.Size(83, 17);
            this.lblScriptVersion.TabIndex = 11;
            this.lblScriptVersion.Text = "Script Version";
            // 
            // txtScriptVersion
            // 
            this.txtScriptVersion.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScriptVersion.Location = new System.Drawing.Point(12, 77);
            this.txtScriptVersion.Name = "txtScriptVersion";
            this.txtScriptVersion.Size = new System.Drawing.Size(401, 29);
            this.txtScriptVersion.TabIndex = 12;
            // 
            // lblAuthor
            // 
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.BackColor = System.Drawing.Color.Transparent;
            this.lblAuthor.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthor.ForeColor = System.Drawing.Color.SlateGray;
            this.lblAuthor.Location = new System.Drawing.Point(9, 10);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(80, 17);
            this.lblAuthor.TabIndex = 9;
            this.lblAuthor.Text = "Script Author";
            // 
            // txtScriptAuthor
            // 
            this.txtScriptAuthor.Font = new System.Drawing.Font("Segoe UI Emoji", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtScriptAuthor.Location = new System.Drawing.Point(12, 27);
            this.txtScriptAuthor.Name = "txtScriptAuthor";
            this.txtScriptAuthor.Size = new System.Drawing.Size(401, 29);
            this.txtScriptAuthor.TabIndex = 10;
            // 
            // frmScriptInformations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 354);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmScriptInformations";
            this.Text = "Script Informations";
            this.Load += new System.EventHandler(this.frmScriptInformations_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblMainLogo;
        private CustomControls.UIPictureButton uiBtnOpen;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.TextBox txtScriptAuthor;
        private System.Windows.Forms.Label lblScriptDescription;
        private System.Windows.Forms.TextBox txtScriptDescription;
        private System.Windows.Forms.Label lblScriptVersion;
        private System.Windows.Forms.TextBox txtScriptVersion;
        private System.Windows.Forms.Label txtTasktVersion;
        private System.Windows.Forms.Label lblTastkVersion;
        private System.Windows.Forms.Label txtRunTimes;
        private System.Windows.Forms.Label lblRunTimes;
        private System.Windows.Forms.Label txtLastRun;
        private System.Windows.Forms.Label lblLastRun;
    }
}
