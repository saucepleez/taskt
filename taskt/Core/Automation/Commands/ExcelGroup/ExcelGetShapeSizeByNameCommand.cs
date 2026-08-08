using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Shape")]
    [Attributes.ClassAttributes.CommandSettings("Get Shape Size By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to get Shape size by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Shape size by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelGetShapeSizeByNameCommand : AExcelDoSomethingToShapeByShapeName, ISizeProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Chart Width")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(true, "Width")]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8000)]
        public string v_Width { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Recieve the Chart Height")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(true, "Height")]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(8001)]
        public string v_Height { get; set; }

        public ExcelGetShapeSizeByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelShapeAction(engine, new Action<Microsoft.Office.Interop.Excel.Shape>(shape =>
            {
                if (!string.IsNullOrEmpty(v_Width))
                {
                    ((double)shape.Width).StoreInUserVariable(engine, v_Width);
                }
                if (!string.IsNullOrEmpty(v_Height))
                {
                    ((double)shape.Height).StoreInUserVariable(engine, v_Height);
                }
            }));
        }
    }
}