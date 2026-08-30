using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Convert Row")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Row To JSON")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Row to JSON")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Row to JSON.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertDataTableRowToJSONCommand : ADataTableGetFromDataTableRowCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        //public string v_DataTable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        //public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_OutputJSONName))]
        public override string v_Result { get; set; }

        public ConvertDataTableRowToJSONCommand()
        {
            //this.CommandName = "ConvertDataTableRowToJSONCommand";
            //this.SelectionName = "Convert DataTable Row To JSON";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;         
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var dicCommand = new ConvertDataTableRowToDictionaryCommand
            //{
            //    v_DataTable = this.v_DataTable,
            //    v_RowIndex = this.v_RowIndex,
            //    v_Result = VariableNameControls.GetInnerVariableName(0, engine)
            //};
            //dicCommand.RunCommand(engine);

            //var tDic = (Dictionary<string, string>)VariableNameControls.GetInnerVariable(0, engine).VariableValue;
            //string json = Newtonsoft.Json.JsonConvert.SerializeObject(tDic);
            //json.StoreInUserVariable(engine, v_Result);

            using (var myDic = new InnerScriptVariable(engine))
            {
                var dicCommand = new ConvertDataTableRowToDictionaryCommand
                {
                    v_DataTable = this.v_DataTable,
                    v_RowIndex = this.v_RowIndex,
                    v_Result = myDic.VariableName,
                };
                dicCommand.RunCommand(engine);

                //string json = Newtonsoft.Json.JsonConvert.SerializeObject((Dictionary<string, string>)myDic.VariableValue);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(EM_CanHandleDictionary.ExpandUserVariableAsDictionary(myDic));
                json.StoreInUserVariable(engine, v_Result);
            }
        }
    }
}