using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Window/Tab")]
    [Attributes.ClassAttributes.CommandSettings("Get Window And Tab Titles As Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Page Titles of Windows and Tabs As Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Page Titles of Windows and Tabs As Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWindowAndTabTitlesAsDictionaryCommand : ASeleniumGetWindowAndTabInformationCommands, IDictionaryResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        //[PropertyParameterOrder(6000)]
        public override string v_Result { get; set; }

        public SeleniumBrowserGetWindowAndTabTitlesAsDictionaryCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var seleniumInstance = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstance(v_InstanceName, engine);

            //var currentHandle = seleniumInstance.CurrentWindowHandle;

            //var ret = this.CreateEmptyDictionary();

            //var handles = seleniumInstance.WindowHandles;
            //foreach(var handle in handles)
            //{
            //    seleniumInstance.SwitchTo().Window(handle);
            //    ret.Add(handle, seleniumInstance.Title);
            //}

            //seleniumInstance.SwitchTo().Window(currentHandle);

            //this.StoreDictionaryInUserVariable(ret, engine);

            this.WebDriverActionCore(new Action<OpenQA.Selenium.IWebDriver>(seleniumInstance =>
            {
                var ret = this.CreateEmptyDictionary();

                //var currentHandle = seleniumInstance.CurrentWindowHandle;

                //var handles = seleniumInstance.WindowHandles;
                //foreach (var handle in handles)
                //{
                //    seleniumInstance.SwitchTo().Window(handle);
                //    ret.Add(handle, seleniumInstance.Title);
                //}

                //seleniumInstance.SwitchTo().Window(currentHandle);

                SeleniumWindowAndTabAction(seleniumInstance, new Action<string>(handle =>
                {
                    ret.Add(handle, seleniumInstance.Title);
                }));

                this.StoreDictionaryInUserVariable(ret, engine);
            }), engine);
        }
    }
}