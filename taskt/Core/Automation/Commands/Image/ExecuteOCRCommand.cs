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
        //[PropertyDescription("Select Image to OCR")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[InputSpecification("Enter or Select the path to the image file.")]
        //[SampleUsage("**c:\\temp\\myimages.png** or **{{{vFileName}}}**")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyDescription("Image File Path")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**C:\\temp\\myimages.png**", "File Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", "File Path")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "png,bmp,gif,jpg,jpeg")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Apply OCR Result To Variable")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyIsVariablesList(true)]
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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));

        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return "OCR '" + v_FilePath + "' and apply result to '" + v_userVariableName + "'";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(v_FilePath))
        //    {
        //        this.validationResult += "File path is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(v_userVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}