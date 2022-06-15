using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("File/Book")]
    [Attributes.ClassAttributes.Description("This command opens an Excel Workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to open an existing Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelOpenWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the workbook file path")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file that should be opened by Excel.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myfile.xlsx** or **{{{vFilePath}}}**")]
        [Attributes.PropertyAttributes.Remarks("If file does not contain extension, supplement extensions supported by Excel.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate open password")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file that should be opened by Excel.")]
        [Attributes.PropertyAttributes.SampleUsage("**myPassword** or **{{{vPassword}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_Password { get; set; }
        public ExcelOpenWorkbookCommand()
        {
            this.CommandName = "ExcelOpenWorkbookCommand";
            this.SelectionName = "Open Workbook";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_InstanceName = "";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //var vFilePath = v_FilePath.ConvertToUserVariable(sender);
            //vFilePath = Core.FilePathControls.formatFilePath(vFilePath, engine);
            //if (!System.IO.File.Exists(vFilePath) && !Core.FilePathControls.hasExtension(vFilePath))
            //{
            //    string[] exts = new string[] { ".xlsx", ".xlsm", ".xls", ".csv", ".ods" };
            //    foreach(string ext in exts)
            //    {
            //        if (System.IO.File.Exists(vFilePath + ext))
            //        {
            //            vFilePath += ext;
            //            break;
            //        }
            //    }
            //}
            string vFilePath = FilePathControls.formatFilePath_NoFileCounter(v_FilePath, engine, new List<string>() { "xlsx", "xlsm", "xls", "csv", "ods" }, true);

            var excelObject = engine.GetAppInstance(vInstance);
            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;

            var pass = v_Password.ConvertToUserVariable(sender);

            if (String.IsNullOrEmpty(pass))
            {
                excelInstance.Workbooks.Open(vFilePath);
            }
            else
            {
                excelInstance.Workbooks.Open(vFilePath, Password: pass);
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
            }

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Open from '" + v_FilePath + "', Instance Name: '" + v_InstanceName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InstanceName))
            {
                this.validationResult += "Instance is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_FilePath))
            {
                this.validationResult += "File is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}