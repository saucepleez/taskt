using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    internal static class SupplementFormsEvents
    {
        public static void SupplementFormLoad(Form fm)
        {
            if (fm.Owner is frmCommandEditor editor)
            {
                if (editor.Owner is frmScriptBuilder builder)
                {
                    builder.SetPositionChildFormOfCommandEditor(fm);
                }
            }
        }

        public static void SupplementFormClosed(object sender, EventArgs e)
        {
            if (sender is Form fm)
            {
                if (fm.Owner is frmCommandEditor editor)
                {
                    if (editor.Owner is frmScriptBuilder builder)
                    {
                        builder.StorePositionChildFormOfCommandEditor(fm);
                    }
                }
            }
        }
    }
}
