using Microsoft.Office.Interop.Outlook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command returns an item (having a specific index) from a list.")]
    public class GetListItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("List")]
        [InputSpecification("Provide a list variable.")]
        [SampleUsage("{vList}")]
        [Remarks("Any type of variable other than list will cause error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Index")]
        [InputSpecification("Specify a valid list item index.")]
        [SampleUsage("0 || {vIndex}")]
        [Remarks("'0' is the index of the first item in a list. Providing an invalid or out-of-bounds index will result in an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ItemIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output List Item Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                  " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public GetListItemCommand()
        {
            CommandName = "GetListItemCommand";
            SelectionName = "Get List Item";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var itemIndex = v_ItemIndex.ConvertToUserVariable(sender);
            int index = int.Parse(itemIndex);
            //get variable by regular name
            ScriptVariable listVariable = LookupVariable(engine);

            //if still null then throw exception
            if (listVariable == null)
            {
                throw new System.Exception(
                    "Complex Variable '" + v_ListName + "' or '" + 
                    v_ListName.ApplyVariableFormatting() + 
                    "' not found. Ensure the variable exists before attempting to modify it."
                    );
            }

            dynamic listToIndex;
            if (listVariable.VariableValue is List<string>)
            {
                listToIndex = (List<string>)listVariable.VariableValue;
            }
            else if (listVariable.VariableValue is List<MailItem>)
            {
                listToIndex = (List<MailItem>)listVariable.VariableValue;
            }
            else if (listVariable.VariableValue is List<IWebElement>)
            {
                listToIndex = (List<IWebElement>)listVariable.VariableValue;
            }
            else if (
                (listVariable.VariableValue.ToString().StartsWith("[")) && 
                (listVariable.VariableValue.ToString().EndsWith("]")) && 
                (listVariable.VariableValue.ToString().Contains(","))
                )
            {
                //automatically handle if user has given a json array
                JArray jsonArray = JsonConvert.DeserializeObject(listVariable.VariableValue.ToString()) as JArray;

                var itemList = new List<string>();
                foreach (var jsonItem in jsonArray)
                {
                    var value = (JValue)jsonItem;
                    itemList.Add(value.ToString());
                }

                listVariable.VariableValue = itemList;
                listToIndex = itemList;
            }
            else
            {
                throw new System.Exception("Complex Variable List Type<T> Not Supported");
            }

            var item = listToIndex[index];

            ScriptVariable newListItem = new ScriptVariable
            {
                VariableName = v_OutputUserVariableName,
                VariableValue = item
            };

            //Overwrites variable if it already exists
            if (engine.VariableList.Exists(x => x.VariableName == newListItem.VariableName))
            {
                ScriptVariable temp = engine.VariableList.Where(x => x.VariableName == newListItem.VariableName).FirstOrDefault();
                engine.VariableList.Remove(temp);
            }

            engine.VariableList.Add(newListItem);
        }
        
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ItemIndex", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From Index '{v_ItemIndex}' of '{v_ListName}' - Store List Item in '{v_OutputUserVariableName}']";
        }

        private ScriptVariable LookupVariable(AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_ListName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) &&
                (v_ListName.StartsWith(sendingInstance.EngineSettings.VariableStartMarker)) &&
                (v_ListName.EndsWith(sendingInstance.EngineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_ListName
                    .Replace(sendingInstance.EngineSettings.VariableStartMarker, "")
                    .Replace(sendingInstance.EngineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
    }
}