using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Commands;

namespace taskt.Core.Script
{

    [Serializable]
    public class ScriptAction
    {
        /// <summary>
        /// generic 'top-level' user-defined script command (ex. not nested)
        /// </summary>
        [XmlElement(Order = 1)]
        public ScriptCommand ScriptCommand { get; set; }
        
        /// <summary>
        /// generic 'sub-level' commands (ex. nested commands within a loop)
        /// </summary>
        [XmlElement(Order = 2)]
        public List<ScriptAction> AdditionalScriptCommands { get; set; }

        /// <summary>
        /// adds a command as a nested command to a top-level command
        /// </summary>
        public ScriptAction AddAdditionalAction(ScriptCommand scriptCommand)
        {
            if (AdditionalScriptCommands == null)
            {
                AdditionalScriptCommands = new List<ScriptAction>();
            }

            ScriptAction newExecutionCommand = new ScriptAction() { ScriptCommand = scriptCommand };
            AdditionalScriptCommands.Add(newExecutionCommand);
            return newExecutionCommand;
        }

        public void ConvertToIntermediate(EngineSettings settings, List<ScriptVariable> variables)
        {
            ScriptCommand.ConvertToIntermediate(settings, variables);
            if (AdditionalScriptCommands != null && AdditionalScriptCommands.Count > 0)
            {
                foreach (var cmd in AdditionalScriptCommands)
                {
                    cmd.ConvertToIntermediate(settings, variables);
                }
            }
        }

        public void ConvertToRaw(EngineSettings settings)
        {
            ScriptCommand.ConvertToRaw(settings);
            if (AdditionalScriptCommands != null && AdditionalScriptCommands.Count > 0)
            {
                foreach (var cmd in AdditionalScriptCommands)
                {
                    cmd.ConvertToRaw(settings);
                }
            }
        }
    }
}
