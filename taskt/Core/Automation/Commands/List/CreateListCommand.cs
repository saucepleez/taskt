using System;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to create new List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create new List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the List Variable Name.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a existing List.")]
        [SampleUsage("**myList** or **{{{myList}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List")]
        public string v_ListName { get; set; }

        [XmlElement]
        [PropertyDescription("Assign to List Values")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true)]
        [PropertyDataGridViewColumnSettings("Values", "Values", false)]
        //[PropertyControlIntoCommandField("ListValuesGridViewHelper")]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls)+"+"+nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyDisplayText(true, "Items")]
        public DataTable v_ListValues { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private DataGridView ListValuesGridViewHelper;

        public CreateListCommand()
        {
            this.CommandName = "CreateListCommand";
            this.SelectionName = "Create List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> newList = new List<string>();

            foreach(DataRow row in v_ListValues.Rows)
            {
                newList.Add(row["Values"].ToString());
            }

            newList.StoreInUserVariable(engine, v_ListName);
        }
        
        //private void ListValuesDataGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex >= 0)
        //    {
        //        //ListValuesGridViewHelper.BeginEdit(false);
        //        ((DataGridView)sender).BeginEdit(false);
        //    }
        //}

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            //var ListValuesGridViewHelper = (DataGridView)this.ControlsList[nameof(v_ListValues)];

            //if (ListValuesGridViewHelper.IsCurrentCellDirty || ListValuesGridViewHelper.IsCurrentRowDirty)
            //{
            //    ListValuesGridViewHelper.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //    var newRow = v_ListValues.NewRow();
            //    v_ListValues.Rows.Add(newRow);
            //    for (var i = v_ListValues.Rows.Count - 1; i >= 0; i--)
            //    {
            //        if (v_ListValues.Rows[i][0].ToString() == "")
            //        {
            //            v_ListValues.Rows[i].Delete();
            //        }
            //    }
            //}
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_ListValues)], v_ListValues);
        }
    }
}