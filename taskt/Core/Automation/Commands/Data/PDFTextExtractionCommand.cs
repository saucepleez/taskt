using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.IO;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.Description("")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class PDFTextExtractionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the PDF file path or PDF file URL")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file or enter file URL.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myfile.pdf** or **{{{vFilePath}}}* or **https://temp.com/myfile.pdf**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select source type of PDF file (default is File Path)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("File Path")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("File URL")]
        [Attributes.PropertyAttributes.InputSpecification("Select source type of PDF file")]
        [Attributes.PropertyAttributes.SampleUsage("Select **File Path**, **File URL**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_FileSourceType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the PDF text")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_applyToVariableName { get; set; }

        public PDFTextExtractionCommand()
        {
            this.CommandName = "PDFTextExtractionCommand";
            this.SelectionName = "PDF Extraction";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {

            //get variable path or URL to source file
            var vSourceFilePath = v_FilePath.ConvertToUserVariable(sender);
            // get source type of file either from a physical file or from a URL
            var vSourceFileType = v_FileSourceType.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(vSourceFileType))
            {
                vSourceFileType = "FilePath";
            }

            if (vSourceFileType == "File URL")
            {
                //create temp directory
                var tempDir = Core.IO.Folders.GetFolder(Folders.FolderType.TempFolder);
                var tempFile = System.IO.Path.Combine(tempDir, $"{ Guid.NewGuid()}.pdf");

                //check if directory does not exist then create directory
                if (!System.IO.Directory.Exists(tempDir))
                {
                    System.IO.Directory.CreateDirectory(tempDir);
                }

                // Create webClient to download the file for extraction
                var webclient = new System.Net.WebClient();
                var uri = new Uri(vSourceFilePath);
                webclient.DownloadFile(uri, tempFile);

                // check if file is downloaded successfully
                if (System.IO.File.Exists(tempFile))
                {
                    vSourceFilePath = tempFile;
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
            if (!System.IO.File.Exists(vSourceFilePath))
            {
                throw new System.IO.FileNotFoundException("Could not find file: " + vSourceFilePath);
            }

            //create process interface
            JavaInterface javaInterface = new JavaInterface();

            //get output from process
            var result = javaInterface.ExtractPDFText(vSourceFilePath);

            //apply to variable
            result.StoreInUserVariable(sender, v_applyToVariableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_FileSourceType", this, editor));

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Extract From '" + v_FilePath + "' and apply result to '" + v_applyToVariableName + "'" ;
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_FilePath))
            {
                this.validationResult += "File path, url is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_applyToVariableName))
            {
                this.validationResult += "Varialbe is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}