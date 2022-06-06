using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.Description("This command allows you to trim a Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select a subset of text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    public class SubstringTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a variable or text")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or text value")]
        [SampleUsage("**Hello** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Start from Position")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Indicate the starting position within the string")]
        [SampleUsage("**1** or **{{{vPosition}}}**")]
        [Remarks("0 for beginning, 1 for first character, etc.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Start Position", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_startIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Extract Length (-1 to keep remainder)")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Indicate if only so many characters should be kept")]
        [SampleUsage("**1** or **-1** or **{{{vLength}}}**")]
        [Remarks("-1 to keep remainder, 1 for 1 position after start index.")]
        [PropertyIsOptional(true, "-1")]
        [PropertyFirstValue("-1")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        public string v_stringLength { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the Result")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_applyToVariableName { get; set; }

        public SubstringTextCommand()
        {
            this.CommandName = "SubstringTextCommand";
            this.SelectionName = "Substring Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            //v_stringLength = "-1";
        }
        public override void RunCommand(object sender)
        {
            Engine.AutomationEngineInstance engine = (Engine.AutomationEngineInstance)sender;

            var variableName = v_userVariableName.ConvertToUserVariable(sender);

            int startIndex, stringLength;

            //int.TryParse(v_startIndex.ConvertToUserVariable(sender), out startIndex);

            //v_stringLength = v_stringLength.ConvertToUserVariable(sender);
            //if (v_stringLength == "")
            //{
            //    v_stringLength = "-1";
            //}
            //int.TryParse(v_stringLength, out stringLength);

            startIndex = v_startIndex.ConvertToUserVariableAsInteger("Start Position", engine);

            if (String.IsNullOrEmpty(v_stringLength))
            {
                v_stringLength = "-1";
            }
            stringLength = v_stringLength.ConvertToUserVariableAsInteger("Length", engine);


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

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_userVariableName", this, editor));

            ////create standard group controls
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_startIndex", this, editor));

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_stringLength", this, editor));

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);

            RenderedControls.AddRange(ctrls);

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Substring to '" + v_userVariableName + "', Start " + v_startIndex + ", Length " + v_stringLength + " ]";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_userVariableName))
        //    {
        //        this.validationResult += "Text is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_startIndex))
        //    {
        //        this.validationResult += "Start is empty.\n";
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