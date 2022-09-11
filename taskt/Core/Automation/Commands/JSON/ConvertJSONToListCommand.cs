using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to convert JSON Array into a List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert JSON Array into a List")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertJSONToListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Supply the JSON Array or Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or json array value")]
        [SampleUsage("**[1,2,3]** or **[{obj1},{obj2}]** or **{{{vArrayVariable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.JSON)]
        [PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "JSON")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the List")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List")]
        public string v_applyToVariableName { get; set; }

        public ConvertJSONToListCommand()
        {
            this.CommandName = "ConvertJSONToListCommand";
            this.SelectionName = "Convert JSON To List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var variableInput = v_InputValue.ConvertToUserVariable(sender).Trim();
            if (variableInput.StartsWith("[") && variableInput.EndsWith("]"))
            {
                // JSON Array
                List<string> resultList = new List<string>();

                Newtonsoft.Json.Linq.JArray arr = Newtonsoft.Json.Linq.JArray.Parse(variableInput);

                foreach(var result in arr)
                {
                    resultList.Add(result.ToString());
                }

                resultList.StoreInUserVariable(engine, v_applyToVariableName);
            }
            else if (variableInput.StartsWith("{") && variableInput.EndsWith("}"))
            {
                // Object
                List<string> resultList = new List<string>();

                Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(variableInput);

                foreach(var result in obj)
                {
                    resultList.Add(result.Value.ToString());
                }

                resultList.StoreInUserVariable(engine, v_applyToVariableName);
            }
            else
            {
                throw new Exception("Strange JSON, can not convert List");
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Convert JSON '" + this.v_InputValue + "' To List " + this.v_applyToVariableName + "]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InputValue))
        //    {
        //        this.validationResult += "JSON Array is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_applyToVariableName))
        //    {
        //        this.validationResult += "Variable to recieve the List is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}