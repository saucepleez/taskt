using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("System Commands")]
    [Attributes.ClassAttributes.CommandSettings("Launch Remote Desktop")]
    [Attributes.ClassAttributes.Description("This command allows you to stop a program or a process.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to close an application by its name such as 'chrome'. Alternatively, you may use the Close Window or Thick App Command instead.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.CloseMainWindow'.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class LaunchRemoteDesktopCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Machine Name")]
        [InputSpecification("Machine Name", true)]
        [PropertyValidationRule("Machine Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Machine Name")]
        public string v_MachineName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("User Name")]
        [InputSpecification("User Name", true)]
        [PropertyValidationRule("User Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "User")]
        public string v_UserName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Password")]
        [InputSpecification("Password", true)]
        [PropertyDisplayText(false, "")]
        public string v_Password { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Width of the RDP Window")]
        [InputSpecification("Width", true)]
        [PropertyIsOptional(true, "Primary Monitor Size")]
        [PropertyValidationRule("Width", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(false, "")]
        public string v_RDPWidth { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Height of the RDP Window")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Height", true)]
        [PropertyIsOptional(true, "Primary Monitor Size")]
        [PropertyValidationRule("Height", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(false, "")]
        public string v_RDPHeight { get; set; }

        public LaunchRemoteDesktopCommand()
        {
            //this.CommandName = "RemoteDesktopCommand";
            //this.SelectionName = "Launch Remote Desktop";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //this.v_RDPWidth = SystemInformation.PrimaryMonitorSize.Width.ToString();
            //this.v_RDPHeight = SystemInformation.PrimaryMonitorSize.Height.ToString();
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var machineName = v_MachineName.ConvertToUserVariable(sender);
            var userName = v_UserName.ConvertToUserVariable(sender);
            var password = v_Password.ConvertToUserVariable(sender);

            //var width = int.Parse(v_RDPWidth.ConvertToUserVariable(sender));
            //var height = int.Parse(v_RDPHeight.ConvertToUserVariable(sender));
            if (String.IsNullOrEmpty(v_RDPWidth))
            {
                v_RDPWidth = SystemInformation.PrimaryMonitorSize.Width.ToString();
            }
            var width = this.ConvertToUserVariableAsInteger(nameof(v_RDPWidth), engine);

            if (String.IsNullOrEmpty(v_RDPHeight))
            {
                v_RDPHeight = SystemInformation.PrimaryMonitorSize.Height.ToString();
            }
            var height = this.ConvertToUserVariableAsInteger(nameof(v_RDPHeight), engine);

            var result = engine.tasktEngineUI.Invoke(new Action(() =>
            {
                engine.tasktEngineUI.LaunchRDPSession(machineName, userName, password, width, height);
            }));
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MachineName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_UserName", this, editor));

        //    //mask passwords
        //    var passwordGroup = CommandControls.CreateDefaultInputGroupFor("v_Password", this, editor);
        //    TextBox inputBox = (TextBox)passwordGroup[2];
        //    inputBox.PasswordChar = '*';

        //    RenderedControls.AddRange(passwordGroup);


        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RDPWidth", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RDPHeight", this, editor));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Logon " + v_UserName + " to " + v_MachineName + "]";
        //}
    }
}