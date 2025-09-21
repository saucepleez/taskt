using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.CommandSettings("Set Cell RC")]
    [Attributes.ClassAttributes.Description("This command sets the value of a cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelSetCellRCCommand : AExcelRCLocationActionCommands, IExcelValueSetProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowLocation))]
        //[PropertyParameterOrder(6001)]
        //public string v_CellRow { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnLocation))]
        //[PropertyParameterOrder(6002)]
        //public string v_CellColumn { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Text to Set")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Text to Set", true)]
        //[SampleUsage("**Hello World** or **{{{vText}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "Text")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueToSet))]
        [PropertyParameterOrder(8000)]
        public string v_TextToSet { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        //[PropertyParameterOrder(6003)]
        //public string v_ValueType { get; set; }

        public ExcelSetCellRCCommand()
        {
            //this.CommandName = "ExcelSetCellRCCommand";
            //this.SelectionName = "Set Cell RC";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //this.v_InstanceName = "";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.RCLocationAction(new Action<Microsoft.Office.Interop.Excel.Worksheet, int, int>((sheet, column, row) =>
            {
                var setFunc = this.ExpandValueOrVaribleAsSetValueAction(engine);
                var targetText = v_TextToSet.ExpandValueOrUserVariable(engine);
                setFunc(targetText, sheet, column, row);
            }), engine);
        }
    }
}