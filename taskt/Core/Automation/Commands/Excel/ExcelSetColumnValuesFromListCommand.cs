using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.Description("This command set Column values as List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set Column values as List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ExcelSetColumnValuesFromListCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyValidationRule("Instance Name", Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.Empty)]
        [Attributes.PropertyAttributes.PropertyFirstValue("%kwd_default_excel_instance%")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Specify Column Type")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Range** or **RC**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Range")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Range")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("RC")]
        [Attributes.PropertyAttributes.PropertyValueSensitive(false)]
        public string v_ColumnType { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Column Location or Index")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyValidationRule("Column", Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.Empty | Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.LessThanZero | Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        public string v_ColumnIndex { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Start Row Index")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyValidationRule("Start Row", Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_RowStart { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the End Row Index")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyValidationRule("End Row", Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_RowEnd { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the List Variable Name to set")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        [Attributes.PropertyAttributes.PropertyValidationRule("List", Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ListVariable { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the Value type to get")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Cell** or **Formula** or **Format** or **Color** or **Comment**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Cell")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Formula")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Format")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Font Color")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Back Color")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Cell")]
        [Attributes.PropertyAttributes.PropertyValueSensitive(false)]
        public string v_ValueType { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify If List Items not enough")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Ignore** or **Error**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Ignore")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ignore")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error")]
        [Attributes.PropertyAttributes.PropertyValueSensitive(false)]
        public string v_IfListNotEnough { get; set; }

        public ExcelSetColumnValuesFromListCommand()
        {
            this.CommandName = "ExcelSetColumnValuesAsListCommand";
            this.SelectionName = "Set Column Values From List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var excelInstance = ExcelControls.getExcelInstance(engine, v_InstanceName.ConvertToUserVariable(engine));
            var excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet;

            int columnIndex = 0;
            switch (v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine))
            {
                case "range":
                    string colName = v_ColumnIndex.ConvertToUserVariable(engine);
                    columnIndex = ((Microsoft.Office.Interop.Excel.Range)excelSheet.Columns[colName]).Column;
                    break;
                case "rc":
                    columnIndex = int.Parse(v_ColumnIndex.ConvertToUserVariable(engine));
                    break;
            }

            if (columnIndex < 1)
            {
                throw new Exception("Column index is less than 1");
            }

            int rowStart = int.Parse(v_RowStart.ConvertToUserVariable(engine));
            int rowEnd = int.Parse(v_RowEnd.ConvertToUserVariable(engine));

            if (rowStart > rowEnd)
            {
                int t = rowStart;
                rowStart = rowEnd;
                rowEnd = t;
            }
            int range = rowEnd - rowStart + 1;

            List<string> myList = v_ListVariable.GetListVariable(engine);

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

            string valueType = v_ValueType.GetUISelectionValue("v_ValueType", this, engine);
            Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = null;
            switch (valueType)
            {
                case "cell":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Value = value;
                    };
                    break;
                case "formula":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Formula = value;
                    };
                    break;
                case "format":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).NumberFormatLocal = value;
                    };
                    break;
                case "fore color":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Font.Color = long.Parse(value);
                    };
                    break;
                case "back color":
                    setFunc = (value, sheet, column, row) =>
                    {
                        ((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Interior.Color = long.Parse(value);
                    };
                    break;
            }

            for (int i = 0; i < max; i++)
            {
                setFunc(myList[i], excelSheet, columnIndex, rowStart + i);
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set " + v_ValueType + " Values From '" + v_RowStart + "' to '" + v_RowEnd + "' Column '" + v_ColumnIndex + "' as List '" + v_ListVariable + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}