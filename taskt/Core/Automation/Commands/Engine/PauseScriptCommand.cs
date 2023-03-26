using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.CommandSettings("Pause Script")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time specified in milliseconds.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to pause your script for a specific amount of time.  After the specified time is finished, the script will resume execution.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class PauseScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Amount of Time to Pause for (in milliseconds)")]
        [InputSpecification("Pause Time", true)]
        [PropertyDetailSampleUsage("**8000**", PropertyDetailSampleUsage.ValueType.Value, "Pause")]
        [PropertyDetailSampleUsage("**{{{vTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Pause")]
        [PropertyValidationRule("Pause", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Pause", "ms")]
        public string v_PauseLength { get; set; }

        public PauseScriptCommand()
        {
            //this.CommandName = "PauseCommand";
            //this.SelectionName = "Pause Script";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var pauseLength = this.ConvertToUserVariableAsInteger(nameof(v_PauseLength), engine);

            //var userPauseLength = v_PauseLength.ConvertToUserVariable(sender);
            //var pauseLength = int.Parse(userPauseLength);
            System.Threading.Thread.Sleep(pauseLength);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PauseLength", this, editor));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Wait for " + v_PauseLength + "ms]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    this.IsValid = true;
        //    this.validationResult = "";

        //    int pauseValue;

        //    if (String.IsNullOrEmpty(v_PauseLength))
        //    {
        //        this.validationResult += "Time of pause is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        if (int.TryParse(v_PauseLength, out pauseValue))
        //        {
        //            if (pauseValue < 0)
        //            {
        //                this.validationResult += "Specify a value of 0 or more for Time of pause.\n";
        //                this.IsValid = false;
        //            }
        //        }
        //    }

        //    return this.IsValid;
        //}
    }
}