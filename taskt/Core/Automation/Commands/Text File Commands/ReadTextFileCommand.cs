using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Text File Commands")]
    [Description("This command reads text data from a text file and stores it in a variable.")]
    public class ReadTextFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("File Path")]
        [InputSpecification("Enter or Select the path to the text file.")]
        [SampleUsage(@"C:\temp\myfile.txt || {ProjectPath}\myText.txt || {vTextFilePath}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Text Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not " +
                 "required to pre-define your variables, however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public ReadTextFileCommand()
        {
            CommandName = "ReadTextFileCommand";
            SelectionName = "Read Text File";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //convert variables
            var filePath = v_FilePath.ConvertToUserVariable(sender);
            //read text from file
            var textFromFile = File.ReadAllText(filePath);
            //assign text to user variable
            textFromFile.StoreInUserVariable(sender, v_OutputUserVariableName);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(
                CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor)
            );

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Read Text From '{v_FilePath}' - Store Text in '{v_OutputUserVariableName}']";
        }
    }
}