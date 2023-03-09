using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.CommandSettings("Set Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to modify variables.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify the value of variables.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class VariableCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Variable Name")]
        //[InputSpecification("Variable Name", true)]
        //[PropertyDetailSampleUsage("**vSomeVariable**", PropertyDetailSampleUsage.ValueType.Value, "Variable Name")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        //[PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Variable")]
        [PropertyVirtualProperty(nameof(VariableNameControls), nameof(VariableNameControls.v_VariableName))]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Variable Value")]
        //[InputSpecification("Variable Value", true)]
        //[PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Variable Value")]
        //[PropertyDetailSampleUsage("**{{{vNum}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Variable Value")]
        //[Remarks("")]
        //[PropertyIsOptional(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "Value")]
        [PropertyVirtualProperty(nameof(VariableNameControls), nameof(VariableNameControls.v_VariableValue))]
        public string v_Input { get; set; }

        [XmlAttribute]
        [PropertyDescription("Convert Variables in Input Text Above")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("", true)]
        [Remarks("If **{{{vNum}}}** has **'1'** and You select **'Yes'**, Variable will be Assigned **'1'**. If You Select **'No'**, Variable will be assigned **'{{{vNum}}}'**.")]
        [PropertyIsOptional(true, "Yes")]
        public string v_ReplaceInputVariables { get; set; }

        public VariableCommand()
        {
            //this.CommandName = "VariableCommand";
            //this.SelectionName = "Set Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_ReplaceInputVariables = "Yes";
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var isRepalce = this.GetUISelectionValue(nameof(v_ReplaceInputVariables), engine);
            string variableValue;
            if (isRepalce == "yes")
            {
                variableValue = v_Input.ConvertToUserVariable(engine);
            }
            else
            {
                variableValue = v_Input;
            }

            var variableName = VariableNameControls.GetVariableName(v_userVariableName, engine);
            if (VariableNameControls.IsVariableExists(variableName, engine))
            {
                variableValue.StoreInUserVariable(engine, variableName);
            }
            else
            {
                throw new Exception("Variable Name '" + variableName + "' does not exists.");
            }
        }
    }
}