using SimpleNLG;
using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("NLG Commands")]
    [Attributes.ClassAttributes.CommandSettings("Set NLG Parameter")]
    [Attributes.ClassAttributes.Description("This command allows you to define a NLG parameter")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to define NLG parameters")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class NLGSetNLGParameterCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NLGControls), nameof(NLGControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("NLG Parameter Type")]
        [PropertyUISelectionOption("Set Subject")]
        [PropertyUISelectionOption("Set Verb")]
        [PropertyUISelectionOption("Set Object")]
        [PropertyUISelectionOption("Add Complement")]
        [PropertyUISelectionOption("Add Modifier")]
        [PropertyUISelectionOption("Add Pre-Modifier")]
        [PropertyUISelectionOption("Add Front Modifier")]
        [PropertyUISelectionOption("Add Post Modifier")]
        [PropertyValidationRule("Parameter Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Paramter Type")]
        public string v_ParameterType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_OneLineTextBox))]
        [PropertyDescription("Parameter Value")]
        [InputSpecification("Parameter Value")]
        [PropertyDisplayText(true, "Parameter Value")]
        public string v_Parameter { get; set; }

        public NLGSetNLGParameterCommand()
        {
            //this.CommandName = "NLGSetParameterCommand";
            //this.SelectionName = "Set NLG Parameter";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_InstanceName = "";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var p = (SPhraseSpec)engine.GetAppInstance(vInstance);

            var userInput = v_Parameter.ConvertToUserVariable(sender);

            switch (this.GetUISelectionValue(nameof(v_ParameterType), engine))
            {
                case "set subject":
                    p.setSubject(userInput);
                    break;
                case "set object":
                    p.setObject(userInput);
                    break;
                case "set verb":
                    p.setVerb(userInput);
                    break;
                case "add complement":
                    p.addComplement(userInput);
                    break;
                case "add modifier":
                    p.addModifier(userInput);             
                    break;
                case "add front modifier":
                    p.addFrontModifier(userInput);
                    break;
                case "add post modifier":
                    p.addPostModifier(userInput);
                    break;
                case "add pre-modifier":
                    p.addPreModifier(userInput);
                    break;
            }

            //remove existing associations if override app instances is not enabled
            engine.AppInstances.Remove(vInstance);

            //add to app instance to track
            engine.AddAppInstance(vInstance, p);
        }
    }
}