﻿namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// excel instance properties
    /// </summary>
    public interface ILExcelInstanceProperties : ILExpandableProperties
    {
        /// <summary>
        /// excel instance name
        /// </summary>
        string v_InstanceName { get; set; }
    }
}
