using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Dictionary Commands")]
    [Description("This command creates a new Dictionary.")]
    public class CreateDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("New Dictionary Name")]
        [InputSpecification("Specify a unique reference name for the new dictionary.")]
        [SampleUsage("vDictionaryName")]
        [Remarks("")]
        public string v_DictionaryName { get; set; }

        [XmlElement]
        [PropertyDescription("Keys and Values")]
        [InputSpecification("Enter the Keys and Values required for the new dictionary.")]
        [SampleUsage("[FirstName | John] || [{vKey} | {vValue}]")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_ColumnNameDataTable { get; set; }

        public CreateDictionaryCommand()
        {
            CommandName = "CreateDictionaryCommand";
            SelectionName = "Create Dictionary";
            CommandEnabled = true;
            CustomRendering = true;

            //initialize Datatable
            v_ColumnNameDataTable = new DataTable
            {
                TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            v_ColumnNameDataTable.Columns.Add("Keys");
            v_ColumnNameDataTable.Columns.Add("Values");
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            Dictionary<string, string> outputDictionary = new Dictionary<string, string>();

            foreach (DataRow rwColumnName in v_ColumnNameDataTable.Rows)
            {
                outputDictionary.Add(
                    rwColumnName.Field<string>("Keys").ConvertToUserVariable(engine), 
                    rwColumnName.Field<string>("Values").ConvertToUserVariable(engine));
            }

            engine.AddVariable(v_DictionaryName, outputDictionary);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DictionaryName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDataGridViewGroupFor("v_ColumnNameDataTable", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Having Name '{v_DictionaryName}' With {v_ColumnNameDataTable.Rows.Count} Entries]";
        }
    }
}