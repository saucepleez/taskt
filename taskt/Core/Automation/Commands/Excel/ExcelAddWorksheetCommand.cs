using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.Description("This command adds a new Excel Worksheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a new worksheet to an Excel Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelAddWorksheetCommand : ScriptCommand
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
        //[PropertyFirstValue("%kwd_default_excel_instance%")]
        //[PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Instance")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter the new sheet name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        //[SampleUsage("**myNewSheet** or **{{{vSheet}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Sheet Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Sheet")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        [PropertyDescription("New Sheet Name")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**mySheet**", PropertyDetailSampleUsage.ValueType.Value, "Sheet Name")]
        [PropertyDetailSampleUsage("**{{{vSheet}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Sheet Name")]
        public string v_NewSheetName { get; set; }

        public ExcelAddWorksheetCommand()
        {
            this.CommandName = "ExcelAddWorksheetCommand";
            this.SelectionName = "Add Worksheet";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            
            var excelInstance = v_InstanceName.GetExcelInstance(engine);
            excelInstance.Worksheets.Add();

            var sheetName = v_NewSheetName.ConvertToUserVariable(engine);
            if (!String.IsNullOrEmpty(sheetName))
            {
                ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet).Name = sheetName;
            }
        }
    }
}