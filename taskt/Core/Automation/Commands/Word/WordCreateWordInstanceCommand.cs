using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command creates a Word Instance.")]
    [Attributes.ClassAttributes.CommandSettings("Create Word Instance")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Word.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordCreateWordInstanceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.TextBox)]
        [PropertyTextBoxSetting(1, false)]
        public string v_InstanceName { get; set; }

        public WordCreateWordInstanceCommand()
        {
            //this.CommandName = "WordCreateApplicationCommand";
            //this.SelectionName = "Create Word Application";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var newWordSession = new Microsoft.Office.Interop.Word.Application
            {
                Visible = true
            };

            engine.AddAppInstance(vInstance, newWordSession);
        }
    }
}