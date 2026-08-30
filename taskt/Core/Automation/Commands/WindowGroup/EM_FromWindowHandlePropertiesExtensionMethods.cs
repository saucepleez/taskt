namespace taskt.Core.Automation.Commands.WindowGroup
{
    public static class EM_FromWindowHandlePropertiesExtensionMethods
    {
        /// <summary>
        /// store window name result in user variable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="windowName"></param>
        /// <param name="engine"></param>
        public static void StoreWindowNameResultInUserVariable(this IFromWindowHandleResultsProperties command, string windowName, Engine.AutomationEngineInstance engine)
        {
            if (!string.IsNullOrEmpty(command.v_WindowNameResult))
            {
                windowName.StoreInUserVariable(engine, command.v_WindowNameResult);
            }
        }
    }
}
