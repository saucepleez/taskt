using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.CommandSettings("Write Row")]
    [Attributes.ClassAttributes.Description("This command writes a DataRow to an excel sheet starting from the given cell address.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelWriteRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [SampleUsage("**myInstance** or **excelInstance**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [PropertyDescription("Please Enter the Row to Set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the text value that will be set (This could be a DataRow).")]
        [SampleUsage("Hello,World or {vText}")]
        [Remarks("")]
        public string v_DataRowToSet { get; set; }
        [XmlAttribute]
        [PropertyDescription("Please Enter the Cell Location to start from (ex. A1 or B2)")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the actual location of the cell.")]
        [SampleUsage("A1, B10, {vAddress}")]
        [Remarks("")]
        public string v_ExcelCellAddress { get; set; }

        public ExcelWriteRowCommand()
        {
            //this.CommandName = "ExcelWriteRowCommand";
            //this.SelectionName = "Write Row";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            this.v_InstanceName = "";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            var dataRowVariable = LookupVariable(engine);
            var variableList = engine.VariableList;
            DataRow row;

            var targetAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);

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

        private Script.ScriptVariable LookupVariable(Engine.AutomationEngineInstance sendingInstance)
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
            var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
            CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.Excel);
            RenderedControls.AddRange(instanceCtrls);
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataRowToSet", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellAddress", this, editor));

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
            }

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Writing Cells starting from '" + v_ExcelCellAddress + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}