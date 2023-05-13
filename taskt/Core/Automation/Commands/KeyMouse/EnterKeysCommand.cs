using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Key/Mouse Commands")]
    [Attributes.ClassAttributes.SubGruop("Key")]
    [Attributes.ClassAttributes.CommandSettings("Enter Keys")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class EnterKeysCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text or Keys to Send.")]
        [PropertyCustomUIHelper("Keys Builder", nameof(lnkKeysBulider_Click))]
        [PropertyCustomUIHelper("Encrypt Text", nameof(lnkEncryptText_Click))]
        [InputSpecification("Text to Send", true)]
        //[SampleUsage("**Hello, World!** or **^s** or **{{{vEntryText}}}** or **{WIN_KEY}** or **{WIN_KEY+R}**")]
        [PropertyDetailSampleUsage("**Hello, World!**", PropertyDetailSampleUsage.ValueType.Value, "Text")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text")]
        [PropertyDetailSampleUsage("**^s**", "Specify **Ctrl+S** for Enter Keys")]
        [PropertyDetailSampleUsage("**{WIN_KEY}**", "Specify **Windows Key** for Enter Keys")]
        [PropertyDetailSampleUsage("**{WIN_KEY+R}**", "Specify **Windows Key** and **R** for Enter Keys")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIntermediateConvert(nameof(ApplicationSettings.EngineSettings.convertToIntermediateVariableParser), "")]
        [PropertyDisplayText(true, "Text")]
        public string v_TextToSend { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Text is Encrypted or Not")]
        [PropertyUISelectionOption("Not Encrypted")]
        [PropertyUISelectionOption("Encrypted")]
        [PropertyIsOptional(true, "Not Encrypted")]
        public string v_EncryptionOption { get; set; }

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
        public string v_WaitTime { get; set; }

        public EnterKeysCommand()
        {
            //this.CommandName = "SendKeysCommand";
            //this.SelectionName = "Send Keystrokes";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_EncryptionOption = "Not Encrypted";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetWindow = v_WindowName.ConvertToUserVariable(engine);
            if (targetWindow != engine.engineSettings.CurrentWindowKeyword)
            {
                var activateWindow = new ActivateWindowCommand
                {
                    v_WindowName = v_WindowName,
                    v_SearchMethod = v_SearchMethod,
                    v_MatchMethod= v_MatchMethod,
                    v_TargetWindowIndex = v_TargetWindowIndex,
                    v_WaitTime = v_WaitForWindow
                };
                activateWindow.RunCommand(engine);
            }

            var textToSend = v_TextToSend.ConvertToUserVariable(engine);

            var encryptOption = this.GetUISelectionValue(nameof(v_EncryptionOption), engine);
            if (encryptOption == "encrypted")
            {
                textToSend = EncryptionServices.DecryptString(textToSend, "TASKT");
            }

            if (textToSend == "{WIN_KEY}")
            {
                //User32Functions.KeyDown(Keys.LWin);
                //User32Functions.KeyUp(Keys.LWin);
                KeyMouseControls.KeyDown(Keys.LWin);
                KeyMouseControls.KeyUp(Keys.LWin);
            }
            else if (textToSend.StartsWith("{WIN_KEY+") && textToSend.EndsWith("}"))
            {
                //User32Functions.KeyDown(Keys.LWin);
                KeyMouseControls.KeyDown(Keys.LWin);
                var remainingText = textToSend.Replace("{WIN_KEY+", "").Replace("}","");

                foreach (var c in remainingText)
                {
                    Keys key = (Keys)Enum.Parse(typeof(Keys), c.ToString());
                    //User32Functions.KeyDown(key);
                    KeyMouseControls.KeyDown(key);
                }

                //User32Functions.KeyUp(Keys.LWin);
                KeyMouseControls.KeyUp(Keys.LWin);

                foreach (var c in remainingText)
                {
                    Keys key = (Keys)Enum.Parse(typeof(Keys), c.ToString());
                    //User32Functions.KeyUp(key);
                    KeyMouseControls.KeyUp(key);
                }
            }
            else
            {
                SendKeys.SendWait(textToSend);
            }

            var waitTime = this.ConvertToUserVariableAsInteger(nameof(v_WaitTime), engine);
            System.Threading.Thread.Sleep(waitTime);
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        private void lnkEncryptText_Click(object sender, EventArgs e)
        {
            //var InputText = (TextBox)ControlsList[nameof(v_TextToSend)];
            var InputText = ControlsList.GetPropertyControl<TextBox>(nameof(v_TextToSend));

            if (string.IsNullOrEmpty(InputText.Text))
            {
                MessageBox.Show("Text to send is empty.", "Notice");
                return;
            }

            var encrypted = EncryptionServices.EncryptString(InputText.Text, "TASKT");
            this.v_EncryptionOption = "Encrypted";

            InputText.Text = encrypted;
        }

        private void lnkKeysBulider_Click(object sender, EventArgs e)
        {
            using (var fm = new taskt.UI.Forms.Supplement_Forms.frmKeysBuilder())
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    //var InputText = (TextBox)ControlsList[nameof(v_TextToSend)];
                    var InputText = ControlsList.GetPropertyControl<TextBox>(nameof(v_TextToSend));
                    InputText.Text = fm.Result;
                }
            }
        }
    }
}