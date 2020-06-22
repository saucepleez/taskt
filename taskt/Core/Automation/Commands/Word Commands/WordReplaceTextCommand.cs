using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using Application = Microsoft.Office.Interop.Word.Application;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Word Commands")]
    [Description("This command replaces specific text in a Word Document.")]

    public class WordReplaceTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance || {vWordInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Find")]
        [InputSpecification("Enter the text to find.")]
        [SampleUsage("old text || {vFindText}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_FindText { get; set; }

        [XmlAttribute]
        [PropertyDescription("Replace")]
        [InputSpecification("Enter the text to replace with.")]
        [SampleUsage("new text || {vReplaceText}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ReplaceWithText { get; set; }

        public WordReplaceTextCommand()
        {
            CommandName = "WordReplaceTextCommand";
            SelectionName = "Replace Text";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
        }
        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vFindText = v_FindText.ConvertToUserVariable(engine);
            var vReplaceWithText = v_ReplaceWithText.ConvertToUserVariable(engine);

            //get word app object
            var wordObject = engine.GetAppInstance(vInstance);

            //convert object
            Application wordInstance = (Application)wordObject;
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

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FindText", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ReplaceWithText", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Replace '{v_FindText}' With '{v_ReplaceWithText}' - Instance Name '{v_InstanceName}']";
        }
    }
}