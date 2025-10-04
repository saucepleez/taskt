using System;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Window/Tab")]
    [Attributes.ClassAttributes.CommandSettings("Get Window And Tab Handles As List")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Web Browser Handles of Windows and Tabs As List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Web Browser Handles of Windows and Tabs As List.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWindowAndTabHandlesAsListCommand : ScriptCommand, IListResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_Result { get; set; }

        public SeleniumBrowserGetWindowAndTabHandlesAsListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var seleniumInstance = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstance(v_InstanceName, engine);

            this.StoreListInUserVariable(seleniumInstance.WindowHandles.ToList(), engine);
        }
    }
}