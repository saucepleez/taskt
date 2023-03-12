using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Check/Get")]
    [Attributes.ClassAttributes.CommandSettings("Get Word Count")]
    [Attributes.ClassAttributes.Description("This command allows you to you to retrieve the word count of a Text or Variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to find word count of a Text or Variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetWordCountCommand : ScriptCommand
    {
        [XmlAttribute]
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
        //[PropertyDescription("Please select the Variable to Receive Result Word Count")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Resut", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public GetWordCountCommand()
        {
            //this.CommandName = "GetWordCountCommand";
            //this.SelectionName = "Get Word Count";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get engine
            var engine = (Engine.AutomationEngineInstance)sender;

            //get input value
            var stringRequiringCount = v_InputValue.ConvertToUserVariable(engine);

            //count number of words
            var wordCount = stringRequiringCount.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries).Length;

            //store word count into variable
            wordCount.ToString().StoreInUserVariable(engine, v_applyToVariableName);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));

        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Apply Result to: '" + v_applyToVariableName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InputValue))
        //    {
        //        this.validationResult += "Value is empty.\n";
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