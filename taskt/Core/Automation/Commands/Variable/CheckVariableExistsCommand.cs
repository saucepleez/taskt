using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.CommandSettings("Check Variable Exists")]
    [Attributes.ClassAttributes.Description("This command allows you to check variable existance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check variable existance.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CheckVariableExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Variable Name")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vSomeVariable**", PropertyDetailSampleUsage.ValueType.Value, "Variable Name")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the Variable to store result")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter the input that the variable's value should be set to.")]
        //[SampleUsage("**vResult** or **{{{vResult}}}**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Variable Exists, Result is **True**")]
        public string v_Result { get; set; }

        public CheckVariableExistsCommand()
        {
            this.CommandName = "CheckVariableExistsCommand";
            this.SelectionName = "Check Variable Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            //v_userVariableName = v_userVariableName.ConvertToUserVariable(engine);

            //var variableList = engine.VariableList;
            //foreach(var v in variableList)
            //{
            //    if (v.VariableName == v_userVariableName)
            //    {
            //        true.StoreInUserVariable(engine, v_Result);
            //        return;
            //    }
            //}

            //false.StoreInUserVariable(engine, v_Result);

            var variableName = VariableControls.GetVariableName(v_userVariableName, engine);
            VariableControls.IsVariableExists(variableName, engine).StoreInUserVariable(engine, v_Result);
        }

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Check Variable '" + v_userVariableName + "', Result '" + v_Result + "']";
        //}

        //public override List<Control> Render(UI.Forms.frmCommandEditor editor)
        //{
        //    //custom rendering
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    return RenderedControls;
        //}
    }
}