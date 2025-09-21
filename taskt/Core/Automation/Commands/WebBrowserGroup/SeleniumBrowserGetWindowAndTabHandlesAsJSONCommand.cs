using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Window/Tab")]
    [Attributes.ClassAttributes.CommandSettings("Get Window And Tab Handles As JSON")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Web Browser Handles of Windows and Tabs.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Web Browser Handles of Windows and Tabs.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWindowAndTabHandlesAsJSONCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_OutputJSONName))]
        public string v_Result { get; set; }

        public SeleniumBrowserGetWindowAndTabHandlesAsJSONCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var myList = new InnerScriptVariable(engine))
            {
                var getHandles = new SeleniumBrowserGetWindowAndTabHandlesAsListCommand()
                {
                    v_InstanceName = this.v_InstanceName,
                    v_Result = myList.VariableName,
                };
                getHandles.RunCommand(engine);

                var convJson = new ConvertListToJSONCommand()
                {
                    v_List = myList.VariableName,
                    v_Result = this.v_Result,
                };
                convJson.RunCommand(engine);
            }
        }
    }
}