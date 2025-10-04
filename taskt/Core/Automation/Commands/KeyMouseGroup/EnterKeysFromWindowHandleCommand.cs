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
    [Attributes.ClassAttributes.CommandSettings("Enter Keys From Window Handle")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class EnterKeysFromWindowHandleCommand : AWindowHandleActionCommands, IEnterKeysProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_TextToSend))]
        [PropertyParameterOrder(5100)]
        public string v_TextToSend { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_EncryptionOption))]
        [PropertyParameterOrder(5200)]
        public string v_EncryptionOption { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_UseClipBoard))]
        [PropertyParameterOrder(5300)]
        public string v_UseClipBoard { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_WaitTimeAfterKeyEnter))]
        [PropertyParameterOrder(8010)]
        public string v_WaitTimeAfterKeyEnter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_ActivateCurrentWindow))]
        [PropertyParameterOrder(8020)]
        public string v_ActivateCurrentWindow { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_ClearClipboardAfterPaste))]
        [PropertyParameterOrder(9000)]
        public string v_ClearClipboardAfterPaste { get; set; }
        
        [XmlAttribute]
        [PropertyIsOptional(true, "Yes")]
        [PropertyFirstValue("Yes")]
        public override string v_ActivateBeforeAction { get; set; }

        public EnterKeysFromWindowHandleCommand()
        {
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

            this.WindowHandleActionBeforeWaitActivate(engine, new Action<IntPtr>((whnd) =>
            {
                if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ActivateBeforeAction), engine))
                {
                    if (this.IsCurrentWindowHandleKeyword(engine))
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

                var textToSend = v_TextToSend.ExpandValueOrUserVariable(engine);

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
    }
}