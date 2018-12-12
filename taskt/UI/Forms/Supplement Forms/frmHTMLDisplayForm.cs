using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplemental
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class frmHTMLDisplayForm : Form
    {
        public DialogResult Result { get; set; }
        public string TemplateHTML { get; set; }
        public frmHTMLDisplayForm()
        {
            InitializeComponent();
        }

        private void frmHTMLDisplayForm_Load(object sender, EventArgs e)
        {
            webBrowserHTML.ScriptErrorsSuppressed = true;
            webBrowserHTML.ObjectForScripting = this;
            webBrowserHTML.DocumentText = TemplateHTML;
            this.TopMost = true;
        }

        public void OK()
        {
            //Todo: figure out why return DialogResult not working for some reason

            Result = DialogResult.OK;
            this.Close();
        }
        public void Cancel()
        {
            //Todo: figure out why return DialogResult not working for some reason

            Result = DialogResult.Cancel;
            this.Close();
        }

        public List<Core.Script.ScriptVariable> GetVariablesFromHTML(string tagSearch)
        {

            var varList = new List<Core.Script.ScriptVariable>();
  
            HtmlElementCollection collection = webBrowserHTML.Document.GetElementsByTagName(tagSearch);
            for (int i = 0; i < collection.Count; i++)
            {
                var variableName = collection[i].GetAttribute("v_applyToVariable");

                if (!string.IsNullOrEmpty(variableName))
                {
                    var parentElement = collection[i];

                    if (tagSearch == "select")
                    {
                      
                        foreach (HtmlElement item in parentElement.Children)
                        {
                            if (item.GetAttribute("selected") == "True")
                            {
                                varList.Add(new Core.Script.ScriptVariable() { VariableName = variableName, VariableValue = item.InnerText });
                            }
                        }
                    }
                    else
                    {
                        if (parentElement.GetAttribute("type") == "checkbox")
                        {
                            var inputValue = collection[i].GetAttribute("checked");
                            varList.Add(new Core.Script.ScriptVariable() { VariableName = variableName, VariableValue = inputValue });
                        }
                        else
                        {
                            var inputValue = collection[i].GetAttribute("value");
                            varList.Add(new Core.Script.ScriptVariable() { VariableName = variableName, VariableValue = inputValue });
                        }

                      
                    }


                    




                }

               
            }

            return varList;

        }
    }
}
