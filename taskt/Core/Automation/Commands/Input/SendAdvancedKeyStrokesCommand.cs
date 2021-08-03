using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.User32;
using taskt.UI.Forms;
using System.Windows.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Sends advanced keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send advanced keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'User32' method to achieve automation.")]
    public class SendAdvancedKeyStrokesCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name (ex. Untitled - Notepad, %kwd_current_window%, {{{vWindowName}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindowName}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsWindowNamesList(true)]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Window name search method (Default is Contains)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Contains")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Start with")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("End with")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Exact match")]
        [Attributes.PropertyAttributes.SampleUsage("**Contains** or **Start with** or **End with** or **Exact match**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_SearchMethod { get; set; }

        [Attributes.PropertyAttributes.PropertyDescription("Set Keys and Parameters")]
        [Attributes.PropertyAttributes.InputSpecification("Define the parameters for the actions.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("Select Valid Options from the dropdowns")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        public DataTable v_KeyActions { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Return all keys to 'UP' position after execution (Default is No)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Select either 'Yes' or 'No' as to a preference")]
        [Attributes.PropertyAttributes.SampleUsage("**Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        public string v_KeyUpDefault { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView KeystrokeGridHelper;

        public SendAdvancedKeyStrokesCommand()
        {
            this.CommandName = "SendAdvancedKeyStrokesCommand";
            this.SelectionName = "Send Advanced Keystrokes";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_KeyActions = new DataTable();
            this.v_KeyActions.Columns.Add("Key");
            this.v_KeyActions.Columns.Add("Action");
            this.v_KeyActions.TableName = "SendAdvancedKeyStrokesCommand" + DateTime.Now.ToString("MMddyy.hhmmss");
            v_KeyUpDefault = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var targetWindow = v_WindowName.ConvertToUserVariable(sender);
            var searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(searchMethod))
            {
                searchMethod = "Contains";
            }

            //activate anything except current window
            if (targetWindow != ((Automation.Engine.AutomationEngineInstance)sender).engineSettings.CurrentWindowKeyword)
            {
                ActivateWindowCommand activateWindow = new ActivateWindowCommand
                {
                    v_WindowName = targetWindow,
                    v_SearchMethod = searchMethod
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
            if (v_KeyUpDefault.ConvertToUserVariable(sender) == "Yes")
            {
                foreach (var key in keysDown)
                {
                    User32Functions.KeyUp(key);
                }
            }
        
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            var WindowNameControl = UI.CustomControls.CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames(editor);
            RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            RenderedControls.Add(WindowNameControl);

            RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateDefaultDropdownGroupFor("v_SearchMethod", this, editor));

            //KeystrokeGridHelper = new DataGridView();
            //KeystrokeGridHelper.DataBindings.Add("DataSource", this, "v_KeyActions", false, DataSourceUpdateMode.OnPropertyChanged);
            //KeystrokeGridHelper.AllowUserToDeleteRows = true;
            //KeystrokeGridHelper.AutoGenerateColumns = false;
            //KeystrokeGridHelper.Width = 500;
            //KeystrokeGridHelper.Height = 140;
            KeystrokeGridHelper = CommandControls.CreateDataGridView(this, "v_KeyActions", true, true, false, 500, 140, false);
            KeystrokeGridHelper.CellClick += KeystrokeGridHelper_CellClick;

            DataGridViewComboBoxColumn propertyName = new DataGridViewComboBoxColumn();
            propertyName.DataSource = Core.Common.GetAvailableKeys();
            propertyName.HeaderText = "Selected Key";
            propertyName.DataPropertyName = "Key";
            KeystrokeGridHelper.Columns.Add(propertyName);

            DataGridViewComboBoxColumn propertyValue = new DataGridViewComboBoxColumn();
            propertyValue.DataSource = new List<string> { "Key Press (Down + Up)", "Key Down", "Key Up" };
            propertyValue.HeaderText = "Selected Action";
            propertyValue.DataPropertyName = "Action";
            KeystrokeGridHelper.Columns.Add(propertyValue);

            //KeystrokeGridHelper.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            //KeystrokeGridHelper.AllowUserToAddRows = true;
            //KeystrokeGridHelper.AllowUserToDeleteRows = true;

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_KeyActions", this));
            RenderedControls.Add(KeystrokeGridHelper);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_KeyUpDefault", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Send To Window '" + v_WindowName + "']";
        }
        private void KeystrokeGridHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (KeystrokeGridHelper.CurrentCell.Value.ToString() == "")
                {
                    SendKeys.Send("%{DOWN}");
                }
            }
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            if (KeystrokeGridHelper.IsCurrentCellDirty || KeystrokeGridHelper.IsCurrentRowDirty)
            {
                KeystrokeGridHelper.CommitEdit(DataGridViewDataErrorContexts.Commit);
                var newRow = v_KeyActions.NewRow();
                v_KeyActions.Rows.Add(newRow);
                for (var i = v_KeyActions.Rows.Count - 1; i >= 0; i--)
                {
                    if (v_KeyActions.Rows[i][0].ToString() == "" && v_KeyActions.Rows[i][1].ToString() == "")
                    {
                        v_KeyActions.Rows[i].Delete();
                    }
                }
            }
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_WindowName))
            {
                this.validationResult += "Windows name is empty.\n";
                this.IsValid = false;
            }
            for (int i = 0; i < v_KeyActions.Rows.Count; i++)
            {
                var row = v_KeyActions.Rows[i];
                if (String.IsNullOrEmpty(row["Key"].ToString()))
                {
                    this.validationResult += "Selected Key #" + (i + 1) + " is empty.\n";
                    this.IsValid = false;
                }
                else if (String.IsNullOrEmpty(row["Action"].ToString()))
                {
                    this.validationResult += "Selected Action #" + (i + 1) + " is empty.\n";
                    this.IsValid = false;
                }
            }

            return this.IsValid;
        }
    }
}