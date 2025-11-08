using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Search UIElements Information From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElements Information from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElements Information from UIElement. Search for Descendants Elements.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchUIElementsInformationFromUIElementCommand : ADeepSearchUIElementsFromSomethingByTreeWalkerCommands, IUIElementCoreProperties, IGetUIElementsInformationProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_InputUIElementName))]
        [PropertyParameterOrder(5000)]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        public UIAutomationSearchUIElementsInformationFromUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var targetElement = this.ExpandUserVariableAsUIElement(engine);
            var elems = this.DeepSearchUIElements(targetElement, engine);
            this.StoreUIElementsInformationInUserVariable(elems, engine);
        }
    }
}