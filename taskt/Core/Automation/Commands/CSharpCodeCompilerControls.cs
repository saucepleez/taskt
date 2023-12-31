using System.CodeDom.Compiler;

namespace taskt.Core.Automation.Commands
{
    public static class CSharpCodeCompilerControls
    {
        private const string compilerName = "tasktOnTheFly.exe";

        public static CompilerResults CompileInput(string codeInput)
        {
            //define file output
            //string Output = "tasktOnTheFly.exe";

            //create provider
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");

            //create compile parameters
            CompilerParameters parameters = new CompilerParameters
            {
                GenerateExecutable = true,
                OutputAssembly = compilerName,
            };

            //compile
            //CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, codeInput);
            //return results;
            return codeProvider.CompileAssemblyFromSource(parameters, codeInput);
        }
    }
}
