using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Text.RegularExpressions;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Regex Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Regex Matches")]
    [Attributes.ClassAttributes.Description("This command allows you to loop through an Excel Dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to iterate over a series of Excel cells.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to loop through a known Excel DataSet")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetRegexMatchesCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_InputData { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Regex Expression")]
        [InputSpecification("Reget Expression", true)]
        [PropertyDetailSampleUsage("**\\w**", PropertyDetailSampleUsage.ValueType.Value, "Regex")]
        [PropertyDetailSampleUsage("**{{{vRegex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Regex")]
        [PropertyValidationRule("Regex", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Reget")]
        public string v_Regex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("If the Matched Text is **A**, **B**, and **C**, the result will be **A,B,C**")]
        public string v_OutputVariable { get; set; }

        public GetRegexMatchesCommand()
        {
            //this.CommandName = "GetRegexMatchesCommand";
            //this.SelectionName = "Get Regex Matches";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var vInputData = v_InputData.ConvertToUserVariable(engine);
            var vRegex = v_Regex.ConvertToUserVariable(engine);

            Match[] matches = Regex.Matches(vInputData, vRegex)
                       .Cast<Match>()
                       .ToArray();
            var arr = string.Join(",", matches.AsEnumerable());

            arr.StoreInUserVariable(engine, v_OutputVariable);
        }
    }
}