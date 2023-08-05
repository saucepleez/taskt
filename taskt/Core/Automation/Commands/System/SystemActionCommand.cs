using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("System Commands")]
    [Attributes.ClassAttributes.CommandSettings("System Action")]
    [Attributes.ClassAttributes.Description("This command allows you to perform an account action")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to perform an action such as logoff, restart, shutdown or restart.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SystemActionCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("System Action")]
        [PropertyUISelectionOption("Shutdown")]
        [PropertyUISelectionOption("Restart")]
        [PropertyUISelectionOption("Logoff")]
        [PropertyUISelectionOption("Lock Screen")]
        [PropertyValidationRule("Action", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Action")]
        public string v_ActionName { get; set; }

        public SystemActionCommand()
        {
            //this.CommandName = "SystemActionCommand";
            //this.SelectionName = "System Action";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var action = this.GetUISelectionValue(nameof(v_ActionName), engine);

            switch (action)
            {
                case "shutdown":
                    System.Diagnostics.Process.Start("shutdown", "/s /t 0");
                    break;
                case "restart":
                    System.Diagnostics.Process.Start("shutdown", "/r /t 0");
                    break;
                case "logoff":
                    //User32.User32Functions.WindowsLogOff();
                    SystemControls.WindowsLogOff();
                    break;
                case "lock screen":
                    //User32.User32Functions.LockWorkStation();
                    SystemControls.UserLock();
                    break;
            }
        }
    }
}