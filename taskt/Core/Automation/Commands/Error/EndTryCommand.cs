using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.Description("This command specifies the end of a try/catch block.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to enclose your try/catch block.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class EndTryCommand : ScriptCommand
    {
        public EndTryCommand()
        {
            this.CommandName = "EndTryCommand";
            this.SelectionName = "End Try";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
           //no execution required, used as a marker by the Automation Engine
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Comment", this, editor));
            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}