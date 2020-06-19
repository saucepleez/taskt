using SHDocVw;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
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
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Navigate to URL")]
        [InputSpecification("Enter the destination URL that you want the IE instance to navigate to.")]
        [SampleUsage("https://example.com/ || {vURL}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
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

            browserInstance.Navigate(v_URL.ConvertToUserVariable(sender));
            IECreateBrowserCommand.WaitForReadyState(browserInstance);
        }

        public override List<Control> Render(frmCommandEditor editor)
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