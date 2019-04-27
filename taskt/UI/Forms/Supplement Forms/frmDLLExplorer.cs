using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplemental
{
    public partial class frmDLLExplorer : UIForm
    {
        public List<Class> Classes { get; set; } = new List<Class>();
        public string FileName { get; set; }
        public List<string> SelectedParameters { get; set;}
        public frmDLLExplorer()
        {
            InitializeComponent();
        }

        private void lstClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstMethods.Items.Clear();

            var availableMethods = Classes.Where(f => f.ClassName == (string)lstClasses.SelectedItem).Select(f => f.Methods).FirstOrDefault();


            foreach (var method in availableMethods)
            {
                lstMethods.Items.Add(method.MethodName);
            }


        }

        private void lstMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstParameters.Items.Clear();

            var availableMethods = Classes.Where(f => f.ClassName == (string)lstClasses.SelectedItem).Select(f => f.Methods).FirstOrDefault();
            var method = availableMethods.Where(f => f.MethodName == (string)lstMethods.SelectedItem).FirstOrDefault();

            if (method.Parameters.Count == 0)
            {
                lstParameters.Items.Add("This method requires no parameters!");
            }
            else
            {
                SelectedParameters = new List<string>();
                foreach (var param in method.Parameters)
                {
                    lstParameters.Items.Add("Parameter: " + param.ParameterName + "<" + param.ParameterType + ">");
                    SelectedParameters.Add(param.ParameterName);
                }
            }

        }

        private void uiPictureButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    if (!ofd.FileName.ToLower().Contains(".dll"))
                    {
                        MessageBox.Show("Please select a valid DLL file!", "Invalid File Extension Selected", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    //track filename
                    FileName = ofd.FileName;


                    //add class list
                    Classes = new List<Class>();

                    //load assembly
                    Assembly requiredAssembly = Assembly.LoadFrom(FileName);

                    //get classes
                    var ClassTypes = requiredAssembly.GetTypes();


                    //load each class found
                    foreach (var classType in ClassTypes)
                    {
                        var newClass = new Class();
                        newClass.ClassName = classType.FullName;

                        var methods = classType.GetMethods();
                        foreach (var method in methods)
                        {
                            var newMethod = new Method();
                            newMethod.MethodName = method.ToString();

                            var reqdParams = method.GetParameters();

                            foreach (var param in reqdParams)
                            {
                                var newParam = new Parameter();
                                newParam.ParameterName = param.Name;
                                newParam.ParameterType = param.ParameterType.FullName;
                                newMethod.Parameters.Add(newParam);
                            }

                            newClass.Methods.Add(newMethod);

                        }

                        Classes.Add(newClass);

                    }


                }

                //populate class listbox
                lstClasses.Items.Clear();
                foreach (var className in Classes)
                {
                    lstClasses.Items.Add(className.ClassName);
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("There was an error: " + ex.ToString(), "Error");
            }
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }

    public class Class
    {
        public string ClassName { get; set; }
        public List<Method> Methods { get; set; } = new List<Method>();
    }
    public class Method
    {
        public string MethodName { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();
    }
    public class Parameter
    {
        public string ParameterName { get; set; }
        public string ParameterType { get; set; }
    }
}
