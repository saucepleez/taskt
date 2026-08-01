using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Chart")]
    [Attributes.ClassAttributes.CommandSettings("Check Chart Exists By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to Check Chart Exists by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Check Chart Exists by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelCheckChartExistsByNameCommand : AExcelDoSomethingToChartByChartName, IResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(8000)]
        public string v_Result { get; set; }

        public ExcelCheckChartExistsByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelChartAction(engine,
                new Action<Microsoft.Office.Interop.Excel.ChartObject>(chart =>
                {
                    true.StoreInUserVariable(engine, v_Result);
                }),
                new Action<Exception>(ex =>
                {
                    false.StoreInUserVariable(engine, v_Result);
                })
            );
        }
    }
}