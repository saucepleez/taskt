using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Switch Commands")]
    [Description("This command defines a case block whose commands will execute if the value specified in the "+
                 "case is equal to that of the preceding Switch Command.")]
    public class CaseCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Case")]
        [InputSpecification("This block will be executed if the specified case value matches the value in the Switch Command.")]
        [SampleUsage("1 || hello")]
        [Remarks("")]
        public string v_CaseValue { get; set; }

        public CaseCommand()
        {
            CommandName = "CaseCommand";
            SelectionName = "Case";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //no execution required, used as a marker by the Automation Engine
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_CaseValue", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            if (v_CaseValue == "Default")
                return "Default:";
            else
                return base.GetDisplayValue() + $" {v_CaseValue}:";
        }
    }
}
