using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Check Excel Instance Exists")]
    [Attributes.ClassAttributes.Description("This command returns existance of Excel instance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check Excel instance.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCheckExcelInstanceExistsCommand : AExcelInstanceCommand
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Excel Instance Exists, Result is **True**")]
        [PropertyParameterOrder(6000)]
        public string v_applyToVariableName { get; set; }

        public ExcelCheckExcelInstanceExistsCommand()
        {
            //this.CommandName = "CheckExcelInstanceExistsCommand";
            //this.SelectionName = "Check Excel Instance Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            try
            {
                var excelInstance = v_InstanceName.ExpandValueOrUserVariableAsExcelInstance(engine);
                true.StoreInUserVariable(engine, v_applyToVariableName);
            }
            catch
            {
                false.StoreInUserVariable(engine, v_applyToVariableName);
            }
        }
    }
}