using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Image Commands")]
    [Description("This command allows you to covert an image file into text for parsing.")]
    [UsesDescription("Use this command when you want to convert an image into text.  You can then use additional commands to parse the data.")]
    [ImplementationDescription("This command has a dependency on and implements OneNote OCR to achieve automation.")]
    public class PerformOCRCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Select Image to OCR")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        [InputSpecification("Enter or Select the path to the image file.")]
        [SampleUsage(@"**c:\temp\myimages.png")]
        [Remarks("")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Apply OCR Result To Variable")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }

        public PerformOCRCommand()
        {
            DefaultPause = 0;
            CommandName = "PerformOCRCommand";
            SelectionName = "Perform OCR";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var ocrEngine = new OneNoteOCRDll.OneNoteOCR();
            var arr = ocrEngine.OcrTexts(v_FilePath.ConvertToUserVariable(engine)).ToArray();

            string endResult = "";
            foreach (var text in arr)
            {
                endResult += text.Text;
            }

            //apply to user variable
            endResult.StoreInUserVariable(engine, v_userVariableName);
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return "OCR '" + v_FilePath + "' and apply result to '" + v_userVariableName + "'";
        }
    }
}