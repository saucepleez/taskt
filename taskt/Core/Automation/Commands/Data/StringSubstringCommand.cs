using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.SubGruop("Text")]
    [Attributes.ClassAttributes.Description("This command allows you to trim a string")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select a subset of text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    public class StringSubstringCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable or text")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or text value")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello** or **{{{vText}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]

        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Start from Position")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the starting position within the string")]
        [Attributes.PropertyAttributes.SampleUsage("**1** or **{{{vPosition}}}**")]
        [Attributes.PropertyAttributes.Remarks("0 for beginning, 1 for first character, etc.")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_startIndex { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Length (-1 to keep remainder) (Default is -1)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Indicate if only so many characters should be kept")]
        [Attributes.PropertyAttributes.SampleUsage("**1** or **-1** or **{{{vLength}}}**")]
        [Attributes.PropertyAttributes.Remarks("-1 to keep remainder, 1 for 1 position after start index.")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_stringLength { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the changes")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_applyToVariableName { get; set; }
        public StringSubstringCommand()
        {
            this.CommandName = "StringSubstringCommand";
            this.SelectionName = "Substring";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            v_stringLength = "-1";
        }
        public override void RunCommand(object sender)
        {
            var variableName = v_userVariableName.ConvertToUserVariable(sender);

            int startIndex, stringLength;
            int.TryParse(v_startIndex.ConvertToUserVariable(sender), out startIndex);

            v_stringLength = v_stringLength.ConvertToUserVariable(sender);
            if (v_stringLength == "")
            {
                v_stringLength = "-1";
            }
            int.TryParse(v_stringLength, out stringLength);

            //apply substring
            if (stringLength >= 0)
            {
                variableName = variableName.Substring(startIndex, stringLength);
            }
            else
            {
                variableName = variableName.Substring(startIndex);
            }

            variableName.StoreInUserVariable(sender, v_applyToVariableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_userVariableName", this, editor));

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_startIndex", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_stringLength", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Substring to '" + v_userVariableName + "', Start " + v_startIndex + ", Length " + v_stringLength + " ]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_userVariableName))
            {
                this.validationResult += "Text is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_startIndex))
            {
                this.validationResult += "Start is empty.\n";
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