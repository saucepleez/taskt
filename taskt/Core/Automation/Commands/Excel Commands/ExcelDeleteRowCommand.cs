using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command deletes a specific row in an Excel Worksheet.")]

    public class ExcelDeleteRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Row Number")]
        [InputSpecification("Enter the number of the row to be deleted.")]
        [SampleUsage("1 || {vRowNumber}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_RowNumber { get; set; }

        [XmlAttribute]
        [PropertyDescription("Shift Cells Up")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether the row(s) below will be shifted up to replace the old row(s).")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ShiftUp { get; set; }

        public ExcelDeleteRowCommand()
        {
            CommandName = "ExcelDeleteRowCommand";
            SelectionName = "Delete Row";
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
            Worksheet workSheet = excelInstance.ActiveSheet;
            string vRowToDelete = v_RowNumber.ConvertToUserVariable(sender);

            var cells = workSheet.Range["A" + vRowToDelete, Type.Missing];
            var entireRow = cells.EntireRow;
            if (v_ShiftUp == "Yes")            
                entireRow.Delete();            
            else            
                entireRow.Clear();      
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RowNumber", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ShiftUp", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Delete Row '{v_RowNumber}' - Instance Name '{v_InstanceName}']";
        }
    }
}