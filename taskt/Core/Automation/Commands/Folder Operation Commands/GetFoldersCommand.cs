using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using System.Linq;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation Commands")]
    [Attributes.ClassAttributes.Description("This command returns a list of folder directories from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a list of folder directories from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetFoldersCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source folder")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the folder.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfolder or [vTextFolderPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SourceFolderPath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_UserVariableName { get; set; }

        public GetFoldersCommand()
        {
            this.CommandName = "GetFoldersCommand";
            this.SelectionName = "Get Folders";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            //apply variable logic
            var sourceFolder = v_SourceFolderPath.ConvertToUserVariable(sender);

            //delete folder
            //System.IO.Directory.Delete(sourceFolder, true);
            var directoriesList = System.IO.Directory.GetDirectories(sourceFolder).ToList();

            Script.ScriptVariable newDirectoriesList = new Script.ScriptVariable
            {
                VariableName = v_UserVariableName,
                VariableValue = directoriesList
            };
            //Overwrites variable if it already exists
            if (engine.VariableList.Exists(x => x.VariableName == newDirectoriesList.VariableName))
            {
                Script.ScriptVariable temp = engine.VariableList.Where(x => x.VariableName == newDirectoriesList.VariableName).FirstOrDefault();
                engine.VariableList.Remove(temp);
            }
            engine.VariableList.Add(newDirectoriesList);

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolderPath", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_UserVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_UserVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_UserVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [From: '" + v_SourceFolderPath + "', Store In: '"+ v_UserVariableName +"']";
        }
    }
}