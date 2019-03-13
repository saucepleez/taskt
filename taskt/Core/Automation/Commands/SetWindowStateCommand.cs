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
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command sets a target window's state.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change a window's state to minimized, maximized, or restored state")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class SetWindowStateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter or select the window that you want to target for change.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to change.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please choose the new required state of the window.")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Maximize")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Minimize")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Restore")]
        [Attributes.PropertyAttributes.InputSpecification("Select the appropriate window state required")]
        [Attributes.PropertyAttributes.SampleUsage("Choose from **Minimize**, **Maximize** and **Restore**")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            //convert window name
            string windowName = v_WindowName.ConvertToUserVariable(sender);

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window and set the window state
            foreach (var targetedWindow in targetWindows)
            {
                User32Functions.WindowState WINDOW_STATE = User32Functions.WindowState.SW_SHOWNORMAL;
                switch (v_WindowState)
                {
                    case "Maximize":
                        WINDOW_STATE = User32Functions.WindowState.SW_MAXIMIZE;
                        break;

                    case "Minimize":
                        WINDOW_STATE = User32Functions.WindowState.SW_MINIMIZE;
                        break;

                    case "Restore":
                        WINDOW_STATE = User32Functions.WindowState.SW_RESTORE;
                        break;

                    default:
                        break;
                }

                User32Functions.SetWindowState(targetedWindow, WINDOW_STATE);
            }     
        }
        public override List<Control> Render(frmCommandEditor editor)
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
        public override void Refresh(frmCommandEditor editor)
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