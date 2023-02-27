using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.CommandSettings("Delete DataTable Row")]
    [Attributes.ClassAttributes.Description("This command allows you to delete a DataTable Row")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to delete a DataTable Row.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class DeleteDataTableRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        public string v_RowIndex { get; set; }

        public DeleteDataTableRowCommand()
        {
            //this.CommandName = "DeleteDataTableRowCommand";
            //this.SelectionName = "Delete DataTable Row";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var myDT, var rowIndex) = this.GetDataTableVariableAndRowIndex(nameof(v_DataTableName), nameof(v_RowIndex), engine);

            myDT.Rows[rowIndex].Delete();
        }
    }
}