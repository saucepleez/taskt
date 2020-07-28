using SHDocVw;
using System;
using System.Collections.Generic;
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
    [Group("IE Browser Commands")]
    [Description("This command navigates an existing IE web browser session to a given URL or web resource.")]
    public class IENavigateToURLCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("IE Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("IEBrowser || {vIEBrowser}")]
        [Remarks("Failure to enter the correct instance or failure to first call **Create Browser** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Navigate to URL")]
        [InputSpecification("Enter the destination URL that you want the IE instance to navigate to.")]
        [SampleUsage("https://example.com/ || {vURL}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        public IENavigateToURLCommand()
        {
            CommandName = "IENavigateToURLCommand";
            SelectionName = "Navigate to URL";
            v_InstanceName = "default";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            object browserObject = null;

            var engine = (AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            browserObject = engine.GetAppInstance(vInstance);
            var browserInstance = (InternetExplorer)browserObject;

            browserInstance.Navigate(v_URL.ConvertToUserVariable(engine));
            IECreateBrowserCommand.WaitForReadyState(browserInstance);
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
            return base.GetDisplayValue() + $" [Navigate to '{v_URL}' - Instance Name '{v_InstanceName}']";
        }
    }
}