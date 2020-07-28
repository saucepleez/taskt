using Newtonsoft.Json.Linq;
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
    [Description("This command parses a JSON array into a list.")]
    public class ParseJSONArrayCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("JSON Array")]
        [InputSpecification("Provide a variable or JSON array value.")]
        [SampleUsage("[{\"rect\":{\"length\":10, \"width\":5}}] || {vArrayVariable}")]
        [Remarks("Providing data of a type other than a 'JSON Array' will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_JsonArrayName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output List Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                  " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public ParseJSONArrayCommand()
        {
            CommandName = "ParseJSONArrayCommand";
            SelectionName = "Parse JSON Array";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //get variablized input
            var variableInput = v_JsonArrayName.ConvertToUserVariable(engine);

            //create objects
            JArray arr;
            List<string> resultList = new List<string>();

            //parse json
            try
            {
                arr = JArray.Parse(variableInput);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
            }
 
            //add results to result list since list<string> is supported
            foreach (var result in arr)
            {
                resultList.Add(result.ToString());
            }

            //get variable
            var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_OutputUserVariableName).FirstOrDefault();

            //create if var does not exist
            if (requiredComplexVariable == null)
            {
                engine.VariableList.Add(
                    new ScriptVariable() 
                    { 
                        VariableName = v_OutputUserVariableName, 
                        CurrentPosition = 0 
                    });
                requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_OutputUserVariableName).FirstOrDefault();
            }

            //assign value to variable
            requiredComplexVariable.VariableValue = resultList;
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_JsonArrayName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Parse '{v_JsonArrayName}' - Store List in '{v_OutputUserVariableName}']";
        }
    }
}