namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// both JSONObject and JSONArray
    /// </summary>
    public interface ICanHandleJContainer : ICanHandleJSONArray, ICanHandleJSONObject
    {
        // nothing
    }
}
