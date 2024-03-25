using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to save a Word document.")]
    [Attributes.ClassAttributes.CommandSettings("Save Document")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to save changes to a document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordSaveDocumentCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        public WordSaveDocumentCommand()
        {
            //this.CommandName = "WordSaveCommand";
            //this.SelectionName = "Save Document";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var wordInstance = v_InstanceName.ExpandValueOrUserVariableAsWordInstance(engine);

            //save
            wordInstance.ActiveDocument.Save();
        }
    }
}