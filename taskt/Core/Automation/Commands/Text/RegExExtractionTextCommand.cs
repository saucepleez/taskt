using System;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.Description("This command allows you to perform advanced string formatting using RegEx.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform an advanced RegEx extraction from a text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class RegExExtractionTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please supply the value or variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or text value")]
        [SampleUsage("**Hello** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Input the RegEx Extractor Pattern")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the RegEx extractor pattern that should be used to extract the text")]
        [SampleUsage("**\\w+** or **^([\\w\\-]+)**")]
        [Remarks("If an extractor splits each word in a sentence, for example, you will need to specify the associated index of the word that is required.")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_RegExExtractor { get; set; }

        [XmlAttribute]
        [PropertyDescription("Select Matching Group Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define the index of the result")]
        [SampleUsage("1")]
        [Remarks("The extractor will split multiple patterns found into multiple indexes.  Test which index is required to retrieve the value or create a better/more define extractor.")]
        public string v_MatchGroupIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the RegEx result")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        public string v_applyToVariableName { get; set; }

        public RegExExtractionTextCommand()
        {
            this.CommandName = "RegExExtractionTextCommand";
            this.SelectionName = "RegEx Extraction Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //apply default
            v_MatchGroupIndex = "0";
        }

        public override void RunCommand(object sender)
        {
            //get variablized strings
            var variableInput = v_InputValue.ConvertToUserVariable(sender);
            var variableExtractorPattern = v_RegExExtractor.ConvertToUserVariable(sender);
            var variableMatchGroup = v_MatchGroupIndex.ConvertToUserVariable(sender);

            //create regex matcher
            Regex regex = new Regex(variableExtractorPattern);
            Match match = regex.Match(variableInput);

            int matchGroup = 0;
            if (!int.TryParse(variableMatchGroup, out matchGroup))
            {
                matchGroup = 0;
            }

            if (!match.Success)
            {
                //throw exception if no match found
                throw new Exception("RegEx Match was not found! Input: " + variableInput + ", Pattern: " + variableExtractorPattern);
            }
            else
            {
                //store string in variable
                string matchedValue = match.Groups[matchGroup].Value;
                matchedValue.StoreInUserVariable(sender, v_applyToVariableName);
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RegExExtractor", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MatchGroupIndex", this, editor));


            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_applyToVariableName", this, VariableNameControl, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Extracted Text To Variable: " + v_applyToVariableName + "]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InputValue))
            {
                this.validationResult += "Value is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_RegExExtractor))
            {
                this.validationResult += "RegEx Extractor pattern is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_MatchGroupIndex))
            {
                this.validationResult += "Group index is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_applyToVariableName))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}