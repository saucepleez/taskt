using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dialog/Message Commands")]
    [Attributes.ClassAttributes.CommandSettings("Show Folder Dialog")]
    [Attributes.ClassAttributes.Description("Show FolderBrowserDialog")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select folder.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ShowFolderDialogCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public ShowFolderDialogCommand()
        {
            //this.CommandName = "FolderDialogCommand";
            //this.SelectionName = "Folder Dialog";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //object result = null;
            //engine.tasktEngineUI.Invoke(new Action(() =>
            //    {
            //        result = engine.tasktEngineUI.ShowFolderDialog();
            //    }
            //));
            //if (result != null)
            //{
            //    result.ToString().StoreInUserVariable(sender, v_applyToVariableName);
            //}
            engine.tasktEngineUI.Invoke(new Action(() =>
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    string result;
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        result = dialog.SelectedPath;
                    }
                    else
                    {
                        result = "";
                    }
                    result.StoreInUserVariable(engine, v_applyToVariableName);
                }
            }));
        }
    }
}