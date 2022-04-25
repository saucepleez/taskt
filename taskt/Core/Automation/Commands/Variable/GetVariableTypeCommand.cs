using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to get variable type.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get variable type.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class GetVariableTypeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a variable to get type")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vVariable** **{{{vVariable}}}**")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Variable Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [PropertyDescription("Please specify the Variable to store variable type")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the input that the variable's value should be set to.")]
        [SampleUsage("**vResult** or **{{{vResult}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        public string v_Result { get; set; }

        public GetVariableTypeCommand()
        {
            this.CommandName = "GetVariableTypeCommand";
            this.SelectionName = "Get Variable Type";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            string variableName = v_userVariableName.ConvertToUserVariable(engine);

            var variableList = engine.VariableList;
            foreach(var v in variableList)
            {
                if (v.VariableName == variableName)
                {
                    object variableValue = v.VariableValue;
                    if (variableValue is DataTable)
                    {
                        "DATATABLE".StoreInUserVariable(engine, v_Result);
                    }
                    else if (variableValue is Dictionary<string, string>)
                    {
                        "DICTIONARY".StoreInUserVariable(engine, v_Result);
                    }
                    else if (variableValue is List<string>)
                    {
                        "LIST".StoreInUserVariable(engine, v_Result);
                    }
                    else if (variableValue is string)
                    {
                        "BASIC".StoreInUserVariable(engine, v_Result);
                    }
                    else
                    {
                        "UNKNOWN".StoreInUserVariable(engine, v_Result);
                    }
                    return;
                }
            }

            throw new Exception("Variable Name '" + v_userVariableName + "' does not exists");
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Check Variable '" + v_userVariableName + "', Result '" + v_Result + "']";
        }

        public override List<Control> Render(UI.Forms.frmCommandEditor editor)
        {
            //custom rendering
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }
    }
}