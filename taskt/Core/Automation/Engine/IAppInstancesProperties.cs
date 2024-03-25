using System.Collections.Generic;

namespace taskt.Core.Automation.Engine
{
    /// <summary>
    /// App Instances properties
    /// </summary>
    public interface IAppInstancesProperties
    {
        Dictionary<string, object> AppInstances { get; set; }
    }
}
