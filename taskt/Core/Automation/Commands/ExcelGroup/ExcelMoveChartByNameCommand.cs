using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Chart")]
    [Attributes.ClassAttributes.CommandSettings("Move Chart By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to move Chart by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move Chart by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelMoveChartByNameCommand : AExcelChartActionCommands, IPositionProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("X Position")]
        [PropertyIsOptional(true, "Current X Position")]
        [PropertyDisplayText(true, "X Position")]
        [PropertyValidationRule("X Postition", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8000)]
        public string v_XPosition { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Y Position")]
        [PropertyIsOptional(true, "Current Y Position")]
        [PropertyDisplayText(true, "Y Position")]
        [PropertyValidationRule("Y Postition", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8001)]
        public string v_YPosition { get; set; }

        public ExcelMoveChartByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelChartAction(engine, new Action<Microsoft.Office.Interop.Excel.ChartObject>(chart =>
            {
                if (!string.IsNullOrEmpty(v_XPosition))
                {
                    var left = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_XPosition), "X Position", engine);
                    if (left >= 0.0)
                    {
                        chart.Left = left;
                    }
                    else
                    {
                        throw new Exception($"Strange X Position. Value: '{v_XPosition}', Expanded Value: '{left}'");
                    }
                }
                if (!string.IsNullOrEmpty(v_YPosition))
                {
                    var top = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_YPosition), "Y Position", engine);
                    if (top >= 0.0)
                    {
                        chart.Top = top;
                    }
                    else
                    {
                        throw new Exception($"Strange Y Position. Value: '{v_YPosition}', Expanded Value: '{top}'");
                    }
                }
            }));
        }
    }
}