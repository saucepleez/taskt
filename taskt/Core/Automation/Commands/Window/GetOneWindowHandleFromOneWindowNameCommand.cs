using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get One Window Handle From One Window Name")]
    [Attributes.ClassAttributes.Description("This command returns one window handle.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get one window handle.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetOneWindowHandleFromOneWindowNameCommand : AOneWindowNameCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyParameterOrder(6500)]
        public string v_Result { get; set; }

        public GetOneWindowHandleFromOneWindowNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNameAction(engine, new Action<IntPtr, string>((whnd, name) =>
            {
                whnd.StoreInUserVariable(engine, v_Result);
            }));
        }
    }
}
