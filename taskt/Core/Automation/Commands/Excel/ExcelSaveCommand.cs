using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("File/Book")]
    [Attributes.ClassAttributes.CommandSettings("Save Workbook")]
    [Attributes.ClassAttributes.Description("This command allows you to save an Excel workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to save changes to a workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSaveCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        public ExcelSaveCommand()
        {
            //this.CommandName = "ExcelSaveCommand";
            //this.SelectionName = "Save Workbook";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            if (excelInstance.ActiveWorkbook != null)
            {
                if (System.IO.File.Exists(excelInstance.ActiveWorkbook.FullName))
                {
                    //save
                    excelInstance.ActiveWorkbook.Save();
                }
                else
                {
                    throw new Exception("Excel Instance '" + v_InstanceName + "' Workbook does not saved Excel File.");
                }
            }
            else
            {
                throw new Exception("Excel Instance '" + v_InstanceName + "' has no Workbook.");
            }
        }
    }
}