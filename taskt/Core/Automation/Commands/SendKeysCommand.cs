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
    public class SendKeysCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter text to send")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text that should be sent to the specified window.")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello, World!** or **[vEntryText]**")]
        [Attributes.PropertyAttributes.Remarks("This command supports sending variables within brackets [vVariable]")]
        public string v_TextToSend { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Indicate if Text is Encrypted")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Not Encrypted")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Encrypted")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate if the text in 'TextToSend' is Encrypted.")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_EncryptionOption { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private TextBox InputText;

        public SendKeysCommand()
        {
            this.CommandName = "SendKeysCommand";
            this.SelectionName = "Send Keystrokes";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_EncryptionOption = "Not Encrypted";
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

            string textToSend = v_TextToSend.ConvertToUserVariable(sender);

            if (v_EncryptionOption == "Encrypted")
            {
                textToSend = Core.EncryptionServices.DecryptString(textToSend, "TASKT");
            }

            if (textToSend == "{WIN_KEY}")
            {
                User32Functions.KeyDown(System.Windows.Forms.Keys.LWin);
                User32Functions.KeyUp(System.Windows.Forms.Keys.LWin);

            }
            else if (textToSend.Contains("{WIN_KEY+"))
            {
                User32Functions.KeyDown(System.Windows.Forms.Keys.LWin);
                var remainingText = textToSend.Replace("{WIN_KEY+", "").Replace("}","");

                foreach (var c in remainingText)
                {
                    System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), c.ToString());
                    User32Functions.KeyDown(key);
                }

                User32Functions.KeyUp(System.Windows.Forms.Keys.LWin);

                foreach (var c in remainingText)
                {
                    System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), c.ToString());
                    User32Functions.KeyUp(key);
                }
            }
            else
            {
                System.Windows.Forms.SendKeys.SendWait(textToSend);
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

            taskt.UI.CustomControls.CommandItemControl helperControl = new taskt.UI.CustomControls.CommandItemControl();

            var textInputGroup = CommandControls.CreateDefaultInputGroupFor("v_TextToSend", this, editor);
            RenderedControls.AddRange(textInputGroup);

            InputText = (TextBox)textInputGroup[2];

            helperControl.ForeColor = Color.White;
            helperControl.CommandDisplay = "Encrypt Text";
            helperControl.Click += HelperControl_Click;
            RenderedControls.Add(helperControl);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_EncryptionOption", this, editor));

            return RenderedControls;

        }

        private void HelperControl_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(InputText.Text))
                return;

            var encrypted = EncryptionServices.EncryptString(InputText.Text, "TASKT");
            this.v_EncryptionOption = "Encrypted";

            InputText.Text = encrypted;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Send '" + v_TextToSend + "' to '" + v_WindowName + "']";
        }
    }
}