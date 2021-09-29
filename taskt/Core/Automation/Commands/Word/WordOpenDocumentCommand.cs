using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command opens an Word Document.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to open an existing Word Document.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    public class WordOpenDocumentCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Word** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **wordInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Word** command will cause an error.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the workbook file path")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file that should be opened by Excel.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.docx or {vFilePath}")]
        [Attributes.PropertyAttributes.Remarks("If file does not contain extensin, suppliment extentions supported by Word.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        public string v_FilePath { get; set; }
        public WordOpenDocumentCommand()
        {
            this.CommandName = "WordOpenDocumentCommand";
            this.SelectionName = "Open Document";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vFilePath = v_FilePath.ConvertToUserVariable(sender);
            vFilePath = Core.FilePathControls.formatFilePath(vFilePath, engine);
            if (!System.IO.File.Exists(vFilePath) && !Core.FilePathControls.hasExtension(vFilePath))
            {
                string[] exts = new string[] { ".docx", "*.docm", "*doc", "*.odt", "*.rtf" };
                foreach(string ext in exts)
                {
                    if (System.IO.File.Exists(vFilePath + ext))
                    {
                        vFilePath += ext;
                        break;
                    }
                }
            }

            var wordObject = engine.GetAppInstance(vInstance);
            Microsoft.Office.Interop.Word.Application wordInstance = (Microsoft.Office.Interop.Word.Application)wordObject;
            wordInstance.Documents.Open(vFilePath);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
            UI.CustomControls.CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word);
            RenderedControls.AddRange(instanceCtrls);
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultWordInstanceName;
            }

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Open from '" + v_FilePath + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}