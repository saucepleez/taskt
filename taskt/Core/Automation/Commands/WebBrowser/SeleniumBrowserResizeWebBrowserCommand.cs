using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Web Browser Actions")]
    [Attributes.ClassAttributes.CommandSettings("Resize Web Browser")]
    [Attributes.ClassAttributes.Description("This command allows you to change web browser window size.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change web browser window size.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserResizeWebBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Web Browser Window Width")]
        [InputSpecification("Width", true)]
        [PropertyDetailSampleUsage("**640**", PropertyDetailSampleUsage.ValueType.Value, "Window Width")]
        [PropertyDetailSampleUsage("**{{{vWidth}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Window Width")]
        [Remarks("Empty means Current Width")]
        [PropertyIsOptional(true, "Empty and means Current Width")]
        [PropertyDisplayText(true, "Width")]
        public string v_BrowserWidth { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Web Browser Window Height")]
        [InputSpecification("Height", true)]
        [PropertyDetailSampleUsage("**480**", PropertyDetailSampleUsage.ValueType.Value, "Window Height")]
        [PropertyDetailSampleUsage("**{{{vHeight}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Window Height")]
        [Remarks("Empty means Current Height")]
        [PropertyIsOptional(true, "Empty and means Current Height")]
        [PropertyDisplayText(true, "Height")]
        public string v_BrowserHeight { get; set; }

        public SeleniumBrowserResizeWebBrowserCommand()
        {
            //this.CommandName = "SeleniumBrowserResizeBrowserCommand";
            //this.SelectionName = "Resize Browser";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var seleniumInstance = v_InstanceName.GetSeleniumBrowserInstance(engine);

            var currentSize = seleniumInstance.Manage().Window.Size;

            int width;
            if (String.IsNullOrEmpty(v_BrowserWidth))
            {
                width = currentSize.Width;
            }
            else
            {
                width = this.ConvertToUserVariableAsInteger(nameof(v_BrowserWidth), engine);
            }

            int height;
            if (String.IsNullOrEmpty(v_BrowserHeight))
            {
                height = currentSize.Height;
            }
            else
            {
                height = this.ConvertToUserVariableAsInteger(nameof(v_BrowserHeight), engine);
            }
            seleniumInstance.Manage().Window.Size = new System.Drawing.Size(width, height);
        }
    }
}