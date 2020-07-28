using Microsoft.Office.Interop.Word;
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
using Group = taskt.Core.Attributes.ClassAttributes.Group;

namespace taskt.Commands
{
    [Serializable]
    [Group("Word Commands")]
    [Description("This command appends an image to a Word Document.")]

    public class WordAppendImageCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance || {vWordInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Image File Path")]    
        [InputSpecification("Enter the file path of the image to append to the Document.")]
        [SampleUsage(@"C:\temp\myImage.png || {vImageFilePath} || {ProjectPath}\myImage.png")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_ImagePath { get; set; }

        public WordAppendImageCommand()
        {
            CommandName = "WordAppendImageCommand";
            SelectionName = "Append Image";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vImagePath = v_ImagePath.ConvertToUserVariable(engine);
            var wordObject = engine.GetAppInstance(vInstance);

            Application wordInstance = (Application)wordObject;
            Document wordDocument = wordInstance.ActiveDocument;

            //Appends image after text/images
            object collapseEnd = WdCollapseDirection.wdCollapseEnd;
            Range imageRange = wordDocument.Content;
            imageRange.Collapse(ref collapseEnd);
            imageRange.InlineShapes.AddPicture(vImagePath, Type.Missing, Type.Missing, imageRange);

            Paragraph paragraph = wordDocument.Content.Paragraphs.Add();
            paragraph.Format.SpaceAfter = 10f;
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ImagePath", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Append '{v_ImagePath}' - Instance Name '{v_InstanceName}']";
        }
    }
}