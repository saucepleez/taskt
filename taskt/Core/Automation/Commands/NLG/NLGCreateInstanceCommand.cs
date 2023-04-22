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
    public class NLGCreateInstanceCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create NLG Instance** command")]
        //[SampleUsage("**nlgDefaultInstance** or **myInstance**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create NLG Instance** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.NLG)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyVirtualProperty(nameof(NLGControls), nameof(NLGControls.v_InstanceName))]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        public string v_InstanceName { get; set; }

        public NLGCreateInstanceCommand()
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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.NLG);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultNLGInstanceName;
        //    }

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        //}
    }
}