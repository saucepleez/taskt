using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.IO;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("File Operation Commands")]
    [Description("This command extracts file(s) from a file having specific format.")]
    public class ExtractFilesCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Source File Format")]
        [PropertyUISelectionOption("zip")]
        [PropertyUISelectionOption("7z")]
        [PropertyUISelectionOption("xz")]
        [PropertyUISelectionOption("bzip2")]
        [PropertyUISelectionOption("tar")]
        [PropertyUISelectionOption("wim")]
        [PropertyUISelectionOption("iso")]
        [InputSpecification("Select source file format.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FileType { get; set; }

        [XmlAttribute]
        [PropertyDescription("File Source Type")]
        [PropertyUISelectionOption("File Path")]
        [PropertyUISelectionOption("File URL")]
        [InputSpecification("Select file source type.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FileSourceType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Source File Path / URL")]
        [InputSpecification("Enter or Select the Path / URL to the applicable file.")]
        [SampleUsage(@"C:\temp\myfile.zip || {ProjectPath}\myfile.zip || https://temp.com/myfile.zip || {vFileSourcePath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePathOrigin { get; set; }

        [XmlAttribute]
        [PropertyDescription("Extracted File(s) Directory Path")]
        [InputSpecification("Enter or Select the Folder Path / URL to move extracted file(s) to.")]
        [SampleUsage(@"C:\temp || {ProjectPath}\temp || {vFilesPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_PathDestination { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Extracted File Path(s) List Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public ExtractFilesCommand()
        {
            CommandName = "ExtractFilesCommand";
            SelectionName = "Extract Files";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get variable path or URL to source file
            var vSourceFilePathOrigin = v_FilePathOrigin.ConvertToUserVariable(sender);

            // get file path to destination files
            var vFilePathDestination = v_PathDestination.ConvertToUserVariable(sender);

            if (v_FileSourceType == "File URL")
            {
                //create temp directory
                var tempDir = Folders.GetFolder(Folders.FolderType.TempFolder);
                var tempFile = Path.Combine(tempDir, $"{ Guid.NewGuid()}." + v_FileType);

                //check if directory does not exist then create directory
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                // Create webClient to download the file for extraction
                var webclient = new WebClient();
                var uri = new Uri(vSourceFilePathOrigin);
                webclient.DownloadFile(uri, tempFile);

                // check if file is downloaded successfully
                if (File.Exists(tempFile))
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
            if (!File.Exists(vSourceFilePathOrigin))
                throw new FileNotFoundException("Could not find file: " + vSourceFilePathOrigin);

            // Get 7Z app
            var zPath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources", "7z.exe");

            // If the directory doesn't exist, create it.
            if (!Directory.Exists(vFilePathDestination))
                Directory.CreateDirectory(vFilePathDestination);

            var result = "";
            Process process = new Process();

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

                result.StoreInUserVariable(sender, v_OutputUserVariableName);
            }
            catch (Exception Ex)
            {
                process.Kill();
                throw Ex;
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FileType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FileSourceType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePathOrigin", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PathDestination", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Extract From '{v_FilePathOrigin}' to '{v_PathDestination}' - " +
                $"Store Extracted File Path(s) List in '{v_OutputUserVariableName}']";
        }
    }
}