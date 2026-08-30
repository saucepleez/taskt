using System;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Shape")]
    [Attributes.ClassAttributes.CommandSettings("Remove Shape By Name")]
    [Attributes.ClassAttributes.Description("This command allows you to remove Shape by Name")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to remove Shape by Name")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelRemoveShapeByNameCommand : AExcelShapeActionCommands
    {
        public ExcelRemoveShapeByNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelShapeAction(engine, new Action<Microsoft.Office.Interop.Excel.Shape>(shape =>
            {
                shape.Delete();
            }));
        }
    }
}