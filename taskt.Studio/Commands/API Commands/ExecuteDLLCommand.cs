using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{

    [Serializable]
    [Group("API Commands")]
    [Description("This command invokes a method of a specific class from a DLL.")]

    public class ExecuteDLLCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("DLL File Path")]
        [InputSpecification("Enter or Select the path to the DLL File.")]
        [SampleUsage("C:\\temp\\myfile.dll || {vDLLFilePath}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowDLLExplorer)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Class Name")]
        [InputSpecification("Provide the parent class name of the method to be invoked in the DLL.")]
        [SampleUsage("myNamespace.myClassName || {vClassName}")]
        [Remarks("Namespace should be included")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ClassName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Method Name")]
        [InputSpecification("Provide the method name to be invoked in the DLL.")]
        [SampleUsage("GetSomething || {vMethodName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MethodName { get; set; }

        [XmlIgnore]
        [PropertyDescription("Parameters (Optional)")]
        [InputSpecification("Select the 'Generate Parameters' button once you have indicated a file, class, and method.")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.GenerateDLLParameters)]
        public DataTable v_MethodParameters { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Result Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _parametersGridViewHelper;

        public ExecuteDLLCommand()
        {
            CommandName = "ExecuteDLLCommand";
            SelectionName = "Execute DLL";
            CommandEnabled = true;
            CustomRendering = true;

            v_MethodParameters = new DataTable();
            v_MethodParameters.Columns.Add("Parameter Name");
            v_MethodParameters.Columns.Add("Parameter Value");
            v_MethodParameters.TableName = DateTime.Now.ToString("MethodParameterTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            _parametersGridViewHelper = new DataGridView();
            _parametersGridViewHelper.AllowUserToAddRows = true;
            _parametersGridViewHelper.AllowUserToDeleteRows = true;
            _parametersGridViewHelper.Size = new Size(350, 125);
            _parametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _parametersGridViewHelper.DataBindings.Add("DataSource", this, "v_MethodParameters", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //get file path
            var filePath = v_FilePath.ConvertToUserVariable(engine);
            var className = v_ClassName.ConvertToUserVariable(engine);
            var methodName = v_MethodName.ConvertToUserVariable(engine);

            //if file path does not exist
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("DLL was not found at " + filePath);
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
                    var requiredParameterValue = v_MethodParameters.AsEnumerable()
                                                                   .Where(rws => rws.Field<string>("Parameter Name") == paramName)
                                                                   .Select(rws => rws.Field<string>("Parameter Value"))
                                                                   .FirstOrDefault()
                                                                   .ConvertToUserVariable(engine);

                    dynamic parseResult;
                    //check namespace and convert
                    if (param.ParameterType.IsArray)
                    {
                        parseResult = requiredParameterValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        switch (param.ParameterType.FullName)
                        {
                            case "System.Int32":
                                parseResult = int.Parse(requiredParameterValue);
                                break;
                            case "System.Int64":
                                parseResult = long.Parse(requiredParameterValue);
                                break;
                            case "System.Double":
                                parseResult = double.Parse(requiredParameterValue);
                                break;
                            case "System.Boolean":
                                parseResult = bool.Parse(requiredParameterValue);
                                break;
                            case "System.Decimal":
                                parseResult = decimal.Parse(requiredParameterValue);
                                break;
                            case "System.Single":
                                parseResult = float.Parse(requiredParameterValue);
                                break;
                            case "System.Char":
                                parseResult = char.Parse(requiredParameterValue);
                                break;
                            case "System.String":
                                parseResult = requiredParameterValue;
                                break;
                            case "System.DateTime":
                                parseResult = DateTime.Parse(requiredParameterValue);
                                break;
                            default:
                                throw new NotImplementedException("Only system parameter types are supported!");
                        }
                    }
                    parameters.Add(parseResult);
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
            result.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ClassName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MethodName", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_MethodParameters", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_MethodParameters", this, new Control[] { _parametersGridViewHelper }, editor));
            RenderedControls.Add(_parametersGridViewHelper);

            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Call Method '{v_MethodName}' in '{v_ClassName}' - Store Result in '{v_OutputUserVariableName}']";
        }
    }
}
