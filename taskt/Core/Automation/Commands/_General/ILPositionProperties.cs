using taskt.Core.Automation.Commands._General;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// general position properties
    /// </summary>
    public interface ILPositionProperties : ILExpandableProperties
    {
        /// <summary>
        /// x position
        /// </summary>
        string v_XPosition { get; set; }

        /// <summary>
        /// y position
        /// </summary>
        string v_YPosition { get; set; }
    }
}
