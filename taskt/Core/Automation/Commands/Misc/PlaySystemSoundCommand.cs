using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Play System Sound")]
    [Attributes.ClassAttributes.Description("This command allows you to Play System Sound.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Play System Sound.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class PlaySystemSoundCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select Sound to Play")]
        [InputSpecification("")]
        [SampleUsage("**Asterisk** or **Beep** or **Excalamation** or **Hand** or **Question**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Asterisk")]
        [PropertyUISelectionOption("Beep")]
        [PropertyUISelectionOption("Exclamation")]
        [PropertyUISelectionOption("Hand")]
        [PropertyUISelectionOption("Question")]
        [PropertyValidationRule("Sound", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Sound")]
        public string v_SoundType { get; set; }

        public PlaySystemSoundCommand()
        {
            //this.CommandName = "PlaySystemSoundCommand";
            //this.SelectionName = "Play System Sound";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (taskt.Core.Automation.Engine.AutomationEngineInstance)sender;
            string sound = v_SoundType.GetUISelectionValue("v_SoundType", this, engine);
            switch (sound)
            {
                case "asterisk":
                    System.Media.SystemSounds.Asterisk.Play();
                    break;
                case "beep":
                    System.Media.SystemSounds.Beep.Play();
                    break;
                case "exclamation":
                    System.Media.SystemSounds.Exclamation.Play();
                    break;
                case "hand":
                    System.Media.SystemSounds.Hand.Play();
                    break;
                case "question":
                    System.Media.SystemSounds.Question.Play();
                    break;
            }
        }
    }
}
