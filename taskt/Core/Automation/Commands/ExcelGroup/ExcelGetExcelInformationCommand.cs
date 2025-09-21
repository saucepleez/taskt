using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using Microsoft.Office.Interop.Excel;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Get Excel Information")]
    [Attributes.ClassAttributes.Description("This command allows you to get current sheet name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ExcelGetExcelInformationCommand : AExcelInstanceCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the information type to receive")]
        [InputSpecification("")]
        [SampleUsage("**File name** or **Full path file name** or **Current sheet** or **Number of sheets** or **First sheet** or **Last sheet**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("File Name")]
        [PropertyUISelectionOption("Full Path File Name")]
        [PropertyUISelectionOption("Current Worksheet")]
        [PropertyUISelectionOption("Number Of Worksheets")]
        [PropertyUISelectionOption("First Worksheet")]
        [PropertyUISelectionOption("Last Worksheet")]
        [PropertyValidationRule("Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        [PropertyParameterOrder(6000)]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6001)]
        public string v_Result { get; set; }

        public ExcelGetExcelInformationCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var excelInstance = this.ExpandValueOrVariableAsExcelInstance(engine);

            string ret = "";
            switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_InfoType), "Info Type", engine))
            {
                case "file name":
                    ret = excelInstance.ActiveWorkbook?.Name ?? "";
                    break;
                case "full path file name":
                    ret = excelInstance.ActiveWorkbook?.FullName ?? "";
                    break;
                case "current worksheet":
                    ret = (excelInstance.Worksheets.Count > 0) ? excelInstance.ActiveSheet.Name : "";
                    break;
                case "number of worksheets":
                    try
                    {
                        ret = excelInstance.Worksheets.Count.ToString();
                    }
                    catch
                    {
                        ret = "0";
                    }
                    break;
                case "first worksheet":
                    try
                    {

                        ret = ((Worksheet)excelInstance.Worksheets[1]).Name;
                    }
                    catch
                    {
                        ret = "";
                    }
                    break;
                case "last worksheet":
                    try
                    {
                        ret = ((Worksheet)excelInstance.Worksheets[excelInstance.Worksheets.Count]).Name;
                    }
                    catch
                    {
                        ret = "";
                    }
                    break;
            }

            ret.StoreInUserVariable(engine, v_Result);
        }
    }
}