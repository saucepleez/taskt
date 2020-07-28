using MSHTML;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Common;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("IE Browser Commands")]
    [Description("This command provides a number of functionalities or actions to perform Automation on Web Elements through the IE Browser.")]
    public class IEElementActionCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("IE Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("IEBrowser || {vIEBrowser}")]
        [Remarks("Failure to enter the correct instance or failure to first call **Create Browser** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlElement]
        [PropertyDescription("Element Search Parameters")]
        [InputSpecification("Select the element search parameters appropriately to target an element efficiently.")]
        [SampleUsage("")]
        [Remarks("")]
        public DataTable v_WebSearchParameter { get; set; }

        [XmlElement]
        [PropertyDescription("IE Element Action")]
        [PropertyUISelectionOption("Invoke Click")]
        [PropertyUISelectionOption("Left Click")]
        [PropertyUISelectionOption("Middle Click")]
        [PropertyUISelectionOption("Right Click")]
        [PropertyUISelectionOption("Get Text")]
        [PropertyUISelectionOption("Set Text")]
        [PropertyUISelectionOption("Get Attribute")]
        [PropertyUISelectionOption("Set Attribute")]
        [PropertyUISelectionOption("Fire onmousedown event")]
        [PropertyUISelectionOption("Fire onmouseover event")]
        [InputSpecification("Select the appropriate action to perform on an element once it has been located.")]
        [SampleUsage("")]
        [Remarks("Selecting this field changes the parameters that will be required in the next step.")]
        public string v_WebAction { get; set; }

        [XmlElement]
        [PropertyDescription("Action Parameters")]
        [InputSpecification("Enter the action parameter values based on the selection of an Element Action.")]
        [SampleUsage("{vParameterValue}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_WebActionParameterTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _elementsGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox _elementActionDropdown;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> _searchParameterControls;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _searchGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> _elementParameterControls;

        [XmlIgnore]
        [NonSerialized]
        private static IHTMLElementCollection _lastElementCollectionFound;

        [XmlIgnore]
        [NonSerialized]
        private static HTMLDocument _lastDocFound;

        public IEElementActionCommand()
        {
            CommandName = "IEElementActionCommand";
            SelectionName = "Element Action";
            CommandEnabled = true;
            v_InstanceName = "default";
            CustomRendering = true;

            v_WebSearchParameter = new DataTable();
            v_WebSearchParameter.TableName = DateTime.Now.ToString("WebSearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            v_WebSearchParameter.Columns.Add("Enabled", typeof(Boolean));
            v_WebSearchParameter.Columns.Add("Property Name");
            v_WebSearchParameter.Columns.Add("Property Value");

            v_WebActionParameterTable = new DataTable();
            v_WebActionParameterTable.TableName = DateTime.Now.ToString("WebActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            v_WebActionParameterTable.Columns.Add("Parameter Name");
            v_WebActionParameterTable.Columns.Add("Parameter Value");

            _searchGridViewHelper = new DataGridView();
            _searchGridViewHelper.AllowUserToAddRows = true;
            _searchGridViewHelper.AllowUserToDeleteRows = true;
            _searchGridViewHelper.Size = new Size(400, 250);
            _searchGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _searchGridViewHelper.DataBindings.Add("DataSource", this, "v_WebSearchParameter", false, DataSourceUpdateMode.OnPropertyChanged);

            _elementsGridViewHelper = new DataGridView();
            _elementsGridViewHelper.AllowUserToAddRows = true;
            _elementsGridViewHelper.AllowUserToDeleteRows = true;
            _elementsGridViewHelper.Size = new Size(400, 250);
            _elementsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _elementsGridViewHelper.DataBindings.Add("DataSource", this, "v_WebActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        [STAThread]
        public override void RunCommand(object sender)
        {
            object browserObject = null;

            var engine = (AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            browserObject = engine.GetAppInstance(vInstance);
            var browserInstance = (InternetExplorer)browserObject;

            DataTable searchTable = Common.Clone(v_WebSearchParameter);

            DataColumn matchFoundColumn = new DataColumn();
            matchFoundColumn.ColumnName = "Match Found";
            matchFoundColumn.DefaultValue = false;
            searchTable.Columns.Add(matchFoundColumn);

            var elementSearchProperties = from rws in searchTable.AsEnumerable()
                                          where rws.Field<Boolean>("Enabled").ToString() == "True"
                                          select rws;
            foreach (DataRow seachCriteria in elementSearchProperties)
            {
                string searchPropertyValue = seachCriteria.Field<string>("Property Value");
                searchPropertyValue = searchPropertyValue.ConvertToUserVariable(engine);
                seachCriteria.SetField<string>("Property Value", searchPropertyValue);
            }

            bool qualifyingElementFound = false;

            HTMLDocument doc = browserInstance.Document;

            if (doc == _lastDocFound)
            {
                qualifyingElementFound = InspectFrame(_lastElementCollectionFound, elementSearchProperties, sender, browserInstance);
            }
            if (!qualifyingElementFound)
            {
                qualifyingElementFound = InspectFrame(doc.all, elementSearchProperties, sender, browserInstance);
            }
            if (qualifyingElementFound)
            {
                _lastDocFound = doc;
            }

            if (!qualifyingElementFound)
            {
                throw new Exception("Could not find the element!");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            _searchParameterControls = new List<Control>();
            _searchParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_WebSearchParameter", this));
            _searchParameterControls.AddRange(CommandControls.CreateUIHelpersFor("v_WebSearchParameter", this, new Control[] { _searchGridViewHelper }, editor));

            _searchParameterControls.Add(_searchGridViewHelper);
            RenderedControls.AddRange(_searchParameterControls);

            _elementActionDropdown = (ComboBox)CommandControls.CreateDropdownFor("v_WebAction", this);
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WebAction", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_WebAction", this, new Control[] { _elementActionDropdown }, editor));
            _elementActionDropdown.SelectionChangeCommitted += ElementActionDropdown_SelectionChangeCommitted;
            RenderedControls.Add(_elementActionDropdown);

            _elementParameterControls = new List<Control>();
            _elementParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_WebActionParameterTable", this));
            _elementParameterControls.AddRange(CommandControls.CreateUIHelpersFor("v_WebActionParameterTable", this, new Control[] { _elementsGridViewHelper }, editor));
            _elementParameterControls.Add(_elementsGridViewHelper);
            RenderedControls.AddRange(_elementParameterControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            string parameters = string.Empty;
            //foreach (DataRow oRow in v_WebActionParameterTable.Rows)
            foreach (DataRow oRow in v_WebSearchParameter.Rows)
            {
                parameters += ", " + oRow["Property Name"] + "=" + oRow["Property Value"];
            }
            if (parameters.Length > 0) parameters = parameters.Substring(1);
            return base.GetDisplayValue() + $" [Perform Action '{v_WebAction} {parameters}' - Instance Name '{v_InstanceName}']";
        }

        private bool FindQualifyingElement(EnumerableRowCollection<DataRow> elementSearchProperties, IHTMLElement element)
        {
            foreach (DataRow seachCriteria in elementSearchProperties)
            {
                string searchPropertyName = seachCriteria.Field<string>("Property Name");
                string searchPropertyValue = seachCriteria.Field<string>("Property Value");
                string searchPropertyFound = seachCriteria.Field<string>("Match Found");

                string innerHTML = element.innerHTML;
                string outerHTML = element.outerHTML;

                searchPropertyFound = "False";

                try
                {
                    //if (element.GetType().GetProperty(searchPropertyName) == null)
                    if ((outerHTML == null) ||
                        (element.getAttribute(searchPropertyName) == null) ||
                        (Convert.IsDBNull(element.getAttribute(searchPropertyName))))
                    {
                        return false;
                    }

                    if (searchPropertyName.ToLower() == "href")
                    {
                        try
                        {
                            HTMLAnchorElement anchor = (HTMLAnchorElement)element;
                            if (anchor.href.Contains(searchPropertyValue))
                            {
                                seachCriteria.SetField<string>("Match Found", "True");
                            }
                            else
                            {
                                seachCriteria.SetField<string>("Match Found", "False");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        int searchValue;
                        if (int.TryParse(searchPropertyValue, out searchValue))
                        {
                            //int elementValue = (int)element.GetType().GetProperty(searchPropertyName).GetValue(element, null);
                            int elementValue = (int)element.getAttribute(searchPropertyName);
                            if (elementValue == searchValue)
                            {
                                seachCriteria.SetField<string>("Match Found", "True");
                            }
                            else
                            {
                                seachCriteria.SetField<string>("Match Found", "False");
                            }
                        }
                        else
                        {
                            //string elementValue = (string)element.GetType().GetProperty(searchPropertyName).GetValue(element, null);
                            string elementValue = (string)element.getAttribute(searchPropertyName);
                            if ((elementValue != null) && (elementValue == searchPropertyValue))
                            {
                                seachCriteria.SetField<string>("Match Found", "True");
                            }
                            else
                            {
                                seachCriteria.SetField<string>("Match Found", "False");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            /*foreach (var seachCriteria in elementSearchProperties)
            {
                Console.WriteLine(seachCriteria.Field<string>("Property Value"));
            }*/

            return elementSearchProperties.Where(seachCriteria => seachCriteria.Field<string>("Match Found") == "True").Count() == elementSearchProperties.Count();
        }

        private void ElementActionDropdown_SelectionChangeCommitted(object sender, EventArgs e)
        {

            IEElementActionCommand cmd = (IEElementActionCommand)this;
            DataTable actionParameters = cmd.v_WebActionParameterTable;

            if (sender != null)
            {
                actionParameters.Rows.Clear();
            }


            switch (_elementActionDropdown.SelectedItem)
            {
                case "Invoke Click":
                case "Fire onmousedown event":
                case "Fire onmouseover event":
                case "Clear Element":

                    foreach (var ctrl in _elementParameterControls)
                    {
                        ctrl.Hide();
                    }

                    break;

                case "Set Text":
                    foreach (var ctrl in _elementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Text To Set");
                    }

                    break;

                case "Get Text":
                case "Get Matching Elements":
                    foreach (var ctrl in _elementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Variable Name");
                    }
                    break;

                case "Get Attribute":
                    foreach (var ctrl in _elementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Attribute Name");
                        actionParameters.Rows.Add("Variable Name");
                    }
                    break;

                case "Set Attribute":
                    foreach (var ctrl in _elementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Attribute Name");
                        actionParameters.Rows.Add("Value To Set");
                    }

                    break;

                default:
                    break;
            }

            _elementsGridViewHelper.DataSource = v_WebActionParameterTable;
        }

        private void RunCommandActions(IHTMLElement element, object sender, InternetExplorer browserInstance)
        {
            var engine = (AutomationEngineInstance)sender;
            switch (v_WebAction)
            {
                case "Fire onmousedown event":
                    ((IHTMLElement3)element).FireEvent("onmousedown");
                    break;
                case "Fire onmouseover event":
                    ((IHTMLElement3)element).FireEvent("onmouseover");
                    break;
                case "Invoke Click":
                    element.click();
                    IECreateBrowserCommand.WaitForReadyState(browserInstance);
                    break;

                case "Left Click":
                case "Middle Click":
                case "Right Click":
                    int elementXposition = FindElementXPosition(element);
                    int elementYposition = FindElementYPosition(element);

                    //inputs need to be validated

                    int userXAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                       where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                       select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(engine));

                    int userYAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                       where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                       select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(engine));

                    var ieClientLocation = User32Functions.GetWindowPosition(new IntPtr(browserInstance.HWND));

                    SendMouseMoveCommand newMouseMove = new SendMouseMoveCommand();

                    newMouseMove.v_XMousePosition = ((elementXposition + ieClientLocation.left + 10) + userXAdjust).ToString(); // + 10 gives extra padding
                    newMouseMove.v_YMousePosition = ((elementYposition + ieClientLocation.top + 90 + SystemInformation.CaptionHeight) + userYAdjust).ToString(); // +90 accounts for title bar height
                    newMouseMove.v_MouseClick = v_WebAction;
                    newMouseMove.RunCommand(sender);
                    break;
                case "Set Attribute":
                    string setAttributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "Attribute Name"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string valueToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Value To Set"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    valueToSet = valueToSet.ConvertToUserVariable(engine);

                    element.setAttribute(setAttributeName, valueToSet);
                    break;
                case "Set Text":
                    string setTextAttributeName = "value";

                    string textToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Text To Set"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    textToSet = textToSet.ConvertToUserVariable(engine);

                    element.setAttribute(setTextAttributeName, textToSet);
                    break;
                case "Get Attribute":
                    string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Attribute Name"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string variableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Variable Name"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string convertedAttribute = Convert.ToString(element.getAttribute(attributeName));

                    convertedAttribute.StoreInUserVariable(engine, variableName);
                    break;
            }
        }

        private int FindElementXPosition(IHTMLElement obj)
        {
            int curleft = 0;
            if (obj.offsetParent != null)
            {
                while (obj.offsetParent != null)
                {
                    curleft += obj.offsetLeft;
                    obj = obj.offsetParent;
                }
            }

            return curleft;
        }

        private int FindElementYPosition(IHTMLElement obj)
        {
            int curtop = 0;
            if (obj.offsetParent != null)
            {
                while (obj.offsetParent != null)
                {
                    curtop += obj.offsetTop;
                    obj = obj.offsetParent;
                }
            }

            return curtop;
        }

        private Boolean InspectFrame(IHTMLElementCollection elementCollection, EnumerableRowCollection<DataRow> elementSearchProperties, object sender, SHDocVw.InternetExplorer browserInstance)
        {
            bool qualifyingElementFound = false;
            foreach (IHTMLElement element in elementCollection)
            {
                if (element.outerHTML != null)
                {
                    string outerHtml = element.outerHTML.ToLower().Trim();

                    if (!outerHtml.StartsWith("<html") &&
                        !outerHtml.StartsWith("<body") &&
                        !outerHtml.StartsWith("<head") &&
                        !outerHtml.StartsWith("<!doctype"))
                    {
                        qualifyingElementFound = FindQualifyingElement(elementSearchProperties, element);
                        if (qualifyingElementFound)
                        {
                            RunCommandActions(element, sender, browserInstance);
                            _lastElementCollectionFound = elementCollection;
                            return (true);
                            //break;
                        }
                        if (element.outerHTML != null && element.outerHTML.ToLower().Trim().StartsWith("<frame "))
                        {
                            string frameId = element.getAttribute("id");
                            if (frameId == null)
                            {
                                frameId = element.getAttribute("name");
                            }
                            if (frameId != null)
                            {
                                qualifyingElementFound = InspectFrame(browserInstance.Document.getElementById(frameId).contentDocument.all, elementSearchProperties, sender, browserInstance);
                            }
                        }
                        if (qualifyingElementFound)
                        {
                            break;
                        }
                    }
                }
            }
            return (qualifyingElementFound);
        }

    }

}