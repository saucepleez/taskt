using System;
using System.Drawing;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using Tesseract;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Image")]
    [Attributes.ClassAttributes.CommandSettings("Execute Tesseract OCR")]
    [Attributes.ClassAttributes.Description("This command allows you to covert an image file into text for parsing.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert an image into text.  You can then use additional commands to parse the data.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command has a dependency on Tesseract OCR which has to be installed on the PC.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_camera))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExecuteTesseractOCRCommand : ScriptCommand, IFileExistsFilePathProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyDescription("Image File Path")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**C:\\temp\\myimages.png**", "File Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", "File Path")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "png,bmp,gif,jpg,jpeg")]
        [PropertyIsOptional(true, "")]
        [PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_TargetFilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Image variable")]
        [PropertyIsOptional(true, "")]
        public string v_ImageVar { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Result Text")]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyDescription("Tesseract Data Path, it must point to the locally installed Tesseract, tessdata folder")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**C:\\Program Fles\\Tesseract-OCR\\tessdata**", "Tessdata Folder Path")]
        [PropertyDetailSampleUsage("**{{{vFolderPath}}}**", "Tessdata Folder Path")]
        [PropertyIsOptional(true, @"C:\Program Files\Tesseract-OCR\tessdata")]
        [PropertyValidationRule("tessdata Folder Path", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_TessData { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitTimeForFile { get; set; }

        public ExecuteTesseractOCRCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            try
            {
                string tessDataPath = v_TessData.ExpandValueOrUserVariable(engine);
                if (tessDataPath == string.Empty) tessDataPath = @"C:\Program Files\Tesseract-OCR\tessdata";

                using (var tsengine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default))
                {
                    Pix img;

                    if (v_TargetFilePath != string.Empty)
                    {
                        //var imagePath = FilePathControls.WaitForFile(this, nameof(v_TargetFilePath), nameof(v_WaitTimeForFile), engine);
                        var imagePath = this.WaitForFile(engine);
                        img = Pix.LoadFromFile(imagePath);
                    }
                    else if (v_ImageVar != string.Empty)
                    {
                        var imagevar = ExtensionMethods.GetRawVariable(v_ImageVar, engine);
                        Bitmap bmp = (Bitmap)imagevar.VariableValue;
                        ImageConverter converter = new ImageConverter();
                        byte[] bytes = (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                        img = Pix.LoadFromMemory(bytes);
                    }
                    else
                    {
                        throw new Exception("Please specify at least one: imagepath or imagevar!");
                    }

                    using (var page = tsengine.Process(img))
                    {
                        string text = page.GetText();
                        text.StoreInUserVariable(engine, v_Result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("OCR Error. Message: " + ex.Message);
            }
        }
    }
}