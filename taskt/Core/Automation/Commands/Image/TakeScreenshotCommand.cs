using System;
using System.Windows.Forms;
using System.Xml.Serialization;
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
    public class TakeScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        [PropertyIsWindowNamesList(true, true, false, true)]
        public string v_ScreenshotWindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyDescription("Image File Path")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**C:\\temp\\myimages.png**", "File Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", "File Path")]
        [Remarks("If file does not contain extensin, suppliment png extension.\nIf file does not contain folder path, file will be saved in the same folder as script file.\nIf file path contains FileCounter variable, it will be replaced by a number that will become the name of a non-existent file.")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.FirstNotExists)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

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

        public TakeScreenshotCommand()
        {
            //this.CommandName = "ScreenshotCommand";
            //this.SelectionName = "Take Screenshot";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string targetWindowName;
            if (v_ScreenshotWindowName == "Desktop")
            {
                targetWindowName = "Desktop";
            }
            else
            {
                var wins = WindowNameControls.FindWindows(this, nameof(v_ScreenshotWindowName), nameof(v_SearchMethod), nameof(v_MatchMethod), nameof(v_TargetWindowIndex), nameof(v_WaitForWindow), engine);
                targetWindowName = wins[0].Item2;
            }

            //var image = User32Functions.CaptureWindow(targetWindowName);
            var image = WindowNameControls.CaptureWindow(targetWindowName, engine);

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
            //ComboBox cmb = (ComboBox)ControlsList[nameof(v_ScreenshotWindowName)];
            //cmb.AddWindowNames();
            ControlsList.GetPropertyControl<ComboBox>(nameof(v_ScreenshotWindowName)).AddWindowNames();
        }
    }
}