using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Automation;
using System.Reflection;
using taskt.Core.Automation.User32;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Drawing;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Combined implementation of the ThickAppClick/GetText command but includes an advanced Window Recorder to record the required element.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows UI Automation' to find elements and invokes a Variable Command to assign data and achieve automation")]
    public class UIAutomationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the action")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Click Element")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Value From Element")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Check If Element Exists")]
        public string v_AutomationType { get; set; }

        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Window to Automate")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WindowName { get; set; }

        [Attributes.PropertyAttributes.PropertyDescription("Set Search Parameters")]
        [Attributes.PropertyAttributes.InputSpecification("Use the Element Recorder to generate a listing of potential search parameters.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("Once you have clicked on a valid window the search parameters will be populated.  Enable only the ones required to be a match at runtime.")]
        public DataTable v_UIASearchParameters { get; set; }

        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Set Action Parameters")]
        [Attributes.PropertyAttributes.InputSpecification("Define the parameters for the actions.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("Parameters change depending on the Automation Type selected.")]
        public DataTable v_UIAActionParameters { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox AutomationTypeControl;


        [XmlIgnore]
        [NonSerialized]
        private ComboBox WindowNameControl;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView SearchParametersGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView ActionParametersGridViewHelper;

        public UIAutomationCommand()
        {
            this.CommandName = "UIAutomationCommand";
            this.SelectionName = "UI Automation";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //set up search parameter table
            this.v_UIASearchParameters = new DataTable();
            this.v_UIASearchParameters.Columns.Add("Enabled");
            this.v_UIASearchParameters.Columns.Add("Parameter Name");
            this.v_UIASearchParameters.Columns.Add("Parameter Value");
            this.v_UIASearchParameters.TableName = DateTime.Now.ToString("UIASearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            this.v_UIAActionParameters = new DataTable();
            this.v_UIAActionParameters.Columns.Add("Parameter Name");
            this.v_UIAActionParameters.Columns.Add("Parameter Value");
            this.v_UIAActionParameters.TableName = DateTime.Now.ToString("UIAActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

        }

        private void ActionParametersGridViewHelper_MouseEnter(object sender, EventArgs e)
        {
            this.UIAType_SelectionChangeCommitted(null, null);
        }

        public PropertyCondition CreatePropertyCondition(string propertyName, object propertyValue)
        {
            string propName = propertyName + "Property";

            switch (propertyName)
            {
                case "AcceleratorKey":
                    return new PropertyCondition(AutomationElement.AcceleratorKeyProperty, propertyValue);
                case "AccessKey":
                    return new PropertyCondition(AutomationElement.AccessKeyProperty, propertyValue);
                case "AutomationId":
                    return new PropertyCondition(AutomationElement.AutomationIdProperty, propertyValue);
                case "ClassName":
                    return new PropertyCondition(AutomationElement.ClassNameProperty, propertyValue);
                case "FrameworkId":
                    return new PropertyCondition(AutomationElement.FrameworkIdProperty, propertyValue);
                case "HasKeyboardFocus":
                    return new PropertyCondition(AutomationElement.HasKeyboardFocusProperty, propertyValue);
                case "HelpText":
                    return new PropertyCondition(AutomationElement.HelpTextProperty, propertyValue);
                case "IsContentElement":
                    return new PropertyCondition(AutomationElement.IsContentElementProperty, propertyValue);
                case "IsControlElement":
                    return new PropertyCondition(AutomationElement.IsControlElementProperty, propertyValue);
                case "IsEnabled":
                    return new PropertyCondition(AutomationElement.IsEnabledProperty, propertyValue);
                case "IsKeyboardFocusable":
                    return new PropertyCondition(AutomationElement.IsKeyboardFocusableProperty, propertyValue);
                case "IsOffscreen":
                    return new PropertyCondition(AutomationElement.IsOffscreenProperty, propertyValue);
                case "IsPassword":
                    return new PropertyCondition(AutomationElement.IsPasswordProperty, propertyValue);
                case "IsRequiredForForm":
                    return new PropertyCondition(AutomationElement.IsRequiredForFormProperty, propertyValue);
                case "ItemStatus":
                    return new PropertyCondition(AutomationElement.ItemStatusProperty, propertyValue);
                case "ItemType":
                    return new PropertyCondition(AutomationElement.ItemTypeProperty, propertyValue);
                case "LocalizedControlType":
                    return new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, propertyValue);
                case "Name":
                    return new PropertyCondition(AutomationElement.NameProperty, propertyValue);
                case "NativeWindowHandle":
                    return new PropertyCondition(AutomationElement.NativeWindowHandleProperty, propertyValue);
                case "ProcessID":
                    return new PropertyCondition(AutomationElement.ProcessIdProperty, propertyValue);
                default:
                    throw new NotImplementedException("Property Type '" + propertyName + "' not implemented");
            }





        }

        public AutomationElement SearchForGUIElement(object sender, string variableWindowName)
        {

            //create search params
            var searchParams = from rw in v_UIASearchParameters.AsEnumerable()
                               where rw.Field<string>("Enabled") == "True"
                               select rw;

            //create and populate condition list
            var conditionList = new List<Condition>();
            foreach (var param in searchParams)
            {
                var parameterName = (string)param["Parameter Name"];
                var parameterValue = (string)param["Parameter Value"];

                parameterName = parameterName.ConvertToUserVariable(sender);
                parameterValue = parameterValue.ConvertToUserVariable(sender);

                PropertyCondition propCondition;
                if (bool.TryParse(parameterValue, out bool bValue))
                {
                    propCondition = CreatePropertyCondition(parameterName, bValue);
                }
                else
                {
                    propCondition = CreatePropertyCondition(parameterName, parameterValue);
                }



               
                conditionList.Add(propCondition);
            }

            //concatenate or take first condition
            Condition searchConditions;
            if (conditionList.Count > 1)
            {
                searchConditions = new AndCondition(conditionList.ToArray());

            }
            else
            {
                searchConditions = conditionList[0];
            }

            //find window
            var windowElement = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, variableWindowName));

            //if window was not found
            if (windowElement == null)
                throw new Exception("Window named '" + variableWindowName + "' was not found!");

            //find required handle based on specified conditions
            var element = windowElement.FindFirst(TreeScope.Descendants, searchConditions);
            return element;

        }
        public override void RunCommand(object sender)
        {

            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //create variable window name
            var variableWindowName = v_WindowName.ConvertToUserVariable(sender);

            if (variableWindowName == "Current Window")
            {
                variableWindowName = User32Functions.GetActiveWindowTitle();
            }

            var requiredHandle =  SearchForGUIElement(sender, variableWindowName);


            //if element exists type
            if (v_AutomationType == "Check If Element Exists")
            {
                //apply to variable
                var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Apply To Variable"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault();

                
                //remove brackets from variable
                applyToVariable = applyToVariable.Replace(engine.engineSettings.VariableStartMarker, "").Replace(engine.engineSettings.VariableEndMarker, "");

                //declare search result
                string searchResult;

                //determine search result
                if (requiredHandle == null)
                {
                    searchResult = "FALSE";
  
                }
                else
                {
                    searchResult = "TRUE";
                }

              //store data
                searchResult.StoreInUserVariable(sender, applyToVariable);

            }

            //determine element click type
           else if (v_AutomationType == "Click Element")
            {

                //if handle was not found
                if (requiredHandle == null)
                    throw new Exception("Element was not found in window '" + variableWindowName + "'");

                //create search params
                var clickType = (from rw in v_UIAActionParameters.AsEnumerable()
                                 where rw.Field<string>("Parameter Name") == "Click Type"
                                 select rw.Field<string>("Parameter Value")).FirstOrDefault();

                //get x adjust
                var xAdjust = (from rw in v_UIAActionParameters.AsEnumerable()
                               where rw.Field<string>("Parameter Name") == "X Adjustment"
                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

                //get y adjust
                var yAdjust = (from rw in v_UIAActionParameters.AsEnumerable()
                               where rw.Field<string>("Parameter Name") == "Y Adjustment"
                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

                //convert potential variable
                var xAdjustVariable = xAdjust.ConvertToUserVariable(sender);
                var yAdjustVariable = yAdjust.ConvertToUserVariable(sender);

                //parse to int
                var xAdjustInt = int.Parse(xAdjustVariable);
                var yAdjustInt = int.Parse(yAdjustVariable);

                //get clickable point
                var newPoint = requiredHandle.GetClickablePoint();

                //send mousemove command
                var newMouseMove = new SendMouseMoveCommand
                {
                    v_XMousePosition = (newPoint.X + xAdjustInt).ToString(),
                    v_YMousePosition = (newPoint.Y + yAdjustInt).ToString(),
                    v_MouseClick = clickType
                };

                //run commands
                newMouseMove.RunCommand(sender);
            }
            else if (v_AutomationType == "Get Value From Element")
            {

                //if handle was not found
                if (requiredHandle == null)
                    throw new Exception("Element was not found in window '" + variableWindowName + "'");
                //get value from property
                var propertyName = (from rw in v_UIAActionParameters.AsEnumerable()
                                    where rw.Field<string>("Parameter Name") == "Get Value From"
                                    select rw.Field<string>("Parameter Value")).FirstOrDefault();
               
                //apply to variable
                var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Apply To Variable"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault();

                //remove brackets from variable
                applyToVariable = applyToVariable.Replace(engine.engineSettings.VariableStartMarker, "").Replace(engine.engineSettings.VariableEndMarker, "");

                //get required value
                var requiredValue = requiredHandle.Current.GetType().GetRuntimeProperty(propertyName)?.GetValue(requiredHandle.Current).ToString();

                //store into variable
                requiredValue.StoreInUserVariable(sender, applyToVariable);

            }
            else
            {
                throw new NotImplementedException("Automation type '" + v_AutomationType + "' not supported.");
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create search param grid
            SearchParametersGridViewHelper = new DataGridView();
            SearchParametersGridViewHelper.Width = 500;
            SearchParametersGridViewHelper.Height = 140;
            SearchParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_UIASearchParameters", false, DataSourceUpdateMode.OnPropertyChanged);

            DataGridViewCheckBoxColumn enabled = new DataGridViewCheckBoxColumn();
            enabled.HeaderText = "Enabled";
            enabled.DataPropertyName = "Enabled";
            SearchParametersGridViewHelper.Columns.Add(enabled);

            DataGridViewTextBoxColumn propertyName = new DataGridViewTextBoxColumn();
            propertyName.HeaderText = "Parameter Name";
            propertyName.DataPropertyName = "Parameter Name";
            SearchParametersGridViewHelper.Columns.Add(propertyName);

            DataGridViewTextBoxColumn propertyValue = new DataGridViewTextBoxColumn();
            propertyValue.HeaderText = "Parameter Value";
            propertyValue.DataPropertyName = "Parameter Value";
            SearchParametersGridViewHelper.Columns.Add(propertyValue);

            SearchParametersGridViewHelper.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            SearchParametersGridViewHelper.AllowUserToAddRows = false;
            SearchParametersGridViewHelper.AllowUserToDeleteRows = false;

            //create actions
            ActionParametersGridViewHelper = new DataGridView();
            ActionParametersGridViewHelper.Width = 500;
            ActionParametersGridViewHelper.Height = 140;
            ActionParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_UIAActionParameters", false, DataSourceUpdateMode.OnPropertyChanged);
            ActionParametersGridViewHelper.MouseEnter += ActionParametersGridViewHelper_MouseEnter;

            propertyName = new DataGridViewTextBoxColumn();
            propertyName.HeaderText = "Parameter Name";
            propertyName.DataPropertyName = "Parameter Name";
            ActionParametersGridViewHelper.Columns.Add(propertyName);

            propertyValue = new DataGridViewTextBoxColumn();
            propertyValue.HeaderText = "Parameter Value";
            propertyValue.DataPropertyName = "Parameter Value";
            ActionParametersGridViewHelper.Columns.Add(propertyValue);

            ActionParametersGridViewHelper.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            ActionParametersGridViewHelper.AllowUserToAddRows = false;
            ActionParametersGridViewHelper.AllowUserToDeleteRows = false;




            //create helper control
            CommandItemControl helperControl = new CommandItemControl();
            helperControl.Padding = new Padding(10, 0, 0, 0);
            helperControl.ForeColor = Color.AliceBlue;
            helperControl.Font = new Font("Segoe UI Semilight", 10);         
            helperControl.CommandImage = UI.Images.GetUIImage("ClipboardGetTextCommand");
            helperControl.CommandDisplay = "Element Recorder";
            helperControl.Click += ShowRecorder;



            //automation type
            var automationTypeGroup = CommandControls.CreateDefaultDropdownGroupFor("v_AutomationType", this, editor);
            AutomationTypeControl = (ComboBox)automationTypeGroup.Where(f => f is ComboBox).FirstOrDefault();
            AutomationTypeControl.SelectionChangeCommitted += UIAType_SelectionChangeCommitted;
            RenderedControls.AddRange(automationTypeGroup);



            //window name
            RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_WindowName", this));
            WindowNameControl = UI.CustomControls.CommandControls.CreateStandardComboboxFor("v_WindowName", this).AddWindowNames();
            RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_WindowName", this, new Control[] { WindowNameControl }, editor));
            RenderedControls.Add(WindowNameControl);

            //create search parameters   
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_UIASearchParameters", this));
            RenderedControls.Add(helperControl);
            RenderedControls.Add(SearchParametersGridViewHelper);

            //create action parameters
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_UIAActionParameters", this));
            RenderedControls.Add(ActionParametersGridViewHelper);

            return RenderedControls;

        }
        public void ShowRecorder(object sender, EventArgs e)
        {
            //get command reference
            //create recorder
            UI.Forms.Supplemental.frmThickAppElementRecorder newElementRecorder = new UI.Forms.Supplemental.frmThickAppElementRecorder();
            newElementRecorder.searchParameters = this.v_UIASearchParameters;

            //show form
            newElementRecorder.ShowDialog();

            WindowNameControl.Text = newElementRecorder.cboWindowTitle.Text;


            this.v_UIASearchParameters.Rows.Clear();
            foreach (DataRow rw in newElementRecorder.searchParameters.Rows)
            {
                this.v_UIASearchParameters.ImportRow(rw);
            }

           
            SearchParametersGridViewHelper.DataSource = this.v_UIASearchParameters;
            SearchParametersGridViewHelper.Refresh();

        }

        public void UIAType_SelectionChangeCommitted(object sender, EventArgs e)
        {
  
            ComboBox selectedAction = AutomationTypeControl;

            if (selectedAction == null)
                return;

            DataGridView actionParameterView = ActionParametersGridViewHelper;
            actionParameterView.Refresh();

            DataTable actionParameters = this.v_UIAActionParameters;

            if (sender != null)
            {
                actionParameters.Rows.Clear();
            }

  
            switch (selectedAction.SelectedItem)
            {
                case "Click Element":
                    var mouseClickBox = new DataGridViewComboBoxCell();
                    mouseClickBox.Items.Add("Left Click");
                    mouseClickBox.Items.Add("Middle Click");
                    mouseClickBox.Items.Add("Right Click");
                    mouseClickBox.Items.Add("Left Down");
                    mouseClickBox.Items.Add("Middle Down");
                    mouseClickBox.Items.Add("Right Down");
                    mouseClickBox.Items.Add("Left Up");
                    mouseClickBox.Items.Add("Middle Up");
                    mouseClickBox.Items.Add("Right Up");


                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Click Type", "");
                        actionParameters.Rows.Add("X Adjustment", 0);
                        actionParameters.Rows.Add("Y Adjustment", 0);
                    }

                    actionParameterView.Rows[0].Cells[1] = mouseClickBox;
                    break;

                case "Check If Element Exists":

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Apply To Variable", "");
                    }
                    break;
                default:
                    var parameterName = new DataGridViewComboBoxCell();
                    parameterName.Items.Add("AcceleratorKey");
                    parameterName.Items.Add("AccessKey");
                    parameterName.Items.Add("AutomationId");
                    parameterName.Items.Add("ClassName");
                    parameterName.Items.Add("FrameworkId");
                    parameterName.Items.Add("HasKeyboardFocus");
                    parameterName.Items.Add("HelpText");
                    parameterName.Items.Add("IsContentElement");
                    parameterName.Items.Add("IsControlElement");
                    parameterName.Items.Add("IsEnabled");
                    parameterName.Items.Add("IsKeyboardFocusable");
                    parameterName.Items.Add("IsOffscreen");
                    parameterName.Items.Add("IsPassword");
                    parameterName.Items.Add("IsRequiredForForm");
                    parameterName.Items.Add("ItemStatus");
                    parameterName.Items.Add("ItemType");
                    parameterName.Items.Add("LocalizedControlType");
                    parameterName.Items.Add("Name");
                    parameterName.Items.Add("NativeWindowHandle");
                    parameterName.Items.Add("ProcessID");

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Get Value From", "");
                        actionParameters.Rows.Add("Apply To Variable", "");
                        actionParameterView.Refresh();
                        try
                        {
                            actionParameterView.Rows[0].Cells[1] = parameterName;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to select first row, second cell to apply '" + parameterName + "': " + ex.ToString());
                        }

                    }

                    break;
            }


            actionParameterView.Refresh();

        }

        public override string GetDisplayValue()
        {
            if (v_AutomationType == "Click Element")
            {
                //create search params
                var clickType = (from rw in v_UIAActionParameters.AsEnumerable()
                                 where rw.Field<string>("Parameter Name") == "Click Type"
                                 select rw.Field<string>("Parameter Value")).FirstOrDefault();


                return base.GetDisplayValue() + " [" + clickType + " element in window '" + v_WindowName + "']";
            }
            else if(v_AutomationType == "Check If Element Exists")
            {

                //apply to variable
                var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Apply To Variable"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault();

                return base.GetDisplayValue() + " [Check for element in window '" + v_WindowName + "' and apply to '" + applyToVariable + "']";
            }
            else
            {
                //get value from property
                var propertyName = (from rw in v_UIAActionParameters.AsEnumerable()
                                    where rw.Field<string>("Parameter Name") == "Get Value From"
                                    select rw.Field<string>("Parameter Value")).FirstOrDefault();

                //apply to variable
                var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Apply To Variable"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault();

                return base.GetDisplayValue() + " [Get value from '" + propertyName + "' in window '" + v_WindowName + "' and apply to '" + applyToVariable + "']";
            }



        }
    }
}