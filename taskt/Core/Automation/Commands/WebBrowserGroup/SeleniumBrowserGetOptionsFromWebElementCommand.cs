using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Get From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Options From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Options Value from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Options Value from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetOptionsFromWebElementCommand : ASeleniumGetOneResultFromWebElementCommands, IListResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        //public string v_WebElement { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Attribute Name")]
        //[PropertyDetailSampleUsage("**textContent**", PropertyDetailSampleUsage.ValueType.Value, "Attribute")]
        //[PropertyDetailSampleUsage("**value**", PropertyDetailSampleUsage.ValueType.Value, "Attribute")]
        //[PropertyDetailSampleUsage("**{{{vAttribute}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Attribute")]
        //[PropertyValidationRule("Attribute", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Attribute")]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_AttributeName))]
        [PropertyParameterOrder(6000)]
        public string v_AttributeName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        [PropertyParameterOrder(7000)]
        public override string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("When the WebElement does not have the Attribute")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyIsOptional(true, "Error")]
        //[PropertyDisplayText(false, "")]
        //public string v_WhenValueCanNotRetrieved { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        //[PropertySelectionChangeEvent(nameof(cmbScrollToElement_SelectionChange))]
        //public string v_ScrollToWebElement { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //[PropertyIsOptional(true)]
        //public string v_InstanceName { get; set; }

        public SeleniumBrowserGetOptionsFromWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var elem = v_WebElement.ExpandUserVariableAsWebElement("WebElement", engine);

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

            //if (!elem.CheckTagName("select"))
            //{
            //    throw new Exception("WebElement is not Select");
            //}

            //var sel = new SelectElement(elem);
            //var options = sel.Options;

            //var attributeName = v_AttributeName.ExpandValueOrUserVariable(engine);

            //var throwError = (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenValueCanNotRetrieved), engine) == "error");

            //var lst = new List<string>();
            //foreach(var opt in options)
            //{
            //    var a = opt.GetAttribute(attributeName);
            //    if (a == null)
            //    {
            //        if (throwError)
            //        {
            //            throw new Exception("Attribute '" + attributeName + "' does not exists.");
            //        }
            //        else
            //        {
            //            lst.Add("");
            //        }
            //    }
            //    else
            //    {
            //        lst.Add(a);
            //    }
            //}
            //this.StoreListInUserVariable(lst, engine);

            this.GetFromWebElementAction(new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((elem, seleniumInstance) =>
            {
                if (this.TagName(elem) != "select")
                {
                    throw new Exception($"WebElement is not Select. Tag Name: '{this.TagName(elem)}'");
                }

                var sel = new SelectElement(elem);
                var options = sel.Options;

                var attributeName = v_AttributeName.ExpandValueOrUserVariable(engine);

                bool throwError = false;
                bool setEmpty = false;
                switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenValueCanNotRetrieved), engine))
                {
                    case "error":
                        throwError = true;
                        break;
                    case "set emtpy value":
                        setEmpty = true;
                        break;
                }

                var lst = this.CreateEmptyList();
                foreach (var opt in options)
                {
                    var a = opt.GetAttribute(attributeName);
                    if (a == null)
                    {
                        if (throwError)
                        {
                            throw new Exception($"Attribute does not exists. Attribute: '{v_AttributeName}', Expand: '{attributeName}'");
                        }
                        else if (setEmpty)
                        {
                            lst.Add(string.Empty);
                        }
                    }
                    else
                    {
                        lst.Add(a);
                    }
                }
                this.StoreListInUserVariable(lst, engine);
            }), new Action<Engine.AutomationEngineInstance>(e =>
            {
                this.StoreListInUserVariable(this.CreateEmptyList(), engine);
            }), engine);
        }

        //private void cmbScrollToElement_SelectionChange(object sender, EventArgs e)
        //{
        //    SeleniumBrowserControls.ScrollToWebElement_SelectionChange((ComboBox)sender, ControlsList, nameof(v_InstanceName));
        //}
    }
}