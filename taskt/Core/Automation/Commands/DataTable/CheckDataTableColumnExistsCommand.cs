using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.Description("This command allows you to check the column name existance")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check the column name existance.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CheckDataTableColumnExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please indicate the DataTable Variable Name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a existing DataTable.")]
        //[SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "DataTable")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Name of Column")]
        [InputSpecification("")]
        //[SampleUsage("**colName** or **{{{vColName}}}**")]
        [PropertyDetailSampleUsage("**name**", PropertyDetailSampleUsage.ValueType.Value, "Column Name to be Checked")]
        [PropertyDetailSampleUsage("**{{{vColName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column Name to be Checked")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Column")]
        public string v_ColumnName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify the Variable Name To Assign the result")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Result")]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        public string v_OutputVariableName { get; set; }

        public CheckDataTableColumnExistsCommand()
        {
            this.CommandName = "CheckDataTableColumnExistsCommand";
            this.SelectionName = "Check DataTable Column Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            string targetColumnName = v_ColumnName.ConvertToUserVariable(engine);

            //bool isExists = false;
            //foreach(DataColumn col in myDT.Columns)
            //{
            //    if (col.ColumnName == targetColumnName)
            //    {
            //        isExists = true;
            //        break;
            //    }
            //}
            //isExists.StoreInUserVariable(engine, v_OutputVariableName);
            myDT.Columns.Contains(targetColumnName).StoreInUserVariable(engine, v_OutputVariableName);
        }
    }
}