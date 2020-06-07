using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.IO;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ExtractFileCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the file location (https:// is supported)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file or enter file URL.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.zip , [vFilePath] or https://temp.com/myfile.zip")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FilePathOrigin { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the extraction folder (ex. C:\\temp\\myzip\\")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file or enter file URL.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\ or [vFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_PathDestination { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Indicate the archive password")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("L.")]
        [Attributes.PropertyAttributes.SampleUsage(@"mypass or {vPass}")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Password { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Indicate the variable to receive a list of extracted file names")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        public ExtractFileCommand()
        {
            this.CommandName = "ExtractFileCommand";
            this.SelectionName = "Extract File";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //get absolute variable path or URL to source file
            var vSourceFile = v_FilePathOrigin.ConvertToUserVariable(sender);
            //track local file location
            string vLocalSourceFile = vSourceFile;
            //get file path to destination files
            var vExtractionFolder = v_PathDestination.ConvertToUserVariable(sender);
            //get optional password
            var vPassword = v_Password.ConvertToUserVariable(sender);
            //auto-detect extension
            var vFileType = Path.GetExtension(vSourceFile);
            //create tracking list
            var fileList = new List<string>();

            if (vSourceFile.StartsWith("http://") || vSourceFile.StartsWith("https://") || vSourceFile.StartsWith("www."))
            {
                //create temp directory
                var tempDir = Core.IO.Folders.GetFolder(Folders.FolderType.TempFolder);
                var tempFile = System.IO.Path.Combine(tempDir, $"{ Guid.NewGuid()}." + vFileType);

                //check if directory does not exist then create directory
                if (!System.IO.Directory.Exists(tempDir))
                {
                    System.IO.Directory.CreateDirectory(tempDir);
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
                uri = null;
                if (webclient != null)
                {
                    webclient.Dispose();
                    webclient = null;
                }
            }


            // Check if file exists before proceeding
            if (!System.IO.File.Exists(vLocalSourceFile))
            {
                throw new FileNotFoundException($"Could not find file: {vLocalSourceFile}");
            }
    
            // If the directory doesn't exist, create it.
            if (!Directory.Exists(vExtractionFolder))
                Directory.CreateDirectory(vExtractionFolder);

            try
            {
    
                using (Stream stream = File.OpenRead(vLocalSourceFile))
                {
                    IReader reader;

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
                            Console.WriteLine(reader.Entry.Key);
                            reader.WriteEntryToDirectory(vExtractionFolder, new ExtractionOptions() { ExtractFullPath = true, Overwrite = true });
                            fileList.Add(vExtractionFolder + reader.Entry.Key.Replace("/", "\\"));

                        }
                    }
                }

                if (!string.IsNullOrEmpty(v_applyToVariableName))
                {
                    engine.StoreComplexObjectInVariable(v_applyToVariableName, fileList);
                }


            }
            catch (Exception)
            {
                throw; 
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePathOrigin", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PathDestination", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Password", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Source File: '{v_FilePathOrigin}', Destination Folder: '{v_PathDestination}'";
        }
    }
}