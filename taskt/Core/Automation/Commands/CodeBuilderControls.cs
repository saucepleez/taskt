using System;
using System.Windows.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    static internal class CodeBuilderControls
    {
        public static void ShowCodeBuilderLink_Clicked(object sender, EventArgs e)
        {
            var targetTextbox = (TextBox)((CommandItemControl)sender).Tag;
            using (UI.Forms.Supplemental.frmCodeBuilder codeBuilder = new UI.Forms.Supplemental.frmCodeBuilder(targetTextbox.Text))
            {
                if (codeBuilder.ShowDialog() == DialogResult.OK)
                {
                    targetTextbox.Text = codeBuilder.rtbCode.Text;
                }
            }
        }
    }
}
