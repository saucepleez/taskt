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
    [Description("This command allows you to define a NLG parameter")]
    [UsesDescription("Use this command when you want to define NLG parameters")]
    public class SetNLGParameterCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create NLG Instance** command")]
        [SampleUsage("**nlgDefaultInstance** or **myInstance**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create NLG Instance** command will cause an error")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the NLG Parameter Type")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create NLG Instance** command")]
        [SampleUsage("**nlgDefaultInstance** or **myInstance**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create NLG Instance** command will cause an error")]
        [PropertyUISelectionOption("Set Subject")]
        [PropertyUISelectionOption("Set Verb")]
        [PropertyUISelectionOption("Set Object")]
        [PropertyUISelectionOption("Add Complement")]
        [PropertyUISelectionOption("Add Modifier")]
        [PropertyUISelectionOption("Add Pre-Modifier")]
        [PropertyUISelectionOption("Add Front Modifier")]
        [PropertyUISelectionOption("Add Post Modifier")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ParameterType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please define the input")]
        [InputSpecification("Enter the value that should be associated to the parameter")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Parameter { get; set; }

        public SetNLGParameterCommand()
        {
            CommandName = "SetNLGParameterCommand";
            SelectionName = "Set NLG Parameter";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "nlgDefaultInstance";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var p = (SPhraseSpec)engine.GetAppInstance(vInstance);

            var userInput = v_Parameter.ConvertToUserVariable(engine);


            switch (v_ParameterType)
            {
                case "Set Subject":
                    p.setSubject(userInput);
                    break;
                case "Set Object":
                    p.setObject(userInput);
                    break;
                case "Set Verb":
                    p.setVerb(userInput);
                    break;
                case "Add Complement":
                    p.addComplement(userInput);
                    break;
                case "Add Modifier":
                    p.addModifier(userInput);             
                    break;
                case "Add Front Modifier":
                    p.addFrontModifier(userInput);
                    break;
                case "Add Post Modifier":
                    p.addPostModifier(userInput);
                    break;
                case "Add Pre-Modifier":
                    p.addPreModifier(userInput);
                    break;
                default:
                    break;
            }

            //remove existing associations if override app instances is not enabled
            engine.AppInstances.Remove(vInstance);

            //add to app instance to track
            engine.AddAppInstance(vInstance, p);


        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ParameterType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Parameter", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_ParameterType + ": '" + v_Parameter + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}