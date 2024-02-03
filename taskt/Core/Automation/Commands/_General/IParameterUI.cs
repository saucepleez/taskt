using System.Collections.Generic;
using System.Windows.Forms;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for search & use Control for ScriptCommand parameters
    /// </summary>
    public interface IParameterUI
    {
        Dictionary<string, Control> ControlsList { get; }
    }
}
