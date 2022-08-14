using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Net;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("File")]
    [Attributes.ClassAttributes.Description("This command allows you to read text file into a variable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to read data from text files.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class ReadTextFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the path to the file")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the text file.")]
        [SampleUsage("**C:\\temp\\myfile.txt** or **{{{vTextFilePath}}}** or **http://example.com/mytext.txt** or **{{{vURL}}}**")]
        [Remarks("If file does not contain extensin, supplement txt automatically.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the read type")]
        [InputSpecification("Select the appropriate window state required")]
        [SampleUsage("**Content** or **Line Count**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Content")]
        [PropertyUISelectionOption("Line Count")]
        [PropertyIsOptional(true, "Content")]
        public string v_ReadOption { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Variable the text should be stored")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vTextFile** or **{{{vTextFile}}}**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_userVariableName { get; set; }


        public ReadTextFileCommand()
        {
            this.CommandName = "ReadTextFileCommand";
            this.SelectionName = "Read Text File";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_ReadOption = "Content";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            bool isURL = FilePathControls.isURL(v_FilePath.ConvertToUserVariable(engine));

            string result;
            if (!isURL)
            {
                string filePath;
                if (FilePathControls.containsFileCounter(v_FilePath, engine))
                {
                    filePath = FilePathControls.formatFilePath_ContainsFileCounter(v_FilePath, engine, "txt", true);
                }
                else
                {
                    filePath = FilePathControls.formatFilePath_NoFileCounter(v_FilePath, engine, "txt", true, true);
                }
                result = System.IO.File.ReadAllText(filePath);
            }
            else
            {
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                result = webClient.DownloadString(v_FilePath.ConvertToUserVariable(engine));
            }

            var readPreference = v_ReadOption.GetUISelectionValue("v_ReadOption", this, engine);
            if (readPreference == "line count")
            {
                result = result.Replace("\r\n", "\n");  // \n\n -> \n
                result = result.Split(new char[] { '\n', '\r' }).Length.ToString();
            }

            //assign text to user variable
            result.StoreInUserVariable(sender, v_userVariableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Read: '" + v_ReadOption + "', File: '" + v_FilePath + "', Store: '" + v_userVariableName + "']";
        }
    }
}