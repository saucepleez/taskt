namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for C# code/file compile properties
    /// </summary>
    interface ICSharpCompileProperties
    {
        /// <summary>
        /// executable file name
        /// </summary>
        string v_ExecutableFileName { get; set; }

        /// <summary>
        /// C# language version
        /// </summary>
        string v_CSharpLanguageVersion { get; set; }
    }
}
