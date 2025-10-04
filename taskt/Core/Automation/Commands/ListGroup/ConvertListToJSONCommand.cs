using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("Convert")]
    [Attributes.ClassAttributes.CommandSettings("Convert List To JSON")]
    [Attributes.ClassAttributes.Description("This command convert a JSON array to a list.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ConvertListToJSONCommand : AListGetFromListCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        //public string v_List { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_OutputJSONName))]
        public override string v_Result { get; set; }

        public ConvertListToJSONCommand()
        {
            //this.CommandName = "ConvertListToJSONCommand";
            //this.SelectionName = "Convert List To JSON";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //List<string> targetList = v_List.ExpandUserVariableAsList(engine);
            var targetList = this.ExpandUserVariableAsList(engine);

            // convert json
            try
            {
                var convertedList = Newtonsoft.Json.JsonConvert.SerializeObject(targetList);
                convertedList.StoreInUserVariable(engine, v_Result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Fail Convert to Json. Message: {ex}");
            }
        }
    }
}
