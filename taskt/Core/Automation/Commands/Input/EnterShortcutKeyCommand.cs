using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
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
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

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

            // TODO: all EnterKeys

            // activate
            var activateWindow = new ActivateWindowCommand
            {
                v_WindowName = v_WindowName,
                v_SearchMethod = v_SearchMethod,
                v_MatchMethod = v_MatchMethod,
                v_TargetWindowIndex = v_TargetWindowIndex,
                v_WaitTime = v_WaitForWindow
            };
            activateWindow.RunCommand(engine);

            switch (this.GetUISelectionValue(nameof(v_Hotkey), engine))
            {
                case "new":
                    User32Functions.KeyDownKeyUp(new []{ Keys.ControlKey, Keys.N});                 
                    break;
                case "new window":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.ShiftKey, Keys.N });
                    break;
                case "open":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.O });
                    break;
                case "print":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.P });
                    break;
                case "save":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.S });
                    break;
                case "save as":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.ShiftKey, Keys.S });
                    break;
                case "undo":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.Z });
                    break;
                case "cut":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.X });
                    break;
                case "copy":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.C });
                    break;
                case "paste":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.V });
                    break;
                case "delete":
                    User32Functions.KeyDownKeyUp(new[] { Keys.Delete });
                    break;
                case "search":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.E });
                    break;
                case "find":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.F });
                    break;
                case "find next":
                    User32Functions.KeyDownKeyUp(new[] { Keys.F3 });
                    break;
                case "find previous":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ShiftKey, Keys.F3 });
                    break;
                case "replace":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.H });
                    break;
                case "go to":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.G });
                    break;
                case "select all":
                    User32Functions.KeyDownKeyUp(new[] { Keys.ControlKey, Keys.A });
                    break;
            }

            var waitKeyEnter = this.ConvertToUserVariableAsInteger(nameof(v_WaitAfterKeyEnter), engine);
            System.Threading.Thread.Sleep(waitKeyEnter);
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        private void cmbHotkey_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var searchedKey = parameterHotkey.Text;
            var searchedKey = ((ComboBox)sender).SelectedItem?.ToString() ?? "";

            var dic = (Dictionary<string, string>)((Label)ControlsList["lbl_" + nameof(v_Hotkey)]).Tag;

            var hotkey2ndLabel = (Label)ControlsList["lbl2_" + nameof(v_Hotkey)];
            hotkey2ndLabel.Text = dic.ContainsKey(searchedKey) ? dic[searchedKey] : "";
        }

        public override void ConvertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToIntermediateWindowName");
            ConvertToIntermediate(settings, cnv, variables);
        }

        public override void ConvertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToRawWindowName");
            ConvertToRaw(settings, cnv);
        }
    }
}