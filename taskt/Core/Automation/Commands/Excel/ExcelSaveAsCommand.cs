using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("File/Book")]
    [Attributes.ClassAttributes.Description("This command allows you to save an Excel workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to save a workbook to a file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSaveAsCommand : ScriptCommand
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
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the file path to save")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the file.")]
        [SampleUsage("**C:\\temp\\myfile.xlsx** or **{{{vExcelFilePath}}}**")]
        [Remarks("If file does not contain extensin, supplement xlsx extension.\nIf file does not contain folder path, file will be saved in the same folder as script file.")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File")]
        public string v_FileName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify If Excel File Exists")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**Error** or **Overwrite** or **Ignore**")]
        [Remarks("")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Overwrite")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "Error")]
        public string v_IfExcelFileExists { get; set; }

        public ExcelSaveAsCommand()
        {
            this.CommandName = "ExcelSaveAsCommand";
            this.SelectionName = "Save Workbook As";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            string fileName;
            if (FilePathControls.containsFileCounter(v_FileName, engine))
            {
                fileName = FilePathControls.formatFilePath_ContainsFileCounter(v_FileName, engine, "xlsx");
            }
            else
            {
                fileName = FilePathControls.formatFilePath_NoFileCounter(v_FileName, engine, "xlsx");
            }

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
                            break;
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