using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Drawing;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.Description("This command Adds a key and value to a existing Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add to a dictionary")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class AddDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Indicate Dictionary Name")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate a Dictionary to add to")]
        [Attributes.PropertyAttributes.SampleUsage("vMyDictionary")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DictionaryName { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Define Keys and Values")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the Keys and Values required for your dictionary")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_ColumnNameDataTable { get; set; }


        public AddDictionaryCommand()
        {
            this.CommandName = "CreateDictionaryCommand";
            this.SelectionName = "Add Dictionary Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //initialize Datatable
            this.v_ColumnNameDataTable = new System.Data.DataTable
            {
                TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            this.v_ColumnNameDataTable.Columns.Add("Keys");
            this.v_ColumnNameDataTable.Columns.Add("Values");



        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var dictionaryName = v_DictionaryName.ConvertToUserVariable(sender);
            var dataSetVariable = LookupVariable(engine);


            Dictionary<string, string> outputDictionary = (Dictionary<string, string>)dataSetVariable.VariableValue;

            foreach (DataRow rwColumnName in v_ColumnNameDataTable.Rows)
            {
                outputDictionary.Add(rwColumnName.Field<string>("Keys"), rwColumnName.Field<string>("Values"));
            }
            dataSetVariable.VariableValue = outputDictionary;


        }
        private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DictionaryName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_DictionaryName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_DictionaryName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_DictionaryName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DictionaryName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDataGridViewGroupFor("v_ColumnNameDataTable", this, editor));
            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Name: '{v_DictionaryName}' with {v_ColumnNameDataTable.Rows.Count} Entries]";
        }
    }
}