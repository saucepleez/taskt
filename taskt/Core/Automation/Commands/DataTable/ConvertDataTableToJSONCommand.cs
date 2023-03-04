using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable To JSON")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable to JSON")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable to JSON.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDataTableToJSONCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_OutputJSONName))]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableToJSONCommand()
        {
            //this.CommandName = "ConvertDataTableToJSONCommand";
            //this.SelectionName = "Convert DataTable To JSON";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataTable srcDT = v_DataTableName.GetDataTableVariable(engine);

            List<Dictionary<string, string>> jsonList = new List<Dictionary<string, string>>();
            for (int j = 0; j < srcDT.Rows.Count; j++)
            {
                Dictionary<string, string> tDic = new Dictionary<string, string>();
                for (int i = 0; i < srcDT.Columns.Count; i++)
                {
                    tDic.Add(srcDT.Columns[i].ColumnName, srcDT.Rows[j][i]?.ToString() ?? "");
                }

                jsonList.Add(tDic);
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonList);
            json.StoreInUserVariable(engine, v_OutputVariableName);
        }
    }
}