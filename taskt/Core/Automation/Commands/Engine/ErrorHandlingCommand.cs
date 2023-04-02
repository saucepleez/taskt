using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.CommandSettings("Error Handling")]
    [Attributes.ClassAttributes.Description("This command specifies what to do  after an error is encountered.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to define how your script should behave when an error is encountered.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ErrorHandlingCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Action to Take in the Event an Error Occurs")]
        [PropertyUISelectionOption("Stop Processing")]
        [PropertyUISelectionOption("Continue Processing")]
        [PropertyDetailSampleUsage("**Stop Processing**", "End the Script if an Error is Encountered")]
        [PropertyDetailSampleUsage("**Continue Processing**", "Continue Running the Script")]
        [PropertyDetailSampleUsage("**{{{vAction}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Action")]
        [Remarks("**If Command** allows you to specify and test if a line number encountered an error. In order to use that functionality, you must specify **Continue Processing**")]
        [PropertyValidationRule("Action", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Action")]
        public string v_ErrorHandlingAction { get; set; }

        public ErrorHandlingCommand()
        {
            //this.CommandName = "ErrorHandlingCommand";
            //this.SelectionName = "Error Handling";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var act = this.GetUISelectionValue(nameof(v_ErrorHandlingAction), engine);
            switch (act)
            {
                case "stop processing":
                    v_ErrorHandlingAction = "Stop Processing";
                    break;
                case "continue processing":
                    v_ErrorHandlingAction = "Continue Processing";
                    break;
            }
            engine.ErrorHandler = this;
        }
    }
}