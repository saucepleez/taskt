using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Shape")]
    [Attributes.ClassAttributes.CommandSettings("Move Shape By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to move Shape by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move Shape by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelMoveShapeByNameCommand : AExcelShapeActionCommands, IPositionProperties
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

        public ExcelMoveShapeByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelShapeAction(engine, new Action<Microsoft.Office.Interop.Excel.Shape>(shape =>
            {
                if (!string.IsNullOrEmpty(v_XPosition))
                {
                    var left = (float)this.ExpandValueOrUserVariableAsDecimal(nameof(v_XPosition), "X Position", engine);
                    if (left >= 0.0)
                    {
                        shape.Left = left;
                    }
                    else
                    {
                        throw new Exception($"Strange X Position. Value: '{v_XPosition}', Expanded Value: '{left}'");
                    }
                }
                if (!string.IsNullOrEmpty(v_YPosition))
                {
                    var top = (float)this.ExpandValueOrUserVariableAsDecimal(nameof(v_YPosition), "Y Position", engine);
                    if (top >= 0.0)
                    {
                        shape.Top = top;
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