using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close Word.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an open instance of Word.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    public class WordCloseApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Word** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **wordInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Word** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate if the Document should be saved")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a TRUE or FALSE value")]
        [Attributes.PropertyAttributes.SampleUsage("'TRUE' or 'FALSE'")]
        [Attributes.PropertyAttributes.Remarks("")]
        public bool v_WordSaveOnExit { get; set; }
        public WordCloseApplicationCommand()
        {
            this.CommandName = "WordCloseApplicationCommand";
            this.SelectionName = "Close Word Application";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var wordObject = engine.GetAppInstance(vInstance);


            Microsoft.Office.Interop.Word.Application wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;


            //check if document exists and save
            if (wordInstance.Documents.Count >= 1)
            {
                wordInstance.ActiveDocument.Close(v_WordSaveOnExit);
            }

            //close word
            wordInstance.Quit();

            //remove instance
            engine.RemoveAppInstance(vInstance);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_WordSaveOnExit", this, editor));

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Save On Close: " + v_WordSaveOnExit + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
}