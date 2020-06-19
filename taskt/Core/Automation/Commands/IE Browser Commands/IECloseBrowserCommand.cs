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
    [Description("This command closes the associated IE Web Browser.")]
    public class IECloseBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("IE Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("IEBrowser || {vIEBrowser}")]
        [Remarks("Failure to enter the correct instance or failure to first call **Create Browser** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        public IECloseBrowserCommand()
        {
            CommandName = "IECloseBrowserCommand";
            SelectionName = "Close Browser";
            CommandEnabled = true;
            v_InstanceName = "default";
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);

            var browserInstance = (InternetExplorer)browserObject;
            browserInstance.Quit();

            engine.RemoveAppInstance(vInstance);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
        }
    }

}