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
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.Description("This command copy a Excel Worksheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy a new worksheet to an Excel Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelCopyWorksheetCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the target worksheet name")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**mySheet** or **%kwd_current_worksheet%** or **{{{vSheet}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_sourceSheet { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the new worksheet name")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**newSheet** or **{{{vNewSheet}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_newSheetName { get; set; }

        public ExcelCopyWorksheetCommand()
        {
            this.CommandName = "ExcelCopyWorksheetCommand";
            this.SelectionName = "Copy Worksheet";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var targetSheetName = v_sourceSheet.ConvertToUserVariable(engine);

            //var excelObject = engine.GetAppInstance(vInstance);
            //Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;

            //if (targetSheetName == engine.engineSettings.CurrentWorksheetKeyword)
            //{
            //    ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet).Copy(Before:excelInstance.Worksheets[1]);
            //}
            //else
            //{
            //    ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[targetSheetName]).Copy(Before: excelInstance.Worksheets[1]);
            //}

            Microsoft.Office.Interop.Excel.Application excelInstance = ExcelControls.getExcelInstance(engine, vInstance);
            Microsoft.Office.Interop.Excel.Worksheet targetSheet = ExcelControls.getWorksheet(engine, excelInstance, targetSheetName);

            if (targetSheet != null)
            {
                targetSheet.Copy(Before: excelInstance.Worksheets[1]);
            }
            else
            {
                throw new Exception("Worksheet " + targetSheetName + " does not exists.");
            }

            var newName = v_newSheetName.ConvertToUserVariable(sender);
            if (!String.IsNullOrEmpty(newName))
            {
                ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet).Name = newName;
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

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
            return base.GetDisplayValue() + " [Copy sheet '" + v_sourceSheet + "', Instance Name: '" + v_InstanceName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InstanceName))
            {
                this.validationResult += "Instance is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_sourceSheet))
            {
                this.validationResult += "Worksheet is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }

        public override void convertToIntermediate(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_sourceSheet", "convertToIntermediateExcelSheet");
            convertToIntermediate(settings, cnv);
        }

        public override void convertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_sourceSheet", "convertToRawExcelSheet");
            convertToRaw(settings, cnv);
        }
    }
}