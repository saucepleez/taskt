using System;
using System.Drawing;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Image")]
    [Attributes.ClassAttributes.CommandSettings("Take Screenshot")]
    [Attributes.ClassAttributes.Description("This command takes a screenshot and saves it to a location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to take and save a screenshot.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements User32 CaptureWindow to achieve automation")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_camera))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class TakeScreenshotCommand : AOneWindowNameActionCommands, ICanHandleFilePath
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowName))]
        //[PropertyIsWindowNamesList(true, true, false, true)]
        //[PropertyAvailableSystemVariable(Engine.SystemVariables.LimitedSystemVariableNames.Window_Desktop)]
        //public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyDescription("Image File Path")]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**C:\\temp\\myimages.png**", "File Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", "File Path")]
        [Remarks("If file does not contain extensin, suppliment png extension.\nIf file does not contain folder path, file will be saved in the same folder as script file.\nIf file path contains FileCounter variable, it will be replaced by a number that will become the name of a non-existent file.")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.FirstNotExists)]
        [PropertyParameterOrder(5100)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Activate Window Before Capture")]
        [PropertyIsOptional(true, "No")]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Activate Window")]
        [PropertyParameterOrder(5200)]
        public string v_ActivateWindowBeforeCapture { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Wait Time before Capture")]
        [PropertyIsOptional(true, "500")]
        [PropertyValidationRule("Before Capture", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyFirstValue("500")]
        [PropertyDisplayText(false, "Wait Time before Capture")]
        [PropertyParameterOrder(5300)]
        public string v_WaitTimeBeforeCapture { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_CompareMethod))]
        //public string v_CompareMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_MatchMethod_Single))]
        //[PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        //public string v_MatchMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_TargetWindowIndex))]
        //public string v_TargetWindowIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        //public string v_WaitTimeForWindow { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowNameResult))]
        //public string v_NameResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        //public string v_HandleResult { get; set; }

        public TakeScreenshotCommand()
        {
            //this.CommandName = "ScreenshotCommand";
            //this.SelectionName = "Take Screenshot";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //string targetWindowName;
            //if (v_WindowName == "Desktop")
            //{
            //    targetWindowName = "Desktop";
            //}
            //else
            //{
            //    var wins = WindowNameControls.FindWindows(this, nameof(v_WindowName), nameof(v_SearchMethod), nameof(v_MatchMethod), nameof(v_TargetWindowIndex), nameof(v_WaitForWindow), engine);
            //    targetWindowName = wins[0].Item2;
            //}

            ////var image = User32Functions.CaptureWindow(targetWindowName);
            //var image = WindowNameControls.CaptureWindow(targetWindowName, engine);

            //var outputFile = this.ExpandValueOrUserVariableAsFilePath(nameof(v_FilePath), engine);

            //image.Save(outputFile);

            //WindowControls.WindowAction(this, engine,
            //    new Action<List<(IntPtr, string)>>(wins =>
            //    {
            //        var whnd = wins[0].Item1;

            //        if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ActivateWindowBeforeCapture), engine))
            //        {
            //            WindowControls.ActivateWindow(whnd);
            //        }

            //        // wait time
            //        var waitTime = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTimeBeforeCapture), engine);
            //        System.Threading.Thread.Sleep(waitTime);

            //        var image = CaptureWindow(whnd);
            //        //var outputFile = this.ExpandValueOrUserVariableAsFilePath(nameof(v_FilePath), engine);
            //        // MEMO: now, this.ExpandValueOrUserVariableAsFilePath has 2 methods
            //        var outputFile = EM_CanHandleFilePathExtentionMethods.ExpandValueOrUserVariableAsFilePath(this, nameof(v_FilePath), engine);

            //        image.Save(outputFile);
            //    })
            //);

            this.WindowNameActionAndWait(engine, new Action<IntPtr, string>((whnd, name) =>
            {
                if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ActivateWindowBeforeCapture), engine))
                {
                    var activate = new ActivateWindowByWindowHandleCommand()
                    {
                        v_WindowHandle = whnd.ToString(),
                    };
                    activate.RunCommand(engine);
                }

                // wait time
                var waitTime = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTimeBeforeCapture), engine);
                System.Threading.Thread.Sleep(waitTime);

                var image = CaptureWindow(whnd);

                // MEMO: now, this.ExpandValueOrUserVariableAsFilePath has 2 methods
                var outputFile = EM_CanHandleFilePathExtentionMethods.ExpandValueOrUserVariableAsFilePath(this, nameof(v_FilePath), engine);

                image.Save(outputFile);
            }));
        }

        private static Bitmap CaptureWindow(IntPtr whnd)
        {
            var r = EM_WindowRECTPropertiesExtentionMethods.GetWindowRect(whnd);
            var bounds = new Rectangle(r.left, r.top, r.right - r.left, r.bottom - r.top);
            var screenshot = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(screenshot))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return screenshot;
        }

        //private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    WindowControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        //}

        //public override void Refresh(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    //ComboBox cmb = (ComboBox)ControlsList[nameof(v_ScreenshotWindowName)];
        //    //cmb.AddWindowNames();
        //    ControlsList.GetPropertyControl<ComboBox>(nameof(v_WindowName)).AddWindowNames();
        //}
    }
}