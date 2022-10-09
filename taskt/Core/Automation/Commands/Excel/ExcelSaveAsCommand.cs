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

        public ExcelSaveAsCommand()
        {
            this.CommandName = "ExcelSaveAsCommand";
            this.SelectionName = "Save Workbook As";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_InstanceName = "";
        }
        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Engine.AutomationEngineInstance)sender;

            //convert variables
            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            ////get excel app object
            //var excelObject = engine.GetAppInstance(vInstance);
            ////convert object
            //Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            var excelInstance = v_InstanceName.getExcelInstance(engine);

            //var fileName = v_FileName.ConvertToUserVariable(engine);
            //fileName = Core.FilePathControls.formatFilePath(fileName, engine);
            //if (!Core.FilePathControls.hasExtension(fileName))
            //{
            //    fileName += ".xlsx";
            //}
            string fileName;
            if (FilePathControls.containsFileCounter(v_FileName, engine))
            {
                fileName = FilePathControls.formatFilePath_ContainsFileCounter(v_FileName, engine, "xlsx");
            }
            else
            {
                fileName = FilePathControls.formatFilePath_NoFileCounter(v_FileName, engine, "xlsx");
            }

            //overwrite and save
            excelInstance.DisplayAlerts = false;
            excelInstance.ActiveWorkbook.SaveAs(fileName);
            excelInstance.DisplayAlerts = true;
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));
        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Save To '" + v_FileName + "', Instance Name: '" + v_InstanceName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_FileName))
        //    {
        //        this.validationResult += "File is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}