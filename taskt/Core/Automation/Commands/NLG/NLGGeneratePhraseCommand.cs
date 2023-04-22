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
    public class NLGGeneratePhraseCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create NLG Instance** command")]
        //[SampleUsage("**nlgDefaultInstance** or **myInstance**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create NLG Instance** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.NLG)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(NLGControls), nameof(NLGControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Select Variable to Receive Output")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public NLGGeneratePhraseCommand()
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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultNLGInstanceName;
        //    }

        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.NLG);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

        //    //apply to variable name
        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    var applyToVariableControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_applyToVariableName", this, applyToVariableControl, editor));
        //    RenderedControls.Add(applyToVariableControl);

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Apply to '" + v_applyToVariableName +  "', Instance Name: '" + v_InstanceName + "']";
        //}
    }
}