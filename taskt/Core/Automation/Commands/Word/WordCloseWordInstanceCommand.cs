using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close Word Instance.")]
    [Attributes.ClassAttributes.CommandSettings("Close Word Instance")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an open instance of Word.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordCloseWordInstanceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Document should be Saved")]
        [PropertyUISelectionOption("TRUE")]
        [PropertyUISelectionOption("FALSE")]
        [PropertyDisplayText(false, "")]
        public string v_WordSaveOnExit { get; set; }

        public WordCloseWordInstanceCommand()
        {
            //this.CommandName = "WordCloseApplicationCommand";
            //this.SelectionName = "Close Word Application";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var wordObject = engine.GetAppInstance(vInstance);

            var wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;


            //check if document exists and save
            if (wordInstance.Documents.Count >= 1)
            {
                var isSave = v_WordSaveOnExit.ConvertToUserVariableAsBool("Document should be saved", engine);
                wordInstance.ActiveDocument.Close(isSave);
            }

            //close word
            wordInstance.Quit();

            //remove instance
            engine.RemoveAppInstance(vInstance);
        }
    }
}