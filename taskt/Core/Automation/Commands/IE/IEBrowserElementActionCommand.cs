using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft.Json;
using OpenQA.Selenium;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using SHDocVw;
using System.Data;
using MSHTML;
using taskt.Core.Automation.User32;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to manipulate (get or set) elements within the HTML document of the associated IE web browser.  Features an assisting element capture form")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll and MSHTML.dll to achieve automation.")]
    public class IEBrowserElementActionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter or capture element search parameters")]
        public System.Data.DataTable v_WebSearchParameter { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("IE Element Action")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Invoke Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Set Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Attribute")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Set Attribute")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Fire onmousedown event")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Fire onmouseover event")]
        [Attributes.PropertyAttributes.InputSpecification("Select the appropriate corresponding action to take once the element has been located")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Invoke Click**, **Set Text**, **Get Text**, **Get Attribute**")]
        [Attributes.PropertyAttributes.Remarks("Selecting this field changes the parameters that will be required in the next step")]
        public string v_WebAction { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Action Parameters")]
        public System.Data.DataTable v_WebActionParameterTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView ElementsGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox ElementActionDropdown;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> SearchParameterControls;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView SearchGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> ElementParameterControls;

        [XmlIgnore]
        [NonSerialized]
        private static IHTMLElementCollection lastElementCollectionFound;

        [XmlIgnore]
        [NonSerialized]
        private static HTMLDocument lastDocFound;

        public IEBrowserElementActionCommand()
        {
            this.CommandName = "IEBrowserElementActionCommand";
            this.SelectionName = "Element Action";
            this.CommandEnabled = true;
            this.v_InstanceName = "";
            this.CustomRendering = true;

            this.v_WebSearchParameter = new System.Data.DataTable();
            this.v_WebSearchParameter.TableName = DateTime.Now.ToString("WebSearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            this.v_WebSearchParameter.Columns.Add("Enabled", typeof(Boolean));
            this.v_WebSearchParameter.Columns.Add("Property Name");
            this.v_WebSearchParameter.Columns.Add("Property Value");

            this.v_WebActionParameterTable = new System.Data.DataTable();
            this.v_WebActionParameterTable.TableName = DateTime.Now.ToString("WebActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            this.v_WebActionParameterTable.Columns.Add("Parameter Name");
            this.v_WebActionParameterTable.Columns.Add("Parameter Value");
        }

        private Boolean InspectFrame(IHTMLElementCollection elementCollection, EnumerableRowCollection<DataRow> elementSearchProperties, object sender, SHDocVw.InternetExplorer browserInstance)
        {
            bool qualifyingElementFound = false;
            foreach (IHTMLElement element in elementCollection) // browserInstance.Document.All)
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
                            lastElementCollectionFound = elementCollection;
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

        [STAThread]
        public override void RunCommand(object sender)
        {
            object browserObject = null;

            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            browserObject = engine.GetAppInstance(vInstance);

            var browserInstance = (SHDocVw.InternetExplorer)browserObject;

            DataTable searchTable = Core.Common.Clone<DataTable>(v_WebSearchParameter);

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

            if (doc == lastDocFound)
            {
                qualifyingElementFound = InspectFrame(lastElementCollectionFound, elementSearchProperties, sender, browserInstance);
            }
            if (!qualifyingElementFound)
            {
                qualifyingElementFound = InspectFrame(doc.all, elementSearchProperties, sender, browserInstance);
            }
            if (qualifyingElementFound)
            {
                lastDocFound = doc;
            }

            if (!qualifyingElementFound)
            {
                throw new Exception("Could not find the element!");
            }
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
                    if ((outerHTML == null) || (element.getAttribute(searchPropertyName) == null) || (System.Convert.IsDBNull(element.getAttribute(searchPropertyName))))
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
                        catch (Exception ex) { }
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
                catch (Exception ex) { }
            }

            /*foreach (var seachCriteria in elementSearchProperties)
            {
                Console.WriteLine(seachCriteria.Field<string>("Property Value"));
            }*/

            return elementSearchProperties.Where(seachCriteria => seachCriteria.Field<string>("Match Found") == "True").Count() == elementSearchProperties.Count();
        }

        private void RunCommandActions(IHTMLElement element, object sender, InternetExplorer browserInstance)
        {
            if (v_WebAction == "Fire onmousedown event")
            {
                ((IHTMLElement3)element).FireEvent("onmousedown");
            }
            else if (v_WebAction == "Fire onmouseover event")
            {
                ((IHTMLElement3)element).FireEvent("onmouseover");
            }
            else if (v_WebAction == "Invoke Click")
            {
                element.click();
                IEBrowserCreateCommand.WaitForReadyState(browserInstance);
            }
            else if ((v_WebAction == "Left Click") || (v_WebAction == "Middle Click") || (v_WebAction == "Right Click"))
            {
                int elementXposition = FindElementXPosition(element);
                int elementYposition = FindElementYPosition(element);

                //inputs need to be validated

                int userXAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                   where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());

                int userYAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                   where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());

                var ieClientLocation = User32Functions.GetWindowPosition(new IntPtr(browserInstance.HWND));

                SendMouseMoveCommand newMouseMove = new SendMouseMoveCommand();

                newMouseMove.v_XMousePosition = ((elementXposition + ieClientLocation.left + 10) + userXAdjust).ToString(); // + 10 gives extra padding
                newMouseMove.v_YMousePosition = ((elementYposition + ieClientLocation.top + 90 + System.Windows.Forms.SystemInformation.CaptionHeight) + userYAdjust).ToString(); // +90 accounts for title bar height
                newMouseMove.v_MouseClick = v_WebAction;
                newMouseMove.RunCommand(sender);
            }
            else if (v_WebAction == "Set Attribute")
            {
                string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Attribute Name"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault();

                string valueToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                     where rw.Field<string>("Parameter Name") == "Value To Set"
                                     select rw.Field<string>("Parameter Value")).FirstOrDefault();

                valueToSet = valueToSet.ConvertToUserVariable(sender);

                element.setAttribute(attributeName, valueToSet);
            }
            else if (v_WebAction == "Set Text")
            {
                string attributeName = "value";

                string textToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                    where rw.Field<string>("Parameter Name") == "Text To Set"
                                    select rw.Field<string>("Parameter Value")).FirstOrDefault();

                textToSet = textToSet.ConvertToUserVariable(sender);

                element.setAttribute(attributeName, textToSet);
            }
            else if (v_WebAction == "Get Attribute")
            {
                string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Attribute Name"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault();

                string variableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Variable Name"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault();

                string convertedAttribute = Convert.ToString(element.getAttribute(attributeName));

                convertedAttribute.StoreInUserVariable(sender, variableName);
            }
        }

        private int FindElementXPosition(MSHTML.IHTMLElement obj)
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

        public int FindElementYPosition(MSHTML.IHTMLElement obj)
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

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //SearchGridViewHelper = new DataGridView();
            //SearchGridViewHelper.AllowUserToAddRows = true;
            //SearchGridViewHelper.AllowUserToDeleteRows = true;
            //SearchGridViewHelper.Size = new Size(400, 250);
            //SearchGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //SearchGridViewHelper.DataBindings.Add("DataSource", this, "v_WebSearchParameter", false, DataSourceUpdateMode.OnPropertyChanged);
            SearchGridViewHelper = CommandControls.CreateDataGridView(this, "v_WebSearchParameter", true, true, false, 400, 250);
            SearchGridViewHelper.CellClick += SearchGridViewHelper_CellClick;

            //ElementsGridViewHelper = new DataGridView();
            //ElementsGridViewHelper.AllowUserToAddRows = true;
            //ElementsGridViewHelper.AllowUserToDeleteRows = true;
            //ElementsGridViewHelper.Size = new Size(400, 250);
            //ElementsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //ElementsGridViewHelper.DataBindings.Add("DataSource", this, "v_WebActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
            ElementsGridViewHelper = CommandControls.CreateDataGridView(this, "v_WebActionParameterTable", false , false, false, 400, 150);
            ElementsGridViewHelper.CellClick += ElementsGridViewHelper_CellClick;
            ElementsGridViewHelper.CellBeginEdit += ElementsGridViewHelper_CellBeginEdit;

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            SearchParameterControls = new List<Control>();
            SearchParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_WebSearchParameter", this));
            SearchParameterControls.AddRange(CommandControls.CreateUIHelpersFor("v_WebSearchParameter", this, new Control[] { SearchGridViewHelper }, editor));

            SearchParameterControls.Add(SearchGridViewHelper);
            RenderedControls.AddRange(SearchParameterControls);

            ElementActionDropdown = (ComboBox)CommandControls.CreateDropdownFor("v_WebAction", this);
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_WebAction", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_WebAction", this, new Control[] { ElementActionDropdown }, editor));
            ElementActionDropdown.SelectionChangeCommitted += ElementActionDropdown_SelectionChangeCommitted;
            RenderedControls.Add(ElementActionDropdown);

            ElementParameterControls = new List<Control>();
            ElementParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_WebActionParameterTable", this));
            ElementParameterControls.AddRange(CommandControls.CreateUIHelpersFor("v_WebActionParameterTable", this, new Control[] { ElementsGridViewHelper }, editor));
            ElementParameterControls.Add(ElementsGridViewHelper);
            RenderedControls.AddRange(ElementParameterControls);

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultBrowserInstanceName;
            }

            return RenderedControls;
        }

        private void ElementActionDropdown_SelectionChangeCommitted(object sender, EventArgs e)
        {

            Core.Automation.Commands.IEBrowserElementActionCommand cmd = (Core.Automation.Commands.IEBrowserElementActionCommand)this;
            DataTable actionParameters = cmd.v_WebActionParameterTable;
            DataGridViewComboBoxCell comparisonComboBox;

            if (sender != null)
            {
                actionParameters.Rows.Clear();
            }


            switch (ElementActionDropdown.SelectedItem)
            {
                case "Invoke Click":
                case "Fire onmousedown event":
                case "Fire onmouseover event":
                case "Clear Element":

                    foreach (var ctrl in ElementParameterControls)
                    {
                        ctrl.Hide();
                    }

                    break;

                case "Set Text":
                    foreach (var ctrl in ElementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Text To Set");
                        //actionParameters.Rows.Add("Clear Element Before Setting Text");
                    }

                    //comparisonComboBox = new DataGridViewComboBoxCell();
                    //comparisonComboBox.Items.Add("Yes");
                    //comparisonComboBox.Items.Add("No");

                    //assign cell as a combobox
                    //if (sender != null)
                    //{
                    //    ElementsGridViewHelper.Rows[1].Cells[1].Value = "No";
                    //}
                    //ElementsGridViewHelper.Rows[1].Cells[1] = comparisonComboBox;


                    break;

                case "Get Text":
                case "Get Matching Elements":
                    foreach (var ctrl in ElementParameterControls)
                    {
                        ctrl.Show();
                    }
                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Variable Name");
                    }
                    break;

                case "Get Attribute":
                    foreach (var ctrl in ElementParameterControls)
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
                    foreach (var ctrl in ElementParameterControls)
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

            ElementsGridViewHelper.DataSource = v_WebActionParameterTable;
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
            return base.GetDisplayValue() + " [ Action: '" + v_WebAction + " " + parameters + "', Instance Name: '" + v_InstanceName + "']";
        }


        private void SearchGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                if (e.ColumnIndex != 0)
                {
                    SearchGridViewHelper.BeginEdit(false);
                }
            }
            else
            {
                SearchGridViewHelper.EndEdit();
            }
        }


        private void ElementsGridViewHelper_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Cancel = true;
            }
        }

        private void ElementsGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                ElementsGridViewHelper.BeginEdit(false);
            }
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            if (SearchGridViewHelper.IsCurrentCellDirty || SearchGridViewHelper.IsCurrentRowDirty)
            {
                SearchGridViewHelper.CommitEdit(DataGridViewDataErrorContexts.Commit);
                var newRow = v_WebSearchParameter.NewRow();
                v_WebSearchParameter.Rows.Add(newRow);
                for (var i = v_WebSearchParameter.Rows.Count - 1; i >= 0; i--)
                {
                    if (v_WebSearchParameter.Rows[i][1].ToString() == "" && v_WebSearchParameter.Rows[i][2].ToString() == "")
                    {
                        v_WebSearchParameter.Rows[i].Delete();
                    }
                }
            }
        }

    }

}