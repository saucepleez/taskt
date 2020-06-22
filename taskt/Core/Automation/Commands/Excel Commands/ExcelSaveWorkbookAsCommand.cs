using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command saves an Excel Workbook to a specific file.")]

    public class ExcelSaveWorkbookAsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Workbook Location")]
        [InputSpecification("Enter or Select the path of the folder to save the Workbook to.")]
        [SampleUsage(@"C:\temp || {vFolderPath} || {ProjectPath}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_FolderPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Workbook File Name")]
        [InputSpecification("Enter or Select the name of the Workbook file.")]
        [SampleUsage("myFile.xlsx || {vFilename}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_FileName { get; set; }

        public ExcelSaveWorkbookAsCommand()
        {
            CommandName = "ExcelSaveWorkbookAsCommand";
            SelectionName = "Save Workbook As";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vFolderPath = v_FolderPath.ConvertToUserVariable(engine);
            var vFileName = v_FileName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);
            var excelInstance = (Application)excelObject;

            //overwrite and save
            excelInstance.DisplayAlerts = false;
            excelInstance.ActiveWorkbook.SaveAs(Path.Combine(vFolderPath, vFileName));
            excelInstance.DisplayAlerts = true;
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FolderPath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Save to '{v_FolderPath}\\{v_FileName}' - Instance Name '{v_InstanceName}']";
        }
    }
}