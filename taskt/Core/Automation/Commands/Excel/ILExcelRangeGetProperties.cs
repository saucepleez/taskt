﻿namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// excel Range get properties
    /// </summary>
    public interface ILExcelRangeGetProperties : ILExpandableProperties
    {
        /// <summary>
        /// variable name to store result
        /// </summary>
        string v_Result { get; set; }
    }
}
