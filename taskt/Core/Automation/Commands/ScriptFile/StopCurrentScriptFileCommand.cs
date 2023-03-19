using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Script File Commands")]
    [Attributes.ClassAttributes.CommandSettings("Stop Current Script File")]
    [Attributes.ClassAttributes.Description("This command stops the current task.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to stop the current running task.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true, true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]

    public class StopCurrentScriptFileCommand : ScriptCommand
    {
        public StopCurrentScriptFileCommand()
        {
            //this.CommandName = "StopTaskCommand";
            //this.SelectionName = "Stop Current Script File";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {

        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_Comment", this));
        //    RenderedControls.Add(CommandControls.CreateDefaultInputFor("v_Comment", this, 100, 300));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue();
        //}
    }
}