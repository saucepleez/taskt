using System;

namespace taskt.Core.Automation.Engine
{
    /// <summary>
    /// AppInstance properties extension methods
    /// </summary>
    public static class EM_AppInstancesPropertiesExtensionMethods
    {
        public static void AddAppInstance(this IAppInstancesProperties me, string instanceName, object appInstance, AutomationEngineInstance engine)
        {
            //if (me.AppInstances.ContainsKey(instanceName) && engine.engineSettings.OverrideExistingAppInstances)
            //{
            //    engine.ReportProgress($"Overriding Existing Instance. Instance Name: '{instanceName}'");
            //    me.AppInstances.Remove(instanceName);
            //}
            //else if (me.AppInstances.ContainsKey(instanceName) && !engineSettings.OverrideExistingAppInstances)
            //{
            //    throw new Exception("App Instance already exists and override has been disabled in engine settings! Enable override existing app instances or use unique instance names!");
            //}

            if (me.AppInstances.ContainsKey(instanceName))
            {
                if (engine.engineSettings.OverrideExistingAppInstances)
                {
                    engine.ReportProgress($"Overriding Existing Instance. Instance Name: '{instanceName}'");
                    me.AppInstances.Remove(instanceName);
                }
                else
                {
                    throw new Exception($"App Instance '{instanceName}' is already exists and override has been disabled in engine settings! Enable override existing app instances or use unique instance names!");
                }
            }

            try
            {
                me.AppInstances.Add(instanceName, appInstance);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get app instance
        /// </summary>
        /// <param name="me"></param>
        /// <param name="instanceName"></param>
        /// <returns></returns>
        public static object GetAppInstance(this IAppInstancesProperties me, string instanceName)
        {
            try
            {
                if (me.AppInstances.TryGetValue(instanceName, out object appObject))
                {
                    return appObject;
                }
                else
                {
                    throw new Exception($"App Instance '{instanceName}' not found!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// remove app instance
        /// </summary>
        /// <param name="me"></param>
        /// <param name="instanceName"></param>
        public static void RemoveAppInstance(this IAppInstancesProperties me, string instanceName)
        {
            try
            {
                if (me.AppInstances.ContainsKey(instanceName))
                {
                    me.AppInstances.Remove(instanceName);
                }
                else
                {
                    throw new Exception($"App Instance '{instanceName}' not found!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// create new instance name (not dup)
        /// </summary>
        /// <param name="me"></param>
        /// <param name="prefixName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetNewInstanceName(this IAppInstancesProperties me, string prefixName = "")
        {
            if (string.IsNullOrEmpty(prefixName))
            {
                prefixName = "AppInstance";
            }

            for (int i = 0; i < int.MaxValue; i++)
            {
                for (int j = 0;  j < int.MaxValue; j++)
                {
                    var checkInstanceName = $"{prefixName}_{i}_{j}";
                    if (!me.AppInstances.ContainsKey(checkInstanceName))
                    {
                        return (checkInstanceName);
                    }
                }
            }
            throw new Exception($"Sorry, New Instance Name does not created. Prefix: '{prefixName}'");
        }
    }
}
