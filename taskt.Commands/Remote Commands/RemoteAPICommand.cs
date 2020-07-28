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
using taskt.Server;
using taskt.UI.CustomControls;

namespace taskt.Commands
{

    [Serializable]
    [Group("Remote Commands")]
    [Description("This command allows you to execute automation against another taskt Client.")]
    [UsesDescription("Use this command when you want to automate against a taskt instance that enables Local Listener.")]
    [ImplementationDescription("This command uses Core.Server.LocalTCPListener")]
    public class RemoteAPICommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please enter the IP:Port (ex. 192.168.2.200:19312)")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Define any IP endpoint which is enabled for local listening.")]
        [SampleUsage("**https://example.com** or **{vMyUrl}**")]
        [Remarks("")]
        public string v_BaseURL { get; set; }

        [XmlAttribute]
        [PropertyDescription("Select Parameter Type")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUISelectionOption("Get Engine Status")]
        [PropertyUISelectionOption("Restart taskt")]
        [InputSpecification("Select the necessary API Method")]
        [Remarks("")]
        public string v_ParameterType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Request Timeout (ms)")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the length of time to wait before the request times out ")]
        [Remarks("")]
        public string v_RequestTimeout { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the response")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }

        public RemoteAPICommand()
        {
            CommandName = "RemoteAPICommand";
            SelectionName = "Remote API";
            CommandEnabled = true;
            CustomRendering = true;
            v_RequestTimeout = "5000";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            try
            {
                var server = v_BaseURL.ConvertToUserVariable(engine);
                var paramType = v_ParameterType.ConvertToUserVariable(engine);
                var timeout = v_RequestTimeout.ConvertToUserVariable(engine);
                
                var response = LocalTCPClient.SendAutomationTask(server, paramType, timeout);
                response.StoreInUserVariable(engine, v_userVariableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_BaseURL", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ParameterType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RequestTimeout", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_ParameterType} on {v_BaseURL}]";
        }

    }

}

