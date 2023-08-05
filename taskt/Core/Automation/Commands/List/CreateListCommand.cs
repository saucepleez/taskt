using System;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Create List")]
    [Attributes.ClassAttributes.Description("This command allows you to create new List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create new List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_ListName { get; set; }

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
        public DataTable v_ListValues { get; set; }

        public CreateListCommand()
        {
            //this.CommandName = "CreateListCommand";
            //this.SelectionName = "Create List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> newList = new List<string>();

            newList.AddRange(v_ListValues.AsEnumerable().Select(r => r["Values"]?.ToString() ?? "").ToArray());

            newList.StoreInUserVariable(engine, v_ListName);
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_ListValues)], v_ListValues);
        }
    }
}