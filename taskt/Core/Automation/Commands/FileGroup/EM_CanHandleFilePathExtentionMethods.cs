using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// extension methods for ICanHandleFilePath
    /// </summary>
    public static class EM_CanHandleFilePathExtentionMethods
    {
        /// <summary>
        /// check file path is full path, call EM_CanHandleFileOrFolderPathProperties
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFullPath(string path)
        {
            return EM_CanHandleFileOrFolderPathExtensionMethods.IsFullPath(path);
        }

        /// <summary>
        /// check file path is URL, call EM_CanHandleFileOrFolderPathProperties
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsURL(string path)
        {
            return EM_CanHandleFileOrFolderPathExtensionMethods.IsURL(path);
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
        /// check file path is valid
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsValidPathString(string path)
        {
            return EM_CanHandleFileOrFolderPathExtensionMethods.IsValidPathString(path);
        }

        /// <summary>
        /// check file path contains FileCounter variable
        /// </summary>
        /// <param name="path">don't convert variable</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static bool ContainsFileCounterVariable(string path, AutomationEngineInstance engine)
        {
            path = path ?? "";
            (_, var index) = GetLastFileCounter(path, engine);

            return (index >= 0);
        }

        /// <summary>
        /// get last FileCounter Variable name and position
        /// </summary>
        /// <param name="path">don't convert variable</param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static (string variableName, int index) GetLastFileCounter(string path, AutomationEngineInstance engine)
        {
            var f0 = VariableNameControls.GetWrappedVariableName(SystemVariables.FileCounter_F0.VariableName, engine);
            var f00 = VariableNameControls.GetWrappedVariableName(SystemVariables.FileCounter_F00.VariableName, engine);
            var f000 = VariableNameControls.GetWrappedVariableName(SystemVariables.FileCounter_F000.VariableName, engine);

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
        /// Get Before FileCounter Text, FileCounter Variable Name, After FileCounter Text
        /// </summary>
        /// <param name="path"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static (string beforeCounter, string counterVariable, string afterCounter) ParseFileCounterContainPath(string path, AutomationEngineInstance engine)
        {
            var r = GetLastFileCounter(path, engine);
            if (r.index < 0)
            {
                throw new Exception($"No FileCounter Variables contains. Path: '{path}'");
            }
            else
            {
                return (path.Substring(0, r.index), r.variableName, path.Substring(r.index + r.variableName.Length));
            }
        }

        /// <summary>
        /// expand value or user variable as FilePath support FileCounterVariable
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="settings"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string ExpandValueOrUserVariableAsFilePath_SupportFileCounter(string parameterValue, PropertyFilePathSetting settings, AutomationEngineInstance engine)
        {
            // check contains FileCounter
            if (!ContainsFileCounterVariable(parameterValue, engine))
            {
                // don't contains FileCounter
                return ExpandValueOrUserVariableAsFilePath_NoSupportFileCounter(parameterValue, settings, engine);
            }

            (var beforeVariable, var wrappedCounterVariableName, var afterVariable) = ParseFileCounterContainPath(parameterValue, engine);

            beforeVariable = beforeVariable.ExpandValueOrUserVariable(engine);
            afterVariable = afterVariable.ExpandValueOrUserVariable(engine);
            var counterVariableName = VariableNameControls.GetVariableName(wrappedCounterVariableName, engine);

            // URL Check
            var checkPath = $"{beforeVariable}0{afterVariable}";
            if (EM_CanHandleFileOrFolderPathExtensionMethods.IsURL(checkPath))
            {
                // path is URL, FileCounter, supportExtension does not work
                if (!settings.allowURL)
                {
                    throw new Exception($"Path is URL. Value: '{beforeVariable}{wrappedCounterVariableName}{afterVariable}'");
                }
                else
                {
                    return checkPath;
                }
            }
            else
            {
                // not URL
                int digits;
                if (counterVariableName == SystemVariables.FileCounter_F0.VariableName)
                {
                    digits = 1;
                }
                else if (counterVariableName == SystemVariables.FileCounter_F00.VariableName)
                {
                    digits = 2;
                }
                else if (counterVariableName == SystemVariables.FileCounter_F000.VariableName)
                {
                    digits = 3;
                }
                else
                {
                    throw new Exception($"Strange File Counter Variable. Name: '{counterVariableName}'");
                }

                var folderPath = Path.GetDirectoryName(beforeVariable);
                var fileNameBeforeCounter = Path.GetFileName(beforeVariable);

                var fileNameAfterCounter = Path.GetFileNameWithoutExtension(afterVariable);
                var fileExtension = Path.GetExtension(afterVariable);

                string res;
                switch (settings.supportFileCounter)
                {
                    case PropertyFilePathSetting.FileCounterBehavior.FirstNotExists:
                        using (var fn = new InnerScriptVariable(engine))
                        {
                            var notExists = new GetNonExistentFilePathCommand()
                            {
                                v_TargetFolderPath = folderPath,
                                v_BeforeFileCounter = fileNameBeforeCounter,
                                v_Digits = digits.ToString(),
                                v_AfterFileCounter = fileNameAfterCounter,
                                v_Extension = fileExtension,
                                v_Result = fn.VariableName,
                            };
                            notExists.RunCommand(engine);
                            res = fn.VariableValue.ToString();
                        }
                        break;
                    case PropertyFilePathSetting.FileCounterBehavior.LastExists:
                        using (var fn = new InnerScriptVariable(engine))
                        {
                            var lastExists = new GetLastExistentNumberedFilePathCommand()
                            {
                                v_TargetFolderPath = folderPath,
                                v_BeforeFileCounter = fileNameBeforeCounter,
                                v_Digits = digits.ToString(),
                                v_AfterFileCounter = fileNameAfterCounter,
                                v_Extension = fileExtension,
                                v_Result = fn.VariableName,
                            };
                            lastExists.RunCommand(engine);
                            res = fn.VariableValue.ToString();
                        }
                        break;
                    default:
                        throw new Exception($"Strange FilePathSetting Attribute Value. Value: '{settings.supportFileCounter.ToString()}'");
                }
                return res;
            }
        }

        /// <summary>
        /// expand value or User variable as FilePath not support FileCounterVariable
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="settings"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string ExpandValueOrUserVariableAsFilePath_NoSupportFileCounter(string parameterValue, PropertyFilePathSetting settings, AutomationEngineInstance engine)
        {
            var path = parameterValue.ExpandValueOrUserVariable(engine);

            if (EM_CanHandleFileOrFolderPathExtensionMethods.IsURL(path))
            {
                // path is URL
                if (!settings.allowURL)
                {
                    throw new Exception($"Path is URL. Value: '{path}'");
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
                if (!EM_CanHandleFileOrFolderPathExtensionMethods.IsFullPath(path))
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

                    var extensions = settings.GetExtensions();
                    switch (settings.supportExtension)
                    {
                        case PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension:
                            return path;

                        case PropertyFilePathSetting.ExtensionBehavior.RequiredExtension:
                            if (extensions.Count > 0)
                            {
                                return $"{path}.{extensions[0]}";
                            }
                            break;

                        case PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists:
                            foreach (var ext in extensions)
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
                        return $"{path}.{extensions[0]}";
                    }
                    else
                    {
                        return path;
                    }
                }
            }
        }

        /// <summary>
        /// method call controller expand value or user variable as filepath
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <param name="settings"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static string ExpandValueOrUserVarialbeAsFilePath_Controller(string parameterValue, PropertyFilePathSetting settings, AutomationEngineInstance engine)
        {
            string p;
            if ((settings.supportFileCounter != PropertyFilePathSetting.FileCounterBehavior.NoSupport) &&
                (settings.supportExtension != PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists))
            {
                p = ExpandValueOrUserVariableAsFilePath_SupportFileCounter(parameterValue, settings, engine);
            }
            else
            {
                p = ExpandValueOrUserVariableAsFilePath_NoSupportFileCounter(parameterValue, settings, engine);
            }

            if (IsValidPathString(p))
            {
                return p;
            }
            else
            {
                throw new Exception($"File Path contains Invalid chars. Path: '{p}'");
            }
        }

        /// <summary>
        /// Expand Value or user variable as FilePath, specified FilePathSettings
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="settings"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ExpandValueOrUserVariableAsFilePath(this ICanHandleFilePath command, string parameterName, PropertyFilePathSetting settings, AutomationEngineInstance engine)
        {
            var prop = command.ToScriptCommand().GetProperty(parameterName);
            string parameterValue = prop.GetValue(command)?.ToString() ?? "";
            return ExpandValueOrUserVarialbeAsFilePath_Controller(parameterValue, settings, engine);
        }

        /// <summary>
        /// expand value or user variable to FilePath. this method use PropertyFilePathSetting
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ExpandValueOrUserVariableAsFilePath(this ICanHandleFilePath command, string parameterName, AutomationEngineInstance engine)
        {
            var prop = command.ToScriptCommand().GetProperty(parameterName);
            var vProp = prop.GetVirtualProperty();
            string parameterValue = prop.GetValue(command)?.ToString() ?? "";

            var pathSetting = PropertyControls.GetCustomAttributeWithVirtual<PropertyFilePathSetting>(prop, vProp) ?? new PropertyFilePathSetting();

            return ExpandValueOrUserVarialbeAsFilePath_Controller(parameterValue, pathSetting, engine);
        }

        /// <summary>
        /// Store File Path in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="path"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <exception cref="Exception"></exception>
        public static void StoreFilePathInUserVariable(this ICanHandleFilePath command, string path, string parameterName, AutomationEngineInstance engine)
        {
            if (IsValidPathString(path))
            {
                var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(parameterName, "File Path");
                ExtensionMethods.StoreInUserVariable(variableName, path, engine);
            }
            else
            {
                throw new Exception($"Invalid File Path. Can not Store In User Variables. Path: '{path}'");
            }
        }
    }
}
