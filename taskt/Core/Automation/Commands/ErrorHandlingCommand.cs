using System;
using System.Xml.Serialization;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command specifies what to do  after an error is encountered.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to define how your script should behave when an error is encountered.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class ErrorHandlingCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Action On Error")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Stop Processing")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Continue Processing")]
        [Attributes.PropertyAttributes.InputSpecification("Select the action you want to take when you come across an error.")]
        [Attributes.PropertyAttributes.SampleUsage("**Stop Processing** to end the script if an error is encountered or **Continue Processing** to continue running the script")]
        [Attributes.PropertyAttributes.Remarks("**If Command** allows you to specify and test if a line number encountered an error. In order to use that functionality, you must specify **Continue Processing**")]
        public string v_ErrorHandlingAction { get; set; }

        public ErrorHandlingCommand()
        {
            this.CommandName = "ErrorHandlingCommand";
            this.SelectionName = "Error Handling";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            engine.ErrorHandler = this;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Action: " + v_ErrorHandlingAction + "]";
        }
    }
}