using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.CommandSettings("Finally")]
    [Attributes.ClassAttributes.Description("This command specifies execution that should occur whether or not an error occured")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to always execute a specific command before leaving the try/catch block")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true, true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class FinallyCommand : ScriptCommand
    {
        public FinallyCommand()
        {
            //this.CommandName = "FinallyCommand";
            //this.SelectionName = "Finally";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender, Script.ScriptAction parentCommand)
        {
            //no execution required, used as a marker by the Automation Engine
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Comment", this, editor));
        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(new List<string>() { nameof(v_Comment) }, this, editor));
        //    return RenderedControls;
        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue();
        //}
    }
}