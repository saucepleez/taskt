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
    public class FileExtractionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select file type")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("zip")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("7z")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("xz")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("bzip2")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("tar")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("wim")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("iso")]
        [Attributes.PropertyAttributes.InputSpecification("Select source file type")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Type file**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FileType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the file path or file URL to be extract")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file or enter file URL.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.zip , [vFilePath] or https://temp.com/myfile.zip")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FilePathOrigin { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select source file")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("File Path")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("File URL")]
        [Attributes.PropertyAttributes.InputSpecification("Select source path")]
        [Attributes.PropertyAttributes.SampleUsage("Select **File Path**, **File URL**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FileSourceType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the file path to be send after extract")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file or enter file URL.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\ or [vFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_PathDestination { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the list of extract files")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        public FileExtractionCommand()
        {
            this.CommandName = "FileExtractionCommand";
            this.SelectionName = "File Extraction";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            // get  type of file either from a physical file or from a URL
            var vFileType = v_FileType.ConvertToUserVariable(sender);
            //get variable path or URL to source file
            var vSourceFilePathOrigin = v_FilePathOrigin.ConvertToUserVariable(sender);
            // get source type of file either from a physical file or from a URL
            var vSourceFileType = v_FileSourceType.ConvertToUserVariable(sender);
            // get file path to destination files
            var vFilePathDestination = v_PathDestination.ConvertToUserVariable(sender);
            // get file path to destination files
            var v_VariableName = v_applyToVariableName.ConvertToUserVariable(sender);

            if (vSourceFileType == "File URL")
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
                var uri = new Uri(vSourceFilePathOrigin);
                webclient.DownloadFile(uri, tempFile);

                // check if file is downloaded successfully
                if (System.IO.File.Exists(tempFile))
                {
                    vSourceFilePathOrigin = tempFile;
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
            if (!System.IO.File.Exists(vSourceFilePathOrigin))
            {
                throw new System.IO.FileNotFoundException("Could not find file: " + vSourceFilePathOrigin);
            }

            // Get 7Z app
            var zPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Resources", "7z.exe");

            // If the directory doesn't exist, create it.
            if (!Directory.Exists(vFilePathDestination))
                Directory.CreateDirectory(vFilePathDestination);

            var result = "";
            System.Diagnostics.Process process = new System.Diagnostics.Process();

            try
            {
                var temp = Guid.NewGuid();
                //Extract in temp to get list files and directories and delete
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.UseShellExecute = false;
                pro.FileName = zPath;
                pro.RedirectStandardOutput = true;
                pro.Arguments = "x " + vSourceFilePathOrigin + " -o" + vFilePathDestination + "/" + temp + " -aoa";
                process.StartInfo = pro;
                process.Start();
                process.WaitForExit();
                string[] dirPaths = Directory.GetDirectories(vFilePathDestination + "/" + temp, "*", SearchOption.TopDirectoryOnly);
                string[] filePaths = Directory.GetFiles(vFilePathDestination + "/" + temp, "*", SearchOption.TopDirectoryOnly);

                foreach (var item in dirPaths)
                {
                    result = result + item + Environment.NewLine;
                }
                foreach (var item in filePaths)
                {
                    result = result + item + Environment.NewLine;
                }
                result = result.Replace("/" + temp, "");
                Directory.Delete(vFilePathDestination + "/" + temp, true);

                //Extract 
                pro.Arguments = "x " + vSourceFilePathOrigin + " -o" + vFilePathDestination + " -aoa";
                process.StartInfo = pro;
                process.Start();
                process.WaitForExit();


                result.StoreInUserVariable(sender, v_applyToVariableName);
            }
            catch (System.Exception Ex)
            {
                process.Kill();
                v_applyToVariableName = Ex.Message;
            }







        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FileType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePathOrigin", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FileSourceType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PathDestination", this, editor));
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Extract From '" + v_FilePathOrigin + " to " + v_PathDestination + "' and apply result to '" + v_applyToVariableName + "'";
        }
    }
}