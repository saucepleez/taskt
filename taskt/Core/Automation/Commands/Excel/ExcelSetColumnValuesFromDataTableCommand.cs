using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.Description("This command set Column values from DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set Column values from DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetColumnValuesFromDataTableCommand : ScriptCommand
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
        [PropertyDescription("Please Specify Excel Column Type")]
        [InputSpecification("")]
        [SampleUsage("**Range** or **RC**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Range")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Range")]
        [PropertyUISelectionOption("RC")]
        [PropertyValueSensitive(false)]
        [PropertyDisplayText(true, "Column Type")]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Excel Column Location or Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyDisplayText(true, "Column")]
        public string v_ExcelColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Start Row Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "1")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Start Row")]
        public string v_RowStart { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the End Row Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "End of DataTable Row")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "End Row")]
        public string v_RowEnd { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the DataTable Variable Name to set")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_DataTableVariable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the DataTable Column Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Column")]
        public string v_DataTableColumnIndex { get; set; }

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
        [PropertyValueSensitive(false)]
        [PropertyDisplayText(true, "Value Type")]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify If DataTable Items not enough")]
        [InputSpecification("")]
        [SampleUsage("**Ignore** or **Error**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyValueSensitive(false)]
        public string v_IfDataTableNotEnough { get; set; }

        public ExcelSetColumnValuesFromDataTableCommand()
        {
            this.CommandName = "ExcelSetColumnValuesFromDataTableCommand";
            this.SelectionName = "Set Column Values From DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (_, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            DataTable myDT = v_DataTableVariable.GetDataTableVariable(engine);

            //int excelColumnIndex = 0;
            //switch (v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine))
            //{
            //    case "range":
            //        excelColumnIndex = ExcelControls.GetColumnIndex(excelSheet, v_ExcelColumnIndex.ConvertToUserVariable(engine));
            //        break;
            //    case "rc":
            //        excelColumnIndex = v_ExcelColumnIndex.ConvertToUserVariableAsInteger("Excel Column", engine);
            //        break;
            //}

            //int rowStart = v_RowStart.ConvertToUserVariableAsInteger("v_RowStart", "Start Row", engine, this);

            //int rowEnd;
            //if (String.IsNullOrEmpty(v_RowEnd))
            //{
            //    rowEnd = rowStart + myDT.Rows.Count - 1;
            //}
            //else
            //{
            //    rowEnd = v_RowEnd.ConvertToUserVariableAsInteger("Row End", engine);
            //}

            //if (rowStart > rowEnd)
            //{
            //    int t = rowStart;
            //    rowStart = rowEnd;
            //    rowEnd = t;
            //}
            //int range = rowEnd - rowStart + 1;

            //ExcelControls.CheckCorrectRCRange(rowStart, excelColumnIndex, rowEnd, excelColumnIndex, excelInstance);

            (int excelColumnIndex, int rowStart, int rowEnd, string valueType) =
                ExcelControls.GetRangeIndeiesColumnDirection(
                    nameof(v_ExcelColumnIndex), nameof(v_ColumnType),
                    nameof(v_RowStart), nameof(v_RowEnd),
                    nameof(v_ValueType), engine, excelSheet, this,
                    myDT
                );

            int range = rowEnd - rowStart + 1;

            //int dtColumnIndex = int.Parse(v_DataTableColumnIndex.ConvertToUserVariable(engine));
            int dtColumnIndex = this.ConvertToUserVariableAsInteger(nameof(v_DataTableColumnIndex), "DataTable Column Index", engine);
            if ((dtColumnIndex < 0) || (dtColumnIndex >= myDT.Columns.Count))
            {
                throw new Exception("Column index " + v_DataTableColumnIndex + " is not exists");
            }

            //string ifListNotEnough = v_IfDataTableNotEnough.GetUISelectionValue("v_IfDataTableNotEnough", this, engine);
            string ifDataTableNotEnough = this.GetUISelectionValue(nameof(v_IfDataTableNotEnough), "Id DataTable Not Enough", engine);
            if (ifDataTableNotEnough == "error")
            {
                if (range > myDT.Rows.Count)
                {
                    throw new Exception("DataTable items not enough");
                }
            }

            int max = range;
            if (range > myDT.Rows.Count)
            {
                max = myDT.Rows.Count;
            }

            Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.SetCellValueFunction(v_ValueType.GetUISelectionValue("v_ValueType", this, engine));

            for (int i = 0; i < max; i++)
            {
                //string value = myDT.Rows[i][dtColumnIndex] == null ? "" : myDT.Rows[i][dtColumnIndex].ToString();
                string value = myDT.Rows[i][dtColumnIndex]?.ToString() ?? "";
                setFunc(value, excelSheet, excelColumnIndex, rowStart + i);
            }
        }
    }
}