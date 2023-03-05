using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Check Browser Instance Exists")]
    [Attributes.ClassAttributes.Description("This command returns existance of browser instance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an open instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SeleniumBrowserCheckBrowserInstanceExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When WebBrowser Instance Exists, Result is **True**")]
        public string v_applyToVariableName { get; set; }

        public SeleniumBrowserCheckBrowserInstanceExistsCommand()
        {
            //this.CommandName = "SeleniumBrowserCheckBrowserInstanceExistsCommand";
            //this.SelectionName = "Check Browser Instance Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //this.v_InstanceName = "";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            try
            {
                var _ = v_InstanceName.GetSeleniumBrowserInstance(engine);
                true.StoreInUserVariable(engine, v_applyToVariableName);
            }
            catch
            {
                false.StoreInUserVariable(engine, v_applyToVariableName);
            }
        }
    }
}