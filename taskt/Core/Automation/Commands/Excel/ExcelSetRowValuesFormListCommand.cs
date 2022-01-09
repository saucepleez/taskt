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
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.Description("This command set Row values as List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a Row values as List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ExcelSetRowValuesFromListCommand : ScriptCommand
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
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Row Index")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyValidationRule("Row Index", Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.Empty | Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.LessThanZero | Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        public string v_RowIndex { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Start Column Location")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**A** or **B** or **{{{vColumn}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyValidationRule("Start Column", Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ColumnStart { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the End Column Location")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**A** or **B** or **{{{vColumn}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyValidationRule("End Column", Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ColumnEnd { get; set; }
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
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the Value type to set")]
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

        public ExcelSetRowValuesFromListCommand()
        {
            this.CommandName = "ExcelSetRowValuesFromListCommand";
            this.SelectionName = "Set Row Values From List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var excelInstance = ExcelControls.getExcelInstance(engine, v_InstanceName.ConvertToUserVariable(engine));
            var excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet;

            int rowIndex = int.Parse(v_RowIndex.ConvertToUserVariable(engine));
            if (rowIndex < 1)
            {
                throw new Exception("Row index is less than 1");
            }
            string columnStart = v_ColumnStart.ConvertToUserVariable(engine);
            string columnEnd = v_ColumnEnd.ConvertToUserVariable(engine);

            int columnStartIndex = ((Microsoft.Office.Interop.Excel.Range)excelSheet.Columns[columnStart]).Column;
            int columnEndIndex = ((Microsoft.Office.Interop.Excel.Range)excelSheet.Columns[columnEnd]).Column;
            if (columnStartIndex > columnEndIndex)
            {
                int t = columnStartIndex;
                columnStartIndex = columnEndIndex;
                columnEndIndex = t;
            }

            List<string> myList = v_ListVariable.GetListVariable(engine);

            string ifListNotEnough = v_IfListNotEnough.GetUISelectionValue("v_IfListNotEnough", this, engine);
            int range = columnEndIndex - columnStartIndex + 1;
            if (ifListNotEnough == "error")
            {
                if (range > myList.Count)
                {
                    throw new Exception("List Items not enough");
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
                setFunc(myList[i], excelSheet, columnStartIndex + i, rowIndex);
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
            return base.GetDisplayValue() + " [Set " + v_ValueType + " Values From '" + v_ColumnStart + "' to '" + v_ColumnEnd + "' Row '" + v_RowIndex + "' as List '" + v_ListVariable + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}