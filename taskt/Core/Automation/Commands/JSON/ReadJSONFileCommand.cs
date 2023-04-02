using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("File")]
    [Attributes.ClassAttributes.CommandSettings("Read JSON File")]
    [Attributes.ClassAttributes.Description("This command reads JSON data into a variable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to read data from JSON files.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ReadJSONFileCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Path to the File")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[InputSpecification("Enter or Select the path to the text file.")]
        //[SampleUsage("**C:\\temp\\myfile.txt** or **{{{vTextFilePath}}}** or **http://example.com/api/** or **{{{vURL}}}**")]
        //[Remarks("If file does not contain extensin, supplement txt automatically.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Path")]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**C:\\temp\\myfile.json**", PropertyDetailSampleUsage.ValueType.Value, "File Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Path")]
        [PropertyDetailSampleUsage("**http://example.com/api/**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Path")]
        [PropertyDetailSampleUsage("**{{{vURL}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Path")]
        [PropertyFilePathSetting(true, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "json,txt")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_OutputJSONName))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitForFile { get; set; }

        public ReadJSONFileCommand()
        {
            //this.CommandName = "ReadJSONFileCommand";
            //this.SelectionName = "Read JSON File";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //string filePath;
            //if (!FilePathControls.IsURL(v_FilePath))
            //{
            //     filePath = FilePathControls.FormatFilePath_NoFileCounter(v_FilePath, engine, "json", true);
            //}
            //else
            //{
            //    filePath = v_FilePath.ConvertToUserVariable(engine);
            //}
            string filePath = FilePathControls.WaitForFile(this, nameof(v_FilePath), nameof(v_WaitForFile), engine);

            ScriptCommand readFile = new ReadTextFileCommand
            {
                v_FilePath = filePath,
                v_userVariableName = this.v_userVariableName
            };
            readFile.RunCommand(sender);
        }
    }
}