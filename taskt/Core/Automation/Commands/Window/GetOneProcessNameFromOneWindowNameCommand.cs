using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get One Process Name From One Window Name")]
    [Attributes.ClassAttributes.Description("This command returns one window process name.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get one window process name.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetOneProcessNameFromOneWindowNameCommand : AOneWindowNameCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyParameterOrder(6500)]
        public string v_Result { get; set; }

        public GetOneProcessNameFromOneWindowNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNameAction(engine, new Action<IntPtr, string>((whnd, name) =>
            {
                var getProcess = new GetProcessNameFromWindowHandleCommand()
                {
                    v_WindowHandle = whnd.ToString(),
                    v_Result = this.v_Result,
                };
                getProcess.RunCommand(engine);
            }));
        }
    }
}
