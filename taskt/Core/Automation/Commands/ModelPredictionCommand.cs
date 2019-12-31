using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.ML.Models;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("ML Commands (Beta)")]
    [Attributes.ClassAttributes.Description("This command allows you to show a message to the user.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to present or display a value on screen to the user.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    public class ModelPredictionCommand : ScriptCommand
    {
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Enter Model Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a name for your model")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ModelName { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the input to test")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Input { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the extracted json")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Prediction Verbosity")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Label Only")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Verbose")]
        [Attributes.PropertyAttributes.InputSpecification("Select the appropriate corresponding verbosity")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_PredictionVerbosity { get; set; }

        public ModelPredictionCommand()
        {
            this.CommandName = "ModelPredictionCommand";
            this.SelectionName = "Model Prediction";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_ModelName = "myModel";
            this.v_PredictionVerbosity = "Label Only";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var modelName = v_ModelName.ConvertToUserVariable(sender);
            var input = v_Input.ConvertToUserVariable(sender);

            var requiredModel = engine.LoadedModels[modelName];
    
            //load model
            var mlService = new ML.Service();
            var scoringRequest = new ScoringRequest()
            {
                Input = input,
                Model = requiredModel
            };

            var output = mlService.Predict(scoringRequest);

            var verbosity = v_PredictionVerbosity.ConvertToUserVariable(sender);
            if (verbosity == "Verbose")
            {
                var verboseOutput = Newtonsoft.Json.JsonConvert.SerializeObject(output);
                verboseOutput.StoreInUserVariable(sender, v_applyToVariableName);
            }
            else
            {
                output.Prediction.StoreInUserVariable(sender, v_applyToVariableName);
            }

        }

       
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ModelName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Input", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_applyToVariableName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_PredictionVerbosity", this, editor));
            return RenderedControls;

        }

       

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Predict '{v_Input}' against Model '{v_ModelName}']";
                      
        }

    }
}
