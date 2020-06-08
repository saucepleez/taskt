using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Utilities.CommonUtilities;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close the associated IE web browser")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IECloseBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public IECloseBrowserCommand()
        {
            this.CommandName = "IECloseBrowserCommand";
            this.SelectionName = "Close Browser";
            this.CommandEnabled = true;
            this.v_InstanceName = "default";
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);


            var browserInstance = (SHDocVw.InternetExplorer)browserObject;
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
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }

}