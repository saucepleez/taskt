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
using Application = Microsoft.Office.Interop.Word.Application;

namespace taskt.Commands
{
    [Serializable]
    [Group("Word Commands")]
    [Description("This command closes an open Word Document and Instance.")]

    public class WordCloseApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance || {vWordInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Save Document")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether the Document should be saved before closing.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_WordSaveOnExit { get; set; }

        public WordCloseApplicationCommand()
        {
            CommandName = "WordCloseApplicationCommand";
            SelectionName = "Close Word Application";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
            v_WordSaveOnExit = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var wordObject = engine.GetAppInstance(vInstance);
            Application wordInstance = (Application)wordObject;
            bool saveOnExit;
            if (v_WordSaveOnExit == "Yes")
                saveOnExit = true;
            else
                saveOnExit = false;

            //check if document exists and save
            if (wordInstance.Documents.Count >= 1)
                wordInstance.ActiveDocument.Close(saveOnExit);

            //close word
            wordInstance.Quit();

            //remove instance
            engine.RemoveAppInstance(vInstance);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_WordSaveOnExit", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Save on Close '{v_WordSaveOnExit}' - Instance Name '{v_InstanceName}']";
        }
    }
}