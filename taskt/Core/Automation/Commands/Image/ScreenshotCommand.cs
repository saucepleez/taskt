using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Image Commands")]
    [Attributes.ClassAttributes.Description("This command takes a screenshot and saves it to a location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to take and save a screenshot.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements User32 CaptureWindow to achieve automation")]
    public class ScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to take a screenshot of.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **Desktop** or **{{{vWindow}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsWindowNamesList(true, true, false, true)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_ScreenshotWindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to save the image")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.SampleUsage("**c:\\Temp\\image.png** or **{{{vPath}}}**")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.Remarks("If file does not contain extensin, suppliment png extension.\nIf file does not contain folder path, file will be saved in the same folder as script file.\nIf file path contains FileCounter variable, it will be replaced by a number that will become the name of a non-existent file.")]
        public string v_FilePath { get; set; }
        public ScreenshotCommand()
        {
            this.CommandName = "ScreenshotCommand";
            this.SelectionName = "Take Screenshot";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            Engine.AutomationEngineInstance engine = (Engine.AutomationEngineInstance)sender;

            var targetWindowName = v_ScreenshotWindowName.ConvertToUserVariable(sender);
            if (targetWindowName == ((Engine.AutomationEngineInstance)sender).engineSettings.CurrentWindowKeyword)
            {
                targetWindowName = User32Functions.GetActiveWindowTitle();
            }

            var image = User32Functions.CaptureWindow(targetWindowName);

            string outputFile;
            //if (Core.FilePathControls.containsFileCounter(v_FilePath, engine))
            //{
            //     outputFile= Core.FilePathControls.formatFileCounter_NotExists(v_FilePath, engine, ".png");
            //}
            //else
            //{
            //    outputFile = v_FilePath.ConvertToUserVariable(sender);
            //    outputFile = Core.FilePathControls.formatFilePath(outputFile, (Engine.AutomationEngineInstance)sender);
            //    if (!Core.FilePathControls.hasExtension(outputFile))
            //    {
            //        outputFile += ".png";
            //    }
            //}
            if (FilePathControls.containsFileCounter(v_FilePath, engine))
            {
                outputFile = FilePathControls.formatFilePath_ContainsFileCounter(v_FilePath, engine, "png");
            }
            else
            {
                outputFile = FilePathControls.formatFilePath_NoFileCounter(v_FilePath, engine, "png");
            }

            image.Save(outputFile);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrl = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrl);

            //create window name helper control
            //RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_ScreenshotWindowName", this));
            //var WindowNameControl = UI.CustomControls.CommandControls.CreateStandardComboboxFor("v_ScreenshotWindowName", this).AddWindowNames(editor);
            //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_ScreenshotWindowName", this, new Control[] { WindowNameControl }, editor));
            //RenderedControls.Add(WindowNameControl);

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));


            return RenderedControls;
        }


        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: '" + v_ScreenshotWindowName + "', File Path: '" + v_FilePath + "]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);
            
            if (String.IsNullOrEmpty(v_ScreenshotWindowName))
            {
                this.validationResult += "Window name is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(v_FilePath))
            {
                this.validationResult += "File path is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}