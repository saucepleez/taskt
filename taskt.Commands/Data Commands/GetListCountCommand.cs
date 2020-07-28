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
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command returns the count of items contained in a list.")]
    public class GetListCountCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("List")]
        [InputSpecification("Provide a list variable.")]
        [SampleUsage("{vList}")]
        [Remarks("Providing any type of variable other than a list will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Count Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                  " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public GetListCountCommand()
        {
            CommandName = "GetListCountCommand";
            SelectionName = "Get List Count";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
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

            dynamic listToCount; 
            if (listVariable.VariableValue is List<string>)
            {
                listToCount = (List<string>)listVariable.VariableValue;
            }
            else if (listVariable.VariableValue is List<MailItem>)
            {
                listToCount = (List<MailItem>)listVariable.VariableValue;
            }
            else if (listVariable.VariableValue is List<IWebElement>)
            {
                listToCount = (List<IWebElement>)listVariable.VariableValue;
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
                foreach (var item in jsonArray)
                {
                    var value = (JValue)item;
                    itemList.Add(value.ToString());
                }

                listVariable.VariableValue = itemList;
                listToCount = itemList;
            }
            else
            {
                throw new System.Exception("Complex Variable List Type<T> Not Supported");
            }

            string count = listToCount.Count.ToString();
            count.StoreInUserVariable(engine, v_OutputUserVariableName);
        }
        
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_ListName}' - Store Count in '{v_OutputUserVariableName}']";
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