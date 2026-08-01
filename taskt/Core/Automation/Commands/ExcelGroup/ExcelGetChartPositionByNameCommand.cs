using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Chart")]
    [Attributes.ClassAttributes.CommandSettings("Get Chart Position By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to get Chart position by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Chart position by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelGetChartPositionByNameCommand : AExcelDoSomethingToChartByChartName, IPositionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Chart X Position")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "X Position")]
        [PropertyValidationRule("X Postition", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8000)]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Chart Y Position")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "Y Position")]
        [PropertyValidationRule("Y Postition", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8001)]
        public string v_YPosition { get; set; }

        public ExcelGetChartPositionByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelChartAction(engine, new Action<Microsoft.Office.Interop.Excel.ChartObject>(chart =>
            {
                if (!string.IsNullOrEmpty(v_XPosition))
                {
                    chart.Left.StoreInUserVariable(engine, v_XPosition);
                }
                if (!string.IsNullOrEmpty(v_YPosition))
                {
                    chart.Top.StoreInUserVariable(engine, v_YPosition);
                }
            }));
        }
    }
}