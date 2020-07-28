using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command allows you to set delays between execution of commands in a running instance.")]
    [UsesDescription("Use this command when you want to change the execution speed between commands.")]
    [ImplementationDescription("")]
    public class SetEngineDelayCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Set Delay between commands (in milliseconds).")]
        [InputSpecification("Enter a specific amount of time in milliseconds (ex. to specify 8 seconds, one would enter 8000) or specify a variable containing a value.")]
        [SampleUsage("**250** or **[vVariableSpeed]**")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_EngineSpeed { get; set; }

        public SetEngineDelayCommand()
        {
            CommandName = "SetEngineDelayCommand";
            SelectionName = "Set Engine Delay";
            CommandEnabled = true;
            CustomRendering = true;
            v_EngineSpeed = "250";
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_EngineSpeed", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set Delay to " + v_EngineSpeed + "ms between commands]";
        }
    }
}