using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.KeyMouseGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Key/Mouse")]
    [Attributes.ClassAttributes.SubGruop("Key")]
    [Attributes.ClassAttributes.CommandSettings("Send Advanced Keystrokes From Window Handle")]
    [Attributes.ClassAttributes.Description("Sends advanced keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send advanced keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'User32' method to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SendAdvancedKeyStrokesFromWindowHandleCommand : AWindowHandleActionCommands, ISendAdvancedKeyStrokesProperties
    {
        [XmlElement]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_KeyActions))]
        [PropertyParameterOrder(5010)]
        public DataTable v_KeyActions { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_KeyUpDefault))]
        [PropertyParameterOrder(5020)]
        public string v_KeyUpDefault { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_WaitTimeAfterKeyEnter))]
        [PropertyParameterOrder(8010)]
        public string v_WaitTimeAfterKeyEnter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_KeyMouseControls), nameof(VP_KeyMouseControls.v_ActivateCurrentWindow))]
        [PropertyParameterOrder(8020)]
        public string v_ActivateCurrentWindow { get; set; }

        [XmlAttribute]
        [PropertyIsOptional(true, "Yes")]
        [PropertyFirstValue("Yes")]
        public override string v_ActivateBeforeAction { get; set; }

        public SendAdvancedKeyStrokesFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
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
                this.WaitAfterFindWindowProcess(engine);

                // track all keys down
                var keysDown = new List<Keys>();

                // run each selected item
                foreach (DataRow rw in v_KeyActions.Rows)
                {
                    // get key name
                    var keyName = rw.Field<string>("Key");

                    // get key action
                    var action = rw.Field<string>("Action");

                    // parse OEM key name
                    string oemKeyString = keyName.Split('[', ']')[1];

                    var oemKeyName = (Keys)Enum.Parse(typeof(Keys), oemKeyString);

                    // "Key Press (Down + Up)", "Key Down", "Key Up"
                    switch (action)
                    {
                        case "Key Press (Down + Up)":
                            // simulate press
                            KeyMouseControls.KeyDown(oemKeyName);
                            KeyMouseControls.KeyUp(oemKeyName);

                            // key returned to UP position so remove if we added it to the keys down list
                            if (keysDown.Contains(oemKeyName))
                            {
                                keysDown.Remove(oemKeyName);
                            }
                            break;

                        case "Key Down":
                            // simulate down
                            KeyMouseControls.KeyDown(oemKeyName);

                            // track via keys down list
                            if (!keysDown.Contains(oemKeyName))
                            {
                                keysDown.Add(oemKeyName);
                            }
                            break;

                        case "Key Up":
                            // simulate up
                            KeyMouseControls.KeyUp(oemKeyName);

                            // remove from key down
                            if (keysDown.Contains(oemKeyName))
                            {
                                keysDown.Remove(oemKeyName);
                            }
                            break;

                        default:
                            break;
                    }
                }

                // return key to up position if requested
                if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_KeyUpDefault), engine))
                {
                    foreach (var key in keysDown)
                    {
                        KeyMouseControls.KeyUp(key);
                    }
                }
            }));

            //this.WindowNameAction(engine, new Action<IntPtr, string>((whnd, name) =>
            //{
            //    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ActivateBeforeAction), engine))
            //    {
            //        if (VariableNameControls.GetWrappedVariableName(Engine.SystemVariables.Window_CurrentWindowName.VariableName, engine) == v_WindowName)
            //        {
            //            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ActivateCurrentWindow), engine))
            //            {
            //                ActivateWindowProcess(whnd);
            //            }
            //        }
            //        else
            //        {
            //            ActivateWindowProcess(whnd);
            //        }
            //    }
            //    this.WaitAfterFindWindowProcess(engine);

            //    // track all keys down
            //    var keysDown = new List<Keys>();

            //    // run each selected item
            //    foreach (DataRow rw in v_KeyActions.Rows)
            //    {
            //        // get key name
            //        var keyName = rw.Field<string>("Key");

            //        // get key action
            //        var action = rw.Field<string>("Action");

            //        // parse OEM key name
            //        string oemKeyString = keyName.Split('[', ']')[1];

            //        var oemKeyName = (Keys)Enum.Parse(typeof(Keys), oemKeyString);


            //        // "Key Press (Down + Up)", "Key Down", "Key Up"
            //        switch (action)
            //        {
            //            case "Key Press (Down + Up)":
            //                // simulate press
            //                KeyMouseControls.KeyDown(oemKeyName);
            //                KeyMouseControls.KeyUp(oemKeyName);

            //                // key returned to UP position so remove if we added it to the keys down list
            //                if (keysDown.Contains(oemKeyName))
            //                {
            //                    keysDown.Remove(oemKeyName);
            //                }
            //                break;

            //            case "Key Down":
            //                // simulate down
            //                KeyMouseControls.KeyDown(oemKeyName);

            //                // track via keys down list
            //                if (!keysDown.Contains(oemKeyName))
            //                {
            //                    keysDown.Add(oemKeyName);
            //                }
            //                break;

            //            case "Key Up":
            //                // simulate up
            //                KeyMouseControls.KeyUp(oemKeyName);

            //                // remove from key down
            //                if (keysDown.Contains(oemKeyName))
            //                {
            //                    keysDown.Remove(oemKeyName);
            //                }
            //                break;

            //            default:
            //                break;
            //        }
            //    }

            //    // return key to up position if requested
            //    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_KeyUpDefault), engine))
            //    {
            //        foreach (var key in keysDown)
            //        {
            //            KeyMouseControls.KeyUp(key);
            //        }
            //    }
            //}));
        }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            var dgv = ControlsList.GetPropertyControl<DataGridView>(nameof(v_KeyActions));

            var column = (DataGridViewComboBoxColumn)dgv.Columns[0];
            column.DataSource = KeyMouseControls.KeysList;
        }

        public override bool IsValidate(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            base.IsValidate(editor);

            for (int i = 0; i < v_KeyActions.Rows.Count; i++)
            {
                var row = v_KeyActions.Rows[i];
                if (string.IsNullOrEmpty(row.Field<string>("Key")))
                {
                    this.validationResult += "Selected Key #" + (i + 1) + " is empty.\n";
                    this.IsValid = false;
                }
                if (string.IsNullOrEmpty(row.Field<string>("Action")))
                {
                    this.validationResult += "Selected Action #" + (i + 1) + " is empty.\n";
                    this.IsValid = false;
                }
            }

            return this.IsValid;
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();

            var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_KeyActions));
            DataTableControls.BeforeValidate(dgv, v_KeyActions);
        }
    }
}