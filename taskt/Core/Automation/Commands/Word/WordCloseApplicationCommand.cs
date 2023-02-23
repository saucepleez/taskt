using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close Word.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an open instance of Word.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordCloseApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Word** command")]
        //[SampleUsage("**myInstance** or **wordInstance**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Word** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Word)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Document should be Saved")]
        [PropertyUISelectionOption("TRUE")]
        [PropertyUISelectionOption("FALSE")]
        [PropertyDisplayText(false, "")]
        public string v_WordSaveOnExit { get; set; }

        public WordCloseApplicationCommand()
        {
            this.CommandName = "WordCloseApplicationCommand";
            this.SelectionName = "Close Word Application";
            this.CommandEnabled = true;
            this.CustomRendering = true;
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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.Word);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_WordSaveOnExit", this, editor));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultWordInstanceName;
        //    }

        //    return RenderedControls;

        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Save On Close: " + v_WordSaveOnExit + ", Instance Name: '" + v_InstanceName + "']";
        //}
    }
}