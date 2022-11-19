using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.Description("This command sets the value of a cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetCellRCCommand : ScriptCommand
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
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        [PropertyFirstValue("%kwd_default_excel_instance%")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter text to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the text value that will be set.")]
        [SampleUsage("**Hello World** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Text")]
        public string v_TextToSet { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Cell Row")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the actual location of the cell row.")]
        [SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Row", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Row")]
        public string v_ExcelCellRow { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Cell Column")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the actual location of the cell column.")]
        [SampleUsage("**1** or **2** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Column")]
        public string v_ExcelCellColumn { get; set; }

        [XmlAttribute]
        [PropertyDescription("Value type")]
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
        [PropertyDisplayText(true, "Value Type")]
        public string v_ValueType { get; set; }

        public ExcelSetCellRCCommand()
        {
            this.CommandName = "ExcelSetCellRCCommand";
            this.SelectionName = "Set Cell RC";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_InstanceName = "";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            //var rg = ((v_ExcelCellRow, "v_ExcelCellRow"), (v_ExcelCellColumn, "v_ExcelCellColumn")).ConvertToExcelRange(engine, excelInstance, excelSheet, this);
            var rg = this.GetExcelRange(nameof(v_ExcelCellRow), nameof(v_ExcelCellColumn), engine, excelInstance, excelSheet);

            var targetText = v_TextToSet.ConvertToUserVariable(sender);

            string valueType = this.GetUISelectionValue(nameof(v_ValueType), "Value Type", engine);

            var setFunc = ExcelControls.SetCellValueFunctionFromRange(valueType);

            setFunc(targetText, excelSheet, rg);
        }
    }
}