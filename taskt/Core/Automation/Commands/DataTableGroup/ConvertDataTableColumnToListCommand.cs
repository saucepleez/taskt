using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Convert Column")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Column To List")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Column to List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Column to List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDataTableColumnToListCommand : ADataTableGetFromDataTableColumnCommands, IListResultProperties
    {
        //    [XmlAttribute]
        //    [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        //    public string v_DataTable { get; set; }

        //    [XmlAttribute]
        //    [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        //    public string v_ColumnType { get; set; }

        //    [XmlAttribute]
        //    [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        //    public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public override string v_Result { get; set; }

        public ConvertDataTableColumnToListCommand()
        {
            //this.CommandName = "ConvertDataTableColumnToListCommand";
            //this.SelectionName = "Convert DataTable Column To List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;         
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var srcDT, var colIndex) = this.ExpandUserVariablesAsDataTableAndColumnIndex(nameof(v_DataTable), nameof(v_ColumnType), nameof(v_ColumnIndex), engine);
            (var srcDT, var colIndex, _) = this.ExpandValueOrUserVariableAsDataTableAndColumn(engine);
            
            var myList = new List<string>();
            for (int i = 0; i < srcDT.Rows.Count; i++)
            {
                myList.Add(srcDT.Rows[i][colIndex]?.ToString() ?? "");
            }

            //myList.StoreInUserVariable(engine, v_Result);
            //this.StoreListInUserVariable(myList, nameof(v_Result), engine);
            this.StoreListInUserVariable(myList, engine);
        }
    }
}