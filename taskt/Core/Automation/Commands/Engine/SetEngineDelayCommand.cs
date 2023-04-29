using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.CommandSettings("Set Engine Delay")]
    [Attributes.ClassAttributes.Description("This command allows you to set delays between execution of commands in a running instance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change the execution speed between commands.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetEngineDelayCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Delay between Commands (in milliseconds)")]
        [InputSpecification("Delay", true)]
        [PropertyDetailSampleUsage("**1000**", PropertyDetailSampleUsage.ValueType.Value, "Delay")]
        [PropertyDetailSampleUsage("**{{{vTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Delay")]
        [PropertyValidationRule("Delay", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Delay", "ms")]
        public string v_EngineSpeed { get; set; }

        public SetEngineDelayCommand()
        {
            //this.CommandName = "SetEngineDelayCommand";
            //this.SelectionName = "Set Engine Delay";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_EngineSpeed = "";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var delay = this.ConvertToUserVariableAsInteger(nameof(v_EngineSpeed), engine);
            engine.engineSettings.DelayBetweenCommands = delay;
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_EngineSpeed", this, editor));

            // TODO: support keyword
            this.v_EngineSpeed = editor.appSettings.EngineSettings.DelayBetweenCommands.ToString();
            
            return RenderedControls;
        }
    }
}