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
    [Attributes.ClassAttributes.Description("This command allows you to convert JSON to Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert JSON to Dictionary")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertJSONToDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Supply the JSON Object or Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or json array value")]
        [SampleUsage("**{\"id\":123, \"name\": \"John\"}** or **{{{vJSON}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.JSON)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "JSON")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the Dictionary")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary")]
        public string v_applyToVariableName { get; set; }

        public ConvertJSONToDictionaryCommand()
        {
            this.CommandName = "ConvertJSONToDictionaryCommand";
            this.SelectionName = "Convert JSON To Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var variableInput = v_InputValue.ConvertToUserVariable(sender).Trim();
            if (variableInput.StartsWith("{") && variableInput.EndsWith("}"))
            {
                Dictionary<string, string> resultDic = new Dictionary<string, string>();
                Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(variableInput);

                foreach(var result in obj)
                {
                    resultDic.Add(result.Key, result.Value.ToString());
                }
                resultDic.StoreInUserVariable(engine, v_applyToVariableName);
            }
            else if (variableInput.StartsWith("[") && variableInput.EndsWith("]"))
            {
                Dictionary<string, string> resultDic = new Dictionary<string, string>();
                Newtonsoft.Json.Linq.JArray arr = Newtonsoft.Json.Linq.JArray.Parse(variableInput);

                for (int i = 0; i < arr.Count; i++)
                {
                    resultDic.Add("key" + i.ToString(), arr[i].ToString());
                }
                resultDic.StoreInUserVariable(engine, v_applyToVariableName);
            }
            else
            {
                throw new Exception("Strange JSON");
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
        //    return base.GetDisplayValue() + " [Convert JSON " + this.v_InputValue + " To Dictionary " + this.v_applyToVariableName + "]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InputValue))
        //    {
        //        this.validationResult += "JSON Object is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_applyToVariableName))
        //    {
        //        this.validationResult += "Variable to recieve the Dictionary is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}