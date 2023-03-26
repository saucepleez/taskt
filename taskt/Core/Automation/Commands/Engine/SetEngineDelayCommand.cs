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
        [PropertyDisplayText(true, "Delay")]
        public string v_EngineSpeed { get; set; }

        public SetEngineDelayCommand()
        {
            //this.CommandName = "SetEngineDelayCommand";
            //this.SelectionName = "Set Engine Delay";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_EngineSpeed = "";
        }

        // TODO: add Run method

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_EngineSpeed", this, editor));

            this.v_EngineSpeed = editor.appSettings.EngineSettings.DelayBetweenCommands.ToString();
            
            return RenderedControls;
        }

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Set Delay to " + v_EngineSpeed + "ms between commands]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    this.IsValid = true;
        //    this.validationResult = "";

        //    int speedValue;

        //    if (String.IsNullOrEmpty(v_EngineSpeed))
        //    {
        //        this.validationResult += "Delay is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        if (int.TryParse(v_EngineSpeed, out speedValue))
        //        {
        //            if (speedValue < 0)
        //            {
        //                this.validationResult += "Specify a value of 0 or more for Delay.\n";
        //                this.IsValid = false;
        //            }
        //        }
        //    }

        //    return this.IsValid;
        //}
    }
}