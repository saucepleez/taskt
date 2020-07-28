using OpenQA.Selenium;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Web Browser Commands")]
    [Description("This command allows you to navigate a Selenium web browser session to a given URL or resource.")]

    public class SeleniumNavigateToURLCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Browser Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("MyBrowserInstance || {vBrowserInstance}")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("URL")]
        [InputSpecification("Enter the URL that you want the selenium instance to navigate to.")]
        [SampleUsage("https://mycompany.com/orders || {vURL}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        public SeleniumNavigateToURLCommand()
        {
            CommandName = "SeleniumNavigateToURLCommand";
            SelectionName = "Navigate to URL";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultBrowser";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var browserObject = engine.GetAppInstance(vInstance);
            var seleniumInstance = (IWebDriver)browserObject;
            seleniumInstance.Navigate().GoToUrl(v_URL.ConvertToUserVariable(engine));
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_URL", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [URL '{v_URL}' - Instance Name '{v_InstanceName}']";
        }

        private void WaitForReadyState(InternetExplorer ieInstance)
        {
            DateTime waitExpires = DateTime.Now.AddSeconds(15);
            do
            {
                Thread.Sleep(500);
            }
            while ((ieInstance.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
        }
    }
}