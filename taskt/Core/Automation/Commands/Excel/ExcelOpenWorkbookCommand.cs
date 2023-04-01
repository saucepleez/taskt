using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("File/Book")]
    [Attributes.ClassAttributes.CommandSettings("Open Workbook")]
    [Attributes.ClassAttributes.Description("This command opens an Excel Workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to open an existing Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelOpenWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "xlsx,xlsm,xls,xlm,csv,ods")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Open Password")]
        [InputSpecification("Excel Password", true)]
        //[SampleUsage("**myPassword** or **{{{vPassword}}}**")]
        [PropertyDetailSampleUsage("**myPassword**", PropertyDetailSampleUsage.ValueType.Value, "Password")]
        [PropertyDetailSampleUsage("**{{{vPassword}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Password")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(false, "")]
        public string v_Password { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Worksheet Exists")]
        [InputSpecification("", true)]
        //[SampleUsage("**Error** or **Ignore** or **Open**")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Nothing to do")]
        [PropertyDetailSampleUsage("**Open**", "Open the specified file")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Open")]
        [PropertyIsOptional(true, "Error")]
        public string v_IfWorksheetExists { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitForFile { get; set; }

        public ExcelOpenWorkbookCommand()
        {
            //this.CommandName = "ExcelOpenWorkbookCommand";
            //this.SelectionName = "Open Workbook";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            var vFilePath = FilePathControls.WaitForFile(this, nameof(v_FilePath), nameof(v_WaitForFile), engine);

            var pass = v_Password.ConvertToUserVariable(sender);

            int worksheets;
            try
            {
                worksheets = excelInstance.Worksheets.Count;
            }
            catch
            {
                worksheets = 0;
            }

            Action openFileProcess = () =>
            {
                if (String.IsNullOrEmpty(pass))
                {
                    excelInstance.Workbooks.Open(vFilePath);
                }
                else
                {
                    excelInstance.Workbooks.Open(vFilePath, Password: pass);
                }
            };

            if (worksheets == 0) 
            {
                openFileProcess();
            }
            else
            {
                switch(this.GetUISelectionValue(nameof(v_IfWorksheetExists), "If Worksheet Exists", engine))
                {
                    case "error":
                        throw new Exception("Excel Instance '" + v_InstanceName + "' has Worksheets.");
                        
                    case "ignore":
                        // nothing
                        break;
                    case "open":
                        openFileProcess();
                        break;
                }
            }
        }
    }
}