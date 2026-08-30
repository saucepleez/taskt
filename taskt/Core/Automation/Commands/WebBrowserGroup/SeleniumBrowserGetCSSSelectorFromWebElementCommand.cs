using OpenQA.Selenium;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Get From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get CSS Selector From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Get CSS Selector from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get CSS Selector from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetCSSSelectorFromWebElementCommand : ASeleniumGetWebElementSelectorFromWebElementCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyParameterOrder(6000)]
        //public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        //[PropertyDescription("Believe ID Attribute")]
        //[PropertyIsOptional(true, "No")]
        //[PropertyDisplayText(false, "Believe ID")]
        //[PropertyParameterOrder(7000)]
        //public string v_BeliveIDAttribute { get; set; }

        public SeleniumBrowserGetCSSSelectorFromWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.GetFromWebElementAction(new Action<IWebElement, IWebDriver>((elem, seleniumInstance) =>
            {
                // MEMO: it's probably works fine. :-)
                var path = string.Empty;

                var believeId = this.ExpandValueOrUserVariableAsYesNo(nameof(v_BeliveIDAttribute), engine);

                var curElem = elem;
                var curElemId = curElem.ToString();

                var pElem = curElem.FindElement(By.XPath("parent::*"));
                while (true)
                {
                    var cTag = curElem.TagName.ToLower();

                    var cElems = pElem.FindElements(By.XPath($"{cTag}"));
                    if (cElems.Count > 1)
                    {
                        int index = 1;
                        foreach (var e in cElems)
                        {
                            if (e.ToString() == curElemId)
                            {
                                break;
                            }
                            index++;
                        }

                        var idAttr = curElem.GetAttribute("id");
                        string addPath;
                        if (!string.IsNullOrEmpty(idAttr) && believeId)
                        {
                            addPath = $"{cTag}#{idAttr}";
                        }
                        else
                        {
                            addPath = $"{cTag}:nth-of-type({index})";
                        }
                        path = (path.Length > 0) ? $"{addPath} > {path}" : addPath;
                    }
                    else
                    {
                        var idAttr = curElem.GetAttribute("id");
                        string addPath;
                        if (!string.IsNullOrEmpty(idAttr) && believeId)
                        {
                            addPath = $"{cTag}#{idAttr}";
                        }
                        else
                        {
                            addPath = cTag;
                        }

                        path = (path.Length > 0) ? $"{addPath} > {path}" : addPath;
                    }

                    if (cTag == "body")
                    {
                        break;
                    }
                    else
                    {
                        curElem = pElem;
                        curElemId = curElem.ToString();
                        pElem = curElem.FindElement(By.XPath("parent::*"));
                    }
                }
                path.StoreInUserVariable(engine, v_Result);
            }), new Action<Engine.AutomationEngineInstance>(e =>
            {
                string.Empty.StoreInUserVariable(engine, v_Result);
            }), engine);
        }
    }
}