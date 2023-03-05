using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("File/Book")]
    [Attributes.ClassAttributes.CommandSettings("Add Workbook")]
    [Attributes.ClassAttributes.Description("This command adds a new Excel Workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a new workbook to an Exel Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelAddWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("When Workbook Exists")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**Ignore**", "Do not add a Workbook")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [PropertyDetailSampleUsage("**Add**", "Add a Workbook. This should result in two Workbooks open")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Add")]
        [PropertyIsOptional(true, "Error")]
        public string v_IfWorkbookExists { get; set; }

        public ExcelAddWorkbookCommand()
        {
            //this.CommandName = "ExcelAddWorkbookCommand";
            //this.SelectionName = "Add Workbook";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            var ifWorkbookExists = this.GetUISelectionValue(nameof(v_IfWorkbookExists), engine);

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