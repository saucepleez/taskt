using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    public static class GlobalAppInstances
    {
        private static Dictionary<string, object> AppInstances { get; set; } = new Dictionary<string, object>();

        public static void AddInstance(string instanceName, object instanceObject)
        {
            //remove if already exists
            if (AppInstances.ContainsKey(instanceName))
            {
                AppInstances.Remove(instanceName);
            }

            //add to instance tracking
            AppInstances.Add(instanceName, instanceObject);
        }

        public static Dictionary<string, object> GetInstances()
        {
            return AppInstances;
        }


    }

    


}
