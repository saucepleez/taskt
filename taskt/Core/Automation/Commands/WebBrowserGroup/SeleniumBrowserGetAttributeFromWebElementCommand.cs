using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Get From WebElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Attribute From WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Attribute Value from WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Attribute Value from WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetAttributeFromWebElementCommand : ASeleniumGetOneResultFromWebElementCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        //public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_AttributeName))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Attribute Name")]
        //[PropertyDetailSampleUsage("**class**", PropertyDetailSampleUsage.ValueType.Value, "Attribute")]
        //[PropertyDetailSampleUsage("**{{{vAttribute}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Attribute")]
        //[PropertyDetailSampleUsage("**@tag**", "Get Tab name from WebElement. Use Get Special Value From WebElement command.")]
        //[PropertyValidationRule("Attribute", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Attribute")]
        [PropertyParameterOrder(6000)]
        public string v_AttributeName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
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

        public SeleniumBrowserGetAttributeFromWebElementCommand()
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

            //var attributeName = v_AttributeName.ExpandValueOrUserVariable(engine);

            //var v = elem.GetAttribute(attributeName);

            //if (v != null)
            //{
            //    v.StoreInUserVariable(engine, v_Result);
            //}
            //else
            //{
            //    if (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenValueCanNotRetrieved), engine) == "error")
            //    {
            //        throw new Exception("Attribute '" + attributeName + "' does not exists.");
            //    }
            //    else
            //    {
            //        "".StoreInUserVariable(engine, v_Result);
            //    }
            //}

            var attributeName = this.ExpandValueOrUserVariable(nameof(v_AttributeName), "Attribute", engine);
            if (attributeName.StartsWith("@"))
            {
                // get special value
                var getSpecial = new SeleniumBrowserGetSpecialValueFromWebElementCommand()
                {
                    v_WebElement = this.v_WebElement,
                    v_ValueType = attributeName.Substring(1),
                    v_Result = this.v_Result,
                    v_ScrollToWebElement = this.v_ScrollToWebElement,
                    v_WhenFailAction = this.v_WhenFailAction,
                    v_WhenValueCanNotRetrieved = this.v_WhenValueCanNotRetrieved,
                };
                getSpecial.RunCommand(engine);
            }
            else
            {
                this.GetFromWebElementAction(new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((elem, seleniumInstance) =>
                {
                    var v = elem.GetAttribute(attributeName);

                    if (v != null)
                    {
                        v.StoreInUserVariable(engine, v_Result);
                    }
                    else
                    {
                        throw new Exception($"WebElement does not have Attribute. Attribute: '{v_AttributeName}', Expand: '{attributeName}'");
                    }
                }), this.StoreEmptyValueToResult, engine);
            }
        }

        //private void cmbScrollToElement_SelectionChange(object sender, EventArgs e)
        //{
        //    SeleniumBrowserControls.ScrollToWebElement_SelectionChange((ComboBox)sender, ControlsList, nameof(v_InstanceName));
        //}
    }
}