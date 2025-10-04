using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.KeyMouseGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Key/Mouse")]
    [Attributes.ClassAttributes.SubGruop("Key")]
    [Attributes.ClassAttributes.CommandSettings("Enter Shortcut Key From Window Handle")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class EnterShortcutKeyFromWindowHandleCommand : AWindowHandleActionCommands, IEnterShortcutKeyProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_Hotkey))]
        [PropertySelectionChangeEvent(nameof(cmbHotkey_SelectedIndexChanged))]
        [PropertyParameterOrder(5100)]
        public string v_Hotkey { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_WaitTimeAfterKeyEnter))]
        [PropertyParameterOrder(8010)]
        public string v_WaitTimeAfterKeyEnter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_ActivateCurrentWindow))]
        [PropertyParameterOrder(8020)]
        public string v_ActivateCurrentWindow { get; set; }

        [XmlAttribute]
        [PropertyIsOptional(true, "Yes")]
        [PropertyFirstValue("Yes")]
        public override string v_ActivateBeforeAction { get; set; }

        public EnterShortcutKeyFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            string sendKey = "";
            switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_Hotkey), engine))
            {
                case "new":
                    sendKey = "^n";
                    break;
                case "new window":
                    sendKey = "^n";
                    break;
                case "open":
                    sendKey = "^o";
                    break;
                case "print":
                    sendKey = "^p";
                    break;
                case "save":
                    sendKey = "^s";
                    break;
                case "save as":
                    sendKey = "^+s";
                    break;
                case "undo":
                    sendKey = "^z";
                    break;
                case "cut":
                    sendKey = "^x";
                    break;
                case "copy":
                    sendKey = "^c";
                    break;
                case "paste":
                    sendKey = "^v";
                    break;
                case "delete":
                    sendKey = "{DEL}";
                    break;
                case "search":
                    sendKey = "^e";
                    break;
                case "find":
                    sendKey = "^f";
                    break;
                case "find next":
                    sendKey = "{F3}";
                    break;
                case "find previous":
                    sendKey = "+{F3}";
                    break;
                case "replace":
                    sendKey = "^h";
                    break;
                case "go to":
                    sendKey = "^g";
                    break;
                case "select all":
                    sendKey = "^a";
                    break;
            }

            var enterKeysCommand = new EnterKeysFromWindowHandleCommand
            {
                v_WindowHandle = this.v_WindowHandle,
                v_TextToSend = sendKey,
                v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                v_WaitTimeAfterKeyEnter = this.v_WaitTimeAfterKeyEnter,
                v_WindowNameResult = this.v_WindowNameResult,
                v_WaitTimeBetweenFindAndAction = this.v_WaitTimeBetweenFindAndAction,
                v_ActivateBeforeAction = this.v_ActivateBeforeAction,
                v_ActivateCurrentWindow = this.v_ActivateCurrentWindow,
            };
            enterKeysCommand.RunCommand(engine);
        }

        private void cmbHotkey_SelectedIndexChanged(object sender, EventArgs e)
        {
            var searchedKey = ((ComboBox)sender).SelectedItem?.ToString() ?? "";

            ControlsList.SecondLabelProcess(nameof(v_Hotkey), nameof(v_Hotkey), searchedKey);
        }
    }
}