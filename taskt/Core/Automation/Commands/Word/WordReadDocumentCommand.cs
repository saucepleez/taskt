using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to save a Word document.")]
    [Attributes.ClassAttributes.CommandSettings("Read Document")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to save changes to a document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordReadDocumentCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_UserVariableName { get; set; }

        public WordReadDocumentCommand()
        {
            //this.CommandName = "WordReadDocumentCommand";
            //this.SelectionName = "Read Document";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Engine.AutomationEngineInstance)sender;

            (var _, var wordDocument) = v_InstanceName.GetWordInstanceAndDocument(engine);

            //store text in variable
            string textFromDocument = wordDocument.Content.Text;
            textFromDocument.StoreInUserVariable(engine, v_UserVariableName);
        }
    }
}