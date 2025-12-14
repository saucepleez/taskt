using System;
using System.Windows.Automation;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    public static class EM_UIElementSearchUIElementFromWindowSomethingByAnywayPropertiesExtensionMethods
    {
        /// <summary>
        /// Search Window after Action command core
        /// </summary>
        /// <param name="command"></param>
        /// <param name="engine"></param>
        /// <param name="searchFunc">search window process, InnerScriptVariable is variable to store Window UIElement</param>
        /// <param name="actionFunc">action process, InnerScriptVariable is Window UIElement</param>
        public static void SearchWindowAfterActionCore(this IUIElementSearchUIElementFromWindowSomethingByAnywayProperties command, Engine.AutomationEngineInstance engine, Func<InnerScriptVariable, ScriptCommand> searchFunc, Func<InnerScriptVariable, ScriptCommand> actionFunc)
        {
            using (var winElem = new InnerScriptVariable(engine))
            {
                var winSearch = searchFunc(winElem);
                winSearch.RunCommand(engine);

                var actionCmd = actionFunc(winElem);
                actionCmd.RunCommand(engine);

                command.StoreWindowUIElementInUserVariable((AutomationElement)winElem.VariableValue, engine);
            }
        }
    }
}
