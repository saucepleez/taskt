using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.CommandSettings("End Try")]
    [Attributes.ClassAttributes.Description("This command specifies the end of a try/catch block.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to enclose your try/catch block.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true, true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class EndTryCommand : ScriptCommand
    {
        public EndTryCommand()
        {
            //this.CommandName = "EndTryCommand";
            //this.SelectionName = "End Try";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
           //no execution required, used as a marker by the Automation Engine
        }
    }
}