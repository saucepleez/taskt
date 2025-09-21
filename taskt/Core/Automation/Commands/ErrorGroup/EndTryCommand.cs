using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling")]
    [Attributes.ClassAttributes.CommandSettings("End Try")]
    [Attributes.ClassAttributes.Description("This command specifies the end of a try/catch block.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to enclose your try/catch block.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_try))]
    [Attributes.ClassAttributes.EnableAutomateRender(true, true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class EndTryCommand : ScriptCommand, IEndOfStacturedCommand
    {
        public EndTryCommand()
        {
            //this.CommandName = "EndTryCommand";
            //this.SelectionName = "End Try";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
           //no execution required, used as a marker by the Automation Engine
        }
    }
}