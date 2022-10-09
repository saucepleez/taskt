using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.Description("This command delete a Excel Worksheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a new worksheet to an Excel Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelDeleteWorksheetCommand : ScriptCommand
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
        [PropertyValidationRule("Worksheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Worksheet")]
        public string v_SheetName { get; set; }

        public ExcelDeleteWorksheetCommand()
        {
            this.CommandName = "ExcelDeleteWorksheetCommand";
            this.SelectionName = "Delete Worksheet";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //var excelObject = engine.GetAppInstance(vInstance);

            //Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;

            //if (targetSheet == engine.engineSettings.CurrentWorksheetKeyword)
            //{
            //    ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet).Delete();
            //}
            //else
            //{
            //    ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[targetSheet]).Delete();
            //}

            //Microsoft.Office.Interop.Excel.Application excelInstance = ExcelControls.getExcelInstance(engine, vInstance);

            var excelInstance = v_InstanceName.getExcelInstance(engine);

            //var targetSheetName = v_SheetName.ConvertToUserVariable(engine);
            //Microsoft.Office.Interop.Excel.Worksheet targetSheet = ExcelControls.getWorksheet(engine, excelInstance, targetSheetName);

            //if (targetSheet != null)
            //{
            //    targetSheet.Delete();
            //}
            //else
            //{
            //    throw new Exception("Worksheet " + targetSheetName + " does not exists.");
            //}

            var targetSheet = v_SheetName.GetExcelWorksheet(engine, excelInstance);
            if (targetSheet != null)
            {
                targetSheet.Delete();
            }
            else
            {
                throw new Exception("Worksheet " + v_SheetName + " does not exists.");
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_sourceSheet", this, editor));
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
        //    return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SheetName))
        //    {
        //        this.validationResult += "Worksheet is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}

        public override void convertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_SheetName", "convertToIntermediateExcelSheet");
            convertToIntermediate(settings, cnv, variables);
        }

        public override void convertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_SheetName", "convertToRawExcelSheet");
            convertToRaw(settings, cnv);
        }
    }
}