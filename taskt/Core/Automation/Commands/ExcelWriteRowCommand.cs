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
    [Attributes.ClassAttributes.Description("This command writes a DataRow to an excel sheet starting from the given cell address.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelWriteRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **excelInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Row to Set")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text value that will be set (This could be a DataRow).")]
        [Attributes.PropertyAttributes.SampleUsage("Hello,World or [vText]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataRowToSet { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location to start from (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("A1, B10, [vAddress]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ExcelCellAddress { get; set; }

        public ExcelWriteRowCommand()
        {
            this.CommandName = "ExcelWriteRowCommand";
            this.SelectionName = "Write Row";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var dataRowVariable = LookupVariable(engine);
            var variableList = engine.VariableList;
            DataRow row;

            var targetAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);
            var excelObject = engine.GetAppInstance(vInstance);

            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            var excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet;

            //check in case of looping through datatable using BeginListLoopCommand
            if (dataRowVariable.VariableValue is DataTable && engine.VariableList.Exists(x => x.VariableName == "Loop.CurrentIndex"))
            {
                var loopIndexVariable = engine.VariableList.Where(x => x.VariableName == "Loop.CurrentIndex").FirstOrDefault();
                int loopIndex = int.Parse(loopIndexVariable.VariableValue.ToString());
                row = ((DataTable)dataRowVariable.VariableValue).Rows[loopIndex - 1];
            }

            else row = (DataRow)dataRowVariable.VariableValue;


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

           
            //Write row
            string cellValue;
            for (int j = 0; j < row.ItemArray.Length; j++)
            {
                if (row.ItemArray[j] == null)
                    cellValue = string.Empty;

                else
                    cellValue = row.ItemArray[j].ToString();

                excelSheet.Cells[Int32.Parse(numberOfRow), j + sum] = cellValue;
            }                
        }        

        private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataRowToSet).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_DataRowToSet.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_DataRowToSet.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_DataRowToSet.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataRowToSet", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellAddress", this, editor));

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Writing Cells starting from '" + v_ExcelCellAddress + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}