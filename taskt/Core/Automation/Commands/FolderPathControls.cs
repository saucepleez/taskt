using System;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for folder path methods
    /// </summary>
    internal static class FolderPathControls
    {
        /// <summary>
        /// folder path
        /// </summary>
        [PropertyDescription("Folder Path")]
        [InputSpecification("Folder Path", true)]
        [PropertyDetailSampleUsage("**C:\\temp**", PropertyDetailSampleUsage.ValueType.Value, "Folder Path")]
        [PropertyDetailSampleUsage("**{{{vFilePath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Folder Path")]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Folder Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Folder")]
        public static string v_FolderPath { get; }

        /// <summary>
        /// for wait time
        /// </summary>
        [PropertyDescription("Wait Time for the Folder to Exist (sec)")]
        [InputSpecification("Wait Time", true)]
        [PropertyDetailSampleUsage("**10**", PropertyDetailSampleUsage.ValueType.Value, "Wait Time")]
        [PropertyDetailSampleUsage("**{{{vWaitTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Wait Time")]
        [Remarks("Specify how long to Wait before an Error will occur because the Folder is not Found.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyIsOptional(true, "10")]
        [PropertyFirstValue("10")]
        [PropertyDisplayText(true, "Wait", "s")]
        public static string v_WaitTime { get; }

        /// <summary>
        /// folder path result
        /// </summary>
        [PropertyDescription("Variable Name to Store Folder Path")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vPath**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyIsOptional(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Folder Path Result", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public static string v_FolderPathResult { get; }

        /// <summary>
        /// Wait For Folder
        /// </summary>
        /// <param name="path"></param>
        /// <param name="waitTime"></param>
        /// <param name="engine"></param>
        /// <returns>file path</returns>
        /// <exception cref="Exception"></exception>
        public static string WaitForFolder(string path, int waitTime, Engine.AutomationEngineInstance engine)
        {
            var ret = WaitControls.WaitProcess(waitTime, "Folder Path", new Func<(bool, object)>(() =>
            {
                if (Directory.Exists(path))
                {
                    return (true, path);
                }
                else
                {
                    return (false, null);
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
        /// wait for folder
        /// </summary>
        /// <param name="pathValue">NOT use PropertyFilePathSetting</param>
        /// <param name="waitTimeValue"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string WaitForFolder(string pathValue, string waitTimeValue, Engine.AutomationEngineInstance engine)
        {
            var path = pathValue.ConvertToUserVariableAsFolderPath(engine);
            var waitTime = waitTimeValue.ConvertToUserVariableAsInteger("Wait Time", engine);
            return WaitForFolder(path, waitTime, engine);
        }

        /// <summary>
        /// wait for folder
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pathName">use PropertyFilePathSetting</param>
        /// <param name="waitTimeName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string WaitForFolder(ScriptCommand command, string pathName, string waitTimeName, Engine.AutomationEngineInstance engine)
        {
            var path = command.ConvertToUserVariableAsFolderPath(pathName, engine);
            var waitTime = command.ConvertToUserVariableAsInteger(waitTimeName, "Wait Time", engine);
            return WaitForFolder(path, waitTime, engine);
        }

        /// <summary>
        /// general folder action. This method search target folder before execute actionFunc, and try store Found Folder Path after execute actionFunc. 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pathName"></param>
        /// <param name="waitTimeName"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="pathResultName"></param>
        /// <param name="errorFunc"></param>
        public static void FolderAction(ScriptCommand command, string pathName, string waitTimeName, Engine.AutomationEngineInstance engine, Action<string> actionFunc, string pathResultName = "", Action<Exception> errorFunc = null)
        {
            try
            {
                var path = WaitForFolder(command, pathName, waitTimeName, engine);
                actionFunc(path);

                if (!string.IsNullOrEmpty(pathResultName))
                {
                    var pathResult = command.GetRawPropertyString(pathResultName, "Folder Path Result");
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
        /// general folder action. This method search target folder before execute actionFunc, and try store Found Folder Path after execute actionFunc. This method specifies the parameter from the value of PropertyVirtualProperty.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="actionFunc"></param>
        /// <param name="errorFunc"></param>
        public static void FolderAction(ScriptCommand command, Engine.AutomationEngineInstance engine, Action<string> actionFunc,  Action<Exception> errorFunc = null)
        {
            var props = command.GetParameterProperties();

            var folderPath = props.GetProperty(new PropertyVirtualProperty(nameof(FolderPathControls), nameof(v_FolderPath)))?.Name ?? "";
            var waitTime = props.GetProperty(new PropertyVirtualProperty(nameof(FolderPathControls), nameof(v_WaitTime)))?.Name ?? "";
            var folderResult = props.GetProperty(new PropertyVirtualProperty(nameof(FolderPathControls), nameof(v_FolderPathResult)))?.Name ?? "";

            FolderAction(command, folderPath, waitTime, engine, actionFunc, folderResult, errorFunc);
        }

        /// <summary>
        /// Convert to FullPath specified path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        private static string ConvertToFullPath(string path, Engine.AutomationEngineInstance engine)
        {
            if (FilePathControls.IsFullPath(path))
            {
                return path;
            }
            else
            {
                return Path.Combine(Path.GetDirectoryName(engine.FileName), path);
            }
        }

        /// <summary>
        /// convert to Folder Path
        /// </summary>
        /// <param name="value"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ConvertToUserVariableAsFolderPath(this string value, Engine.AutomationEngineInstance engine)
        {
            var p = ConvertToFullPath(value.ConvertToUserVariable(engine), engine);
            var invs = Path.GetInvalidPathChars();
            if (p.IndexOfAny(invs) < 0)
            {
                return p;
            }
            else
            {
                throw new Exception("Folder Path contains Invalid chars. Path: '" + p + "'");
            }
        }

        /// <summary>
        /// convert to Folder Path
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterValue"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string ConvertToUserVariableAsFolderPath(this ScriptCommand command, string parameterValue, Engine.AutomationEngineInstance engine)
        {
            return command.ConvertToUserVariable(parameterValue, "Folder Path", engine).ConvertToUserVariableAsFolderPath(engine);
        }

        /// <summary>
        /// convert to folder name
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ConvertToUserVariableAsFolderName(this string folderName, Engine.AutomationEngineInstance engine)
        {
            var fn = folderName.ConvertToUserVariable(engine);
            var invs = Path.GetInvalidFileNameChars();
            if (fn.IndexOfAny(invs) < 0)
            {
                return fn;
            }
            else
            {
                throw new Exception("Folder Name contains invalid chars. Folder: '" + fn + "'");
            }
        }
    }
}
