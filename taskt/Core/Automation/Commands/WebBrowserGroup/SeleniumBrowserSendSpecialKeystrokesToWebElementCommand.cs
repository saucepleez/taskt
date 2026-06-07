using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Send Special Keystrokes To WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Send Special Keystrokes in WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to SendSpecial  Keystrokes in WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserSendSpecialKeystrokesToWebElementCommand : ASeleniumWebElementActionAndScrollCommands, ISeleniumSendSpecialKeystrokesProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        //public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Send Key")]
        [PropertyValidationRule("Send Key", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Send Key")]
        [PropertyComboBoxItemMethod(nameof(CreateSendKeyList))]
        [PropertyParameterOrder(6000)]
        public string v_SendKey { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Use Control Key")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyParameterOrder(6100)]
        public string v_ControlKey { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Use Shift Key")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyParameterOrder(6200)]
        public string v_ShiftKey { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Use Alt Key")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyParameterOrder(6300)]
        public string v_AltKey { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("When the WebElement does not support Set Text")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyIsOptional(true, "Error")]
        //[PropertyDisplayText(false, "")]
        //public string v_WhenFailAction { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        //[PropertySelectionChangeEvent(nameof(cmbScrollToElement_SelectionChange))]
        //public string v_ScrollToWebElement { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //[PropertyIsOptional(true)]
        //public string v_InstanceName { get; set; }

        public SeleniumBrowserSendSpecialKeystrokesToWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ScrollToWebElement), engine))
            //{
            //    var scroll = new SeleniumBrowserScrollToWebElementCommand
            //    {
            //        //v_InstanceName = this.v_InstanceName,
            //        v_WebElement = this.v_WebElement,
            //        v_WhenFailAction = "ignore"
            //    };
            //    scroll.RunCommand(engine);
            //}

            //var elem = v_WebElement.ExpandUserVariableAsWebElement("WebElement", engine);

            //string sendKey = "";

            //if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ControlKey), engine))
            //{
            //    sendKey += OpenQA.Selenium.Keys.Control;
            //}
            //if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ShiftKey), engine))
            //{
            //    sendKey += OpenQA.Selenium.Keys.Shift;
            //}
            //if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_AltKey), engine))
            //{
            //    sendKey += OpenQA.Selenium.Keys.Alt;
            //}

            //// TODO: not case sensitive
            //// send key
            //var key = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_SendKey), "Send Key", engine);

            //// get key value
            //var tp = typeof(OpenQA.Selenium.Keys);
            //var info = tp.GetField(key);

            //sendKey += $"{info.GetValue(null)}";

            //try
            //{
            //    elem.SendKeys(sendKey);
            //}
            //catch
            //{
            //    if (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFailAction), engine) == "error")
            //    {
            //        throw new Exception("Fail Setting Text. TagName: '" + elem.TagName + "'");
            //    }
            //}

            this.WebElementActionAndScroll(
                new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((el, dr) =>
                {
                    var sendKey = string.Empty;

                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ControlKey), engine))
                    {
                        sendKey += OpenQA.Selenium.Keys.Control;
                    }
                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ShiftKey), engine))
                    {
                        sendKey += OpenQA.Selenium.Keys.Shift;
                    }
                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_AltKey), engine))
                    {
                        sendKey += OpenQA.Selenium.Keys.Alt;
                    }

                    // TODO: not case sensitive
                    // send key
                    var key = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_SendKey), "Send Key", engine);

                    // get key value
                    var tp = typeof(OpenQA.Selenium.Keys);
                    var info = tp.GetField(key);

                    sendKey += $"{info.GetValue(null)}";

                    el.SendKeys(sendKey);
                }), engine,
                new Action<Exception>(ex =>
                {
                    throw new Exception($"{EM_SeleniumWebElementActionPropertiesExtensionMehtods.GetFailActionMessage("Send Key")} SendKey: '{v_SendKey}'");
                })
            );
        }

        //private void cmbScrollToElement_SelectionChange(object sender, EventArgs e)
        //{
        //    SeleniumBrowserControls.ScrollToWebElement_SelectionChange((ComboBox)sender, ControlsList, nameof(v_InstanceName));
        //}

        /// <summary>
        /// Get key list of OpenQA
        /// </summary>
        /// <returns></returns>
        private List<string> CreateSendKeyList()
        {
            //var fields = typeof(OpenQA.Selenium.Keys).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            //return fields.Select(f => f.Name).ToList();

            // call extention method
            return this.GetSpecialKeysList();
        }
    }
}