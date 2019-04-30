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
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the DLL file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the DLL File")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfile.dll or [vDLLFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowDLLExplorer)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the name of the class that contains the method to be invoked")]
        [Attributes.PropertyAttributes.InputSpecification("Provide the parent class name in the DLL.")]
        [Attributes.PropertyAttributes.SampleUsage("Namespace should be included, myNamespace.myClass*")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ClassName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the name of the method in the class to invoke")]
        [Attributes.PropertyAttributes.InputSpecification("Provide the method name in the DLL to be invoked.")]
        [Attributes.PropertyAttributes.SampleUsage("**GetSomething**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_MethodName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the result")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.GenerateDLLParameters)]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the parameters (if required)")]
        [Attributes.PropertyAttributes.InputSpecification("Select the 'Generate Parameters' button once you have indicated a file, class, and method.")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
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

            ParametersGridViewHelper = new DataGridView();
            ParametersGridViewHelper.AllowUserToAddRows = true;
            ParametersGridViewHelper.AllowUserToDeleteRows = true;
            ParametersGridViewHelper.Size = new Size(350, 125);
            ParametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_MethodParameters", false, DataSourceUpdateMode.OnPropertyChanged);

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
                result = Newtonsoft.Json.JsonConvert.SerializeObject(result, settings);
            }
    
            //store result in variable
            result.ToString().StoreInUserVariable(sender, v_applyToVariableName);

        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ClassName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MethodName", this, editor));

            //create control for variable name
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_MethodParameters", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_MethodParameters", this, new Control[] { ParametersGridViewHelper }, editor));
            RenderedControls.Add(ParametersGridViewHelper);
    


            return RenderedControls;
        }


        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Call Method '" + v_MethodName + "' in '" + v_ClassName + "']";
        }
    }
}
