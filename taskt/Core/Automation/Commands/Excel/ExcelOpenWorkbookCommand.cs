using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("File/Book")]
    [Attributes.ClassAttributes.Description("This command opens an Excel Workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to open an existing Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelOpenWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        [PropertyFirstValue("%kwd_default_excel_instance%")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the workbook file path")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the applicable file that should be opened by Excel.")]
        [SampleUsage("**C:\\temp\\myfile.xlsx** or **{{{vFilePath}}}**")]
        [Remarks("If file does not contain extension, supplement extensions supported by Excel.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("File", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate open password")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter or Select the path to the applicable file that should be opened by Excel.")]
        [SampleUsage("**myPassword** or **{{{vPassword}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        public string v_Password { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify If Worksheet Exists")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**Error** or **Ignore** or **Open**")]
        [Remarks("")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Open")]
        [PropertyIsOptional(true, "Error")]
        public string v_IfWorksheetExists { get; set; }

        public ExcelOpenWorkbookCommand()
        {
            this.CommandName = "ExcelOpenWorkbookCommand";
            this.SelectionName = "Open Workbook";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            string vFilePath = FilePathControls.formatFilePath_NoFileCounter(v_FilePath, engine, new List<string>() { "xlsx", "xlsm", "xls", "csv", "ods" }, true);

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
                        break;
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