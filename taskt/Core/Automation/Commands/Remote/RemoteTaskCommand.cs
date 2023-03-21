using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Remote Commands")]
    [Attributes.ClassAttributes.CommandSettings("Remote Task")]
    [Attributes.ClassAttributes.Description("This command allows you to execute a task remotely on another taskt instance")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to execute a command on another client that has local listener enabled")]
    [Attributes.ClassAttributes.ImplementationDescription("This command interfaces against Core.Server.LocalTCPListener")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RemoteTaskCommand : ScriptCommand
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
        [PropertyUISelectionOption("Run Raw Script Data")]
        [PropertyUISelectionOption("Run Local File")]
        [PropertyUISelectionOption("Run Remote File")]
        [PropertyUISelectionOption("Run Command Json")]
        [PropertyValidationRule("Paramter Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Parameter Type")]
        public string v_ParameterType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Execution Preference")]
        [PropertyUISelectionOption("Continue Execution")]
        [PropertyUISelectionOption("Await For Result")]
        [PropertyFirstValue("Continue Execution")]
        [PropertyIsOptional(true, "Continue Execution")]
        [PropertyDisplayText(false, "")]
        public string v_ExecuteAwait { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Script Parameter Data")]
        [InputSpecification("Script Parameter", true)]
        [PropertyIsOptional(true, "")]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_Parameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Request Timeout (ms)")]
        [InputSpecification("Request Timeout", true)]
        [PropertyDetailSampleUsage("**1000**", PropertyDetailSampleUsage.ValueType.Value, "Timeout")]
        [PropertyDetailSampleUsage("**{{{vTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Timeout")]
        [PropertyFirstValue("120000")]
        [PropertyValidationRule("Timeout", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Timeout")]
        public string v_RequestTimeout { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_userVariableName { get; set; }

        public RemoteTaskCommand()
        {
            //this.CommandName = "RemoteTaskCommand";
            //this.SelectionName = "Remote Task";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_ExecuteAwait = "Continue Execution";
            //this.v_RequestTimeout = "120000";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            try
            {
                var server = v_BaseURL.ConvertToUserVariable(engine);
                //var paramType = v_ParameterType.ConvertToUserVariable(engine);
                var paramType = this.GetUISelectionValue(nameof(v_ParameterType), engine);
                var parameter = v_Parameter.ConvertToUserVariable(engine);
                //var awaitPreference = v_ExecuteAwait.ConvertToUserVariable(engine);
                var awaitPreference = this.GetUISelectionValue(nameof(v_ExecuteAwait), engine);
                var timeout = v_RequestTimeout.ConvertToUserVariable(engine);

                var response = Server.LocalTCPListener.SendAutomationTask(server, paramType, timeout, parameter, awaitPreference);

                response.StoreInUserVariable(engine, v_userVariableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

