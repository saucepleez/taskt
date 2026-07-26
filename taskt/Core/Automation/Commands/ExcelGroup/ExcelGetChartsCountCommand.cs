using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Chart")]
    [Attributes.ClassAttributes.CommandSettings("Get Chart Count")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Charts count from Current Worksheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Charts count from Current Worksheet.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelGetChartsCountCommand : AExcelChartCommands, IResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        public ExcelGetChartsCountCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelChartsAction(engine, new Action<System.Collections.Generic.List<Microsoft.Office.Interop.Excel.ChartObject>>(charts =>
            {
                charts.Count.StoreInUserVariable(engine, v_Result);
            }));
        }
    }
}