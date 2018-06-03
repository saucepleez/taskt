namespace taskt.UI.Forms.Supplemental
{
    partial class frmShowCursorPosition
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmShowCursorPosition));
            this.tmrGetPosition = new System.Windows.Forms.Timer(this.components);
            this.lblhelperText = new System.Windows.Forms.Label();
            this.lblYPosition = new System.Windows.Forms.Label();
            this.lblXPosition = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tmrGetPosition
            // 
            this.tmrGetPosition.Enabled = true;
            this.tmrGetPosition.Tick += new System.EventHandler(this.tmrGetPosition_Tick);
            // 
            // lblhelperText
            // 
            this.lblhelperText.BackColor = System.Drawing.Color.Transparent;
            this.lblhelperText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblhelperText.ForeColor = System.Drawing.Color.White;
            this.lblhelperText.Location = new System.Drawing.Point(5, 95);
            this.lblhelperText.Name = "lblhelperText";
            this.lblhelperText.Size = new System.Drawing.Size(353, 43);
            this.lblhelperText.TabIndex = 2;
            this.lblhelperText.Text = "hint: With this window focused, press the spacebar to capture the current mouse p" +
    "osition";
            // 
            // lblYPosition
            // 
            this.lblYPosition.AutoSize = true;
            this.lblYPosition.BackColor = System.Drawing.Color.Transparent;
            this.lblYPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYPosition.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblYPosition.Location = new System.Drawing.Point(12, 44);
            this.lblYPosition.Name = "lblYPosition";
            this.lblYPosition.Size = new System.Drawing.Size(128, 29);
            this.lblYPosition.TabIndex = 1;
            this.lblYPosition.Text = "Y Position:";
            // 
            // lblXPosition
            // 
            this.lblXPosition.AutoSize = true;
            this.lblXPosition.BackColor = System.Drawing.Color.Transparent;
            this.lblXPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXPosition.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblXPosition.Location = new System.Drawing.Point(11, 9);
            this.lblXPosition.Name = "lblXPosition";
            this.lblXPosition.Size = new System.Drawing.Size(129, 29);
            this.lblXPosition.TabIndex = 0;
            this.lblXPosition.Text = "X Position:";
            // 
            // frmShowCursorPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.BackgroundChangeIndex = 92;
            this.ClientSize = new System.Drawing.Size(398, 145);
            this.Controls.Add(this.lblhelperText);
            this.Controls.Add(this.lblYPosition);
            this.Controls.Add(this.lblXPosition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmShowCursorPosition";
            this.Text = "Cursor Position Tracker";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ShowCursorPosition_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShowCursorPosition_KeyDown);
            this.MouseEnter += new System.EventHandler(this.frmShowCursorPosition_MouseEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblXPosition;
        private System.Windows.Forms.Label lblYPosition;
        private System.Windows.Forms.Timer tmrGetPosition;
        private System.Windows.Forms.Label lblhelperText;
    }
}