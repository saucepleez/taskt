using System;
using System.Xml.Serialization;
using System.Net;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text")]
    [Attributes.ClassAttributes.SubGruop("File")]
    [Attributes.ClassAttributes.CommandSettings("Read Text File")]
    [Attributes.ClassAttributes.Description("This command allows you to read text file into a variable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to read data from text files.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ReadTextFileCommand : ScriptCommand, IFileExistsFilePathProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathAndURL))]
        [PropertyFilePathSetting(true, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "txt,log,json")]
        public string v_TargetFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Read Type")]
        [PropertyUISelectionOption("Content")]
        [PropertyUISelectionOption("Line Count")]
        [PropertyIsOptional(true, "Content")]
        [PropertyFirstValue("Content")]
        public string v_ReadOption { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitTimeForFile { get; set; }

        public ReadTextFileCommand()
        {
            //this.CommandName = "ReadTextFileCommand";
            //this.SelectionName = "Read Text File";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_ReadOption = "Content";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //bool isURL = FilePathControls.IsURL(v_FilePath.ConvertToUserVariable(engine));

            //string result;
            //if (!isURL)
            //{
            //    string filePath;
            //    if (FilePathControls.ContainsFileCounter(v_FilePath, engine))
            //    {
            //        filePath = FilePathControls.FormatFilePath_ContainsFileCounter(v_FilePath, engine, "txt", true);
            //    }
            //    else
            //    {
            //        filePath = FilePathControls.FormatFilePath_NoFileCounter(v_FilePath, engine, "txt", true, true);
            //    }
            //    result = System.IO.File.ReadAllText(filePath);
            //}
            //else
            //{
            //    WebClient webClient = new WebClient();
            //    webClient.Encoding = System.Text.Encoding.UTF8;
            //    webClient.Headers.Add("user-agent", "request");
            //    result = webClient.DownloadString(v_FilePath.ConvertToUserVariable(engine));
            //}

            //var filePath = FilePathControls.WaitForFile(this, nameof(v_TargetFilePath), nameof(v_WaitTimeForFile), engine);
            var filePath = this.WaitForFile(engine);
            string result;
            if (EM_CanHandleFilePathExtentionMethods.IsURL(filePath))
            {
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                webClient.Headers.Add("user-agent", "request");
                result = webClient.DownloadString(v_TargetFilePath.ExpandValueOrUserVariable(engine));
            }
            else
            {
                result = System.IO.File.ReadAllText(filePath);
            }

            //var readPreference = v_ReadOption.GetUISelectionValue("v_ReadOption", this, engine);
            var readPreference = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ReadOption), engine);
            if (readPreference == "line count")
            {
                result = result.Replace("\r\n", "\n");  // \n\n -> \n
                result = result.Split(new char[] { '\n', '\r' }).Length.ToString();
            }

            //assign text to user variable
            result.StoreInUserVariable(engine, v_userVariableName);
        }
    }
}