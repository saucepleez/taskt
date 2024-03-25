﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml.Serialization;
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
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class EnterKeysCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text or Keys to Send.")]
        [PropertyCustomUIHelper("Keys Builder", nameof(lnkKeysBulider_Click))]
        [PropertyCustomUIHelper("Encrypt Text", nameof(lnkEncryptText_Click))]
        [InputSpecification("Text to Send", true)]
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
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_CompareMethod))]
        public string v_CompareMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_MatchMethod_Single))]
        [PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        public string v_MatchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_TargetWindowIndex))]
        public string v_TargetWindowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        public string v_WaitTimeForWindow { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_WaitTimeAfterKeyEnter))]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Try Activate Window, when Specifiy Current Window Variable")]
        [PropertyIsOptional(true, "No")]
        public string v_ActivateCurrentWindow { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowNameResult))]
        public string v_NameResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        public string v_HandleResult { get; set; }

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
            //var targetWindow = v_WindowName.ExpandValueOrUserVariable(engine);
            //if (targetWindow != engine.engineSettings.CurrentWindowKeyword)
            //{
            //    var activateWindow = new ActivateWindowCommand
            //    {
            //        v_WindowName = v_WindowName,
            //        v_SearchMethod = v_SearchMethod,
            //        v_MatchMethod= v_MatchMethod,
            //        v_TargetWindowIndex = v_TargetWindowIndex,
            //        v_WaitTime = v_WaitForWindow
            //    };
            //    activateWindow.RunCommand(engine);
            //}

            //var textToSend = v_TextToSend.ExpandValueOrUserVariable(engine);

            //var encryptOption = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_EncryptionOption), engine);
            //if (encryptOption == "encrypted")
            //{
            //    textToSend = EncryptionServices.DecryptString(textToSend, "TASKT");
            //}

            //if (textToSend == "{WIN_KEY}")
            //{
            //    KeyMouseControls.KeyDown(Keys.LWin);
            //    KeyMouseControls.KeyUp(Keys.LWin);
            //}
            //else if (textToSend.StartsWith("{WIN_KEY+") && textToSend.EndsWith("}"))
            //{
            //    KeyMouseControls.KeyDown(Keys.LWin);
            //    var remainingText = textToSend.Replace("{WIN_KEY+", "").Replace("}","");

            //    foreach (var c in remainingText)
            //    {
            //        Keys key = (Keys)Enum.Parse(typeof(Keys), c.ToString());
            //        KeyMouseControls.KeyDown(key);
            //    }

            //    KeyMouseControls.KeyUp(Keys.LWin);

            //    foreach (var c in remainingText)
            //    {
            //        Keys key = (Keys)Enum.Parse(typeof(Keys), c.ToString());
            //        KeyMouseControls.KeyUp(key);
            //    }
            //}
            //else
            //{
            //    SendKeys.SendWait(textToSend);
            //}

            //var waitTime = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTime), engine);
            //System.Threading.Thread.Sleep(waitTime);

            WindowControls.WindowAction(this, engine,
                new Action<List<(IntPtr, string)>>(wins =>
                {
                    //if (WindowControls.GetActiveWindowHandle() != wins[0].Item1)
                    //{
                    //    // DBG
                    //    Console.WriteLine($"#### strange win-handle. cur: {WindowControls.GetActiveWindowHandle()}, arg: {wins[0].Item1}");
                    //    WindowControls.ActivateWindow(wins[0].Item1);
                    //}
                    //else
                    //{
                    //    // DBG
                    //    Console.WriteLine($"#### same win-handle. key: {v_TextToSend}");
                    //}
                    //if (VariableNameControls.GetWrappedVariableName(Engine.SystemVariables.Window_CurrentWindowName.VariableName, engine) == v_WindowName)
                    //{
                    //    // DBG
                    //    Console.WriteLine($"#### same win-handle. key: {v_TextToSend}");
                    //}
                    //else
                    //{
                    //    // DBG
                    //    Console.WriteLine($"#### strange win-handle. cur: {WindowControls.GetActiveWindowHandle()}, arg: {wins[0].Item1}");
                    //    WindowControls.ActivateWindow(wins[0].Item1);
                    //}

                    if (VariableNameControls.GetWrappedVariableName(Engine.SystemVariables.Window_CurrentWindowName.VariableName, engine) == v_WindowName)
                    {
                        if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ActivateCurrentWindow), engine))
                        {
                            WindowControls.ActivateWindow(wins[0].Item1);
                        }
                    }
                    else
                    {
                        WindowControls.ActivateWindow(wins[0].Item1);
                    }

                    var textToSend = v_TextToSend.ExpandValueOrUserVariable(engine);

                    var encryptOption = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_EncryptionOption), engine);
                    if (encryptOption == "encrypted")
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
                        SendKeys.SendWait(textToSend);
                    }

                    var waitTime = this.ExpandValueOrUserVariableAsInteger(nameof(v_WaitTime), engine);
                    System.Threading.Thread.Sleep(waitTime);
                })
            );
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        private void lnkEncryptText_Click(object sender, EventArgs e)
        {
            var inputText = ControlsList.GetPropertyControl<TextBox>(nameof(v_TextToSend));

            if (string.IsNullOrEmpty(inputText.Text))
            {
                MessageBox.Show("Text to send is empty.", "Notice");
                return;
            }

            var encrypted = EncryptionServices.EncryptString(inputText.Text, "TASKT");
            this.v_EncryptionOption = "Encrypted";

            inputText.Text = encrypted;
        }

        private void lnkKeysBulider_Click(object sender, EventArgs e)
        {
            using (var fm = new UI.Forms.ScriptBuilder.CommandEditor.Supplemental.frmKeysBuilder())
            {
                if (fm.ShowDialog(((Control)sender).FindForm()) == DialogResult.OK)
                {
                    var inputText = ControlsList.GetPropertyControl<TextBox>(nameof(v_TextToSend));
                    inputText.Text = fm.Result;
                }
            }
        }
    }
}