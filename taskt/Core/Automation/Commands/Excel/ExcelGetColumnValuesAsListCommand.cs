using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.Description("This command get Column values as List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Column values as List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ExcelGetColumnValuesAsListCommand : ScriptCommand
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
        public string v_RowEnd { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the List Variable Name to store results")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
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
        public string v_ValueType { get; set; }

        public ExcelGetColumnValuesAsListCommand()
        {
            this.CommandName = "ExcelGetColumnValuesAsListCommand";
            this.SelectionName = "Get Column Values As List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = ExcelControls.getExcelInstance(engine, v_InstanceName.ConvertToUserVariable(engine));
            var excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet;

            int columnIndex = 0;
            switch (v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine))
            {
                case "range":
                    columnIndex = ExcelControls.getColumnIndex(excelSheet, v_ColumnIndex.ConvertToUserVariable(engine)) ;
                    break;
                case "rc":
                    //columnIndex = int.Parse(v_ColumnIndex.ConvertToUserVariable(engine));
                    columnIndex = v_ColumnIndex.ConvertToUserVariableAsInteger("Column Index", engine);
                    break;
            }

            if (columnIndex < 1)
            {
                throw new Exception("Column index is less than 1");
            }

            string valueType = v_ValueType.GetUISelectionValue("v_ValueType", this, engine);

            //int rowStart = int.Parse(v_RowStart.ConvertToUserVariable(engine));
            //int rowEnd = int.Parse(v_RowEnd.ConvertToUserVariable(engine));
            if (String.IsNullOrEmpty(v_RowStart))
            {
                v_RowStart = "1";
            }
            int rowStart = v_RowStart.ConvertToUserVariableAsInteger("Row Start", engine);

            int rowEnd;
            if (String.IsNullOrEmpty(v_RowEnd))
            {
                rowEnd = ExcelControls.getLastRowIndex(excelSheet, columnIndex, rowStart, valueType);
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

            Func<Microsoft.Office.Interop.Excel.Worksheet, int, int, string> getFunc = ExcelControls.getCellValueFunction(valueType);

            List<string> newList = new List<string>();

            for (int i = rowStart; i <= rowEnd; i++)
            {
                newList.Add(getFunc(excelSheet, columnIndex, i));
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
            return base.GetDisplayValue() + " [Get " + v_ValueType + " Values From '" + v_RowStart + "' to '" + v_RowEnd + "' Column '" + v_ColumnIndex + "' as List '" + v_userVariableName + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}