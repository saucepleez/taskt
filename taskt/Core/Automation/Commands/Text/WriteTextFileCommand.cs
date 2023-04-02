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
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "txt")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        [PropertyDescription("Text to be Written")]
        public string v_TextToWrite { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Overwrite Option")]
        [PropertyUISelectionOption("Append")]
        [PropertyUISelectionOption("Overwrite")]
        [PropertyIsOptional(true, "Overwrite")]
        public string v_Overwrite { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Replace [crLF] to Line Break")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [PropertyIsOptional(true, "No")]
        public string v_ReplaceToLineBreak { get; set; }

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
            //string filePath;
            //if (FilePathControls.ContainsFileCounter(v_FilePath, engine))
            //{
            //    filePath = FilePathControls.FormatFilePath_ContainsFileCounter(v_FilePath, engine, "txt", false);
            //}
            //else
            //{
            //    filePath = FilePathControls.FormatFilePath_NoFileCounter(v_FilePath, engine, "txt");
            //}
            var filePath = this.ConvertToUserVariableAsFilePath(nameof(v_FilePath), engine);

            //var outputText = v_TextToWrite.ConvertToUserVariable(sender).ToString().Replace("[crLF]",Environment.NewLine);
            var outputText = v_TextToWrite.ConvertToUserVariable(engine);
            if (this.GetUISelectionValue(nameof(v_ReplaceToLineBreak), engine) == "yes")
            {
                outputText = outputText.Replace("[crLF]", Environment.NewLine);
            }

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