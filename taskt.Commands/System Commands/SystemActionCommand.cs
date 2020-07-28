using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("System Commands")]
    [Description("This command allows you to perform an account action")]
    [UsesDescription("Use this command to perform an action such as logoff, restart, shutdown or restart.")]
    [ImplementationDescription("")]
    public class SystemActionCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Select a system action to perform")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select from one of the options")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ActionName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox ActionNameComboBox;

        public SystemActionCommand()
        {
            CommandName = "SystemActionCommand";
            SelectionName = "System Action";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var action = (string)v_ActionName.ConvertToUserVariable(engine);
            switch (action)
            {
                case "Shutdown":
                    System.Diagnostics.Process.Start("shutdown", "/s /t 0");
                    break;
                case "Restart":
                    System.Diagnostics.Process.Start("shutdown", "/r /t 0");
                    break;
                case "Logoff":
                    User32Functions.WindowsLogOff();
                    break;
                case "Lock Screen":
                    User32Functions.LockWorkStation();
                    break;
                default:
                    break;
            }
        }
        public override List<Control> Render(IfrmCommandEditor editor)
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