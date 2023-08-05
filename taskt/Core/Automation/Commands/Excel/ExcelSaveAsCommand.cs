using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("File/Book")]
    [Attributes.ClassAttributes.CommandSettings("Save Workbook As")]
    [Attributes.ClassAttributes.Description("This command allows you to save an Excel workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to save a workbook to a file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSaveAsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_FilePath))]
        [PropertyDescription("Excel File Path to Save")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "xlsx")]
        public string v_FileName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Excel File Exists")]
        [InputSpecification("", true)]
        //[SampleUsage("**Error** or **Overwrite** or **Ignore**")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [PropertyDetailSampleUsage("**Overwrite**", "Overwrite file")]
        [PropertyDetailSampleUsage("**Ignore**", "Don't save the file")]
        [Remarks("")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Overwrite")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "Error")]
        public string v_IfExcelFileExists { get; set; }

        public ExcelSaveAsCommand()
        {
            //this.CommandName = "ExcelSaveAsCommand";
            //this.SelectionName = "Save Workbook As";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            //string fileName;
            //if (FilePathControls.ContainsFileCounter(v_FileName, engine))
            //{
            //    fileName = FilePathControls.FormatFilePath_ContainsFileCounter(v_FileName, engine, "xlsx");
            //}
            //else
            //{
            //    fileName = FilePathControls.FormatFilePath_NoFileCounter(v_FileName, engine, "xlsx");
            //}
            string fileName = this.ConvertToUserVariableAsFilePath(nameof(v_FileName), engine);

			// TODO: support xlsm
            Action saveAsProcess = () =>
            {
                //overwrite and save
                excelInstance.DisplayAlerts = false;
                excelInstance.ActiveWorkbook.SaveAs(fileName);
                excelInstance.DisplayAlerts = true;
            };

            if (excelInstance.ActiveWorkbook != null)
            {
                if (!System.IO.File.Exists(fileName))
                {
                    saveAsProcess();
                }
                else
                {
                    switch(this.GetUISelectionValue(nameof(v_IfExcelFileExists), "If Excel File Exists", engine))
                    {
                        case "error":
                            throw new Exception("Excel file '" + v_FileName + "' is already exists.");
                            
                        case "overwrite":
                            saveAsProcess();
                            break;
                        case "ignore":
                            // nothing
                            break;
                    }
                }
            }
            else
            {
                throw new Exception("Excel Instance '" + v_InstanceName + "' has no Workbook.");
            }
        }
    }
}