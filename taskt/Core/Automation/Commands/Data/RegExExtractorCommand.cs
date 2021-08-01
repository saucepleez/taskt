using System;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.SubGruop("Text")]
    [Attributes.ClassAttributes.Description("This command allows you to perform advanced string formatting using RegEx.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform an advanced RegEx extraction from a text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class RegExExtractorCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the value or variable")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or text value")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello** or **{{{vText}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Input the RegEx Extractor Pattern")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the RegEx extractor pattern that should be used to extract the text")]
        [Attributes.PropertyAttributes.SampleUsage("**\\w+** or **^([\\w\\-]+)**")]
        [Attributes.PropertyAttributes.Remarks("If an extractor splits each word in a sentence, for example, you will need to specify the associated index of the word that is required.")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_RegExExtractor { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Matching Group Index")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the index of the result")]
        [Attributes.PropertyAttributes.SampleUsage("1")]
        [Attributes.PropertyAttributes.Remarks("The extractor will split multiple patterns found into multiple indexes.  Test which index is required to retrieve the value or create a better/more define extractor.")]
        public string v_MatchGroupIndex { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the RegEx result")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_applyToVariableName { get; set; }

        public RegExExtractorCommand()
        {
            this.CommandName = "RegExExtractorCommand";
            this.SelectionName = "RegEx Extraction";
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
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
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