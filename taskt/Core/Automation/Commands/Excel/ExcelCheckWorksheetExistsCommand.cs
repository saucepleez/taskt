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
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Instance")]
        //[PropertyFirstValue("%kwd_default_excel_instance%")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Indicate the name of the sheet within the Workbook to check")]
        //[InputSpecification("Specify the name of the actual sheet")]
        //[SampleUsage("**mySheet**, **%kwd_current_worksheet%**, **{{{vSheet}}}**")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Sheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Sheet")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        public string v_SheetName { get; set; }

        //[PropertyDescription("Please select the variable to receive the existance sheet")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("Result is **TRUE** or **FALSE**")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Sheet Exists, Result is **True**")]
        public string v_applyToVariable { get; set; }

        public ExcelCheckWorksheetExistsCommand()
        {
            this.CommandName = "ExcelCheckWorksheetExistsCommand";
            this.SelectionName = "Check Worksheet Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (_, var sht) = (v_InstanceName, v_SheetName).GetExcelInstanceAndWorksheet(engine, true);

            (sht != null).StoreInUserVariable(engine, v_applyToVariable);
        }

        //public override void ConvertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        //{
        //    var cnv = new Dictionary<string, string>();
        //    cnv.Add(nameof(v_SheetName), "convertToIntermediateExcelSheet");
        //    ConvertToIntermediate(settings, cnv, variables);
        //}

        //public override void ConvertToRaw(EngineSettings settings)
        //{
        //    var cnv = new Dictionary<string, string>();
        //    cnv.Add(nameof(v_SheetName), "convertToRawExcelSheet");
        //    ConvertToRaw(settings, cnv);
        //}
    }
}