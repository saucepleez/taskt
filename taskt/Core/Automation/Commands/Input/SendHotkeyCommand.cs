using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Linq;

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
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindowName}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsWindowNamesList(true)]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Window name search method (Default is Contains)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Contains")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Start with")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("End with")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Exact match")]
        [Attributes.PropertyAttributes.SampleUsage("**Contains** or **Start with** or **End with** or **Exact match**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select Hotkey to Send.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text that should be sent to the specified window.")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("New")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("New Window")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Open")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Print")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Save")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Save As")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Undo")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Cut")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Copy")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Paste")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Delete")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Search")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Next")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Previous")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Replace")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Go To")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Select All")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertySecondaryLabel(true)]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("New", "Send Ctrl + N")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("New Window", "Send Ctrl + Shift + N")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Open", "Send Ctrl + O")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Print", "Send Ctrl + P")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Save", "Send Ctrl + S")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Save As", "Send Ctrl + Shift + S")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Undo", "Send Ctrl + Z")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Cut", "Send Ctrl + X")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Copy", "Send Ctrl + C")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Paste", "Send Ctrl + V")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Delete", "Send Delete")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Search", "Send Ctrl + E")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Find", "Send Ctrl + F")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Find Next", "Send F3")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Find Previous", "Send Shift + F3")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Replace", "Send Ctrl + H")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Go To", "Send Ctrl + G")]
        [Attributes.PropertyAttributes.PropertyAddtionalParameterInfo("Select All", "Send Ctrl + A")]
        public string v_Hotkey { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox parameterHotkey;

        [XmlIgnore]
        [NonSerialized]
        private Label hotkey2ndLabel;

        [XmlIgnore]
        [NonSerialized]
        private List<Core.Automation.Attributes.PropertyAttributes.PropertyAddtionalParameterInfo> hotkeyInfo;

        public SendHotkeyCommand()
        {
            this.CommandName = "SendHotkeyCommand";
            this.SelectionName = "Send Hotkey";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            var variableProperties = this.GetType().GetProperties().Where(f => f.Name == "v_Hotkey").FirstOrDefault();
            hotkeyInfo = variableProperties.GetCustomAttributes(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyAddtionalParameterInfo), true).Cast<Core.Automation.Attributes.PropertyAttributes.PropertyAddtionalParameterInfo>().ToList();
        }

        public override void RunCommand(object sender)
        {
            var targetWindow = v_WindowName.ConvertToUserVariable(sender);
            var searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(searchMethod))
            {
                searchMethod = "Contains";
            }
            if (targetWindow != ((Automation.Engine.AutomationEngineInstance)sender).engineSettings.CurrentWindowKeyword)
            {
                ActivateWindowCommand activateWindow = new ActivateWindowCommand
                {
                    v_WindowName = targetWindow,
                    v_SearchMethod = searchMethod
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

            //RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            //var WindowNameControl = UI.CustomControls.CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
            //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            //RenderedControls.Add(WindowNameControl);

            //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

            //var controls = CommandControls.CreateDefaultDropdownGroupFor("v_Hotkey", this, editor);
            //var hotkey = controls[2] as ComboBox;

            //hotkey.Items.Add("New");
            //hotkey.Items.Add("New Window");
            //hotkey.Items.Add("Open");
            //hotkey.Items.Add("Print");
            //hotkey.Items.Add("Save");
            //hotkey.Items.Add("Save As");

            //hotkey.Items.Add("Undo");
            //hotkey.Items.Add("Cut");
            //hotkey.Items.Add("Copy");
            //hotkey.Items.Add("Paste");
            //hotkey.Items.Add("Delete");
            //hotkey.Items.Add("Search");
            //hotkey.Items.Add("Find");
            //hotkey.Items.Add("Find Next");
            //hotkey.Items.Add("Find Previous");
            //hotkey.Items.Add("Replace");
            //hotkey.Items.Add("Go To");
            //hotkey.Items.Add("Select All");

            //RenderedControls.AddRange(controls);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            parameterHotkey = (ComboBox)ctrls.Where(t => t.Name == "v_Hotkey").FirstOrDefault();
            parameterHotkey.SelectedIndexChanged += (sender, e) => hotkeyCombobox_SelectedIndexChanged(sender, e);
            hotkey2ndLabel = (Label)ctrls.Where(t => t.Name == "lbl2_v_Hotkey").FirstOrDefault();

            return RenderedControls;

        }

        private void hotkeyCombobox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var seachedKey = parameterHotkey.Text;
            var targetInfo = hotkeyInfo.Where(t => t.searchKey == seachedKey).FirstOrDefault();
            hotkey2ndLabel.Text = (targetInfo != null) ? targetInfo.description : "";
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Send '" + v_Hotkey + "' to '" + v_WindowName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_WindowName))
            {
                this.validationResult += "Window name is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_Hotkey))
            {
                this.validationResult += "Hotkey is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}