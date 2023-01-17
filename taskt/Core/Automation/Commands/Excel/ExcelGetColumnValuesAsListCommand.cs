using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Column")]
    [Attributes.ClassAttributes.Description("This command get Column values as List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Column values as List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetColumnValuesAsListCommand : ScriptCommand
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
        //[PropertyValidationRule("Instance Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyFirstValue("%kwd_default_excel_instance%")]
        //[PropertyDisplayText(true, "Instance")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify Column Type")]
        //[InputSpecification("")]
        //[SampleUsage("**Range** or **RC**")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "Range")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Range")]
        //[PropertyUISelectionOption("RC")]
        //[PropertySelectionValueSensitive(false)]
        //[PropertyDisplayText(true, "Column Type")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter the Column Location or Index")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        //[PropertyDisplayText(true, "Column")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnNameOrIndex))]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter the Start Row Index")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "1")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "Start Row")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowStart))]
        public string v_RowStart { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter the End Row Index")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "Last Row")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "End Row")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowEnd))]
        public string v_RowEnd { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the List Variable Name to store results")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        //[PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the Value type to get")]
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
        //[PropertySelectionValueSensitive(false)]
        //[PropertyDisplayText(true, "Value Type")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
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

            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            (int columnIndex, int rowStart, int rowEnd, string valueType) =
                ExcelControls.GetRangeIndeiesColumnDirection(
                    nameof(v_ColumnIndex), nameof(v_ColumnType),
                    nameof(v_RowStart), nameof(v_RowEnd), nameof(v_ValueType),
                    engine, excelSheet, this
                );

            Func<Microsoft.Office.Interop.Excel.Worksheet, int, int, string> getFunc = ExcelControls.GetCellValueFunction(valueType);

            List<string> newList = new List<string>();

            for (int i = rowStart; i <= rowEnd; i++)
            {
                newList.Add(getFunc(excelSheet, columnIndex, i));
            }

            newList.StoreInUserVariable(engine, v_userVariableName);
        }
    }
}