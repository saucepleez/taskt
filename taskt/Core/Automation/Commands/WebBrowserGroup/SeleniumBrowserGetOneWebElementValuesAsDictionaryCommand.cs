using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Scraping")]
    [Attributes.ClassAttributes.CommandSettings("Get One WebElement Values As Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to get Attributes value for One WebElement As Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Attributes value for One WebElement As Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetOneWebElementValuesAsDictionaryCommand : ASeleniumGetOneWebElementValuesAsSomethingCommands, IDictionaryResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //public string v_InstanceName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchMethod))]
        //[PropertySelectionChangeEvent(nameof(SearchMethodComboBox_SelectionChangeCommitted))]
        //public string v_SearchMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_SearchParameter))]
        //public string v_SearchParameter { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ElementIndex))]
        //public string v_WebElementIndex { get; set; }

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_AttributesName))]
        //public DataTable v_AttributesName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_WaitTime))]
        //public string v_WaitTimeForWebElement { get; set; }

        public SeleniumBrowserGetOneWebElementValuesAsDictionaryCommand()
        {
            //this.CommandName = "SeleniumBrowserGetAnElementValuesAsDictionaryCommand";
            //this.SelectionName = "Get An Element Values As Dictionary";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            ////(var _, var trgElem) = SeleniumBrowserControls.GetSeleniumBrowserInstanceAndElement(this, nameof(v_InstanceName), nameof(v_SeleniumSearchType), nameof(v_SeleniumSearchParameter), nameof(v_ElementIndex), engine);
            //(var _, var trgElem) = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstanceAndWebElement(this, nameof(v_InstanceName), nameof(v_SearchMethod), nameof(v_SearchParameter), nameof(v_WebElementIndex), nameof(v_WaitTimeForWebElement), engine);

            //Dictionary<string, string> newDic = new Dictionary<string, string>();

            //SeleniumBrowserControls.GetElementAttributes(trgElem, v_AttributesName, engine, new Action<string, string>((name, value) =>
            //    {
            //        if (newDic.Keys.Contains(name))
            //        {
            //            newDic[name] = value;
            //        }
            //        else
            //        {
            //            newDic.Add(name, value);
            //        }
            //    })
            //);

            ////newDic.StoreInUserVariable(engine, v_DictionaryVariableName);
            //this.StoreDictionaryInUserVariable(newDic, nameof(v_Result), engine);

            var ret = this.CreateEmptyDictionary();
            this.GetOneWebElementMultiValuesAction(new Action<string, string, int>((attrName, attrValue, index) =>
            {
                if (ret.ContainsKey(attrName))
                {
                    ret[attrName] = attrValue;
                }
                else
                {
                    ret.Add(attrName, attrValue);
                }
            }), engine);
            this.StoreDictionaryInUserVariable(ret, engine);
        }

        //private void SearchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    SeleniumBrowserControls.SearchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_WebElementIndex));
        //}

        //public override void BeforeValidate()
        //{
        //    base.BeforeValidate();
        //    DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_AttributesName)], v_AttributesName);
        //}
    }
}