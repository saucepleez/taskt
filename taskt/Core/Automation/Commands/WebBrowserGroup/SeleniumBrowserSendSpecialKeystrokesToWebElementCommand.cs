using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

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
    public sealed class SeleniumBrowserSendSpecialKeystrokesToWebElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Send Key")]
        [PropertyValidationRule("Send Key", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Send Key")]
        [PropertyComboBoxItemMethod(nameof(CreateSendKeyList))]
        public string v_SendKey { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Use Control Key")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        public string v_ControlKey { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Use Shift Key")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        public string v_ShiftKey { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Use Alt Key")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        public string v_AltKey { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When the WebElement does not support Set Text")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "Error")]
        [PropertyDisplayText(false, "")]
        public string v_WhenSetNotSupported { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        [PropertySelectionChangeEvent(nameof(cmbScrollToElement_SelectionChange))]
        public string v_ScrollToElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        [PropertyIsOptional(true)]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserSendSpecialKeystrokesToWebElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ScrollToElement), engine))
            {
                var scroll = new SeleniumBrowserScrollToWebElementCommand
                {
                    v_InstanceName = this.v_InstanceName,
                    v_WebElement = this.v_WebElement,
                    v_WhenFailScroll = "ignore"
                };
                scroll.RunCommand(engine);
            }

            var elem = v_WebElement.ExpandUserVariableAsWebElement("WebElement", engine);

            string sendKey = "";

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
            //var keys = this.CreateSendKeyList();
            //string fieldKeyName = "";
            //foreach(var k in keys)
            //{
            //    if (key == k.ToLower())
            //    {
            //        fieldKeyName = k;
            //    }
            //}

            // get key value
            var tp = typeof(OpenQA.Selenium.Keys);
            var info = tp.GetField(key);

            sendKey += $"{info.GetValue(null)}";

            try
            {
                elem.SendKeys(sendKey);
            }
            catch
            {
                if (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenSetNotSupported), engine) == "error")
                {
                    throw new Exception("Fail Setting Text. TagName: '" + elem.TagName + "'");
                }
            }
        }

        private void cmbScrollToElement_SelectionChange(object sender, EventArgs e)
        {
            SeleniumBrowserControls.ScrollToWebElement_SelectionChange((ComboBox)sender, ControlsList, nameof(v_InstanceName));
        }

        /// <summary>
        /// OpenQAのキー一覧を取得する
        /// </summary>
        /// <returns></returns>
        private List<string> CreateSendKeyList()
        {
            var fields = typeof(OpenQA.Selenium.Keys).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            return fields.Select(f => f.Name).ToList();
        }
    }
}