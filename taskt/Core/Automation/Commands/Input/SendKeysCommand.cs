using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

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
        [PropertyDescription("Please Enter the Window name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindowName}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsWindowNamesList(true)]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Window name search method")]
        [InputSpecification("")]
        [PropertyUISelectionOption("Contains")]
        [PropertyUISelectionOption("Starts with")]
        [PropertyUISelectionOption("Ends with")]
        [PropertyUISelectionOption("Exact match")]
        [SampleUsage("**Contains** or **Starts with** or **Ends with** or **Exact match**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Contains")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter text to send.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyCustomUIHelper("Keys Builder", "lnkKeysBulider_Click")]
        [PropertyCustomUIHelper("Encrypt Text", "lnkEncryptText_Click")]
        [InputSpecification("Enter the text that should be sent to the specified window.")]
        [SampleUsage("**Hello, World!** or **^s** or **{{{vEntryText}}}** or **{WIN_KEY}** or **{WIN_KEY+R}**")]
        [Remarks("This command supports sending variables within brackets {{{vVariable}}}")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_TextToSend { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Indicate if Text is Encrypted")]
        [PropertyUISelectionOption("Not Encrypted")]
        [PropertyUISelectionOption("Encrypted")]
        [InputSpecification("Indicate if the text in 'TextToSend' is Encrypted.")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true, "Not Encrypted")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_EncryptionOption { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify waiting time after keystrokes (ms)")]
        [InputSpecification("")]
        [SampleUsage("**500** or **{{{vWaitTime}}}")]
        [Remarks("If less than 100 is specified, it will be 100")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyIsOptional(true, "500")]
        [PropertyFirstValue("500")]
        public string v_WaitTime { get; set; }

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
            var engine = (taskt.Core.Automation.Engine.AutomationEngineInstance)sender;

            string targetWindowName = v_WindowName.ConvertToUserVariable(sender);
            var searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(searchMethod))
            {
                searchMethod = "Contains";
            }
            if (targetWindowName != ((Engine.AutomationEngineInstance)sender).engineSettings.CurrentWindowKeyword)
            {
                ActivateWindowCommand activateWindow = new ActivateWindowCommand
                {
                    v_WindowName = targetWindowName,
                    v_SearchMethod = searchMethod
                };
                activateWindow.RunCommand(sender);
            }

            string textToSend = v_TextToSend.ConvertToUserVariable(sender);
            string EncryptionOption = v_EncryptionOption.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(EncryptionOption))
            {
                EncryptionOption = "Not Encrypted";
            }

            if (EncryptionOption == "Encrypted")
            {
                textToSend = EncryptionServices.DecryptString(textToSend, "TASKT");
            }

            if (String.IsNullOrEmpty(v_WaitTime))
            {
                v_WaitTime = "500";
            }
            int waitTime = v_WaitTime.ConvertToUserVariableAsInteger("Wait Time", engine);
            if (waitTime < 0)
            {
                throw new Exception("Wait time is less than 0 ms.");
            }
            else if (waitTime < 100)
            {
                waitTime = 100;
            }

            if (textToSend == "{WIN_KEY}")
            {
                User32Functions.KeyDown(Keys.LWin);
                User32Functions.KeyUp(Keys.LWin);

            }
            else if (textToSend.Contains("{WIN_KEY+"))
            {
                User32Functions.KeyDown(Keys.LWin);
                var remainingText = textToSend.Replace("{WIN_KEY+", "").Replace("}","");

                foreach (var c in remainingText)
                {
                    Keys key = (Keys)Enum.Parse(typeof(Keys), c.ToString());
                    User32Functions.KeyDown(key);
                }

                User32Functions.KeyUp(Keys.LWin);

                foreach (var c in remainingText)
                {
                    Keys key = (Keys)Enum.Parse(typeof(Keys), c.ToString());
                    User32Functions.KeyUp(key);
                }
            }
            else
            {
                SendKeys.SendWait(textToSend);
            }

            System.Threading.Thread.Sleep(waitTime);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);


            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            var WindowNameControl = CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_WindowName", this, WindowNameControl, editor));
            RenderedControls.Add(WindowNameControl);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

            //taskt.UI.CustomControls.CommandItemControl helperControl = new taskt.UI.CustomControls.CommandItemControl();
            //CommandItemControl encryptLink = CommandControls.CreateUIHelper();
            //encryptLink.CommandDisplay = "Encrypt Text";
            //encryptLink.Click += (sender, e) => lnkEncryptText_Click(sender, e);

            var textInputGroup = CommandControls.CreateDefaultInputGroupFor("v_TextToSend", this, editor);
            RenderedControls.AddRange(textInputGroup);

            //InputText = (TextBox)textInputGroup[2];
            InputText = (TextBox)textInputGroup.Find(t => t is TextBox);

            //helperControl.ForeColor = Color.White;
            //helperControl.CommandDisplay = "Encrypt Text";
            //helperControl.Click += HelperControl_Click;
            //RenderedControls.Add(helperControl);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_EncryptionOption", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_WaitTime", this, editor));

            return RenderedControls;

        }

        private void lnkEncryptText_Click(object sender, EventArgs e)
        {
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
            using(var fm = new taskt.UI.Forms.Supplement_Forms.frmKeysBuilder())
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    InputText.Text = fm.Result;
                }
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Send '" + v_TextToSend + "' to '" + v_WindowName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_WindowName))
            {
                this.validationResult += "Window name is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }

        public override void convertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToIntermediateWindowName");
            cnv.Add("v_TextToSend", "convertToIntermediateVariableParser");
            convertToIntermediate(settings, cnv, variables);
        }

        public override void convertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>();
            cnv.Add("v_WindowName", "convertToRawWindowName");
            convertToRaw(settings, cnv);
        }
    }
}