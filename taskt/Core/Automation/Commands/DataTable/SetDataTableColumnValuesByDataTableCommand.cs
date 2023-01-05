using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.Description("This command allows you to set a column to a DataTable by a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a column to a DataTable by a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetDataTableColumnValuesByDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please indicate the DataTable Variable Name")]
        //[InputSpecification("Enter a existing DataTable to add rows to.")]
        //[SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("DataTable to setted", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "DataTable to setted")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        [PropertyDescription("DataTable Variable Name to be Setted")]
        [PropertyValidationRule("DataTable to be Setted", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to be Setted")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify Column type")]
        //[InputSpecification("")]
        //[SampleUsage("**Column Name** or **Index**")]
        //[Remarks("")]
        //[PropertyUISelectionOption("Column Name")]
        //[PropertyUISelectionOption("Index")]
        //[PropertyIsOptional(true, "Column Name")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyDisplayText(true, "Column Type")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the Column Name to set")]
        //[InputSpecification("")]
        //[SampleUsage("**0** or **newColumn** or **{{{vNewColumn}}}** or **-1**")]
        //[Remarks("If **-1** is specified for Column Index, it means the last column.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Column")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        public string v_SetColumnName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the DataTable to set new Column values")]
        //[InputSpecification("")]
        //[SampleUsage("**vDataTable** or **{{{vDataTable}}}**")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        //[PropertyValidationRule("DataTable to set", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "DataTable to set")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyDescription("DataTable Variable Name to Set")]
        [PropertyValidationRule("DataTable to Set", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to Set")]
        public string v_SetDataTableName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("If the number of rows is less than the DataTable to set")]
        //[InputSpecification("")]
        //[SampleUsage("**Ignore** or **Add Rows** or **Error**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyUISelectionOption("Add Rows")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyIsOptional(true, "Ignore")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_WhenLessRows))]
        [PropertyDescription("When there are Less Rows than DataTable to set")]
        public string v_IfRowNotEnough { set; get; }

        [XmlAttribute]
        //[PropertyDescription("If the number of DataTable items is less than the rows to setted DataTable")]
        //[InputSpecification("")]
        //[SampleUsage("**Ignore** or **Error**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyIsOptional(true, "Ignore")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_WhenGreaterRows))]
        public string v_IfSetDataTableNotEnough { set; get; }

        public SetDataTableColumnValuesByDataTableCommand()
        {
            this.CommandName = "SetDataTableColumnByDataTableCommand";
            this.SelectionName = "Set DataTable Column By DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var myDT, var colIndex) = this.GetDataTableVariableAndColumnIndex(nameof(v_DataTableName), nameof(v_ColumnType), nameof(v_SetColumnName), engine);

            string trgColName = myDT.Columns[colIndex].ColumnName;


            DataTable setDT = v_SetDataTableName.GetDataTableVariable(engine);

            string ifRowNotEnough = this.GetUISelectionValue(nameof(v_IfRowNotEnough), "Row Not Enough", engine);
            // rows check
            if (myDT.Rows.Count < setDT.Rows.Count)
            {
                switch (ifRowNotEnough)
                {
                    case "ignore":
                    case "add rows":
                        break;

                    case "error":
                        throw new Exception("The number of rows is less than the DataTable to set");
                }
            }

            string ifDataTableNotEnough = this.GetUISelectionValue(nameof(v_IfSetDataTableNotEnough), "DataTable Not Enough", engine);
            if ((myDT.Rows.Count > setDT.Rows.Count) && (ifDataTableNotEnough == "error"))
            {
                throw new Exception("The number of DataTable items is less than the rows to settedd");
            }

            int maxRow = (myDT.Rows.Count > setDT.Rows.Count) ? setDT.Rows.Count : myDT.Rows.Count;
            for (int i = 0; i < maxRow; i++)
            {
                myDT.Rows[i][trgColName] = setDT.Rows[i][trgColName];
            }
            if ((myDT.Rows.Count < setDT.Rows.Count) && (ifRowNotEnough == "add rows"))
            {
                for (int i = myDT.Rows.Count; i < setDT.Rows.Count; i++)
                {
                    myDT.Rows.Add();
                    myDT.Rows[i][trgColName] = setDT.Rows[i][trgColName];
                }
            }
        }
    }
}