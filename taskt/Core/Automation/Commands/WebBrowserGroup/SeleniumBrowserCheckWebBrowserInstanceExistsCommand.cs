using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.WebBrowserGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Check Web Browser Instance Exists")]
    [Attributes.ClassAttributes.Description("This command returns existance of Web Browser instance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check Web Browser Instance existance.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserCheckWebBrowserInstanceExistsCommand : ASeleniumGetFromWebDriverCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_WebBrowserControls), nameof(VP_WebBrowserControls.v_InputInstanceName))]
        public override string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When WebBrowser Instance Exists, Result is **True**")]
        [PropertyParameterOrder(6000)]
        public override string v_Result { get; set; }

        public SeleniumBrowserCheckWebBrowserInstanceExistsCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            try
            {
                var ins = this.GetWebBrowserIntance(v_InstanceName, engine);
                (ins != null).StoreInUserVariable(engine, v_Result);
            }
            catch
            {
                false.StoreInUserVariable(engine, v_Result);
            }
        }
    }
}