using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to write a Value to a DataRow")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to write a Value to a DataRow.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class WriteDataRowValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataRow Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataTable to add rows to.")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataRowName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select value by Index or Column Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Index")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Column Name")]
        [Attributes.PropertyAttributes.InputSpecification("Select whether the DataRow value should be found by index or column name")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Index** or **Column Name**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Option { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the index of the DataRow Value")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a valid DataRow index value")]
        [Attributes.PropertyAttributes.SampleUsage("0 or [vIndex]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataValueIndex { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the Value")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the value to write to the DataRow cell")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataRowValue { get; set; }

        public WriteDataRowValueCommand()
        {
            this.CommandName = "WriteDataRowValueCommand";
            this.SelectionName = "Write DataRow Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            v_Option = "Index";

        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var dataRowValue = v_DataRowValue.ConvertToUserVariable(sender);

            var dataRowVariable = LookupVariable(engine);
            var variableList = engine.VariableList;
            DataRow dataRow;

            //check in case of looping through datatable using BeginListLoopCommand
            if (dataRowVariable.VariableValue is DataTable && engine.VariableList.Exists(x => x.VariableName == "Loop.CurrentIndex"))
            {
                var loopIndexVariable = engine.VariableList.Where(x => x.VariableName == "Loop.CurrentIndex").FirstOrDefault();
                int loopIndex = int.Parse(loopIndexVariable.VariableValue.ToString());
                dataRow = ((DataTable)dataRowVariable.VariableValue).Rows[loopIndex - 1];
            }

            else dataRow = (DataRow)dataRowVariable.VariableValue;

            var valueIndex = v_DataValueIndex.ConvertToUserVariable(sender);

            if (v_Option == "Index")
            {
                int index = int.Parse(valueIndex);
                dataRow[index] = dataRowValue;

            }
            else if (v_Option == "Column Name")
            {
                string index = valueIndex;
                dataRow.SetField<string>(index, dataRowValue);
            }

        }
        private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataRowName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_DataRowName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_DataRowName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_DataRowName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataRowName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataValueIndex", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataRowValue", this, editor));


            return RenderedControls;
        }



        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Write Value '{v_DataRowValue}' to '{v_DataValueIndex}' in '{v_DataRowName}']";
        }
    }
}