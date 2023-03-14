using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Replace Text")]
    [Attributes.ClassAttributes.Description("This command allows you to replace text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to replace existing text within text or a variable with new text")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ReplaceTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text to be Replaced")]
        [InputSpecification("Text to be Replaced", true)]
        [PropertyDetailSampleUsage("**H**", PropertyDetailSampleUsage.ValueType.Value, "Text to be Replaced")]
        [PropertyDetailSampleUsage("**{{{vTextA}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text to be Replaced")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Text to be Replaced", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "To be Replaced")]
        public string v_replacementText { get; set; }

        [XmlAttribute]
        [PropertyDescription("Replacement Text")]
        [InputSpecification("Replacement Text", true)]
        [PropertyDetailSampleUsage("**J**", PropertyDetailSampleUsage.ValueType.Value, "Replacement Text")]
        [PropertyDetailSampleUsage("**{{{vTextB}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Replacement Text")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(true, "Replacement")]
        public string v_replacementValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public ReplaceTextCommand()
        {
            //this.CommandName = "ReplaceTextCommand";
            //this.SelectionName = "Replace Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //get full text
            string targetText = v_userVariableName.ConvertToUserVariable(engine);

            //get replacement text and value
            string replacementText = v_replacementText.ConvertToUserVariable(engine);
            string replacementValue = v_replacementValue.ConvertToUserVariable(engine);

            //perform replacement
            targetText = targetText.Replace(replacementText, replacementValue);

            //store in variable
            targetText.StoreInUserVariable(engine, v_applyToVariableName);
        }
    }
}