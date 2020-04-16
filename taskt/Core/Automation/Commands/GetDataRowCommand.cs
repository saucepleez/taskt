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
    [Attributes.ClassAttributes.Description("This command allows you to get a DataRow from a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a datarow to a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetDataRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataTable to fet rows from.")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the index of the DataRow")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a valid DataRow index value")]
        [Attributes.PropertyAttributes.SampleUsage("0 or [vIndex]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataRowIndex { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_UserVariableName { get; set; }

        public GetDataRowCommand()
        {
            this.CommandName = "GetDataRowCommand";
            this.SelectionName = "Get DataRow";
            this.CommandEnabled = true;
            this.CustomRendering = true;         

        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var dataSetVariable = LookupVariable(engine);
            DataTable dataTable = (DataTable)dataSetVariable.VariableValue;

            var rowIndex = v_DataRowIndex.ConvertToUserVariable(sender);
            int index = int.Parse(rowIndex);

            DataRow row = dataTable.Rows[index];

            Script.ScriptVariable newDataRow = new Script.ScriptVariable
            {
                VariableName = v_UserVariableName,
                VariableValue = row
            };

            //Overwrites variable if it already exists
            if (engine.VariableList.Exists(x => x.VariableName == newDataRow.VariableName))
            {
                Script.ScriptVariable temp = engine.VariableList.Where(x => x.VariableName == newDataRow.VariableName).FirstOrDefault();
                engine.VariableList.Remove(temp);
            }
            engine.VariableList.Add(newDataRow);

        }
        private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataTableName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_DataTableName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_DataTableName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_DataTableName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTableName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataRowIndex", this, editor));
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_UserVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_UserVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_UserVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }

        

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get DataRow '{v_DataRowIndex}' from '{v_DataTableName}', Store In: '{v_UserVariableName}']";
        }
    }
}