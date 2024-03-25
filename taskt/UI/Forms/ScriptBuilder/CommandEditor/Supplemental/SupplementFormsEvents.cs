using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder.CommandEditor.Supplemental
{
    internal static class SupplementFormsEvents
    {
        public static void SupplementFormLoad(Form fm)
        {
            if (fm.Owner is ScriptBuilder.CommandEditor.frmCommandEditor editor)
            {
                if (editor.Owner is ScriptBuilder.frmScriptBuilder builder)
                {
                    if (builder.appSettings.ClientSettings.RememberSupplementFormsForCommandEditorPosition)
                    {
                        builder.SetPositionChildFormOfCommandEditor(fm);
                    }
                }
            }
        }

        public static void SupplementFormClosed(object sender, EventArgs e)
        {
            if (sender is Form fm)
            {
                if (fm.Owner is ScriptBuilder.CommandEditor.frmCommandEditor editor)
                {
                    if (editor.Owner is ScriptBuilder.frmScriptBuilder builder)
                    {
                        builder.StorePositionChildFormOfCommandEditor(fm);
                    }
                }
            }
        }
    }
}
