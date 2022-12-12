using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.Description("This command get Row values as DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a Row values as DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetRowValuesAsDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Instance Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyFirstValue("%kwd_default_excel_instance%")]
        [PropertyDisplayText(true, "Instance")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Row Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Row Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyDisplayText(true, "Row")]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Column Type")]
        [InputSpecification("")]
        [SampleUsage("**Range** or **RC**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Range")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Range")]
        [PropertyUISelectionOption("RC")]
        [PropertySelectionValueSensitive(false)]
        [PropertyDisplayText(true, "Column Type")]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Start Column Location")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "A or 1")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Start Column")]
        public string v_ColumnStart { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the End Column Location")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Last Column")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "End Column")]
        public string v_ColumnEnd { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the DataTable Variable Name to store results")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Value type to get")]
        [InputSpecification("")]
        [SampleUsage("**Cell** or **Formula** or **Format** or **Color** or **Comment**")]
        [Remarks("")]
        [PropertyUISelectionOption("Cell")]
        [PropertyUISelectionOption("Formula")]
        [PropertyUISelectionOption("Format")]
        [PropertyUISelectionOption("Font Color")]
        [PropertyUISelectionOption("Back Color")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Cell")]
        [PropertySelectionValueSensitive(false)]
        [PropertyDisplayText(true, "Value Type")]
        public string v_ValueType { get; set; }

        public ExcelGetRowValuesAsDataTableCommand()
        {
            this.CommandName = "ExcelGetRowValuesAsDataTableCommand";
            this.SelectionName = "Get Row Values As DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            //int rowIndex = v_RowIndex.ConvertToUserVariableAsInteger("v_RowIndex", "Row", engine, this);

            //string valueType = v_ValueType.GetUISelectionValue("v_ValueType", this, engine);

            //int columnStartIndex = 0;
            //int columnEndIndex = 0;
            //switch(v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine))
            //{
            //    case "range":
            //        if (String.IsNullOrEmpty(v_ColumnStart))
            //        {
            //            v_ColumnStart = "A";
            //        }
            //        columnStartIndex = ExcelControls.GetColumnIndex(excelSheet, v_ColumnStart.ConvertToUserVariable(engine));

            //        if (String.IsNullOrEmpty(v_ColumnEnd))
            //        {
            //            columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowIndex, columnStartIndex, valueType);
            //        }
            //        else
            //        {
            //            columnEndIndex = ExcelControls.GetColumnIndex(excelSheet, v_ColumnEnd.ConvertToUserVariable(engine));
            //        }
            //        break;

            //    case "rc":
            //        if (String.IsNullOrEmpty(v_ColumnStart))
            //        {
            //            v_ColumnStart = "1";
            //        }
            //        columnStartIndex = v_ColumnStart.ConvertToUserVariableAsInteger("Column Start", engine);

            //        if (String.IsNullOrEmpty(v_ColumnEnd))
            //        {
            //            columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowIndex, columnStartIndex, valueType);
            //        }
            //        else
            //        {
            //            columnEndIndex = v_ColumnEnd.ConvertToUserVariableAsInteger("Column End", engine);
            //        }

            //        break;
            //}

            //if (columnStartIndex > columnEndIndex)
            //{
            //    int t = columnStartIndex;
            //    columnStartIndex = columnEndIndex;
            //    columnEndIndex = t;
            //}

            //ExcelControls.CheckCorrectRCRange(rowIndex, columnStartIndex, rowIndex, columnEndIndex, excelInstance);

            (int rowIndex, int columnStartIndex, int columnEndIndex, string valueType) =
                ExcelControls.GetRangeIndeiesRowDirection(
                    nameof(v_RowIndex),
                    nameof(v_ColumnType), nameof(v_ColumnStart), nameof(v_ColumnEnd),
                    nameof(v_ValueType), engine, excelSheet, this
                );

            Func<Microsoft.Office.Interop.Excel.Worksheet, int, int, string> getFunc = ExcelControls.GetCellValueFunction(valueType);

            DataTable newDT = new DataTable();
            newDT.Rows.Add();

            int tblCol = 0;
            for (int i = columnStartIndex; i <= columnEndIndex; i++)
            {
                newDT.Columns.Add(ExcelControls.GetColumnName(excelSheet, i));
                newDT.Rows[0][tblCol] = getFunc(excelSheet, i, rowIndex);
                tblCol++;
            }

            newDT.StoreInUserVariable(engine, v_userVariableName);
        }
    }
}