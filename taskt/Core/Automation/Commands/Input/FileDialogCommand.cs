using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.User32;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Show OpenFileDialog or SaveFileDialog")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select file to save or open.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class FileDialogCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify the type of dialog")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Open")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Save")]
        [Attributes.PropertyAttributes.InputSpecification("OpenFileDialog or SaveFileDialog")]
        [Attributes.PropertyAttributes.SampleUsage("**Open** or **Save**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_DialogType { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify the value of the Filter property")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Text File (\\*.txt)\\|\\*.txt** or **{{{vFilter}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_Filter { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify the value of the FilterIndex property (Default is 1)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**1** or **2** or **{{{vIndex}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_FilterIndex { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify the value of the InitialDirectory property (Default is documents)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\Users\\myUser\\Documents** or **{{{vFolderPath}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_InitialDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive file name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        public string v_applyToVariableName { get; set; }

        public FileDialogCommand()
        {
            this.CommandName = "FileDialogCommand";
            this.SelectionName = "File Dialog";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_DialogType = "Open";
            this.v_Filter = "Text file (*.txt)|*.txt";
            this.v_FilterIndex = "";
            this.v_InitialDirectory = "";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var dialogType = v_DialogType.ConvertToUserVariable(sender);
            var filter = v_Filter.ConvertToUserVariable(sender);
            var index = v_FilterIndex.ConvertToUserVariable(sender);
            var directory = v_InitialDirectory.ConvertToUserVariable(sender);

            if (String.IsNullOrEmpty(index))
            {
                index = "1";
            }
            if (String.IsNullOrEmpty(directory))
            {
                directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            int vIndex = 1;
            vIndex = int.Parse(index);

            object result = "";
            switch (dialogType)
            {
                case "Open":
                    engine.tasktEngineUI.Invoke(new Action(() =>
                    {
                        result = engine.tasktEngineUI.ShowOpenFileDialog(filter, vIndex, directory);
                    }
                    ));
                    break;
                case "Save":
                    engine.tasktEngineUI.Invoke(new Action(() =>
                    {
                        result = engine.tasktEngineUI.ShowSaveFileDialog(filter, vIndex, directory);
                    }
                    ));
                    break;
                default:
                    throw new Exception("Dialog Type " + dialogType + " is not support.");
                    break;
            }
            if (result != null)
            {
                result.ToString().StoreInUserVariable(sender, v_applyToVariableName);
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_DialogType", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Filter", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilterIndex", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InitialDirectory", this, editor));

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Show " + v_DialogType + "FileDialog, Filter '" + v_Filter + "', Apply Result to Variable '" + v_applyToVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_DialogType))
            {
                this.validationResult += "Dialog type is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_Filter))
            {
                this.validationResult += "Filter is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_applyToVariableName))
            {
                this.validationResult += "Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}