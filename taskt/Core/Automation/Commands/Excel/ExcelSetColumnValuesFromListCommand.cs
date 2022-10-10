using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.Description("This command set Column values from List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set Column values from List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetColumnValuesFromListCommand : ScriptCommand
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
        [PropertyDescription("Please Enter the Column Location or Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyDisplayText(true, "Column")]
        public string v_ColumnIndex { get; set; }

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
        [PropertyIsOptional(true, "End of List")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "End Row")]
        public string v_RowEnd { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the List Variable Name to set")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_ListVariable { get; set; }

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
        [PropertyDescription("Please specify If List Items not enough")]
        [InputSpecification("")]
        [SampleUsage("**Ignore** or **Error**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyValueSensitive(false)]
        public string v_IfListNotEnough { get; set; }

        public ExcelSetColumnValuesFromListCommand()
        {
            this.CommandName = "ExcelSetColumnValuesFromListCommand";
            this.SelectionName = "Set Column Values From List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var excelInstance = ExcelControls.getExcelInstance(engine, v_InstanceName.ConvertToUserVariable(engine));
            var excelInstance = v_InstanceName.GetExcelInstance(engine);
            var excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet;

            int columnIndex = 0;
            switch (v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine))
            {
                case "range":
                    columnIndex = ExcelControls.getColumnIndex(excelSheet, v_ColumnIndex.ConvertToUserVariable(engine));
                    break;
                case "rc":
                    columnIndex = v_ColumnIndex.ConvertToUserVariableAsInteger("Column Index", engine);
                    break;
            }

            //if (columnIndex < 1)
            //{
            //    throw new Exception("Column index is less than 1");
            //}

            List<string> myList = v_ListVariable.GetListVariable(engine);

            //int rowStart = int.Parse(v_RowStart.ConvertToUserVariable(engine));
            //int rowEnd = int.Parse(v_RowEnd.ConvertToUserVariable(engine));
            //if (String.IsNullOrEmpty(v_RowStart))
            //{
            //    v_RowStart = "1";
            //}
            //int rowStart = v_RowStart.ConvertToUserVariableAsInteger("Row Start", engine);
            int rowStart = v_RowStart.ConvertToUserVariableAsInteger("v_rowStart", "Start Row", engine, this);

            int rowEnd;
            if (String.IsNullOrEmpty(v_RowEnd))
            {
                rowEnd = rowStart + myList.Count - 1;
            }
            else
            {
                rowEnd = v_RowEnd.ConvertToUserVariableAsInteger("Row End", engine);
            }

            if (rowStart > rowEnd)
            {
                int t = rowStart;
                rowStart = rowEnd;
                rowEnd = t;
            }
            int range = rowEnd - rowStart + 1;

            //if (!ExcelControls.CheckCorrectRC(rowStart, columnIndex, excelInstance))
            //{
            //    throw new Exception("Invalid Start Location. Row: " + rowStart + ", Column: " + columnIndex);
            //}
            //if (!ExcelControls.CheckCorrectRC(rowEnd, columnIndex, excelInstance))
            //{
            //    throw new Exception("Invalid End Location. Row: " + rowEnd + ", Column: " + columnIndex);
            //}
            ExcelControls.CheckCorrectRCRange(rowStart, columnIndex, rowEnd, columnIndex, excelInstance);

            string ifListNotEnough = v_IfListNotEnough.GetUISelectionValue("v_IfListNotEnough", this, engine);
            if (ifListNotEnough == "error")
            {
                if (range > myList.Count)
                {
                    throw new Exception("List items not enough");
                }
            }

            int max = range;
            if (range > myList.Count)
            {
                max = myList.Count;
            }

            Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.setCellValueFunction(v_ValueType.GetUISelectionValue("v_ValueType", this, engine));

            for (int i = 0; i < max; i++)
            {
                setFunc(myList[i], excelSheet, columnIndex, rowStart + i);
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
        //    return base.GetDisplayValue() + " [Set " + v_ValueType + " Values From '" + v_RowStart + "' to '" + v_RowEnd + "' Column '" + v_ColumnIndex + "' from List '" + v_ListVariable + "', Instance Name: '" + v_InstanceName + "']";
        //}
    }
}