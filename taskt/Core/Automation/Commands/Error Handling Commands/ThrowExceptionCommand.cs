using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Error Handling Commands")]
    [Description("This command throws an exception during script execution.")]
    public class ThrowExceptionCommand : ScriptCommand
    {
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }

        public ThrowExceptionCommand()
        {
            CommandName = "ThrowExceptionCommand";
            SelectionName = "Throw Exception";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            throw new Exception();
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Comment", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}