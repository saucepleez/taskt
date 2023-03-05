using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Sheet")]
    [Attributes.ClassAttributes.CommandSettings("Get Worksheet Info")]
    [Attributes.ClassAttributes.Description("This command allows you to get a sheet info.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetWorksheetInfoCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_SheetName))]
        public string v_SheetName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Information Type")]
        [InputSpecification("", true)]
        [SampleUsage("**Name** or **Visible** or **Is first sheet** or **Is last sheet** or **Next sheet** or **Previous sheet** or **Sheet index**")]
        [Remarks("")]
        [PropertyUISelectionOption("Name")]
        [PropertyUISelectionOption("Visible")]
        [PropertyUISelectionOption("Is first sheet")]
        [PropertyUISelectionOption("Is last sheet")]
        [PropertyUISelectionOption("Next sheet")]
        [PropertyUISelectionOption("Previous sheet")]
        [PropertyUISelectionOption("Sheet index")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Information Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariable { get; set; }

        public ExcelGetWorksheetInfoCommand()
        {
            //this.CommandName = "ExcelWorksheetInfoCommand";
            //this.SelectionName = "Get Worksheet Info";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var excelInstance, var targetSheet) = (v_InstanceName, v_SheetName).GetExcelInstanceAndWorksheet(engine);

            string ret = "";
            int idx = 1;

            var infoType = this.GetUISelectionValue(nameof(v_InfoType), "Info Type", engine);
            switch (infoType)
            {
                case "name":
                    ret = targetSheet.Name;
                    break;
                case "visible":
                    ret = (targetSheet.Visible == Microsoft.Office.Interop.Excel.XlSheetVisibility.xlSheetVisible) ? "TRUE" : "FALSE";
                    break;
                case "is first sheet":
                    ret = (((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[1]).Name == targetSheet.Name) ? "TRUE" : "FALSE";
                    break;
                case "is last sheet":
                    ret = (((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[excelInstance.Worksheets.Count]).Name == targetSheet.Name) ? "TRUE" : "FALSE";
                    break;
                case "next sheet":
                    var nextSheet = engine.engineSettings.NextWorksheetKeyword.GetExcelWorksheet(engine, excelInstance, true);
                    ret = (nextSheet == null) ? "" : nextSheet.Name;
                    break;
                case "previous sheet":
                    var prevSheet = engine.engineSettings.PreviousWorksheetKeyword.GetExcelWorksheet(engine, excelInstance, true);
                    ret = (prevSheet == null) ? "" : prevSheet.Name;
                    break;
                case "sheet index":
                    foreach (Microsoft.Office.Interop.Excel.Worksheet sht in excelInstance.Worksheets)
                    {
                        if (sht.Name == targetSheet.Name)
                        {
                            break;
                        }
                        idx++;
                    }
                    ret = idx.ToString();
                    break;
            }

            ret.StoreInUserVariable(sender, v_applyToVariable);
        }
    }
}