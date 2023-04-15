using System;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Image Commands")]
    [Attributes.ClassAttributes.CommandSettings("Execute OCR")]
    [Attributes.ClassAttributes.Description("This command allows you to covert an image file into text for parsing.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert an image into text.  You can then use additional commands to parse the data.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command has a dependency on and implements OneNote OCR to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExecuteOCRCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyDescription("Image File Path")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**C:\\temp\\myimages.png**", "File Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", "File Path")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "png,bmp,gif,jpg,jpeg")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitForFile { get; set; }

        public ExecuteOCRCommand()
        {
            //this.DefaultPause = 0;
            //this.CommandName = "OCRCommand";
            //this.SelectionName = "Perform OCR";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var filePath = FilePathControls.WaitForFile(this, nameof(v_FilePath), nameof(v_WaitForFile), engine);

            try
            {
                var ocrEngine = new OneNoteOCRDll.OneNoteOCR();
                var arr = ocrEngine.OcrTexts(filePath).ToArray();

                string endResult = "";
                foreach (var text in arr)
                {
                    endResult += text.Text;
                }

                //apply to user variable
                endResult.StoreInUserVariable(engine, v_userVariableName);
            }
            catch (Exception ex)
            {
                throw new Exception("OCR Error. Message: " + ex.Message);
            }
        }
    }
}