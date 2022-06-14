using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("File")]
    [Attributes.ClassAttributes.Description("This command writes specified data to a text file")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to write data to text files.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class WriteTextFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the path to the file")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the text file.")]
        [SampleUsage("**C:\\temp\\myfile.txt** or **{{{vTextFilePath}}}**")]
        [Remarks("If file does not contain extensin, supplement txt automatically.\nIf file does not contain folder path, file will be saved in the same folder as script file.\nIf file path contains FileCounter variable, it will be replaced by a number that will become the name of a non-existent file.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the text to be written. [crLF] inserts a newline.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Indicate the text should be written to files.")]
        [SampleUsage("**{{{vText}}}** or **Hello World!**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        public string v_TextToWrite { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select overwrite option")]
        [InputSpecification("Indicate whether this command should append the text to or overwrite all existing text in the file")]
        [SampleUsage("Select from **Append** or **Overwrite**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Append")]
        [PropertyUISelectionOption("Overwrite")]
        [PropertyIsOptional(true, "Overwrite")]
        public string v_Overwrite { get; set; }
        public WriteTextFileCommand()
        {
            this.CommandName = "WriteTextFileCommand";
            this.SelectionName = "Write Text File";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            Engine.AutomationEngineInstance engine = (Engine.AutomationEngineInstance)sender;

            //convert variables
            string filePath;
            //if (FilePathControls.containsFileCounter(v_FilePath, engine))
            //{
            //    filePath = FilePathControls.formatFileCounter_NotExists(v_FilePath, engine, ".txt");
            //}
            //else
            //{
            //    filePath = v_FilePath.ConvertToUserVariable(sender);
            //    filePath = FilePathControls.formatFilePath(filePath, (Engine.AutomationEngineInstance)sender);
            //    if (!FilePathControls.hasExtension(filePath))
            //    {
            //        filePath += ".txt";
            //    }
            //}
            //filePath = FilePathControls.formatFilePath_ContainsFileCounter(v_FilePath, engine, "txt", false);

            if (FilePathControls.containsFileCounter(v_FilePath, engine))
            {
                filePath = FilePathControls.formatFilePath_ContainsFileCounter(v_FilePath, engine, "txt", false);
            }
            else
            {
                filePath = FilePathControls.formatFilePath_NoFileCounter(v_FilePath, engine, "txt");
            }
            
            var outputText = v_TextToWrite.ConvertToUserVariable(sender).ToString().Replace("[crLF]",Environment.NewLine);

            var overwrite = v_Overwrite.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(overwrite))
            {
                overwrite = "Overwrite";
            }

            //append or overwrite as necessary
            if (overwrite == "Append")
            {
                System.IO.File.AppendAllText(filePath, outputText);
            }
            else
            {
                System.IO.File.WriteAllText(filePath, outputText);
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToWrite", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_Overwrite", this, editor));

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }


        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_Overwrite + " to '" + v_FilePath + "']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_FilePath))
        //    {
        //        this.validationResult += "File is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}