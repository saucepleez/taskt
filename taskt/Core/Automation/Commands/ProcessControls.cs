using System;
using System.Management;

namespace taskt.Core.Automation.Commands
{
    static internal class ProcessControls
    {
        /// <summary>
        /// get child processid
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="childIndex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static int GetChildProcessId(int processId, int childIndex)
        {
            if (childIndex < 0)
            {
                throw new Exception("Child Index is Less than Zero. Value: " + childIndex);
            }

            var query = "SELECT ProcessID FROM win32_process WHERE ParentProcessID = " + processId;
            var result = new ManagementObjectSearcher(query);
            var objCollection = result.Get();
            if (objCollection.Count < childIndex)
            {
                throw new Exception("Few Child Processes. Count: " +  objCollection.Count);
            }

            var cnt = 0;
            var procId = -1;
            foreach(var obj in objCollection)
            {
                if (cnt == childIndex)
                {
                    procId = int.Parse(obj.Properties["ProcessID"].Value.ToString());
                    break;
                }
                cnt++;
            }
            return procId;
        }
    }
}
