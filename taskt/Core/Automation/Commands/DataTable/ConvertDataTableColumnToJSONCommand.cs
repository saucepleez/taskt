using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert Column")]
    [Attributes.ClassAttributes.CommandSettings("Convert DataTable Column To JSON")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Column to JSON")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Column to JSON.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDataTableColumnToJSONCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        public string v_DataColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_OutputJSONName))]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableColumnToJSONCommand()
        {
            //this.CommandName = "ConvertDataTableColumnToJSONCommand";
            //this.SelectionName = "Convert DataTable Column To JSON";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var listCommand = new ConvertDataTableColumnToListCommand
            {
                v_DataTableName = this.v_DataTableName,
                v_ColumnType = this.v_ColumnType,
                v_DataColumnIndex = this.v_DataColumnIndex,
                v_OutputVariableName = VariableNameControls.GetInnerVariableName(0, engine)
            };
            listCommand.RunCommand(engine);

            List<string> myList = (List<string>)VariableNameControls.GetInnerVariable(0, engine).VariableValue;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(myList);
            json.StoreInUserVariable(engine, v_OutputVariableName);
        }
    }
}