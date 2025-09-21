using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.CommandSettings("Add DataTable Row By Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to add a DataTable Row to a DataTable by a Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a DataTable Row to a DataTable by a Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class AddDataTableRowByDictionaryCommand : ADataTableBothDataTableCommands, ICanHandleDictionary
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        [PropertyDescription("DataTable Variable Name to be Added a Row")]
        public override string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        [PropertyDescription("Dictionary Variable Name to add to the DataTable")]
        [PropertyParameterOrder(6000)]
        public string v_RowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_WhenColumnNotExists))]
        [PropertyDescription("When Dictionary Key does not Exists")]
        [PropertyParameterOrder(7000)]
        public string v_WhenColumnNotExists { get; set; }

        public AddDataTableRowByDictionaryCommand()
        {
            //this.CommandName = "AddDataTableRowByDictionaryCommand";
            //this.SelectionName = "Add DataTable Row By Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //DataTable myDT = v_DataTable.ExpandUserVariableAsDataTable(engine);
            var myDT = this.ExpandUserVariableAsDataTable(engine);

            //Dictionary<string, string> myDic = v_RowName.ExpandUserVariableAsDictinary(engine);
            var myDic = this.ExpandUserVariableAsDictionary(nameof(v_RowName), engine);

            string notExistsKey = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenColumnNotExists), "Key Does Not Exists", engine);

            // get columns list
            //var columns = myDT.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            var columns = myDT.GetColumnNameList();

            // check column exists
            if (notExistsKey == "error")
            {
                foreach (var item in myDic)
                {
                    if (!columns.Contains(item.Key))
                    {
                        throw new Exception("Column name " + item.Key + " does not exists");
                    }
                }
            }

            var row = myDT.NewRow();
            foreach(var item in myDic)
            {
                if (columns.Contains(item.Key))
                {
                    row[item.Key] = item.Value;
                }
            }
            myDT.Rows.Add(row);
        }
    }
}