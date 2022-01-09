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
    [Attributes.ClassAttributes.Description("This command get Row values as List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a Row values as List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ExcelGetRowValuesAsListCommand : ScriptCommand
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
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the List Variable Name to store results")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        [Attributes.PropertyAttributes.PropertyValidationRule("List", Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_userVariableName { get; set; }

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

        public ExcelGetRowValuesAsListCommand()
        {
            this.CommandName = "ExcelGetRowValuesAsListCommand";
            this.SelectionName = "Get Row Values As List";
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

            string valueType = v_ValueType.GetUISelectionValue("v_ValueType", this, engine);
            Func<Microsoft.Office.Interop.Excel.Worksheet, int, int, string> getFunc = null;
            switch (valueType)
            {
                case "cell":
                    getFunc = (sheet, column, row) =>
                    {
                        return (string)((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Text;
                    };
                    break;
                case "formula":
                    getFunc = (sheet, column, row) =>
                    {
                        return (string)((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Formula;
                    };
                    break;
                case "format":
                    getFunc = (sheet, column, row) =>
                    {
                        return (string)((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).NumberFormatLocal;
                    };
                    break;
                case "fore color":
                    getFunc = (sheet, column, row) =>
                    {
                        return ((long)((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Font.Color).ToString();
                    };
                    break;
                case "back color":
                    getFunc = (sheet, column, row) =>
                    {
                        return ((long)((Microsoft.Office.Interop.Excel.Range)sheet.Cells[row, column]).Interior.Color).ToString();
                    };
                    break;
            }


            List<string> newList = new List<string>();

            for (int i = columnStartIndex; i <= columnEndIndex; i++)
            {
                newList.Add(getFunc(excelSheet, i, rowIndex));
            }

            newList.StoreInUserVariable(engine, v_userVariableName);
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
            return base.GetDisplayValue() + " [Get " + v_ValueType + " Values From '" + v_ColumnStart + "' to '" + v_ColumnEnd + "' Row '" + v_RowIndex + "' as List '" + v_userVariableName + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}