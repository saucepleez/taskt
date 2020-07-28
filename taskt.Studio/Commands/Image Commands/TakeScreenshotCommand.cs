using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Image Commands")]
    [Description("This command takes a screenshot and saves it to a location")]
    [UsesDescription("Use this command when you want to take and save a screenshot.")]
    [ImplementationDescription("This command implements User32 CaptureWindow to achieve automation")]
    public class TakeScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the Window name")]
        [InputSpecification("Input or Type the name of the window that you want to take a screenshot of.")]
        [SampleUsage("**Untitled - Notepad**")]
        [Remarks("")]
        public string v_ScreenshotWindowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate the path to save the image")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }

        public TakeScreenshotCommand()
        {
            CommandName = "TakeScreenshotCommand";
            SelectionName = "Take Screenshot";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            string vFilePath = v_FilePath.ConvertToUserVariable(engine);
            Bitmap image;

            if (v_ScreenshotWindowName == "Current Window")
            {
                image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                using (Graphics g = Graphics.FromImage(image))
                {
                    g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                }
            }
            else
            {
                image = User32Functions.CaptureWindow(v_ScreenshotWindowName);
            }

            image.Save(vFilePath);
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_ScreenshotWindowName", this));
            var WindowNameControl = CommandControls.CreateStandardComboboxFor("v_ScreenshotWindowName", this).AddWindowNames();
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_ScreenshotWindowName", this, new Control[] { WindowNameControl }, editor));
            RenderedControls.Add(WindowNameControl);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));


            return RenderedControls;
        }


        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: '" + v_ScreenshotWindowName + "', File Path: '" + v_FilePath + "]";
        }
    }
}