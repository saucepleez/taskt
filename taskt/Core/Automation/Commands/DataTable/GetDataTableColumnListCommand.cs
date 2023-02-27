using System;
using System.Xml.Serialization;
using System.Data;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.CommandSettings("Get DataTable Column List")]
    [Attributes.ClassAttributes.Description("This command allows you to get the column name List of a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get the column name List of a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetDataTableColumnListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_OutputList { get; set; }

        public GetDataTableColumnListCommand()
        {
            //this.CommandName = "GetDataTableColumnListCommand";
            //this.SelectionName = "Get DataTable Column List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            myDT.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToList().StoreInUserVariable(engine, v_OutputList);
        }
    }
}