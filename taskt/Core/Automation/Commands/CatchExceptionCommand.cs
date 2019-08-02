using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to define actions that should occur after encountering an error.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to define how your script should behave when an error is encountered.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class CatchExceptionCommand : ScriptCommand
    {
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }

        public CatchExceptionCommand()
        {
            this.CommandName = "CatchExceptionCommand";
            this.SelectionName = "Catch Exception";
            this.CommandEnabled = true;
            this.CustomRendering = true;
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