using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command adds a new Word Document.")]
    [Attributes.ClassAttributes.CommandSettings("Add Document")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a new document to a Word Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordAddDocumentCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        public WordAddDocumentCommand()
        {
            //this.CommandName = "WordAddDocumentCommand";
            //this.SelectionName = "Add Document";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var wordInstance = v_InstanceName.GetWordInstance(engine);

            wordInstance.Documents.Add();
        }
    }
}