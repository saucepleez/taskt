using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Search WebElement From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Matched WebElements HTML As List From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Matched WebElements HTML from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Matched WebElements HTML from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetMatchedWebElementsHTMLAsListFromWebElementCommand : ASeleniumSearchMultiWebElementsFromWebElementCommands, IListResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        //public string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserGetMatchedWebElementsHTMLAsListFromWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.SearchMultiWebElementsAction(new Action<List<OpenQA.Selenium.IWebElement>>(elems =>
            {
                var lst = this.CreateEmptyList();
                foreach(var elem in elems)
                {
                    lst.Add(elem.GetAttribute("outerHTML"));
                }
                this.StoreListInUserVariable(lst, engine);
            }), engine);
        }
    }
}