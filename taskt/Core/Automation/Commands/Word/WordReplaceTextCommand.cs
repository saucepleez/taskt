using System;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Word;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to replace text in a Word document.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to replace text in a document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordReplaceTextCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Word** command")]
        //[SampleUsage("**myInstance** or **wordInstance**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Word** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Word)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        //[PropertyDescription("Please define the text to find")]
        //[InputSpecification("Enter the text you wish to find.")]
        //[SampleUsage("**findText**")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDescription("Text to Find")]
        [InputSpecification("Text to Find", true)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Text to Find")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text to Find")]
        [PropertyValidationRule("Text to Find", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Text to Find")]
        public string v_FindText { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        //[PropertyDescription("Please define the text to replace with")]
        //[InputSpecification("Enter the text you wish to replace the found text.")]
        //[SampleUsage("**replaceWithText**")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDescription("Text to Replace with")]
        [InputSpecification("Text to Replace with", true)]
        [PropertyDetailSampleUsage("**Hi!**", PropertyDetailSampleUsage.ValueType.Value, "Text to Replace with")]
        [PropertyDetailSampleUsage("**{{{vNewText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text to Replace with")]
        [PropertyIsOptional(true)]
        [PropertyDisplayText(true, "Text to Replace with")]
        public string v_ReplaceWithText { get; set; }

        public WordReplaceTextCommand()
        {
            this.CommandName = "WordReplaceTextCommand";
            this.SelectionName = "Replace Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Engine.AutomationEngineInstance)sender;

            ////convert variables
            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            ////get excel app object
            //var wordObject = engine.GetAppInstance(vInstance);
            ////convert object
            //Microsoft.Office.Interop.Word.Application wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;
            //Document wordDocument = wordInstance.ActiveDocument;
            (var _, var wordDocument) = v_InstanceName.GetWordInstanceAndDocument(engine);

            var vFindText = v_FindText.ConvertToUserVariable(engine);
            var vReplaceWithText = v_ReplaceWithText.ConvertToUserVariable(engine);

            Range range = wordDocument.Content;

            //replace text
            Find findObject = range.Find;
            findObject.ClearFormatting();
            findObject.Text = vFindText;
            findObject.Replacement.ClearFormatting();
            findObject.Replacement.Text = vReplaceWithText;

            object replaceAll = WdReplace.wdReplaceAll;
            findObject.Execute(Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                               Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                               ref replaceAll, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.Word);
        //    RenderedControls.AddRange(instanceCtrls);
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FindText", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ReplaceWithText", this, editor));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultWordInstanceName;
        //    }

        //    return RenderedControls;
        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Find: '" + v_FindText + "', Replace With: '" + v_ReplaceWithText + "', Instance Name: '" + v_InstanceName + "']";
        //}
    }
}