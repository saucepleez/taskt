using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.CommandSettings("Append Cell")]
    [Attributes.ClassAttributes.Description("Append input to last row of sheet into the first cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to the last cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelAppendCellCommand : AExcelColumnSpecifiedCommands
    {
        //[XmlAttribute]
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        //[SampleUsage("**myInstance** or **excelInstance**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter text to set")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter the text value that will be set.")]
        //[SampleUsage("Hello World or {vText}")]
        //[Remarks("")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueToSet))]
        [PropertyParameterOrder(6000)]
        public string v_TextToSet { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        //public string v_ColumnType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnLocation))]
        //public string v_ColumnIndex { get; set; }

        //[XmlAttribute]
        //public string v_ValueType { get; set; }

        public ExcelAppendCellCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (var sheet, var columnIndex) = this.ExpandValueOrVariableAsExcelCurrentWorksheetAndColumnIndex(engine);

            var rowIndex = sheet.FirstBlankRowIndex(columnIndex, 1, this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ValueType), "Value Type", engine));

            var targetText = v_TextToSet.ExpandValueOrUserVariable(engine);

            var setFunc = this.ExpandValueOrVaribleAsSetValueAction(engine);

            setFunc(targetText, sheet, columnIndex, rowIndex);
        }
    }
}