using System;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Word;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command appends an image to a word document.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to append an image to a specific document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordAppendImageCommand : ScriptCommand
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
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Path to the Image")]
        [InputSpecification("Path to the Image", true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[SampleUsage("C:\\temp\\myimage.png or {vTextFilePath}")]
        [PropertyDetailSampleUsage("**C:\\temp\\myimage.png**", PropertyDetailSampleUsage.ValueType.Value, "Path to the Image")]
        [PropertyDetailSampleUsage("**{{{vImagePath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Path to the Image")]
        [PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Path")]
        public string v_ImagePath { get; set; }

        public WordAppendImageCommand()
        {
            this.CommandName = "WordAppendImageCommand";
            this.SelectionName = "Append Image";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //var wordObject = engine.GetAppInstance(vInstance);
            //Microsoft.Office.Interop.Word.Application wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;
            //Document wordDocument = wordInstance.ActiveDocument;
            (var _, var wordDocument) = v_InstanceName.GetWordInstanceAndDocument(engine);

            //Appends image after text/images
            var vImagePath = v_ImagePath.ConvertToUserVariable(engine);
            object collapseEnd = WdCollapseDirection.wdCollapseEnd;
            Range imageRange = wordDocument.Content;
            imageRange.Collapse(ref collapseEnd);
            imageRange.InlineShapes.AddPicture(vImagePath, Type.Missing, Type.Missing, imageRange);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.Word);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ImagePath", this, editor));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultWordInstanceName;
        //    }

        //    return RenderedControls;
        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " ['" + v_ImagePath + "' To Instance Name: '" + v_InstanceName + "']";
        //}
    }
}