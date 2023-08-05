using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.CommandSettings("Add DataTable Row By Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to add a DataTable Row to a DataTable by a Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a DataTable Row to a DataTable by a Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class AddDataTableRowByDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        [PropertyDescription("DataTable Variable Name to be Added a Row")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        [PropertyDescription("Dictionary Variable Name to add to the DataTable")]
        public string v_RowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_WhenColumnNotExists))]
        [PropertyDescription("When Dictionary Key does not Exists")]
        public string v_NotExistsKey { get; set; }

        public AddDataTableRowByDictionaryCommand()
        {
            //this.CommandName = "AddDataTableRowByDictionaryCommand";
            //this.SelectionName = "Add DataTable Row By Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataTable myDT = v_DataTableName.GetDataTableVariable(engine);

            Dictionary<string, string> myDic = v_RowName.GetDictionaryVariable(engine);

            string notExistsKey = this.GetUISelectionValue(nameof(v_NotExistsKey), "Key Does Not Exists", engine);

            // get columns list
            List<string> columns = myDT.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

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

            DataRow row = myDT.NewRow();
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