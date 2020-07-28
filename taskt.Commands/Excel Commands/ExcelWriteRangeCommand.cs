using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
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
using Group = taskt.Core.Attributes.ClassAttributes.Group;

namespace taskt.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command writes a DataTable to an Excel Worksheet starting from a specific cell address.")]

    public class ExcelWriteRangeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("DataTable")]
        [InputSpecification("Enter the DataTable to write to the Worksheet.")]
        [SampleUsage("{vDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataTableToSet { get; set; }

        [XmlAttribute]
        [PropertyDescription("Cell Location")]
        [InputSpecification("Enter the location of the cell to set the DataTable at.")]
        [SampleUsage("A1 || {vCellLocation}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_CellLocation { get; set; }

        [XmlAttribute]
        [PropertyDescription("Add Headers")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("When selected, the column headers from the specified DataTable are also written.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_AddHeaders { get; set; }

        public ExcelWriteRangeCommand()
        {
            CommandName = "ExcelWriteRangeCommand";
            SelectionName = "Write Range";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_AddHeaders = "Yes";
            v_CellLocation = "A1";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vDataSetVariable = LookupVariable(engine);
            var vTargetAddress = v_CellLocation.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);

            var excelInstance = (Application)excelObject;
            var excelSheet = (Worksheet)excelInstance.ActiveSheet;

            DataTable Dt = (DataTable)vDataSetVariable.VariableValue;
            if (string.IsNullOrEmpty(vTargetAddress) || vTargetAddress.Contains(":")) 
                throw new Exception("Cell Location is invalid or empty");
          
            var numberOfRow = Regex.Match(vTargetAddress, @"\d+").Value;
            vTargetAddress = Regex.Replace(vTargetAddress, @"[\d-]", string.Empty);
            vTargetAddress = vTargetAddress.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < vTargetAddress.Length; i++)
            {   
                sum *= 26;
                sum += (vTargetAddress[i] - 'A' + 1);
            }

            if (v_AddHeaders == "Yes")
            {
                //Write column names
                string columnName;
                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    if (Dt.Columns[j].ColumnName == "null")                    
                        columnName = string.Empty;                  
                    else                    
                        columnName = Dt.Columns[j].ColumnName;
                    
                    excelSheet.Cells[int.Parse(numberOfRow), j + sum] = columnName;
                }

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    for (int j = 0; j < Dt.Columns.Count; j++)
                    {
                        if (Dt.Rows[i][j].ToString() == "null")
                        {
                            Dt.Rows[i][j] = string.Empty;
                        }
                        excelSheet.Cells[i + int.Parse(numberOfRow) + 1, j + sum] = Dt.Rows[i][j].ToString();
                    }
                }
            }
            else { 
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    for (int j = 0; j < Dt.Columns.Count; j++)
                    {
                        if (Dt.Rows[i][j].ToString() == "null")
                        {
                            Dt.Rows[i][j] = string.Empty;
                        }
                        excelSheet.Cells[i + int.Parse(numberOfRow), j + sum] = Dt.Rows[i][j].ToString();
                    }
                }
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTableToSet", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_AddHeaders", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Write '{v_DataTableToSet}' to Cell '{v_CellLocation}' - Instance Name '{v_InstanceName}']";
        }

        private ScriptVariable LookupVariable(AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataTableToSet).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if (requiredVariable == null && v_DataTableToSet.StartsWith(sendingInstance.EngineSettings.VariableStartMarker) 
                                         && v_DataTableToSet.EndsWith(sendingInstance.EngineSettings.VariableEndMarker))
            {
                //reformat and attempt
                var reformattedVariable = v_DataTableToSet.Replace(sendingInstance.EngineSettings.VariableStartMarker, "")
                                                          .Replace(sendingInstance.EngineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
    }
}