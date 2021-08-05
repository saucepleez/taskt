using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("System Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to perform an account action")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to perform an action such as logoff, restart, shutdown or restart.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SystemActionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select a system action to perform")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select from one of the options")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ActionName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox ActionNameComboBox;

        public SystemActionCommand()
        {
            this.CommandName = "SystemActionCommand";
            this.SelectionName = "System Action";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var action = (string)v_ActionName.ConvertToUserVariable(sender);
            switch (action)
            {
                case "Shutdown":
                    System.Diagnostics.Process.Start("shutdown", "/s /t 0");
                    break;
                case "Restart":
                    System.Diagnostics.Process.Start("shutdown", "/r /t 0");
                    break;
                case "Logoff":
                    User32.User32Functions.WindowsLogOff();
                    break;
                case "Lock Screen":
                    User32.User32Functions.LockWorkStation();
                    break;
                default:
                    break;
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ActionNameComboBoxLabel = CommandControls.CreateDefaultLabelFor("v_ActionName", this);
            ActionNameComboBox = (ComboBox)CommandControls.CreateDropdownFor("v_ActionName", this);
            ActionNameComboBox.DataSource = new List<string> { "Shutdown", "Restart", "Lock Screen", "Logoff" };
            RenderedControls.Add(ActionNameComboBoxLabel);
            RenderedControls.Add(ActionNameComboBox);


            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Action: " + v_ActionName + "]";
        }
    }
}