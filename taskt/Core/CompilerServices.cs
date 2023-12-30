using System.CodeDom.Compiler;

namespace taskt.Core
{
    public class CompilerServices
    {
        public CompilerResults CompileInput(string codeInput)
        {
            //define file output
            string Output = "tasktOnTheFly.exe";

            //create provider
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");

            //create compile parameters
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = Output;

            //compile
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, codeInput);
            return results;
        }
    }
}
