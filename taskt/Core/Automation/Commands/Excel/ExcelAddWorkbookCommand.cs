using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("File/Book")]
    [Attributes.ClassAttributes.Description("This command adds a new Excel Workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a new workbook to an Exel Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelAddWorkbookCommand : ScriptCommand
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
        [PropertyFirstValue("%kwd_default_excel_instance%")]
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Select If Workbook Exists")]
        [InputSpecification("")]
        [SampleUsage("**Ignore** or **Error** or **Add**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Add")]
        [PropertyIsOptional(true, "Error")]
        public string v_IfWorkbookExists { get; set; }

        public ExcelAddWorkbookCommand()
        {
            this.CommandName = "ExcelAddWorkbookCommand";
            this.SelectionName = "Add Workbook";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            var ifWorkbookExists = v_IfWorkbookExists.GetUISelectionValue("v_IfWorkbookExists", this, engine);

            if (excelInstance.Workbooks.Count > 0)
            {
                switch (ifWorkbookExists)
                {
                    case "error":
                        throw new Exception("Excel Instance '" + v_InstanceName + "' has Workbook.");

                    case "ignore":
                        break;

                    case "add":
                        excelInstance.Workbooks.Add();
                        break;
                }
            }
            else
            {
                excelInstance.Workbooks.Add();
            }
        }
    }
}