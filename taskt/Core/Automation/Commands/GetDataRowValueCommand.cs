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
    [Attributes.ClassAttributes.Description("This command allows you to get a DataRow Value from a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a datarow to a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetDataRowValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataRow Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataRow to get Values from.")]
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
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_UserVariableName { get; set; }

        public GetDataRowValueCommand()
        {
            this.CommandName = "GetDataRowValueCommand";
            this.SelectionName = "Get DataRow Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            v_Option = "Index";

        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var dataRowVariable = LookupVariable(engine);
            var variableList = engine.VariableList;
            DataRow dataRow;
            //check if currently looping through datatable using BeginListLoopCommand
            if (dataRowVariable.VariableValue is DataTable && engine.VariableList.Exists(x => x.VariableName == "Loop.CurrentIndex"))
            {
                var loopIndexVariable = engine.VariableList.Where(x => x.VariableName == "Loop.CurrentIndex").FirstOrDefault();
                int loopIndex = int.Parse(loopIndexVariable.VariableValue.ToString());
                dataRow = ((DataTable)dataRowVariable.VariableValue).Rows[loopIndex-1];
            }

            else dataRow = (DataRow)dataRowVariable.VariableValue;

            var valueIndex = v_DataValueIndex.ConvertToUserVariable(sender);
            string value = "";
            if (v_Option == "Index")
            {
                int index = int.Parse(valueIndex);
                value = dataRow[index].ToString();

            }
            else if (v_Option == "Column Name")
            {
                string index = valueIndex;
                value = dataRow.Field<string>(index);
            }

            value.StoreInUserVariable(sender, v_UserVariableName);

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
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_UserVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_UserVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_UserVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }



        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get Value '{v_DataValueIndex}' from '{v_DataRowName}', Store In: '{v_UserVariableName}']";
        }
    }
}