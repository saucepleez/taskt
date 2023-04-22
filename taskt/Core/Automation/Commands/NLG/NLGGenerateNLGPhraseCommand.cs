using SimpleNLG;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("NLG Commands")]
    [Attributes.ClassAttributes.CommandSettings("Generate NLG Phrase")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time specified in milliseconds.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to pause your script for a specific amount of time.  After the specified time is finished, the script will resume execution.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class NLGGenerateNLGPhraseCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NLGControls), nameof(NLGControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public NLGGenerateNLGPhraseCommand()
        {
            //this.CommandName = "NLGGeneratePhraseCommand";
            //this.SelectionName = "Generate NLG Phrase";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_InstanceName = "";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var p = (SPhraseSpec)engine.GetAppInstance(vInstance);

            Lexicon lexicon = Lexicon.getDefaultLexicon();
            Realiser realiser = new Realiser(lexicon);

            String phraseOutput = realiser.realiseSentence(p);
            phraseOutput.StoreInUserVariable(sender, v_applyToVariableName);
        }
    }
}