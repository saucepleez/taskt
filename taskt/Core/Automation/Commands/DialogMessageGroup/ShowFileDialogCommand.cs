using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dialog/Message")]
    [Attributes.ClassAttributes.CommandSettings("Show File Dialog")]
    [Attributes.ClassAttributes.Description("Show OpenFileDialog or SaveFileDialog")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select file to save or open.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ShowFileDialogCommand : AShowFileFolderDialogCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Dialog Type")]
        [PropertyUISelectionOption("Open")]
        [PropertyUISelectionOption("Save")]
        [PropertyIsOptional(true, "Open")]
        [PropertyFirstValue("Open")]
        [PropertyDisplayText(true, "Type")]
        [PropertyParameterOrder(5000)]
        public string v_DialogType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Value of the Filter Property")]
        [InputSpecification("Filter Text", true)]
        [PropertyIsOptional(true, "All Files (*.*)|*.*")]
        [PropertyFirstValue("All Files (*.*)|*.*")]
        [PropertyDisplayText(true, "Filter")]
        [Remarks("https://learn.microsoft.com/en-us/dotnet/api/microsoft.win32.filedialog.filter?view=windowsdesktop-8.0")]
        [PropertyParameterOrder(6000)]
        public string v_Filter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Value of the FilterIndex Property")]
        [InputSpecification("Filter Index Number", true)]
        [PropertyDetailSampleUsage("**1**", "Specify the First Filter")]
        [PropertyDetailSampleUsage("**2**", PropertyDetailSampleUsage.ValueType.Value, "FilterIndex")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "FilterIndex")]
        [PropertyIsOptional(true, "1")]
        [PropertyValidationRule("FilterIndex", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(7000)]
        public string v_FilterIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //[PropertyDescription("Value of the InitialDirectory Property")]
        //[InputSpecification("InitialDirectory", true)]
        //[PropertyDetailSampleUsage("**C:\\Users\\myUser\\Documents**", PropertyDetailSampleUsage.ValueType.Value, "InitialDirectory")]
        //[PropertyDetailSampleUsage("**{{{vFolderPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "InitialDirectory")]
        //[PropertyIsOptional(true, "Documents")]
        //[PropertyFirstValue("")]
        //[PropertyValidationRule("InitialDirectory", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //public string v_InitialDirectory { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //public string v_Result { get; set; }

        public ShowFileDialogCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var filter = v_Filter.ExpandValueOrUserVariable(engine);
            if (!CheckFileterProperty(filter))
            {
                throw new Exception($"Strange Filter Property. Value: '{filter}'");
            }

            var index = this.ExpandValueOrUserVariableAsInteger(nameof(v_FilterIndex), engine);
            if (index < 1)
            {
                throw new Exception($"Strange FilterIndex Property: Value: {index}");
            }

            //string directory;
            //if (string.IsNullOrEmpty(v_InitialDirectory))
            //{
            //    directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //}
            //else
            //{
            //    directory = this.ExpandValueOrUserVariableAsFolderPath(nameof(v_InitialDirectory), engine);
            //}

            Type tp = null;
            switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_DialogType), engine))
            {
                case "open":
                    tp = typeof(OpenFileDialog);
                    break;
                case "save":
                    tp = typeof(SaveFileDialog);
                    break;
            }

            engine.tasktEngineUI.Invoke(new Action(() =>
            {
                using (var dialog = (FileDialog)Activator.CreateInstance(tp))
                {
                    dialog.Filter = filter;
                    dialog.FilterIndex = index;
                    dialog.InitialDirectory = this.GetInitialDirectory(engine);

                    this.ShowDialogProcess(dialog,
                        new Func<string>(() => dialog.FileName),
                        tp.Name, engine);
                }
            }));
        }

        private static bool CheckFileterProperty(string filter)
        {
            return (filter.Split('|').Length % 2 == 0);
        }
    }
}
