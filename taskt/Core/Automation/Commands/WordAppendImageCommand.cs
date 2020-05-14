using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Word;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command appends an image to a word document.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to append an image to a specific document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    public class WordAppendImageCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Word** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **wordInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Word** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the image")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the image.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myimage.png or [vTextFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vImagePath = v_ImagePath.ConvertToUserVariable(engine);
            var wordObject = engine.GetAppInstance(vInstance);

            Microsoft.Office.Interop.Word.Application wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;
            Document wordDocument = wordInstance.ActiveDocument;

            //Appends image after text/images
            object collapseEnd = WdCollapseDirection.wdCollapseEnd;
            Range imageRange = wordDocument.Content;
            imageRange.Collapse(ref collapseEnd);
            imageRange.InlineShapes.AddPicture(vImagePath, Type.Missing, Type.Missing, imageRange);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ImagePath", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " ['" + v_ImagePath + "' To Instance Name: '" + v_InstanceName + "']";
        }
    }
}