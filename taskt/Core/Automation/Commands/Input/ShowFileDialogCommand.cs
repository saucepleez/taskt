using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.CommandSettings("Show File Dialog")]
    [Attributes.ClassAttributes.Description("Show OpenFileDialog or SaveFileDialog")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select file to save or open.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ShowFileDialogCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Dialog Type")]
        [PropertyUISelectionOption("Open")]
        [PropertyUISelectionOption("Save")]
        [PropertyIsOptional(true, "Open")]
        [PropertyFirstValue("Open")]
        [PropertyDisplayText(true, "Type")]
        public string v_DialogType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Value of the Filter Property")]
        [InputSpecification("Filter", true)]
        [PropertyIsOptional(true, "All Files (*.*)|*.*")]
        [PropertyFirstValue("All Files (*.*)|*.*")]
        [PropertyDisplayText(true, "Filter")]
        public string v_Filter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Value of the FilterIndex Property")]
        [InputSpecification("FilterIndex", true)]
        //[SampleUsage("**1** or **2** or **{{{vIndex}}}**")]
        [PropertyDetailSampleUsage("**1**", "Specify the First Filter")]
        [PropertyDetailSampleUsage("**2**", PropertyDetailSampleUsage.ValueType.Value, "FilterIndex")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "FilterIndex")]
        [PropertyIsOptional(true, "1")]
        [PropertyValidationRule("FilterIndex", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_FilterIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        [PropertyDescription("Value of the InitialDirectory Property")]
        [InputSpecification("InitialDirectory", true)]
        //[SampleUsage("**C:\\Users\\myUser\\Documents** or **{{{vFolderPath}}}**")]
        [PropertyDetailSampleUsage("**C:\\Users\\myUser\\Documents**", PropertyDetailSampleUsage.ValueType.Value, "InitialDirectory")]
        [PropertyDetailSampleUsage("**{{{vFolderPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "InitialDirectory")]
        [PropertyIsOptional(true, "Documents")]
        [PropertyFirstValue("")]
        [PropertyValidationRule("InitialDirectory", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_InitialDirectory { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the variable to receive file name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public ShowFileDialogCommand()
        {
            //this.CommandName = "FileDialogCommand";
            //this.SelectionName = "File Dialog";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //this.v_DialogType = "Open";
            //this.v_Filter = "Text file (*.txt)|*.txt";
            //this.v_FilterIndex = "";
            //this.v_InitialDirectory = "";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            
            //var dialogType = v_DialogType.ConvertToUserVariable(sender);
            
            var filter = v_Filter.ConvertToUserVariable(engine);
            if (!checkFileterProperty(filter))
            {
                throw new Exception("Strange Filter Property. Value: '" + filter + "'");
            }

            //var index = v_FilterIndex.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(index))
            //{
            //    index = "1";
            //}
            var index = this.ConvertToUserVariableAsInteger(nameof(v_FilterIndex), engine);
            if (index < 1)
            {
                throw new Exception("Strange FilterIndex Property: Value: " + index);
            }

            //var directory = v_InitialDirectory.ConvertToUserVariable(sender);
            string directory;
            if (String.IsNullOrEmpty(v_InitialDirectory))
            {
                directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                directory = this.ConvertToUserVariableAsFolderPath(nameof(v_InitialDirectory), engine);
            }
            //int vIndex = 1;
            //vIndex = int.Parse(index);

            object result = null;
            switch (this.GetUISelectionValue(nameof(v_DialogType), engine))
            {
                case "open":
                    engine.tasktEngineUI.Invoke(new Action(() =>
                    {
                        result = engine.tasktEngineUI.ShowOpenFileDialog(filter, index, directory);
                    }
                    ));
                    break;
                case "save":
                    engine.tasktEngineUI.Invoke(new Action(() =>
                    {
                        result = engine.tasktEngineUI.ShowSaveFileDialog(filter, index, directory);
                    }
                    ));
                    break;
            }
            if (result != null)
            {
                result.ToString().StoreInUserVariable(engine, v_applyToVariableName);
            }
        }

        private static bool checkFileterProperty(string filter)
        {
            //var count = filter.Split('|').Length;
            //if (count == 0)
            //{
            //    return false;
            //}
            //else
            //{
            //    return (count % 2 == 1);
            //}
            return (filter.Split('|').Length % 2 == 1);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_DialogType", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Filter", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilterIndex", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InitialDirectory", this, editor));

        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Show " + v_DialogType + "FileDialog, Filter '" + v_Filter + "', Apply Result to Variable '" + v_applyToVariableName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_DialogType))
        //    {
        //        this.validationResult += "Dialog type is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_Filter))
        //    {
        //        this.validationResult += "Filter is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_applyToVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}