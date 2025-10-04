using System;
using System.IO;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    public static class EM_CanHandleFolderPathExtensionMethods
    {
        /// <summary>
        /// Expand Value or user variable as Folder Path
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string ExpandValueOrUserVariableAsFolderPath(this ICanHandleFolderPath command, string parameterName, AutomationEngineInstance engine)
        {
            var prop = command.ToScriptCommand().GetProperty(parameterName);
            string parameterValue = prop.GetValue(command)?.ToString() ?? "";

            var path = parameterValue.ExpandValueOrUserVariable(engine);

            if (!EM_CanHandleFileOrFolderPathExtensionMethods.IsFullPath(path))
            {
                path = Path.Combine(Path.GetDirectoryName(engine.FileName), path);
            }

            if (EM_CanHandleFileOrFolderPathExtensionMethods.IsValidPathString(path)) 
            {
                return path;
            }
            else
            {
                throw new Exception($"Folder Path contains invalid chars. Path: '{path}'");
            }
        }

        /// <summary>
        /// store folder path in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="path"></param>
        /// <param name="parameterName"></param>
        /// <param name="engine"></param>
        /// <exception cref="Exception"></exception>
        public static void StoreFolderPathInUserVariable(this ICanHandleFolderPath command, string path, string parameterName, AutomationEngineInstance engine)
        {
            if (EM_CanHandleFileOrFolderPathExtensionMethods.IsValidPathString(path))
            {
                var variableName = command.ToScriptCommand().GetRawPropertyValueAsString(parameterName, "Folderle Path");
                ExtensionMethods.StoreInUserVariable(variableName, path, engine);
            }
            else
            {
                throw new Exception($"Invalid Folder Path. Can not Store In User Variables. Path: '{path}'");
            }
        }
    }
}
