using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Key/Mouse Commands")]
    [Attributes.ClassAttributes.SubGruop("Key")]
    [Attributes.ClassAttributes.CommandSettings("Enter Shortcut Key")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class EnterShortcutKeyCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

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
        public string v_Hotkey { get; set; }

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

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_WaitTimeAfterKeyEnter))]
        public string v_WaitAfterKeyEnter { get; set; }

        public EnterShortcutKeyCommand()
        {
            //this.CommandName = "SendHotkeyCommand";
            //this.SelectionName = "Send Hotkey";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string sendKey = "";
            switch (this.GetUISelectionValue(nameof(v_Hotkey), engine))
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
                v_WindowName = v_WindowName,
                v_SearchMethod = v_SearchMethod,
                v_TextToSend = sendKey,
                v_MatchMethod = v_MatchMethod,
                v_TargetWindowIndex = v_TargetWindowIndex,
                v_WaitForWindow = v_WaitForWindow,
                v_WaitTime = v_WaitAfterKeyEnter
            };
            enterKeysCommand.RunCommand(engine);
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        private void cmbHotkey_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var searchedKey = parameterHotkey.Text;
            var searchedKey = ((ComboBox)sender).SelectedItem?.ToString() ?? "";

            //var dic = (Dictionary<string, string>)((Label)ControlsList["lbl_" + nameof(v_Hotkey)]).Tag;

            //var hotkey2ndLabel = (Label)ControlsList["lbl2_" + nameof(v_Hotkey)];
            //hotkey2ndLabel.Text = dic.ContainsKey(searchedKey) ? dic[searchedKey] : "";

            ControlsList.SecondLabelProcess(nameof(v_Hotkey), nameof(v_Hotkey), searchedKey);
        }
    }
}