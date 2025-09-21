using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Key/Mouse")]
    [Attributes.ClassAttributes.SubGruop("Key")]
    [Attributes.ClassAttributes.CommandSettings("Enter Shortcut Key")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class EnterShortcutKeyCommand : AOneWindowNameActionCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowName))]
        //public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Shortcut Key to Enter")]
        [PropertyUISelectionOption("New")]
        [PropertyUISelectionOption("New Window")]
        [PropertyUISelectionOption("Open")]
        [PropertyUISelectionOption("Print")]
        [PropertyUISelectionOption("Save")]
        [PropertyUISelectionOption("Save As")]
        [PropertyUISelectionOption("Undo")]
        [PropertyUISelectionOption("Cut")]
        [PropertyUISelectionOption("Copy")]
        [PropertyUISelectionOption("Paste")]
        [PropertyUISelectionOption("Delete")]
        [PropertyUISelectionOption("Search")]
        [PropertyUISelectionOption("Find")]
        [PropertyUISelectionOption("Find Next")]
        [PropertyUISelectionOption("Find Previous")]
        [PropertyUISelectionOption("Replace")]
        [PropertyUISelectionOption("Go To")]
        [PropertyUISelectionOption("Select All")]
        [PropertySecondaryLabel(true)]
        [PropertyAddtionalParameterInfo("New", "Send Ctrl + N")]
        [PropertyAddtionalParameterInfo("New Window", "Send Ctrl + Shift + N")]
        [PropertyAddtionalParameterInfo("Open", "Send Ctrl + O")]
        [PropertyAddtionalParameterInfo("Print", "Send Ctrl + P")]
        [PropertyAddtionalParameterInfo("Save", "Send Ctrl + S")]
        [PropertyAddtionalParameterInfo("Save As", "Send Ctrl + Shift + S")]
        [PropertyAddtionalParameterInfo("Undo", "Send Ctrl + Z")]
        [PropertyAddtionalParameterInfo("Cut", "Send Ctrl + X")]
        [PropertyAddtionalParameterInfo("Copy", "Send Ctrl + C")]
        [PropertyAddtionalParameterInfo("Paste", "Send Ctrl + V")]
        [PropertyAddtionalParameterInfo("Delete", "Send Delete")]
        [PropertyAddtionalParameterInfo("Search", "Send Ctrl + E")]
        [PropertyAddtionalParameterInfo("Find", "Send Ctrl + F")]
        [PropertyAddtionalParameterInfo("Find Next", "Send F3")]
        [PropertyAddtionalParameterInfo("Find Previous", "Send Shift + F3")]
        [PropertyAddtionalParameterInfo("Replace", "Send Ctrl + H")]
        [PropertyAddtionalParameterInfo("Go To", "Send Ctrl + G")]
        [PropertyAddtionalParameterInfo("Select All", "Send Ctrl + A")]
        [PropertySelectionChangeEvent(nameof(cmbHotkey_SelectedIndexChanged))]
        [PropertyValidationRule("Shortcut Key", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Shortcut")]
        [PropertyParameterOrder(5100)]
        public string v_Hotkey { get; set; }

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

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_WaitTimeAfterKeyEnter))]
        [PropertyParameterOrder(8010)]
        public string v_WaitTimeAfterKeyEnter { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowNameResult))]
        //public string v_NameResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        //public string v_HandleResult { get; set; }

        [XmlAttribute]
        [PropertyIsOptional(true, "Yes")]
        [PropertyFirstValue("Yes")]
        public override string v_ActivateBeforeAction { get; set; }

        public EnterShortcutKeyCommand()
        {
            //this.CommandName = "SendHotkeyCommand";
            //this.SelectionName = "Send Hotkey";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
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

            var enterKeysCommand = new EnterKeysCommand
            {
                v_WindowName = this.v_WindowName,
                v_CompareMethod = this.v_CompareMethod,
                v_TextToSend = sendKey,
                v_MatchMethod = this.v_MatchMethod,
                v_TargetWindowIndex = this.v_TargetWindowIndex,
                v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                v_WaitTimeAfterKeyEnter = this.v_WaitTimeAfterKeyEnter,
                v_NameResult = this.v_NameResult,
                v_HandleResult = this.v_HandleResult,
                v_WaitTimeBetweenFindAndAction = this.v_WaitTimeBetweenFindAndAction,
                v_ActivateBeforeAction = this.v_ActivateBeforeAction,
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