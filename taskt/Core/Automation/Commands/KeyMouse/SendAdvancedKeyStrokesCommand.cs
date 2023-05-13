using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.User32;
using taskt.UI.Forms;
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
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SendAdvancedKeyStrokesCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowNameControls), nameof(WindowNameControls.v_WindowName))]
        public string v_WindowName { get; set; }

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

        [XmlElement]
        [PropertyVirtualProperty(nameof(SelectionControls), nameof(SelectionControls.v_YesNoComboBox))]
        [PropertyDescription("Return all keys to 'UP' position after execution")]
        [PropertyIsOptional(true, "No")]
        public string v_KeyUpDefault { get; set; }

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
        public string v_WaitAfterKeyEnter { get; set; }

        public SendAdvancedKeyStrokesCommand()
        {
            //this.CommandName = "SendAdvancedKeyStrokesCommand";
            //this.SelectionName = "Send Advanced Keystrokes";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetWindow = v_WindowName.ConvertToUserVariable(sender);

            if (targetWindow != engine.engineSettings.CurrentWindowKeyword)
            {
                var activateWindow = new ActivateWindowCommand
                {
                    v_WindowName = v_WindowName,
                    v_SearchMethod = v_SearchMethod,
                    v_MatchMethod = v_MatchMethod,
                    v_TargetWindowIndex = v_TargetWindowIndex,
                    v_WaitTime = v_WaitForWindow
                };
                activateWindow.RunCommand(engine);
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
                        //User32Functions.KeyDown(oemKeyName);
                        //User32Functions.KeyUp(oemKeyName);
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
                        //User32Functions.KeyDown(oemKeyName);
                        KeyMouseControls.KeyDown(oemKeyName);

                        //track via keys down list
                        if (!keysDown.Contains(oemKeyName))
                        {
                            keysDown.Add(oemKeyName);
                        }
                    
                        break;
                    case "Key Up":
                        //simulate up
                        //User32Functions.KeyUp(oemKeyName);
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
            if (this.GetYesNoSelectionValue(nameof(v_KeyUpDefault), engine))
            {
                foreach (var key in keysDown)
                {
                    //User32Functions.KeyUp(key);
                    KeyMouseControls.KeyUp(key);
                }
            }
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowNameControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        public override void AfterShown()
        {
            //var dgv = (DataGridView)ControlsList[nameof(v_KeyActions)];
            var dgv = ControlsList.GetPropertyControl<DataGridView>(nameof(v_KeyActions));

            var column = (DataGridViewComboBoxColumn)dgv.Columns[0];
            column.DataSource = Common.GetAvailableKeys();
        }

        public override bool IsValidate(frmCommandEditor editor)
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
    }
}