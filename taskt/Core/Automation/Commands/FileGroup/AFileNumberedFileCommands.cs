using System;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    public abstract class AFileNumberedFileCommands : AFolderExistsFolderPathCommands, ICanHandleFileName
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        [PropertyIsOptional(true, "Script File Folder")]
        [PropertyValidationRule("Folder", PropertyValidationRule.ValidationRuleFlags.None)]
        public override string v_TargetFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("File Name Before Counter")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**log_**", PropertyDetailSampleUsage.ValueType.Value, "File Name Before Counter")]
        [PropertyDetailSampleUsage("**{{{vPreFix}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name Before Counter")]
        [PropertyValidationRule("Before", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Before")]
        [PropertyParameterOrder(6000)]
        public virtual string v_BeforeFileCounter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("File Counter Digits")]
        [Remarks("")]
        [PropertyIsOptional(true, "3")]
        [PropertyFirstValue("3")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**3**", PropertyDetailSampleUsage.ValueType.Value, "File Counter")]
        [PropertyDetailSampleUsage("**{{{vDigits}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Counter")]
        [PropertyValidationRule("Digits", PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        [PropertyDisplayText(true, "Digits")]
        [PropertyParameterOrder(6100)]
        public virtual string v_Digits { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("File Name After Counter")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**_log**", PropertyDetailSampleUsage.ValueType.Value, "File Name After Counter")]
        [PropertyDetailSampleUsage("**{{{vPostFix}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name After Counter")]
        [PropertyValidationRule("After", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "After")]
        [PropertyParameterOrder(6200)]
        public virtual string v_AfterFileCounter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("File Extension")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**txt**", "Specify Text File")]
        [PropertyDetailSampleUsage("**{{{vExtension}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name After Counter")]
        [PropertyValidationRule("Extension", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Extension")]
        [PropertyParameterOrder(6300)]
        public virtual string v_Extension { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6400)]
        public virtual string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Counter Start Value")]
        [Remarks("")]
        [PropertyIsOptional(true, "1")]
        [PropertyFirstValue("1")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Start Value")]
        [PropertyDetailSampleUsage("**{{{vStart}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Start Value")]
        [PropertyValidationRule("Start", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Start Value")]
        [PropertyParameterOrder(6500)]
        public virtual string v_StartValue { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        //[PropertyFirstValue("10")]
        //public override string v_WaitTimeForFolder { get; set; }

        public AFileNumberedFileCommands()
        {
        }

        /// <summary>
        /// search non existent file
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected void SearchNonExistentFileAction(PropertyFilePathSetting setting, Engine.AutomationEngineInstance engine)
        {
            this.FolderAction(engine,
                new Func<string, string>(targetFolder =>
                {
                    string preCounter = (string.IsNullOrEmpty(v_BeforeFileCounter)) ? "" : this.ExpandValueOrUserVariable(nameof(v_BeforeFileCounter), "Before File Counter", engine);
                    string postCoutner = (string.IsNullOrEmpty(v_AfterFileCounter)) ? "" : this.ExpandValueOrUserVariable(nameof(v_AfterFileCounter), "After File Counter", engine);
                    string extension = (string.IsNullOrEmpty(v_Extension)) ? "" : this.ExpandValueOrUserVariable(nameof(v_Extension), "Extension", engine);
                    if (!string.IsNullOrEmpty(extension) && !extension.StartsWith("."))
                    {
                        extension = $".{extension}";
                    }

                    if (!EM_CanHandleFileNameExtensionMethods.IsValidFileName(preCounter))
                    {
                        throw new Exception($"Before File Counter has Invalid Charactor. Value: '{v_BeforeFileCounter}', Expand Value: '{preCounter}'");
                    }
                    if (!EM_CanHandleFileNameExtensionMethods.IsValidFileName(postCoutner))
                    {
                        throw new Exception($"After File Counter has Invalid Charactor. Value: '{v_AfterFileCounter}', Expand Value: '{postCoutner}'");
                    }
                    if (!EM_CanHandleFileNameExtensionMethods.IsValidFileName(extension))
                    {
                        throw new Exception($"Extension has Invalid Charactor. Value: '{v_Extension}', Expand Value: '{extension}'");
                    }

                    int digits = this.ExpandValueOrUserVariableAsInteger(nameof(v_Digits), engine);
                    string counterFormat = "{0:";
                    for (int i = 0; i < digits; i++)
                    {
                        counterFormat += "0";
                    }
                    counterFormat += "}";

                    if (string.IsNullOrEmpty(v_StartValue))
                    {
                        v_StartValue = "1";
                    }

                    int counter = this.ExpandValueOrUserVariableAsInteger(nameof(v_StartValue), engine);
                    int firstValue = counter;
                    string path;
                    while (true)
                    {
                        path = Path.Combine(targetFolder, $"{preCounter}{string.Format(counterFormat, counter)}{postCoutner}{extension}");

                        if (!EM_CanHandleFilePathExtentionMethods.IsValidPathString(path))
                        {
                            throw new Exception($"Contains Invalid File Path Charactor. Path: '{path}'");
                        }

                        if (File.Exists(path))
                        {
                            counter++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    string res;
                    switch (setting.supportFileCounter)
                    {
                        case PropertyFilePathSetting.FileCounterBehavior.LastExists:
                            if (counter == firstValue)
                            {
                                // not exist
                                res = "";
                            }
                            else
                            {
                                counter--;
                                res = Path.Combine(targetFolder, $"{preCounter}{string.Format(counterFormat, counter)}{postCoutner}{extension}");
                            }
                            break;

                        case PropertyFilePathSetting.FileCounterBehavior.FirstNotExists:
                        default:
                            res = path;
                            break;
                    }

                    res.StoreInUserVariable(engine, v_Result);

                    return targetFolder;
                })
            );
        }
    }
}