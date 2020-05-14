using Microsoft.Office.Interop.Word;
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
    [Attributes.ClassAttributes.Description("This command allows you to replace text in a Word document.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to replace text in a document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    public class WordReplaceTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Word** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **wordInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Word** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the text to find")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text you wish to find.")]
        [Attributes.PropertyAttributes.SampleUsage("**findText**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_FindText { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the text to replace with")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text you wish to replace the found text.")]
        [Attributes.PropertyAttributes.SampleUsage("**replaceWithText**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
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
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //convert variables
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vFindText = v_FindText.ConvertToUserVariable(engine);
            var vReplaceWithText = v_ReplaceWithText.ConvertToUserVariable(engine);

            //get excel app object
            var wordObject = engine.GetAppInstance(vInstance);

            //convert object
            Microsoft.Office.Interop.Word.Application wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;
            Document wordDocument = wordInstance.ActiveDocument;
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
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FindText", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ReplaceWithText", this, editor));

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Find: '" + v_FindText + "', Replace With: '" + v_ReplaceWithText + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}