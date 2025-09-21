using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Add Dictionary Items or Create Dictionary commands
    /// </summary>
    public abstract class ADictionaryAddCreateCommands : ADictionaryInputDictionaryCommands, IDictionaryAddCreateProperties
    {
        [XmlElement]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_KeyAndValue))]
        [PropertyParameterOrder(6000)]
        public virtual DataTable v_ColumnNameDataTable { get; set; }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_ColumnNameDataTable)], v_ColumnNameDataTable);
        }

        /// <summary>
        /// Add new item to Dictionary from DataTable. check key name is empty
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="engine"></param>
        /// <exception cref="Exception"></exception>
        public void AddDataAndValueFromDataTable(Dictionary<string, string> dic, Engine.AutomationEngineInstance engine)
        {
            // Check Items
            int rows = 0;
            foreach (DataRow row in v_ColumnNameDataTable.Rows)
            {
                rows++;
                var k = (row.Field<string>("Keys") ?? "").ExpandValueOrUserVariable(engine);
                if (k == "")
                {
                    throw new Exception($"Key name is empty. Row: {rows}");
                }
            }

            // Add Items
            foreach (DataRow row in v_ColumnNameDataTable.Rows)
            {
                var key = row.Field<string>("Keys").ExpandValueOrUserVariable(engine);
                var value = (row.Field<string>("Values") ?? "").ExpandValueOrUserVariable(engine);
                dic.Add(key, value);
            }
        }
    }
}
