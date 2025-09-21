using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Window/Tab")]
    [Attributes.ClassAttributes.CommandSettings("Get Window And Tab Informations As JSON")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Web Browser Informations of Windows and Tabs.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Web Browser Informations of Windows and Tabs.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWindowAndTabInformationsAsJSONCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_OutputJSONName))]
        public string v_Result { get; set; }

        public SeleniumBrowserGetWindowAndTabInformationsAsJSONCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var myList = new InnerScriptVariable(engine))
            {
                var getInfos = new SeleniumBrowserGetWindowAndTabInformationsAsDataTableCommand()
                {
                    v_InstanceName = this.v_InstanceName,
                    v_Result = myList.VariableName,
                };
                getInfos.RunCommand(engine);

                var convJson = new ConvertDataTableToJSONCommand()
                {
                    v_DataTable = myList.VariableName,
                    v_Result = this.v_Result,
                };
                convJson.RunCommand(engine);
            }
        }
    }
}