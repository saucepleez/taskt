using SimpleNLG;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("NLG Commands")]
    [Description("This command pauses the script for a set amount of time specified in milliseconds.")]
    [UsesDescription("Use this command when you want to pause your script for a specific amount of time.  After the specified time is finished, the script will resume execution.")]
    [ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class CreateNLGInstanceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create NLG Instance** command")]
        [SampleUsage("**nlgDefaultInstance** or **myInstance**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create NLG Instance** command will cause an error")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        public CreateNLGInstanceCommand()
        {
            CommandName = "CreateNLGInstanceCommand";
            SelectionName = "Create NLG Instance";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "nlgDefaultInstance";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
  
            Lexicon lexicon = Lexicon.getDefaultLexicon();
            NLGFactory nlgFactory = new NLGFactory(lexicon);
            SPhraseSpec p = nlgFactory.createClause();

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            engine.AddAppInstance(vInstance, p);

        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
}