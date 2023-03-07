using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Item")]
    [Attributes.ClassAttributes.CommandSettings("Get List Item")]
    [Attributes.ClassAttributes.Description("This command allows you to get an item from a List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get an item from a List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetListItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ListIndex))]
        public string v_ItemIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_UserVariableName { get; set; }

        public GetListItemCommand()
        {
            //this.CommandName = "GetListItemCommand";
            //this.SelectionName = "Get List Item";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var listVariable = v_ListName.GetRawVariable(engine);
            //if (listVariable == null)
            //{
            //    throw new Exception("Complex Variable '" + v_ListName + "' or '" + v_ListName.ApplyVariableFormatting(engine) + "' not found. Ensure the variable exists before attempting to modify it.");
            //}

            //dynamic listToIndex;
            //var varType = listVariable.VariableValue.GetType();
            //if (listVariable.VariableValue is List<string> list_string)
            //{
            //    listToIndex = list_string;
            //}
            //else if (varType.IsGenericType && (varType.GetGenericTypeDefinition() == typeof(List<>)))
            //{
            //    listToIndex = listVariable.VariableValue;
            //}
            //else
            //{
            //    var listValue = listVariable.VariableValue;
            //    if ((listValue is string) &&
            //            (listValue.ToString().StartsWith("[") && listValue.ToString().EndsWith("]") && listValue.ToString().Contains(",")))
            //    {
            //        Newtonsoft.Json.Linq.JArray jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(listVariable.VariableValue.ToString()) as Newtonsoft.Json.Linq.JArray;

            //        var itemList = new List<string>();
            //        foreach (var jsonItem in jsonArray)
            //        {
            //            var value = (Newtonsoft.Json.Linq.JValue)jsonItem;
            //            itemList.Add(value.ToString());
            //        }
            //        listToIndex = itemList;
            //    }
            //    else
            //    {
            //        throw new Exception(v_ListName + " is not List");
            //    }
            //}

            //int index;
            //if (String.IsNullOrEmpty(v_ItemIndex))
            //{
            //    index = listVariable.CurrentPosition;
            //}
            //else
            //{
            //    index = v_ItemIndex.ConvertToUserVariableAsInteger("Index", engine);
            //}

            //if (index < 0)
            //{
            //    index = listToIndex.Count + index;
            //}

            //if ((index >= 0) && (index < listToIndex.Count))
            //{
            //    if (listToIndex is List<string>)
            //    {
            //        ((string)listToIndex[index]).StoreInUserVariable(engine, v_UserVariableName);
            //    }
            //    else
            //    {
            //        // set new variable
            //        "".StoreInUserVariable(engine, v_UserVariableName);
            //        var targetVariable = v_UserVariableName.GetRawVariable(engine);
            //        targetVariable.VariableValue = listToIndex[index];
            //    }
            //}
            //else
            //{
            //    throw new Exception("Strange index " + v_ItemIndex + ", parsed " + index);
            //}

            //var list = v_ListName.GetListVariable(engine);
            //int index;
            //if (String.IsNullOrEmpty(v_ItemIndex))
            //{
            //    var rawVariable = v_ListName.GetRawVariable(engine);
            //    index = rawVariable.CurrentPosition;
            //}
            //else
            //{
            //    index = this.ConvertToUserVariableAsInteger(nameof(v_ItemIndex), engine);
            //}

            (var list, var index) = this.GetListVariableAndIndex(nameof(v_ListName), nameof(v_ItemIndex), engine);

            if (index < 0)
            {
                index += list.Count;
            }

            if ((index >= 0) && (index < list.Count))
            {
                list[index].StoreInUserVariable(engine, v_UserVariableName);
            }
            else
            {
                throw new Exception("Strange index " + v_ItemIndex + ", parsed " + index);
            }
        }
    }
}