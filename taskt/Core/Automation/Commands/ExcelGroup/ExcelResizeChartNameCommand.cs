using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Chart")]
    [Attributes.ClassAttributes.CommandSettings("Resize Chart By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to resize Chart by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to resize Chart by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelResizeChartByNameCommand : AExcelChartActionCommands, ISizeProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Chart Width")]
        [PropertyIsOptional(true, "Current Width")]
        [PropertyDisplayText(false, "Width")]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8000)]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Chart Height")]
        [PropertyIsOptional(true, "Current Height")]
        [PropertyDisplayText(false, "Height")]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8001)]
        public string v_Height { get; set; }

        public ExcelResizeChartByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelChartAction(engine, new Action<Microsoft.Office.Interop.Excel.ChartObject>(chart =>
            {
                if (!string.IsNullOrEmpty(v_Width))
                {
                    var width = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Width), "Width", engine);
                    if (width > 0.0)
                    {
                        chart.Width = width;
                    }
                    else
                    {
                        throw new Exception($"Strange Width Value. Value: '{v_Width}', Expanded Value: '{width}'");
                    }
                }
                if (!string.IsNullOrEmpty(v_Height))
                {
                    var height = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Height), "Height", engine);
                    if (height > 0.0)
                    {
                        chart.Height = height;
                    }
                    else
                    {
                        throw new Exception($"Strange Height Value. Value: '{v_Height}', Expanded Value: '{height}'");
                    }
                }
            }));
        }
    }
}