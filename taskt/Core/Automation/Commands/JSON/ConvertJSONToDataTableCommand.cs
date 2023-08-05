using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using Newtonsoft.Json.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert JSON To DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to convert JSON to DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert JSON to DataTable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertJSONToDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_InputJSONName))]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public string v_applyToVariableName { get; set; }

        public ConvertJSONToDataTableCommand()
        {
            //this.CommandName = "ConvertJSONToDataTableCommand";
            //this.SelectionName = "Convert JSON To DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            Action<JObject> objFunc = new Action<JObject>((obj) =>
            {
                DataTable resultDT = new DataTable();

                resultDT.Rows.Add();
                int i = 0;
                foreach (var result in obj)
                {
                    resultDT.Columns.Add(result.Key);
                    resultDT.Rows[0][i] = result.Value.ToString();
                    i++;
                }
                resultDT.StoreInUserVariable(engine, v_applyToVariableName);
            });
            Action<JArray> aryFunc = new Action<JArray>((ary) =>
            {
                DataTable resultDT = new DataTable();
                parseJSONArrayAsDataTable(ary, resultDT).StoreInUserVariable(engine, v_applyToVariableName);
            });
            this.JSONProcess(nameof(v_InputValue), objFunc, aryFunc, engine);
        }

        private static DataTable parseJSONArrayAsDataTable(JArray arr, DataTable DT)
        {
            var arr0 = arr[0].ToString();
            if (arr0.StartsWith("{") && arr0.EndsWith("}"))
            {
                // Object
                JObject col = JObject.Parse(arr[0].ToString());
                int colSize = col.Count;
                DT.Rows.Add();
                foreach (var co in col)
                {
                    DT.Columns.Add(co.Key);
                    DT.Rows[0][co.Key] = co.Value.ToString();
                }

                for (int i = 1; i < arr.Count; i++)
                {
                    DT.Rows.Add();
                    JObject row = JObject.Parse(arr[i].ToString());
                    foreach(var co in row)
                    {
                        DT.Rows[i][co.Key] = co.Value.ToString();
                    }
                }
            }
            else if (arr0.StartsWith("[") && arr0.EndsWith("]"))
            {
                // 2Array
                JArray col = JArray.Parse(arr[0].ToString());
                int colSize = col.Count;
                DT.Rows.Add();
                for (int i = 0; i < colSize; i++)
                {
                    DT.Columns.Add("column" + i.ToString());
                    DT.Rows[0][i] = col[i].ToString();
                }

                for (int i = 1; i < arr.Count; i++)
                {
                    DT.Rows.Add();
                    JArray row = JArray.Parse(arr[i].ToString());
                    int count = (row.Count < colSize) ? row.Count : colSize;
                    for (int j = 0; j < count; j++)
                    {
                        DT.Rows[i][j] = row[j].ToString();
                    }
                }
            }
            else
            {
                // 1Array
                DT.Rows.Add();
                for (int i = 0; i < arr.Count; i++)
                {
                    DT.Columns.Add("column" + i.ToString());
                    DT.Rows[0][i] = arr[i].ToString();
                }
            }

            return DT;
        }
    }
}