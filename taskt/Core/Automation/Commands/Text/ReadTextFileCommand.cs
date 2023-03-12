using System;
using System.Xml.Serialization;
using System.Net;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("File")]
    [Attributes.ClassAttributes.CommandSettings("Read Text File")]
    [Attributes.ClassAttributes.Description("This command allows you to read text file into a variable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to read data from text files.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ReadTextFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Path to the File")]
        [InputSpecification("Path of the File", true)]
        [Remarks("If file does not contain extensin, supplement txt automatically.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyDetailSampleUsage("**C:\\temp\\myfile.txt**", PropertyDetailSampleUsage.ValueType.Value, "Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Path")]
        [PropertyDetailSampleUsage("**http://exmample.com/mytext.txt**", PropertyDetailSampleUsage.ValueType.Value, "Path")]
        [PropertyDetailSampleUsage("**{{{vURL}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Path")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Path")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Read Type")]
        [PropertyUISelectionOption("Content")]
        [PropertyUISelectionOption("Line Count")]
        [PropertyIsOptional(true, "Content")]
        [PropertyFirstValue("Content")]
        public string v_ReadOption { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify Variable the text should be stored")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vTextFile** or **{{{vTextFile}}}**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_userVariableName { get; set; }


        public ReadTextFileCommand()
        {
            //this.CommandName = "ReadTextFileCommand";
            //this.SelectionName = "Read Text File";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_ReadOption = "Content";
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
                webClient.Headers.Add("user-agent", "request");
                result = webClient.DownloadString(v_FilePath.ConvertToUserVariable(engine));
            }

            //var readPreference = v_ReadOption.GetUISelectionValue("v_ReadOption", this, engine);
            var readPreference = this.GetUISelectionValue(nameof(v_ReadOption), engine);
            if (readPreference == "line count")
            {
                result = result.Replace("\r\n", "\n");  // \n\n -> \n
                result = result.Split(new char[] { '\n', '\r' }).Length.ToString();
            }

            //assign text to user variable
            result.StoreInUserVariable(engine, v_userVariableName);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Read: '" + v_ReadOption + "', File: '" + v_FilePath + "', Store: '" + v_userVariableName + "']";
        //}
    }
}