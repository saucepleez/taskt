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
    [Attributes.ClassAttributes.Description("This command allows you to parse a JSON Array into a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract data from a JSON object")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ParseJSONArrayCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Supply the JSON Array or Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or json array value")]
        [SampleUsage("**[1,2,3]** or **[{obj1},{obj2}]** or **{{{vArrayVariable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
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
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List")]
        public string v_applyToVariableName { get; set; }

        public ParseJSONArrayCommand()
        {
            this.CommandName = "ParseJSONArrayCommand";
            this.SelectionName = "Parse JSON Array";
            this.CommandEnabled = true;
            this.CustomRendering = true; 
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //get variablized input
            var variableInput = v_InputValue.ConvertToUserVariable(sender);

            //create objects
            Newtonsoft.Json.Linq.JArray arr;
            List<string> resultList = new List<string>();

            //parse json
            try
            {
                arr = Newtonsoft.Json.Linq.JArray.Parse(variableInput);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
            }
 
            //add results to result list since list<string> is supported
            foreach (var result in arr)
            {
                resultList.Add(result.ToString());
            }

            resultList.StoreInUserVariable(engine, v_applyToVariableName);

        }
        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Apply Result(s) To List Variable: " + this.v_applyToVariableName + "]";
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
        //        this.validationResult += "Variable to recieve the list is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}