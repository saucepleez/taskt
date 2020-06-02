using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to parse a JSON Array into a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract data from a JSON object")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ParseJSONArrayCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the JSON Array or Variable (ex. {someVariable})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or json array value")]
        [Attributes.PropertyAttributes.SampleUsage("**[{obj1},{obj2}]** or **{vArrayVariable}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the list")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        public ParseJSONArrayCommand()
        {
            this.CommandName = "ParseJSONArrayCommand";
            this.SelectionName = "Parse JSON Array";
            this.CommandEnabled = true;
            this.CustomRendering = true;
          
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;


            //get variablized input
            var variableInput = v_InputValue.ConvertToUserVariable(sender);


            //create objects
            Newtonsoft.Json.Linq.JArray arr;
            List<string> resultList = new List<string>();

            //parse json
            try
            {
                arr = Newtonsoft.Json.Linq.JArray.Parse(variableInput);
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
            var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();

            //create if var does not exist
            if (requiredComplexVariable == null)
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_applyToVariableName, CurrentPosition = 0 });
                requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();
            }

            //assign value to variable
            requiredComplexVariable.VariableValue = resultList;

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));


            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;

        }



        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Result(s) To List Variable: " + v_applyToVariableName + "]";
        }
    }
}