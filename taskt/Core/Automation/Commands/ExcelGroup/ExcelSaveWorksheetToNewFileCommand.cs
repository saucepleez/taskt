using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Worksheet")]
    [Attributes.ClassAttributes.CommandSettings("Save Worksheet To New File")]
    [Attributes.ClassAttributes.Description("This command allows you to Save Worksheet to New File")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Save Worksheet to New File")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelSaveWorksheetToNewFileCommand : AExcelSheetCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SaveFilePath))]
        [PropertyDescription("Excel File Path to Save")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "xlsx")]
        [PropertyParameterOrder(7000)]
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
        [PropertyParameterOrder(7001)]
        public string v_IfExcelFileExists { get; set; }

        public ExcelSaveWorksheetToNewFileCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (var excelInstance, var targetSheet) = this.ExpandValueOrVariableAsExcelInstanceAndWorksheet(engine);

            var currentWorkbooks = excelInstance.Workbooks.Count;
            var currentWB = excelInstance.ActiveWorkbook;
            excelInstance.Workbooks.Add();

            var newWorkbook = excelInstance.Workbooks[currentWorkbooks + 1];

            targetSheet.Copy(After: newWorkbook.Worksheets[1]);

            // remove Sheet1
            ((Microsoft.Office.Interop.Excel.Worksheet)newWorkbook.Worksheets[1]).Delete();
            // rename
            ((Microsoft.Office.Interop.Excel.Worksheet)newWorkbook.Worksheets[1]).Name = targetSheet.Name;

            var saveAs = new ExcelSaveAsCommand()
            {
                v_InstanceName = this.v_InstanceName,
                v_FileName = this.v_FileName,
                v_IfExcelFileExists = this.v_IfExcelFileExists,
            };
            saveAs.RunCommand(engine);
            newWorkbook.Close();

            currentWB.Activate();
        }
    }
}