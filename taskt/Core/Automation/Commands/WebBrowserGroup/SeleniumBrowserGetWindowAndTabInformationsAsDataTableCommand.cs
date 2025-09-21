using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser")]
    [Attributes.ClassAttributes.SubGruop("Window/Tab")]
    [Attributes.ClassAttributes.CommandSettings("Get Window And Tab Informations As DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Handle, Title, and URL of Windows and Tabs.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Handle, Title, and URL of Windows and Tabs.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SeleniumBrowserGetWindowAndTabInformationsAsDataTableCommand : ScriptCommand, IDataTableResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SeleniumBrowserControls), nameof(SeleniumBrowserControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public string v_Result { get; set; }

        public SeleniumBrowserGetWindowAndTabInformationsAsDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var seleniumInstance = SeleniumBrowserControls.ExpandValueOrUserVariableAsSeleniumBrowserInstance(v_InstanceName, engine);

            var currentHandle = seleniumInstance.CurrentWindowHandle;

            var ret = this.CreateEmptyDataTable();
            ret.Columns.Add("handle");
            ret.Columns.Add("url");
            ret.Columns.Add("title");

            var handles = seleniumInstance.WindowHandles;
            foreach(var handle in handles)
            {
                seleniumInstance.SwitchTo().Window(handle);
                ret.Rows.Add(new string[] { handle, seleniumInstance.Url, seleniumInstance.Title });
            }

            seleniumInstance.SwitchTo().Window(currentHandle);

            this.StoreDataTableInUserVariable(ret, engine);
        }
    }
}