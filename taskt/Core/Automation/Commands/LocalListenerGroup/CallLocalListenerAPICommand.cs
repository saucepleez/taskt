using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("LocalListener")]
    [Attributes.ClassAttributes.CommandSettings("Call LocalListener API")]
    [Attributes.ClassAttributes.Description("This command allows you to make API calls to taskt has LocalListener enabled")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you  want to make API calls to taskt has LocalListener enabled")]
    [Attributes.ClassAttributes.ImplementationDescription("This command interfaces against Core.Server.LocalTCPListener")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_remote))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CallLocalListenerAPICommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("taskt IP Address or URL")]
        [InputSpecification("IP Address", true)]
        [PropertyDetailSampleUsage("**192.168.0.15**", PropertyDetailSampleUsage.ValueType.Value, "IP Address")]
        [PropertyDetailSampleUsage("**{{{vRemoteHost}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "IP Address")]
        [PropertyDetailSampleUsage("**{{{vIP}}}**", PropertyDetailSampleUsage.ValueType.Value, "IP Address")]
        [PropertyValidationRule("IP Address or URL", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "IP Address or URL")]
        public string v_BaseURL { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Port")]
        [InputSpecification("Port", true)]
        [PropertyIsOptional(true, "19312")]
        [PropertyFirstValue("19312")]
        [PropertyDetailSampleUsage("**19312**", PropertyDetailSampleUsage.ValueType.Value, "Port")]
        [PropertyDetailSampleUsage("**{{{vPort}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Port")]
        [PropertyValidationRule("Port", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Port")]
        public string v_Port { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Parameter Type")]
        [PropertyUISelectionOption("Run Raw Script Data")]
        [PropertyUISelectionOption("Run Local File")]
        [PropertyUISelectionOption("Run Remote File")]
        [PropertyUISelectionOption("Run Command Json")]
        [PropertyUISelectionOption("Get Engine Status")]
        [PropertyUISelectionOption("Restart taskt")]
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
        [PropertyDescription("Request Timeout (sec)")]
        [InputSpecification("Request Timeout", true)]
        [PropertyDetailSampleUsage("**1000**", PropertyDetailSampleUsage.ValueType.Value, "Timeout")]
        [PropertyDetailSampleUsage("**{{{vTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Timeout")]
        [PropertyFirstValue("12")]
        [PropertyValidationRule("Timeout", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Timeout")]
        public string v_RequestTimeout { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Use Auth Key")]
        [PropertyIsOptional(true, "No")]
        public string v_UseAuthKey { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Auth Key")]
        [InputSpecification("Auth Key", true)]
        [PropertyIsOptional(true)]
        [PropertyDetailSampleUsage("**01234567-89ab-cdef-0123-456789abcedf**", PropertyDetailSampleUsage.ValueType.Value, "Auth Key")]
        [PropertyDetailSampleUsage("**{{{vKey}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Auth Key")]
        [PropertyValidationRule("Auth Key", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_AuthKey { get; set; }

        public CallLocalListenerAPICommand()
        {
            //this.CommandName = "RemoteTaskCommand";
            //this.SelectionName = "Remote Task";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_ExecuteAwait = "Continue Execution";
            //this.v_RequestTimeout = "120000";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            try
            {
                var server = v_BaseURL.ExpandValueOrUserVariable(engine);
                var port = this.ExpandValueOrUserVariableAsInteger(nameof(v_Port), "Port", engine);
                //var paramType = v_ParameterType.ConvertToUserVariable(engine);
                var paramType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ParameterType), engine);
                var parameter = v_Parameter.ExpandValueOrUserVariable(engine);
                //var awaitPreference = v_ExecuteAwait.ConvertToUserVariable(engine);
                var awaitPreference = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ExecuteAwait), engine);
                var timeout = v_RequestTimeout.ExpandValueOrUserVariable(engine);

                var useAuthKey = this.ExpandValueOrUserVariableAsYesNo(nameof(v_UseAuthKey), engine);
                string authKey = (useAuthKey) ? v_AuthKey.ExpandValueOrUserVariable(engine) : "";

                var response = Server.LocalTCPListener.SendAutomationTask($"{server}:{port}", paramType, timeout, parameter, awaitPreference, authKey);

                response.StoreInUserVariable(engine, v_userVariableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

