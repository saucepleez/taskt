using OpenQA.Selenium;
using System;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands.WebBrowserGroup
{
    public static class EM_CanHandleWebBrowserExtentionMethods
    {
        /// <summary>
        /// Create WebBrowser Intance
        /// </summary>
        /// <param name="command"></param>
        /// <param name="instanceName">instance name</param>
        /// <param name="driver"></param>
        /// <param name="profilePath"></param>
        /// <param name="engine"></param>
        public static void CreateWebBrowserInstance(this ICanHandleWebDriver command, string instanceName, IWebDriver driver, string profilePath, Engine.AutomationEngineInstance engine)
        {
            var expandedInstanceName = instanceName.ExpandValueOrUserVariable(engine);

            engine.AddAppInstance(expandedInstanceName, (driver, profilePath));
        }

        /// <summary>
        /// Get WebBrowser Instance and profile
        /// </summary>
        /// <param name="command"></param>
        /// <param name="instanceName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static (IWebDriver, string) GetWebBrowserInstanceAndProfilePath(this ICanHandleWebDriver command, string instanceName, AutomationEngineInstance engine)
        {
            var expandedInstanceName = instanceName.ExpandValueOrUserVariable(engine);

            var obj = engine.GetAppInstance(expandedInstanceName);

            if (IsWebBrowserInstance(obj, out ValueTuple<IWebDriver, string> pair)) 
            {
                return pair;
            }
            else
            {
                throw new Exception($"Specified Instance is Not WebBrowser Intance. Instance Name: '{instanceName}', Expand: '{expandedInstanceName}'");
            }
        }

        /// <summary>
        /// get WebBrowser Instance
        /// </summary>
        /// <param name="command"></param>
        /// <param name="instanceName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static IWebDriver GetWebBrowserIntance(this ICanHandleWebDriver command, string instanceName, AutomationEngineInstance engine)
        {
            (var ins, _) = command.GetWebBrowserInstanceAndProfilePath(instanceName, engine);
            return ins;
        }

        /// <summary>
        /// get WebBrowser Profile Path
        /// </summary>
        /// <param name="command"></param>
        /// <param name="instanceName"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string GetWebBrowserProfilePath(this ICanHandleWebDriver command, string instanceName, AutomationEngineInstance engine)
        {
            (_, var prof) = command.GetWebBrowserInstanceAndProfilePath(instanceName, engine);
            return prof;
        }

        /// <summary>
        /// get instance name from WebBrowser
        /// </summary>
        /// <param name="command"></param>
        /// <param name="driver"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string GetInstanceNameFromWebBrowserInstance(this ICanHandleWebDriver command, IWebDriver driver, AutomationEngineInstance engine)
        {
            (var name, _) = GetInstanceNameAndProfilePathFromWebDriver(driver, engine);
            return name;
        }

        /// <summary>
        /// get Profile path from WebBroswer Instance
        /// </summary>
        /// <param name="command"></param>
        /// <param name="driver"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string GetProfilePathFromWebBroswerInstance(this ICanHandleWebDriver command, IWebDriver driver, AutomationEngineInstance engine)
        {
            (_, var prof) = GetInstanceNameAndProfilePathFromWebDriver(driver, engine);
            return prof;
        }

        /// <summary>
        /// get WebBrowser instance name and profile path from WebDriver
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="engine"></param>
        /// <returns>(instance name, profile path)</returns>
        private static (string, string) GetInstanceNameAndProfilePathFromWebDriver(IWebDriver driver, AutomationEngineInstance engine)
        {
            var instances = engine.AppInstances;
            foreach (var kv in instances)
            {
                if (IsWebBrowserInstance(kv.Value, out ValueTuple<IWebDriver, string> pair))
                {
                    if (pair.Item1 == driver)
                    {
                        return (kv.Key, pair.Item2);
                    }
                }
            }
            return (string.Empty, string.Empty);
        }

        /// <summary>
        /// check object is WebBrowser instance
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ret"></param>
        /// <returns></returns>
        private static bool IsWebBrowserInstance(object obj, out ValueTuple<IWebDriver, string> ret)
        {
            ret = (null, null);
            if (obj is ValueTuple<IWebDriver, string> pair)
            {
                ret = pair;
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
