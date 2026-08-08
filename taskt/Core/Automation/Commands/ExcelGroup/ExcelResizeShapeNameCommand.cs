using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Shape")]
    [Attributes.ClassAttributes.CommandSettings("Resize Shape By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to resize Shape by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to resize Shape by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelResizeShapeByNameCommand : AExcelShapeActionCommands, ISizeProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Shape Width")]
        [PropertyIsOptional(true, "Current Width")]
        [PropertyDisplayText(true, "Width")]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8000)]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Shape Height")]
        [PropertyIsOptional(true, "Current Height")]
        [PropertyDisplayText(true, "Height")]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8001)]
        public string v_Height { get; set; }

        public ExcelResizeShapeByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelShapeAction(engine, new Action<Microsoft.Office.Interop.Excel.Shape>(shape =>
            {
                if (!string.IsNullOrEmpty(v_Width))
                {
                    var width = (float)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Width), "Width", engine);
                    if (width > 0.0)
                    {
                        shape.Width = width;
                    }
                    else
                    {
                        throw new Exception($"Strange Width Value. Value: '{v_Width}', Expanded Value: '{width}'");
                    }
                }
                if (!string.IsNullOrEmpty(v_Height))
                {
                    var height = (float)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Height), "Height", engine);
                    if (height > 0.0)
                    {
                        shape.Height = height;
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