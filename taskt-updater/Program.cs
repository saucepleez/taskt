using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt_updater
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Debug
            if (args.Length == 0)
            {
                //args[0] = "https://github.com/rcktrncn/taskt-up-test/releases/download/v3.5.1.5/taskt-uob_v3.5.1.5.zip";
                string[] newArg = new string[1];
                newArg[0] = "https://github.com/rcktrncn/taskt-up-test/releases/download/v3.5.1.5/taskt-uob_v3.5.1.5.zip";
                args = newArg;
            }

            if (args.Count() == 0)
            {
                MessageBox.Show("Update Tool requires a package argument!");
                Application.Exit();
            }
            else
            {
                try
                {
                    Application.Run(new frmUpdating(args[0]));
                }
                catch (System.Reflection.TargetInvocationException)
                {

                }
            }
        }
    }
}
