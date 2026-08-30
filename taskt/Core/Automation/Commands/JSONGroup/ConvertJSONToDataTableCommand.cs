using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert JSON To DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to convert JSON to DataTable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert JSON to DataTable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertJSONToDataTableCommand : AJSONGetFromJContainerCommands, IDataTableResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_InputJSONName))]
        //public string v_Json { get; set; }

        //[XmlAttribute]
        //public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public override string v_Result { get; set; }

        public ConvertJSONToDataTableCommand()
        {
            //this.CommandName = "ConvertJSONToDataTableCommand";
            //this.SelectionName = "Convert JSON To DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //Action<JObject> objFunc = new Action<JObject>((obj) =>
            //{
            //    var resultDT = new DataTable();

            //    resultDT.Rows.Add();
            //    int i = 0;
            //    foreach (var result in obj)
            //    {
            //        resultDT.Columns.Add(result.Key);
            //        resultDT.Rows[0][i] = result.Value.ToString();
            //        i++;
            //    }
            //    //resultDT.StoreInUserVariable(engine, v_applyToVariableName);
            //    this.StoreDataTableInUserVariable(resultDT, nameof(v_Result), engine);
            //});
            //Action<JArray> aryFunc = new Action<JArray>((ary) =>
            //{
            //    var resultDT = new DataTable();
            //    //parseJSONArrayAsDataTable(ary, resultDT).StoreInUserVariable(engine, v_applyToVariableName);
            //    this.StoreDataTableInUserVariable(parseJSONArrayAsDataTable(ary, resultDT), nameof(v_Result), engine);
            //});
            //this.JSONProcess(nameof(v_Json), objFunc, aryFunc, engine);

            //(_, var jCon, _) = this.ExpandValueOrUserVariableAsJSON(engine);
            (_, var jCon, _) = this.ExpandUserVariableAsJSONByJSONPath(engine);

            var res = this.CreateEmptyDataTable();
            if (jCon is JObject obj) 
            {
                res.Rows.Add();
                int i = 0;
                foreach(var item in obj)
                {
                    res.Columns.Add(item.Key);
                    res.Rows[0][i] = item.Value.ToString();
                    i++;
                }
            }
            else if (jCon is JArray ary)
            {
                ParseJSONArrayAsDataTable(ary, res);
            }
            else if (jCon is JValue va)
            {
                res.Columns.Add("column0");
                res.Rows.Add();
                res.Rows[0][0] = va.ToString();
            }
            else
            {
                throw new Exception($"Extraction Result is NOT Supported Type. Result: '{jCon}', JSONPath: '{v_JsonExtractor}'");
            }
            this.StoreDataTableInUserVariable(res, engine);
        }

        private static DataTable ParseJSONArrayAsDataTable(JArray arr, DataTable DT)
        {
            var arr0 = arr[0].ToString();
            if (arr0.StartsWith("{") && arr0.EndsWith("}"))
            {
                // Object
                JObject col = JObject.Parse(arr[0].ToString());
                //int colSize = col.Count;
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
                    DT.Columns.Add($"column{i}");
                    DT.Rows[0][i] = arr[i].ToString();
                }
            }

            return DT;
        }
    }
}