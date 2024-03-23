using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.CommandSettings("Get DataTable Column Count")]
    [Attributes.ClassAttributes.Description("This command allows you to get the column count of a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get the column count of a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetDataTableColumnCountCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public GetDataTableColumnCountCommand()
        {
            //this.CommandName = "GetDataTableColumnCountCommand";
            //this.SelectionName = "Get DataTable Column Count";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            DataTable myDT = v_DataTable.ExpandUserVariableAsDataTable(engine);

            myDT.Columns.Count.ToString().StoreInUserVariable(engine, v_Result);
        }
    }
}