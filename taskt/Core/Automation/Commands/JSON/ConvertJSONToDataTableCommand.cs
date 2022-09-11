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
    [Attributes.ClassAttributes.Description("This command allows you to convert JSON to DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert JSON to DataTable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertJSONToDataTableCommand : ScriptCommand
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
        [PropertyDescription("Please select the variable to receive the DataTable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable")]
        public string v_applyToVariableName { get; set; }

        public ConvertJSONToDataTableCommand()
        {
            this.CommandName = "ConvertJSONToDataTableCommand";
            this.SelectionName = "Convert JSON To DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var variableInput = v_InputValue.ConvertToUserVariable(sender).Trim();
            if (variableInput.StartsWith("{") && variableInput.EndsWith("}"))
            {
                DataTable resultDT = new DataTable();
                Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(variableInput);

                resultDT.Rows.Add();
                int i = 0;
                foreach(var result in obj)
                {
                    resultDT.Columns.Add(result.Key);
                    resultDT.Rows[0][i] = result.Value.ToString();
                    i++;
                }
                resultDT.StoreInUserVariable(engine, v_applyToVariableName);
            }
            else if (variableInput.StartsWith("[") && variableInput.EndsWith("]"))
            {
                DataTable resultDT = new DataTable();
                Newtonsoft.Json.Linq.JArray arr = Newtonsoft.Json.Linq.JArray.Parse(variableInput);

                parseJSONArrayAsDataTable(arr, resultDT).StoreInUserVariable(engine, v_applyToVariableName);
            }
            else
            {
                throw new Exception("Strange JSON");
            }
        }

        private static DataTable parseJSONArrayAsDataTable(Newtonsoft.Json.Linq.JArray arr, DataTable DT)
        {
            var arr0 = arr[0].ToString();
            if (arr0.StartsWith("{") && arr0.EndsWith("}"))
            {
                // Object
                Newtonsoft.Json.Linq.JObject col = Newtonsoft.Json.Linq.JObject.Parse(arr[0].ToString());
                int colSize = col.Count;
                DT.Rows.Add();
                foreach (var co in col)
                {
                    DT.Columns.Add(co.Key);
                    DT.Rows[0][co.Key] = co.Value.ToString();
                }

                for (int i = 1; i < arr.Count; i++)
                {
                    DT.Rows.Add();
                    Newtonsoft.Json.Linq.JObject row = Newtonsoft.Json.Linq.JObject.Parse(arr[i].ToString());
                    foreach(var co in row)
                    {
                        DT.Rows[i][co.Key] = co.Value.ToString();
                    }
                }
            }
            else if (arr0.StartsWith("[") && arr0.EndsWith("]"))
            {
                // 2Array
                Newtonsoft.Json.Linq.JArray col = Newtonsoft.Json.Linq.JArray.Parse(arr[0].ToString());
                int colSize = col.Count;
                DT.Rows.Add();
                for (int i = 0; i < colSize; i++)
                {
                    DT.Columns.Add("column" + i.ToString());
                    DT.Rows[0][i] = col[i].ToString();
                }

                for (int i = 1; i < arr.Count; i++)
                {
                    DT.Rows.Add();
                    Newtonsoft.Json.Linq.JArray row = Newtonsoft.Json.Linq.JArray.Parse(arr[i].ToString());
                    int count = (row.Count < colSize) ? row.Count : colSize;
                    for (int j = 0; j < count; j++)
                    {
                        DT.Rows[i][j] = row[j].ToString();
                    }
                }
            }
            else
            {
                // 1Array
                DT.Rows.Add();
                for (int i = 0; i < arr.Count; i++)
                {
                    DT.Columns.Add("column" + i.ToString());
                    DT.Rows[0][i] = arr[i].ToString();
                }
            }

            return DT;
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Convert JSON " + this.v_InputValue + " To DataTable " + this.v_applyToVariableName + "]";
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
        //        this.validationResult += "Variable to recieve the DataTable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}