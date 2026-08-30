using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Get From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Text From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Text Value from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Text Value from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetTextFromWebElementCommand : ASeleniumGetOneResultFromWebElementCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        //public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6000)]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        //[PropertySelectionChangeEvent(nameof(cmbScrollToElement_SelectionChange))]
        //public string v_ScrollToWebElement { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //[PropertyIsOptional(true)]
        //public string v_InstanceName { get; set; }

        public SeleniumBrowserGetTextFromWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ScrollToWebElement), engine))
            //{
            //    var scrollCommand = new SeleniumBrowserScrollToWebElementCommand()
            //    {
            //        //v_InstanceName = this.v_InstanceName,
            //        v_WebElement = this.v_WebElement,
            //        v_WhenFailAction = "ignore"
            //    };
            //    scrollCommand.RunCommand(engine);
            //}

            //var elem = v_WebElement.ExpandUserVariableAsWebElement("WebElement", engine);

            //var v = elem.Text;

            //if (string.IsNullOrEmpty(v))
            //{
            //    v = elem.GetAttribute("textContent");
            //}
            //if (string.IsNullOrEmpty(v) && (elem.TagName.ToLower() == "input"))
            //{
            //    v = elem.GetAttribute("value");
            //}

            //v.StoreInUserVariable(engine, v_Result);

            this.GetFromWebElementAction(new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((elem, seleniumInstance) =>
            {
                var v = elem.Text;

                if (string.IsNullOrEmpty(v))
                {
                    v = elem.GetAttribute("textContent");
                }
                if (string.IsNullOrEmpty(v) && (this.TagName(elem) == "input"))
                {
                    v = elem.GetAttribute("value");
                }
                v.StoreInUserVariable(engine, v_Result);
            }), new Action<Engine.AutomationEngineInstance>(e =>
            {
                string.Empty.StoreInUserVariable(engine, v_Result);
            }), engine);
        }

        //private void cmbScrollToElement_SelectionChange(object sender, EventArgs e)
        //{
        //    SeleniumBrowserControls.ScrollToWebElement_SelectionChange((ComboBox)sender, ControlsList, nameof(v_InstanceName));
        //}
    }
}