using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.ExcelGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Shape")]
    [Attributes.ClassAttributes.CommandSettings("Get Shape Names As List")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Shape Names from Current Worksheet As List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Shape Names from Current Worksheet As List")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelGetShapeNamesAsListCommand : AExcelShapesCommands, IListResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        public ExcelGetShapeNamesAsListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.ExcelShapesAction(engine, new Action<System.Collections.Generic.List<Microsoft.Office.Interop.Excel.Shape>>(shapes =>
            {
                var ret = this.CreateEmptyList();
                foreach (var shape in shapes)
                {
                    ret.Add(shape.Name);
                }
                this.StoreListInUserVariable(ret, engine);
            }));
        }
    }
}