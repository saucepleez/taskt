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
    [Attributes.ClassAttributes.Description("This command sets a target window's state.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change a window's state to minimized, maximized, or restored state")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SetWindowStateCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please enter or select the window that you want to target for change.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Input or Type the name of the window that you want to change.")]
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
        [PropertyDescription("Please choose the new required state of the window.")]
        [PropertyUISelectionOption("Maximize")]
        [PropertyUISelectionOption("Minimize")]
        [PropertyUISelectionOption("Restore")]
        [InputSpecification("Select the appropriate window state required")]
        [SampleUsage("Choose from **Minimize**, **Maximize** and **Restore**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Window State", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_WindowState { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;

        public SetWindowStateCommand()
        {
            this.CommandName = "SetWindowStateCommand";
            this.SelectionName = "Set Window State";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            ////convert window name
            //string windowName = v_WindowName.ConvertToUserVariable(sender);

            //string searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(searchMethod))
            //{
            //    searchMethod = "Contains";
            //}

            //bool targetIsCurrentWindow = ((Engine.AutomationEngineInstance)sender).engineSettings.CurrentWindowKeyword == windowName;

            //var targetWindows = User32Functions.FindTargetWindows(windowName, targetIsCurrentWindow, (searchMethod != "Contains"));

            //User32Functions.WindowState WINDOW_STATE = User32Functions.WindowState.SW_SHOWNORMAL;
            //switch (v_WindowState)
            //{
            //    case "Maximize":
            //        WINDOW_STATE = User32Functions.WindowState.SW_MAXIMIZE;
            //        break;

            //    case "Minimize":
            //        WINDOW_STATE = User32Functions.WindowState.SW_MINIMIZE;
            //        break;

            //    case "Restore":
            //        WINDOW_STATE = User32Functions.WindowState.SW_RESTORE;
            //        break;

            //    default:
            //        break;
            //}

            //if (searchMethod == "Contains" || targetIsCurrentWindow)
            //{
            //    //loop each window and set the window state
            //    foreach (var targetedWindow in targetWindows)
            //    {
            //        //if (User32Functions.IsIconic(targetedWindow) && (WINDOW_STATE != User32Functions.WindowState.SW_MINIMIZE))
            //        //{
            //        //    User32Functions.ShowWindowAsync(targetedWindow, WINDOW_STATE);
            //        //}

            //        //User32Functions.SetWindowState(targetedWindow, WINDOW_STATE);

            //        //if (WINDOW_STATE != User32Functions.WindowState.SW_MINIMIZE)
            //        //{
            //        //    User32Functions.SetForegroundWindow(targetedWindow);
            //        //}
            //        SetWindowState(targetedWindow, WINDOW_STATE);
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

            //    bool isChanged = false;
            //    foreach (var targetedWindow in targetWindows)
            //    {
            //        if (searchFunc(User32Functions.GetWindowTitle(targetedWindow)))
            //        {
            //            //if (User32Functions.IsIconic(targetedWindow) && (WINDOW_STATE != User32Functions.WindowState.SW_MINIMIZE))
            //            //{
            //            //    User32Functions.ShowWindowAsync(targetedWindow, WINDOW_STATE);
            //            //}

            //            //User32Functions.SetWindowState(targetedWindow, WINDOW_STATE);

            //            //if (WINDOW_STATE != User32Functions.WindowState.SW_MINIMIZE)
            //            //{
            //            //    User32Functions.SetForegroundWindow(targetedWindow);
            //            //}
            //            SetWindowState(targetedWindow, WINDOW_STATE);
            //            isChanged = true;
            //        }
            //    }
            //    if (!isChanged)
            //    {
            //        throw new Exception("Window name '" + windowName + "' is not found.Search method " + searchMethod + ".");
            //    }
            //}

            var engine = (Engine.AutomationEngineInstance)sender;

            string windowName = v_WindowName.ConvertToUserVariable(sender);
            string searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);

            string windowState = v_WindowState.GetUISelectionValue("v_WindowState", this, engine);
            User32Functions.WindowState state = User32Functions.WindowState.SW_RESTORE;
            switch (windowState.ToLower())
            {
                case "maximize":
                    state = User32Functions.WindowState.SW_MAXIMIZE;
                    break;
                case "minimize":
                    state = User32Functions.WindowState.SW_MINIMIZE;
                    break;
            }

            IntPtr wHnd = WindowNameControls.FindWindow(windowName, searchMethod, engine);

            if (User32Functions.IsIconic(wHnd) && (state != User32Functions.WindowState.SW_MINIMIZE))
            {
                User32Functions.ShowWindowAsync(wHnd, state);
            }
            User32Functions.SetWindowState(wHnd, state);
        }

        //private void SetWindowState(IntPtr whnd, User32Functions.WindowState state)
        //{
        //    if (User32Functions.IsIconic(whnd) && (state != User32Functions.WindowState.SW_MINIMIZE))
        //    {
        //        User32Functions.ShowWindowAsync(whnd, state);
        //    }

        //    User32Functions.SetWindowState(whnd, state);

        //    if (state != User32Functions.WindowState.SW_MINIMIZE)
        //    {
        //        User32Functions.SetForegroundWindow(whnd);
        //    }
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

            //var windowStateLabel = CommandControls.CreateDefaultLabelFor("v_WindowState", this);
            //RenderedControls.Add(windowStateLabel);

            //var windowStateControl = CommandControls.CreateDropdownFor("v_WindowState", this);
            //RenderedControls.Add(windowStateControl);
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
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Window State: " + v_WindowState + "]";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_WindowName))
        //    {
        //        this.validationResult += "Window is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_WindowState))
        //    {
        //        this.validationResult += "State of the window is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}

        public override void convertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToIntermediateWindowName");
            convertToIntermediate(settings, cnv, variables);
        }

        public override void convertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToRawWindowName");
            convertToRaw(settings, cnv);
        }
    }
}