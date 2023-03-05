using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.CommandSettings("Set Cell")]
    [Attributes.ClassAttributes.Description("This command sets the value of a cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text to Set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Text to Set", true)]
        [SampleUsage("**Hello** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value")]
        public string v_TextToSet { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_CellRangeLocation))]
        public string v_ExcelCellAddress { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        public string v_ValueType { get; set; }

        public ExcelSetCellCommand()
        {
            //this.CommandName = "ExcelSetCellCommand";
            //this.SelectionName = "Set Cell";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            var rg = v_ExcelCellAddress.GetExcelRange(engine, excelInstance, excelSheet, this);

            var targetText = v_TextToSet.ConvertToUserVariable(sender);

            string valueType = this.GetUISelectionValue(nameof(v_ValueType), "Value Type", engine);

            var setFunc = ExcelControls.SetCellValueFunctionFromRange(valueType);
            setFunc(targetText, excelSheet, rg);
        }
    }
}