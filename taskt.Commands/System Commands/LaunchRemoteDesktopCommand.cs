using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Commands
{
    [Serializable]
    [Group("System Commands")]
    [Description("This command allows you to stop a program or a process.")]
    [UsesDescription("Use this command to close an application by its name such as 'chrome'. Alternatively, you may use the Close Window or Thick App Command instead.")]
    [ImplementationDescription("This command implements 'Process.CloseMainWindow'.")]
    public class LaunchRemoteDesktopCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Enter the name of the machine")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_MachineName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Enter the username")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_UserName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Enter the password")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Password { get; set; }

        [XmlAttribute]
        [PropertyDescription("Enter the width of the RDP window")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_RDPWidth { get; set; }

        [XmlAttribute]
        [PropertyDescription("Enter the height of the RDP window")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_RDPHeight { get; set; }

        public LaunchRemoteDesktopCommand()
        {
            CommandName = "LaunchRemoteDesktopCommand";
            SelectionName = "Launch Remote Desktop";
            CommandEnabled = true;
            CustomRendering = true;

            v_RDPWidth = SystemInformation.PrimaryMonitorSize.Width.ToString();
            v_RDPHeight = SystemInformation.PrimaryMonitorSize.Height.ToString();

        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var machineName = v_MachineName.ConvertToUserVariable(engine);
            var userName = v_UserName.ConvertToUserVariable(engine);
            var password = v_Password.ConvertToUserVariable(engine);
            var width = int.Parse(v_RDPWidth.ConvertToUserVariable(engine));
            var height = int.Parse(v_RDPHeight.ConvertToUserVariable(engine));


            var result = ((frmScriptEngine)engine.TasktEngineUI).Invoke(new Action(() =>
            {
                engine.TasktEngineUI.LaunchRDPSession(machineName, userName, password, width, height);
            }));


        }
        public override List<Control> Render(IfrmCommandEditor editor)
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