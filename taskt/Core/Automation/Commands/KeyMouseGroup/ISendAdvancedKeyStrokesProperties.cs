using System.Data;

namespace taskt.Core.Automation.Commands.KeyMouseGroup
{
    /// <summary>
    /// send advanced key strokes properties
    /// </summary>
    public interface ISendAdvancedKeyStrokesProperties : IKeyActionCoreProperties, IHaveDataTableElements
    {
        /// <summary>
        /// key actions
        /// </summary>
        DataTable v_KeyActions { get; set; }

        /// <summary>
        /// force key up after keys down
        /// </summary>
        string v_KeyUpDefault { get; set; }
    }
}
