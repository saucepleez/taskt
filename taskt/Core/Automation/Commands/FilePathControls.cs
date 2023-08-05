using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    ///  for file path methods
    /// </summary>
    internal static class FilePathControls
    {
        #region VirtualProperty
        /// <summary>
        /// for file path (recommended use PropertyFilePathSetting attribute)
        /// </summary>
        [PropertyDescription("File Path")]
        [InputSpecification("File Path", true)]
        [PropertyDetailSampleUsage("**C:\\temp\\myfile.txt**", PropertyDetailSampleUsage.ValueType.Value, "File Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Path")]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File")]
        public static string v_FilePath { get; }

        /// <summary>
        /// for file path (recommended use PropertyFilePathSetting attribute)
        /// </summary>
        [PropertyDescription("File Path")]
        [InputSpecification("File Path", true)]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("File Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "File")]
        public static string v_NoSample_FilePath { get; }

        /// <summary>
        /// file does not exists behavior
        /// </summary>
        [PropertyDescription("When the File does Not Exists")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyDetailSampleUsage("**Ignore**", "Continue Script File")]
        [PropertyDetailSampleUsage("**Error**", "Rise A Error")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Error")]
        [PropertyDisplayText(false, "")]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        public static string v_WhenFileDoesNotExists { get; }

        /// <summary>
        /// for wait time
        /// </summary>
        [PropertyDescription("Wait Time for the File to Exist (sec)")]
        [InputSpecification("Wait Time", true)]
        [PropertyDetailSampleUsage("**10**", PropertyDetailSampleUsage.ValueType.Value, "Wait Time")]
        [PropertyDetailSampleUsage("**{{{vWaitTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Wait Time")]
        [Remarks("Specify how long to Wait before an Error will occur because the File is not Found.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyIsOptional(true, "10")]
        [PropertyFirstValue("10")]
        [PropertyDisplayText(true, "Wait", "s")]
        public static string v_WaitTime { get; }

        /// <summary>
        /// file path result
        /// </summary>
        [PropertyDescription("Variable Name to Store File Path")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vPath**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyIsOptional(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("File Path Result", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public static string v_FilePathResult { get; }
        #endregion

        #region check methods

        /// <summary>
        /// check file path is full path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFullPath(string path)
        {
            //return (path != Path.GetFileName(path));
            return !String.IsNullOrEmpty(Path.GetPathRoot(path));
        }

        /// <summary>
        /// check file path has extension
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool HasExtension(string path)
        {
            return (Path.GetExtension(path).Length > 0);
        }

        /// <summary>
        /// get last FileCounter Variable name and position
        /// </summary>
        /// <param name="path">don't convert variable</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static (string variableName, int index) GetLastFileCounter(string path, Engine.AutomationEngineInstance engine)
        {
            var f0 = VariableNameControls.GetWrappedVariableName("FileCounter.F0", engine);
            var f00 = VariableNameControls.GetWrappedVariableName("FileCounter.F00", engine);
            var f000 = VariableNameControls.GetWrappedVariableName("FileCounter.F000", engine);

            var indices = new Dictionary<string, int>()
            {
                { f0, path.LastIndexOf(f0) },
                { f00, path.LastIndexOf(f00) },
                { f000, path.LastIndexOf(f000) },
            };
            var maxItem = indices.OrderByDescending(c => c.Value).First();

            return (maxItem.Key, maxItem.Value);
        }

        /// <summary>
        /// check file path contains FileCounter variable
        /// </summary>
        /// <param name="path">don't convert variable</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static bool ContainsFileCounter(string path, Engine.AutomationEngineInstance engine)
        {
            path = path ?? "";
            (_, var index) = GetLastFileCounter(path, engine);

            return (index >= 0);
        }

        /// <summary>
        /// check file path is URL
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsURL(string path)
        {
            return (path.StartsWith("http:") || path.StartsWith("https:"));
        }

        #endregion

        #region convert methods

        /// <summary>
        /// Get Before FileCounter Text, FileCounter Variable Name, After FileCounter Text
        /// </summary>
        /// <param name="path"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static (string beforeCounter, string counterVariable, string afterCounter) ParseFileCounter(string path, Engine.AutomationEngineInstance engine)
        {
            var r = GetLastFileCounter(path, engine);
            if (r.index < 0)
            {
                throw new Exception("No FileCounter Variables contains. Path: '" + path + "'");
            }
            else
            {
                return (path.Substring(0, r.index), r.variableName, path.Substring(r.index + r.variableName.Length + 1));
            }
        }

        /// <summary>
        /// convert to FilePath support FileCounter
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="setting"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string ConvertToUserVariableAsFilePath_SupportFileCounter(string parameterValue, PropertyFilePathSetting setting, Engine.AutomationEngineInstance engine)
        {
            // check contains FileCounter
            if (!ContainsFileCounter(parameterValue, engine))
            {
                // don't contains FileCounter
                return ConvertToUserVariableAsFilePath_NoSupportFileCounter(parameterValue, setting, engine);
            }

            (var beforeVariable, var wrappedCounterVariableName, var afterVariable) = ParseFileCounter(parameterValue, engine);

            beforeVariable = beforeVariable.ConvertToUserVariable(engine);
            afterVariable = afterVariable.ConvertToUserVariable(engine);
            var counterVariableName = VariableNameControls.GetVariableName(wrappedCounterVariableName, engine);

            // URL Check
            var checkPath = beforeVariable + "0" + afterVariable;
            if (IsURL(checkPath))
            {
                // path is URL, FileCounter, supportExtension does not work
                if (!setting.allowURL)
                {
                    throw new Exception("Path is URL. Value: '" + beforeVariable + wrappedCounterVariableName + afterVariable + "'");
                }
                else
                {
                    return checkPath;
                }
            }
            else
            {
                // not URL
                // check folder path
                if (!IsFullPath(checkPath))
                {
                    beforeVariable = Path.Combine(Path.GetDirectoryName(engine.FileName), beforeVariable);
                }
                // check extension
                if (!HasExtension(checkPath) && (setting.supportExtension == PropertyFilePathSetting.ExtensionBehavior.RequiredExtension))
                {
                    var extensions = setting.GetExtensions();
                    if (extensions.Count > 0)
                    {
                        afterVariable = afterVariable + "." + extensions[0];
                    }
                }

                string fmt = "";
                switch (counterVariableName)
                {
                    case "FileCounter.F0":
                        fmt = "0";
                        break;
                    case "FileCounter.F00":
                        fmt = "00";
                        break;
                    case "FileCounter.F000":
                        fmt = "000";
                        break;
                }

                int max = engine.engineSettings.MaxFileCounter;
                switch(setting.supportFileCounter)
                {
                    case PropertyFilePathSetting.FileCounterBehavior.FirstNotExists:
                        for (int i = 1; i <= max; i++)
                        {
                            var testPath = beforeVariable + i.ToString(fmt) + afterVariable;
                            if (!File.Exists(testPath))
                            {
                                return testPath;
                            }
                        }
                        break;

                    case PropertyFilePathSetting.FileCounterBehavior.LastExists:
                        for (int i = 1; i <= max; i++)
                        {
                            var testPath = beforeVariable + i.ToString(fmt) + afterVariable;
                            if (!File.Exists(testPath))
                            {
                                if (i > 1)
                                {
                                    return beforeVariable + (i - 1).ToString(fmt) + afterVariable;
                                }
                                else
                                {
                                    return testPath;
                                }
                            }
                        }
                        break;
                }
                // not found :-(
                return beforeVariable + max.ToString(fmt) + afterVariable;
            }
        }

        /// <summary>
        /// convert to FilePath not support FileCounter
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="setting"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string ConvertToUserVariableAsFilePath_NoSupportFileCounter(string parameterValue, PropertyFilePathSetting setting, Engine.AutomationEngineInstance engine)
        {
            var path = parameterValue.ConvertToUserVariable(engine);

            if (IsURL(path))
            {
                // path is URL
                if (!setting.allowURL)
                {
                    throw new Exception("Path is URL. Value: '" + path + "'");
                }
                else
                {
                    return path;
                }
            }
            else
            {
                // path is not URL
                // when folder path not contains
                if (!IsFullPath(path))
                {
                    path = Path.Combine(Path.GetDirectoryName(engine.FileName), path);
                }

                if (HasExtension(path))
                {
                    // has extension. no more path convert process
                    return path;
                }
                else
                {
                    // don't has extension

                    var extensions = setting.GetExtensions();
                    switch (setting.supportExtension)
                    {
                        case PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension:
                            return path;

                        case PropertyFilePathSetting.ExtensionBehavior.RequiredExtension:
                            if (extensions.Count > 0)
                            {
                                return path + "." + extensions[0];
                            }
                            break;

                        case PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists:
                            foreach(var ext in extensions)
                            {
                                var testPath = path + "." + ext;
                                if (File.Exists(testPath))
                                {
                                    return testPath;
                                }
                            }
                            break;
                    }

                    if (extensions.Count > 0)
                    {
                        return path + "." + extensions[0];
                    }
                    else
                    {
                        return path;
                    }
                }
            }
        }

        /// <summary>
        /// convert to FilePath. this method use the specified PropertyFilePathSetting
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="setting"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ConvertToUserVariableAsFilePath(this string parameterValue, PropertyFilePathSetting setting, Engine.AutomationEngineInstance engine)
        {
            string p;
            if ((setting.supportFileCounter != PropertyFilePathSetting.FileCounterBehavior.NoSupport) &&
                (setting.supportExtension != PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists))
            {
                p = ConvertToUserVariableAsFilePath_SupportFileCounter(parameterValue, setting, engine);
            }
            else
            {
                p = ConvertToUserVariableAsFilePath_NoSupportFileCounter(parameterValue, setting, engine);
            }

            var invs = Path.GetInvalidPathChars();
            if (p.IndexOfAny(invs) < 0)
            {
                return p;
            }
            else
            {
                throw new Exception("File Path contains Invalid chars. Path: '" + p + "'");
            }
        }

        /// <summary>
        /// convert to FilePath. this method use PropertyFilePathSetting
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ConvertToUserVariableAsFilePath(this ScriptCommand command, string parameterName, Engine.AutomationEngineInstance engine)
        {
            var prop = command.GetProperty(parameterName);
            var vProp = prop.GetVirtualProperty();
            string parameterValue = prop.GetValue(command)?.ToString() ?? "";

            var pathSetting = PropertyControls.GetCustomAttributeWithVirtual<PropertyFilePathSetting>(prop, vProp) ?? new PropertyFilePathSetting();

            return ConvertToUserVariableAsFilePath(parameterValue, pathSetting, engine);
        }

        /// <summary>
        /// convert to File Name
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ConvertToUserVariableAsFileName(this string fileName, Engine.AutomationEngineInstance engine)
        {
            var fn = fileName.ConvertToUserVariable(engine);
            var invs = Path.GetInvalidFileNameChars();
            if (fn.IndexOfAny(invs) < 0)
            {
                return fn;
            }
            else
            {
                throw new Exception("File Name contains invalid chars. File: '" + fn + "'");
            }
        }

        /// <summary>
        /// format file/folder path to specified format
        /// </summary>
        /// <param name="path"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string FormatFileFolderPath(string path, string format)
        {
            switch (format.ToLower())
            {
                case "file":
                case "filename":
                case "fn":
                    return Path.GetFileName(path);

                case "folder":
                case "directory":
                case "dir":
                    return Path.GetDirectoryName(path);

                case "filewithoutextension":
                case "filenamewithoutextension":
                case "fnwoext":
                    return Path.GetFileNameWithoutExtension(path);

                case "extension":
                case "ext":
                    return Path.GetExtension(path);

                case "drive":
                case "drivename":
                case "root":
                    return Path.GetPathRoot(path);

                default:
                    return "";
            }
        }

        #endregion

        #region wait for file
        /// <summary>
        /// Wait For File
        /// </summary>
        /// <param name="path"></param>
        /// <param name="waitTime"></param>
        /// <param name="engine"></param>
        /// <returns>file path</returns>
        /// <exception cref="Exception"></exception>
        public static string WaitForFile(string path, int waitTime, Engine.AutomationEngineInstance engine)
        {
            var ret = WaitControls.WaitProcess(waitTime, "File Path", new Func<(bool, object)>(() =>
            {
                if (IsURL(path))
                {
                    // if path is URL, don't check existance
                    return (true, path);
                }
                else
                {
                    if (File.Exists(path))
                    {
                        return (true, path);
                    }
                    else
                    {
                        return (false, null);
                    }
                }
            }), engine);

            if (ret is string returnPath)
            {
                return returnPath;
            }
            else
            {
                throw new Exception("Strange Value returned in WaitForFile. Type: " + ret.GetType().FullName);
            }
        }

        /// <summary>
        /// wait for file. this method NOT use PropertyFilePathSetting
        /// </summary>
        /// <param name="pathValue">NOT use PropertyFilePathSetting</param>
        /// <param name="waitTimeValue"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string WaitForFile(string pathValue, string waitTimeValue, Engine.AutomationEngineInstance engine)
        {
            var path = pathValue.ConvertToUserVariable(engine);
            var invs = Path.GetInvalidPathChars();
            if (path.IndexOfAny(invs) >= 0)
            {
                throw new Exception("File Path contains Invalid chars. Path: '" + path + "'");
            }

            var waitTime = waitTimeValue.ConvertToUserVariableAsInteger("Wait Time", engine);
            return WaitForFile(path, waitTime, engine);
        }

        /// <summary>
        /// wait for file. this method use PropertyFilePathSetting
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pathName">use PropertyFilePathSetting</param>
        /// <param name="waitTimeName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string WaitForFile(ScriptCommand command, string pathName, string waitTimeName, Engine.AutomationEngineInstance engine)
        {
            var path = command.ConvertToUserVariableAsFilePath(pathName, engine);
            var waitTime = command.ConvertToUserVariableAsInteger(waitTimeName, "Wait Time", engine);
            return WaitForFile(path, waitTime, engine);
        }

        /// <summary>
        /// general file action. This method search target file before execute actionFunc, and try store Found File Path after execute actionFunc.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pathName"></param>
        /// <param name="waitTimeName"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="pathResultName"></param>
        /// <param name="errorFunc"></param>
        private static void FileAction(ScriptCommand command, string pathName, string waitTimeName, Engine.AutomationEngineInstance engine, Action<string> actionFunc, string pathResultName = "", Action<Exception> errorFunc = null)
        {
            try
            {
                var path = WaitForFile(command, pathName, waitTimeName, engine);

                actionFunc(path);

                if (!string.IsNullOrEmpty(pathResultName))
                {
                    var pathResult = command.GetRawPropertyString(pathResultName, "Path Result");
                    if (!string.IsNullOrEmpty(pathResult))
                    {
                        path.StoreInUserVariable(engine, pathResult);
                    }
                }
            }
            catch(Exception ex)
            {
                if (errorFunc != null)
                {
                    errorFunc(ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// general file action. This method search target file before execute actionFunc, and try store Found File Path after execute actionFunc. This method specifies the parameter from the value of PropertyVirtualProperty
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void FileAction(ScriptCommand command, Engine.AutomationEngineInstance engine, Action<string> actionFunc, Action<Exception> errorFunc = null)
        {
            //var filePath = command.GetProperty(new PropertyVirtualProperty(nameof(FilePathControls), nameof(v_FilePath)))?.Name ??
            //                command.GetProperty(new PropertyVirtualProperty(nameof(FilePathControls), nameof(v_NoSample_FilePath)))?.Name ?? "";
            //var waitTime = command.GetProperty(new PropertyVirtualProperty(nameof(FilePathControls), nameof(v_WaitTime)))?.Name ?? "";
            //var pathResult = command.GetProperty(new PropertyVirtualProperty(nameof(FilePathControls), nameof(v_FilePathResult)))?.Name ?? "";

            var props = command.GetParameterProperties();
            var filePath = props.GetProperty(new PropertyVirtualProperty(nameof(FilePathControls), nameof(v_FilePath)))?.Name ??
                            props.GetProperty(new PropertyVirtualProperty(nameof(FilePathControls), nameof(v_NoSample_FilePath)))?.Name ?? "";
            var waitTime = props.GetProperty(new PropertyVirtualProperty(nameof(FilePathControls), nameof(v_WaitTime)))?.Name ?? "";
            var pathResult = props.GetProperty(new PropertyVirtualProperty(nameof(FilePathControls), nameof(v_FilePathResult)))?.Name ?? "";

            FileAction(command, filePath, waitTime, engine, actionFunc, pathResult, errorFunc);
        }
        #endregion

        /// <summary>
        /// get format information
        /// </summary>
        /// <returns></returns>
        public static string GetFormatHelp()
        {
            string help =
                @"File
FileName
fn
    File name.
Folder
Directory
dir
    Folder name.
FileWithoutExtension
FileNameWithoutExtension
fnwoext
    File name without extension.
Extension
ext
    File extension.
Drive
DriveName
root
    Drive name.

!!These are Case insensitive!!";
            return help;
        }
    }
}
