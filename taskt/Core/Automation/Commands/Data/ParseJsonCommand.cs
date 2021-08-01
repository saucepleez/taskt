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
    [Attributes.ClassAttributes.SubGruop("JSON")]
    [Attributes.ClassAttributes.Description("This command allows you to parse a JSON object into a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract data from a JSON object")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ParseJsonCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the JSON text or variable requiring extraction")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or text value")]
        [Attributes.PropertyAttributes.SampleUsage("**{\"id\":2}** or **{{{vSomeVariable}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify a JSON extractor")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input a JSON token extractor")]
        [Attributes.PropertyAttributes.SampleUsage("**$.id**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the extracted JSON")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_applyToVariableName { get; set; }

        public ParseJsonCommand()
        {
            this.CommandName = "ParseJsonCommand";
            this.SelectionName = "Parse JSON Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;
          
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;


            var forbiddenMarkers = new List<string> { "[", "]" };

            if (forbiddenMarkers.Any(f => f == engine.engineSettings.VariableStartMarker) || (forbiddenMarkers.Any(f => f == engine.engineSettings.VariableEndMarker)))
            {
                throw new Exception("Cannot use Parse JSON command with square bracket variable markers [ ]");
            }

            //get variablized input
            var variableInput = v_InputValue.ConvertToUserVariable(sender);

            //get variablized token
            var jsonSearchToken = v_JsonExtractor.ConvertToUserVariable(sender);

            //create objects
            Newtonsoft.Json.Linq.JObject o;
            IEnumerable<Newtonsoft.Json.Linq.JToken> searchResults;
            List<string> resultList = new List<string>();

            //parse json
            try
            {
                 o = Newtonsoft.Json.Linq.JObject.Parse(variableInput);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured Parsing Tokens: " + ex.ToString());
            }
 

            //select results
            try
            {
                searchResults = o.SelectTokens(jsonSearchToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
            }
        

            //add results to result list since list<string> is supported
            foreach (var result in searchResults)
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
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_JsonExtractor", this, editor));

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Selector: " + v_JsonExtractor + ", Apply Result(s) To Variable: " + v_applyToVariableName + "]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InputValue))
            {
                this.validationResult += "JSON text is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_JsonExtractor))
            {
                this.validationResult += "JSON extractor is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_applyToVariableName))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}