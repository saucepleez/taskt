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
    [Attributes.ClassAttributes.Description("This command allows you to define a NLG parameter")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to define NLG parameters")]
    public class NLGSetParameterCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create NLG Instance** command")]
        [Attributes.PropertyAttributes.SampleUsage("**nlgDefaultInstance** or **myInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create NLG Instance** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the NLG Parameter Type")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create NLG Instance** command")]
        [Attributes.PropertyAttributes.SampleUsage("**nlgDefaultInstance** or **myInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create NLG Instance** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Set Subject")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Set Verb")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Set Object")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Complement")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Modifier")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Pre-Modifier")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Front Modifier")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Post Modifier")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ParameterType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the input")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the value that should be associated to the parameter")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Parameter { get; set; }

        public NLGSetParameterCommand()
        {
            this.CommandName = "NLGSetParameterCommand";
            this.SelectionName = "Set NLG Parameter";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_InstanceName = "nlgDefaultInstance";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var p = (SPhraseSpec)engine.GetAppInstance(vInstance);

            var userInput = v_Parameter.ConvertToUserVariable(sender);


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
        public override List<Control> Render(frmCommandEditor editor)
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