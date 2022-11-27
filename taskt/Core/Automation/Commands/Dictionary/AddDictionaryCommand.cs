using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Item")]
    [Attributes.ClassAttributes.Description("This command Adds a key and value to a existing Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add to a dictionary")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class AddDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Indicate Dictionary Name")]
        [InputSpecification("Indicate a Dictionary to add to")]
        [SampleUsage("**vMyDictionary** or **{{{vDictionary}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary")]
        public string v_DictionaryName { get; set; }

        [XmlElement]
        [PropertyDescription("Define Keys and Values")]
        [InputSpecification("Enter the Keys and Values required for your dictionary")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true)]
        [PropertyDataGridViewColumnSettings("Keys", "Keys", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("Values", "Values", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewCellEditEvent(nameof(ColumnNameDataGridViewHelper_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyDisplayText(true, "Items")]
        public DataTable v_ColumnNameDataTable { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private DataGridView ColumnNameDataGridViewHelper;

        public AddDictionaryCommand()
        {
            this.CommandName = "AddDictionaryItemCommand";
            this.SelectionName = "Add Dictionary Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            Dictionary<string, string> outputDictionary = v_DictionaryName.GetDictionaryVariable(engine);

            foreach (DataRow rwColumnName in v_ColumnNameDataTable.Rows)
            {
                outputDictionary.Add(rwColumnName.Field<string>("Keys"), rwColumnName.Field<string>("Values"));
            }
        }
        
        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    ColumnNameDataGridViewHelper = (DataGridView)ctrls.GetControlsByName("v_ColumnNameDataTable")[0];

        //    return RenderedControls;
        //}

        private void ColumnNameDataGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView ColumnNameDataGridViewHelper = (DataGridView)sender;
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
            //DataGridView ColumnNameDataGridViewHelper = (DataGridView)ControlsList[nameof(v_ColumnNameDataTable)];
            //if (ColumnNameDataGridViewHelper.IsCurrentCellDirty || ColumnNameDataGridViewHelper.IsCurrentRowDirty)
            //{
            //    ColumnNameDataGridViewHelper.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //    var newRow = v_ColumnNameDataTable.NewRow();
            //    v_ColumnNameDataTable.Rows.Add(newRow);
            //    for (var i = v_ColumnNameDataTable.Rows.Count - 1; i >= 0; i--)
            //    {
            //        if (v_ColumnNameDataTable.Rows[i][0].ToString() == "" && v_ColumnNameDataTable.Rows[i][1].ToString() == "")
            //        {
            //            v_ColumnNameDataTable.Rows[i].Delete();
            //        }
            //    }
            //}
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_ColumnNameDataTable)], v_ColumnNameDataTable);
        }
    }
}