namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmAddElement
    {
        /// <summary>
        /// Required designer element.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddElement));
            this.lblDefineName = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.txtElementName = new System.Windows.Forms.TextBox();
            this.lblDefineNameDescription = new System.Windows.Forms.Label();
            this.lblDefineDefaultValueDescriptor = new System.Windows.Forms.Label();
            this.txtDefaultValue = new System.Windows.Forms.TextBox();
            this.lblDefineDefaultValue = new System.Windows.Forms.Label();
            this.uiBtnOk = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.lblDefineType = new System.Windows.Forms.Label();
            this.cbxElementType = new System.Windows.Forms.ComboBox();
            this.lblDefineTypeDescription = new System.Windows.Forms.Label();
            this.lblElementNameError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDefineName
            // 
            this.lblDefineName.AutoSize = true;
            this.lblDefineName.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineName.ForeColor = System.Drawing.Color.White;
            this.lblDefineName.Location = new System.Drawing.Point(16, 58);
            this.lblDefineName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineName.Name = "lblDefineName";
            this.lblDefineName.Size = new System.Drawing.Size(212, 28);
            this.lblDefineName.TabIndex = 15;
            this.lblDefineName.Text = "Define Element Name";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeader.Location = new System.Drawing.Point(8, 4);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(241, 54);
            this.lblHeader.TabIndex = 14;
            this.lblHeader.Text = "add element";
            // 
            // txtElementName
            // 
            this.txtElementName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtElementName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtElementName.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtElementName.Location = new System.Drawing.Point(21, 167);
            this.txtElementName.Margin = new System.Windows.Forms.Padding(4);
            this.txtElementName.Name = "txtElementName";
            this.txtElementName.Size = new System.Drawing.Size(563, 32);
            this.txtElementName.TabIndex = 16;
            // 
            // lblDefineNameDescription
            // 
            this.lblDefineNameDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineNameDescription.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineNameDescription.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDefineNameDescription.Location = new System.Drawing.Point(16, 85);
            this.lblDefineNameDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineNameDescription.Name = "lblDefineNameDescription";
            this.lblDefineNameDescription.Size = new System.Drawing.Size(577, 79);
            this.lblDefineNameDescription.TabIndex = 17;
            this.lblDefineNameDescription.Text = "Define a name for your element, such as \'eNumber\'.  Remember to enclose the name " +
    "within brackets in order to use the element in commands, ex. <vNumber>.";
            // 
            // lblDefineDefaultValueDescriptor
            // 
            this.lblDefineDefaultValueDescriptor.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineDefaultValueDescriptor.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineDefaultValueDescriptor.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDefineDefaultValueDescriptor.Location = new System.Drawing.Point(16, 403);
            this.lblDefineDefaultValueDescriptor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineDefaultValueDescriptor.Name = "lblDefineDefaultValueDescriptor";
            this.lblDefineDefaultValueDescriptor.Size = new System.Drawing.Size(577, 54);
            this.lblDefineDefaultValueDescriptor.TabIndex = 20;
            this.lblDefineDefaultValueDescriptor.Text = "Optionally, define a default value for the element.  The element will represent t" +
    "his value until changed during the task by a task command.";
            // 
            // txtDefaultValue
            // 
            this.txtDefaultValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefaultValue.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDefaultValue.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtDefaultValue.Location = new System.Drawing.Point(21, 461);
            this.txtDefaultValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtDefaultValue.Name = "txtDefaultValue";
            this.txtDefaultValue.Size = new System.Drawing.Size(563, 32);
            this.txtDefaultValue.TabIndex = 19;
            // 
            // lblDefineDefaultValue
            // 
            this.lblDefineDefaultValue.AutoSize = true;
            this.lblDefineDefaultValue.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineDefaultValue.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineDefaultValue.ForeColor = System.Drawing.Color.White;
            this.lblDefineDefaultValue.Location = new System.Drawing.Point(16, 377);
            this.lblDefineDefaultValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineDefaultValue.Name = "lblDefineDefaultValue";
            this.lblDefineDefaultValue.Size = new System.Drawing.Size(280, 28);
            this.lblDefineDefaultValue.TabIndex = 18;
            this.lblDefineDefaultValue.Text = "Define Element Default Value";
            // 
            // uiBtnOk
            // 
            this.uiBtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uiBtnOk.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOk.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOk.DisplayText = "Ok";
            this.uiBtnOk.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOk.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOk.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOk.Image")));
            this.uiBtnOk.IsMouseOver = false;
            this.uiBtnOk.Location = new System.Drawing.Point(17, 508);
            this.uiBtnOk.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(71, 60);
            this.uiBtnOk.TabIndex = 21;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Text = "Ok";
            this.uiBtnOk.Click += new System.EventHandler(this.uiBtnOk_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(87, 508);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(71, 60);
            this.uiBtnCancel.TabIndex = 22;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // lblDefineType
            // 
            this.lblDefineType.AutoSize = true;
            this.lblDefineType.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineType.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineType.ForeColor = System.Drawing.Color.White;
            this.lblDefineType.Location = new System.Drawing.Point(16, 241);
            this.lblDefineType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineType.Name = "lblDefineType";
            this.lblDefineType.Size = new System.Drawing.Size(201, 28);
            this.lblDefineType.TabIndex = 23;
            this.lblDefineType.Text = "Define Element Type";
            // 
            // cbxElementType
            // 
            this.cbxElementType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxElementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxElementType.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cbxElementType.ForeColor = System.Drawing.Color.SteelBlue;
            this.cbxElementType.FormattingEnabled = true;
            this.cbxElementType.Location = new System.Drawing.Point(21, 302);
            this.cbxElementType.Name = "cbxElementType";
            this.cbxElementType.Size = new System.Drawing.Size(563, 33);
            this.cbxElementType.TabIndex = 24;
            this.cbxElementType.SelectedIndexChanged += new System.EventHandler(this.cbxElementType_SelectedIndexChanged);
            // 
            // lblDefineTypeDescription
            // 
            this.lblDefineTypeDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineTypeDescription.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineTypeDescription.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDefineTypeDescription.Location = new System.Drawing.Point(16, 272);
            this.lblDefineTypeDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineTypeDescription.Name = "lblDefineTypeDescription";
            this.lblDefineTypeDescription.Size = new System.Drawing.Size(577, 27);
            this.lblDefineTypeDescription.TabIndex = 25;
            this.lblDefineTypeDescription.Text = "Define a type for your element, such as \'XPath\'.";
            // 
            // lblElementNameError
            // 
            this.lblElementNameError.BackColor = System.Drawing.Color.Transparent;
            this.lblElementNameError.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElementNameError.ForeColor = System.Drawing.Color.Red;
            this.lblElementNameError.Location = new System.Drawing.Point(16, 203);
            this.lblElementNameError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElementNameError.Name = "lblElementNameError";
            this.lblElementNameError.Size = new System.Drawing.Size(577, 36);
            this.lblElementNameError.TabIndex = 26;
            // 
            // frmAddElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 583);
            this.Controls.Add(this.lblElementNameError);
            this.Controls.Add(this.lblDefineTypeDescription);
            this.Controls.Add(this.cbxElementType);
            this.Controls.Add(this.lblDefineType);
            this.Controls.Add(this.uiBtnOk);
            this.Controls.Add(this.uiBtnCancel);
            this.Controls.Add(this.lblDefineDefaultValueDescriptor);
            this.Controls.Add(this.txtDefaultValue);
            this.Controls.Add(this.lblDefineDefaultValue);
            this.Controls.Add(this.lblDefineNameDescription);
            this.Controls.Add(this.txtElementName);
            this.Controls.Add(this.lblDefineName);
            this.Controls.Add(this.lblHeader);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(627, 630);
            this.Name = "frmAddElement";
            this.Text = "Add Element";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAddElement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDefineName;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblDefineNameDescription;
        private System.Windows.Forms.Label lblDefineDefaultValueDescriptor;
        private System.Windows.Forms.Label lblDefineDefaultValue;
        private CustomControls.CustomUIControls.UIPictureButton uiBtnOk;
        private CustomControls.CustomUIControls.UIPictureButton uiBtnCancel;
        public System.Windows.Forms.TextBox txtElementName;
        public System.Windows.Forms.TextBox txtDefaultValue;
        private System.Windows.Forms.Label lblDefineType;
        public System.Windows.Forms.ComboBox cbxElementType;
        private System.Windows.Forms.Label lblDefineTypeDescription;
        private System.Windows.Forms.Label lblElementNameError;
    }
}