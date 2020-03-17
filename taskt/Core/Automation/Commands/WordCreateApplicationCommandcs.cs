using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command creates a Word Application.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Word.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    public class WordCreateApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Signifies a unique name that will represemt the application instance.  This unique name allows you to refer to the instance by name in future commands, ensuring that the commands you specify run against the correct application.")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **wordInstance**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        public WordCreateApplicationCommand()
        {
            this.CommandName = "WordCreateApplicationCommand";
            this.SelectionName = "Create Word Application";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);


            var newWordSession = new Word.Application
            {
                Visible = true
            };

            engine.AddAppInstance(vInstance, newWordSession);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
}