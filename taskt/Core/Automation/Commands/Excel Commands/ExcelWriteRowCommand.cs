using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;
using Group = taskt.Core.Automation.Attributes.ClassAttributes.Group;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command writes a DataRow to an Excel Worksheet starting from a specific cell address.")]

    public class ExcelWriteRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Row")]
        [InputSpecification("Enter the text value that will be set in the selected row (Can be a DataRow).")]
        [SampleUsage("Hello,World || {vData1},{vData2} || {vDataRow}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_RowToSet { get; set; }

        [XmlAttribute]
        [PropertyDescription("Cell Location")]
        [InputSpecification("Enter the location of the cell to write the row to.")]
        [SampleUsage("A1 || {vCellLocation}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_CellLocation { get; set; }

        public ExcelWriteRowCommand()
        {
            CommandName = "ExcelWriteRowCommand";
            SelectionName = "Write Row";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_CellLocation = "A1";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vRow = LookupVariable(engine);
            var vTargetAddress = v_CellLocation.ConvertToUserVariable(sender);
            var excelObject = engine.GetAppInstance(vInstance);
            var excelInstance = (Application)excelObject;
            var excelSheet = (Worksheet)excelInstance.ActiveSheet;
            
            if (string.IsNullOrEmpty(vTargetAddress)) 
                throw new ArgumentNullException("columnName");

            var numberOfRow = int.Parse(Regex.Match(vTargetAddress, @"\d+").Value);
            vTargetAddress = Regex.Replace(vTargetAddress, @"[\d-]", string.Empty);
            vTargetAddress = vTargetAddress.ToUpperInvariant();

            int sum = 0;
            for (int i = 0; i < vTargetAddress.Length; i++)
            {
                sum *= 26;
                sum += (vTargetAddress[i] - 'A' + 1);
            }

            //Write row
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

                    excelSheet.Cells[numberOfRow, j + sum] = cellValue;
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

                    excelSheet.Cells[numberOfRow, j + sum] = cellValue;
                }
            }
            else
            {
                string vRowString = v_RowToSet.ConvertToUserVariable(sender);
                var splittext = vRowString.Split(',');

                string cellValue;
                for (int j = 0; j < splittext.Length; j++)
                {
                    cellValue = splittext[j];
                    if (cellValue == "null")
                    {
                        cellValue = string.Empty;
                    }
                    excelSheet.Cells[numberOfRow, j + sum] = cellValue;
                }
            }
        }        

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RowToSet", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Write '{v_RowToSet}' to Row '{v_CellLocation}' - Instance Name '{v_InstanceName}']";
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