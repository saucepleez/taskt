using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.CommandSettings("Set Cell")]
    [Attributes.ClassAttributes.Description("This command sets the value of a cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelSetCellCommand : AExcelCellActionCommands, IExcelValueSetProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_CellRangeLocation))]
        //[PropertyParameterOrder(6001)]
        //public string v_CellLocation { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Text to Set")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Text to Set", true)]
        //[SampleUsage("**Hello** or **{{{vText}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "Value")]
        //[PropertyParameterOrder(6500)]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueToSet))]
        [PropertyParameterOrder(6500)]
        public string v_TextToSet { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6002)]
        //public string v_ValueType { get; set; }

        public ExcelSetCellCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var rg = this.ExpandValueOrVariableAsExcelSingleCellLocation(engine);
            var setFunc = this.ExpandValueOrVariableAsSetValueAction(engine);
            var targetText = v_TextToSet.ExpandValueOrUserVariable(engine);
            setFunc(rg, targetText);
        }
    }
}