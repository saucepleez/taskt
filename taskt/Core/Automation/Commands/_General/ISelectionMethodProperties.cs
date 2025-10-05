using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// general selection properties
    /// </summary>
    public interface ISelectionMethodProperties
    {
        /// <summary>
        /// selection method
        /// </summary>
        string v_SelectionMethod { get; set; }
    }
}
