using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dialog/Message Commands")]
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
            
            var filter = v_Filter.ConvertToUserVariable(engine);
            if (!checkFileterProperty(filter))
            {
                throw new Exception("Strange Filter Property. Value: '" + filter + "'");
            }

            var index = this.ConvertToUserVariableAsInteger(nameof(v_FilterIndex), engine);
            if (index < 1)
            {
                throw new Exception("Strange FilterIndex Property: Value: " + index);
            }

            string directory;
            if (String.IsNullOrEmpty(v_InitialDirectory))
            {
                directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                directory = this.ConvertToUserVariableAsFolderPath(nameof(v_InitialDirectory), engine);
            }

            Type tp = null;
            switch (this.GetUISelectionValue(nameof(v_DialogType), engine))
            {
                case "open":
                    //engine.tasktEngineUI.Invoke(new Action(() =>
                    //{
                    //    result = engine.tasktEngineUI.ShowOpenFileDialog(filter, index, directory);
                    //}
                    //));
                    //engine.tasktEngineUI.Invoke(new Action(() =>
                    //{
                    //    using (var dialog = new OpenFileDialog())
                    //    {
                    //        dialog.Filter = filter;
                    //        dialog.FilterIndex = index;
                    //        dialog.InitialDirectory = directory;
                    //        if (dialog.ShowDialog() == DialogResult.OK)
                    //        {
                    //            result = dialog.FileName;
                    //        }
                    //        else
                    //        {
                    //            result = "";
                    //        }
                    //    }
                    //}));
                    tp = typeof(OpenFileDialog);
                    break;
                case "save":
                    //engine.tasktEngineUI.Invoke(new Action(() =>
                    //{
                    //    result = engine.tasktEngineUI.ShowSaveFileDialog(filter, index, directory);
                    //}
                    //));
                    //engine.tasktEngineUI.Invoke(new Action(() =>
                    //{
                    //    using (var dialog = new SaveFileDialog())
                    //    {
                    //        dialog.Filter = filter;
                    //        dialog.FilterIndex = index;
                    //        dialog.InitialDirectory = directory;
                    //        if (dialog.ShowDialog() == DialogResult.OK)
                    //        {
                    //            result = dialog.FileName;
                    //        }
                    //        else
                    //        {
                    //            result = "";
                    //        }
                    //    }
                    //}));
                    tp = typeof(SaveFileDialog);
                    break;
            }

            engine.tasktEngineUI.Invoke(new Action(() =>
            {
                using (var dialog = (FileDialog)Activator.CreateInstance(tp))
                {
                    dialog.Filter = filter;
                    dialog.FilterIndex = index;
                    dialog.InitialDirectory = directory;

                    string result = "";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        result = dialog.FileName;
                    }
                    else
                    {
                        result = "";
                    }

                    //result = result.Replace("\\", "\\\\");

                    result.StoreInUserVariable(engine, v_applyToVariableName);
                    //DBG
                    //Console.WriteLine("####" + result);
                    //Console.WriteLine("####" + v_applyToVariableName.GetRawVariable(engine).VariableValue.ToString());
                }
            }));
        }

        private static bool checkFileterProperty(string filter)
        {
            return (filter.Split('|').Length % 2 == 0);
        }
    }
}