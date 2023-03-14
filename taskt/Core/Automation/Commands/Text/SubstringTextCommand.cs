using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Substring Text")]
    [Attributes.ClassAttributes.Description("This command allows you to trim a Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select a subset of text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SubstringTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Start Position")]
        [InputSpecification("Start Position", true)]
        [PropertyDetailSampleUsage("**0**", "Specify **First Charactor** for Start Position")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Start Position")]
        [PropertyDetailSampleUsage("**{{{vStart}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Start Position")]
        [PropertyValidationRule("Start Position", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Start")]
        public string v_startIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Extract Length")]
        [InputSpecification("Extract Length", true)]
        [PropertyDetailSampleUsage("**-1**", "Specify **Keep Remainder** for Extract Length")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Extract Length")]
        [PropertyDetailSampleUsage("**{{{vLength}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Extract Length")]
        [PropertyIsOptional(true, "-1")]
        [PropertyFirstValue("-1")]
        [PropertyDisplayText(true, "Length")]
        public string v_stringLength { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public SubstringTextCommand()
        {
            //this.CommandName = "SubstringTextCommand";
            //this.SelectionName = "Substring Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //v_stringLength = "-1";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var variableName = v_userVariableName.ConvertToUserVariable(engine);

            var startIndex = this.ConvertToUserVariableAsInteger(nameof(v_startIndex), engine);
            var stringLength = this.ConvertToUserVariableAsInteger(nameof(v_stringLength), engine);

            //apply substring
            string subStr;
            if (stringLength >= 0)
            {
                subStr = variableName.Substring(startIndex, stringLength);
            }
            else
            {
                subStr = variableName.Substring(startIndex);
            }

            subStr.StoreInUserVariable(engine, v_applyToVariableName);
        }
    }
}