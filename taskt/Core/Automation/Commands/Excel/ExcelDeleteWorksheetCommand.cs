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
        //[PropertyDescription("Please Enter the target worksheet name")]
        //[InputSpecification("")]
        //[SampleUsage("**mySheet** or **%kwd_current_worksheet%** or **{{{vSheet}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Worksheet", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Worksheet")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
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

            (_, var targetSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            targetSheet.Delete();
        }

        public override void ConvertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add(nameof(v_SheetName), "convertToIntermediateExcelSheet");
            ConvertToIntermediate(settings, cnv, variables);
        }

        public override void ConvertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add(nameof(v_SheetName), "convertToRawExcelSheet");
            ConvertToRaw(settings, cnv);
        }
    }
}