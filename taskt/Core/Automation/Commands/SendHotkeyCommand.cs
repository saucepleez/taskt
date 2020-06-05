using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    public class SendHotkeyCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Hotkey to Send")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text that should be sent to the specified window.")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello, World!** or **[vEntryText]**")]
        [Attributes.PropertyAttributes.Remarks("This command supports sending variables within brackets [vVariable]")]
        public string v_Hotkey { get; set; }

        public SendHotkeyCommand()
        {
            this.CommandName = "SendHotkeyCommand";
            this.SelectionName = "Send Hotkey";
            this.CommandEnabled = true;
            this.CustomRendering = true;
     
        }

        public override void RunCommand(object sender)
        {
            if (v_WindowName != "Current Window")
            {
                ActivateWindowCommand activateWindow = new ActivateWindowCommand
                {
                    v_WindowName = v_WindowName
                };
                activateWindow.RunCommand(sender);
            }

            var hotkey = v_Hotkey.ConvertToUserVariable(sender);

            switch (hotkey)
            {
                case "New":
                    User32Functions.KeyDownKeyUp(new []{ Keys.ControlKey, Keys.N});                 
                    break;
                case "New Window":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.ShiftKey, Keys.N });
                    break;
                case "Open":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.O });
                    break;
                case "Print":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.P });
                    break;
                case "Save":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.S });
                    break;
                case "Save As":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.ShiftKey, Keys.S });
                    break;
                case "Undo":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.Z });
                    break;
                case "Cut":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.X });
                    break;
                case "Copy":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.C });
                    break;
                case "Paste":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.V });
                    break;
                case "Delete":
                    User32Functions.KeyDownKeyUp(new[] { Keys.Delete });
                    break;
                case "Search":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.E });
                    break;
                case "Find":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.F });
                    break;
                case "Find Next":
                    User32Functions.KeyDownKeyUp(new[] { Keys.F3 });
                    break;
                case "Find Previous":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ShiftKey, Keys.F3 });
                    break;
                case "Replace":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.H });
                    break;
                case "Go To":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.G });
                    break;
                case "Select All":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.A });
                    break;
                default:
                    break;
            }

            System.Threading.Thread.Sleep(500);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);


            RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            var WindowNameControl = UI.CustomControls.CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames();
            RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            RenderedControls.Add(WindowNameControl);

            var controls = CommandControls.CreateDefaultDropdownGroupFor("v_Hotkey", this, editor);
            var hotkey = controls[2] as ComboBox;

            hotkey.Items.Add("New");
            hotkey.Items.Add("New Window");
            hotkey.Items.Add("Open");
            hotkey.Items.Add("Print");
            hotkey.Items.Add("Save");
            hotkey.Items.Add("Save As");

            hotkey.Items.Add("Undo");
            hotkey.Items.Add("Cut");
            hotkey.Items.Add("Copy");
            hotkey.Items.Add("Paste");
            hotkey.Items.Add("Delete");
            hotkey.Items.Add("Search");
            hotkey.Items.Add("Find");
            hotkey.Items.Add("Find Next");
            hotkey.Items.Add("Find Previous");
            hotkey.Items.Add("Replace");
            hotkey.Items.Add("Go To");
            hotkey.Items.Add("Select All");

            RenderedControls.AddRange(controls);

            return RenderedControls;

        }

       

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Send '" + v_Hotkey + "' to '" + v_WindowName + "']";
        }
    }
}