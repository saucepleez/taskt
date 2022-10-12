using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.Description("This command allows you to check existance sheet")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to switch to a specific worksheet")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCheckWorksheetExistsCommand : ScriptCommand
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
        [PropertyDescription("Indicate the name of the sheet within the Workbook to check")]
        [InputSpecification("Specify the name of the actual sheet")]
        [SampleUsage("**mySheet**, **%kwd_current_worksheet%**, **{{{vSheet}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Sheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Sheet")]
        public string v_SheetName { get; set; }

        [PropertyDescription("Please select the variable to receive the existance sheet")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("Result is **TRUE** or **FALSE**")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_applyToVariable { get; set; }

        public ExcelCheckWorksheetExistsCommand()
        {
            this.CommandName = "ExcelCheckWorksheetExistsCommand";
            this.SelectionName = "Check Worksheet Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_InstanceName = "";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            //var excelObject = engine.GetAppInstance(vInstance);

            //Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            //string targetSheet = v_SheetName.ConvertToUserVariable(sender);

            //if (targetSheet == engine.engineSettings.CurrentWorksheetKeyword)
            //{
            //    "TRUE".StoreInUserVariable(sender, v_applyToVariable);
            //}
            //else
            //{
            //    bool result = false;
            //    foreach (Microsoft.Office.Interop.Excel.Worksheet sht in excelInstance.Worksheets)
            //    {
            //        if (sht.Name == targetSheet)
            //        {
            //            result = true;
            //            break;
            //        }
            //    }
            //    (result ? "TRUE" : "FALSE").StoreInUserVariable(sender, v_applyToVariable);
            //}

            //Microsoft.Office.Interop.Excel.Application excelInstance = ExcelControls.getExcelInstance(engine, vInstance);

            //var excelInstance = v_InstanceName.GetExcelInstance(engine);

            //string targetSheet = v_SheetName.ConvertToUserVariable(sender);
            //Microsoft.Office.Interop.Excel.Worksheet sht = ExcelControls.getWorksheet(engine, excelInstance, targetSheet);
            //var sht = v_SheetName.GetExcelWorksheet(engine, excelInstance, true);

            (_, var sht) = (v_InstanceName, v_SheetName).GetExcelInstanceAndWorksheet(engine, true);

            //(sht != null ? "TRUE" : "FALSE").StoreInUserVariable(sender, v_applyToVariable);
            (sht != null).StoreInUserVariable(engine, v_applyToVariable);
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
        //    return base.GetDisplayValue() + " [Check Existance Sheet Name: " + v_SheetName + ", Instance Name: '" + v_InstanceName + "', Result: '" + v_applyToVariable + "']";
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
        //        this.validationResult += "Sheet is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_applyToVariable))
        //    {
        //        this.validationResult += "Variable is empty.\n";
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