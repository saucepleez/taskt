using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable to JSON")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable to JSON.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDataTableToJSONCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a existing DataTable to fet rows from.")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify the Variable Name To Assign The JSON")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.JSON, true)]
        [PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableToJSONCommand()
        {
            this.CommandName = "ConvertDataTableToJSONCommand";
            this.SelectionName = "Convert DataTable To JSON";
            this.CommandEnabled = true;
            this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataTable srcDT = v_DataTableName.GetDataTableVariable(engine);

            List<Dictionary<string, string>> jsonList = new List<Dictionary<string, string>>();
            for (int j = 0; j < srcDT.Rows.Count; j++)
            {
                Dictionary<string, string> tDic = new Dictionary<string, string>();
                for (int i = 0; i < srcDT.Columns.Count; i++)
                {
                    tDic.Add(srcDT.Columns[i].ColumnName, srcDT.Rows[j][i]?.ToString() ?? "");
                }

                jsonList.Add(tDic);
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonList);
            json.StoreInUserVariable(engine, v_OutputVariableName);
        }
    }
}