using System;

namespace taskt.Core.Automation.Commands
{
    internal static class WaitControls
    {
        public static void WaitProcess(this ScriptCommand command, string waitTimeName, string targetName, Func<bool> waitFunc, Engine.AutomationEngineInstance engine)
        {
            int pauseTime = command.ConvertToUserVariableAsInteger(waitTimeName, "Wait Time", engine);

            var stopWaiting = DateTime.Now.AddSeconds(pauseTime);

            var fileFound = false;

            //while target not been found
            while (!fileFound)
            {
                if (waitFunc())
                {
                    fileFound = true;
                }

                //test if we should exit and throw exception
                if (DateTime.Now > stopWaiting)
                {
                    throw new Exception(targetName + " was not found in time!");
                }

                //put thread to sleep before iterating
                engine.ReportProgress(targetName + " Not Yet Found... " + (int)((stopWaiting - DateTime.Now).TotalSeconds) + "s remain");
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
