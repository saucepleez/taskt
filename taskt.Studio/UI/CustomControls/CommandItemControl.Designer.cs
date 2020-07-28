namespace taskt.UI.CustomControls
{
    partial class CommandItemControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CommandItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Raleway", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CommandItemControl";
            this.Size = new System.Drawing.Size(258, 18);
            this.Load += new System.EventHandler(this.CommandItemControl_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CommandItemControl_Paint);
            this.MouseEnter += new System.EventHandler(this.CommandItemControl_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.CommandItemControl_MouseLeave);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
