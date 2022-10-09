using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.Description("This command copy a Excel Worksheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy a new worksheet to an Excel Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCopyWorksheetCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the target worksheet name")]
        [InputSpecification("")]
        [SampleUsage("**mySheet** or **%kwd_current_worksheet%** or **{{{vSheet}}}**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Target Sheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Target Sheet")]
        public string v_sourceSheet { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the new worksheet name")]
        [InputSpecification("")]
        [SampleUsage("**newSheet** or **{{{vNewSheet}}}**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(true, "New Sheet")]
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
            var engine = (Engine.AutomationEngineInstance)sender;

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

            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //Microsoft.Office.Interop.Excel.Application excelInstance = ExcelControls.getExcelInstance(engine, vInstance);

            var excelInstance = v_InstanceName.getExcelInstance(engine);

            //var targetSheetName = v_sourceSheet.ConvertToUserVariable(engine);
            //Microsoft.Office.Interop.Excel.Worksheet targetSheet = ExcelControls.getWorksheet(engine, excelInstance, targetSheetName);

            //if (targetSheet != null)
            //{
            //    targetSheet.Copy(Before: excelInstance.Worksheets[1]);
            //}
            //else
            //{
            //    throw new Exception("Worksheet " + targetSheetName + " does not exists.");
            //}

            var targetSheet = v_sourceSheet.GetExcelWorksheet(engine, excelInstance);
            if (targetSheet == null)
            {
                throw new Exception("Worksheet " + v_sourceSheet + " does not exists.");
            }

            targetSheet.Copy(Before: excelInstance.Worksheets[1]);

            var newName = v_newSheetName.ConvertToUserVariable(sender);
            if (!String.IsNullOrEmpty(newName))
            {
                ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet).Name = newName;
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

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
        //    return base.GetDisplayValue() + " [Copy sheet '" + v_sourceSheet + "', Instance Name: '" + v_InstanceName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_sourceSheet))
        //    {
        //        this.validationResult += "Worksheet is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}

        public override void convertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_sourceSheet", "convertToIntermediateExcelSheet");
            convertToIntermediate(settings, cnv, variables);
        }

        public override void convertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_sourceSheet", "convertToRawExcelSheet");
            convertToRaw(settings, cnv);
        }
    }
}