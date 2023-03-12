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
        //[PropertyDescription("Please supply the value or variable")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Select or provide a variable or text value")]
        //[SampleUsage("**Hello** or **{{{vText}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        [PropertyDescription("Text")]
        [InputSpecification("Text", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Text")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyDisplayText(true, "Text")]
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
        [SampleUsage("1")]
        [PropertyFirstValue("0")]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Index")]
        public string v_MatchGroupIndex { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the variable to receive the RegEx result")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
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

            //get variablized strings
            var variableInput = v_InputValue.ConvertToUserVariable(engine);
            var variableExtractorPattern = v_RegExExtractor.ConvertToUserVariable(engine);
            
            //var variableMatchGroup = v_MatchGroupIndex.ConvertToUserVariable(engine);
            
            //create regex matcher
            Regex regex = new Regex(variableExtractorPattern);
            Match match = regex.Match(variableInput);

            //int matchGroup = 0;
            //if (!int.TryParse(variableMatchGroup, out matchGroup))
            //{
            //    matchGroup = 0;
            //}

            if (!match.Success)
            {
                //throw exception if no match found
                throw new Exception("RegEx Match was not found! Input: " + variableInput + ", Pattern: " + variableExtractorPattern);
            }
            else
            {
                //store string in variable
                var matchGroup = this.ConvertToUserVariableAsInteger(nameof(v_MatchGroupIndex), engine);

                string matchedValue = match.Groups[matchGroup].Value;
                matchedValue.StoreInUserVariable(sender, v_applyToVariableName);
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RegExExtractor", this, editor));

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MatchGroupIndex", this, editor));


        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_applyToVariableName", this, VariableNameControl, editor));
        //    RenderedControls.Add(VariableNameControl);

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Apply Extracted Text To Variable: " + v_applyToVariableName + "]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InputValue))
        //    {
        //        this.validationResult += "Value is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_RegExExtractor))
        //    {
        //        this.validationResult += "RegEx Extractor pattern is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_MatchGroupIndex))
        //    {
        //        this.validationResult += "Group index is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_applyToVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}