using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// for get multi WebElements value as something commands
    /// </summary>
    public abstract class ASeleniumGetWebElementsValueAsSomethingCommands : ASeleniumSearchMultiWebElementsFromWebDriverCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_AttributeName))]
        [PropertyParameterOrder(8000)]
        public virtual string v_AttributeName { get; set; }

        [XmlAttribute]
        [PropertyParameterOrder(8100)]
        public override string v_Result { get; set; }

        /// <summary>
        /// get muti WebElements value action
        /// </summary>
        /// <param name="storeValueFunc">(attribute-name, attribute-value, index)</param>
        /// <param name="engine"></param>
        protected void GetMultiWebElementValueAction(Action<string, string, int> storeValueFunc, Engine.AutomationEngineInstance engine)
        {
            this.SearchMultiWebElementsAction(new Action<List<IWebElement>>(elems =>
            {
                using (var elemVar = new InnerScriptVariable(engine))
                {
                    using (var resVar = new InnerScriptVariable(engine))
                    {
                        var attrName = this.ExpandValueOrUserVariable(nameof(v_AttributeName), "Attribute", engine);

                        var getAttr = new SeleniumBrowserGetAttributeFromWebElementCommand()
                        {
                            v_WebElement = elemVar.VariableName,
                            v_AttributeName = this.v_AttributeName,
                            v_Result = resVar.VariableName,
                        };

                        int idx = 0;
                        foreach (var elem in elems)
                        {
                            elemVar.VariableValue = new ValueTuple<IWebElement, IWebDriver>(elem, null);

                            getAttr.RunCommand(engine);

                            // newDic.Add($"element_{idx}", resVar.VariableValue.ToString());
                            storeValueFunc(attrName, resVar.VariableValue.ToString(), idx);

                            idx++;
                        }
                    }
                }
            }), engine);
        }
    }
}
