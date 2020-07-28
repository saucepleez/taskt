using System.Collections.Generic;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Script;

namespace taskt.Core.Infrastructure
{
    public interface IfrmCommandEditor
    {
        List<ScriptVariable> ScriptVariables { get; set; }
        List<ScriptElement> ScriptElements { get; set; }
        ScriptCommand SelectedCommand { get; set; }
        ScriptCommand OriginalCommand { get; set; }
        CreationMode CreationModeInstance { get; set; }
        string DefaultStartupCommand { get; set; }
        ScriptCommand EditingCommand { get; set; }
        List<ScriptCommand> ConfiguredCommands { get; set; }
    }
}
