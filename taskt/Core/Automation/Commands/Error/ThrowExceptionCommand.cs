using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to throw an exception error.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to throw an exception error")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ThrowExceptionCommand : ScriptCommand
    {
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }

        public ThrowExceptionCommand()
        {
            this.CommandName = "ThrowExceptionCommand";
            this.SelectionName = "Throw Exception";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            throw new System.Exception();
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