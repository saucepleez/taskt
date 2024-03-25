﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Key/Mouse Commands")]
    [Attributes.ClassAttributes.SubGruop("Key")]
    [Attributes.ClassAttributes.CommandSettings("Send Advanced Keystrokes")]
    [Attributes.ClassAttributes.Description("Sends advanced keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send advanced keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'User32' method to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SendAdvancedKeyStrokesCommand : ScriptCommand, IHaveDataTableElements
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlElement]
        [PropertyDescription("Keys and Action Type")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true, 400, 250)]
        [PropertyDataGridViewColumnSettings("Key", "Key", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.ComboBox)]
        [PropertyDataGridViewColumnSettings("Action", "Action", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.ComboBox, "Key Press (Down + Up)\nKey Down\nKey Up")]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public DataTable v_KeyActions { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Return all keys to 'UP' position after execution")]
        [PropertyIsOptional(true, "Yes")]
        public string v_KeyUpDefault { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_CompareMethod))]
        public string v_CompareMethod { get; set; }
        
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_WaitTimeAfterKeyEnter))]
        public string v_WaitAfterKeyEnter { get; set; }

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

        public SendAdvancedKeyStrokesCommand()
        {
            //this.CommandName = "SendAdvancedKeyStrokesCommand";
            //this.SelectionName = "Send Advanced Keystrokes";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            WindowControls.WindowAction(this, engine,
                new Action<List<(IntPtr, string)>>(wins =>
                {
                    //var whnd = wins[0].Item1;
                    //WindowControls.ActivateWindow(whnd);
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

                    //track all keys down
                    var keysDown = new List<Keys>();

                    //run each selected item
                    foreach (DataRow rw in v_KeyActions.Rows)
                    {
                        //get key name
                        var keyName = rw.Field<string>("Key");

                        //get key action
                        var action = rw.Field<string>("Action");

                        //parse OEM key name
                        string oemKeyString = keyName.Split('[', ']')[1];

                        var oemKeyName = (Keys)Enum.Parse(typeof(Keys), oemKeyString);


                        //"Key Press (Down + Up)", "Key Down", "Key Up"
                        switch (action)
                        {
                            case "Key Press (Down + Up)":
                                //simulate press
                                KeyMouseControls.KeyDown(oemKeyName);
                                KeyMouseControls.KeyUp(oemKeyName);

                                //key returned to UP position so remove if we added it to the keys down list
                                if (keysDown.Contains(oemKeyName))
                                {
                                    keysDown.Remove(oemKeyName);
                                }
                                break;

                            case "Key Down":
                                //simulate down
                                KeyMouseControls.KeyDown(oemKeyName);

                                //track via keys down list
                                if (!keysDown.Contains(oemKeyName))
                                {
                                    keysDown.Add(oemKeyName);
                                }
                                break;

                            case "Key Up":
                                //simulate up
                                KeyMouseControls.KeyUp(oemKeyName);

                                //remove from key down
                                if (keysDown.Contains(oemKeyName))
                                {
                                    keysDown.Remove(oemKeyName);
                                }
                                break;

                            default:
                                break;
                        }
                    }

                    //return key to up position if requested
                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_KeyUpDefault), engine))
                    {
                        foreach (var key in keysDown)
                        {
                            KeyMouseControls.KeyUp(key);
                        }
                    }
                })
            );

            //var targetWindow = v_WindowName.ExpandValueOrUserVariable(engine);

            //if (targetWindow != engine.engineSettings.CurrentWindowKeyword)
            //{
            //    var activateWindow = new ActivateWindowCommand
            //    {
            //        v_WindowName = v_WindowName,
            //        v_SearchMethod = v_SearchMethod,
            //        v_MatchMethod = v_MatchMethod,
            //        v_TargetWindowIndex = v_TargetWindowIndex,
            //        v_WaitTime = v_WaitForWindow
            //    };
            //    activateWindow.RunCommand(engine);
            //}

            ////track all keys down
            //var keysDown = new List<Keys>();

            ////run each selected item
            //foreach (DataRow rw in v_KeyActions.Rows)
            //{
            //    //get key name
            //    var keyName = rw.Field<string>("Key");

            //    //get key action
            //    var action = rw.Field<string>("Action");

            //    //parse OEM key name
            //    string oemKeyString = keyName.Split('[', ']')[1];

            //    var oemKeyName = (Keys)Enum.Parse(typeof(Keys), oemKeyString);

            
            //    //"Key Press (Down + Up)", "Key Down", "Key Up"
            //    switch (action)
            //    {
            //        case "Key Press (Down + Up)":
            //            //simulate press
            //            KeyMouseControls.KeyDown(oemKeyName);
            //            KeyMouseControls.KeyUp(oemKeyName);
                        
            //            //key returned to UP position so remove if we added it to the keys down list
            //            if (keysDown.Contains(oemKeyName))
            //            {
            //                keysDown.Remove(oemKeyName);
            //            }
            //            break;

            //        case "Key Down":
            //            //simulate down
            //            KeyMouseControls.KeyDown(oemKeyName);

            //            //track via keys down list
            //            if (!keysDown.Contains(oemKeyName))
            //            {
            //                keysDown.Add(oemKeyName);
            //            }
            //            break;

            //        case "Key Up":
            //            //simulate up
            //            KeyMouseControls.KeyUp(oemKeyName);

            //            //remove from key down
            //            if (keysDown.Contains(oemKeyName))
            //            {
            //                keysDown.Remove(oemKeyName);
            //            }
            //            break;

            //        default:
            //            break;
            //    }
            //}

            ////return key to up position if requested
            //if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_KeyUpDefault), engine))
            //{
            //    foreach (var key in keysDown)
            //    {
            //        KeyMouseControls.KeyUp(key);
            //    }
            //}
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
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