using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get Process Names From Window Names As List")]
    [Attributes.ClassAttributes.Description("This command returns window process names.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get window process names.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetProcessNamesFromWindowNamesAsListCommand : GetFromWindowNamesAsListCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterOrder(6500)]
        public string v_Result { get; set; }

        public GetProcessNamesFromWindowNamesAsListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var tb = new InnerScriptVariable(engine))
            {
                var getProcesses = new GetProcessNamesFromWindowNamesAsDataTableCommand()
                {
                    v_WindowName = this.v_WindowName,
                    v_CompareMethod = this.v_CompareMethod,
                    v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                    v_Result = tb.VariableName,
                };
                getProcesses.RunCommand(engine);

                this.StoreListInUserVariable(GetColumnValues(tb, 2), nameof(v_Result), engine);
            }
        }
    }
}
