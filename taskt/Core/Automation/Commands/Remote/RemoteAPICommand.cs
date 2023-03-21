using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Remote Commands")]
    [Attributes.ClassAttributes.CommandSettings("Remote API")]
    [Attributes.ClassAttributes.Description("This command allows you to execute automation against another taskt Client.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to automate against a taskt instance that enables Local Listener.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses Core.Server.LocalTCPListener")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RemoteAPICommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("IP:Port")]
        [InputSpecification("IP and Port", true)]
        [PropertyDetailSampleUsage("**192.168.0.15:19312**", PropertyDetailSampleUsage.ValueType.Value, "IP and Port")]
        [PropertyDetailSampleUsage("**{{{vRemoteHost}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "IP and Port")]
        [PropertyDetailSampleUsage("**{{{vIP}}}:{{{vPort}}}", PropertyDetailSampleUsage.ValueType.Value, "IP and Port")]
        [PropertyValidationRule("IP and Port", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "IP:Port")]
        public string v_BaseURL { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Parameter Type")]
        [PropertyUISelectionOption("Get Engine Status")]
        [PropertyUISelectionOption("Restart taskt")]
        [PropertyValidationRule("Parameter Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Parameter Type")]
        public string v_ParameterType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Request Timeout (ms)")]
        [InputSpecification("Request Timeout", true)]
        [PropertyDetailSampleUsage("**1000**", PropertyDetailSampleUsage.ValueType.Value, "Timeout")]
        [PropertyDetailSampleUsage("**{{{vTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Timeout")]
        [PropertyFirstValue("5000")]
        [PropertyValidationRule("Timeout", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Timeout")]
        public string v_RequestTimeout { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Receive the Response")]
        public string v_userVariableName { get; set; }

        public RemoteAPICommand()
        {
            //this.CommandName = "RemoteAPICommand";
            //this.SelectionName = "Remote API";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_RequestTimeout = "5000";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            try
            {
                var server = v_BaseURL.ConvertToUserVariable(engine);
                //var paramType = v_ParameterType.ConvertToUserVariable(engine);
                var paramType = this.GetUISelectionValue(nameof(v_ParameterType), engine);
                var timeout = v_RequestTimeout.ConvertToUserVariable(engine);

                var response = Server.LocalTCPListener.SendAutomationTask(server, paramType, timeout);

                response.StoreInUserVariable(engine, v_userVariableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

