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
        //[PropertyDescription("Please select a variable or text")]
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
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Start Position")]
        [InputSpecification("Start Position", true)]
        //[SampleUsage("**1** or **{{{vPosition}}}**")]
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
        //[SampleUsage("**1** or **-1** or **{{{vLength}}}**")]
        [PropertyDetailSampleUsage("**-1**", "Specify **Keep Remainder** for Extract Length")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Extract Length")]
        [PropertyDetailSampleUsage("**{{{vLength}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Extract Length")]
        [PropertyIsOptional(true, "-1")]
        [PropertyFirstValue("-1")]
        [PropertyDisplayText(true, "Length")]
        public string v_stringLength { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the variable to receive the Result")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
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

            //int startIndex, stringLength;
            //startIndex = v_startIndex.ConvertToUserVariableAsInteger("Start Position", engine);

            //if (String.IsNullOrEmpty(v_stringLength))
            //{
            //    v_stringLength = "-1";
            //}
            //stringLength = v_stringLength.ConvertToUserVariableAsInteger("Length", engine);

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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_userVariableName", this, editor));

        //    ////create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_startIndex", this, editor));

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_stringLength", this, editor));

        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);

        //    RenderedControls.AddRange(ctrls);

        //    return RenderedControls;

        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Apply Substring to '" + v_userVariableName + "', Start " + v_startIndex + ", Length " + v_stringLength + " ]";
        //}

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