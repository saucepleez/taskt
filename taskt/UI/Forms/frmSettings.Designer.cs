namespace taskt.UI.Forms
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.chkRetryOnDisconnect = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAutomaticallyConnect = new System.Windows.Forms.CheckBox();
            this.chkServerEnabled = new System.Windows.Forms.CheckBox();
            this.txtServerURL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtPublicKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkEnableLogging = new System.Windows.Forms.CheckBox();
            this.chkAutoCloseWindow = new System.Windows.Forms.CheckBox();
            this.chkShowDebug = new System.Windows.Forms.CheckBox();
            this.uiBtnOpen = new taskt.UI.CustomControls.UIPictureButton();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.lblOptions = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnConnect.Location = new System.Drawing.Point(8, 209);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 17;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblStatus.Location = new System.Drawing.Point(6, 238);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(228, 20);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Text = "Current Status: Not Connected";
            // 
            // chkRetryOnDisconnect
            // 
            this.chkRetryOnDisconnect.AutoSize = true;
            this.chkRetryOnDisconnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkRetryOnDisconnect.Location = new System.Drawing.Point(7, 95);
            this.chkRetryOnDisconnect.Name = "chkRetryOnDisconnect";
            this.chkRetryOnDisconnect.Size = new System.Drawing.Size(170, 20);
            this.chkRetryOnDisconnect.TabIndex = 15;
            this.chkRetryOnDisconnect.Text = "Retry If Connection Fails";
            this.chkRetryOnDisconnect.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(4, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(452, 40);
            this.label1.TabIndex = 14;
            this.label1.Text = "Optionally, the server component allows for remote execution of tasks as well as " +
    "visiblity and tracking into the taskt workforce.\r\n";
            // 
            // chkAutomaticallyConnect
            // 
            this.chkAutomaticallyConnect.AutoSize = true;
            this.chkAutomaticallyConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutomaticallyConnect.Location = new System.Drawing.Point(6, 75);
            this.chkAutomaticallyConnect.Name = "chkAutomaticallyConnect";
            this.chkAutomaticallyConnect.Size = new System.Drawing.Size(222, 20);
            this.chkAutomaticallyConnect.TabIndex = 13;
            this.chkAutomaticallyConnect.Text = "Automatically Connect on Startup";
            this.chkAutomaticallyConnect.UseVisualStyleBackColor = true;
            // 
            // chkServerEnabled
            // 
            this.chkServerEnabled.AutoSize = true;
            this.chkServerEnabled.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkServerEnabled.Location = new System.Drawing.Point(6, 56);
            this.chkServerEnabled.Name = "chkServerEnabled";
            this.chkServerEnabled.Size = new System.Drawing.Size(191, 20);
            this.chkServerEnabled.TabIndex = 12;
            this.chkServerEnabled.Text = "Server Connection Enabled";
            this.chkServerEnabled.UseVisualStyleBackColor = true;
            // 
            // txtServerURL
            // 
            this.txtServerURL.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtServerURL.Location = new System.Drawing.Point(6, 136);
            this.txtServerURL.Name = "txtServerURL";
            this.txtServerURL.Size = new System.Drawing.Size(371, 22);
            this.txtServerURL.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(3, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(232, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Server URL ex. ws://localhost:port/ws)\r";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.White;
            this.groupBox3.Controls.Add(this.txtPublicKey);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.btnConnect);
            this.groupBox3.Controls.Add(this.lblStatus);
            this.groupBox3.Controls.Add(this.chkRetryOnDisconnect);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.chkAutomaticallyConnect);
            this.groupBox3.Controls.Add(this.chkServerEnabled);
            this.groupBox3.Controls.Add(this.txtServerURL);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.SteelBlue;
            this.groupBox3.Location = new System.Drawing.Point(9, 42);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(464, 267);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "taskt Server Settings";
            // 
            // txtPublicKey
            // 
            this.txtPublicKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtPublicKey.Location = new System.Drawing.Point(7, 180);
            this.txtPublicKey.Name = "txtPublicKey";
            this.txtPublicKey.Size = new System.Drawing.Size(371, 22);
            this.txtPublicKey.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.SteelBlue;
            this.label2.Location = new System.Drawing.Point(5, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "Connect Key:";
            // 
            // chkEnableLogging
            // 
            this.chkEnableLogging.AutoSize = true;
            this.chkEnableLogging.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableLogging.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableLogging.ForeColor = System.Drawing.Color.White;
            this.chkEnableLogging.Location = new System.Drawing.Point(7, 390);
            this.chkEnableLogging.Name = "chkEnableLogging";
            this.chkEnableLogging.Size = new System.Drawing.Size(207, 24);
            this.chkEnableLogging.TabIndex = 14;
            this.chkEnableLogging.Text = "Enable Diagnostic Logging";
            this.chkEnableLogging.UseVisualStyleBackColor = false;
            // 
            // chkAutoCloseWindow
            // 
            this.chkAutoCloseWindow.AutoSize = true;
            this.chkAutoCloseWindow.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoCloseWindow.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoCloseWindow.ForeColor = System.Drawing.Color.White;
            this.chkAutoCloseWindow.Location = new System.Drawing.Point(7, 365);
            this.chkAutoCloseWindow.Name = "chkAutoCloseWindow";
            this.chkAutoCloseWindow.Size = new System.Drawing.Size(366, 24);
            this.chkAutoCloseWindow.TabIndex = 13;
            this.chkAutoCloseWindow.Text = "Automatically Close Debug Window (Success Only)";
            this.chkAutoCloseWindow.UseVisualStyleBackColor = false;
            // 
            // chkShowDebug
            // 
            this.chkShowDebug.AutoSize = true;
            this.chkShowDebug.BackColor = System.Drawing.Color.Transparent;
            this.chkShowDebug.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowDebug.ForeColor = System.Drawing.Color.White;
            this.chkShowDebug.Location = new System.Drawing.Point(7, 339);
            this.chkShowDebug.Name = "chkShowDebug";
            this.chkShowDebug.Size = new System.Drawing.Size(172, 24);
            this.chkShowDebug.TabIndex = 12;
            this.chkShowDebug.Text = "Show Debug Window";
            this.chkShowDebug.UseVisualStyleBackColor = false;
            // 
            // uiBtnOpen
            // 
            this.uiBtnOpen.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOpen.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOpen.DisplayText = "Ok";
            this.uiBtnOpen.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOpen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOpen.Image")));
            this.uiBtnOpen.IsMouseOver = false;
            this.uiBtnOpen.Location = new System.Drawing.Point(7, 420);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(48, 48);
            this.uiBtnOpen.TabIndex = 13;
            this.uiBtnOpen.TabStop = false;
            this.uiBtnOpen.Click += new System.EventHandler(this.uiBtnOpen_Click);
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMainLogo.Location = new System.Drawing.Point(5, 3);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(135, 37);
            this.lblMainLogo.TabIndex = 14;
            this.lblMainLogo.Text = "settings";
            // 
            // lblOptions
            // 
            this.lblOptions.AutoSize = true;
            this.lblOptions.BackColor = System.Drawing.Color.Transparent;
            this.lblOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptions.ForeColor = System.Drawing.Color.White;
            this.lblOptions.Location = new System.Drawing.Point(5, 312);
            this.lblOptions.Name = "lblOptions";
            this.lblOptions.Size = new System.Drawing.Size(152, 24);
            this.lblOptions.TabIndex = 15;
            this.lblOptions.Text = "Debug Settings";
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 166;
            this.ClientSize = new System.Drawing.Size(485, 474);
            this.Controls.Add(this.lblOptions);
            this.Controls.Add(this.chkEnableLogging);
            this.Controls.Add(this.lblMainLogo);
            this.Controls.Add(this.chkAutoCloseWindow);
            this.Controls.Add(this.uiBtnOpen);
            this.Controls.Add(this.chkShowDebug);
            this.Controls.Add(this.groupBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServerURL;
        private System.Windows.Forms.CheckBox chkAutomaticallyConnect;
        private System.Windows.Forms.CheckBox chkServerEnabled;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkRetryOnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtPublicKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkAutoCloseWindow;
        private System.Windows.Forms.CheckBox chkShowDebug;
        private CustomControls.UIPictureButton uiBtnOpen;
        private System.Windows.Forms.CheckBox chkEnableLogging;
        private System.Windows.Forms.Label lblMainLogo;
        private System.Windows.Forms.Label lblOptions;
    }
}