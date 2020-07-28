namespace taskt.Core.Model.EngineModel
{
    public class ScriptError
    {
        public string SourceFile { get; set; }
        public int LineNumber { get; set; }
        public string StackTrace { get; set; }
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
