using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Window/Tab")]
    [Attributes.ClassAttributes.CommandSettings("Get Window And Tab Titles As List")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Page Titles of Windows and Tabs As List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Page Titles of Windows and Tabs As List.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWindowAndTabTitlesAsListCommand : ScriptCommand, IListResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_Result { get; set; }

        public SeleniumBrowserGetWindowAndTabTitlesAsListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var seleniumInstance = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstance(v_InstanceName, engine);

            var currentHandle = seleniumInstance.CurrentWindowHandle;

            var ret = this.CreateEmptyList();

            var handles = seleniumInstance.WindowHandles;
            foreach(var handle in handles)
            {
                seleniumInstance.SwitchTo().Window(handle);
                ret.Add(seleniumInstance.Title);
            }

            seleniumInstance.SwitchTo().Window(currentHandle);

            this.StoreListInUserVariable(ret, engine);
        }
    }
}