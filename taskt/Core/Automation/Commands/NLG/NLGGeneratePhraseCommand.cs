using SimpleNLG;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("NLG Commands")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time specified in milliseconds.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to pause your script for a specific amount of time.  After the specified time is finished, the script will resume execution.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class NLGGeneratePhraseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create NLG Instance** command")]
        [Attributes.PropertyAttributes.SampleUsage("**nlgDefaultInstance** or **myInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create NLG Instance** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Variable to Receive Output")]
        public string v_applyToVariableName { get; set; }

        public NLGGeneratePhraseCommand()
        {
            this.CommandName = "NLGGeneratePhraseCommand";
            this.SelectionName = "Generate NLG Phrase";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_InstanceName = "nlgDefaultInstance";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var p = (SPhraseSpec)engine.GetAppInstance(vInstance);

            Lexicon lexicon = Lexicon.getDefaultLexicon();
            Realiser realiser = new Realiser(lexicon);

            String phraseOutput = realiser.realiseSentence(p);
            phraseOutput.StoreInUserVariable(sender, v_applyToVariableName);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            //apply to variable name
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var applyToVariableControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { applyToVariableControl }, editor));
            RenderedControls.Add(applyToVariableControl);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply to '" + v_applyToVariableName +  "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}