using System;
using System.Collections.Generic;
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
    [Attributes.ClassAttributes.CommandSettings("Enter Keys")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class EnterKeysCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the Window name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        //[SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindowName}}}**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsWindowNamesList(true)]
        //[PropertyShowSampleUsageInDescription(true)]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Window name search method")]
        //[InputSpecification("")]
        //[PropertyUISelectionOption("Contains")]
        //[PropertyUISelectionOption("Starts with")]
        //[PropertyUISelectionOption("Ends with")]
        //[PropertyUISelectionOption("Exact match")]
        //[SampleUsage("**Contains** or **Starts with** or **Ends with** or **Exact match**")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "Contains")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_CompareMethod))]
        public string v_SearchMethod { get; set; }

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
        //[PropertyDescription("Please specify waiting time after keystrokes (ms)")]
        //[InputSpecification("")]
        //[SampleUsage("**500** or **{{{vWaitTime}}}")]
        //[Remarks("If less than 100 is specified, it will be 100")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyIsOptional(true, "500")]
        //[PropertyFirstValue("500")]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_WaitTimeAfterKeyEnter))]
        public string v_WaitTime { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private TextBox InputText;

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

            //string targetWindowName = v_WindowName.ConvertToUserVariable(sender);
            //var searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(searchMethod))
            //{
            //    searchMethod = "Contains";
            //}
            //if (targetWindowName != ((Engine.AutomationEngineInstance)sender).engineSettings.CurrentWindowKeyword)
            //{
            //    ActivateWindowCommand activateWindow = new ActivateWindowCommand
            //    {
            //        v_WindowName = targetWindowName,
            //        v_SearchMethod = searchMethod
            //    };
            //    activateWindow.RunCommand(sender);
            //}
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
            //string EncryptionOption = v_EncryptionOption.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(EncryptionOption))
            //{
            //    EncryptionOption = "Not Encrypted";
            //}
            //if (EncryptionOption == "Encrypted")
            //{
            //    textToSend = EncryptionServices.DecryptString(textToSend, "TASKT");
            //}
            var encryptOption = this.GetUISelectionValue(nameof(v_EncryptionOption), engine);
            if (encryptOption == "encrypted")
            {
                textToSend = EncryptionServices.DecryptString(textToSend, "TASKT");
            }

            //if (String.IsNullOrEmpty(v_WaitTime))
            //{
            //    v_WaitTime = "500";
            //}
            //int waitTime = v_WaitTime.ConvertToUserVariableAsInteger("Wait Time", engine);
            //if (waitTime < 0)
            //{
            //    throw new Exception("Wait time is less than 0 ms.");
            //}
            //else if (waitTime < 100)
            //{
            //    waitTime = 100;
            //}

            if (textToSend == "{WIN_KEY}")
            {
                User32Functions.KeyDown(Keys.LWin);
                User32Functions.KeyUp(Keys.LWin);
            }
            else if (textToSend.StartsWith("{WIN_KEY+") && textToSend.EndsWith("}"))
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

            var waitTime = this.ConvertToUserVariableAsInteger(nameof(v_WaitTime), engine);
            System.Threading.Thread.Sleep(waitTime);
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        private void lnkEncryptText_Click(object sender, EventArgs e)
        {
            var InputText = (TextBox)ControlsList[nameof(v_TextToSend)];

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
                    var InputText = (TextBox)ControlsList[nameof(v_TextToSend)];
                    InputText.Text = fm.Result;
                }
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);


        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WindowName", this));
        //    var WindowNameControl = CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_WindowName", this, WindowNameControl, editor));
        //    RenderedControls.Add(WindowNameControl);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

        //    //taskt.UI.CustomControls.CommandItemControl helperControl = new taskt.UI.CustomControls.CommandItemControl();
        //    //CommandItemControl encryptLink = CommandControls.CreateUIHelper();
        //    //encryptLink.CommandDisplay = "Encrypt Text";
        //    //encryptLink.Click += (sender, e) => lnkEncryptText_Click(sender, e);

        //    var textInputGroup = CommandControls.CreateDefaultInputGroupFor("v_TextToSend", this, editor);
        //    RenderedControls.AddRange(textInputGroup);

        //    //InputText = (TextBox)textInputGroup[2];
        //    InputText = (TextBox)textInputGroup.Find(t => t is TextBox);

        //    //helperControl.ForeColor = Color.White;
        //    //helperControl.CommandDisplay = "Encrypt Text";
        //    //helperControl.Click += HelperControl_Click;
        //    //RenderedControls.Add(helperControl);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_EncryptionOption", this, editor));

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_WaitTime", this, editor));

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Send '" + v_TextToSend + "' to '" + v_WindowName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_WindowName))
        //    {
        //        this.validationResult += "Window name is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}

        public override void ConvertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            var cnv = new Dictionary<string, string>
            {
                { nameof(v_WindowName), "convertToIntermediateWindowName" },
                { nameof(v_TextToSend), "convertToIntermediateVariableParser" }
            };
            ConvertToIntermediate(settings, cnv, variables);
        }

        public override void ConvertToRaw(EngineSettings settings)
        {
            var cnv = new Dictionary<string, string>
            {
                { nameof(v_WindowName), "convertToRawWindowName" }
            };
            ConvertToRaw(settings, cnv);
        }
    }
}