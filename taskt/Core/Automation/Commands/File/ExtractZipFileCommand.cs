using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Extract Zip File")]
    [Attributes.ClassAttributes.Description("This command extracts files from a compressed file")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExtractZipFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(true, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "zip")]
        public string v_FilePathOrigin { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        [PropertyDescription("Extraction Folder")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        //[InputSpecification("Enter or Select the path to the applicable file or enter file URL.")]
        //[SampleUsage(@"**C:\temp\** or **{{{vFilePath}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Extract Folder")]
        public string v_PathDestination { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyVirtualProperty(nameof(SelectionControls), nameof(SelectionControls.v_YesNoComboBox))]
        [PropertyDescription("Create Folder When Destination Does not Exist")]
        //[PropertyUISelectionOption("Yes")]
        //[PropertyUISelectionOption("No")]
        [PropertyIsOptional(true, "No")]
        public string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Archive Password")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter archive files password.")]
        //[SampleUsage(@"**mypass** or {{{vPass}}}")]
        [PropertyDetailSampleUsage("**mypass**", PropertyDetailSampleUsage.ValueType.Value, "Password")]
        [PropertyDetailSampleUsage("**{{{vPass}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Password")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        public string v_Password { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        [PropertyDescription("List Variable Name to Receive a List of Extracted File Names")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_applyToVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        public ExtractZipFileCommand()
        {
            //this.CommandName = "ExtractFileCommand";
            //this.SelectionName = "Extract File";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //get absolute variable path or URL to source file
            var vSourceFile = this.ConvertToUserVariableAsFilePath(nameof(v_FilePathOrigin), engine);

            //track local file location
            string vLocalSourceFile = vSourceFile;

            //auto-detect extension
            var vFileType = Path.GetExtension(vSourceFile);

            if (FilePathControls.IsURL(vSourceFile))
            {
                //create temp directory
                var tempDir = Folders.GetFolder(Folders.FolderType.TempFolder);
                var tempFile = Path.Combine(tempDir, $"{ Guid.NewGuid()}." + vFileType);

                //check if directory does not exist then create directory
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                // Create webClient to download the file for extraction
                var webclient = new System.Net.WebClient();
                var uri = new Uri(vSourceFile);
                webclient.DownloadFile(uri, tempFile);

                // check if file is downloaded successfully
                if (File.Exists(tempFile))
                {
                    //override source file location
                    vLocalSourceFile = tempFile;
                }

                // Free not needed resources
                webclient?.Dispose();
            }

            //get file path to destination files
            //var vExtractionFolder = v_PathDestination.ConvertToUserVariable(engine);
            var vExtractionFolder = v_PathDestination.ConvertToUserVariableAsFolderPath(engine);

            // If the directory doesn't exist, create it.
            if (!Directory.Exists(vExtractionFolder))
            {
                //var isCreateDirectory = this.GetUISelectionValue(nameof(v_CreateDirectory), engine);

                //if (isCreateDirectory == "yes")
                //{
                //    Directory.CreateDirectory(vExtractionFolder);
                //}
                if (this.GetYesNoSelectionValue(nameof(v_CreateDirectory), engine))
                {
                    Directory.CreateDirectory(vExtractionFolder);
                }
            }
            
            try
            {
                //create tracking list
                var fileList = new List<string>();

                using (Stream stream = File.OpenRead(vLocalSourceFile))
                {
                    IReader reader;

                    //get optional password
                    var vPassword = v_Password.ConvertToUserVariable(engine);

                    //check if password is needed
                    if (string.IsNullOrEmpty(vPassword))
                    {
                        //password not required
                        reader = ReaderFactory.Open(stream);
                    }
                    else
                    {
                        //password required
                        reader = ReaderFactory.Open(stream, new ReaderOptions() { Password = vPassword });
                    }
           
                    while (reader.MoveToNextEntry())
                    {
                        if (!reader.Entry.IsDirectory)
                        {
                            //Console.WriteLine(reader.Entry.Key);
                            reader.WriteEntryToDirectory(vExtractionFolder, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                            fileList.Add(vExtractionFolder + reader.Entry.Key.Replace("/", "\\"));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(v_applyToVariableName))
                {
                    fileList.StoreInUserVariable(engine, v_applyToVariableName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Zip Extraction Error. Message: " + ex.Message); 
            }
        }
    }
}