using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("File")]
    [Attributes.ClassAttributes.CommandSettings("Write Text File")]
    [Attributes.ClassAttributes.Description("This command writes specified data to a text file")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to write data to text files.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WriteTextFileCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Path to the File")]
        //[InputSpecification("Path to the File", true)]
        //[Remarks("If file does not contain extensin, supplement txt automatically.\nIf file does not contain folder path, file will be saved in the same folder as script file.\nIf file path contains FileCounter variable, it will be replaced by a number that will become the name of a non-existent file.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[PropertyDetailSampleUsage("**C:\\temp\\myfile.txt**", PropertyDetailSampleUsage.ValueType.Value, "Path to the File")]
        //[PropertyDetailSampleUsage("**{{{vFile}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Path to the File")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Path")]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_FilePath))]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Text to be Written. [crLF] inserts a newline.")]
        //[InputSpecification("Text", true)]
        //[Remarks("")]
        //[PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Text")]
        //[PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        [PropertyDescription("Text to be Written")]
        public string v_TextToWrite { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Overwrite Option")]
        [InputSpecification("", true)]
        [PropertyUISelectionOption("Append")]
        [PropertyUISelectionOption("Overwrite")]
        [PropertyIsOptional(true, "Overwrite")]
        public string v_Overwrite { get; set; }

        public WriteTextFileCommand()
        {
            //this.CommandName = "WriteTextFileCommand";
            //this.SelectionName = "Write Text File";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //convert variables
            string filePath;
            if (FilePathControls.containsFileCounter(v_FilePath, engine))
            {
                filePath = FilePathControls.formatFilePath_ContainsFileCounter(v_FilePath, engine, "txt", false);
            }
            else
            {
                filePath = FilePathControls.formatFilePath_NoFileCounter(v_FilePath, engine, "txt");
            }
            
            var outputText = v_TextToWrite.ConvertToUserVariable(sender).ToString().Replace("[crLF]",Environment.NewLine);

            var isOverwrite = this.GetUISelectionValue(nameof(v_Overwrite), engine);
            //append or overwrite as necessary
            if (isOverwrite == "append")
            {
                System.IO.File.AppendAllText(filePath, outputText);
            }
            else
            {
                System.IO.File.WriteAllText(filePath, outputText);
            }
        }
    }
}