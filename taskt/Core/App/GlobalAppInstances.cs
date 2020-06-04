using System.Collections.Generic;

namespace taskt.Core.App
{
    public static class GlobalAppInstances
    {
        private static Dictionary<string, object> _appInstances { get; set; } = new Dictionary<string, object>();

        public static void AddInstance(string instanceName, object instanceObject)
        {
            //remove if already exists
            if (_appInstances.ContainsKey(instanceName))
            {
                _appInstances.Remove(instanceName);
            }

            //add to instance tracking
            _appInstances.Add(instanceName, instanceObject);
        }

        public static Dictionary<string, object> GetInstances()
        {
            return _appInstances;
        }
    }
}
