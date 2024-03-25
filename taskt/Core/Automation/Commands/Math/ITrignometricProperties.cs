﻿namespace taskt.Core.Automation.Commands
{
    public interface ITrignometricProperties : ILMathValueResultProperties
    {
        /// <summary>
        /// agnle type Radian or Degree
        /// </summary>
        string v_AngleType { get; set; }
    }
}
