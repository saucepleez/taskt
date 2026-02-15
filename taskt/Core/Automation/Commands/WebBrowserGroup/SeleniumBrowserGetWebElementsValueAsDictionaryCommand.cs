using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Scraping")]
    [Attributes.ClassAttributes.CommandSettings("Get WebElements Value As Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to get a Attribute value for WegElements As Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a Attribute value for WegElements As Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWebElementsValueAsDictionaryCommand : ASeleniumGetWebElementsValueAsSomethingCommands, IDictionaryResultProperties
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

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_AttributeName))]
        //[PropertyParameterOrder(8000)]
        //public string v_AttributeName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        //public string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserGetWebElementsValueAsDictionaryCommand()
        {
            //this.CommandName = "SeleniumBrowserGetElementsValueAsDictionaryCommand";
            //this.SelectionName = "Get Elements Value As Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            ////(var _, var elems) = SeleniumBrowserControls.GetSeleniumBrowserInstanceAndElements(this, nameof(v_InstanceName), nameof(v_SeleniumSearchType), nameof(v_SeleniumSearchParameter), engine);
            //(var _, var elems) = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElements(this, nameof(v_InstanceName), nameof(v_SearchMethod), nameof(v_SearchParameter), nameof(v_WaitTimeForWebElement), engine);

            //var newDic = new Dictionary<string, string>();

            //SeleniumBrowserControls.GetElementsAttribute(elems, v_AttributeName, engine, new Action<int, string, string>((idx, name, value) =>
            //    {
            //        newDic.Add("element_" + idx, value);
            //    })
            //);

            ////newDic.StoreInUserVariable(engine, v_DictionaryVariableName);
            //this.StoreDictionaryInUserVariable(newDic, nameof(v_Result), engine);

            //this.SearchMultiWebElementsAction(new Action<List<OpenQA.Selenium.IWebElement>>(elems =>
            //{
            //    var newDic = this.CreateEmptyDictionary();

            //    using (var elemVar = new InnerScriptVariable(engine))
            //    {
            //        using (var resVar = new InnerScriptVariable(engine))
            //        {
            //            var getAttr = new SeleniumBrowserGetAttributeFromWebElementCommand()
            //            {
            //                v_WebElement = elemVar.VariableName,
            //                v_AttributeName = this.v_AttributeName,
            //                v_Result = resVar.VariableName,
            //            };

            //            int idx = 0;
            //            foreach (var elem in elems)
            //            {
            //                elemVar.VariableValue = new ValueTuple<IWebElement, IWebDriver>(elem, null);

            //                getAttr.RunCommand(engine);

            //                newDic.Add($"element_{idx}", resVar.VariableValue.ToString());
            //                idx++;
            //            }
            //        }
            //    }

            //    this.StoreDictionaryInUserVariable(newDic, engine);
            //}), engine);

            var newDic = this.CreateEmptyDictionary();
            this.GetMultiWebElementValueAction(new Action<string, string, int>((attrName, attrValue, idx) => {
                newDic.Add($"element_{idx}", attrValue);
            }), engine);
            this.StoreDictionaryInUserVariable(newDic, engine);
        }
    }
}