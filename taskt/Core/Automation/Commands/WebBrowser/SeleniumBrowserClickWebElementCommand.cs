using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("WebElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Click WebElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Click to WebElement.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Click to WebElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserClickWebElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputWebElementName))]
        public string v_WebElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_MouseClickType))]
        [PropertyUISelectionOption("Invoke Click")]
        [PropertyFirstValue("Invoke Click")]
        [PropertySelectionChangeEvent(nameof(cmdClickType_SelectinChange))]
        public string v_ClickType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        [PropertyIsOptional(true)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_XOffsetAdjustment))]
        [PropertyFirstValue("10")]
        [Remarks("It is strongly recommended to Enter a value between **10** and **20** depending on your WebBrowser.")]
        public string v_XOffset { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(KeyMouseControls), nameof(KeyMouseControls.v_YOffsetAdjustment))]
        [PropertyFirstValue("136")]
        [Remarks("It is strongly recommended to Enter a value between **100** and **200** depending on your WebBrowser.")]
        public string v_YOffset { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("When Fail Click")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyIsOptional(true, "Error")]
        [PropertyDisplayText(false, "")]
        public string v_WhenFailClick { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_ScrollToElement))]
        [PropertySelectionChangeEvent(nameof(cmbScrollToElement_SelectionChange))]
        public string v_ScrollToElement { get; set; }

        public SeleniumBrowserClickWebElementCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            if (this.GetYesNoSelectionValue(nameof(v_ScrollToElement), engine))
            {
                var scrollCommand = new SeleniumBrowserScrollToWebElementCommand()
                {
                    v_InstanceName = this.v_InstanceName,
                    v_WebElement = this.v_WebElement,
                    v_WhenFailScroll = "ignore"
                };
                scrollCommand.RunCommand(engine);
            }

            var elem = v_WebElement.ConvertToUserVariableAsWebElement("WebElement", engine);
            var clickType = this.GetUISelectionValue(nameof(v_ClickType), engine);

            Action clickAction;
            switch (clickType)
            {
                case "invoke click":
                    clickAction = new Action(() =>
                    {
                        elem.Click();
                    });
                    break;
                default:
                    clickAction = new Action(() =>
                    {
                        var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);
                        
                        var scrollJson = JObject.Parse(SeleniumBrowserControls.ExcecuteScript(seleniumInstance, 
                                            "return JSON.stringify({x: window.scrollX, y: window.scrollY})").ToString());
                        var scrollX = scrollJson.Value<int>("x");
                        var scrollY = scrollJson.Value<int>("y");

                        var screenJson = JObject.Parse(SeleniumBrowserControls.ExcecuteScript(seleniumInstance,
                                            "return JSON.stringify({x: window.screenX, y: window.screenY})").ToString());
                        var screenX = screenJson.Value<int>("x");
                        var screenY = screenJson.Value<int>("y");

                        var elementLocation = elem.Location;

                        //var html = seleniumInstance.FindElement(By.TagName("html"));
                        //var clientX = html.GetAttribute("clientWidth");
                        //var clientY = html.GetAttribute("clientHeight");

                        // DBG
                        //Console.WriteLine($"Elem x:{elementLocation.X}, y:{elementLocation.Y}");
                        //Console.WriteLine($"Brow x:{screenX}, y:{screenY}");
                        //Console.WriteLine($"Scroll x:{scrollX}, y:{scrollY}");

                        var offsetX = this.ConvertToUserVariableAsInteger(nameof(v_XOffset), engine);
                        var offsetY = this.ConvertToUserVariableAsInteger(nameof(v_YOffset), engine);

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
                    });
                    break;
            }

            try
            {
                clickAction();
            }
            catch
            {
                if (this.GetUISelectionValue(nameof(v_WhenFailClick), engine) == "error")
                {
                    throw new Exception("Fail Click WebElement. Click Type: '" + clickType + "', Location: (" + elem.Location.X + ", " + elem.Location.Y + ")");
                }
            }
        }

        private void cmbScrollToElement_SelectionChange(object sender, EventArgs e)
        {
            //SeleniumBrowserControls.ScrollToWebElement_SelectionChange((ComboBox)sender, ControlsList, nameof(v_InstanceName));
            var useInstance = (((ComboBox)sender).SelectedItem?.ToString().ToLower() ?? "") != "no";

            var inst = ControlsList.GetPropertyControl<ComboBox>(nameof(v_InstanceName));
            useInstance = useInstance && ((inst.SelectedItem?.ToString().ToLower() ?? "") != "invoke click");

            GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_InstanceName), useInstance);
        }

        private void cmdClickType_SelectinChange(object sender, EventArgs e)
        {
            var useOffset = (((ComboBox)sender).SelectedItem?.ToString().ToLower() ?? "") != "invoke click";
            GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_XOffset), useOffset); ;
            GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_YOffset), useOffset); ;

            var scroll = ControlsList.GetPropertyControl<ComboBox>(nameof(v_ScrollToElement));
            var useInstance = useOffset || ((scroll.SelectedItem?.ToString().ToLower() ?? "") != "no");
            GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_InstanceName), useInstance);
        }
    }
}