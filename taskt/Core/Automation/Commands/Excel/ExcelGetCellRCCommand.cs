using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.Description("This command gets text from a specified Excel Cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a value from a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetCellRCCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Instance")]
        //[PropertyFirstValue("%kwd_default_excel_instance%")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter the Cell Row")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter the actual location of the cell row.")]
        //[SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Row", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        //[PropertyDisplayText(true, "Row")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowLocation))]
        public string v_ExcelCellRow { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter the Cell Column")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter the actual location of the cell column.")]
        //[SampleUsage("**1** or **2** or **{{{vColumn}}}**")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        //[PropertyDisplayText(true, "Column")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnLocation))]
        public string v_ExcelCellColumn { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Assign to Variable")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Value type")]
        //[InputSpecification("")]
        //[SampleUsage("**Cell** or **Formula** or **Format** or **Color** or **Comment**")]
        //[Remarks("")]
        //[PropertyUISelectionOption("Cell")]
        //[PropertyUISelectionOption("Formula")]
        //[PropertyUISelectionOption("Format")]
        //[PropertyUISelectionOption("Font Color")]
        //[PropertyUISelectionOption("Back Color")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsOptional(true, "Cell")]
        //[PropertyDisplayText(true, "Type")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        public string v_ValueType { get; set; }

        public ExcelGetCellRCCommand()
        {
            this.CommandName = "ExcelGetCellRCCommand";
            this.SelectionName = "Get Cell RC";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            var rg = this.GetExcelRange(nameof(v_ExcelCellRow), nameof(v_ExcelCellColumn), engine, excelInstance, excelSheet);

            var valueType = this.GetUISelectionValue(nameof(v_ValueType), engine);

            var func = ExcelControls.GetCellValueFunctionFromRange(valueType);

            func(rg).StoreInUserVariable(sender, v_userVariableName);
        }
    }
}