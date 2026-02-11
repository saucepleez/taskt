using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Search WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Matched WebElements")]
    [Attributes.ClassAttributes.Description("This command allows you to get Matched WebElements HTML.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Matched WebElements HTML.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetMatchedWebElementsCommand : ASeleniumSearchMultiWebElementsFromWebDriverCommands, IListResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchMethod))]
        //public string v_SearchMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchParameter))]
        //public string v_SearchParameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        [PropertyParameterOrder(7000)]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        //public string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserGetMatchedWebElementsCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var _, var trgElem) = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElements(this, nameof(v_InstanceName), nameof(v_SearchMethod), nameof(v_SearchParameter), nameof(v_WaitTimeForWebElement), engine);

            //var lst = new List<string>();
            //foreach(var elem in trgElem)
            //{
            //    lst.Add(elem.GetAttribute("outerHTML"));
            //}
            //this.StoreListInUserVariable(lst, engine);

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