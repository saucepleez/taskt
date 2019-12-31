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
    public class ModelTrainingCommand : ScriptCommand
    {
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Enter Model Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a name for your model")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ModelName { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Select Model Type")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("MultiClass Model")]    
        [Attributes.PropertyAttributes.InputSpecification("Select the appropriate corresponding model type")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Binary Model**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ModelType { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Training Data")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public DataTable v_ModelTrainingData { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Model Binary Data")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ModelBinaryData { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox ModelTypeDropdown;

        [XmlIgnore]
        [NonSerialized]
        private TextBox ModelBinaryData;
        public ModelTrainingCommand()
        {
            this.CommandName = "ModelTrainingCommand";
            this.SelectionName = "Train/Load Model";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_ModelName = "myModel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var modelName = v_ModelName.ConvertToUserVariable(sender);

            //train if not pre-trained
            if (string.IsNullOrEmpty(v_ModelBinaryData))
            {
                var trainingResponse = TrainModel();
                v_ModelBinaryData = trainingResponse.ModelData;
            }

            //load model
            var mlService = new ML.Service();
            var loadedModel = mlService.LoadModel(v_ModelBinaryData);

            //add model to loaded models
            engine.LoadedModels.Add(modelName, loadedModel);


        }

        private TrainingResponse TrainModel()
        {
            var mlService = new taskt.Core.ML.Service();

            var trainingRequest = new TrainingRequest();
            trainingRequest.ReturnFileData = true;

            foreach (DataRow rw in v_ModelTrainingData.Rows)
            {
                trainingRequest.Dataset.Add(new MultiClassInput()
                {
                    Statement = rw.Field<string>("Statement"),
                    Label = rw.Field<string>("Label")
                });
            }

            return mlService.Train(trainingRequest);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ModelName", this, editor));

            var modelTypeControls = CommandControls.CreateDefaultDropdownGroupFor("v_ModelType", this, editor);

            ModelTypeDropdown = modelTypeControls[1] as ComboBox;
            ModelTypeDropdown.SelectionChangeCommitted += ModelSelection_SelectionChangeCommitted;

            RenderedControls.AddRange(modelTypeControls);
            RenderedControls.AddRange(CommandControls.CreateDataGridViewGroupFor("v_ModelTrainingData", this, editor));

            var helperControl = CommandControls.CreateHelperControl("Load Sample Data");
            helperControl.Click += HelperControl_Click;
            RenderedControls.Add(helperControl);

            var trainModelControl = CommandControls.CreateHelperControl("Pre-Train Model");
            trainModelControl.Click += TrainModelControl_Click;
            RenderedControls.Add(trainModelControl);

            var modelBinaryDataControls = CommandControls.CreateDefaultInputGroupFor("v_ModelBinaryData", this, editor);
            ModelBinaryData = modelBinaryDataControls[1] as TextBox;
            RenderedControls.AddRange(modelBinaryDataControls);

            return RenderedControls;

        }

        private void TrainModelControl_Click(object sender, EventArgs e)
        {
            var trainingResult = TrainModel();
            MessageBox.Show($"Training Result: {trainingResult.Result}.", "Training Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ModelBinaryData.Text = trainingResult.ModelData;
        }

        private void HelperControl_Click(object sender, EventArgs e)
        {
            switch (ModelTypeDropdown.SelectedItem)
            {
                case "MultiClass Model":
                    v_ModelTrainingData.Rows.Clear();
                    v_ModelTrainingData.Rows.Add("I need to Reset Password", "#ITSupport");
                    v_ModelTrainingData.Rows.Add("I want to Reset Password", "#ITSupport");
                    v_ModelTrainingData.Rows.Add("Password Reset", "#ITSupport");
                    v_ModelTrainingData.Rows.Add("Reset My Password", "#ITSupport");
                    v_ModelTrainingData.Rows.Add("Pass Reset", "#ITSupport");
                    v_ModelTrainingData.Rows.Add("Pay Bill", "#Billing");
                    v_ModelTrainingData.Rows.Add("Pay Invoice", "#Billing");
                    v_ModelTrainingData.Rows.Add("Billing Problems", "#Billing");
                    v_ModelTrainingData.Rows.Add("Review Charges", "#Billing");
                    v_ModelTrainingData.Rows.Add("Pay Charges", "#Billing");
                    v_ModelTrainingData.Rows.Add("Invoice Problem", "#Billing");
                    break;
                default:
                    MessageBox.Show("You must select a model type to generate sample data!");
                    return;
            }
            
        }

        private void ModelSelection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //sender.SelectedItem
            var selectionBox = (ComboBox)sender;
            var selectedItem = selectionBox.SelectedItem;

            this.v_ModelTrainingData = new System.Data.DataTable
            {
                TableName = "ModelTrainingData" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            switch (selectionBox.SelectedItem)
            {
                case "MultiClass Model":
                    this.v_ModelTrainingData.Columns.Add("Statement");
                    this.v_ModelTrainingData.Columns.Add("Label");
                    break;
                default:
                    break;
            }
        }

        public override string GetDisplayValue()
        {
            if (string.IsNullOrEmpty(v_ModelBinaryData))
            {
                return base.GetDisplayValue() + $" [Train '{v_ModelName}' and Load at Runtime]";
            }
            else
            {
                return base.GetDisplayValue() + $" [Pre-Trained, Load '{v_ModelName}' at Runtime]";
            }
           
        }

    }
}
