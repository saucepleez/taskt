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
    [Attributes.ClassAttributes.Description("This command allows you to stop a program or a process.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to close an application by its name such as 'chrome'. Alternatively, you may use the Close Window or Thick App Command instead.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.CloseMainWindow'.")]
    public class RemoteDesktopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the name of the machine")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_MachineName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the username")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_UserName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the password")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Password { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the width of the RDP window")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_RDPWidth { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the height of the RDP window")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_RDPHeight { get; set; }

        public RemoteDesktopCommand()
        {
            this.CommandName = "RemoteDesktopCommand";
            this.SelectionName = "Launch Remote Desktop";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_RDPWidth = SystemInformation.PrimaryMonitorSize.Width.ToString();
            this.v_RDPHeight = SystemInformation.PrimaryMonitorSize.Height.ToString();

        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var machineName = v_MachineName.ConvertToUserVariable(sender);
            var userName = v_UserName.ConvertToUserVariable(sender);
            var password = v_Password.ConvertToUserVariable(sender);
            var width = int.Parse(v_RDPWidth.ConvertToUserVariable(sender));
            var height = int.Parse(v_RDPHeight.ConvertToUserVariable(sender));


            var result = engine.tasktEngineUI.Invoke(new Action(() =>
            {
                engine.tasktEngineUI.LaunchRDPSession(machineName, userName, password, width, height);
            }));


        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MachineName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_UserName", this, editor));

            //mask passwords
            var passwordGroup = CommandControls.CreateDefaultInputGroupFor("v_Password", this, editor);
            TextBox inputBox = (TextBox)passwordGroup[2];
            inputBox.PasswordChar = '*';

            RenderedControls.AddRange(passwordGroup);


            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RDPWidth", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RDPHeight", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Logon " + v_UserName + " to " + v_MachineName + "]";
        }
    }
}