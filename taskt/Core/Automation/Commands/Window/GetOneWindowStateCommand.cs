using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Window")]
    [Attributes.ClassAttributes.SubGruop("Get From Window Name")]
    [Attributes.ClassAttributes.CommandSettings("Get One Window State")]
    [Attributes.ClassAttributes.Description("This command returns one window state.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want one window state.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetOneWindowStateCommand : AWindowNameCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("Restore is **1**, Minimize is **2**, Maximize is **3**")]
        [PropertyParameterOrder(6500)]
        public string v_WindowState { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Window State Text")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Window State Text", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "State Text")]
        [PropertyParameterOrder(6501)]
        public string v_WindowStateText { get; set; }

        public GetOneWindowStateCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowNameAction(engine, new Action<IntPtr, string>((whnd, name) =>
            {
                var getState = new GetWindowStateFromWindowHandleCommand()
                {
                    v_WindowHandle = whnd.ToString(),
                    v_WindowState = this.v_WindowState,
                    v_WindowStateText = this.v_WindowStateText,
                };
                getState.RunCommand(engine);
            }));
        }
    }
}
