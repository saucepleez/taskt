﻿namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// Dictionary properties
    /// </summary>
    public interface ILDictionaryProperties : ICanHandleDictionary
    {
        /// <summary>
        /// Dictionary variable name
        /// </summary>
        string v_Dictionary { get; set; }
    }
}
