using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.IO
{
    public class JavaInterface
    {
        private System.Diagnostics.Process Create(string jarName, string args)
        {
            var jarLibary = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Resources", jarName);

            if (!System.IO.File.Exists(jarLibary))
            {
                throw new System.IO.FileNotFoundException("JAR Library was not found at " + jarLibary);
            }


            System.Diagnostics.Process javaProc = new System.Diagnostics.Process();
            javaProc.StartInfo.FileName = "java";
            javaProc.StartInfo.Arguments = string.Join(" ", "-jar", "\"" + jarLibary + "\"", args);
            javaProc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
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
