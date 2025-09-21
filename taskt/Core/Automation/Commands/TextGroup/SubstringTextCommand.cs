using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Substring Text")]
    [Attributes.ClassAttributes.Description("This command allows you to trim a Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select a subset of text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SubstringTextCommand : ScriptCommand
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
        [PropertyDetailSampleUsage("**-1**", "Specify **Last Charactor** for Start Position")]
        [PropertyDetailSampleUsage("**{{{vStart}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Start Position")]
        [PropertyValidationRule("Start Position", PropertyValidationRule.ValidationRuleFlags.Empty)]
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

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Specified Invalid Position")]
        [PropertyUISelectionOption("Rise A Error")]
        [PropertyUISelectionOption("Get Empty Text")]
        [PropertyDetailSampleUsage("Rise A Error", "Rise A Error")]
        [PropertyDetailSampleUsage("Get Empty Text", "Get Empty Text")]
        [PropertyIsOptional(true, "Get Empty Text")]
        [PropertyFirstValue("Get Empty Text")]
        [PropertyDisplayText(false, "")]
        public string v_WhenInvalidIndex { get; set; }

        public SubstringTextCommand()
        {
            //this.CommandName = "SubstringTextCommand";
            //this.SelectionName = "Substring Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //v_stringLength = "-1";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var targetText = v_userVariableName.ExpandValueOrUserVariable(engine);
            var targetTextLength = targetText.Length;

            var startIndex = this.ExpandValueOrUserVariableAsInteger(nameof(v_startIndex), engine);
            if (startIndex < 0)
            {
                startIndex += targetTextLength;
            }

            if (string.IsNullOrEmpty(v_stringLength))
            {
                v_stringLength = "-1";
            }
            var stringLength = this.ExpandValueOrUserVariableAsInteger(nameof(v_stringLength), engine);

            string subStr = "";
            if (startIndex < 0 || startIndex >= targetTextLength)
            {
                // invalid start index
                switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenInvalidIndex), engine))
                {
                    case "rise a error":
                        throw new Exception($"Invalid Start Index. Value: '{v_startIndex}', Expand Value: '{startIndex}'");

                    case "get empty text":
                        subStr = "";
                        break;
                }
            }
            else if (stringLength >= 0)
            {
                // substring range
                if (startIndex + stringLength >= targetTextLength)
                {
                    subStr = targetText.Substring(startIndex);
                }
                else
                {
                    subStr = targetText.Substring(startIndex, stringLength);
                }
            }
            else
            {   // (stringLength < 0)
                // substring after...
                subStr = targetText.Substring(startIndex);
            }
            subStr.StoreInUserVariable(engine, v_applyToVariableName);
        }
    }
}