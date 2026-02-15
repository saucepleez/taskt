using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    /// <summary>
    /// get one WebElement values as something commands
    /// </summary>
    public abstract class ASeleniumGetOneWebElementValuesAsSomethingCommands : ASeleniumSearchWebElementFromWebDriverCommands, IHaveDataTableElements
    {
        /// <summary>
        /// attribute names
        /// </summary>
        [XmlElement]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_AttributesName))]
        [PropertyParameterOrder(7000)]
        public DataTable v_AttributesName { get; set; }

        [XmlAttribute]
        [PropertyParameterOrder(8000)]
        public override string v_Result { get; set; }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_AttributesName)], v_AttributesName);
        }

        /// <summary>
        /// get multi value from one-WebElement action
        /// </summary>
        /// <param name="storeValueFunc">(attribute-name, attribute-value, index)</param>
        /// <param name="engine"></param>
        protected void GetOneWebElementMultiValuesAction(Action<string, string, int> storeValueFunc, Engine.AutomationEngineInstance engine)
        {
            this.SearchWebElementAction(new Action<IWebElement>(elem =>
            {
                using (var elemVar = new InnerScriptVariable(engine))
                {
                    elemVar.VariableValue = new ValueTuple<IWebElement, IWebDriver>(elem, null);

                    using (var resVar = new InnerScriptVariable(engine))
                    {
                        // create attribute names list
                        var attrs = new List<string>();
                        foreach (DataRow row in v_AttributesName.Rows) 
                        {
                            attrs.Add((row[0]?.ToString() ?? string.Empty).ExpandValueOrUserVariable(engine));
                        }

                        var getAttr = new SeleniumBrowserGetAttributeFromWebElementCommand()
                        {
                            v_WebElement = elemVar.VariableName,
                            v_Result = resVar.VariableName,
                        };

                        int idx = 0;
                        foreach(var attr in attrs)
                        {
                            getAttr.v_AttributeName = attr;
                            getAttr.RunCommand(engine);

                            var tellAttributeName = attr.StartsWith("@") ? attr.Substring(1) : attr;
                            storeValueFunc(tellAttributeName, resVar.VariableValue.ToString(), idx);
                            idx++;
                        }
                    }
                }
            }), engine);
        }
    }
}
