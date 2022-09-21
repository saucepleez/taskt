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
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command extracts files from a compressed file")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExtractFileCommand : ScriptCommand
    {

        [XmlAttribute]
        [PropertyDescription("Please enter the file path or location")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the applicable file or enter file URL.")]
        [SampleUsage(@"**C:\temp\myfile.zip** , **{{{vFilePath}}}** or **https://temp.com/myfile.zip**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File")]
        public string v_FilePathOrigin { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the extraction folder")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [InputSpecification("Enter or Select the path to the applicable file or enter file URL.")]
        [SampleUsage(@"**C:\temp\** or **{{{vFilePath}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Folder", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Extract Folder")]
        public string v_PathDestination { get; set; }

        [XmlAttribute]
        [PropertyDescription("Create folder if destination does not exist")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the directory should be created if it does not already exist.")]
        [SampleUsage("Select **Yes** or **No**")]
        [Remarks("")]
        [PropertyIsOptional(true, "No")]
        public string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Indicate the archive password")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter archive files password.")]
        [SampleUsage(@"**mypass** or {{{vPass}}}")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true)]
        public string v_Password { get; set; }

        [XmlAttribute]
        [PropertyDescription("Indicate the variable to receive a list of extracted file names")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyIsOptional(true)]
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
            var engine = (Engine.AutomationEngineInstance)sender;

            //get absolute variable path or URL to source file
            var vSourceFile = v_FilePathOrigin.ConvertToUserVariable(sender);
            //track local file location
            string vLocalSourceFile = vSourceFile;
            //get file path to destination files
            var vExtractionFolder = v_PathDestination.ConvertToUserVariable(sender);
            var vCreateDirectory = v_CreateDirectory.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(vCreateDirectory))
            {
                vCreateDirectory = "No";
            }
            //get optional password
            var vPassword = v_Password.ConvertToUserVariable(sender);
            //auto-detect extension
            var vFileType = Path.GetExtension(vSourceFile);
            //create tracking list
            var fileList = new List<string>();

            if (vSourceFile.StartsWith("http://") || vSourceFile.StartsWith("https://") || vSourceFile.StartsWith("www."))
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
                uri = null;
                if (webclient != null)
                {
                    webclient.Dispose();
                    webclient = null;
                }
            }


            // Check if file exists before proceeding
            if (!File.Exists(vLocalSourceFile))
            {
                throw new FileNotFoundException($"Could not find file: {vLocalSourceFile}");
            }
    
            // If the directory doesn't exist, create it.
            if (!Directory.Exists(vExtractionFolder))
            {
                if (vCreateDirectory.ToLower() == "yes")
                {
                    Directory.CreateDirectory(vExtractionFolder);
                }
                else
                {
                    throw new Exception("No extraction folder: " + vExtractionFolder);
                }
            }
            
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
                            //Console.WriteLine(reader.Entry.Key);
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
        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePathOrigin", this, editor));
            
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PathDestination", this, editor));

        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_CreateDirectory", this, editor));
            
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Password", this, editor));

        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
        //    RenderedControls.Add(VariableNameControl);

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + $" [Source File: '{v_FilePathOrigin}', Destination Folder: '{v_PathDestination}'";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_FilePathOrigin))
        //    {
        //        this.validationResult += "File path is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_PathDestination))
        //    {
        //        this.validationResult += "Extraction folder is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}