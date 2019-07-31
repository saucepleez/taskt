using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Engine;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.Description("This command specifies execution that should occur whether or not an error occured")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to always execute a specific command before leaving the try/catch block")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class FinallyCommand : ScriptCommand
    {
        public FinallyCommand()
        {
            this.CommandName = "FinallyCommand";
            this.SelectionName = "Finally";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender, Script.ScriptAction parentCommand)
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