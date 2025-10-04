using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Window/Tab")]
    [Attributes.ClassAttributes.CommandSettings("Get Window And Tab Count")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Number of Windows and Tabs.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Number of Windows and Tabs.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWindowAndTabCountCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public SeleniumBrowserGetWindowAndTabCountCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var seleniumInstance = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstance(v_InstanceName, engine);

            seleniumInstance.WindowHandles.Count.StoreInUserVariable(engine, v_Result);
        }
    }
}