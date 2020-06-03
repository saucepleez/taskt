namespace taskt.Core.Automation.Engine
{
    public class ScriptError
    {
        public int LineNumber { get; set; }
        public string StackTrace { get; set; }
        public string ErrorMessage { get; set; }
    }
}
