using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using taskt.Core.Automation.Attributes.ClassAttributes;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// script commands information for forms etc
    /// </summary>
    public class ScriptCommandInformation
    {
        /// <summary>
        /// command name that contains namespace
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// command class name
        /// </summary>
        public string CommandName { get; private set; }

        /// <summary>
        /// display name (selection name)
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// command group name
        /// </summary>
        public string GruopName { get; private set; }

        /// <summary>
        /// command subgroup name
        /// </summary>
        public string SubGroupName { get; private set; }

        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="command"></param>
        public ScriptCommandInformation(Type command)
        {
            this.FullName = command.FullName;
            
            this.CommandName = this.FullName.Substring(this.FullName.LastIndexOf('.') + 1);

            var groupingAttribute = command.GetCustomAttribute<Group>();
            if (groupingAttribute != null)
            {
                var attr = groupingAttribute.groupName;
                if (attr.EndsWith(" Commands"))
                {
                    this.GruopName = attr;
                }
                else
                {
                    this.GruopName = $"{attr} Commands";
                }
            }

            var subGroupAttr = command.GetCustomAttribute<SubGruop>();
            this.SubGroupName = (subGroupAttr != null) ? subGroupAttr.subGruopName : "";

            var tcmd = (ScriptCommand)Activator.CreateInstance(command);
            this.DisplayName = tcmd.SelectionName;
        }

        /// <summary>
        /// create all ScriptCommands informations
        /// </summary>
        /// <returns></returns>
        public static List<ScriptCommandInformation> CreateScriptCommandInformations()
        {
            var commandTypes = Assembly.GetAssembly(typeof(ScriptCommand)).GetTypes()
                .Where(t =>
                {
                    return t.IsSubclassOf(typeof(ScriptCommand)) && !t.IsAbstract;
                });

            var ret = new List<ScriptCommandInformation>();
            foreach (var cmd in commandTypes)
            {
                ret.Add(new ScriptCommandInformation(cmd));
            }

            return ret;
        }
    }
}
