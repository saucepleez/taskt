using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command signifies the current loop should exit and resume work past the point of the current loop.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to signify that looping should end and commands outside the loop should resume execution.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is used by the engine to exit a loop")]
    public class ExitLoopCommand : ScriptCommand
    {
        public ExitLoopCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "ExitLoopCommand";
            this.SelectionName = "Exit Loop";
            this.CommandEnabled = true;
        }

        public override string GetDisplayValue()
        {
            return "Exit Loop";
        }
    }
}