//Copyright (c) 2018 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using SHDocVw;
using System.Data;
using MSHTML;
using System.Windows.Automation;
using System.Net.Mail;
using taskt.Core;
using System.Net;
using System.IO;
using System.Drawing;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using System.Reflection;

namespace taskt.Core.AutomationCommands
{
    #region Base Command
    [Serializable]
    [XmlInclude(typeof(SendKeysCommand))]
    [XmlInclude(typeof(SendMouseMoveCommand))]
    [XmlInclude(typeof(PauseCommand))]
    [XmlInclude(typeof(ActivateWindowCommand))]
    [XmlInclude(typeof(MoveWindowCommand))]
    [XmlInclude(typeof(CommentCommand))]
    [XmlInclude(typeof(ThickAppClickItemCommand))]
    [XmlInclude(typeof(ThickAppGetTextCommand))]
    [XmlInclude(typeof(UIAutomationCommand))]
    [XmlInclude(typeof(ResizeWindowCommand))]
    [XmlInclude(typeof(WaitForWindowCommand))]
    [XmlInclude(typeof(MessageBoxCommand))]
    [XmlInclude(typeof(StopProcessCommand))]
    [XmlInclude(typeof(StartProcessCommand))]
    [XmlInclude(typeof(VariableCommand))]
    [XmlInclude(typeof(RunScriptCommand))]
    [XmlInclude(typeof(CloseWindowCommand))]
    [XmlInclude(typeof(IEBrowserCreateCommand))]
    [XmlInclude(typeof(IEBrowserNavigateCommand))]
    [XmlInclude(typeof(IEBrowserCloseCommand))]
    [XmlInclude(typeof(IEBrowserElementCommand))]
    [XmlInclude(typeof(IEBrowserFindBrowserCommand))]
    [XmlInclude(typeof(SetWindowStateCommand))]
    [XmlInclude(typeof(BeginExcelDatasetLoopCommand))]
    [XmlInclude(typeof(ExitLoopCommand))]
    [XmlInclude(typeof(EndLoopCommand))]
    [XmlInclude(typeof(ClipboardGetTextCommand))]
    [XmlInclude(typeof(ScreenshotCommand))]
    [XmlInclude(typeof(ExcelOpenWorkbookCommand))]
    [XmlInclude(typeof(ExcelCreateApplicationCommand))]
    [XmlInclude(typeof(ExcelAddWorkbookCommand))]
    [XmlInclude(typeof(ExcelGoToCellCommand))]
    [XmlInclude(typeof(ExcelSetCellCommand))]
    [XmlInclude(typeof(ExcelCloseApplicationCommand))]
    [XmlInclude(typeof(ExcelGetCellCommand))]
    [XmlInclude(typeof(ExcelRunMacroCommand))]
    [XmlInclude(typeof(ExcelActivateSheetCommand))]
    [XmlInclude(typeof(ExcelDeleteRowCommand))]
    [XmlInclude(typeof(ExcelDeleteCellCommand))]
    [XmlInclude(typeof(ExcelGetLastRowCommand))]
    [XmlInclude(typeof(SeleniumBrowserCreateCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateURLCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateForwardCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateBackCommand))]
    [XmlInclude(typeof(SeleniumBrowserRefreshCommand))]
    [XmlInclude(typeof(SeleniumBrowserCloseCommand))]
    [XmlInclude(typeof(SeleniumBrowserElementActionCommand))]
    [XmlInclude(typeof(SMTPSendEmailCommand))]
    [XmlInclude(typeof(ErrorHandlingCommand))]
    [XmlInclude(typeof(StringSubstringCommand))]
    [XmlInclude(typeof(StringSplitCommand))]
    [XmlInclude(typeof(BeginIfCommand))]
    [XmlInclude(typeof(EndIfCommand))]
    [XmlInclude(typeof(ElseCommand))]
    [XmlInclude(typeof(OCRCommand))]
    [XmlInclude(typeof(HTTPRequestCommand))]
    [XmlInclude(typeof(HTTPQueryResultCommand))]
    [XmlInclude(typeof(ImageRecognitionCommand))]
    [XmlInclude(typeof(SendMouseClickCommand))]
    [XmlInclude(typeof(ExcelCreateDataSetCommand))]
    [XmlInclude(typeof(DatabaseRunQueryCommand))]
    [XmlInclude(typeof(BeginNumberOfTimesLoopCommand))]
    [XmlInclude(typeof(BeginListLoopCommand))]
    [XmlInclude(typeof(BeginContinousLoopCommand))]
    [XmlInclude(typeof(SequenceCommand))]
    [XmlInclude(typeof(StopTaskCommand))]
    [XmlInclude(typeof(RunTaskCommand))]
    [XmlInclude(typeof(WriteTextFileCommand))]
    [XmlInclude(typeof(ReadTextFileCommand))]
    [XmlInclude(typeof(MoveFileCommand))]
    [XmlInclude(typeof(DeleteFileCommand))]
    [XmlInclude(typeof(RenameFileCommand))]
    [XmlInclude(typeof(WaitForFileToExistCommand))]
    [XmlInclude(typeof(RunCustomCodeCommand))]
    [XmlInclude(typeof(DateCalculationCommand))]
    [XmlInclude(typeof(RegExExtractorCommand))]
    [XmlInclude(typeof(TextExtractorCommand))]
    [XmlInclude(typeof(FormatDataCommand))]
    [XmlInclude(typeof(LogDataCommand))]
    public abstract class ScriptCommand
    {
        [XmlAttribute]
        public string CommandName { get; set; }
        [XmlAttribute]
        public bool IsCommented { get; set; }
        [XmlAttribute]
        public string SelectionName { get; set; }
        [XmlAttribute]
        public int DefaultPause { get; set; }
        [XmlAttribute]
        public int LineNumber { get; set; }
        [XmlAttribute]
        public bool PauseBeforeExeucution { get; set; }
        [XmlIgnore]
        public Color DisplayForeColor { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Comment Field (Optional)")]
        public string v_Comment { get; set; }
        [XmlAttribute]
        public bool CommandEnabled { get; set; }

        public ScriptCommand()
        {
            this.DisplayForeColor = System.Drawing.Color.SteelBlue;
            this.CommandEnabled = false;
            this.DefaultPause = 250;
            this.IsCommented = false;
        }

        public virtual void RunCommand(object sender)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }
        public virtual void RunCommand(object sender, Core.Script.ScriptAction command)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }

        public virtual string GetDisplayValue()
        {
            return SelectionName;
        }
    }

    #endregion Base Command

    #region Legacy IE Web Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new IE web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserCreateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public IEBrowserCreateCommand()
        {
            this.CommandName = "IEBrowserCreateCommand";
            this.SelectionName = "Create Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = false;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            var newBrowserSession = new InternetExplorer
            {
                Visible = true
            };
            engine.AppInstances.Add(v_InstanceName, newBrowserSession);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to find and attach to an existing IE web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserFindBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Enter the Browser Name")]
        public string v_IEBrowserName { get; set; }

        public IEBrowserFindBrowserCommand()
        {
            this.CommandName = "IEBrowserFindBrowserCommand";
            this.SelectionName = "Find Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = false;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            bool browserFound = false;
            var shellWindows = new ShellWindows();
            foreach (IWebBrowser2 shellWindow in shellWindows)
            {
                if ((shellWindow.Document is MSHTML.HTMLDocument) && (shellWindow.Document.Title == v_IEBrowserName))
                {
                    engine.AppInstances.Add(v_InstanceName, shellWindow.Application);
                    browserFound = true;
                    break;
                }
            }

            //try partial match
            if (!browserFound)
            {
                foreach (IWebBrowser2 shellWindow in shellWindows)
                {
                    if ((shellWindow.Document is MSHTML.HTMLDocument) && (shellWindow.Document.Title.Contains(v_IEBrowserName)))
                    {
                        engine.AppInstances.Add(v_InstanceName, shellWindow.Application);
                        browserFound = true;
                        break;
                    }
                }
            }

            if (!browserFound)
            {
                throw new Exception("Browser was not found!");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Browser Name: '" + v_IEBrowserName + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate the associated IE web browser to a URL.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserNavigateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL to navigate to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        public IEBrowserNavigateCommand()
        {
            this.CommandName = "WebBrowserNavigateCommand";
            this.SelectionName = "Navigate";
            this.v_InstanceName = "default";
            this.CommandEnabled = false;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object browserObject))
            {
                var browserInstance = (SHDocVw.InternetExplorer)browserObject;
                browserInstance.Navigate(v_URL.ConvertToUserVariable(sender));
                WaitForReadyState(browserInstance);
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [URL: '" + v_URL + "', Instance Name: '" + v_InstanceName + "']";
        }
        private void WaitForReadyState(SHDocVw.InternetExplorer ieInstance)
        {
            DateTime waitExpires = DateTime.Now.AddSeconds(15);

            do

            {
                System.Threading.Thread.Sleep(500);
            }

            while ((ieInstance.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close the associated IE web browser")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll to achieve automation.")]
    public class IEBrowserCloseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public IEBrowserCloseCommand()
        {
            this.CommandName = "IEBrowserCloseCommand";
            this.SelectionName = "Close Browser";
            this.CommandEnabled = false;
            this.v_InstanceName = "default";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object browserObject))
            {
                var browserInstance = (SHDocVw.InternetExplorer)browserObject;
                browserInstance.Quit();
                engine.AppInstances.Remove(v_InstanceName);

            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("IE Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to manipulate (get or set) elements within the HTML document of the associated IE web browser.  Features an assisting element capture form")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the 'InternetExplorer' application object from SHDocVw.dll and MSHTML.dll to achieve automation.")]
    public class IEBrowserElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter or capture element search parameters")]
        public System.Data.DataTable v_WebSearchTable { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select an action")]
        public string v_WebAction { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Action Parameters")]
        public System.Data.DataTable v_WebActionParameterTable { get; set; }

        public IEBrowserElementCommand()
        {
            this.CommandName = "IEBrowserElementCommand";
            this.SelectionName = "Element Action";
            //this command is broken -- consider enhancing selenium instead
            this.CommandEnabled = false;
            this.v_InstanceName = "default";

            this.v_WebSearchTable = new System.Data.DataTable
            {
                TableName = DateTime.Now.ToString("WebSearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            };
            this.v_WebSearchTable.Columns.Add("Enabled");
            this.v_WebSearchTable.Columns.Add("Property Name");
            this.v_WebSearchTable.Columns.Add("Property Value");

            this.v_WebActionParameterTable = new System.Data.DataTable
            {
                TableName = DateTime.Now.ToString("WebActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            };
            this.v_WebActionParameterTable.Columns.Add("Parameter Name");
            this.v_WebActionParameterTable.Columns.Add("Parameter Value");
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;

            if (engine.AppInstances.TryGetValue(v_InstanceName, out object browserObject))
            {
                var browserInstance = (SHDocVw.InternetExplorer)browserObject;

                DataTable searchTable = Core.Common.Clone<DataTable>(v_WebSearchTable);

                DataColumn matchFoundColumn = new DataColumn
                {
                    ColumnName = "Match Found",
                    DefaultValue = false
                };
                searchTable.Columns.Add(matchFoundColumn);

                var elementSearchProperties = from rws in searchTable.AsEnumerable()
                                              where rws.Field<string>("Enabled") == "True"
                                              select rws;

                bool qualifyingElementFound = false;

                foreach (IHTMLElement element in browserInstance.Document.All)
                {
                    qualifyingElementFound = FindQualifyingElement(elementSearchProperties, element);

                    if ((qualifyingElementFound) && (v_WebAction == "Invoke Click"))
                    {
                        element.click();
                        WaitForReadyState(browserInstance);
                        break;
                    }
                    else if ((qualifyingElementFound) && (v_WebAction == "Left Click") || (qualifyingElementFound) && (v_WebAction == "Middle Click") || (qualifyingElementFound) && (v_WebAction == "Right Click"))
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

                        SendMouseMoveCommand newMouseMove = new SendMouseMoveCommand
                        {
                            v_XMousePosition = (elementXposition + ieClientLocation.left + 10) + userXAdjust, // + 10 gives extra padding
                            v_YMousePosition = (elementYposition + ieClientLocation.top + 90) + userYAdjust, // +90 accounts for title bar height
                            v_MouseClick = v_WebAction
                        };
                        newMouseMove.RunCommand(sender);

                        break;
                    }
                    else if ((qualifyingElementFound) && (v_WebAction == "Set Attribute"))
                    {
                        string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "Attribute Name"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string valueToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                             where rw.Field<string>("Parameter Name") == "Value To Set"
                                             select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        valueToSet = valueToSet.ConvertToUserVariable(sender);

                        element.setAttribute(attributeName, valueToSet);
                        break;
                    }
                    else if ((qualifyingElementFound) && (v_WebAction == "Get Attribute"))
                    {
                        string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "Attribute Name"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string VariableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "Variable Name"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string convertedAttribute = Convert.ToString(element.getAttribute(attributeName));

                        convertedAttribute.StoreInUserVariable(sender, VariableName);

                        break;
                    }
                }

                if (!qualifyingElementFound)
                {
                    throw new Exception("Could not find the element!");
                }
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        private bool FindQualifyingElement(EnumerableRowCollection<DataRow> elementSearchProperties, IHTMLElement element)
        {
            foreach (DataRow seachCriteria in elementSearchProperties)
            {
                string searchPropertyName = seachCriteria.Field<string>("Property Name");
                string searchPropertyValue = seachCriteria.Field<string>("Property Value");
                string searchPropertyFound = seachCriteria.Field<string>("Match Found");

                searchPropertyFound = "False";

                if (element.GetType().GetProperty(searchPropertyName) == null)
                {
                    return false;
                }

                if (int.TryParse(searchPropertyValue, out int searchValue))
                {
                    int elementValue = (int)element.GetType().GetProperty(searchPropertyName).GetValue(element, null);
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
                    string elementValue = (string)element.GetType().GetProperty(searchPropertyName).GetValue(element, null);
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

            foreach (var seachCriteria in elementSearchProperties)
            {
                Console.WriteLine(seachCriteria.Field<string>("Property Value"));
            }

            return elementSearchProperties.Where(seachCriteria => seachCriteria.Field<string>("Match Found") == "True").Count() == elementSearchProperties.Count();
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

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Action: '" + v_WebAction + "', Instance Name: '" + v_InstanceName + "']";
        }

        private void WaitForReadyState(SHDocVw.InternetExplorer ieInstance)
        {
            DateTime waitExpires = DateTime.Now.AddSeconds(15);

            do

            {
                System.Threading.Thread.Sleep(500);
            }

            while ((ieInstance.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
        }
    }

    #endregion Legacy IE Web Commands

    #region Web Selenium

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserCreateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Instance Tracking (after task ends)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Forget Instance")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Keep Instance Alive")]
        public string v_InstanceTracking { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select a Window State")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Normal")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Maximize")]
        public string v_BrowserWindowOption { get; set; }

        public SeleniumBrowserCreateCommand()
        {
            this.CommandName = "SeleniumBrowserCreateCommand";
            this.SelectionName = "Create Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            var driverPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), "Resources");
            OpenQA.Selenium.Chrome.ChromeDriverService driverService = OpenQA.Selenium.Chrome.ChromeDriverService.CreateDefaultService(driverPath);
         
            var newSeleniumSession = new OpenQA.Selenium.Chrome.ChromeDriver(driverService, new OpenQA.Selenium.Chrome.ChromeOptions());

            var instanceName = v_InstanceName.ConvertToUserVariable(sender);

            if (engine.AppInstances.ContainsKey(v_InstanceName))
            {
                //need to figure out how to handle multiple potential session names
                engine.AppInstances.Remove(v_InstanceName);
            }

            //add to engine
            engine.AppInstances.Add(v_InstanceName, newSeleniumSession);



            //handle app instance tracking
            if (v_InstanceTracking == "Keep Instance Alive")
            {
                GlobalAppInstances.AddInstance(instanceName, newSeleniumSession);             
            }

            //handle window type on startup - https://github.com/saucepleez/taskt/issues/22
            switch (v_BrowserWindowOption)
            {
                case "Maximize":
                    newSeleniumSession.Manage().Window.Maximize();
                    break;
                case "Normal":
                case "":
                default:
                    break;
            }


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "', Instance Tracking: " + v_InstanceTracking + "]";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateURLCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL to navigate to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        public SeleniumBrowserNavigateURLCommand()
        {
            this.CommandName = "SeleniumBrowserNavigateURLCommand";
            this.SelectionName = "Navigate to URL";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;

            if (engine.AppInstances.TryGetValue(v_InstanceName, out object browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().GoToUrl(v_URL.ConvertToUserVariable(sender));
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [URL: '" + v_URL + "', Instance Name: '" + v_InstanceName + "']";
        }
        private void WaitForReadyState(SHDocVw.InternetExplorer ieInstance)
        {
            DateTime waitExpires = DateTime.Now.AddSeconds(15);

            do

            {
                System.Threading.Thread.Sleep(500);
            }

            while ((ieInstance.ReadyState != SHDocVw.tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate forward a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateForwardCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserNavigateForwardCommand()
        {
            this.CommandName = "WebBrowserNavigateCommand";
            this.SelectionName = "Navigate Forward";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Forward();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate backwards in a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateBackCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserNavigateBackCommand()
        {
            this.CommandName = "SeleniumBrowserNavigateBackCommand";
            this.SelectionName = "Navigate Back";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Back();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to refresh a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserRefreshCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserRefreshCommand()
        {
            this.CommandName = "SeleniumBrowserRefreshCommand";
            this.SelectionName = "Refresh";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender; 
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Refresh();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserCloseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public SeleniumBrowserCloseCommand()
        {
            this.CommandName = "SeleniumBrowserCloseCommand";
            this.SelectionName = "Close Browser";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Quit();
                seleniumInstance.Dispose();
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserElementActionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Element Search Method")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By XPath")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By ID")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Tag Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Class Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By CSS Selector")]
        public string v_SeleniumSearchType { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Element Search Parameter")]
        public string v_SeleniumSearchParameter { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Element Action")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Invoke Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Double Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Clear Element")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Set Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Attribute")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Wait For Element To Exist")]
        public string v_SeleniumElementAction { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_WebActionParameterTable { get; set; }

        public SeleniumBrowserElementActionCommand()
        {
            this.CommandName = "SeleniumBrowserCreateCommand";
            this.SelectionName = "Element Action";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;

            this.v_WebActionParameterTable = new System.Data.DataTable
            {
                TableName = DateTime.Now.ToString("WebActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            };
            this.v_WebActionParameterTable.Columns.Add("Parameter Name");
            this.v_WebActionParameterTable.Columns.Add("Parameter Value");
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            //convert to user variable -- https://github.com/saucepleez/taskt/issues/22
            var seleniumSearchParam = v_SeleniumSearchParameter.ConvertToUserVariable(sender);
      

            if (engine.AppInstances.TryGetValue(v_InstanceName, out object browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;

                OpenQA.Selenium.IWebElement element = null;

                if (v_SeleniumElementAction == "Wait For Element To Exist")
                {

                    var timeoutText = (from rw in v_WebActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    timeoutText = timeoutText.ConvertToUserVariable(sender);

                    int timeOut = Convert.ToInt32(timeoutText);

                    var timeToEnd = DateTime.Now.AddSeconds(timeOut);

                    while (timeToEnd >= DateTime.Now)
                    {
                        try
                        {
                            element = FindElement(seleniumInstance);
                            break;
                        }
                        catch (Exception)
                        {
                            engine.ReportProgress("Element Not Yet Found... " + (timeToEnd - DateTime.Now).Seconds + "s remain");
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                    if (element == null)
                    {
                        throw new Exception("Element Not Found");
                    }

                    return;
                }
                else
                {

                    element = FindElement(seleniumInstance);
                }


                switch (v_SeleniumElementAction)
                {
                    case "Invoke Click":
                        element.Click();
                        break;

                    case "Left Click":
                    case "Right Click":
                    case "Middle Click":
                    case "Double Left Click":


                        int userXAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                           where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                           select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

                        int userYAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                           where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                           select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

                        var elementLocation = element.Location;
                        SendMouseMoveCommand newMouseMove = new SendMouseMoveCommand();
                        var seleniumWindowPosition = seleniumInstance.Manage().Window.Position;
                        newMouseMove.v_XMousePosition = (seleniumWindowPosition.X + elementLocation.X + 30 + userXAdjust); // added 30 for offset
                        newMouseMove.v_YMousePosition = (seleniumWindowPosition.Y + elementLocation.Y + 130 + userYAdjust); //added 130 for offset
                        newMouseMove.v_MouseClick = v_SeleniumElementAction;
                        newMouseMove.RunCommand(sender);
                        break;

                    case "Set Text":

                        string textToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Text To Set"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        
                        string clearElement = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Clear Element Before Setting Text"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        if (clearElement == null)
                        {
                            clearElement = "No";
                        }

                        if (clearElement == "Yes")
                        {
                            element.Clear();
                        }


                        string[] potentialKeyPresses = textToSet.Split('{', '}');

                        Type seleniumKeys = typeof(OpenQA.Selenium.Keys);
                        System.Reflection.FieldInfo[] fields = seleniumKeys.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

                        //check if chunked string contains a key press command like {ENTER}
                        foreach (string chunkedString in potentialKeyPresses)
                        {
                            if (chunkedString == "")
                                continue;

                            if (fields.Any(f => f.Name == chunkedString))
                            {
                                string keyPress = (string)fields.Where(f => f.Name == chunkedString).FirstOrDefault().GetValue(null);
                                seleniumInstance.Keyboard.PressKey(keyPress);
                            }
                            else
                            {
                                //convert to user variable - https://github.com/saucepleez/taskt/issues/22
                                var convertedChunk = chunkedString.ConvertToUserVariable(sender);
                                element.SendKeys(convertedChunk);
                            }
                        }

                        break;

                    case "Get Text":
                    case "Get Attribute":

                        string VariableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "Variable Name"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "Attribute Name"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender);

                        string elementValue;
                        if (v_SeleniumElementAction == "Get Text")
                        {
                            elementValue = element.Text;
                        }
                        else
                        {
                            elementValue = element.GetAttribute(attributeName);
                        }

                        elementValue.StoreInUserVariable(sender, VariableName);

                        break;
                    case "Clear Element":
                        element.Clear();
                        break;
                    default:
                        throw new Exception("Element Action was not found");
                }
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }
        }

        private OpenQA.Selenium.IWebElement FindElement(OpenQA.Selenium.Chrome.ChromeDriver seleniumInstance)
        {
            OpenQA.Selenium.IWebElement element = null;

            switch (v_SeleniumSearchType)
            {
                case "Find Element By XPath":
                    element = seleniumInstance.FindElementByXPath(v_SeleniumSearchParameter);
                    break;

                case "Find Element By ID":
                    element = seleniumInstance.FindElementById(v_SeleniumSearchParameter);
                    break;

                case "Find Element By Name":
                    element = seleniumInstance.FindElementByName(v_SeleniumSearchParameter);
                    break;

                case "Find Element By Tag Name":
                    element = seleniumInstance.FindElementByTagName(v_SeleniumSearchParameter);
                    break;

                case "Find Element By Class Name":
                    element = seleniumInstance.FindElementByClassName(v_SeleniumSearchParameter);
                    break;
                case "Find Element By CSS Selector":
                    element = seleniumInstance.FindElementByCssSelector(v_SeleniumSearchParameter);
                    break;
                default:
                    throw new Exception("Search Type was not found");
            }

            return element;
        }

        public bool ElementExists(object sender, string searchType, string elementName)
        {
            var engine = (Core.AutomationEngineInstance)sender;

            if (engine.AppInstances.TryGetValue(v_InstanceName, out object browserObject))
            {
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                v_SeleniumSearchType = searchType.ConvertToUserVariable(sender);
                v_SeleniumSearchParameter = elementName.ConvertToUserVariable(sender);

                try
                {
                    var element = FindElement(seleniumInstance);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                throw new Exception("Session Instance was not found");
            }

  
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_SeleniumSearchType + " and " + v_SeleniumElementAction + ", Instance Name: '" + v_InstanceName + "']";
        }
    }

    #endregion Web Selenium

    #region Misc Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time in milliseconds.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class PauseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Amount of time to pause for (in milliseconds).")]
        public int v_PauseLength { get; set; }

        public PauseCommand()
        {
            this.CommandName = "PauseCommand";
            this.SelectionName = "Pause Script";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            System.Threading.Thread.Sleep(v_PauseLength);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Wait for " + v_PauseLength + "ms]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time in milliseconds.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class ErrorHandlingCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Action On Error")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Stop Processing")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Continue Processing")]
        public string v_ErrorHandlingAction { get; set; }

        public ErrorHandlingCommand()
        {
            this.CommandName = "ErrorHandlingCommand";
            this.SelectionName = "Error Handling";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            engine.ErrorHandler = this;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Action: " + v_ErrorHandlingAction + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to add an in-line comment to the configuration.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is for visual purposes only")]
    public class CommentCommand : ScriptCommand
    {
        public CommentCommand()
        {
            this.CommandName = "CommentCommand";
            this.SelectionName = "Add Code Comment";
            this.DisplayForeColor = System.Drawing.Color.ForestGreen;
            this.CommandEnabled = true;
        }

        public override string GetDisplayValue()
        {
            return "// Comment: " + this.v_Comment;
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to show a MessageBox and supports variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    public class MessageBoxCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the message to be displayed.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Message { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Close After X (Seconds) - 0 to bypass")]
        public int v_AutoCloseAfter { get; set; }
        public MessageBoxCommand()
        {
            this.CommandName = "MessageBoxCommand";
            this.SelectionName = "Show Message";
            this.CommandEnabled = true;
            this.v_AutoCloseAfter = 0;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            string variableMessage = v_Message.ConvertToUserVariable(sender);

            if (engine.tasktEngineUI == null)
            {
                engine.ReportProgress("Complex Messagebox Supported With UI Only");
                System.Windows.Forms.MessageBox.Show(variableMessage, "Message Box Command", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);        
                return;
            }

            var result = engine.tasktEngineUI.Invoke(new Action(() =>
            {
                engine.tasktEngineUI.ShowMessage(variableMessage, "MessageBox Command", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, v_AutoCloseAfter);
            }

            ));
            
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Message: " + v_Message + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command activates a window and brings it to the front.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetForegroundWindow', 'ShowWindow' from user32.dll to achieve automation.")]
    public class ActivateWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

        public ActivateWindowCommand()
        {
            this.CommandName = "ActivateWindowCommand";
            this.SelectionName = "Activate Window";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            string windowName = v_WindowName.ConvertToUserVariable(sender);
            IntPtr hWnd = User32Functions.FindWindow(windowName);
            if (hWnd != IntPtr.Zero)
            {
                User32Functions.SetWindowState(hWnd, User32Functions.WindowState.SW_SHOWNORMAL);
                User32Functions.SetForegroundWindow(hWnd);
            }
            else
            {
                throw new Exception("Window not found. Expected to find: " + v_WindowName);
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command moves a window to a specified location on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class MoveWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the X position to move the window to.")]
        public string v_XWindowPosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Y position to move the window to.")]
        public string v_YWindowPosition { get; set; }

        public MoveWindowCommand()
        {
            this.CommandName = "MoveWindowCommand";
            this.SelectionName = "Move Window";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

            string windowName = v_WindowName.ConvertToUserVariable(sender);
            IntPtr hWnd = User32Functions.FindWindow(windowName);

            if (hWnd != IntPtr.Zero)
            {
                
                var variableXPosition = v_XWindowPosition.ConvertToUserVariable(sender);
                var variableYPosition = v_YWindowPosition.ConvertToUserVariable(sender);

                if (!int.TryParse(variableXPosition, out int xPos))
                {
                    throw new Exception("X Position Invalid - " + v_XWindowPosition);
                }
                if (!int.TryParse(variableYPosition, out int yPos))
                {
                    throw new Exception("X Position Invalid - " + v_XWindowPosition);
                }


                User32Functions.SetWindowPosition(hWnd, xPos, yPos);
            }
            else
            {
                throw new Exception("Window not found");
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Coordinates (" + v_XWindowPosition + "," + v_YWindowPosition + ")]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command resizes a window to a specified size.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class ResizeWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the new window width")]
        public string v_XWindowSize { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the new window height")]
        public string v_YWindowSize { get; set; }

        public ResizeWindowCommand()
        {
            this.CommandName = "ResizeWindowCommand";
            this.SelectionName = "Resize Window";

            //not working
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            string windowName = v_WindowName.ConvertToUserVariable(sender);
            IntPtr hWnd = User32Functions.FindWindow(windowName);

            if (hWnd != IntPtr.Zero)
            {
                var variableXSize = v_XWindowSize.ConvertToUserVariable(sender);
                var variableYSize = v_YWindowSize.ConvertToUserVariable(sender);

                if (!int.TryParse(variableXSize, out int xPos))
                {
                    throw new Exception("X Position Invalid - " + v_XWindowSize);
                }
                if (!int.TryParse(variableYSize, out int yPos))
                {
                    throw new Exception("X Position Invalid - " + v_YWindowSize);
                }

                User32Functions.SetWindowSize(hWnd, xPos, yPos);
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Size (" + v_XWindowSize + "," + v_YWindowSize + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command closes an open window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SendMessage' from user32.dll to achieve automation.")]
    public class CloseWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

        public CloseWindowCommand()
        {
            this.CommandName = "CloseWindowCommand";
            this.SelectionName = "Close Window";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            string windowName = v_WindowName.ConvertToUserVariable(sender);
            IntPtr hWnd = User32Functions.FindWindow(windowName);

            if (hWnd != IntPtr.Zero)
            {
                User32Functions.CloseWindow(hWnd);
            }
            else
            {
                throw new Exception("Window not found");
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command sets a target windows state (minimize, maximize, restore)")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class SetWindowStateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select a Window State")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Maximize")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Minimize")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Restore")]
        public string v_WindowState { get; set; }

        public SetWindowStateCommand()
        {
            this.CommandName = "SetWindowStateCommand";
            this.SelectionName = "Set Window State";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            string windowName = v_WindowName.ConvertToUserVariable(sender);
            IntPtr hWnd = User32Functions.FindWindow(windowName);

            if (hWnd != IntPtr.Zero) //If found
            {
                User32Functions.WindowState WINDOW_STATE = User32Functions.WindowState.SW_SHOWNORMAL;
                switch (v_WindowState)
                {
                    case "Maximize":
                        WINDOW_STATE = User32Functions.WindowState.SW_MAXIMIZE;
                        break;

                    case "Minimize":
                        WINDOW_STATE = User32Functions.WindowState.SW_MINIMIZE;
                        break;

                    case "Restore":
                        WINDOW_STATE = User32Functions.WindowState.SW_RESTORE;
                        break;

                    default:
                        break;
                }

                User32Functions.SetWindowState(hWnd, WINDOW_STATE);
            }
            else
            {
                throw new Exception("Window not found");
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Window State: " + v_WindowState + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command waits for a window to exist")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class WaitForWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Seconds To Wait")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_LengthToWait { get; set; }

        public WaitForWindowCommand()
        {
            this.CommandName = "WaitForWindowCommand";
            this.SelectionName = "Wait For Window To Exist";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var lengthToWait = v_LengthToWait.ConvertToUserVariable(sender);
            var waitUntil = int.Parse(lengthToWait);
            var endDateTime = DateTime.Now.AddSeconds(waitUntil);

            IntPtr hWnd = IntPtr.Zero;

            while (DateTime.Now < endDateTime)
            {
                string windowName = v_WindowName.ConvertToUserVariable(sender);
                hWnd = User32Functions.FindWindow(windowName);

                if (hWnd != IntPtr.Zero) //If found
                    break;

                System.Threading.Thread.Sleep(1000);

            }

            if (hWnd == IntPtr.Zero)
            {
                throw new Exception("Window was not found in the allowed time!");
            }




        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: '" + v_WindowName + "', Wait Up To " + v_LengthToWait + " seconds]";
        }

    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to start a program or a process. You can use short names 'chrome.exe' or fully qualified names 'c:/some.exe'")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start'.")]
    public class StartProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the name or path to the program (ex. notepad, calc)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_ProgramName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter any arguments (if applicable)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ProgramArgs { get; set; }

        public StartProcessCommand()
        {
            this.CommandName = "StartProcessCommand";
            this.SelectionName = "Start Process";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

            string vProgramName = v_ProgramName.ConvertToUserVariable(sender);
            string vProgramArgs = v_ProgramArgs.ConvertToUserVariable(sender);

            if (v_ProgramArgs == "")
            {
                System.Diagnostics.Process.Start(vProgramName);
            }
            else
            {
                System.Diagnostics.Process.Start(vProgramName, vProgramArgs);
            }

            System.Threading.Thread.Sleep(2000);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Process: " + v_ProgramName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to stop a program or a process. You can use the name of the process 'chrome'. Alternatively, you may use the Close Window or Thick App Command instead.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.CloseMainWindow'.")]
    public class StopProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the process name to be stopped (calc, notepad)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ProgramShortName { get; set; }

        public StopProcessCommand()
        {
            this.CommandName = "StopProgramCommand";
            this.SelectionName = "Stop Process";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            string shortName = v_ProgramShortName.ConvertToUserVariable(sender);
            var processes = System.Diagnostics.Process.GetProcessesByName(shortName);

            foreach (var prc in processes)
                prc.CloseMainWindow();
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Process: " + v_ProgramShortName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to run a script or program and wait for it to exit before proceeding.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    public class RunScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the path to the script")]
        public string v_ScriptPath { get; set; }

        public RunScriptCommand()
        {
            this.CommandName = "RunScriptCommand";
            this.SelectionName = "Run Script";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            {
                System.Diagnostics.Process scriptProc = new System.Diagnostics.Process();

                var scriptPath = v_ScriptPath.ConvertToUserVariable(sender);
                scriptProc.StartInfo.FileName = scriptPath;
                scriptProc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                scriptProc.Start();
                scriptProc.WaitForExit();

                scriptProc.Close();
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Script Path: " + v_ScriptPath + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to run C# code from the input")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    public class RunCustomCodeCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Paste the C# code to execute")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowCodeBuilder)]
        public string v_Code { get; set; }

        public RunCustomCodeCommand()
        {
            this.CommandName = "RunCustomCodeCommand";
            this.SelectionName = "Run Custom Code";
            this.CommandEnabled = true;       
        }

        public override void RunCommand(object sender)
        {
            //create compiler service
            var compilerSvc = new Core.CompilerServices();
            var customCode = v_Code.ConvertToUserVariable(sender);

            //compile custom code
            var result = compilerSvc.CompileInput(customCode);

            //check for errors
            if (result.Errors.HasErrors)
            {
                //throw exception
                var errors = string.Join(", ", result.Errors);
                throw new Exception("Errors Occured: " + errors);
            }
            else
            {
                //run code, taskt will wait for the app to exit before resuming
                System.Diagnostics.Process scriptProc = new System.Diagnostics.Process();
                scriptProc.StartInfo.FileName = result.PathToAssembly;
                scriptProc.Start();
                scriptProc.WaitForExit();
                scriptProc.Close();
            }


        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
    
    [Serializable]
    [Attributes.ClassAttributes.Group("Clipboard Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to get text from the clipboard.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against the VariableList from the scripting engine using System.Windows.Forms.Clipboard.")]
    public class ClipboardGetTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to set clipboard contents")]
        public string v_userVariableName { get; set; }

        public ClipboardGetTextCommand()
        {
            this.CommandName = "ClipboardCommand";
            this.SelectionName = "Get Text";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            User32Functions.GetClipboardText().StoreInUserVariable(sender, v_userVariableName);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get Text From Clipboard and Apply to Variable: " + v_userVariableName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to send email using SMTP.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the System.Net Namespace to achieve automation")]
    public class SMTPSendEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Host Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPHost { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Port")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public int v_SMTPPort { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Username")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPUserName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Password")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPPassword { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("From Email")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPFromEmail { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("To Email")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPToEmail { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Subject")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPSubject { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Body")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPBody { get; set; }
        public SMTPSendEmailCommand()
        {
            this.CommandName = "SMTPCommand";
            this.SelectionName = "Send SMTP Email";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            string varSMTPHost = v_SMTPHost.ConvertToUserVariable(sender);
            string varSMTPPort = v_SMTPPort.ToString().ConvertToUserVariable(sender);
            string varSMTPUserName = v_SMTPUserName.ConvertToUserVariable(sender);
            string varSMTPPassword = v_SMTPPassword.ConvertToUserVariable(sender);

            string varSMTPFromEmail = v_SMTPFromEmail.ConvertToUserVariable(sender);
            string varSMTPToEmail = v_SMTPToEmail.ConvertToUserVariable(sender);
            string varSMTPSubject = v_SMTPSubject.ConvertToUserVariable(sender);
            string varSMTPBody = v_SMTPBody.ConvertToUserVariable(sender);

            var client = new SmtpClient(varSMTPHost, int.Parse(varSMTPPort))
            {
                Credentials = new System.Net.NetworkCredential(varSMTPUserName, varSMTPPassword),
                EnableSsl = true
            };

            client.Send(varSMTPFromEmail, varSMTPToEmail, varSMTPSubject, varSMTPBody);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [To Address: '" + v_SMTPToEmail + "']";
        }
    }


    #endregion Misc Commands

    #region Input Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Use this command to send key strokes to the current or a targeted window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    public class SendKeysCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter text to send")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_TextToSend { get; set; }

        public SendKeysCommand()
        {
            this.CommandName = "SendKeysCommand";
            this.SelectionName = "Send Keystrokes";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            if (v_WindowName != "Current Window")
            {
                ActivateWindowCommand activateWindow = new ActivateWindowCommand
                {
                    v_WindowName = v_WindowName
                };
                activateWindow.RunCommand(sender);
            }

            string textToSend = v_TextToSend.ConvertToUserVariable(sender);
            System.Windows.Forms.SendKeys.SendWait(textToSend);

            System.Threading.Thread.Sleep(500);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Send '" + v_TextToSend + "' to '" + v_WindowName + "']";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Use this command to simulate mouse movement and click the mouse on coordinates.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    public class SendMouseMoveCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the X position to move the mouse to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper)]
        public int v_XMousePosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the Y position to move the mouse to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper)]
        public int v_YMousePosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate mouse click type if required")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("None")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Double Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Up")]
        public string v_MouseClick { get; set; }

        public SendMouseMoveCommand()
        {
            this.CommandName = "SendMouseMoveCommand";
            this.SelectionName = "Send Mouse Move";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            User32Functions.SetCursorPosition(v_XMousePosition, v_YMousePosition);
            User32Functions.SendMouseClick(v_MouseClick, v_XMousePosition, v_YMousePosition);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Coordinates (" + v_XMousePosition + "," + v_YMousePosition + ") Click: " + v_MouseClick + "]";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("Command that groups multiple actions")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements many commands in a list.")]
    public class SequenceCommand : ScriptCommand
    {
      public List<ScriptCommand> v_scriptActions = new List<ScriptCommand>();
       

        public SequenceCommand()
        {
            this.CommandName = "SequenceCommand";
            this.SelectionName = "Sequence Command";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {

            var engine = (Core.AutomationEngineInstance)sender;

            foreach (var item in v_scriptActions)
            {

                //exit if cancellation pending
                if (engine.IsCancellationPending)
                {
                    return;
                }

                //only run if not commented
                if (!item.IsCommented)
                    item.RunCommand(sender);
           
             
           
            }
  
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_scriptActions.Count() + " embedded commands]";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Use this command to simulate mouse click on coordinates.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    public class SendMouseClickCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate mouse click type")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Double Left Click")]
        public string v_MouseClick { get; set; }

        public SendMouseClickCommand()
        {
            this.CommandName = "SendMouseClickCommand";
            this.SelectionName = "Send Mouse Click";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var mousePosition = System.Windows.Forms.Cursor.Position;
            User32Functions.SendMouseClick(v_MouseClick, mousePosition.X, mousePosition.Y);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Click Type: " + v_MouseClick + "]";
        }
    }
        [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("This command clicks an item in a Thick Application window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows UI Automation' to find elements and invokes a SendMouseMove Command to click and achieve automation")]
    public class ThickAppClickItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Window to Automate")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_AutomationWindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Appropriate Item")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_AutomationHandleName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate mouse click type if required")]
        public string v_MouseClick { get; set; }

        public ThickAppClickItemCommand()
        {
            this.CommandName = "ThickAppClickItemCommand";
            this.SelectionName = "Click UI Item";
            this.CommandEnabled = true;
            this.DefaultPause = 3000;
        }

        public override void RunCommand(object sender)
        {

            var variableWindowName = v_AutomationWindowName.ConvertToUserVariable(sender);

            var searchItem = AutomationElement.RootElement.FindFirst
            (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
            variableWindowName));

            if (searchItem == null)
            {
                throw new Exception("Window not found");
            }

            var requiredHandleName = v_AutomationHandleName.ConvertToUserVariable(sender);
            var requiredItem = searchItem.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, requiredHandleName));

            var newActivateWindow = new ActivateWindowCommand
            {
                v_WindowName = variableWindowName
            };
            newActivateWindow.RunCommand(sender);

            //get newpoint for now
            var newPoint = requiredItem.GetClickablePoint();

            //send mousemove command
            var newMouseMove = new SendMouseMoveCommand
            {
                v_XMousePosition = (int)newPoint.X,
                v_YMousePosition = (int)newPoint.Y,
                v_MouseClick = v_MouseClick
            };
            newMouseMove.RunCommand(sender);
        }

        public List<string> FindHandleObjects(string windowTitle)
        {
            var automationElement = AutomationElement.RootElement.FindFirst
    (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
    windowTitle));

            var searchItems = automationElement.FindAll(TreeScope.Descendants, PropertyCondition.TrueCondition);

            List<String> handleList = new List<String>();
            foreach (AutomationElement item in searchItems)
            {
                if (item.Current.Name.Trim() != string.Empty)
                    handleList.Add(item.Current.Name);
            }
            // handleList = handleList.OrderBy(x => x).ToList();

            return handleList;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Perform " + v_MouseClick + " on '" + v_AutomationHandleName + "' in Window '" + v_AutomationWindowName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("This command gets text from a Thick Application window and assigns it to a variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows UI Automation' to find elements and invokes a Variable Command to assign data and achieve automation")]
    public class ThickAppGetTextCommand : ScriptCommand
    {
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Window to Automate")]
        public string v_AutomationWindowName { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Appropriate Item")]
        public string v_AutomationHandleDisplayName { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Automation ID of the Item")]
        public string v_AutomationID { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        public string v_userVariableName { get; set; }

        public ThickAppGetTextCommand()
        {
            this.CommandName = "ThickAppGetTextCommand";
            this.SelectionName = "Get UI Item";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var variableWindowName = v_AutomationWindowName.ConvertToUserVariable(sender);

            var searchItem = AutomationElement.RootElement.FindFirst
            (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
            variableWindowName));

            if (searchItem == null)
            {
                throw new Exception("Window not found");
            }

            var requiredItem = searchItem.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, v_AutomationID));

            var newVariableCommand = new Core.AutomationCommands.VariableCommand
            {
                v_userVariableName = v_userVariableName,
                v_Input = requiredItem.Current.Name
            };
            newVariableCommand.RunCommand(sender);
        }

        public string FindHandleID(string windowTitle, string nameProperty)
        {
            var automationElement = AutomationElement.RootElement.FindFirst
    (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
    windowTitle));

            var requiredItem = automationElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, nameProperty));

            return requiredItem.Current.AutomationId;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set Variable [" + v_userVariableName + "] From ID " + v_AutomationID + " (" + v_AutomationHandleDisplayName + ") in Window '" + v_AutomationWindowName + "']";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("This command gets text from a Thick Application window and assigns it to a variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows UI Automation' to find elements and invokes a Variable Command to assign data and achieve automation")]
    public class UIAutomationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the action")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Click Element")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Value From Element")]
        public string v_AutomationType { get; set; }

        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Window to Automate")]
        public string v_WindowName { get; set; }

        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowElementRecorder)]
        [Attributes.PropertyAttributes.PropertyDescription("Set Search Parameters")]
        public DataTable v_UIASearchParameters { get; set; }

        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Set Action Parameters")]
        public DataTable v_UIAActionParameters { get; set; }

        public UIAutomationCommand()
        {
            this.CommandName = "UIAutomationCommand";
            this.SelectionName = "UI Automation";
            this.CommandEnabled = true;

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

        public PropertyCondition CreatePropertyCondition(string propertyName, string propertyValue)
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
   
        public override void RunCommand(object sender)
        {

            //create variable window name
            var variableWindowName = v_WindowName.ConvertToUserVariable(sender);

            //create search params
            var searchParams = from rw in v_UIASearchParameters.AsEnumerable()
                               where rw.Field<string>("Enabled") == "True"
                               select rw;

            //create and populate condition list
            var conditionList = new List<Condition>();
            foreach (var param in searchParams)
            {
              var parameterName =  (string)param["Parameter Name"];
              var parameterValue = (string)param["Parameter Value"];

                parameterName = parameterName.ConvertToUserVariable(sender);
                parameterValue = parameterValue.ConvertToUserVariable(sender);

                var propCondition = CreatePropertyCondition(parameterName, parameterValue);
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
            var requiredHandle = windowElement.FindFirst(TreeScope.Descendants, searchConditions);

            //if handle was not found
            if (requiredHandle == null)
                throw new Exception("Element was not found in window '" + variableWindowName + "'");

            //determine element click type
            if (v_AutomationType == "Click Element")
            {

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
                    v_XMousePosition = (int)newPoint.X + xAdjustInt,
                    v_YMousePosition = (int)newPoint.Y + yAdjustInt,
                    v_MouseClick = clickType
                };

                //run commands
                newMouseMove.RunCommand(sender);
            }
            else if (v_AutomationType == "Get Value From Element")
            {
                //get value from property
                var propertyName = (from rw in v_UIAActionParameters.AsEnumerable()
                                 where rw.Field<string>("Parameter Name") == "Get Value From"
                                 select rw.Field<string>("Parameter Value")).FirstOrDefault();

                //apply to variable
                var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
                                 where rw.Field<string>("Parameter Name") == "Apply To Variable"
                                 select rw.Field<string>("Parameter Value")).FirstOrDefault();

                //remove brackets from variable
                applyToVariable = applyToVariable.Replace("[", "").Replace("]", "");

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
    #endregion Input Commands

    #region Database Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("Database Commands")]
    [Attributes.ClassAttributes.Description("This command selects data from a database and applies it against a dataset")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'OLEDB' to achieve automation.")]
    public class DatabaseRunQueryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please create a dataset variable name")]
        public string v_DataSetName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the connection string")]
        public string v_ConnectionString { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please provide the query to run")]
        public string v_UserQuery { get; set; }
        public DatabaseRunQueryCommand()
        {
            this.CommandName = "DatabaseRunQueryCommand";
            this.SelectionName = "Run Query";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

            DatasetCommands dataSetCommand = new DatasetCommands();
            DataTable requiredData = dataSetCommand.CreateDataTable(v_ConnectionString.ConvertToUserVariable(sender), v_UserQuery);

            var engine = (Core.AutomationEngineInstance)sender;

            Script.ScriptVariable newDataset = new Script.ScriptVariable
            {
                VariableName = v_DataSetName,
                VariableValue = requiredData
            };

            engine.VariableList.Add(newDataset);

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_UserQuery + "]";
        }
    }

    #endregion

    #region Loop Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to repeat actions continuously.  Any 'Begin Loop' command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    public class BeginContinousLoopCommand : ScriptCommand
    {

        public BeginContinousLoopCommand()
        {
            this.CommandName = "BeginContinousLoopCommand";
            this.SelectionName = "Loop Continuously";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {
            Core.AutomationCommands.BeginContinousLoopCommand loopCommand = (Core.AutomationCommands.BeginContinousLoopCommand)parentCommand.ScriptCommand;

            var engine = (Core.AutomationEngineInstance)sender;


            engine.ReportProgress("Starting Continous Loop From Line " + loopCommand.LineNumber);

            while (true)
            {
       

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress("Exiting Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopCancelled = false;
                        return;
                    }
                }
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to repeat actions several times (loop).  Any 'Begin Loop' command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    public class BeginNumberOfTimesLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Enter how many times to perform the loop")]
        public string v_LoopParameter { get; set; }

        public BeginNumberOfTimesLoopCommand()
        {
            this.CommandName = "BeginNumberOfTimesLoopCommand";
            this.SelectionName = "Loop Number Of Times";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {
            Core.AutomationCommands.BeginNumberOfTimesLoopCommand loopCommand = (Core.AutomationCommands.BeginNumberOfTimesLoopCommand)parentCommand.ScriptCommand;

            var engine = (Core.AutomationEngineInstance)sender;

            int loopTimes;
            Script.ScriptVariable complexVarible = null;

            var loopParameter = loopCommand.v_LoopParameter.ConvertToUserVariable(sender);

            loopTimes = int.Parse(loopParameter);

            for (int i = 0; i < loopTimes; i++)
            {
                if (complexVarible != null)
                    complexVarible.CurrentPosition = i;

                engine.ReportProgress("Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber);

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress("Exiting Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopCancelled = false;
                        return;
                    }
                    
                }

                engine.ReportProgress("Finished Loop From Line " + loopCommand.LineNumber);
            }
        }

        public override string GetDisplayValue()
        {
                return "Loop " + v_LoopParameter + " Times";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to repeat actions several times (loop).  Any 'Begin Loop' command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    public class BeginListLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please input the list variable to be looped")]
        public string v_LoopParameter { get; set; }

        public BeginListLoopCommand()
        {
            this.CommandName = "BeginListLoopCommand";
            this.SelectionName = "Loop List";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {
            Core.AutomationCommands.BeginListLoopCommand loopCommand = (Core.AutomationCommands.BeginListLoopCommand)parentCommand.ScriptCommand;
            var engine = (Core.AutomationEngineInstance)sender;

            int loopTimes;
            Script.ScriptVariable complexVariable = null;


           //get variable by regular name
            complexVariable = engine.VariableList.Where(x => x.VariableName == v_LoopParameter).FirstOrDefault();


            //user may potentially include brackets []
            if (complexVariable == null)
            {
                complexVariable = engine.VariableList.Where(x => x.VariableName.ApplyVariableFormatting() == v_LoopParameter).FirstOrDefault();
            }

            //if still null then throw exception
            if (complexVariable == null)
            {
                throw new Exception("Complex Variable '" + v_LoopParameter + "' or '" + v_LoopParameter.ApplyVariableFormatting() + "' not found. Ensure the variable exists before attempting to modify it.");
            }


                var listToLoop = (List<string>)complexVariable.VariableValue;
                loopTimes = listToLoop.Count();


            for (int i = 0; i < loopTimes; i++)
            {
                if (complexVariable != null)
                    complexVariable.CurrentPosition = i;

                engine.ReportProgress("Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber);

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;
                    engine.ExecuteCommand(cmd);
                }

                engine.ReportProgress("Finished Loop From Line " + loopCommand.LineNumber);
            }
        }

        public override string GetDisplayValue()
        {
                return "Loop List Variable '" + v_LoopParameter + "'";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to loop through an Excel Dataset")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to loop through a known Excel DataSet")]
    public class BeginExcelDatasetLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Excel DataSet Name")]
        public string v_DataSetName { get; set; }

        public BeginExcelDatasetLoopCommand()
        {
            this.CommandName = "BeginExcelDataSetLoopCommand";
            this.SelectionName = "Loop Excel Dataset";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {

            Core.AutomationCommands.BeginExcelDatasetLoopCommand loopCommand = (Core.AutomationCommands.BeginExcelDatasetLoopCommand)parentCommand.ScriptCommand;
            var engine = (Core.AutomationEngineInstance)sender;

            var dataSetVariable = engine.VariableList.Where(f => f.VariableName == v_DataSetName).FirstOrDefault();

            if (dataSetVariable == null)
                throw new Exception("DataSet Name Not Found - " + v_DataSetName);



                DataTable excelTable = (DataTable)dataSetVariable.VariableValue;


                var loopTimes = excelTable.Rows.Count;

            for (int i = 0; i < excelTable.Rows.Count; i++)
            {
                dataSetVariable.CurrentPosition = i;

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    //bgw.ReportProgress(0, new object[] { loopCommand.LineNumber, "Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber });

                    if (engine.IsCancellationPending)
                        return;
                    engine.ExecuteCommand(cmd);
                    // bgw.ReportProgress(0, new object[] { loopCommand.LineNumber, "Finished Loop From Line " + loopCommand.LineNumber });
                }
            }








        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command signifies the exit point of looped (repeated) actions.  Required for all loops.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is used by the serializer to signify the end point of a loop.")]
    public class EndLoopCommand : ScriptCommand
    {
        public EndLoopCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "EndLoopCommand";
            this.SelectionName = "End Loop";
            this.CommandEnabled = true;
        }

        public override string GetDisplayValue()
        {
            return "End Loop";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command signifies the current loop should exit and resume work past the point of the current loop.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is used by the engine to exit a loop")]
    public class ExitLoopCommand : ScriptCommand
    {
        public ExitLoopCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "ExitLoopCommand";
            this.SelectionName = "Exit Loop";
            this.CommandEnabled = true;
        }

        public override string GetDisplayValue()
        {
            return "Exit Loop";
        }
    }
    #endregion Loop Commands

    #region Excel Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to open the Excel Application.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelCreateApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public ExcelCreateApplicationCommand()
        {
            this.CommandName = "ExcelOpenApplicationCommand";
            this.SelectionName = "Create Excel Application";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            var newExcelSession = new Microsoft.Office.Interop.Excel.Application
            {
                Visible = true
            };
            engine.AppInstances.Add(v_InstanceName, newExcelSession);
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to open an existing Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelOpenWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the workbook file path")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }
        public ExcelOpenWorkbookCommand()
        {
            this.CommandName = "ExcelOpenWorkbookCommand";
            this.SelectionName = "Open Workbook";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.Workbooks.Open(v_FilePath);
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Open from '" + v_FilePath + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to add a new Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelAddWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }

        public ExcelAddWorkbookCommand()
        {
            this.CommandName = "ExcelAddWorkbookCommand";
            this.SelectionName = "Add Workbook";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.Workbooks.Add();
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to move to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelGoToCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        public string v_CellLocation { get; set; }
        public ExcelGoToCellCommand()
        {
            this.CommandName = "ExcelGoToCellCommand";
            this.SelectionName = "Go To Cell";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                excelSheet.Range[v_CellLocation].Select();
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Go To: '" + v_CellLocation + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to set the value of a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelSetCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter text to set")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_TextToSet { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ExcelCellAddress { get; set; }
        public ExcelSetCellCommand()
        {
            this.CommandName = "ExcelSetCellCommand";
            this.SelectionName = "Set Cell";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object excelObject))
            {
              var targetAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);
              var targetText = v_TextToSet.ConvertToUserVariable(sender);

                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                excelSheet.Range[targetAddress].Value = targetText;
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set Cell '" + v_ExcelCellAddress + "' to '" + v_TextToSet + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command gets text from a specified Excel Cell and assigns it to a variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    public class ExcelGetCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ExcelCellAddress { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        public string v_userVariableName { get; set; }

        public ExcelGetCellCommand()
        {
            this.CommandName = "ExcelGetCellCommand";
            this.SelectionName = "Get Cell";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object excelObject))
            {

                var targetAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);

                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                var cellValue = (string)excelSheet.Range[targetAddress].Text;
                cellValue.StoreInUserVariable(sender, v_userVariableName);
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get Value From '" + v_ExcelCellAddress + "' and apply to variable '" + v_userVariableName + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to run a macro in an Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelRunMacroCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the macro name")]
        public string v_MacroName { get; set; }
        public ExcelRunMacroCommand()
        {
            this.CommandName = "ExcelAddWorkbookCommand";
            this.SelectionName = "Run Macro";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.Run(v_MacroName);
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to find the last row in a used range in an Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelGetLastRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter Letter of the Column to check (ex. A, B, C)")]
        public string v_ColumnLetter { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the row number")]
        public string v_applyToVariableName { get; set; }
        public ExcelGetLastRowCommand()
        {
            this.CommandName = "ExcelGetLastRowCommand";
            this.SelectionName = "Get Last Row";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object excelObject))
            {

                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                var excelSheet = excelInstance.ActiveSheet;
                var lastRow = (int)excelSheet.Cells(excelSheet.Rows.Count, "A").End(Microsoft.Office.Interop.Excel.XlDirection.xlUp).Row;

         
                lastRow.ToString().StoreInUserVariable(sender, v_applyToVariableName);


            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply to '" + v_applyToVariableName + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelCloseApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate if the Workbook should be saved")]
        public bool v_ExcelSaveOnExit { get; set; }
        public ExcelCloseApplicationCommand()
        {
            this.CommandName = "ExcelCloseApplicationCommand";
            this.SelectionName = "Close Application";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.ActiveWorkbook.Close(v_ExcelSaveOnExit);
                excelInstance.Quit();
            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Save On Close: " + v_ExcelSaveOnExit + ", Instance Name: '" + v_InstanceName + "']";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to switch worksheet tabs")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelActivateSheetCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the name of the sheet within the Workbook to activate")]
        public string v_SheetName { get; set; }
        public ExcelActivateSheetCommand()
        {
            this.CommandName = "ExcelActivateSheetCommand";
            this.SelectionName = "Activate Sheet";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object excelObject))
            {
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                string sheetToDelete = v_SheetName.ConvertToUserVariable(sender);
                Microsoft.Office.Interop.Excel.Worksheet workSheet = excelInstance.Sheets[sheetToDelete];
                workSheet.Select();
              


            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Sheet Name: " + v_SheetName + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to delete a specified row in Excel")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelDeleteRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the row number to delete")]
        public string v_RowNumber { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        public string v_ShiftUp { get; set; }
        public ExcelDeleteRowCommand()
        {
            this.CommandName = "ExcelDeleteRowCommand";
            this.SelectionName = "Delete Row";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object excelObject))
            {

                
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet workSheet = excelInstance.ActiveSheet;

                string rowToDelete = v_RowNumber.ConvertToUserVariable(sender);

               var cells = workSheet.Range["A" + rowToDelete, Type.Missing];
                var entireRow = cells.EntireRow;
                if (v_ShiftUp == "Yes")
                {          
                    entireRow.Delete();
                }
                else
                {
                    entireRow.Clear();
                }
           

            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Row Number: " + v_RowNumber + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to delete a specified row in Excel")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelDeleteCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the range to delete ex. A1 or A1:C1")]
        public string v_Range { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Should the cells below shift upward after deletion?")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        public string v_ShiftUp { get; set; }
        public ExcelDeleteCellCommand()
        {
            this.CommandName = "ExcelDeleteCellCommand";
            this.SelectionName = "Delete Cell";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            if (engine.AppInstances.TryGetValue(v_InstanceName, out object excelObject))
            {


                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet workSheet = excelInstance.ActiveSheet;

                string range = v_Range.ConvertToUserVariable(sender);
                var cells = workSheet.Range[range, Type.Missing];


                if (v_ShiftUp == "Yes")
                {  
                    cells.Delete();
                }
                else
                {
                    cells.Clear();
                }
             


            }
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Range: " + v_Range + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command gets a range of cells and applies them against a dataset")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'OLEDB' to achieve automation.")]
    public class ExcelCreateDataSetCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please create a DataSet name")]
        public string v_DataSetName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the workbook file path")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the sheet name")]
        public string v_SheetName { get; set; }

        public ExcelCreateDataSetCommand()
        {
            this.CommandName = "ExcelCreateDatasetCommand";
            this.SelectionName = "Create Dataset";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

           DatasetCommands dataSetCommand = new DatasetCommands();
           DataTable requiredData = dataSetCommand.CreateDataTable(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + v_FilePath + @";Extended Properties=""Excel 12.0;HDR=No;IMEX=1""", "Select * From [" + v_SheetName + "$]");

            var engine = (Core.AutomationEngineInstance)sender;

            Script.ScriptVariable newDataset = new Script.ScriptVariable
            {
                VariableName = v_DataSetName,
                VariableValue = requiredData
            };

            engine.VariableList.Add(newDataset);

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() +  " [Get '" + v_SheetName + "' from '" + v_FilePath + "' and apply to '" + v_DataSetName + "']";
        }
    }

    #endregion Excel Commands

    #region Data Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to modify variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class VariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to modify")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the input to be set to above variable")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Input { get; set; }
        public VariableCommand()
        {
            this.CommandName = "VariableCommand";
            this.SelectionName = "Set Variable";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Core.AutomationEngineInstance)sender;

            var requiredVariable = LookupVariable(engine);

            //if still not found and user has elected option, create variable at runtime
            if ((requiredVariable == null) && (engine.engineSettings.CreateMissingVariablesDuringExecution))
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_userVariableName });
                requiredVariable = LookupVariable(engine);
            }

            if (requiredVariable != null)
            {
                requiredVariable.VariableValue = v_Input.ConvertToUserVariable(sender);
            }
            else
            {
                throw new Exception("Attempted to store data in a variable, but it was not found. Enclose variables within brackets, ex. [vVariable]");
            }
        }

        private Script.ScriptVariable LookupVariable(Core.AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_userVariableName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_userVariableName.StartsWith("[")) && (v_userVariableName.EndsWith("]")))
            {
                //reformat and attempt
                var reformattedVariable = v_userVariableName.Replace("[", "").Replace("]", "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply '" + v_Input + "' to Variable '" + v_userVariableName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to build a date and apply it to a variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class DateCalculationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the date value or variable (ex. [DateTime.Now]")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select a Calculation Method")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Seconds")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Minutes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Hours")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Days")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Add Years")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Subtract Seconds")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Subtract Minutes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Subtract Hours")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Subtract Days")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Subtract Years")]
        public string v_CalculationMethod { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the increment value")]
        public string v_Increment { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Specify String Format")]
        public string v_ToStringFormat { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the date calculation")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_applyToVariableName { get; set; }

        public DateCalculationCommand()
        {
            this.CommandName = "DateCalculationCommand";
            this.SelectionName = "Date Calculation";
            this.CommandEnabled = true;

            this.v_InputValue = "[DateTime.Now]";
            this.v_ToStringFormat = "MM/dd/yyyy hh:mm:ss";

        }

        public override void RunCommand(object sender)
        {
            //get variablized string
            var variableDateTime = v_InputValue.ConvertToUserVariable(sender);

            //convert to date time
            DateTime requiredDateTime;
            if (!DateTime.TryParse(variableDateTime, out requiredDateTime))
            {
                throw new InvalidDataException("Date was unable to be parsed - " + variableDateTime);
            }

            //get increment value
            double requiredInterval;
            var variableIncrement = v_Increment.ConvertToUserVariable(sender);

            //convert to double
            if (!Double.TryParse(variableIncrement, out requiredInterval))
            {
                throw new InvalidDataException("Date was unable to be parsed - " + variableIncrement);
            }

            //perform operation
            switch (v_CalculationMethod)
            {
                case "Add Seconds":
                    requiredDateTime = requiredDateTime.AddSeconds(requiredInterval);
                    break;
                case "Add Minutes":
                    requiredDateTime = requiredDateTime.AddMinutes(requiredInterval);
                    break;
                case "Add Hours":
                    requiredDateTime = requiredDateTime.AddHours(requiredInterval);
                    break;
                case "Add Days":
                    requiredDateTime = requiredDateTime.AddDays(requiredInterval);
                    break;
                case "Add Years":
                    requiredDateTime = requiredDateTime.AddYears((int)requiredInterval);
                    break;
                case "Subtract Seconds":
                    requiredDateTime = requiredDateTime.AddSeconds((requiredInterval * -1));
                    break;
                case "Subtract Minutes":
                    requiredDateTime = requiredDateTime.AddMinutes((requiredInterval * -1));
                    break;
                case "Subtract Hours":
                    requiredDateTime = requiredDateTime.AddHours((requiredInterval * -1));
                    break;
                case "Subtract Days":
                    requiredDateTime = requiredDateTime.AddDays((requiredInterval * -1));
                    break;
                case "Subtract Years":
                    requiredDateTime = requiredDateTime.AddYears(((int)requiredInterval * -1));
                    break;
                default:
                    break;
            }

            //handle if formatter is required     
            var formatting = v_ToStringFormat.ConvertToUserVariable(sender);
            var stringDateFormatted = requiredDateTime.ToString(formatting);


            //store string in variable
            stringDateFormatted.StoreInUserVariable(sender, v_applyToVariableName);

        }

        public override string GetDisplayValue()
        {
            //if calculation method was selected
            if (v_CalculationMethod != null)
            {
                //determine operand and interval
                var operand = v_CalculationMethod.Split(' ')[0];
                var interval = v_CalculationMethod.Split(' ')[1];

                //additional language handling based on selection made
                string operandLanguage;
                if (operand == "Add")
                {
                    operandLanguage = " to ";
                }
                else
                {
                    operandLanguage = " from ";
                }

                //return value
                return base.GetDisplayValue() + " [" + operand + " " + v_Increment + " " + interval + operandLanguage + v_InputValue + ", Apply Result to Variable '" + v_applyToVariableName + "']";
            }
            else
            {
                return base.GetDisplayValue();
            }

        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to apply formatting to a string")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class FormatDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the value or variable (ex. [DateTime.Now]")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the type of data")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Date")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Number")]

        public string v_FormatType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify required output format")]
        public string v_ToStringFormat { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive output")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_applyToVariableName { get; set; }

        public FormatDataCommand()
        {
            this.CommandName = "FormatDataCommand";
            this.SelectionName = "Format Data";
            this.CommandEnabled = true;

            this.v_InputValue = "[DateTime.Now]";
            this.v_FormatType = "Date";
            this.v_ToStringFormat = "MM/dd/yyyy";

        }

        public override void RunCommand(object sender)
        {
            //get variablized string
            var variableString = v_InputValue.ConvertToUserVariable(sender);
            
            //get formatting
            var formatting = v_ToStringFormat.ConvertToUserVariable(sender);

           var variableName = v_applyToVariableName.ConvertToUserVariable(sender);

             
            string formattedString = "";
            switch (v_FormatType)
            {
                case "Date":
                    if (DateTime.TryParse(variableString, out var parsedDate))
                    {
                        formattedString = parsedDate.ToString(formatting);
                    }
                    break;
                case "Number":
                    if (Decimal.TryParse(variableString, out var parsedDecimal))
                    {
                        formattedString = parsedDecimal.ToString(formatting);
                    }
                    break;
                default:
                    throw new Exception("Formatter Type Not Supported: " + v_FormatType);
            }

            if (formattedString == "")
            {
                throw new InvalidDataException("Unable to convert '" + variableString + "' to type '" + v_FormatType + "'");
            }
            else
            {
                formattedString.StoreInUserVariable(sender, "");
            }

          

        }

        public override string GetDisplayValue()
        {
                return base.GetDisplayValue() + " [Format '" + v_InputValue + "' and Apply Result to Variable '" + v_applyToVariableName + "']";
        }
    }




    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to trim a string")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    public class StringSubstringCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to modify")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Start from Position")]
        public int v_startIndex { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Length (-1 to keep remainder)")]
        public int v_stringLength { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the changes")]
        public string v_applyToVariableName { get; set; }
        public StringSubstringCommand()
        {
            this.CommandName = "StringSubstringCommand";
            this.SelectionName = "Substring";
            this.CommandEnabled = true;
            v_stringLength = -1;
        }
        public override void RunCommand(object sender)
        {
            v_userVariableName = v_userVariableName.ConvertToUserVariable(sender);

            //apply substring
            if (v_stringLength >= 0)
            {
                v_userVariableName = v_userVariableName.Substring(v_startIndex, v_stringLength);
            }
            else
            {
                v_userVariableName = v_userVariableName.Substring(v_startIndex);
            }

            v_userVariableName.StoreInUserVariable(sender, v_applyToVariableName);
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Substring to '" + v_userVariableName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to split a string")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Split method to achieve automation.")]
    public class StringSplitCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to split")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Input Delimiter")]
        public string v_splitCharacter { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the list variable which will contain the results")]
        public string v_applyConvertToUserVariableName { get; set; }
        public StringSplitCommand()
        {
            this.CommandName = "StringSplitCommand";
            this.SelectionName = "Split";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var stringVariable = v_userVariableName.ConvertToUserVariable(sender);

            List<string> splitString;
            if (v_splitCharacter == "[crLF]")
            {
                splitString = stringVariable.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            }
            else if (v_splitCharacter == "[chars]")
            {
                splitString = new List<string>();
                var chars = stringVariable.ToCharArray();
                foreach (var c in chars)
                {
                    splitString.Add(c.ToString());
                }

            }
            else
            {
                splitString = stringVariable.Split(new string[] { v_splitCharacter }, StringSplitOptions.None).ToList();
            }

            var engine = (Core.AutomationEngineInstance)sender;
            var v_receivingVariable = v_applyConvertToUserVariableName.Replace("[", "").Replace("]", "");
            //get complex variable from engine and assign
            var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_receivingVariable).FirstOrDefault();
            requiredComplexVariable.VariableValue = splitString;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Split '" + v_userVariableName + "' by '" + v_splitCharacter + "' and apply to '" + v_applyConvertToUserVariableName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to perform advanced string formatting using RegEx.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class RegExExtractorCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the value or variable (ex. [vSomeVariable])")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Input the RegEx Extractor Pattern")]
        public string v_RegExExtractor { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Matching Group Index")]
        public string v_MatchGroupIndex { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the RegEx result")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_applyToVariableName { get; set; }

        public RegExExtractorCommand()
        {
            this.CommandName = "RegExExtractorCommand";
            this.SelectionName = "RegEx Extraction";
            this.CommandEnabled = true;

            //apply default
            v_MatchGroupIndex = "0";
        }

        public override void RunCommand(object sender)
        {
            //get variablized strings
            var variableInput = v_InputValue.ConvertToUserVariable(sender);
            var variableExtractorPattern = v_RegExExtractor.ConvertToUserVariable(sender);
            var variableMatchGroup = v_MatchGroupIndex.ConvertToUserVariable(sender);

            //create regex matcher
            Regex regex = new Regex(variableExtractorPattern);
            Match match = regex.Match(variableInput);

            int matchGroup = 0;
            if (!int.TryParse(variableMatchGroup, out matchGroup))
            {
                matchGroup = 0;
            }

            if (!match.Success)
            {
                //throw exception if no match found
                throw new Exception("RegEx Match was not found! Input: " + variableInput + ", Pattern: " + variableExtractorPattern);
            }
            else
            {
                //store string in variable
                string matchedValue = match.Groups[matchGroup].Value;
                matchedValue.StoreInUserVariable(sender, v_applyToVariableName);
            }

        

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Extracted Text To Variable: " + v_applyToVariableName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to perform advanced string extraction.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class TextExtractorCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the value or variable requiring extraction (ex. [vSomeVariable])")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select text extraction type")]
        public string v_TextExtractionType { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Extraction Parameters")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_TextExtractionTable { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the extracted text")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_applyToVariableName { get; set; }

        public TextExtractorCommand()
        {
            this.CommandName = "TextExtractorCommand";
            this.SelectionName = "Text Extraction";
            this.CommandEnabled = true;
            //define parameter table
            this.v_TextExtractionTable = new System.Data.DataTable
            {
                TableName = DateTime.Now.ToString("TextExtractorParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            };

            this.v_TextExtractionTable.Columns.Add("Parameter Name");
            this.v_TextExtractionTable.Columns.Add("Parameter Value");

        }

        public override void RunCommand(object sender)
        {
            //get variablized input
            var variableInput = v_InputValue.ConvertToUserVariable(sender);


            string variableLeading, variableTrailing, skipOccurences, extractedText;
 
            //handle extraction cases
            switch (v_TextExtractionType)
            {
                case "Extract All After Text":
                    //extract trailing texts            
                    variableLeading = GetParameterValue("Leading Text").ConvertToUserVariable(sender);
                    skipOccurences = GetParameterValue("Skip Past Occurences").ConvertToUserVariable(sender);
                    extractedText = ExtractLeadingText(variableInput, variableLeading, skipOccurences);
                    break;
                case "Extract All Before Text":
                    //extract leading text
                    variableTrailing = GetParameterValue("Trailing Text").ConvertToUserVariable(sender);
                    skipOccurences = GetParameterValue("Skip Past Occurences").ConvertToUserVariable(sender);
                    extractedText = ExtractTrailingText(variableInput, variableTrailing, skipOccurences);
                    break;
                case "Extract All Between Text":
                    //extract leading and then trailing which gives the items between
                    variableLeading = GetParameterValue("Leading Text").ConvertToUserVariable(sender);
                    variableTrailing = GetParameterValue("Trailing Text").ConvertToUserVariable(sender);
                    skipOccurences = GetParameterValue("Skip Past Occurences").ConvertToUserVariable(sender);

                    //extract leading
                    extractedText = ExtractLeadingText(variableInput, variableLeading, skipOccurences);

                    //extract trailing -- assume we will take to the first item
                    extractedText = ExtractTrailingText(extractedText, variableTrailing, "0");

                    break;
                default:
                    throw new NotImplementedException("Extraction Type Not Implemented: " + v_TextExtractionType);
            }

            //store variable
            extractedText.StoreInUserVariable(sender, v_applyToVariableName);

        }

        private string GetParameterValue(string parameterName)
        {
             return ((from rw in v_TextExtractionTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == parameterName
                           select rw.Field<string>("Parameter Value")).FirstOrDefault());
     
        }
        private string ExtractLeadingText(string input, string substring, string occurences)
        {
            
            //verify the occurence index
            int leadingOccurenceIndex = 0;

            if (!int.TryParse(occurences, out leadingOccurenceIndex))
            {
                throw new Exception("Invalid Index For Extraction - " + occurences);
            }

            //find index matches
            var leadingOccurencesFound = Regex.Matches(input, substring).Cast<Match>().Select(m => m.Index).ToList();

            //handle if we are searching beyond what was found
            if (leadingOccurenceIndex >= leadingOccurencesFound.Count)
            {
                throw new Exception("No value was found after skipping " + leadingOccurenceIndex + " instance(s).  Only " + leadingOccurencesFound.Count + " instances exist.");
            }

            //declare start position
            var startPosition = leadingOccurencesFound[leadingOccurenceIndex] + substring.Length;

            //substring and apply to variable
            return input.Substring(startPosition);


        }
        private string ExtractTrailingText(string input, string substring, string occurences)
        {
            //verify the occurence index
            int leadingOccurenceIndex = 0;
            if (!int.TryParse(occurences, out leadingOccurenceIndex))
            {
                throw new Exception("Invalid Index For Extraction - " + occurences);
            }

            //find index matches
            var trailingOccurencesFound = Regex.Matches(input, substring).Cast<Match>().Select(m => m.Index).ToList();

            //handle if we are searching beyond what was found
            if (leadingOccurenceIndex >= trailingOccurencesFound.Count)
            {
                throw new Exception("No value was found after skipping " + leadingOccurenceIndex + " instance(s).  Only " + trailingOccurencesFound.Count + " instances exist.");
            }

            //declare start position
            var endPosition = trailingOccurencesFound[leadingOccurenceIndex];

            //substring
            return input.Substring(0, endPosition);
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Extracted Text To Variable: " + v_applyToVariableName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time in milliseconds.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class LogDataCommand : ScriptCommand
    {


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select existing log file or enter a custom name.")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Engine Logs")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_LogFile { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the text to log.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_LogText { get; set; }

        public LogDataCommand()
        {
            this.CommandName = "LogDataCommand";
            this.SelectionName = "Log Data";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            //get text to log and log file name       
            var textToLog = v_LogText.ConvertToUserVariable(sender);
            var logFile = v_LogFile.ConvertToUserVariable(sender);

            //determine log file
            if (v_LogFile == "Engine Logs")
            {
                //log to the standard engine logs
                var engine = (Core.AutomationEngineInstance)sender;
                engine.engineLogger.Information(textToLog);
            }
            else
            {
                //create new logger and log to custom file
                using (var logger = new Core.Logging().CreateLogger(logFile, Serilog.RollingInterval.Infinite))
                {
                    logger.Information(textToLog);
                }
            }

           
        }

        public override string GetDisplayValue()
        {
            string logFileName;
            if (v_LogFile == "Engine Logs")
            {
                logFileName = "taskt Engine Logs.txt";
            }
            else
            {
                logFileName = "taskt " + v_LogFile + " Logs.txt";
            }


            return base.GetDisplayValue() + " [Write Log to 'taskt\\Logs\\" + logFileName + "']";
        }
    }

    #endregion Data Commands

    #region If Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("If Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to evaluate a logical statement to determine if the statement is true.  Any 'BeginIf' command must have a following 'EndIf' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command evaluates supplied arguments and if evaluated to true runs sub elements")]
    public class BeginIfCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select type of If Command")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Value")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Window Name Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Active Window Name Is")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("File Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Folder Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Web Element Exists")]
        public string v_IfActionType { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_IfActionParameterTable { get; set; }

        public BeginIfCommand()
        {
            this.CommandName = "BeginIfCommand";
            this.SelectionName = "Begin If";
            this.CommandEnabled = true;

            //define parameter table
            this.v_IfActionParameterTable = new System.Data.DataTable
            {
                TableName = DateTime.Now.ToString("IfActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"))
            };
            this.v_IfActionParameterTable.Columns.Add("Parameter Name");
            this.v_IfActionParameterTable.Columns.Add("Parameter Value");
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {
            var engine = (Core.AutomationEngineInstance)sender;



            bool ifResult = false;


            if (v_IfActionType == "Value")
            {
                string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value1"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Operand"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault());
                string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Value2"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

                value1 = value1.ConvertToUserVariable(sender);
                value2 = value2.ConvertToUserVariable(sender);



                decimal cdecValue1, cdecValue2;

                switch (operand)
                {
                    case "is equal to":
                        ifResult = (value1 == value2);
                        break;

                    case "is not equal to":
                        ifResult = (value1 != value2);
                        break;

                    case "is greater than":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 > cdecValue2);
                        break;

                    case "is greater than or equal to":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 >= cdecValue2);
                        break;

                    case "is less than":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 < cdecValue2);
                        break;

                    case "is less than or equal to":
                        cdecValue1 = Convert.ToDecimal(value1);
                        cdecValue2 = Convert.ToDecimal(value2);
                        ifResult = (cdecValue1 <= cdecValue2);
                        break;
                }
            }
            else if (v_IfActionType == "Window Name Exists")
            {
                //get user supplied name
                string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Window Name"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());
                //variable translation
                string variablizedWindowName = windowName.ConvertToUserVariable(sender);

                //search for window
                IntPtr windowPtr = User32Functions.FindWindow(variablizedWindowName);

                //conditional
                if (windowPtr != IntPtr.Zero)
                {
                    ifResult = true;
                }



            }
            else if (v_IfActionType == "Active Window Name Is")
            {
                string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Window Name"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string variablizedWindowName = windowName.ConvertToUserVariable(sender);

                var currentWindowTitle = User32Functions.GetActiveWindowTitle();

                if (currentWindowTitle == variablizedWindowName)
                {
                    ifResult = true;
                }

            }
            else if (v_IfActionType == "File Exists")
            {

                string fileName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "File Path"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string trueWhenFileExists = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "True When"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                var userFileSelected = fileName.ConvertToUserVariable(sender);

                bool existCheck = false;
                if (trueWhenFileExists == "It Does Exist")
                {
                    existCheck = true;
                }


                if (System.IO.File.Exists(userFileSelected) == existCheck)
                {
                    ifResult = true;
                }


            }
            else if (v_IfActionType == "Folder Exists")
            {
                string folderName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                    where rw.Field<string>("Parameter Name") == "Folder Path"
                                    select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string trueWhenFileExists = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                              where rw.Field<string>("Parameter Name") == "True When"
                                              select rw.Field<string>("Parameter Value")).FirstOrDefault());

                var userFolderSelected = folderName.ConvertToUserVariable(sender);

                bool existCheck = false;
                if (trueWhenFileExists == "It Does Exist")
                {
                    existCheck = true;
                }


                if (System.IO.Directory.Exists(folderName) == existCheck)
                {
                    ifResult = true;
                }
    
            }
            else if(v_IfActionType == "Web Element Exists")
            {
                string parameterName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string searchMethod = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Element Search Method"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                SeleniumBrowserElementActionCommand newElementActionCommand = new SeleniumBrowserElementActionCommand();
                bool elementExists = newElementActionCommand.ElementExists(sender, searchMethod, parameterName);
                ifResult = elementExists;
              
            }
            else
            {
                throw new Exception("If type not recognized!");
            }
  


                int startIndex, endIndex, elseIndex;
                if (parentCommand.AdditionalScriptCommands.Any(item => item.ScriptCommand is Core.AutomationCommands.ElseCommand))
                {
                    elseIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is Core.AutomationCommands.ElseCommand);

                    if (ifResult)
                    {
                        startIndex = 0;
                        endIndex = elseIndex;
                    }
                    else
                    {
                        startIndex = elseIndex + 1;
                        endIndex = parentCommand.AdditionalScriptCommands.Count;
                    }
                }
                else if (ifResult)
                {
                    startIndex = 0;
                    endIndex = parentCommand.AdditionalScriptCommands.Count;
                }
                else
                {
                    return;
                }

                for (int i = startIndex; i < endIndex; i++)
                {
                    if ((engine.IsCancellationPending) || (engine.CurrentLoopCancelled))
                        return;

                    engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[i]);
                }
            





        }

        public override string GetDisplayValue()
        {
            switch (v_IfActionType)
            {
                case "Value":

                    string value1 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Value1"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());
                    string operand = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Operand"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault());
                    string value2 = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Value2"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If (" + value1 + " " + operand + " " + value2 + ")";
                case "Window Name Exists":
                case "Active Window Name Is":

                    string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Window Name"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If " + v_IfActionType + " [Name: " + windowName + "]";
                case "File Exists":

                    string filePath = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "File Path"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string fileCompareType = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "True When"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault());


                    return "If " + v_IfActionType + " [File: " + filePath + "]";
                 
                case "Folder Exists":

                    string folderPath = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Folder Path"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string folderCompareType = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "True When"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault());


                    return "If " + v_IfActionType + " [Folder: " + folderPath + "]";

                case "Web Element Exists":


                    string parameterName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                                 where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string searchMethod = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                             where rw.Field<string>("Parameter Name") == "Element Search Method"
                                             select rw.Field<string>("Parameter Value")).FirstOrDefault());


                    return "If Web Element Exists [" + searchMethod +": " + parameterName + "]";

          
                default:

                    return "If .... ";
            }

        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("If Commands")]
    [Attributes.ClassAttributes.Description("This command signifies the exit point of If actions.  Required for all Begin Ifs.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is used by the serializer to signify the end point of an if.")]
    public class EndIfCommand : ScriptCommand
    {
        public EndIfCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "EndIfCommand";
            this.SelectionName = "End If";
            this.CommandEnabled = true;
        }

        public override string GetDisplayValue()
        {
            return "End If";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("If Commands")]
    [Attributes.ClassAttributes.Description("TBD")]
    [Attributes.ClassAttributes.ImplementationDescription("TBD")]
    public class ElseCommand : ScriptCommand
    {
        public ElseCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "ElseCommand";
            this.SelectionName = "Else";
            this.CommandEnabled = true;
        }

        public override string GetDisplayValue()
        {
            return "Else";
        }
    }

    #endregion If Commands

    #region OCR and Image Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Image Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to covert an image file into text for parsing.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command has a dependency on and implements OneNote OCR to achieve automation.")]
    public class OCRCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Image to OCR")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply OCR Result To Variable")]
        public string v_userVariableName { get; set; }
        public OCRCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "OCRCommand";
            this.SelectionName = "Perform OCR";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;

            var ocrEngine = new OneNoteOCRDll.OneNoteOCR();
            var arr = ocrEngine.OcrTexts(v_FilePath.ConvertToUserVariable(engine)).ToArray();

            string endResult = "";
            foreach (var text in arr)
            {
                endResult += text.Text;
            }

            //apply to user variable
            endResult.StoreInUserVariable(sender, v_userVariableName);
        }

        public override string GetDisplayValue()
        {
            return "OCR '" + v_FilePath + "' and apply result to '" + v_userVariableName + "'";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Image Commands")]
    [Attributes.ClassAttributes.Description("This command takes a screenshot and saves it to a location")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements User32 CaptureWindow to achieve automation")]
    public class ScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name")]
        public string v_ScreenshotWindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to save the image")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }
        public ScreenshotCommand()
        {
            this.CommandName = "ScreenshotCommand";
            this.SelectionName = "Take Screenshot";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            var image = User32Functions.CaptureWindow(v_ScreenshotWindowName);
            string ConvertToUserVariabledString = v_FilePath.ConvertToUserVariable(sender);
            image.Save(ConvertToUserVariabledString);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: '" + v_ScreenshotWindowName + "', File Path: '" + v_FilePath + "]";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Image Commands")]
    [Attributes.ClassAttributes.Description("This command attempts to find an existing image on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("TBD")]
    public class ImageRecognitionCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Capture the search image")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowImageRecogitionHelper)]
        public string v_ImageCapture { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Offset X Coordinate - Optional")]
        public int v_xOffsetAdjustment { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Offset Y Coordinate - Optional")]
        public int v_YOffsetAdjustment { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate mouse click type if required")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("None")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Click")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Down")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Left Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Middle Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Right Up")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Double Left Click")]
        public string v_MouseClick { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Timeout (seconds, 0 for unlimited search time)")]
        public double v_TimeoutSeconds { get; set; }
        public ImageRecognitionCommand()
        {
            this.CommandName = "ImageRecognitionCommand";
            this.SelectionName = "Image Recognition";
            this.CommandEnabled = true;

            v_xOffsetAdjustment = 0;
            v_YOffsetAdjustment = 0;
            v_TimeoutSeconds = 30;
        }
        public override void RunCommand(object sender)
        {
            bool testMode = false;
            if (sender is UI.Forms.frmCommandEditor)
            {
                testMode = true;
            }


            //user image to bitmap
            Bitmap userImage = new Bitmap(Core.Common.Base64ToImage(v_ImageCapture));

            //take screenshot
            Size shotSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
            Point upperScreenPoint = new Point(0, 0);
            Point upperDestinationPoint = new Point(0, 0);
            Bitmap desktopImage = new Bitmap(shotSize.Width, shotSize.Height);
            Graphics graphics = Graphics.FromImage(desktopImage);
            graphics.CopyFromScreen(upperScreenPoint, upperDestinationPoint, shotSize);

            //create desktopOutput file
            Bitmap desktopOutput = new Bitmap(desktopImage);

            //get graphics for drawing on output file
            Graphics screenShotUpdate = Graphics.FromImage(desktopOutput);

            //declare maximum boundaries
            int userImageMaxWidth = userImage.Width - 1;
            int userImageMaxHeight = userImage.Height - 1;
            int desktopImageMaxWidth = desktopImage.Width - 1;
            int desktopImageMaxHeight = desktopImage.Height - 1;

            //newfingerprinttechnique

            //create desktopOutput file
            Bitmap sampleOut = new Bitmap(userImage);

            //get graphics for drawing on output file
            Graphics sampleUpdate = Graphics.FromImage(sampleOut);

            List<ImageRecognitionFingerPrint> uniqueFingerprint = new List<ImageRecognitionFingerPrint>();
            Color lastcolor = Color.Transparent;

            //create fingerprint
            var pixelDensity = (userImage.Width * userImage.Height);

            int iteration = 0;
            Random random = new Random();
            while ((uniqueFingerprint.Count() < 10) && (iteration < pixelDensity))
            {
                int x = random.Next(userImage.Width);
                int y = random.Next(userImage.Height);
                Color color = sampleOut.GetPixel(x, y);

                if ((lastcolor != color) && (!uniqueFingerprint.Any(f => f.xLocation == x && f.yLocation == y)))
                {
                    uniqueFingerprint.Add(new ImageRecognitionFingerPrint() { PixelColor = color, xLocation = x, yLocation = y });
                    sampleUpdate.DrawRectangle(Pens.Yellow, x, y, 1, 1);
                }

                iteration++;
            }




            //begin search
            DateTime timeoutDue = DateTime.Now.AddSeconds(v_TimeoutSeconds);


            bool imageFound = false;
            //for each row on the screen
            for (int rowPixel = 0; rowPixel < desktopImage.Height - 1; rowPixel++)
            {

                if (rowPixel + uniqueFingerprint.First().yLocation >= desktopImage.Height)
                    continue;


                //for each column on screen
                for (int columnPixel = 0; columnPixel < desktopImage.Width - 1; columnPixel++)
                {

                    if ((v_TimeoutSeconds > 0) && (DateTime.Now > timeoutDue))
                    {
                        throw new Exception("Image recognition command ran out of time searching for image");
                    }

                    if (columnPixel + uniqueFingerprint.First().xLocation >= desktopImage.Width)
                        continue;



                    try
                    {




                        //get the current pixel from current row and column
                        // userImageFingerPrint.First() for now will always be from top left (0,0)
                        var currentPixel = desktopImage.GetPixel(columnPixel + uniqueFingerprint.First().xLocation, rowPixel + uniqueFingerprint.First().yLocation);

                        //compare to see if desktop pixel matches top left pixel from user image
                        if (currentPixel == uniqueFingerprint.First().PixelColor)
                        {

                            //look through each item in the fingerprint to see if offset pixel colors match
                            int matchCount = 0;
                            for (int item = 0; item < uniqueFingerprint.Count; item++)
                            {
                                //find pixel color from offset X,Y relative to current position of row and column
                                currentPixel = desktopImage.GetPixel(columnPixel + uniqueFingerprint[item].xLocation, rowPixel + uniqueFingerprint[item].yLocation);

                                //if color matches
                                if (uniqueFingerprint[item].PixelColor == currentPixel)
                                {
                                    matchCount++;

                                    //draw on output to demonstrate finding
                                    if (testMode)
                                        screenShotUpdate.DrawRectangle(Pens.Blue, columnPixel + uniqueFingerprint[item].xLocation, rowPixel + uniqueFingerprint[item].yLocation, 5, 5);
                                }
                                else
                                {
                                    //mismatch in the pixel series, not a series of matching coordinate
                                    //?add threshold %?
                                    imageFound = false;

                                    //draw on output to demonstrate finding
                                    if (testMode)
                                        screenShotUpdate.DrawRectangle(Pens.OrangeRed, columnPixel + uniqueFingerprint[item].xLocation, rowPixel + uniqueFingerprint[item].yLocation, 5, 5);
                                }

                            }

                            if (matchCount == uniqueFingerprint.Count())
                            {
                                imageFound = true;

                                var topLeftX = columnPixel;
                                var topLeftY = rowPixel;

                                if (testMode)
                                {
                                    //draw on output to demonstrate finding
                                    var Rectangle = new Rectangle(topLeftX, topLeftY, userImageMaxWidth, userImageMaxHeight);
                                    Brush brush = new SolidBrush(Color.ForestGreen);
                                    screenShotUpdate.FillRectangle(brush, Rectangle);
                                }

                                //move mouse to position
                                var mouseMove = new SendMouseMoveCommand
                                {
                                    v_XMousePosition = topLeftX + (v_xOffsetAdjustment),
                                    v_YMousePosition = topLeftY + (v_xOffsetAdjustment),
                                    v_MouseClick = v_MouseClick
                                };

                                mouseMove.RunCommand(sender);


                            }



                        }


                        if (imageFound)
                            break;

                    }
                    catch (Exception)
                    {
                        //continue
                    }
                }


                if (imageFound)
                    break;
            }













            if (testMode)
            {
                //screenShotUpdate.FillRectangle(Brushes.White, 5, 20, 275, 105);
                //screenShotUpdate.DrawString("Blue = Matching Point", new Font("Arial", 12, FontStyle.Bold), Brushes.SteelBlue, 5, 20);
               // screenShotUpdate.DrawString("OrangeRed = Mismatched Point", new Font("Arial", 12, FontStyle.Bold), Brushes.SteelBlue, 5, 60);
               // screenShotUpdate.DrawString("Green Rectangle = Match Area", new Font("Arial", 12, FontStyle.Bold), Brushes.SteelBlue, 5, 100);

                //screenShotUpdate.DrawImage(sampleOut, desktopOutput.Width - sampleOut.Width, 0);

                UI.Forms.Supplement_Forms.frmImageCapture captureOutput = new UI.Forms.Supplement_Forms.frmImageCapture();
                captureOutput.pbTaggedImage.Image = sampleOut;
                captureOutput.pbSearchResult.Image = desktopOutput;
                captureOutput.Show();
                captureOutput.TopMost = true;
                //captureOutput.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            }

            graphics.Dispose();
            userImage.Dispose();
            desktopImage.Dispose();
            screenShotUpdate.Dispose();

            if (!imageFound)
            {
                throw new Exception("Specified image was not found in window!");
            }


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Find On Screen]";
        }
    }



    #endregion OCR and Image Commands

    #region HTTP Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("WebAPI Commands")]
    [Attributes.ClassAttributes.Description("This command downloads the HTML source of a web page for parsing")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements System.Web to achieve automation")]
    public class HTTPRequestCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL")]
        public string v_WebRequestURL { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply Result To Variable")]
        public string v_userVariableName { get; set; }

        public HTTPRequestCommand()
        {
            this.CommandName = "HTTPRequestCommand";
            this.SelectionName = "HTTP Request";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(v_WebRequestURL.ConvertToUserVariable(sender));
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string strResponse = reader.ReadToEnd();

            strResponse.StoreInUserVariable(sender, v_userVariableName);

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target URL: '" + v_WebRequestURL + "' and apply result to '" + v_userVariableName + "']";
        }

    }

        [Serializable]
        [Attributes.ClassAttributes.Group("WebAPI Commands")]
        [Attributes.ClassAttributes.Description("This command processes an HTML source object")]
        [Attributes.ClassAttributes.ImplementationDescription("TBD")]
        public class HTTPQueryResultCommand : ScriptCommand
        {
            [XmlAttribute]
            [Attributes.PropertyAttributes.PropertyDescription("Select variable containing HTML")]
            public string v_userVariableName { get; set; }

            [XmlAttribute]
            [Attributes.PropertyAttributes.PropertyDescription("XPath Query")]
            public string v_xPathQuery { get; set; }

            [XmlAttribute]
            [Attributes.PropertyAttributes.PropertyDescription("Apply Query Result To Variable")]
            public string v_applyToVariableName { get; set; }

            public HTTPQueryResultCommand()
            {
                this.CommandName = "HTTPRequestQueryCommand";
                this.SelectionName = "HTTP Result Query";
                this.CommandEnabled = true;
            }

            public override void RunCommand(object sender)
            {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(v_userVariableName.ConvertToUserVariable(sender));

            var div = doc.DocumentNode.SelectSingleNode(v_xPathQuery);
            string divString = div.InnerText;

            divString.StoreInUserVariable(sender, v_applyToVariableName);


        }

            public override string GetDisplayValue()
            {
                return base.GetDisplayValue() + " [Query Variable '" + v_userVariableName + "' and apply result to '" + v_applyToVariableName + "']";
            }
        }

    #endregion

    #region Task Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("Task Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to stop the script from executing.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class StopTaskCommand : ScriptCommand
    {


        public StopTaskCommand()
        {
            this.CommandName = "StopTaskCommand";
            this.SelectionName = "Stop Current Task";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Task Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to stop the script from executing.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class RunTaskCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select a Task")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_taskPath { get; set; }

        public RunTaskCommand()
        {
            this.CommandName = "RunTaskCommand";
            this.SelectionName = "Run Task";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            //get assembly reference
            var assembly = System.Reflection.Assembly.GetEntryAssembly().Location;

            //get variable
            var startFile = v_taskPath.ConvertToUserVariable(sender);
            startFile = @"""" + startFile + @"""";

            //start process
            var p = System.Diagnostics.Process.Start(assembly, startFile);
            
            //wait for exit
            p.WaitForExit();

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_taskPath + "]";
        }
    }


    #endregion

    #region Text File Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("Text File Commands")]
    [Attributes.ClassAttributes.Description("This command writes specified data to a text file")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class WriteTextFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the text to be written")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_TextToWrite { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select overwrite option")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Append")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Overwrite")]
        public string v_Overwrite { get; set; }
        public WriteTextFileCommand()
        {
            this.CommandName = "WriteTextFileCommand";
            this.SelectionName = "Write To File";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            //convert variables
            var filePath = v_FilePath.ConvertToUserVariable(sender);
            var outputText = v_TextToWrite.ConvertToUserVariable(sender);

            //append or overwrite as necessary
            if (v_Overwrite == "Append")
            {
                System.IO.File.AppendAllText(filePath, outputText);
            }
            else
            {
                System.IO.File.WriteAllText(filePath, outputText);
            }

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_Overwrite + " to '" + v_FilePath + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Text File Commands")]
    [Attributes.ClassAttributes.Description("This command reads text data into a variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class ReadTextFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define where the text should be stored")]
        public string v_userVariableName { get; set; }


        public ReadTextFileCommand()
        {
            this.CommandName = "ReadTextFileCommand";
            this.SelectionName = "Read Text File";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            //convert variables
            var filePath = v_FilePath.ConvertToUserVariable(sender);
            //read text from file
            var textFromFile = System.IO.File.ReadAllText(filePath);
            //assign text to user variable
            textFromFile.StoreInUserVariable(sender, v_userVariableName);              
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Read from '" + v_FilePath + "']";
        }
    }
    #endregion

    #region File Operation Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command moves a file to a specified destination")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class MoveFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate whether to move or copy the file")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Move File")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Copy File")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the directory to copy to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Create folder if destination does not exist")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        public string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Delete file if it already exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        public string v_DeleteExisting { get; set; }


        public MoveFileCommand()
        {
            this.CommandName = "MoveFileCommand";
            this.SelectionName = "Move/Copy File";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

            //apply variable logic
            var sourceFile = v_SourceFilePath.ConvertToUserVariable(sender);
            var destinationFolder = v_DestinationDirectory.ConvertToUserVariable(sender);

            if ((v_CreateDirectory == "Yes") && (!System.IO.Directory.Exists(destinationFolder)))
            {
                System.IO.Directory.CreateDirectory(destinationFolder); 
            }

            //get source file name and info
            System.IO.FileInfo sourceFileInfo = new FileInfo(sourceFile);

            //create destination
            var destinationPath = System.IO.Path.Combine(destinationFolder, sourceFileInfo.Name);

            //delete if it already exists per user
            if (v_DeleteExisting == "Yes")
            {
                System.IO.File.Delete(destinationPath);
            }

            if (v_OperationType == "Move File")
            {
                //move file
                System.IO.File.Move(sourceFile, destinationPath);
            }
            else
            {
                //copy file
                System.IO.File.Copy(sourceFile, destinationPath);
            }
         
           
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_OperationType + " from '" + v_SourceFilePath + "' to '" + v_DestinationDirectory + "']";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command deletes a file from a specified destination")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class DeleteFileCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_SourceFilePath { get; set; }



        public DeleteFileCommand()
        {
            this.CommandName = "DeleteFileCommand";
            this.SelectionName = "Delete File";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

            //apply variable logic
            var sourceFile = v_SourceFilePath.ConvertToUserVariable(sender);
   
             //delete file
             System.IO.File.Delete(sourceFile);

            }


        
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [delete " + v_SourceFilePath + "']";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command moves a file to a specified destination")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class RenameFileCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the new file name (with extension)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_NewName { get; set; }

       
        public RenameFileCommand()
        {
            this.CommandName = "RenameFileCommand";
            this.SelectionName = "Rename File";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

         //apply variable logic
         var sourceFile = v_SourceFilePath.ConvertToUserVariable(sender);
         var newFileName = v_NewName.ConvertToUserVariable(sender);
            
         //get source file name and info
         System.IO.FileInfo sourceFileInfo = new FileInfo(sourceFile);

          //create destination
          var destinationPath = System.IO.Path.Combine(sourceFileInfo.DirectoryName, newFileName);
       
          //rename file
          System.IO.File.Move(sourceFile, destinationPath);

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [rename " + v_SourceFilePath + " to '" + v_NewName + "']";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.Description("This command waits for a file to exist at a specified destination")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class WaitForFileToExistCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the directory of the file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FileName { get; set; }


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate how many seconds to wait for the file to exist")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WaitTime { get; set; }

        public WaitForFileToExistCommand()
        {
            this.CommandName = "WaitForFileToExistCommand";
            this.SelectionName = "Wait For File";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

            //convert items to variables
            var fileName = v_FileName.ConvertToUserVariable(sender);
            var pauseTime = int.Parse(v_WaitTime.ConvertToUserVariable(sender));

            //determine when to stop waiting based on user config
            var stopWaiting = DateTime.Now.AddSeconds(pauseTime);

            //initialize flag for file found
            var fileFound = false;


            //while file has not been found
            while (!fileFound)
            {

                //if file exists at the file path
                if (System.IO.File.Exists(fileName))
                {
                    fileFound = true;
                }

                //test if we should exit and throw exception
                if (DateTime.Now > stopWaiting)
                {
                    throw new Exception("File was not found in time!");
                }

                //put thread to sleep before iterating
                System.Threading.Thread.Sleep(100);
            }
           



        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [file: " + v_FileName + ", wait " + v_WaitTime + "s]";
        }
    }
    #endregion

}







