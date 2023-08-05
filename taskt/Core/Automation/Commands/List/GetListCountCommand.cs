using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Item")]
    [Attributes.ClassAttributes.CommandSettings("Get List Count")]
    [Attributes.ClassAttributes.Description("This command allows you to get the item count of a List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get the item count of a List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetListCountCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_UserVariableName { get; set; }

        public GetListCountCommand()
        {
            //this.CommandName = "GetListCountCommand";
            //this.SelectionName = "Get List Count";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            
            var listVariable = v_ListName.GetRawVariable(engine);
            dynamic listToCount;

            Type listType = listVariable.VariableValue.GetType();
            if (listType.IsGenericType && (listType.GetGenericTypeDefinition() == typeof(List<>)))
            {
                // List<T>
                listToCount = listVariable.VariableValue;
            }
            else
            {
                if ((listVariable.VariableValue is string) &&
                        listVariable.VariableValue.ToString().StartsWith("[") && listVariable.VariableValue.ToString().EndsWith("]") && listVariable.VariableValue.ToString().Contains(","))
                {
                    // JSON Array
                    Newtonsoft.Json.Linq.JArray jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(listVariable.VariableValue.ToString()) as Newtonsoft.Json.Linq.JArray;

                    var itemList = new List<string>();
                    foreach (var item in jsonArray)
                    {
                        var value = (Newtonsoft.Json.Linq.JValue)item;
                        itemList.Add(value.ToString());
                    }

                    listVariable.VariableValue = itemList;
                    listToCount = itemList;
                }
                else
                {
                    throw new Exception(v_ListName + " is not List");
                }
            }

            string count = listToCount.Count.ToString();
            count.StoreInUserVariable(sender, v_UserVariableName);
        }
    }
}