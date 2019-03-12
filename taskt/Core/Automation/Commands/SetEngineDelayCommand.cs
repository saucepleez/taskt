using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to set delays between execution of commands in a running instance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change the execution speed between commands.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SetEngineDelayCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Set Delay between commands (in milliseconds).")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a specific amount of time in milliseconds (ex. to specify 8 seconds, one would enter 8000) or specify a variable containing a value.")]
        [Attributes.PropertyAttributes.SampleUsage("**250** or **[vVariableSpeed]**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_EngineSpeed { get; set; }

        public SetEngineDelayCommand()
        {
            this.CommandName = "SetEngineDelayCommand";
            this.SelectionName = "Set Engine Delay";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_EngineSpeed = "250";
        }
        public override List<Control> Render(frmCommandEditor editor)
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