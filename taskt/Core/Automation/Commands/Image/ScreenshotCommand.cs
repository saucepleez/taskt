using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Image Commands")]
    [Attributes.ClassAttributes.CommandSettings("Take Screenshot")]
    [Attributes.ClassAttributes.Description("This command takes a screenshot and saves it to a location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to take and save a screenshot.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements User32 CaptureWindow to achieve automation")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the Window name")]
        //[InputSpecification("Input or Type the name of the window that you want to take a screenshot of.")]
        //[SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **Desktop** or **{{{vWindow}}}**")]
        //[Remarks("")]
        //[PropertyIsWindowNamesList(true, true, false, true)]
        //[PropertyShowSampleUsageInDescription(true)]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        [PropertyIsWindowNamesList(true, true, false, true)]
        public string v_ScreenshotWindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please indicate the path to save the image")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[SampleUsage("**c:\\Temp\\image.png** or **{{{vPath}}}**")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyDescription("Image File Path")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**C:\\temp\\myimages.png**", "File Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", "File Path")]
        [Remarks("If file does not contain extensin, suppliment png extension.\nIf file does not contain folder path, file will be saved in the same folder as script file.\nIf file path contains FileCounter variable, it will be replaced by a number that will become the name of a non-existent file.")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.FirstNotExists)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_MatchMethod_Single))]
        [PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        public string v_MatchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_TargetWindowIndex))]
        public string v_TargetWindowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WaitTime))]
        public string v_WaitForWindow { get; set; }

        public ScreenshotCommand()
        {
            //this.CommandName = "ScreenshotCommand";
            //this.SelectionName = "Take Screenshot";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var targetWindowName = v_ScreenshotWindowName.ConvertToUserVariable(sender);
            //if (targetWindowName == ((Engine.AutomationEngineInstance)sender).engineSettings.CurrentWindowKeyword)
            //{
            //    targetWindowName = User32Functions.GetActiveWindowTitle();
            //}

            string targetWindowName;
            if (v_ScreenshotWindowName == "Desktop")
            {
                targetWindowName = "Desktop";
            }
            else
            {
                var targetWindowHandles = WindowNameControls.FindWindows(this, nameof(v_ScreenshotWindowName), nameof(v_SearchMethod), nameof(v_MatchMethod), nameof(v_TargetWindowIndex), nameof(v_WaitForWindow), engine);
                targetWindowName = WindowNameControls.GetWindowNameFromHandle(targetWindowHandles[0]);
            }
            
            var image = User32Functions.CaptureWindow(targetWindowName);

            var outputFile = this.ConvertToUserVariableAsFilePath(nameof(v_FilePath), engine);

            image.Save(outputFile);
        }
        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            ComboBox cmb = (ComboBox)ControlsList[nameof(v_ScreenshotWindowName)];
            cmb.AddWindowNames();
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrl = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrl);

        //    //create window name helper control
        //    //RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_ScreenshotWindowName", this));
        //    //var WindowNameControl = UI.CustomControls.CommandControls.CreateStandardComboboxFor("v_ScreenshotWindowName", this).AddWindowNames(editor);
        //    //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_ScreenshotWindowName", this, new Control[] { WindowNameControl }, editor));
        //    //RenderedControls.Add(WindowNameControl);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));


        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Target Window: '" + v_ScreenshotWindowName + "', File Path: '" + v_FilePath + "]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(v_ScreenshotWindowName))
        //    {
        //        this.validationResult += "Window name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(v_FilePath))
        //    {
        //        this.validationResult += "File path is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}