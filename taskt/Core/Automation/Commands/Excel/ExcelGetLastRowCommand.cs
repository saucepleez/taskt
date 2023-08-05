using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.CommandSettings("Get Last Row Index")]
    [Attributes.ClassAttributes.Description("This command allows you to find the last row in a used range in an Excel Workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to determine how many rows have been used in the Excel Workbook.  You can use this value in a **Number Of Times** Loop to get data.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetLastRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Letter of the Column to check")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Column Letter", true)]
        //[SampleUsage("**A** or **B** or **{{{vColumn}}}**")]
        [PropertyDetailSampleUsage("**A**", "Specify the Column **A**")]
        [PropertyDetailSampleUsage("**B**", "Specify the Column **B**")]
        [PropertyDetailSampleUsage("**{{{vColumn}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "A")]
        [PropertyDisplayText(true, "Column")]
        public string v_ColumnLetter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public ExcelGetLastRowCommand()
        {
            //this.CommandName = "ExcelGetLastRowCommand";
            //this.SelectionName = "Get Last Row Index";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (_, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            if (String.IsNullOrEmpty(v_ColumnLetter))
            {
                v_ColumnLetter = "A";
            }
            var columnLetter = v_ColumnLetter.ConvertToUserVariable(engine);

            var lastRow = (int)excelSheet.Cells[excelSheet.Rows.Count, columnLetter].End(Microsoft.Office.Interop.Excel.XlDirection.xlUp).Row;

            lastRow.ToString().StoreInUserVariable(sender, v_applyToVariableName);
        }
    }
}