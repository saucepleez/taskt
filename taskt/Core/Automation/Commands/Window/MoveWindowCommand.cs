using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.SubGruop("Window Actions")]
    [Attributes.ClassAttributes.Description("This command moves a window to a specified location on screen.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move an existing window by name to a certain point on the screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class MoveWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDescription("Please enter or select the window that you want to move.")]
        [InputSpecification("Input or Type the name of the window that you want to move.")]
        [SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindow}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsWindowNamesList(true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [PropertyDescription("Window title search method")]
        [InputSpecification("")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyUISelectionOption("Exact match")]
        [SampleUsage("**Contains** or **Starts with** or **Ends with** or **Exact match**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Contains")]
        public string v_SearchMethod { get; set; }
        [XmlAttribute]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDescription("Please indicate the new X horizontal coordinate (pixel) for the window's location.  0 starts at the left of the screen.")]
        [InputSpecification("Input the new horizontal coordinate of the window, 0 starts at the left and goes to the right")]
        [SampleUsage("**0** or **{{{vXPos}}}** or **%kwd_current_position%**")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("X Position", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_XWindowPosition { get; set; }
        [XmlAttribute]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDescription("Please indicate the new Y vertical coordinate (pixel) for the window's location.  0 starts at the top of the screen.")]
        [InputSpecification("Input the new vertical coordinate of the window, 0 starts at the top and goes downwards")]
        [SampleUsage("**0** or **{{{vYPos}}}** or **%kwd_current_position%**")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Y Position", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_YWindowPosition { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;


        public MoveWindowCommand()
        {
            this.CommandName = "MoveWindowCommand";
            this.SelectionName = "Move Window";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //var engine = (Engine.AutomationEngineInstance)sender;
            //string windowName = v_WindowName.ConvertToUserVariable(sender);

            //string searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(searchMethod))
            //{
            //    searchMethod = "Contains";
            //}

            //bool targetIsCurrentWindow = ((Engine.AutomationEngineInstance)sender).engineSettings.CurrentWindowKeyword == windowName;

            //var targetWindows = User32Functions.FindTargetWindows(windowName, targetIsCurrentWindow, (searchMethod != "Contains"));

            //var variableXPosition = v_XWindowPosition.ConvertToUserVariable(sender);
            //var variableYPosition = v_YWindowPosition.ConvertToUserVariable(sender);

            //var settings = engine.engineSettings;

            //int xPos, yPos;
            //if (variableXPosition == settings.CurrentWindowPositionKeyword || variableXPosition == settings.CurrentWindowXPositionKeyword ||
            //    variableXPosition == settings.CurrentWindowYPositionKeyword)
            //{
            //    xPos = 0;
            //}
            //else if (!int.TryParse(variableXPosition, out xPos))
            //{
            //    throw new Exception("X Position Invalid - " + v_XWindowPosition);
            //}

            //if (variableYPosition == settings.CurrentWindowPositionKeyword || variableYPosition == settings.CurrentWindowXPositionKeyword ||
            //    variableYPosition == settings.CurrentWindowYPositionKeyword)
            //{
            //    yPos = 0;
            //}
            //else if (!int.TryParse(variableYPosition, out yPos))
            //{
            //    throw new Exception("X Position Invalid - " + v_XWindowPosition);
            //}

            //if (searchMethod == "Contains" || targetIsCurrentWindow)
            //{
            //    loop each window
            //    foreach (var targetedWindow in targetWindows)
            //    {
            //        User32Functions.SetWindowPosition(targetedWindow, xPos, yPos);
            //        MoveWindow(targetedWindow, xPos, yPos, variableXPosition, variableYPosition, settings);
            //    }
            //}
            //else
            //{
            //    Func<string, bool> searchFunc;
            //    switch (searchMethod)
            //    {
            //        case "Starts with":
            //            searchFunc = (s) => s.StartsWith(windowName);
            //            break;

            //        case "Ends with":
            //            searchFunc = (s) => s.EndsWith(windowName);
            //            break;

            //        case "Exact match":
            //            searchFunc = (s) => (s == windowName);
            //            break;

            //        default:
            //            throw new Exception("Search method " + searchMethod + " is not support.");
            //            break;
            //    }

            //    bool isMoveWindow = false;
            //    loop each window
            //    foreach (var targetedWindow in targetWindows)
            //    {
            //        if (searchFunc(User32Functions.GetWindowTitle(targetedWindow)))
            //        {
            //            User32Functions.SetWindowPosition(targetedWindow, xPos, yPos);
            //            MoveWindow(targetedWindow, xPos, yPos, variableXPosition, variableYPosition, settings);
            //            isMoveWindow = true;
            //        }
            //    }
            //    if (!isMoveWindow)
            //    {
            //        throw new Exception("Window name '" + windowName + "' is not found.Search method " + searchMethod + ".");
            //    }
            //}

            var engine = (Engine.AutomationEngineInstance)sender;

            string windowName = v_WindowName.ConvertToUserVariable(sender);
            string searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);

            IntPtr wHnd = WindowNameControls.FindWindow(windowName, searchMethod, engine);
            User32Functions.RECT pos = User32Functions.GetWindowPosition(wHnd);

            var variableXPosition = v_XWindowPosition.ConvertToUserVariable(sender);
            int xPos;
            if ((variableXPosition == engine.engineSettings.CurrentWindowPositionKeyword) || (variableXPosition == engine.engineSettings.CurrentWindowXPositionKeyword))
            {
                xPos = pos.left;
            }
            else if (variableXPosition == engine.engineSettings.CurrentWindowYPositionKeyword)
            {
                xPos = pos.top;
            }
            else
            {
                xPos = v_XWindowPosition.ConvertToUserVariableAsInteger("X Position", engine);
            }

            var variableYPosition = v_YWindowPosition.ConvertToUserVariable(sender);
            int yPos;
            if ((variableYPosition == engine.engineSettings.CurrentWindowPositionKeyword) || (variableYPosition == engine.engineSettings.CurrentWindowYPositionKeyword))
            {
                yPos = pos.top;
            }
            else if (variableYPosition == engine.engineSettings.CurrentWindowXPositionKeyword)
            {
                yPos = pos.left;
            }
            else
            {
                yPos = v_YWindowPosition.ConvertToUserVariableAsInteger("Y Position", engine);
            }

            User32Functions.SetWindowPosition(wHnd, xPos, yPos);
        }

        //private static void MoveWindow(IntPtr hwnd, int xPos, int yPos, string xKeyword, string yKeyword, EngineSettings setting)
        //{
        //    int xWin, yWin;
        //    User32Functions.RECT rc = User32Functions.GetWindowPosition(hwnd);
        //    xWin = rc.left;
        //    yWin = rc.top;

        //    if ((xKeyword == setting.CurrentWindowPositionKeyword) || (xKeyword == setting.CurrentWindowXPositionKeyword))
        //    {
        //        xPos = xWin;
        //    }
        //    else if (xKeyword == setting.CurrentWindowYPositionKeyword)
        //    {
        //        xPos = yWin;
        //    }
        //    if ((yKeyword == setting.CurrentWindowPositionKeyword) || (yKeyword == setting.CurrentWindowYPositionKeyword))
        //    {
        //        yPos = yWin;
        //    }
        //    else if (yKeyword == setting.CurrentWindowXPositionKeyword)
        //    {
        //        yPos = xWin;
        //    }

        //    User32Functions.SetWindowPosition(hwnd, xPos, yPos);
        //}

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            //WindowNameControl = CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            //RenderedControls.Add(WindowNameControl);

            //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

            //var xGroup = CommandControls.CreateDefaultInputGroupFor("v_XWindowPosition", this, editor);
            //var yGroup = CommandControls.CreateDefaultInputGroupFor("v_YWindowPosition", this, editor);
            //RenderedControls.AddRange(xGroup);
            //RenderedControls.AddRange(yGroup);

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_XWindowPosition", this));
            //var xPositionControl = CommandControls.CreateDefaultInputFor("v_XWindowPosition", this);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_XWindowPosition", this, new Control[] { xPositionControl }, editor));
            //RenderedControls.Add(xPositionControl);

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_YWindowPosition", this));
            //var yPositionControl = CommandControls.CreateDefaultInputFor("v_YWindowPosition", this);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_YWindowPosition", this, new Control[] { yPositionControl }, editor));
            //RenderedControls.Add(yPositionControl);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));


            return RenderedControls;

        }
        public override void Refresh(frmCommandEditor editor)
        {
            base.Refresh();
            WindowNameControl.AddWindowNames();
        }


        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Coordinates (" + v_XWindowPosition + "," + v_YWindowPosition + ")]";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_WindowName))
        //    {
        //        this.validationResult += "Windows is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_XWindowPosition))
        //    {
        //        this.validationResult += "X is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_YWindowPosition))
        //    {
        //        this.validationResult += "Y is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}

        public override void convertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToIntermediateWindowName");
            cnv.Add("v_XWindowPosition", "convertToIntermediateWindowPosition");
            cnv.Add("v_YWindowPosition", "convertToIntermediateWindowPosition");
            convertToIntermediate(settings, cnv, variables);
        }

        public override void convertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToRawWindowName");
            cnv.Add("v_XWindowPosition", "convertToRawWindowPosition");
            cnv.Add("v_YWindowPosition", "convertToRawWindowPosition");
            convertToRaw(settings, cnv);
        }
    }
}