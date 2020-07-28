using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Common;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{

    [Serializable]
    [Group("Input Commands")]
    [Description("Sends advanced keystrokes to a targeted window")]
    [UsesDescription("Use this command when you want to send advanced keystroke inputs to a window.")]
    [ImplementationDescription("This command implements 'User32' method to achieve automation.")]
    public class SendAdvancedKeystrokesCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the Window name")]
        [InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [SampleUsage("**Untitled - Notepad**")]
        [Remarks("")]
        public string v_WindowName { get; set; }

        [PropertyDescription("Set Keys and Parameters")]
        [InputSpecification("Define the parameters for the actions.")]
        [SampleUsage("n/a")]
        [Remarks("Select Valid Options from the dropdowns")]
        public DataTable v_KeyActions { get; set; }

        [XmlElement]   
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [PropertyDescription("Optional - Return all keys to 'UP' position after execution")]
        [InputSpecification("Select either 'Yes' or 'No' as to a preference")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_KeyUpDefault { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView KeystrokeGridHelper;

        public SendAdvancedKeystrokesCommand()
        {
            CommandName = "SendAdvancedKeystrokesCommand";
            SelectionName = "Send Advanced Keystrokes";
            CommandEnabled = true;
            CustomRendering = true;
            v_KeyActions = new DataTable();
            v_KeyActions.Columns.Add("Key");
            v_KeyActions.Columns.Add("Action");
            v_KeyActions.TableName = "SendAdvancedKeyStrokesCommand" + DateTime.Now.ToString("MMddyy.hhmmss");
            v_KeyUpDefault = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //activate anything except current window
            if (v_WindowName != "Current Window")
            {
                ActivateWindowCommand activateWindow = new ActivateWindowCommand
                {
                    v_WindowName = v_WindowName
                };
                activateWindow.RunCommand(sender);
            }


            //track all keys down
            var keysDown = new List<System.Windows.Forms.Keys>();

            //run each selected item
            foreach (DataRow rw in v_KeyActions.Rows)
            {
                //get key name
                var keyName = rw.Field<string>("Key");

                //get key action
                var action = rw.Field<string>("Action");

                //parse OEM key name
                string oemKeyString = keyName.Split('[', ']')[1];

                var oemKeyName = (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), oemKeyString);

            
                //"Key Press (Down + Up)", "Key Down", "Key Up"
                switch (action)
                {
                    case "Key Press (Down + Up)":
                        //simulate press
                        User32Functions.KeyDown(oemKeyName);
                        User32Functions.KeyUp(oemKeyName);
                        
                        //key returned to UP position so remove if we added it to the keys down list
                        if (keysDown.Contains(oemKeyName))
                        {
                            keysDown.Remove(oemKeyName);
                        }
                        break;
                    case "Key Down":
                        //simulate down
                        User32Functions.KeyDown(oemKeyName);

                        //track via keys down list
                        if (!keysDown.Contains(oemKeyName))
                        {
                            keysDown.Add(oemKeyName);
                        }
                    
                        break;
                    case "Key Up":
                        //simulate up
                        User32Functions.KeyUp(oemKeyName);

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
            if (v_KeyUpDefault.ConvertToUserVariable(engine) == "Yes")
            {
                foreach (var key in keysDown)
                {
                    User32Functions.KeyUp(key);
                }
            }
        
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            var WindowNameControl = UI.CustomControls.CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames();
            RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            RenderedControls.Add(WindowNameControl);

            KeystrokeGridHelper = new DataGridView();
            KeystrokeGridHelper.DataBindings.Add("DataSource", this, "v_KeyActions", false, DataSourceUpdateMode.OnPropertyChanged);
            KeystrokeGridHelper.AllowUserToDeleteRows = true;
            KeystrokeGridHelper.AutoGenerateColumns = false;
            KeystrokeGridHelper.Width = 500;
            KeystrokeGridHelper.Height = 140;

            DataGridViewComboBoxColumn propertyName = new DataGridViewComboBoxColumn();
            propertyName.DataSource = Common.GetAvailableKeys();
            propertyName.HeaderText = "Selected Key";
            propertyName.DataPropertyName = "Key";
            KeystrokeGridHelper.Columns.Add(propertyName);

            DataGridViewComboBoxColumn propertyValue = new DataGridViewComboBoxColumn();
            propertyValue.DataSource = new List<string> { "Key Press (Down + Up)", "Key Down", "Key Up" };
            propertyValue.HeaderText = "Selected Action";
            propertyValue.DataPropertyName = "Action";
            KeystrokeGridHelper.Columns.Add(propertyValue);

            KeystrokeGridHelper.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            KeystrokeGridHelper.AllowUserToAddRows = true;
            KeystrokeGridHelper.AllowUserToDeleteRows = true;


            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_KeyActions", this));
            RenderedControls.Add(KeystrokeGridHelper);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_KeyUpDefault", this, editor));

            return RenderedControls;
        }
     
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Send To Window '" + v_WindowName + "']";
        }
    }
}