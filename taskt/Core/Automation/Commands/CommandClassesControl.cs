using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// management commands class name and full name
    /// </summary>
    public static class CommandClassesControl
    {
        /// <summary>
        /// classname & fullname dictionary
        /// </summary>
        private static Dictionary<string, string> commandClassDic = null;

        /// <summary>
        /// search base namespace
        /// </summary>
        private const string NSBase = "taskt.Core.Automation.Commands";

        static CommandClassesControl()
        {
            CreateCommandClassDic();
        }

        /// <summary>
        /// create classname and namespace dictionary
        /// </summary>
        private static void CreateCommandClassDic()
        {
            commandClassDic = new Dictionary<string, string>();

            string getClassName(Type x)
            {
                var fn = x.FullName;
                return fn.Substring(fn.LastIndexOf('.') + 1); ;
            }

            var asm = Assembly.GetExecutingAssembly();
            var types = asm.GetTypes().Where(p =>
            {
                if (p.IsClass)
                {
                    //// check abstract class
                    //if (p.IsAbstract && !p.IsSealed)
                    //{
                    //    return false;
                    //}

                    // class, sealed class, static class
                    var ns = p.Namespace ?? "";
                    var name = getClassName(p);
                    return (
                        ((ns == NSBase) || (ns.StartsWith($"{NSBase}."))) &&
                        (!name.StartsWith("EM_")) &&
                        (!name.Contains("+<>"))
                    );
                }
                else
                {
                    return false;
                }
            });
            foreach (var t in types)
            {
                var full = t.FullName;
                var name = getClassName(t);
                if (!commandClassDic.ContainsKey(name))
                {
                    commandClassDic.Add(name, full);
                }
                else
                {
                    throw new Exception($"Duplicate classname. class: '{name}', fullname: '{full}'");
                }
            }
        }

        /// <summary>
        /// return class full name (commands class)
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetClassFullName(string className)
        {
            if (commandClassDic.ContainsKey(className))
            {
                return commandClassDic[className];
            }
            else
            {
                throw new Exception($"Class Name does not exists. class: '{className}'");
            }
        }
    }
}
