using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.Description("This command allows you to get the DataTable value")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get the DataTable value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetDataTableValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a existing DataTable.")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Column value type")]
        [InputSpecification("")]
        [SampleUsage("**Index** or **Column Name**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Index")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyIsOptional(true, "Column Name")]
        [PropertyDisplayText(true, "Column Type")]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Column Name or Index")]
        [InputSpecification("")]
        [SampleUsage("**0** or **id** or **{{{vIndex}}}** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Column")]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Row Index")]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **-1** or **{{{vIndex}}}**")]
        [Remarks("**-1** means index of last row.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Current Row")]
        [PropertyDisplayText(true, "Row")]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify the Variable Name To Assign the Value")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_UserVariableName { get; set; }

        public GetDataTableValueCommand()
        {
            this.CommandName = "GetDataTableValueCommand";
            this.SelectionName = "Get DataTable Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            //string columnType = v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine);

            //string columnPosition = v_ColumnIndex.ConvertToUserVariable(engine);


            //int rowIndex = DataTableControls.GetRowIndex(v_DataTableName, v_RowIndex, engine);

            //string v;
            //if (columnType == "column name")
            //{
            //    v = (myDT.Rows[rowIndex][columnPosition] == null) ? "" : myDT.Rows[rowIndex][columnPosition].ToString();
            //}
            //else
            //{
            //    int colIndex = int.Parse(columnPosition);
            //    v = (myDT.Rows[rowIndex][colIndex] == null) ? "" : myDT.Rows[rowIndex][colIndex].ToString();
            //}

            (var myDT, var rowIndex, var columnIndex) = this.GetDataTableVariableAndRowColumnIndeies(nameof(v_DataTableName), nameof(v_RowIndex), nameof(v_ColumnType), nameof(v_ColumnIndex), engine);

            string v = myDT.Rows[rowIndex][columnIndex]?.ToString() ?? "";

            v.StoreInUserVariable(engine, v_UserVariableName);
        }
    }
}