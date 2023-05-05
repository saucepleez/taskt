using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script Commands")]
    [Attributes.ClassAttributes.SubGruop("taskt Script File")]
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
            var engine = (Engine.AutomationEngineInstance)sender;
            engine.IsCancellationPending = true;
        }
    }
}