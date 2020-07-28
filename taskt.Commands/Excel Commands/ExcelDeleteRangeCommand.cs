using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace taskt.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command deletes a specific cell or range in an Excel Worksheet.")]

    public class ExcelDeleteRangeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Range")]
        [InputSpecification("Enter the location of the cell or range to delete.")]
        [SampleUsage("A1 || A1:B10 || A1: || {vRange} || {vStart}:{vEnd} || {vStart}:")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Range { get; set; }

        [XmlAttribute]
        [PropertyDescription("Shift Cells Up")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether the row(s) below will be shifted up to replace the old row(s).")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ShiftUp { get; set; }

        public ExcelDeleteRangeCommand()
        {
            CommandName = "ExcelDeleteRangeCommand";
            SelectionName = "Delete Range";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_ShiftUp = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);
            var excelInstance = (Application)excelObject;
            Worksheet excelSheet = excelInstance.ActiveSheet;

            string vRange = v_Range.ConvertToUserVariable(engine);
            var splitRange = vRange.Split(':');
            Range cellRange;
            //Delete a range of cells
            try
            {
                Range last = excelSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
                if (splitRange[1] == "")
                    cellRange = excelSheet.Range[splitRange[0], last];
                else
                    cellRange = excelSheet.Range[splitRange[0], splitRange[1]];
            }
            //Delete a cell
            catch (Exception)
            {
                cellRange = excelSheet.Range[splitRange[0], Type.Missing];
            }

            if (v_ShiftUp == "Yes")
                cellRange.Delete();          
            else
                cellRange.Clear();
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Range", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ShiftUp", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Delete Cells '{v_Range}' - Instance Name '{v_InstanceName}']";
        }
    }
}