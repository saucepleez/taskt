using SimpleNLG;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("NLG Commands")]
    [Attributes.ClassAttributes.CommandSettings("Create NLG Instance")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time specified in milliseconds.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to pause your script for a specific amount of time.  After the specified time is finished, the script will resume execution.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class NLGCreateNLGInstanceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NLGControls), nameof(NLGControls.v_InstanceName))]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        public string v_InstanceName { get; set; }

        public NLGCreateNLGInstanceCommand()
        {
            //this.CommandName = "NLGCreateInstanceCommand";
            //this.SelectionName = "Create NLG Instance";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_InstanceName = "";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
  
            Lexicon lexicon = Lexicon.getDefaultLexicon();
            NLGFactory nlgFactory = new NLGFactory(lexicon);
            SPhraseSpec p = nlgFactory.createClause();

            var vInstance = v_InstanceName.ConvertToUserVariable(sender);

            engine.AddAppInstance(vInstance, p);
        }
    }
}