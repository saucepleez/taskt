using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.Excel;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Worksheet")]
    [Attributes.ClassAttributes.CommandSettings("Check Worksheet Exists")]
    [Attributes.ClassAttributes.Description("This command allows you to check existance sheet")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to switch to a specific worksheet")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCheckWorksheetExistsCommand : AExcelSheetCommand
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        //public string v_SheetName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Sheet Exists, Result is **True**")]
        [PropertyParameterOrder(7000)]
        public string v_applyToVariable { get; set; }

        public ExcelCheckWorksheetExistsCommand()
        {
            //this.CommandName = "ExcelCheckWorksheetExistsCommand";
            //this.SelectionName = "Check Worksheet Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }
        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(_, var sht) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(v_SheetName, engine, true);

            //(sht != null).StoreInUserVariable(engine, v_applyToVariable);
            try
            {
                this.ExpandValueOrVariableAsExcelInstanceAndWorksheet(engine);
                true.StoreInUserVariable(engine, v_applyToVariable);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (msg.StartsWith("Worksheet '") && msg.EndsWith("' does not exists."))
                {
                    false.StoreInUserVariable(engine, v_applyToVariable);
                }
                else
                {
                    throw ex;
                }
            }
        }
    }
}