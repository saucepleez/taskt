using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command writes a datatable to an excel sheet starting from the given cell address.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelWriteRangeCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **excelInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Datatable Variable Name to Set")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text value that will be set.")]
        [Attributes.PropertyAttributes.SampleUsage("Hello World or [vText]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataTableToSet { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location to start from (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("A1, B10, [vAddress]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ExcelCellAddress { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Add Headers")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("When selected, the column headers from the specified spreadsheet range are also written.")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_AddHeaders { get; set; }
        public ExcelWriteRangeCommand()
        {
            this.CommandName = "ExcelWriteRangeCommand";
            this.SelectionName = "Write Range";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_AddHeaders = "Yes";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var dataSetVariable = LookupVariable(engine);
            var targetAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);
            var excelObject = engine.GetAppInstance(vInstance);

            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            var excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet;

            DataTable Dt = (DataTable)dataSetVariable.VariableValue;
            if (string.IsNullOrEmpty(targetAddress)) throw new ArgumentNullException("columnName");
          
            var numberOfRow = Regex.Match(targetAddress, @"\d+").Value;
            targetAddress = Regex.Replace(targetAddress, @"[\d-]", string.Empty);
            targetAddress = targetAddress.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < targetAddress.Length; i++)
            {   
                sum *= 26;
                sum += (targetAddress[i] - 'A' + 1);
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
                    
                    excelSheet.Cells[Int32.Parse(numberOfRow), j + sum] = columnName;
                }

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    for (int j = 0; j < Dt.Columns.Count; j++)
                    {
                        if (Dt.Rows[i][j].ToString() == "null")
                        {
                            Dt.Rows[i][j] = string.Empty;
                        }
                        excelSheet.Cells[i + Int32.Parse(numberOfRow) + 1, j + sum] = Dt.Rows[i][j].ToString();
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
                        excelSheet.Cells[i + Int32.Parse(numberOfRow), j + sum] = Dt.Rows[i][j].ToString();
                    }
                }
            }

        }

        private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataTableToSet).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_DataTableToSet.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_DataTableToSet.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_DataTableToSet.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTableToSet", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellAddress", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_AddHeaders", this, editor));

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Writing Cells starting from '" + v_ExcelCellAddress + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}