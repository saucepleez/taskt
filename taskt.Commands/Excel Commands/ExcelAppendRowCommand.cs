using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

namespace taskt.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command appends a row after the last row of an Excel Worksheet.")]

    public class ExcelAppendRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Row")]
        [InputSpecification("Enter the text value that will be set in the appended row (Can be a DataRow).")]
        [SampleUsage("Hello,World || {vData1},{vData2} || {vDataRow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_RowToSet { get; set; }

        public ExcelAppendRowCommand()
        {
            CommandName = "ExcelAppendRowCommand";
            SelectionName = "Append Row";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vRow = LookupVariable(engine);
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);
            var excelInstance = (Application)excelObject;
            Worksheet excelSheet = excelInstance.ActiveSheet;

            int lastUsedRow;
            int i = 1;
            try
            {
                lastUsedRow = excelSheet.Cells.Find("*", Missing.Value, Missing.Value, Missing.Value, XlSearchOrder.xlByRows, 
                                                    XlSearchDirection.xlPrevious, false, Missing.Value, Missing.Value).Row;
            }
            catch(Exception)
            {
                lastUsedRow = 0;
            }

            DataRow row;
            //check in case of looping through datatable using BeginListLoopCommand
            if (vRow != null && vRow.VariableValue is DataTable && engine.VariableList.Exists(x => x.VariableName == "Loop.CurrentIndex"))
            {
                var loopIndexVariable = engine.VariableList.Where(x => x.VariableName == "Loop.CurrentIndex").FirstOrDefault();
                int loopIndex = int.Parse(loopIndexVariable.VariableValue.ToString());
                row = ((DataTable)vRow.VariableValue).Rows[loopIndex - 1];

                string cellValue;
                for (int j = 0; j < row.ItemArray.Length; j++)
                {
                    if (row.ItemArray[j] == null)
                        cellValue = string.Empty;
                    else
                        cellValue = row.ItemArray[j].ToString();

                    excelSheet.Cells[lastUsedRow + 1, i] = cellValue;
                    i++;
                }
            }
            else if (vRow != null && vRow.VariableValue is DataRow)
            {
                row = (DataRow)vRow.VariableValue;

                string cellValue;
                for (int j = 0; j < row.ItemArray.Length; j++)
                {
                    if (row.ItemArray[j] == null)
                        cellValue = string.Empty;
                    else
                        cellValue = row.ItemArray[j].ToString();

                    excelSheet.Cells[lastUsedRow + 1, i] = cellValue;
                    i++;
                }
            }
            else
            {
                string vRowString = v_RowToSet.ConvertToUserVariable(engine);
                var splittext = vRowString.Split(',');
                string cellValue;
                foreach (var item in splittext)
                {
                    cellValue = item;
                    if (cellValue == "null")
                    {
                        cellValue = string.Empty;
                    }
                    excelSheet.Cells[lastUsedRow + 1, i] = cellValue;
                    i++;
                }
            }          
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RowToSet", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Append '{v_RowToSet}' - Instance Name '{v_InstanceName}']";
        }

        private ScriptVariable LookupVariable(AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_RowToSet).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if (requiredVariable == null && v_RowToSet.StartsWith(sendingInstance.EngineSettings.VariableStartMarker) 
                                         && v_RowToSet.EndsWith(sendingInstance.EngineSettings.VariableEndMarker))
            {
                //reformat and attempt
                var reformattedVariable = v_RowToSet.Replace(sendingInstance.EngineSettings.VariableStartMarker, "")
                                                    .Replace(sendingInstance.EngineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
    }
}
