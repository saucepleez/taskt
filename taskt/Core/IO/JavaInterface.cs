using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace taskt.Core.IO
{
    public class JavaInterface
    {
        private Process Create(string jarName, string args)
        {
            var jarLibary = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources", jarName);

            if (!File.Exists(jarLibary))
            {
                throw new FileNotFoundException("JAR Library was not found at " + jarLibary);
            }

            Process javaProc = new Process();
            javaProc.StartInfo.FileName = "java";
            javaProc.StartInfo.Arguments = string.Join(" ", "-jar", "\"" + jarLibary + "\"", args);
            javaProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            javaProc.StartInfo.UseShellExecute = false;
            javaProc.StartInfo.RedirectStandardOutput = true;

            return javaProc;
        }

        public string ExtractPDFText(string pdfFilePath)
        {

            //create pdf path
            var pdfPath = "\"" + pdfFilePath + "\"";

            //create args
            var args = string.Join(" ", pdfPath);

            //create interface process
            var javaInterface = Create("taskt-ExtractPDFText.jar", args);

            //run command line
            javaInterface.Start();

            //track output
            var output = javaInterface.StandardOutput.ReadToEnd();

            //wait for exist
            javaInterface.WaitForExit();

            //close and dispose
            javaInterface.Close();

            //return data
            return output;
        }
    }
}
