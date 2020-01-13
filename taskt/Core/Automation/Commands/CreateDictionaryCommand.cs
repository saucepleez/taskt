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
    [Attributes.ClassAttributes.Description("This command created a DataTable with the column names provided")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a new Dictionary")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class CreateDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Indicate Dictionary Name")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate a unique reference name for later use")]
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


        public CreateDictionaryCommand()
        {
            this.CommandName = "CreateDictionaryCommand";
            this.SelectionName = "Create Dictionary";
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

            Dictionary<string, string> outputDictionary = new Dictionary<string, string>();

            foreach (DataRow rwColumnName in v_ColumnNameDataTable.Rows)
            {
                outputDictionary.Add(rwColumnName.Field<string>("Keys"), rwColumnName.Field<string>("Values"));
            }


            

            //add or override existing variable
            if (engine.VariableList.Any(f => f.VariableName == dictionaryName))
            {
                var selectedVariable = engine.VariableList.Where(f => f.VariableName == dictionaryName).FirstOrDefault();
                selectedVariable.VariableValue = outputDictionary;
            }
            else
            {
                Script.ScriptVariable newDictionary = new Script.ScriptVariable
                {
                    VariableName = dictionaryName,
                    VariableValue = outputDictionary
                };

                engine.VariableList.Add(newDictionary);
            }


     
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