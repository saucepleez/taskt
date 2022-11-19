using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert Column")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Column to DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Column to DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDataTableColumnToDataTableCommand : ScriptCommand
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
        [PropertyDescription("Please specify Column type")]
        [InputSpecification("")]
        [SampleUsage("**Column Name** or **Index**")]
        [Remarks("")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Index")]
        [PropertyIsOptional(true, "Column Name")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyDisplayText(true, "Column Type")]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter the Name or Index of the Column")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a valid Column index value")]
        [SampleUsage("**id** or **0** or **{{{vColumn}}}** or **-1**")]
        [Remarks("If **-1** is specified for Column Index, it means the last column.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Column")]
        public string v_DataColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify the Variable Name To Assign The DataTable")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("Converted DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableColumnToDataTableCommand()
        {
            this.CommandName = "ConvertDataTableColumnToDataTableCommand";
            this.SelectionName = "Convert DataTable Column To DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //DataTable srcDT = v_DataTableName.GetDataTableVariable(engine);

            //string colType = v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine);

            //DataTable myDT = new DataTable();
            //switch (colType)
            //{
            //    case "column name":
            //        string colName = DataTableControls.GetColumnName(srcDT, v_DataColumnIndex, engine);
            //        myDT.Columns.Add(colName);
            //        for (int i = 0; i < srcDT.Rows.Count; i++)
            //        {
            //            myDT.Rows.Add();
            //            myDT.Rows[i][0] = (srcDT.Rows[i][colName] != null) ? srcDT.Rows[i][colName] : "";
            //        }
            //        break;

            //    case "index":
            //        int colIndex = DataTableControls.GetColumnIndex(srcDT, v_DataColumnIndex, engine);
            //        myDT.Columns.Add(srcDT.Columns[colIndex].ColumnName);
            //        for (int i = 0; i < srcDT.Rows.Count; i++)
            //        {
            //            myDT.Rows.Add();
            //            myDT.Rows[i][0] = (srcDT.Rows[i][colIndex] != null) ? srcDT.Rows[i][colIndex] : "";
            //        }
            //        break;
            //}

            (var srcDT, var colIndex) = this.GetDataTableVariableAndColumnIndex(nameof(v_DataTableName), nameof(v_ColumnType), nameof(v_DataColumnIndex), engine);
            DataTable myDT = new DataTable();
            myDT.Columns.Add(srcDT.Columns[colIndex].ColumnName);
            for (int i = 0; i < srcDT.Rows.Count; i++)
            {
                myDT.Rows.Add();
                myDT.Rows[i][0] = srcDT.Rows[i][colIndex] ?? "";
            }

            myDT.StoreInUserVariable(engine, v_OutputVariableName);
        }
    }
}