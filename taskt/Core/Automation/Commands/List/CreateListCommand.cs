﻿using System;
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
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to create new List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create new List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class CreateListCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the List Variable Name.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing List.")]
        [Attributes.PropertyAttributes.SampleUsage("**myList** or **{{{myList}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        public string v_ListName { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Assign to List Values")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [Attributes.PropertyAttributes.PropertyDataGridViewSetting(true, true, true)]
        [Attributes.PropertyAttributes.PropertyDataGridViewColumnSettings("Values", "Values", false)]
        [Attributes.PropertyAttributes.PropertyDataGridViewCellEditEvent("ListValuesDataGridViewHelper_CellClick", Attributes.PropertyAttributes.PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public DataTable v_ListValues { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView ListValuesGridViewHelper;

        public CreateListCommand()
        {
            this.CommandName = "CreateListCommand";
            this.SelectionName = "Create List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            List<string> newList = new List<string>();

            foreach(DataRow row in v_ListValues.Rows)
            {
                newList.Add(row["Values"].ToString());
            }

            newList.StoreInUserVariable(engine, v_ListName);
        }
        
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            ListValuesGridViewHelper = (DataGridView)ctrls.GetControlsByName("v_ListValues")[0];

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Create List '{v_ListName}']";
        }

        private void ListValuesDataGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                ListValuesGridViewHelper.BeginEdit(false);
            }
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            if (ListValuesGridViewHelper.IsCurrentCellDirty || ListValuesGridViewHelper.IsCurrentRowDirty)
            {
                ListValuesGridViewHelper.CommitEdit(DataGridViewDataErrorContexts.Commit);
                var newRow = v_ListValues.NewRow();
                v_ListValues.Rows.Add(newRow);
                for (var i = v_ListValues.Rows.Count - 1; i >= 0; i--)
                {
                    if (v_ListValues.Rows[i][0].ToString() == "")
                    {
                        v_ListValues.Rows[i].Delete();
                    }
                }
            }
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_ListName))
            {
                this.validationResult += "List Name is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}