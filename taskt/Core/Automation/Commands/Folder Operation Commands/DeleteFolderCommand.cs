using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.Description("This command deletes a folder from a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to delete a folder from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class DeleteFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source folder")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the folder.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfolder or [vTextFolderPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SourceFolderPath { get; set; }

        public DeleteFolderCommand()
        {
            this.CommandName = "DeleteFolderCommand";
            this.SelectionName = "Delete Folder";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {

            //apply variable logic
            var sourceFolder = v_SourceFolderPath.ConvertToUserVariable(sender);

            //delete folder
            System.IO.Directory.Delete(sourceFolder, true);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolderPath", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [delete " + v_SourceFolderPath + "']";
        }
    }
}