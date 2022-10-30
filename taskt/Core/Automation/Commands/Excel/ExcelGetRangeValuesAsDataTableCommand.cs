using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Range")]
    [Attributes.ClassAttributes.Description("This command get Range values as DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Range values as DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetRangeValuesAsDataTableCommand : ScriptCommand
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
        [PropertyDescription("Please Specify Column Type")]
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
        [PropertyDescription("Please Enter the Start Column Location")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Start Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
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
        [PropertyDescription("Please Enter the Start Row Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Start Row", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyDisplayText(true, "Start Row")]
        public string v_RowStart { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the End Row Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Last Row")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "End Row")]
        public string v_RowEnd { get; set; }

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
        [PropertyValueSensitive(false)]
        [PropertyDisplayText(true, "Value Type")]
        public string v_ValueType { get; set; }

        public ExcelGetRangeValuesAsDataTableCommand()
        {
            this.CommandName = "ExcelGetRangeValuesAsDataTableCommand";
            this.SelectionName = "Get Range Values As DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            //string valueType = v_ValueType.GetUISelectionValue("v_ValueType", this, engine);
            string valueType = this.GetUISelectionValue(nameof(v_ValueType), "Value Type", engine);

            //int rowStartIndex = v_RowStart.ConvertToUserVariableAsInteger("Start Row", engine);
            int rowStartIndex = this.ConvertToUserVariableAsInteger(nameof(v_RowStart), "Start Row", engine);

            int columnStartIndex = 0;
            int columnEndIndex = 0;
            //switch(v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine))
            switch(this.GetUISelectionValue(nameof(v_ColumnType), "Column Type", engine))
            {
                case "range":
                    columnStartIndex = ExcelControls.GetColumnIndex(excelSheet, v_ColumnStart.ConvertToUserVariable(engine));
                    if (String.IsNullOrEmpty(v_ColumnEnd))
                    {
                        columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowStartIndex, columnStartIndex, valueType);
                    }
                    else
                    {
                        columnEndIndex = ExcelControls.GetColumnIndex(excelSheet, v_ColumnEnd.ConvertToUserVariable(engine));
                    }
                    break;

                case "rc":
                    //columnStartIndex = v_ColumnStart.ConvertToUserVariableAsInteger("Column Start", engine);
                    columnStartIndex = this.ConvertToUserVariableAsInteger(nameof(v_ColumnStart), "Column Start", engine);
                    if (String.IsNullOrEmpty(v_ColumnEnd))
                    {
                        columnEndIndex = ExcelControls.GetLastColumnIndex(excelSheet, rowStartIndex, columnStartIndex, valueType);
                    }
                    else
                    {
                        //columnEndIndex = v_ColumnEnd.ConvertToUserVariableAsInteger("Column End", engine);
                        columnEndIndex = this.ConvertToUserVariableAsInteger(nameof(v_ColumnEnd), "Column End", engine);
                    }
                    break;
            }

            if (columnStartIndex > columnEndIndex)
            {
                int t = columnStartIndex;
                columnStartIndex = columnEndIndex;
                columnEndIndex = t;
            }

            int rowEndIndex;
            if (String.IsNullOrEmpty(v_RowEnd))
            {
                rowEndIndex = ExcelControls.GetLastRowIndex(excelSheet, columnStartIndex, rowStartIndex, valueType);
            }
            else
            {
                //rowEndIndex = v_RowEnd.ConvertToUserVariableAsInteger("Row End", engine);
                rowEndIndex = this.ConvertToUserVariableAsInteger(nameof(v_RowEnd), "Row End", engine);
            }

            if (rowStartIndex > rowEndIndex)
            {
                int t = rowStartIndex;
                rowStartIndex = rowEndIndex;
                rowEndIndex = t;
            }

            ExcelControls.CheckCorrectRCRange(rowStartIndex, columnStartIndex, rowEndIndex, columnEndIndex, excelInstance);

            Func<Microsoft.Office.Interop.Excel.Worksheet, int, int, string> getFunc = ExcelControls.GetCellValueFunction(valueType);

            int rowRange = rowEndIndex - rowStartIndex + 1;
            int colRange = columnEndIndex - columnStartIndex + 1;

            DataTable newDT = new DataTable();
            // set columns
            for (int i = 0; i < colRange; i++) 
            {
                newDT.Columns.Add(ExcelControls.GetColumnName(excelSheet, columnStartIndex + i));
            }

            for (int i = 0; i < rowRange; i++)
            {
                newDT.Rows.Add();
                for (int j = 0; j < colRange; j++)
                {
                    newDT.Rows[i][j] = getFunc(excelSheet, columnStartIndex + j, rowStartIndex + i);
                }
            }

            newDT.StoreInUserVariable(engine, v_userVariableName);
        }
    }
}