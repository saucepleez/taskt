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
    [Attributes.ClassAttributes.CommandSettings("Enter Keys")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class EnterKeysCommand : AOneWindowNameActionCommands, IEnterKeysProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowName))]
        //public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_TextToSend))]
        //[PropertyDescription("Text or Keys to Send")]
        //[PropertyCustomUIHelper("Keys Builder", nameof(lnkKeysBulider_Click))]
        //[PropertyCustomUIHelper("Encrypt Text", nameof(lnkEncryptText_Click))]
        //[InputSpecification("Text to Send", true)]
        //[PropertyDetailSampleUsage("**Hello, World!**", PropertyDetailSampleUsage.ValueType.Value, "Text")]
        //[PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text")]
        //[PropertyDetailSampleUsage("**^s**", "Specify **Ctrl+S** for Enter Keys")]
        //[PropertyDetailSampleUsage("**{WIN_KEY}**", "Specify **Windows Key** for Enter Keys")]
        //[PropertyDetailSampleUsage("**{WIN_KEY+R}**", "Specify **Windows Key** and **R** for Enter Keys")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyIntermediateConvert(nameof(IntermediateControls.ConvertToIntermediate_CheckedVariableMarker), "")]
        //[PropertyDisplayText(true, "Text")]
        [PropertyParameterOrder(5100)]
        public string v_TextToSend { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_EncryptionOption))]
        //[PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        //[PropertyDescription("Text is Encrypted")]
        //[PropertyIsOptional(true, "No")]
        //[PropertyDisplayText(false, "Encrypted")]
        [PropertyParameterOrder(5200)]
        public string v_EncryptionOption { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_UseClipBoard))]
        //[PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        //[PropertyDescription("Use Paste from Clipboard")]
        //[PropertyIsOptional(true, "No")]
        //[Remarks("When entering keys in combination with the Ctrl key, etc., It will NOT work correctly.")]
        //[PropertyDisplayText(false, "Use Clipboard")]
        [PropertyParameterOrder(5300)]
        public string v_UseClipBoard { get; set; }

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
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_WaitTimeAfterKeyEnter))]
        //[PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_WaitTimeAfterKeyEnter))]
        [PropertyParameterOrder(8010)]
        public string v_WaitTimeAfterKeyEnter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_ActivateCurrentWindow))]
        //[PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        //[PropertyDescription("Try Activate Window, when Specifiy Current Window Variable")]
        //[PropertyIsOptional(true, "No")]
        [PropertyParameterOrder(8020)]
        public string v_ActivateCurrentWindow { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowNameResult))]
        //public string v_NameResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        //public string v_HandleResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_ClearClipboardAfterPaste))]
        //[PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        //[PropertyDescription("Clear Clipboard After Paste")]
        //[PropertyIsOptional(true, "No")]
        //[PropertyValidationRule("Clear Clipboard", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "Clear Clipboard after Paste")]
        [PropertyParameterOrder(9000)]
        public string v_ClearClipboardAfterPaste { get; set; }
        
        [XmlAttribute]
        [PropertyIsOptional(true, "Yes")]
        [PropertyFirstValue("Yes")]
        public override string v_ActivateBeforeAction { get; set; }

        public EnterKeysCommand()
        {
            //this.CommandName = "SendKeysCommand";
            //this.SelectionName = "Send Keystrokes";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_EncryptionOption = "Not Encrypted";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            // activate window
            void ActivateWindowProcess(IntPtr h)
            {
                var activateWindow = new ActivateWindowByWindowHandleCommand()
                {
                    v_WindowHandle = h.ToString(),
                };
                activateWindow.RunCommand(engine);
            }

            this.WindowNameAction(engine, new Action<IntPtr, string>((whnd, name) =>
            {
                if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ActivateBeforeAction), engine))
                {
                    if (VariableNameControls.GetWrappedVariableName(Engine.SystemVariables.Window_CurrentWindowName.VariableName, engine) == v_WindowName)
                    {
                        if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ActivateCurrentWindow), engine))
                        {
                            ActivateWindowProcess(whnd);
                        }
                    }
                    else
                    {
                        ActivateWindowProcess(whnd);
                    }
                }
                this.WaitAfterFindWindowProcess(engine);

                var textToSend = v_TextToSend.ExpandValueOrUserVariable(engine);

                //var encryptOption = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_EncryptionOption), engine);
                //if (encryptOption == "encrypted")
                //{
                //    textToSend = EncryptionServices.DecryptString(textToSend, "TASKT");
                //}
                if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_EncryptionOption), engine))
                {
                    textToSend = EncryptionServices.DecryptString(textToSend, "TASKT");
                }

                if (textToSend == "{WIN_KEY}")
                {
                    KeyMouseControls.KeyDown(Keys.LWin);
                    KeyMouseControls.KeyUp(Keys.LWin);
                }
                else if (textToSend.StartsWith("{WIN_KEY+") && textToSend.EndsWith("}"))
                {
                    KeyMouseControls.KeyDown(Keys.LWin);
                    var remainingText = textToSend.Replace("{WIN_KEY+", "").Replace("}", "");

                    foreach (var c in remainingText)
                    {
                        Keys key = (Keys)Enum.Parse(typeof(Keys), c.ToString());
                        KeyMouseControls.KeyDown(key);
                    }

                    KeyMouseControls.KeyUp(Keys.LWin);

                    foreach (var c in remainingText)
                    {
                        Keys key = (Keys)Enum.Parse(typeof(Keys), c.ToString());
                        KeyMouseControls.KeyUp(key);
                    }
                }
                else
                {
                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_UseClipBoard), engine))
                    {
                        ClipboardControls.SetClipboardText(textToSend);
                        textToSend = "^v";  // Ctrl+V
                    }
                    SendKeys.SendWait(textToSend);
                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ClearClipboardAfterPaste), engine))
                    {
                        ClipboardControls.ClearClipboard();
                    }
                }

                var waitTime = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTimeAfterKeyEnter), engine);
                System.Threading.Thread.Sleep(waitTime);
            }));
        }

        //private void lnkEncryptText_Click(object sender, EventArgs e)
        //{
        //    var inputText = ControlsList.GetPropertyControl<TextBox>(nameof(v_TextToSend));

        //    if (string.IsNullOrEmpty(inputText.Text))
        //    {
        //        MessageBox.Show("Text to send is empty.", "Notice");
        //        return;
        //    }

        //    var encrypted = EncryptionServices.EncryptString(inputText.Text, "TASKT");
        //    this.v_EncryptionOption = "Encrypted";

        //    inputText.Text = encrypted;
        //}

        //private void lnkKeysBulider_Click(object sender, EventArgs e)
        //{
        //    using (var fm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmKeysBuilder())
        //    {
        //        if (fm.ShowDialog(((Control)sender).FindForm()) == DialogResult.OK)
        //        {
        //            var inputText = ControlsList.GetPropertyControl<TextBox>(nameof(v_TextToSend));
        //            inputText.Text = fm.Result;
        //        }
        //    }
        //}
    }
}