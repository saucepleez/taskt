using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Click WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Click to WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Click to WebElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserClickWebElementCommand : ASeleniumWebElementActionAndScrollCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        //public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_MouseClickType))]
        [PropertyUISelectionOption("Invoke Click")]
        [PropertyFirstValue("Invoke Click")]
        [PropertySelectionChangeEvent(nameof(cmdClickType_SelectinChange))]
        [PropertyParameterOrder(6000)]
        public string v_ClickType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        //[PropertyIsOptional(true)]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_XOffsetAdjustment))]
        [PropertyFirstValue("16")]
        [Remarks("It is strongly recommended to Enter a value between **10** and **20** depending on your WebBrowser.")]
        [PropertyParameterOrder(7000)]
        public string v_XOffset { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_YOffsetAdjustment))]
        [PropertyFirstValue("136")]
        [Remarks("It is strongly recommended to Enter a value between **100** and **200** depending on your WebBrowser.")]
        [PropertyParameterOrder(7100)]
        public string v_YOffset { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("When Fail Click")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyIsOptional(true, "Error")]
        //[PropertyDisplayText(false, "")]
        //public string v_WhenFailAction { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        //[PropertySelectionChangeEvent(nameof(cmbScrollToElement_SelectionChange))]
        //public string v_ScrollToWebElement { get; set; }

        public SeleniumBrowserClickWebElementCommand()
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
            //var clickType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ClickType), engine);

            //Action clickAction;
            //switch (clickType)
            //{
            //    case "invoke click":
            //        clickAction = new Action(() =>
            //        {
            //            elem.Click();
            //        });
            //        break;
            //    default:
            //        clickAction = new Action(() =>
            //        {
            //            var seleniumInstance = v_InstanceName.ExpandValueOrUserVariableAsSeleniumBrowserInstance(engine);
                        
            //            var scrollJson = JObject.Parse(SeleniumBrowserControls.ExcecuteScript(seleniumInstance, 
            //                                "return JSON.stringify({x: window.scrollX, y: window.scrollY})").ToString());
            //            var scrollX = scrollJson.Value<int>("x");
            //            var scrollY = scrollJson.Value<int>("y");

            //            var screenJson = JObject.Parse(SeleniumBrowserControls.ExcecuteScript(seleniumInstance,
            //                                "return JSON.stringify({x: window.screenX, y: window.screenY})").ToString());
            //            var screenX = screenJson.Value<int>("x");
            //            var screenY = screenJson.Value<int>("y");

            //            var elementLocation = elem.Location;

            //            //var html = seleniumInstance.FindElement(By.TagName("html"));
            //            //var clientX = html.GetAttribute("clientWidth");
            //            //var clientY = html.GetAttribute("clientHeight");

            //            // DBG
            //            //Console.WriteLine($"Elem x:{elementLocation.X}, y:{elementLocation.Y}");
            //            //Console.WriteLine($"Brow x:{screenX}, y:{screenY}");
            //            //Console.WriteLine($"Scroll x:{scrollX}, y:{scrollY}");

            //            var offsetX = this.ExpandValueOrUserVariableAsInteger(nameof(v_XOffset), engine);
            //            var offsetY = this.ExpandValueOrUserVariableAsInteger(nameof(v_YOffset), engine);

            //            var clickX = elementLocation.X - scrollX + screenX + offsetX;
            //            var clickY = elementLocation.Y - scrollY + screenY + offsetY;
                        
            //            // DBG
            //            //Console.WriteLine($"Click x:{clickX}, y:{clickY}");

            //            var clickCommand = new MoveMouseCommand()
            //            {
            //                v_MouseClick = this.v_ClickType,
            //                v_XMousePosition = clickX.ToString(),
            //                v_YMousePosition = clickY.ToString(),
            //            };
            //            clickCommand.RunCommand(engine);
            //        });
            //        break;
            //}

            //try
            //{
            //    clickAction();
            //}
            //catch
            //{
            //    if (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenFailAction), engine) == "error")
            //    {
            //        throw new Exception("Fail Click WebElement. Click Type: '" + clickType + "', Location: (" + elem.Location.X + ", " + elem.Location.Y + ")");
            //    }
            //}

            this.WebElementActionAndScroll(
                new Action<OpenQA.Selenium.IWebElement, OpenQA.Selenium.IWebDriver>((el, dr) =>
                {
                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ClickType), engine))
                    {
                        case "invoke click":
                            el.Click();
                            break;

                        default:
                            var scrollJson = JObject.Parse(
                                                SeleniumBrowserControls.ExcecuteScript(dr,
                                                    "return JSON.stringify({x: window.scrollX, y: window.scrollY})"
                                                ).ToString());
                            var scrollX = scrollJson.Value<int>("x");
                            var scrollY = scrollJson.Value<int>("y");

                            var screenJson = JObject.Parse(
                                                SeleniumBrowserControls.ExcecuteScript(dr,
                                                    "return JSON.stringify({x: window.screenX, y: window.screenY})"
                                                ).ToString());
                            var screenX = screenJson.Value<int>("x");
                            var screenY = screenJson.Value<int>("y");

                            var elementLocation = el.Location;

                            //var html = seleniumInstance.FindElement(By.TagName("html"));
                            //var clientX = html.GetAttribute("clientWidth");
                            //var clientY = html.GetAttribute("clientHeight");

                            // DBG
                            //Console.WriteLine($"Elem x:{elementLocation.X}, y:{elementLocation.Y}");
                            //Console.WriteLine($"Brow x:{screenX}, y:{screenY}");
                            //Console.WriteLine($"Scroll x:{scrollX}, y:{scrollY}");

                            var offsetX = this.ExpandValueOrUserVariableAsInteger(nameof(v_XOffset), engine);
                            var offsetY = this.ExpandValueOrUserVariableAsInteger(nameof(v_YOffset), engine);

                            var clickX = elementLocation.X - scrollX + screenX + offsetX;
                            var clickY = elementLocation.Y - scrollY + screenY + offsetY;

                            // DBG
                            //Console.WriteLine($"Click x:{clickX}, y:{clickY}");

                            var clickCommand = new MoveMouseCommand()
                            {
                                v_MouseClick = this.v_ClickType,
                                v_XMousePosition = clickX.ToString(),
                                v_YMousePosition = clickY.ToString(),
                            };
                            clickCommand.RunCommand(engine);
                            break;
                    }
                }), engine,
                new Action<Exception>(ex =>
                {
                    throw new Exception(EM_SeleniumWebElementActionPropertiesExtensionMehtods.GetFailActionMessage("Click"));
                })
            );
        }

        //private void cmbScrollToElement_SelectionChange(object sender, EventArgs e)
        //{
        //    //SeleniumBrowserControls.ScrollToWebElement_SelectionChange((ComboBox)sender, ControlsList, nameof(v_InstanceName));
        //    var useInstance = (((ComboBox)sender).SelectedItem?.ToString().ToLower() ?? "") != "no";

        //    var inst = ControlsList.GetPropertyControl<ComboBox>(nameof(v_InstanceName));
        //    useInstance = useInstance && ((inst.SelectedItem?.ToString().ToLower() ?? "") != "invoke click");

        //    FormUIControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_InstanceName), useInstance);
        //}

        private void cmdClickType_SelectinChange(object sender, EventArgs e)
        {
            var useOffset = (((ComboBox)sender).SelectedItem?.ToString().ToLower() ?? "") != "invoke click";
            FormUIControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_XOffset), useOffset); ;
            FormUIControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_YOffset), useOffset); ;

            var scroll = ControlsList.GetPropertyControl<ComboBox>(nameof(v_ScrollToWebElement));
            var useInstance = useOffset || ((scroll.SelectedItem?.ToString().ToLower() ?? "") != "no");
            //FormUIControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_InstanceName), useInstance);
        }
    }
}