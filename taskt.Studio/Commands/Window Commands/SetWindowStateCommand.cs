using System;
using System.Collections.Generic;
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
    [Group("Window Commands")]
    [Description("This command sets a target window's state.")]
    [UsesDescription("Use this command when you want to change a window's state to minimized, maximized, or restored state")]
    [ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class SetWindowStateCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please enter or select the window that you want to target for change.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Input or Type the name of the window that you want to change.")]
        [SampleUsage("**Untitled - Notepad**")]
        [Remarks("")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [PropertyDescription("Please choose the new required state of the window.")]
        [PropertyUISelectionOption("Maximize")]
        [PropertyUISelectionOption("Minimize")]
        [PropertyUISelectionOption("Restore")]
        [InputSpecification("Select the appropriate window state required")]
        [SampleUsage("Choose from **Minimize**, **Maximize** and **Restore**")]
        [Remarks("")]
        public string v_WindowState { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;

        public SetWindowStateCommand()
        {
            CommandName = "SetWindowStateCommand";
            SelectionName = "Set Window State";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //convert window name
            string windowName = v_WindowName.ConvertToUserVariable(engine);

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window and set the window state
            foreach (var targetedWindow in targetWindows)
            {
                WindowState WINDOW_STATE = WindowState.SwShowNormal;
                switch (v_WindowState)
                {
                    case "Maximize":
                        WINDOW_STATE = WindowState.SwMaximize;
                        break;

                    case "Minimize":
                        WINDOW_STATE = WindowState.SwMinimize;
                        break;

                    case "Restore":
                        WINDOW_STATE = WindowState.SwRestore;
                        break;

                    default:
                        break;
                }

                User32Functions.SetWindowState(targetedWindow, WINDOW_STATE);
            }     
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            WindowNameControl = CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames();
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            RenderedControls.Add(WindowNameControl);

            var windowStateLabel = CommandControls.CreateDefaultLabelFor("v_WindowState", this);
            RenderedControls.Add(windowStateLabel);

            var windowStateControl = CommandControls.CreateDropdownFor("v_WindowState", this);
            RenderedControls.Add(windowStateControl);

            return RenderedControls;

        }
        public override void Refresh(IfrmCommandEditor editor)
        {
            base.Refresh();
            WindowNameControl.AddWindowNames();
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Window State: " + v_WindowState + "]";
        }
    }
}