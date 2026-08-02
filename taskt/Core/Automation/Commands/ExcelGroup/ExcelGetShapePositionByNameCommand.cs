using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Shape")]
    [Attributes.ClassAttributes.CommandSettings("Get Shape Position By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to get Shape position by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Shape position by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelGetShapePositionByNameCommand : AExcelDoSomethingToShapeByShapeName, IPositionProperties
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

        public ExcelGetShapePositionByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelShapeAction(engine, new Action<Microsoft.Office.Interop.Excel.Shape>(shape =>
            {
                if (!string.IsNullOrEmpty(v_XPosition))
                {
                    ((double)shape.Left).StoreInUserVariable(engine, v_XPosition);
                }
                if (!string.IsNullOrEmpty(v_YPosition))
                {
                    ((double)shape.Top).StoreInUserVariable(engine, v_YPosition);
                }
            }));
        }
    }
}