using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Reflection;
using Newtonsoft.Json;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Drawing;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("API Commands")]
    [Attributes.ClassAttributes.Description("This command processes an HTML source object")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to parse and extract data from a successful **HTTP Request Command**")]
    [Attributes.ClassAttributes.ImplementationDescription("TBD")]
    public class ExecuteDLLCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the path to the DLL file")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter or Select the path to the DLL File")]
        [SampleUsage("C:\\temp\\myfile.dll or {vDLLFilePath}")]
        [Remarks("")]
        //[Attributes.PropertyAttributes.PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowDLLExplorer)]
        [PropertyCustomUIHelper("Launch DLL Explorer", nameof(lnkShowDLLExplorer_Clicked))]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the name of the class that contains the method to be invoked")]
        [InputSpecification("Provide the parent class name in the DLL.")]
        [SampleUsage("Namespace should be included, myNamespace.myClass*")]
        [Remarks("")]
        public string v_ClassName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the name of the method in the class to invoke")]
        [InputSpecification("Provide the method name in the DLL to be invoked.")]
        [SampleUsage("**GetSomething**")]
        [Remarks("")]
        public string v_MethodName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the variable to receive the result")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        [PropertyDescription("Please indicate the parameters (if required)")]
        [InputSpecification("Select the 'Generate Parameters' button once you have indicated a file, class, and method.")]
        [SampleUsage("")]
        [Remarks("")]
        //[Attributes.PropertyAttributes.PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.GenerateDLLParameters)]
        [PropertyCustomUIHelper("Generate DLL Parameter", nameof(lnkGenerateDLLParameters_Clicked))]
        public DataTable v_MethodParameters { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView ParametersGridViewHelper;

        public ExecuteDLLCommand()
        {
            this.CommandName = "ExecuteDLLCommand";
            this.SelectionName = "Execute DLL";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_MethodParameters = new DataTable();
            this.v_MethodParameters.Columns.Add("Parameter Name");
            this.v_MethodParameters.Columns.Add("Parameter Value");
            this.v_MethodParameters.TableName = DateTime.Now.ToString("MethodParameterTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            //ParametersGridViewHelper = new DataGridView();
            //ParametersGridViewHelper.AllowUserToAddRows = true;
            //ParametersGridViewHelper.AllowUserToDeleteRows = true;
            //ParametersGridViewHelper.Size = new Size(350, 125);
            //ParametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //ParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_MethodParameters", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public override void RunCommand(object sender)
        {
            //get file path
            var filePath = v_FilePath.ConvertToUserVariable(sender);
            var className = v_ClassName.ConvertToUserVariable(sender);
            var methodName = v_MethodName.ConvertToUserVariable(sender);

            //if file path does not exist
            if (!System.IO.File.Exists(filePath))
            {
                throw new System.IO.FileNotFoundException("DLL was not found at " + filePath);
            }

            //Load Assembly
            Assembly requiredAssembly = Assembly.LoadFrom(filePath);
            
            //get type
            Type t = requiredAssembly.GetType(className);

            //get all methods
            MethodInfo[] availableMethods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            //get method
            MethodInfo m = availableMethods.Where(f => f.ToString() == methodName).FirstOrDefault();


            //create instance
            var instance = requiredAssembly.CreateInstance(className);

            //check for parameters
            var reqdParams = m.GetParameters();

            //handle parameters
            object result;
            if (reqdParams.Length > 0)
            {

                //create parameter list
                var parameters = new List<object>();

                //get parameter and add to list
                foreach (var param in reqdParams)
                {
                    //declare parameter name
                    var paramName = param.Name;

                    //get parameter value
                    var requiredParameterValue = (from rws in v_MethodParameters.AsEnumerable()
                                                 where rws.Field<string>("Parameter Name") == paramName
                                                 select rws.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);

              
                    //get type of parameter
                    var paramType = param.GetType();


                    //check namespace and convert
                    if ((param.ParameterType.FullName == "System.Int32"))
                    {
                        var parseResult = int.Parse(requiredParameterValue);
                        parameters.Add(parseResult);
                    }
                    else if ((param.ParameterType.FullName == "System.Int64"))
                    {
                        var parseResult = long.Parse(requiredParameterValue);
                        parameters.Add(parseResult);
                    }
                    else if ((param.ParameterType.FullName == "System.Double"))
                    {
                        var parseResult = double.Parse(requiredParameterValue);
                        parameters.Add(parseResult);
                    }
                    else if ((param.ParameterType.FullName == "System.Boolean"))
                    {
                        var parseResult = bool.Parse(requiredParameterValue);
                        parameters.Add(parseResult);
                    }
                    else if ((param.ParameterType.FullName == "System.Decimal"))
                    {
                        var parseResult = decimal.Parse(requiredParameterValue);
                        parameters.Add(parseResult);
                    }
                    else if ((param.ParameterType.FullName == "System.Single"))
                    {
                        var parseResult = float.Parse(requiredParameterValue);
                        parameters.Add(parseResult);
                    }
                    else if ((param.ParameterType.FullName == "System.Char"))
                    {
                        var parseResult = char.Parse(requiredParameterValue);
                        parameters.Add(parseResult);
                    }
                    else if ((param.ParameterType.FullName == "System.String"))
                    {
                        parameters.Add(requiredParameterValue);
                    }
                    else if ((param.ParameterType.FullName == "System.DateTime"))
                    {
                        var parseResult = DateTime.Parse(requiredParameterValue);
                        parameters.Add(parseResult);
                    }
                    else if ((param.ParameterType.IsArray))
                    {
                        var parseResult = requiredParameterValue.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries);
                        parameters.Add(parseResult);
                    }
                    else
                    {
                        throw new NotImplementedException("Only system parameter types are supported!");
                    }
                  
                }

                //invoke
                result = m.Invoke(instance, parameters.ToArray());
            }
            else
            {
                //invoke
                result = m.Invoke(instance, null);
            }

            //check return type
            var returnType = result.GetType();

            //check namespace
            if (returnType.Namespace != "System" || returnType.IsArray)
            {
                //conversion of type is required due to type being a complex object

                //set json settings
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Error = (serializer, err) => {
                    err.ErrorContext.Handled = true;
                };

                //set indent
                settings.Formatting = Formatting.Indented;

                //convert to json
                result = JsonConvert.SerializeObject(result, settings);
            }
    
            //store result in variable
            result.ToString().StoreInUserVariable(sender, v_applyToVariableName);

        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            ParametersGridViewHelper = CommandControls.CreateDefaultDataGridViewFor("v_MethodParameters", this, true, true, true, -1, 135);
            ParametersGridViewHelper.CellClick += DataTableControls.AllEditableDataGridView_CellClick;

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ClassName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MethodName", this, editor));

            //create control for variable name
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_applyToVariableName", this, VariableNameControl , editor));
            RenderedControls.Add(VariableNameControl);

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_MethodParameters", this));
            RenderedControls.AddRange(CommandControls.CreateCustomUIHelpersFor("v_MethodParameters", this, ParametersGridViewHelper, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_MethodParameters", this, ParametersGridViewHelper, editor));
            RenderedControls.Add(ParametersGridViewHelper);

            return RenderedControls;
        }


        //public void ParametersGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex >= 0)
        //    {
        //        ParametersGridViewHelper.BeginEdit(false);
        //    }
        //    else
        //    {
        //        ParametersGridViewHelper.EndEdit();
        //    }
        //}

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Call Method '" + v_MethodName + "' in '" + v_ClassName + "']";
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            if (ParametersGridViewHelper.IsCurrentCellDirty || ParametersGridViewHelper.IsCurrentRowDirty)
            {
                ParametersGridViewHelper.CommitEdit(DataGridViewDataErrorContexts.Commit);
                var newRow = v_MethodParameters.NewRow();
                v_MethodParameters.Rows.Add(newRow);
                for (var i = v_MethodParameters.Rows.Count - 1; i >= 0; i--)
                {
                    if (v_MethodParameters.Rows[i][0].ToString() == "" && v_MethodParameters.Rows[i][1].ToString() == "")
                    {
                        v_MethodParameters.Rows[i].Delete();
                    }
                }
            }

            // not work yet this code
            //DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_MethodParameters)], v_MethodParameters);
        }

        private void lnkGenerateDLLParameters_Clicked(object sender, EventArgs e)
        {
            //Core.Automation.Commands.ExecuteDLLCommand cmd = (Core.Automation.Commands.ExecuteDLLCommand)CurrentEditor.selectedCommand;

            //var filePath = CurrentEditor.flw_InputVariables.Controls["v_FilePath"].Text;
            //var className = CurrentEditor.flw_InputVariables.Controls["v_ClassName"].Text;
            //var methodName = CurrentEditor.flw_InputVariables.Controls["v_MethodName"].Text;
            var filePath = v_FilePath;
            var className = v_ClassName;
            var methodName = v_MethodName;

            //DataGridView parameterBox = (DataGridView)CurrentEditor.flw_InputVariables.Controls["v_MethodParameters"];

            //clear all rows
            //cmd.v_MethodParameters.Rows.Clear();
            v_MethodParameters.Rows.Clear();

            //Load Assembly
            try
            {
                Assembly requiredAssembly = Assembly.LoadFrom(filePath);

                //get type
                Type t = requiredAssembly.GetType(className);

                //verify type was found
                if (t == null)
                {
                    MessageBox.Show("The class '" + className + "' was not found in assembly loaded at '" + filePath + "'", "Class Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get method
                MethodInfo m = t.GetMethod(methodName);

                //verify method found
                if (m == null)
                {
                    MessageBox.Show("The method '" + methodName + "' was not found in assembly loaded at '" + filePath + "'", "Method Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get parameters
                var reqdParams = m.GetParameters();
                if (reqdParams.Length > 0)
                {
                    //cmd.v_MethodParameters.Rows.Clear();
                    v_MethodParameters.Rows.Clear();
                    foreach (var param in reqdParams)
                    {
                        //cmd.v_MethodParameters.Rows.Add(param.Name, "");
                        v_MethodParameters.Rows.Add(param.Name, "");
                    }
                }
                else
                {
                    MessageBox.Show("There are no parameters required for this method!", "No Parameters Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error generating the parameters: " + ex.ToString());
            }
        }

        private void lnkShowDLLExplorer_Clicked(object sender, EventArgs e)
        {
            //create form
            using (UI.Forms.Supplemental.frmDLLExplorer dllExplorer = new UI.Forms.Supplemental.frmDLLExplorer())
            {
                //show dialog
                if (dllExplorer.ShowDialog() == DialogResult.OK)
                {
                    //user accepted the selections
                    //declare command
                    //Core.Automation.Commands.ExecuteDLLCommand cmd = (Core.Automation.Commands.ExecuteDLLCommand)CurrentEditor.selectedCommand;

                    //add file name
                    if (!string.IsNullOrEmpty(dllExplorer.FileName))
                    {
                        //CurrentEditor.flw_InputVariables.Controls["v_FilePath"].Text = dllExplorer.FileName;
                        v_FilePath = dllExplorer.FileName;
                    }

                    //add class name
                    if (dllExplorer.lstClasses.SelectedItem != null)
                    {
                        //CurrentEditor.flw_InputVariables.Controls["v_ClassName"].Text = dllExplorer.lstClasses.SelectedItem.ToString();
                        v_ClassName = dllExplorer.lstClasses.SelectedItem.ToString();
                    }

                    //add method name
                    if (dllExplorer.lstMethods.SelectedItem != null)
                    {
                        //CurrentEditor.flw_InputVariables.Controls["v_MethodName"].Text = dllExplorer.lstMethods.SelectedItem.ToString();
                        v_MethodName = dllExplorer.lstMethods.SelectedItem.ToString();
                    }

                    //cmd.v_MethodParameters.Rows.Clear();
                    v_MethodParameters.Rows.Clear();

                    //add parameters
                    if ((dllExplorer.lstParameters.Items.Count > 0) && (dllExplorer.lstParameters.Items[0].ToString() != "This method requires no parameters!"))
                    {
                        foreach (var param in dllExplorer.SelectedParameters)
                        {
                            //cmd.v_MethodParameters.Rows.Add(param, "");
                            v_MethodParameters.Rows.Add(param, "");
                        }
                    }
                }
            }
        }

    }
}
