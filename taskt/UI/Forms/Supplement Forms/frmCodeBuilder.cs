using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplemental
{
    public partial class frmCodeBuilder : Form
    {
        public frmCodeBuilder()
        {
            InitializeComponent();
        }
        public frmCodeBuilder(string code)
        {
            InitializeComponent();

            if (code != "")
            {
                rtbCode.Text = code;
                rtbCode_TextChanged(null, null);
            }

        }

        private void frmCodeBuilder_Load(object sender, EventArgs e)
        {
           
        }

        private void rtbCode_TextChanged(object sender, EventArgs e)
        {
            //Credits to Apex for RegEx Highlighting Sample
            //http://www.codingvision.net/interface/c-simple-syntax-highlighting


            // getting keywords/functions
            string keywords = @"\b(public|private|partial|static|namespace|class|using|void|foreach|in)\b";
            MatchCollection keywordMatches = Regex.Matches(rtbCode.Text, keywords);

            // getting types/classes from the text 
            string types = @"\b(Console)\b";
            MatchCollection typeMatches = Regex.Matches(rtbCode.Text, types);

            // getting comments (inline or multiline)
            string comments = @"(\/\/.+?$|\/\*.+?\*\/)";
            MatchCollection commentMatches = Regex.Matches(rtbCode.Text, comments, RegexOptions.Multiline);

            // getting strings
            string strings = "\".+?\"";
            MatchCollection stringMatches = Regex.Matches(rtbCode.Text, strings);

            // saving the original caret position + forecolor
            int originalIndex = rtbCode.SelectionStart;
            int originalLength = rtbCode.SelectionLength;
            Color originalColor = Color.Black;

            // MANDATORY - focuses a label before highlighting (avoids blinking)
            lblHeader.Focus();

            // removes any previous highlighting (so modified words won't remain highlighted)
            rtbCode.SelectionStart = 0;
            rtbCode.SelectionLength = rtbCode.Text.Length;
            rtbCode.SelectionColor = originalColor;

            // scanning...
            foreach (Match m in keywordMatches)
            {
                rtbCode.SelectionStart = m.Index;
                rtbCode.SelectionLength = m.Length;
                rtbCode.SelectionColor = Color.Blue;
            }

            foreach (Match m in typeMatches)
            {
                rtbCode.SelectionStart = m.Index;
                rtbCode.SelectionLength = m.Length;
                rtbCode.SelectionColor = Color.DarkCyan;
            }

            foreach (Match m in commentMatches)
            {
                rtbCode.SelectionStart = m.Index;
                rtbCode.SelectionLength = m.Length;
                rtbCode.SelectionColor = Color.Green;
            }

            foreach (Match m in stringMatches)
            {
                rtbCode.SelectionStart = m.Index;
                rtbCode.SelectionLength = m.Length;
                rtbCode.SelectionColor = Color.Brown;
            }

            // restoring the original colors, for further writing
            rtbCode.SelectionStart = originalIndex;
            rtbCode.SelectionLength = originalLength;
            rtbCode.SelectionColor = originalColor;

            // giving back the focus
            rtbCode.Focus();
        }

        private void utBtnCompile_Click(object sender, EventArgs e)
        {
            lstCompilerResults.Items.Clear();

            lstCompilerResults.Items.Add("Initializing Compiler Services..");

            var compilerSvc = new Core.CompilerServices();

            lstCompilerResults.Items.Add("Compiling..");
            var result = compilerSvc.CompileInput(rtbCode.Text);

            if (result.Errors.HasErrors)
            {
                foreach (var error in result.Errors)
                {
                    lstCompilerResults.Items.Add(error);
                }
            }
            else
            {
                lstCompilerResults.Items.Add("Compiled Successfully!");

                if (chkRunAfterCompile.Checked)
                    System.Diagnostics.Process.Start(result.PathToAssembly);
              

            }



        }

        private void uiBtnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnSample_Click(object sender, EventArgs e)
        {
            rtbCode.Text = @"// A Hello World! program in C#.
            using System;

            namespace HelloWorld
            {
            class Hello 
             {
               static void Main(string[] args) 
               {
                

               System.Console.WriteLine(""Number of command line parameters = {0}"", args.Length);

              foreach (string s in args)
              {
                System.Console.WriteLine(""Found Argument: "" + s);
              }


            Console.WriteLine(""Hi! This code was compiled on the fly from taskt!"");

                 //Keep the console window open, remove this if you do not want the exe to block
                 Console.WriteLine(""Press any key to exit."");
                 Console.ReadKey();
               }
             }
           }";
        }
    }
}
    