using System;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("RegEx Extraction Text")]
    [Attributes.ClassAttributes.Description("This command allows you to perform advanced string formatting using RegEx.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform an advanced RegEx extraction from a text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RegExExtractionTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("RegEx Extractor Pattern")]
        [InputSpecification("RegEx Extractor Pattern", true)]
        [Remarks("If an extractor splits each word in a sentence, for example, you will need to specify the associated index of the word that is required.")]
        [SampleUsage("**\\w+** or **^([\\w\\-]+)**")]
        [PropertyValidationRule("Extractor Pattern", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Extractor Pattern")]
        public string v_RegExExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Matching Group Index")]
        [InputSpecification("Index", true)]
        [Remarks("The extractor will split multiple patterns found into multiple indexes.  Test which index is required to retrieve the value or create a better/more define extractor.")]
        //[SampleUsage("1")]
        [PropertyDetailSampleUsage("**0**", "Specify the First Matche")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index")]
        [PropertyDetailSampleUsage("**-1**", "Specify the Last Matches")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index")]
        [PropertyFirstValue("0")]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Index")]
        public string v_MatchGroupIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public RegExExtractionTextCommand()
        {
            //this.CommandName = "RegExExtractionTextCommand";
            //this.SelectionName = "RegEx Extraction Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            ////apply default
            //v_MatchGroupIndex = "0";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            ////get variablized strings
            //var variableInput = v_InputValue.ConvertToUserVariable(engine);
            //var variableExtractorPattern = v_RegExExtractor.ConvertToUserVariable(engine);

            ////create regex matcher
            //Regex regex = new Regex(variableExtractorPattern);
            //Match match = regex.Match(variableInput);

            //if (!match.Success)
            //{
            //    //throw exception if no match found
            //    throw new Exception("RegEx Match was not found! Input: " + variableInput + ", Pattern: " + variableExtractorPattern);
            //}
            //else
            //{
            //    //store string in variable
            //    var matchGroup = this.ConvertToUserVariableAsInteger(nameof(v_MatchGroupIndex), engine);

            //    string matchedValue = match.Groups[matchGroup].Value;
            //    matchedValue.StoreInUserVariable(sender, v_applyToVariableName);
            //}

            var variableInput = v_InputValue.ConvertToUserVariable(engine);
            var variableExtractorPattern = v_RegExExtractor.ConvertToUserVariable(engine);

            var regex = new Regex(variableExtractorPattern);
            var matches = regex.Match(variableInput);
            if (matches.Groups.Count > 0)
            {
                var matchGroup = this.ConvertToUserVariableAsInteger(nameof(v_MatchGroupIndex), engine);

                if (matchGroup < 0)
                {
                    matchGroup += matches.Groups.Count;
                }
                if (matchGroup >= matches.Groups.Count)
                {
                    throw new Exception("Match Group Index is out of Range.");
                }

                matches.Groups[matchGroup].Value.StoreInUserVariable(engine, v_applyToVariableName);
            }
            else
            {
                throw new Exception("RegEx Match was not found! Input: " + variableInput + ", Pattern: " + variableExtractorPattern);
            }
        }
    }
}