using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Create List")]
    [Attributes.ClassAttributes.Description("This command allows you to create new List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create new List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CreateListCommand : AListInputListCommands, IHaveDataTableElements
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public override string v_List { get; set; }

        [XmlElement]
        [PropertyDescription("List Values")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true)]
        [PropertyDataGridViewColumnSettings("Values", "Values", false)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls)+"+"+nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyDisplayText(true, "Items")]
        [PropertyDetailSampleUsage("**1**", "Set **1**")]
        [PropertyDetailSampleUsage("**ABC**", "Set **ABC**")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", "Set Value of Variable **vValue**")]
        [PropertyParameterOrder(6000)]
        public DataTable v_ListValues { get; set; }

        public CreateListCommand()
        {
            //this.CommandName = "CreateListCommand";
            //this.SelectionName = "Create List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var newList = new List<string>();

            //newList.AddRange(v_ListValues.AsEnumerable().Select(r => r["Values"]?.ToString() ?? "").ToArray());
            var ary = v_ListValues.AsEnumerable().Select(r => r["Values"]?.ToString() ?? "").ToArray();
            foreach(var v in ary)
            {
                newList.Add(v.ExpandValueOrUserVariable(engine));
            }

            //newList.StoreInUserVariable(engine, v_List);
            this.StoreListInUserVariable(newList, nameof(v_List), engine);
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_ListValues)], v_ListValues);
        }
    }
}