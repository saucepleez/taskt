using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.Description("This command set Row values from DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a Row values from DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetRowValuesFromDataTableCommand : ScriptCommand
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
        [PropertyDescription("Please Enter the Excel Row Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Excel Row Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyDisplayText(true, "Excel Row")]
        public string v_ExcelRowIndex { get; set; }

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
        [PropertyIsOptional(true, "End of DataTable Column")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "End Column")]
        public string v_ColumnEnd { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the DataTable Variable Name to set")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_DataTableVariable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the DataTable Row Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("DataTable Row Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "DataTable Row")]
        public string v_DataTableRowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Value type to set")]
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

        public ExcelSetRowValuesFromDataTableCommand()
        {
            this.CommandName = "ExcelSetRowValuesFromDataTableCommand";
            this.SelectionName = "Set Row Values From DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var excelInstance = ExcelControls.getExcelInstance(engine, v_InstanceName.ConvertToUserVariable(engine));
            var excelInstance = v_InstanceName.GetExcelInstance(engine);
            var excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet;

            //int excelRowIndex = int.Parse(v_ExcelRowIndex.ConvertToUserVariable(engine));
            //int excelRowIndex = v_ExcelRowIndex.ConvertToUserVariableAsInteger("Excel Row Index", engine);
            //if (excelRowIndex < 1)
            //{
            //    throw new Exception("Excel Row index is less than 1");
            //}
            int excelRowIndex = v_ExcelRowIndex.ConvertToUserVariableAsInteger("v_ExcelRowIndex", "Excel Row", engine, this);

            DataTable myDT = v_DataTableVariable.GetDataTableVariable(engine);

            int columnStartIndex = 0;
            int columnEndIndex = 0;
            switch (v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine))
            {
                case "range":
                    if (String.IsNullOrEmpty(v_ColumnStart))
                    {
                        v_ColumnStart = "A";
                    }
                    columnStartIndex = ExcelControls.getColumnIndex(excelSheet, v_ColumnStart.ConvertToUserVariable(engine));

                    if (String.IsNullOrEmpty(v_ColumnEnd))
                    {
                        columnEndIndex = columnStartIndex + myDT.Columns.Count - 1;
                    }
                    else
                    {
                        columnEndIndex = ExcelControls.getColumnIndex(excelSheet, v_ColumnEnd.ConvertToUserVariable(engine));
                    }
                    break;
                case "rc":
                    if (String.IsNullOrEmpty(v_ColumnStart))
                    {
                        v_ColumnStart = "1";
                    }
                    columnStartIndex = v_ColumnStart.ConvertToUserVariableAsInteger("Column Start", engine);
                    if (String.IsNullOrEmpty(v_ColumnEnd))
                    {
                        columnEndIndex = columnStartIndex + myDT.Columns.Count - 1;
                    }
                    else
                    {
                        columnEndIndex = v_ColumnEnd.ConvertToUserVariableAsInteger("End Column", engine);
                    }
                    //if ((columnStartIndex < 0) || (columnEndIndex < 0))
                    //{
                    //    throw new Exception("Column is less than 0");
                    //}
                    break;
            }
            if (columnStartIndex > columnEndIndex)
            {
                int t = columnStartIndex;
                columnStartIndex = columnEndIndex;
                columnEndIndex = t;
            }

            //if (!ExcelControls.CheckCorrectRC(excelRowIndex, columnStartIndex, excelInstance))
            //{
            //    throw new Exception("Invalid Start Location. Row: " + excelRowIndex + ", Column: " + excelRowIndex);
            //}
            //if (!ExcelControls.CheckCorrectRC(excelRowIndex, columnEndIndex, excelInstance))
            //{
            //    throw new Exception("Invalid End Location. Row: " + excelRowIndex + ", Column: " + columnEndIndex);
            //}
            ExcelControls.CheckCorrectRCRange(excelRowIndex, columnStartIndex, excelRowIndex, columnEndIndex, excelInstance);

            int dtRowIndex = int.Parse(v_DataTableRowIndex.ConvertToUserVariable(engine));
            if (dtRowIndex < 0)
            {
                throw new Exception("DataTable Row index is less than 0");
            }
            else if  (dtRowIndex >= myDT.Rows.Count)
            {
                throw new Exception("DataTable Row " + v_DataTableRowIndex + " is not exists");
            }

            string ifDataTableNotEnough = v_IfDataTableNotEnough.GetUISelectionValue("v_IfDataTableNotEnough", this, engine);
            int range = columnEndIndex - columnStartIndex + 1;
            if (ifDataTableNotEnough == "error")
            {
                if (range > myDT.Columns.Count)
                {
                    throw new Exception("DataTable Items not enough");
                }
            }

            int max = range;
            if (range > myDT.Columns.Count)
            {
                max = myDT.Columns.Count;
            }

            Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.setCellValueFunction(v_ValueType.GetUISelectionValue("v_ValueType", this, engine));

            for (int i = 0; i < max; i++)
            {
                string setValue = (myDT.Rows[dtRowIndex][i] == null) ? "" : myDT.Rows[dtRowIndex][i].ToString();
                setFunc(setValue, excelSheet, columnStartIndex + i, excelRowIndex);
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctls);

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Set " + v_ValueType + " Values From '" + v_ColumnStart + "' to '" + v_ColumnEnd + "' Row '" + v_ExcelRowIndex + "' from DataTable '" + v_DataTableVariable + "', Instance Name: '" + v_InstanceName + "']";
        //}
    }
}