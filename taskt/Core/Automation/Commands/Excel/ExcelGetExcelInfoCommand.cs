using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Get Excel Info")]
    [Attributes.ClassAttributes.Description("This command allows you to get current sheet name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetExcelInfoCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the information type to receive")]
        [InputSpecification("")]
        [SampleUsage("**File name** or **Full path file name** or **Current sheet** or **Number of sheets** or **First sheet** or **Last sheet**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("File name")]
        [PropertyUISelectionOption("Full path file name")]
        [PropertyUISelectionOption("Current sheet")]
        [PropertyUISelectionOption("Number of sheets")]
        [PropertyUISelectionOption("First sheet")]
        [PropertyUISelectionOption("Last sheet")]
        [PropertyValidationRule("Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariable { get; set; }

        public ExcelGetExcelInfoCommand()
        {
            //this.CommandName = "ExcelGetExcelInfoCommand";
            //this.SelectionName = "Get Excel Info";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            var infoType = this.GetUISelectionValue(nameof(v_InfoType), "Info Type", engine);
            string ret = "";
            switch (infoType)
            {
                case "file name":
                    ret = excelInstance.ActiveWorkbook?.Name ?? "";
                    break;
                case "full path file name":
                    ret = excelInstance.ActiveWorkbook?.FullName ?? "";
                    break;
                case "current sheet":
                    var sheet = engine.engineSettings.CurrentWorksheetKeyword.GetExcelWorksheet(engine, excelInstance, true);
                    ret = (sheet == null) ? "" : sheet.Name;
                    break;
                case "number of sheets":
                    try
                    {
                        ret = excelInstance.Worksheets.Count.ToString();
                    }
                    catch
                    {
                        ret = "0";
                    }
                    break;
                case "first sheet":
                    try
                    {

                        ret = ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[1]).Name;
                    }
                    catch
                    {
                        ret = "";
                    }
                    break;
                case "last sheet":
                    try
                    {
                        ret = ((Microsoft.Office.Interop.Excel.Worksheet)excelInstance.Worksheets[excelInstance.Worksheets.Count]).Name;
                    }
                    catch
                    {
                        ret = "";
                    }
                    break;
            }

            ret.StoreInUserVariable(sender, v_applyToVariable);
        }
    }
}