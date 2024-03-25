using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.CommandSettings("Append Row")]
    [Attributes.ClassAttributes.Description("Append to last row of sheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command will take in a comma seprerated value and append it to the end of an excel sheet.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automations.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelAppendRowCommand : AExcelColumnSpecifiedCommands
    {
        //[XmlAttribute]
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        //[SampleUsage("**myInstance** or **excelInstance**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter the Row to set")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter the text value that will be set (This could be a DataRow).")]
        //[SampleUsage("Hello,world or {vText}")]
        //[Remarks("")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueToSet))]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**Hello,World**", "Specified **Hello** and **World**.")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Value To Set")]
        [PropertyParameterOrder(6000)]
        public string v_TextToSet { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text Separator")]
        [InputSpecification("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**,**", PropertyDetailSampleUsage.ValueType.Value, "Text Separator")]
        [PropertyDetailSampleUsage("**{{{vSep}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text Separator")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, ",")]
        [PropertyFirstValue(",")]
        [Remarks("")]
        [PropertyDisplayText(false, "Text Separator")]
        [PropertyParameterOrder(6500)]
        public string v_TextSeparator { get; set; }

        //[XmlAttribute]
        //public string v_ColumnType { get; set; }

        //[XmlAttribute]
        //public string v_ColumnIndex { get; set; }

        //[XmlAttribute]
        //public string v_ValueType { get; set; }

        public ExcelAppendRowCommand()
        {
            //this.CommandName = "ExcelAppendRowCommand";
            //this.SelectionName = "Append Row";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //this.v_InstanceName = "";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var splittext = v_TextToSet.Split(',');

            ////(_, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);
            //(_, var excelSheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            //int i = 1;            
            //int lastUsedRow;
            //try
            //{
            //    lastUsedRow = excelSheet.Cells.Find("*", System.Reflection.Missing.Value,
            //                     System.Reflection.Missing.Value, System.Reflection.Missing.Value,
            //                     Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
            //                     false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;
            //}
            //catch(Exception)
            //{
            //    lastUsedRow = 0;
            //}

            //var targetText = v_TextToSet.ExpandValueOrUserVariable(engine);
            //splittext = targetText.Split(',');

            //foreach (var item in splittext)
            //{
            //    string output = item;
            //    if (item.Contains("[") || item.Contains("]"))
            //        output = item.Trim('[', ']');
            //    output = output.Trim('"');
            //    if (output=="null")
            //    {
            //        output = string.Empty;
            //    }
            //    excelSheet.Cells[lastUsedRow + 1, i] = output;
            //    i++;
            //}

            (var sheet, var columnIndex) = this.ExpandValueOrVariableAsExcelCurrentWorksheetAndColumnIndex(engine);

            var rowIndex = sheet.FirstBlankRowIndex(columnIndex, 1, this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ValueType), "Value Type", engine));

            var setFunc = this.ExpandValueOrVaribleAsSetValueAction(engine);

            var targetText = v_TextToSet.ExpandValueOrUserVariable(engine);
            var separator = v_TextSeparator.ExpandValueOrUserVariable(engine);

            if (separator.Length > 1)
            {
                throw new Exception($"Separator must specify only one character. Value: '{v_TextSeparator}', Expand Value: '{separator}'");
            }

            var lst = targetText.Split(separator[0]);

            int index = 0;
            foreach(var v in lst)
            {
                setFunc(v, sheet, columnIndex + index, rowIndex);
                index++;
            }
        }

        //public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.Excel);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));

        //    if (editor.creationMode == UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Append Row '" +v_TextToSet+ " to last row of workboook with Instance Name: '" + v_InstanceName + "']";
        //}
    }
}