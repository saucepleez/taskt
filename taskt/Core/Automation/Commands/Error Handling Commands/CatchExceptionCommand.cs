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
    [Description("This command defines a catch block whose commands will execute if an exception is thrown from the " +
                 "associated try.")]
    public class CatchExceptionCommand : ScriptCommand
    {
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }

        public CatchExceptionCommand()
        {
            CommandName = "CatchExceptionCommand";
            SelectionName = "Catch Exception";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //no execution required, used as a marker by the Automation Engine
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