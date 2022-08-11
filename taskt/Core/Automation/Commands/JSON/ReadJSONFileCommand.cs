using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.Description("This command reads JSON data into a variable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to read data from JSON files.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class ReadJSONFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the text file.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myfile.txt** or **{{{vTextFilePath}}}** or **http://example.com/api/** or **{{{vURL}}}**")]
        [Attributes.PropertyAttributes.Remarks("If file does not contain extensin, supplement txt automatically.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define where the JSON should be stored")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.JSON, true)]
        public string v_userVariableName { get; set; }

        public ReadJSONFileCommand()
        {
            this.CommandName = "ReadJSONFileCommand";
            this.SelectionName = "Read JSON File";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            string filePath;
            if (!FilePathControls.isURL(v_FilePath))
            {
                 filePath = FilePathControls.formatFilePath_NoFileCounter(v_FilePath, engine, "json", true);
            }
            else
            {
                filePath = v_FilePath.ConvertToUserVariable(engine);
            }

            ScriptCommand readFile = new ReadTextFileCommand
            {
                v_FilePath = filePath,
                v_userVariableName = this.v_userVariableName
            };
            readFile.RunCommand(sender);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Read JSON file " + v_FilePath + " Result '" + v_userVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_FilePath))
            {
                this.validationResult += "File is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_userVariableName))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}