using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Show File/Folder Dialog commands
    /// </summary>
    public abstract class AShowFileFolderDialogCommands : ScriptCommand, IWaitDialogResultProperties, ICanHandleFolderPath
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        [PropertyDescription("Initial Folder Path")]
        [InputSpecification("Initial Folder Path", true)]
        [PropertyDetailSampleUsage("**C:\\Users\\myUser\\Documents**", PropertyDetailSampleUsage.ValueType.Value, "Initial Folder Path")]
        [PropertyDetailSampleUsage("**{{{vFolderPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Initial Folder Path")]
        [PropertyIsOptional(true, "Documents")]
        [PropertyFirstValue("")]
        [PropertyValidationRule("Initial Folder Path", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Initial Folder Path")]
        [PropertyParameterOrder(10000)]
        public string v_InitialDirectory { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(11000)]
        public string v_Result { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_ComboBoxHasErrorIgnore))]
        //[PropertyDescription("When Dialog Result Is Cancel")]
        //[PropertyUISelectionOption("Set Empty")]
        //[PropertyUISelectionOption("Show Dialog Again")]
        //[PropertyDetailSampleUsage("**Ignore**", "Nothing to do. The Result Variable is not Changed.")]
        //[PropertyDetailSampleUsage("**Set Empty**", "Result Variable value is Empty")]
        //[PropertyDetailSampleUsage("**Show Dialog Again", "Show Dialog Again")]
        //[PropertyIsOptional(true, "Show Dialog Again")]
        //[PropertyValidationRule("When Dialog Result Is Cancel", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "When Dialog Result Is Cancel")]
        [PropertyVirtualProperty(nameof(ShowDialogControls), nameof(ShowDialogControls.v_WhenCancel))]
        [PropertyParameterOrder(12000)]
        public string v_WhenCancel { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name To Store Dislog Result")]
        //[Remarks("Value is **OK** or **Cancel**")]
        //[PropertyValidationRule("DialogResult", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "DialogResult")]
        [PropertyVirtualProperty(nameof(ShowDialogControls), nameof(ShowDialogControls.v_DialogResult))]
        [PropertyParameterOrder(13000)]
        public string v_DialogResult { get; set; }

        /// <summary>
        /// get InitialDirectory
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        protected string GetInitialDirectory(Engine.AutomationEngineInstance engine)
        {
            if (string.IsNullOrEmpty(v_InitialDirectory))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                return this.ExpandValueOrUserVariableAsFolderPath(nameof(v_InitialDirectory), engine);
            }
        }

        /// <summary>
        /// show dialog process. When you click Cancel, the process changes according to v_WhenCancel
        /// </summary>
        /// <param name="dlg"></param>
        /// <param name="GetPathFunc"></param>
        /// <param name="dialogTypeName"></param>
        /// <param name="engine"></param>
        /// <exception cref="Exception"></exception>
        protected void ShowDialogProcess(dynamic dialog, Func<string> GetPathFunc, string dialogTypeName, Engine.AutomationEngineInstance engine)
        {
            int okValue;
            if (dialog is CommonDialog)
            {
                okValue = (int)DialogResult.OK;
            }
            else if (dialog is CommonOpenFileDialog)
            {
                okValue = (int)CommonFileDialogResult.Ok;
            }
            else
            {
                throw new Exception($"Strange dialog object. Type; {dialog.GetType().Name}");
            }

            //void StoreDialogResultValue(string v)
            //{
            //    if (!string.IsNullOrEmpty(v_DialogResult))
            //    {
            //        v.StoreInUserVariable(engine, v_DialogResult);
            //    }
            //}

            //var whenCancel = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_WhenCancel), engine);
            var whenCancel = this.ExpandValueOrUserVariableAsWhenCancel(engine);
            switch (whenCancel)
            {
                case "show dialog again":
                    bool isAgain = true;
                    while (isAgain)
                    {
                        if ((int)dialog.ShowDialog() == okValue)
                        {
                            GetPathFunc().StoreInUserVariable(engine, v_Result);
                            isAgain = false;
                        }
                    }
                    //StoreDialogResultValue("OK");
                    this.StoreDialogResultInUserVariable("OK", engine);
                    break;
                default:
                    if ((int)dialog.ShowDialog() == okValue)
                    {
                        GetPathFunc().StoreInUserVariable(engine, v_Result);
                        //StoreDialogResultValue("OK");
                        this.StoreDialogResultInUserVariable("OK", engine);
                    }
                    else
                    {
                        //StoreDialogResultValue("Cancel");
                        this.StoreDialogResultInUserVariable("Cancel", engine);
                        switch (whenCancel)
                        {
                            case "error":
                                throw new Exception($"Error. {dialogTypeName} is Clicked Cancel button.");

                            case "set empty":
                                "".StoreInUserVariable(engine, v_Result);
                                break;
                            case "ignore":
                                // nothing to do
                                break;
                        }
                    }
                    break;
            }
        }
    }
}
