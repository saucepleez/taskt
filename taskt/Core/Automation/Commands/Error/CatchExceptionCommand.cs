using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.CommandSettings("Catch Exception")]
    [Attributes.ClassAttributes.Description("This command allows you to define actions that should occur after encountering an error.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to define how your script should behave when an error is encountered.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true, true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CatchExceptionCommand : ScriptCommand
    {
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }

        public CatchExceptionCommand()
        {
            //this.CommandName = "CatchExceptionCommand";
            //this.SelectionName = "Catch Exception";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //no execution required, used as a marker by the Automation Engine
        }
    }
}