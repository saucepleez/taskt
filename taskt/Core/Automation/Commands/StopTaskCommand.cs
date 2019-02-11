using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Task Commands")]
    [Attributes.ClassAttributes.Description("This command stops the current task.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to stop the current running task.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class StopTaskCommand : ScriptCommand
    {


        public StopTaskCommand()
        {
            this.CommandName = "StopTaskCommand";
            this.SelectionName = "Stop Current Task";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}