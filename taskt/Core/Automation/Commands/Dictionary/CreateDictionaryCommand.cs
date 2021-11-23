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
        [Attributes.PropertyAttributes.PropertyDescription("Please Indicate Dictionary Variable Name")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate a unique reference name for later use")]
        [Attributes.PropertyAttributes.SampleUsage("**vMyDictionary** or **{{{vMyDictionary}}}**")]
        [Attributes.PropertyAttributes.Remarks("Create Dictionary<string, string>")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        public string v_DictionaryName { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Define Keys and Values")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the Keys and Values required for your dictionary")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [Attributes.PropertyAttributes.PropertyDataGridViewColumnSettings("Keys", "Keys", false)]
        [Attributes.PropertyAttributes.PropertyDataGridViewColumnSettings("Values", "Values", false)]
        [Attributes.PropertyAttributes.PropertyDataGridViewSetting(true, true, true)]
        [Attributes.PropertyAttributes.PropertyDataGridViewCellEditEvent("ColumnNameDataGridViewHelper_CellClick", Attributes.PropertyAttributes.PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public DataTable v_ColumnNameDataTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView ColumnNameDataGridViewHelper;

        public CreateDictionaryCommand()
        {
            this.CommandName = "CreateDictionaryCommand";
            this.SelectionName = "Create Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            //var dictionaryName = v_DictionaryName.ConvertToUserVariable(sender);

            Dictionary<string, string> outputDictionary = new Dictionary<string, string>();

            foreach (DataRow rwColumnName in v_ColumnNameDataTable.Rows)
            {
                outputDictionary.Add(rwColumnName.Field<string>("Keys"), rwColumnName.Field<string>("Values"));
            }

            //add or override existing variable
            //if (engine.VariableList.Any(f => f.VariableName == dictionaryName))
            //{
            //    var selectedVariable = engine.VariableList.Where(f => f.VariableName == dictionaryName).FirstOrDefault();
            //    selectedVariable.VariableValue = outputDictionary;
            //}
            //else
            //{
            //    Script.ScriptVariable newDictionary = new Script.ScriptVariable
            //    {
            //        VariableName = dictionaryName,
            //        VariableValue = outputDictionary
            //    };

            //    engine.VariableList.Add(newDictionary);
            //}
            outputDictionary.StoreInUserVariable(engine, v_DictionaryName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //initialize Datatable
            //this.v_ColumnNameDataTable = new System.Data.DataTable
            //{
            //    TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            //};
            //this.v_ColumnNameDataTable.Columns.Add("Keys");
            //this.v_ColumnNameDataTable.Columns.Add("Values");

            //create standard group controls
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DictionaryName", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateInferenceDefaultControlGroupFor("v_DictionaryName", this, editor));

            //RenderedControls.AddRange(CommandControls.CreateDataGridViewGroupFor("v_ColumnNameDataTable", this, editor));

            //ColumnNameDataGridViewHelper = (DataGridView)RenderedControls[RenderedControls.Count - 1];
            //ColumnNameDataGridViewHelper.Tag = "column-a-editable";
            //ColumnNameDataGridViewHelper.CellClick += ColumnNameDataGridViewHelper_CellClick;

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            //ColumnNameDataGridViewHelper = (DataGridView)ctrls.Where(t => (t.Name == "v_ColumnNameDataTable")).FirstOrDefault();
            //ColumnNameDataGridViewHelper.CellClick += ColumnNameDataGridViewHelper_CellClick;
            ColumnNameDataGridViewHelper = (DataGridView)ctrls.GetControlsByName("v_ColumnNameDataTable")[0];

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Name: '{v_DictionaryName}' with {v_ColumnNameDataTable.Rows.Count} Entries]";
        }

        private void ColumnNameDataGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                ColumnNameDataGridViewHelper.BeginEdit(false);
            }
            else
            {
                ColumnNameDataGridViewHelper.EndEdit();
            }
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            if (ColumnNameDataGridViewHelper.IsCurrentCellDirty || ColumnNameDataGridViewHelper.IsCurrentRowDirty)
            {
                ColumnNameDataGridViewHelper.CommitEdit(DataGridViewDataErrorContexts.Commit);
                var newRow = v_ColumnNameDataTable.NewRow();
                v_ColumnNameDataTable.Rows.Add(newRow);
                for (var i = v_ColumnNameDataTable.Rows.Count - 1; i >= 0; i--)
                {
                    if (v_ColumnNameDataTable.Rows[i][0].ToString() == "" && v_ColumnNameDataTable.Rows[i][1].ToString() == "")
                    {
                        v_ColumnNameDataTable.Rows[i].Delete();
                    }
                }
            }
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            if (String.IsNullOrEmpty(v_DictionaryName))
            {
                this.IsValid = false;
                this.validationResult += "Dictionary Variable Name is empty.\n";
            }

            return this.IsValid;
        }
    }
}