//Copyright (c) 2019 Jason Bayldon
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
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using AcroPDFLib;

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
    [XmlInclude(typeof(AddVariableCommand))]
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
    [XmlInclude(typeof(ExcelSaveAsCommand))]
    [XmlInclude(typeof(ExcelSaveCommand))]
    [XmlInclude(typeof(SeleniumBrowserCreateCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateURLCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateForwardCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateBackCommand))]
    [XmlInclude(typeof(SeleniumBrowserRefreshCommand))]
    [XmlInclude(typeof(SeleniumBrowserCloseCommand))]
    [XmlInclude(typeof(SeleniumBrowserElementActionCommand))]
    [XmlInclude(typeof(SeleniumBrowserExecuteScriptCommand))]
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
    [XmlInclude(typeof(StringReplaceCommand))]
    [XmlInclude(typeof(ExecuteDLLCommand))]
    [XmlInclude(typeof(ParseJsonCommand))]
    [XmlInclude(typeof(SetEngineDelayCommand))]
    [XmlInclude(typeof(PDFTextExtractionCommand))]
    [XmlInclude(typeof(UserInputCommand))]
    [XmlInclude(typeof(GetWordCountCommand))]
    [XmlInclude(typeof(HTMLInputCommand))]
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
        [Attributes.PropertyAttributes.InputSpecification("Optional field to enter a custom comment which could potentially describe this command or the need for this command, if required")]
        [Attributes.PropertyAttributes.SampleUsage("I am using this command to ...")]
        [Attributes.PropertyAttributes.Remarks("Optional")]
        public string v_Comment { get; set; }
        [XmlAttribute]
        public bool CommandEnabled { get; set; }

        public ScriptCommand()
        {
            this.DisplayForeColor = System.Drawing.Color.SteelBlue;
            this.CommandEnabled = false;
            this.DefaultPause = 0;
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
                            v_XMousePosition = ((elementXposition + ieClientLocation.left + 10) + userXAdjust).ToString(), // + 10 gives extra padding
                            v_YMousePosition = ((elementYposition + ieClientLocation.top + 90) + userYAdjust).ToString(), // +90 accounts for title bar height
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
    [Attributes.ClassAttributes.Description("This command allows you to create a new Selenium web browser session which enables automation for websites.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a browser that will eventually perform web automation such as checking an internal company intranet site to retrieve data")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserCreateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Signifies a unique name that will represemt the application instance.  This unique name allows you to refer to the instance by name in future commands, ensuring that the commands you specify run against the correct application.")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Instance Tracking (after task ends)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Forget Instance")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Keep Instance Alive")]
        [Attributes.PropertyAttributes.InputSpecification("Specify if taskt should remember this instance name after the script has finished executing.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Forget Instance** to forget the instance or **Keep Instance Alive** to allow subsequent tasks to call the instance by name.")]
        [Attributes.PropertyAttributes.Remarks("Calling the **Close Browser** command or ending the browser session will end the instance.  This command only works during the lifetime of the application.  If the application is closed, the references will be forgetten automatically.")]
        public string v_InstanceTracking { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select a Window State")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Normal")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Maximize")]
        [Attributes.PropertyAttributes.InputSpecification("Select the window state that the browser should start up with.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Normal** to start the browser in normal mode or **Maximize** to start the browser in maximized mode.")]
        [Attributes.PropertyAttributes.Remarks("")]
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

            engine.AddAppInstance(instanceName, newSeleniumSession);


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
    [Attributes.ClassAttributes.Description("This command allows you to navigate a Selenium web browser session to a given URL or resource.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to navigate an existing Selenium instance to a known URL or web resource")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateURLCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL to navigate to")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the destination URL that you want the selenium instance to navigate to")]
        [Attributes.PropertyAttributes.SampleUsage("https://mycompany.com/orders")]
        [Attributes.PropertyAttributes.Remarks("")]
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

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);

   
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().GoToUrl(v_URL.ConvertToUserVariable(sender));

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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to simulate a forward click in the web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateForwardCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
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

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);

     
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Forward();
           
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to navigate backwards in a Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to simulate a back click in the web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserNavigateBackCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
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

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);
            
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Back();
          
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to refresh a Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to simulate a browser refresh click in the web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserRefreshCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
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

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);

            
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Navigate().Refresh();
            
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close and end a web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserCloseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
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

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);

          
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                seleniumInstance.Quit();
                seleniumInstance.Dispose();

            engine.RemoveAppInstance(vInstance);
           
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close a Selenium web browser session.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to manipulate, set, or get data on a webpage within the web browser.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserElementActionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Browser** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Browser** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Element Search Method")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By XPath")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By ID")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Tag Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By Class Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Element By CSS Selector")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By XPath")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By ID")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By Tag Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By Class Name")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Find Elements By CSS Selector")]
        [Attributes.PropertyAttributes.InputSpecification("Select the specific search type that you want to use to isolate the element in the web page.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Find Element By XPath**, **Find Element By ID**, **Find Element By Name**, **Find Element By Tag Name**, **Find Element By Class Name**, **Find Element By CSS Selector**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SeleniumSearchType { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Element Search Parameter")]
        [Attributes.PropertyAttributes.InputSpecification("Specifies the parameter text that matches to the element based on the previously selected search type.")]
        [Attributes.PropertyAttributes.SampleUsage("If search type **Find Element By ID** was specified, for example, given <div id='name'></div>, the value of this field would be **name**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
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
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Get Matching Elements")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Wait For Element To Exist")]
        [Attributes.PropertyAttributes.InputSpecification("Select the appropriate corresponding action to take once the element has been located")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Invoke Click**, **Left Click**, **Right Click**, **Middle Click**, **Double Left Click**, **Clear Element**, **Set Text**, **Get Text**, **Get Attribute**, **Wait For Element To Exist**")]
        [Attributes.PropertyAttributes.Remarks("Selecting this field changes the parameters that will be required in the next step")]
        public string v_SeleniumElementAction { get; set; }
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        [Attributes.PropertyAttributes.InputSpecification("Additioal Parameters will be required based on the action settings selected.")]
        [Attributes.PropertyAttributes.SampleUsage("Additional Parameters range from adding offset coordinates to specifying a variable to apply element text to.")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            //convert to user variable -- https://github.com/saucepleez/taskt/issues/66
            var seleniumSearchParam = v_SeleniumSearchParameter.ConvertToUserVariable(sender);


            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);

            var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;

                dynamic element = null;

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
                            element = FindElement(seleniumInstance, seleniumSearchParam);
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

                    element = FindElement(seleniumInstance, seleniumSearchParam);
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
                        newMouseMove.v_XMousePosition = (seleniumWindowPosition.X + elementLocation.X + 30 + userXAdjust).ToString(); // added 30 for offset
                        newMouseMove.v_YMousePosition = (seleniumWindowPosition.Y + elementLocation.Y + 130 + userYAdjust).ToString(); //added 130 for offset
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
                    case "Get Matching Elements":
                        var variableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                               where rw.Field<string>("Parameter Name") == "Variable Name"
                                               select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == variableName).FirstOrDefault();

                        if (requiredComplexVariable == null)
                        {
                            engine.VariableList.Add(new Script.ScriptVariable() { VariableName = variableName, CurrentPosition = 0 });
                            requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == variableName).FirstOrDefault();
                        }

          
                        //set json settings
                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.Error = (serializer, err) => {
                            err.ErrorContext.Handled = true;
                        };

                      settings.Formatting = Formatting.Indented;

                        //create json list
                        List<string> jsonList = new List<string>();
                        foreach (OpenQA.Selenium.IWebElement item in element)
                        {
                            var json = Newtonsoft.Json.JsonConvert.SerializeObject(item, settings);
                            jsonList.Add(json);          
                        }

                        requiredComplexVariable.VariableValue = jsonList;

                        break;
                    case "Clear Element":
                        element.Clear();
                        break;
                    default:
                        throw new Exception("Element Action was not found");
                }
            
          
        }

        private object FindElement(OpenQA.Selenium.Chrome.ChromeDriver seleniumInstance, string searchParameter)
        {
            object element = null;

            switch (v_SeleniumSearchType)
            {
                case "Find Element By XPath":
                    element = seleniumInstance.FindElementByXPath(searchParameter);
                    break;

                case "Find Element By ID":
                    element = seleniumInstance.FindElementById(searchParameter);
                    break;

                case "Find Element By Name":
                    element = seleniumInstance.FindElementByName(searchParameter);
                    break;

                case "Find Element By Tag Name":
                    element = seleniumInstance.FindElementByTagName(searchParameter);
                    break;

                case "Find Element By Class Name":
                    element = seleniumInstance.FindElementByClassName(searchParameter);
                    break;
                case "Find Element By CSS Selector":
                    element = seleniumInstance.FindElementByCssSelector(searchParameter);
                    break;
                case "Find Elements By XPath":
                    element = seleniumInstance.FindElementsByXPath(searchParameter).ToList();
                    break;

                case "Find Elements By ID":
                    element = seleniumInstance.FindElementsById(searchParameter).ToList();
                    break;

                case "Find Elements By Name":
                    element = seleniumInstance.FindElementsByName(searchParameter).ToList();
                    break;

                case "Find Elements By Tag Name":
                    element = seleniumInstance.FindElementsByTagName(searchParameter).ToList();
                    break;

                case "Find Elements By Class Name":
                    element = seleniumInstance.FindElementsByClassName(searchParameter).ToList();
                    break;

                case "Find Elements By CSS Selector":
                    element = seleniumInstance.FindElementsByCssSelector(searchParameter).ToList();
                    break;

                default:
                    throw new Exception("Search Type was not found");
            }

            return element;
        }

        public bool ElementExists(object sender, string searchType, string elementName)
        {
            //get engine reference
            var engine = (Core.AutomationEngineInstance)sender;
            var seleniumSearchParam = elementName.ConvertToUserVariable(sender);

            //get instance name
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            //get stored app object
            var browserObject = engine.GetAppInstance(vInstance);

            //get selenium instance driver
            var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;

            try
            {
                //search for element
                var element = FindElement(seleniumInstance, seleniumSearchParam);

                //element exists
                return true;
            }
            catch (Exception)
            {
                //element does not exist
                return false;
            }

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_SeleniumSearchType + " and " + v_SeleniumElementAction + ", Instance Name: '" + v_InstanceName + "']";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Web Browser Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to execute a script in a Selenium web browser session.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Selenium to achieve automation.")]
    public class SeleniumBrowserExecuteScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the script code")]
        public string v_ScriptCode { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Supply Arguments")]
        public string v_Args { get; set; }
        public SeleniumBrowserExecuteScriptCommand()
        {
            this.CommandName = "SeleniumBrowserExecuteScriptCommand";
            this.SelectionName = "Execute Script";
            this.v_InstanceName = "default";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var browserObject = engine.GetAppInstance(vInstance);

           
                var script = v_ScriptCode.ConvertToUserVariable(sender);
                var args = v_Args.ConvertToUserVariable(sender);
                var seleniumInstance = (OpenQA.Selenium.Chrome.ChromeDriver)browserObject;
                if (String.IsNullOrEmpty(args))
                {
                    seleniumInstance.ExecuteScript(script);
                }
                else
                {
                    seleniumInstance.ExecuteScript(script, args);
                }

           
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }

    #endregion Web Selenium

    #region Misc Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to add an in-line comment to the script.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add code comments or document code.  Usage of variables (ex. [vVar]) within the comment block will be parsed and displayed when running the script.")]
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
    [Attributes.ClassAttributes.Description("This command allows you to show a message to the user.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to present or display a value on screen to the user.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    public class MessageBoxCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the message to be displayed.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify any text that should be displayed on screen.  You may also include variables for display purposes.")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello World** or **[vMyText]**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Message { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Close After X (Seconds) - 0 to bypass")]
        [Attributes.PropertyAttributes.InputSpecification("Specify how many seconds to display on screen. After the amount of seconds passes, the message box will be automatically closed and script will resume execution.")]
        [Attributes.PropertyAttributes.SampleUsage("**0** to remain open indefinitely or **5** to stay open for 5 seconds.")]
        [Attributes.PropertyAttributes.Remarks("")]
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

            variableMessage = variableMessage.Replace("\\n", Environment.NewLine);

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
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to send email using SMTP protocol.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send an email and have access to SMTP server credentials to generate an email.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements the System.Net Namespace to achieve automation")]
    public class SMTPSendEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Host Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the host/service name that the script should use")]
        [Attributes.PropertyAttributes.SampleUsage("**smtp.gmail.com**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPHost { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Port")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the port number that should be used when contacting the SMTP service")]
        [Attributes.PropertyAttributes.SampleUsage("**587**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public int v_SMTPPort { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Username")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the username to use when contacting the SMTP service")]
        [Attributes.PropertyAttributes.SampleUsage("**username**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPUserName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Password")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the password to use when contacting the SMTP service")]
        [Attributes.PropertyAttributes.SampleUsage("**password**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPPassword { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("From Email")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify how the 'From' field should appear.")]
        [Attributes.PropertyAttributes.SampleUsage("myRobot@company.com")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPFromEmail { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("To Email")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify the destination email that should be addressed.")]
        [Attributes.PropertyAttributes.SampleUsage("jason@company.com")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPToEmail { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Subject")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the text subject (or variable) that the email should have.")]
        [Attributes.PropertyAttributes.SampleUsage("**Alert!** or **[vStatus]**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SMTPSubject { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Body")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify the message that should be sent.")]
        [Attributes.PropertyAttributes.SampleUsage("**Everything ran ok at [DateTime.Now]**")]
        [Attributes.PropertyAttributes.Remarks("")]
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
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to get text from the clipboard.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to copy the data from the clipboard and apply it to a variable.  You can then use the variable to extract the value.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against the VariableList from the scripting engine using System.Windows.Forms.Clipboard.")]
    public class ClipboardGetTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to set clipboard contents")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }

        public ClipboardGetTextCommand()
        {
            this.CommandName = "ClipboardCommand";
            this.SelectionName = "Get Clipboard Text";
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
    #endregion Misc Commands

    #region Window Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command activates a window and brings it to the front.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to active a window by name or bring it to attention.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetForegroundWindow', 'ShowWindow' from user32.dll to achieve automation.")]
    public class ActivateWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
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

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window
            foreach (var targetedWindow in targetWindows)
            {
                User32Functions.SetWindowState(targetedWindow, User32Functions.WindowState.SW_SHOWNORMAL);
                User32Functions.SetForegroundWindow(targetedWindow);
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move an existing window by name to a certain point on the screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class MoveWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to move.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the X position to move the window to.")]
        [Attributes.PropertyAttributes.InputSpecification("Input the new horizontal coordinate of the window, 0 starts at the left and goes to the right")]
        [Attributes.PropertyAttributes.SampleUsage("0")]
        [Attributes.PropertyAttributes.Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        public string v_XWindowPosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Y position to move the window to.")]
        [Attributes.PropertyAttributes.InputSpecification("Input the new vertical coordinate of the window, 0 starts at the top and goes downwards")]
        [Attributes.PropertyAttributes.SampleUsage("0")]
        [Attributes.PropertyAttributes.Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
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

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window
            foreach (var targetedWindow in targetWindows)
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


                User32Functions.SetWindowPosition(targetedWindow, xPos, yPos);
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to reize a window by name to a specific size on screen.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SetWindowPos' from user32.dll to achieve automation.")]
    public class ResizeWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window name")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to resize.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the new window width")]
        [Attributes.PropertyAttributes.InputSpecification("Input the new width size of the window")]
        [Attributes.PropertyAttributes.SampleUsage("0")]
        [Attributes.PropertyAttributes.Remarks("This number is limited by your resolution. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid width range could be 0-1920")]
        public string v_XWindowSize { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the new window height")]
        [Attributes.PropertyAttributes.InputSpecification("Input the new heiht size of the window")]
        [Attributes.PropertyAttributes.SampleUsage("0")]
        [Attributes.PropertyAttributes.Remarks("This number is limited by your resolution. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid height range could be 0-1080")]
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

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window and set the window state
            foreach (var targetedWindow in targetWindows)
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

                User32Functions.SetWindowSize(targetedWindow, xPos, yPos);
            }

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Target Size (" + v_XWindowSize + "," + v_YWindowSize + ")]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command closes an open window.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an existing window by name.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'SendMessage' from user32.dll to achieve automation.")]
    public class CloseWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to close.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
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


            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window
            foreach (var targetedWindow in targetWindows)
            {
                User32Functions.CloseWindow(targetedWindow);
            }
            

           
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command sets a target window's state.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change a window's state to minimized, maximized, or restored state")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class SetWindowStateCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to change.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select a Window State")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Maximize")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Minimize")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Restore")]
        [Attributes.PropertyAttributes.InputSpecification("Select the appropriate window state required")]
        [Attributes.PropertyAttributes.SampleUsage("Choose from **Minimize**, **Maximize** and **Restore**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WindowState { get; set; }

        public SetWindowStateCommand()
        {
            this.CommandName = "SetWindowStateCommand";
            this.SelectionName = "Set Window State";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            //convert window name
            string windowName = v_WindowName.ConvertToUserVariable(sender);

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window and set the window state
            foreach (var targetedWindow in targetWindows)
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

                User32Functions.SetWindowState(targetedWindow, WINDOW_STATE);
            }
        


        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Window: " + v_WindowName + ", Window State: " + v_WindowState + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Window Commands")]
    [Attributes.ClassAttributes.Description("This command waits for a window to exist.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to explicitly wait for a window to exist before continuing script execution.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'FindWindowNative', 'ShowWindow' from user32.dll to achieve automation.")]
    public class WaitForWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Select or Type a window Name")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to wait to exist.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Seconds To Wait")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify how many seconds to wait before an error should be invoked")]
        [Attributes.PropertyAttributes.SampleUsage("**5**")]
        [Attributes.PropertyAttributes.Remarks("")]
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
    #endregion Window Commands

    #region Program/Process Command
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to start a program or a process.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to start applications by entering their name such as 'chrome.exe' or a fully qualified path to a file 'c:/some.exe'")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start'.")]
    public class StartProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the name or path to the program (ex. notepad, calc)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Provide a valid program name or enter a full path to the script/executable including the extension")]
        [Attributes.PropertyAttributes.SampleUsage("**notepad**, **calc**, **c:\\temp\\myapp.exe**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ProgramName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter any arguments (if applicable)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter any arguments or flags if applicable.")]
        [Attributes.PropertyAttributes.SampleUsage(" **-a** or **-version**")]
        [Attributes.PropertyAttributes.Remarks("You will need to consult documentation to determine if your executable supports arguments or flags on startup.")]
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
    [Attributes.ClassAttributes.Description("This command allows you to stop a program or a process.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to close an application by its name such as 'chrome'. Alternatively, you may use the Close Window or Thick App Command instead.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.CloseMainWindow'.")]
    public class StopProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the process name to be stopped (calc, notepad)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Provide the program process name as it appears as a process in Windows Task Manager")]
        [Attributes.PropertyAttributes.SampleUsage("**notepad**, **calc**")]
        [Attributes.PropertyAttributes.Remarks("The program name may vary from the actual process name.  You can use Thick App commands instead to close an application window.")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run a script (such as vbScript, javascript, or executable) but wait for it to close before taskt continues executing.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    public class RunScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the path to the script")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a fully qualified path to the script, including the script extension.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myscript.vbs**")]
        [Attributes.PropertyAttributes.Remarks("This command differs from **Start Process** because this command blocks execution until the script has completed.  If you do not want to stop while the script executes, consider using **Start Process** instead.")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run custom C# code commands.  The code in this command is compiled and run at runtime when this command is invoked.  This command only supports the standard framework classes.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    public class RunCustomCodeCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Paste the C# code to execute")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowCodeBuilder)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the code to be executed or use the builder to create your custom C# code.  The builder contains a Hello World template that you can use to build from.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Code { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply Arguments (optional)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter arguments that the custom code will receive during execution")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Args { get; set; }

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

                var arguments = v_Args.ConvertToUserVariable(sender);
            
                //run code, taskt will wait for the app to exit before resuming
                System.Diagnostics.Process scriptProc = new System.Diagnostics.Process();
                scriptProc.StartInfo.FileName = result.PathToAssembly;
                scriptProc.StartInfo.Arguments = arguments;
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
    #endregion Program/Process Commands

    #region Input Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    public class SendKeysCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter text to send")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text that should be sent to the specified window.")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello, World!** or **[vEntryText]**")]
        [Attributes.PropertyAttributes.Remarks("This command supports sending variables within brackets [vVariable]")]
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
    [Attributes.ClassAttributes.Description("Simulates mouse movements")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to simulate the movement of the mouse, additionally, this command also allows you to perform a click after movement has completed.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'SetCursorPos' function from user32.dll to achieve automation.")]
    public class SendMouseMoveCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the X position to move the mouse to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input the new horizontal coordinate of the mouse, 0 starts at the left and goes to the right")]
        [Attributes.PropertyAttributes.SampleUsage("0")]
        [Attributes.PropertyAttributes.Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1920")]
        public string v_XMousePosition { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the Y position to move the mouse to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowMouseCaptureHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input the new horizontal coordinate of the window, 0 starts at the left and goes down")]
        [Attributes.PropertyAttributes.SampleUsage("0")]
        [Attributes.PropertyAttributes.Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range could be 0-1080")]
        public string v_YMousePosition { get; set; }
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
        [Attributes.PropertyAttributes.InputSpecification("Indicate the type of click required")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Left Click**, **Middle Click**, **Right Click**, **Double Left Click**, **Left Down**, **Middle Down**, **Right Down**, **Left Up**, **Middle Up**, **Right Up** ")]
        [Attributes.PropertyAttributes.Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
        public string v_MouseClick { get; set; }

        public SendMouseMoveCommand()
        {
            this.CommandName = "SendMouseMoveCommand";
            this.SelectionName = "Send Mouse Move";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
  

            var mouseX = v_XMousePosition.ConvertToUserVariable(sender);
            var mouseY = v_YMousePosition.ConvertToUserVariable(sender);



            try
            {
                var xLocation = Convert.ToInt32(Math.Floor(Convert.ToDouble(mouseX)));
                var yLocation = Convert.ToInt32(Math.Floor(Convert.ToDouble(mouseY)));

                User32Functions.SetCursorPosition(xLocation, yLocation);
                User32Functions.SendMouseClick(v_MouseClick, xLocation, yLocation);


            }
            catch (Exception ex)
            {
                throw new Exception("Error parsing input to int type (X: " + v_XMousePosition + ", Y:" + v_YMousePosition + ") " + ex.ToString());
            }

          



        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Coordinates (" + v_XMousePosition + "," + v_YMousePosition + ") Click: " + v_MouseClick + "]";
        }
    }

    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("Command that groups multiple actions")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to group multiple commands together.")]
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
    [Attributes.ClassAttributes.Description("Simulates mouse clicks.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to simulate multiple types of mouse clicks.")]
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
        [Attributes.PropertyAttributes.InputSpecification("Indicate the type of click required")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Left Click**, **Middle Click**, **Right Click**, **Double Left Click**, **Left Down**, **Middle Down**, **Right Down**, **Left Up**, **Middle Up**, **Right Up** ")]
        [Attributes.PropertyAttributes.Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to click a specific item within an application by a window handle.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows UI Automation' to find elements and invokes a SendMouseMove Command to click and achieve automation")]
    public class ThickAppClickItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Window to Automate")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_AutomationWindowName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Appropriate Item")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select one of the valid handles from the window")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("This list is populated after you select which window is required")]
        public string v_AutomationHandleName { get; set; }
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
        [Attributes.PropertyAttributes.InputSpecification("Indicate the type of click required")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Left Click**, **Middle Click**, **Right Click**, **Double Left Click**, **Left Down**, **Middle Down**, **Right Down**, **Left Up**, **Middle Up**, **Right Up** ")]
        [Attributes.PropertyAttributes.Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
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
                v_XMousePosition = newPoint.X.ToString(),
                v_YMousePosition = newPoint.Y.ToString(),
                v_MouseClick = v_MouseClick
            };
            newMouseMove.RunCommand(sender);
        }

        public List<string> FindHandleObjects(string windowTitle)
        {
            var automationElement = AutomationElement.RootElement.FindFirst
    (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
    windowTitle));

            if (automationElement == null)
            {
                throw new Exception("Unable to find the window named '" + windowTitle + "'");
            }

            AutomationElementCollection searchItems;
            try
            {
                searchItems = automationElement.FindAll(TreeScope.Descendants, PropertyCondition.TrueCondition);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to find any child controls in window '" + windowTitle + "'. " + ex.ToString());
            }

            List<String> handleList = new List<String>();
            foreach (AutomationElement item in searchItems)
            {
                try
                {
                    if (item.Current.Name.Trim() != string.Empty)
                        handleList.Add(item.Current.Name);
                }
                catch (Exception)
                {
                    //do not throw
                }
             
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
    [Attributes.ClassAttributes.Description("This command gets text from a Thick Application window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get text from a specific handle in a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows UI Automation' to find elements and invokes a Variable Command to assign data and achieve automation")]
    public class ThickAppGetTextCommand : ScriptCommand
    {
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Window to Automate")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_AutomationWindowName { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Please select the Appropriate Item")]
        [Attributes.PropertyAttributes.InputSpecification("Select one of the valid handles from the window")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("This list is populated after you select which window is required")]
        public string v_AutomationHandleDisplayName { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Automation ID of the Item")]
        [Attributes.PropertyAttributes.InputSpecification("n/a")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("This item is populated after you select which window handle name is required")]
        public string v_AutomationID { get; set; }
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
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

        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowElementRecorder)]
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
            var element = windowElement.FindFirst(TreeScope.Descendants, searchConditions);
            return element;

        }
        public override void RunCommand(object sender)
        {

            var engine = (Core.AutomationEngineInstance)sender;

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
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Sends keystrokes to a targeted window")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to send keystroke inputs to a window.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Windows.Forms.SendKeys' method to achieve automation.")]
    public class UserInputCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify a heading name")]
        [Attributes.PropertyAttributes.InputSpecification("Define the header to be displayed on the input form.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputHeader { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify input directions")]
        [Attributes.PropertyAttributes.InputSpecification("Define the directions you want to give the user.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputDirections { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("User Input Parameters")]
        [Attributes.PropertyAttributes.InputSpecification("Define the required input parameters.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.AddInputParameter)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_UserInputConfig { get; set; }

        public UserInputCommand()
        {
            this.CommandName = "UserInputCommand";
            this.SelectionName = "Prompt for Input";
            this.CommandEnabled = true;

            v_UserInputConfig = new DataTable();
            v_UserInputConfig.TableName = DateTime.Now.ToString("UserInputParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            v_UserInputConfig.Columns.Add("Type");
            v_UserInputConfig.Columns.Add("Label");
            v_UserInputConfig.Columns.Add("Size");
            v_UserInputConfig.Columns.Add("DefaultValue");
            v_UserInputConfig.Columns.Add("UserInput");
            v_UserInputConfig.Columns.Add("ApplyToVariable");

            v_InputHeader = "Please Provide Input";
            v_InputDirections = "Directions: Please fill in the following fields";
        }

        public override void RunCommand(object sender)
        {


            var engine = (Core.AutomationEngineInstance)sender;

                        
            if (engine.tasktEngineUI == null)
            {
                engine.ReportProgress("UserInput Supported With UI Only");
                System.Windows.Forms.MessageBox.Show("UserInput Supported With UI Only", "UserInput Command", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }


            //create clone of original
            var clonedCommand = taskt.Core.Common.Clone(this);

            //translate variable
            clonedCommand.v_InputHeader = clonedCommand.v_InputHeader.ConvertToUserVariable(sender);
            clonedCommand.v_InputDirections = clonedCommand.v_InputDirections.ConvertToUserVariable(sender);

            //translate variables for each label
            foreach (DataRow rw in clonedCommand.v_UserInputConfig.Rows)
            {
                rw["Label"] = rw["Label"].ToString().ConvertToUserVariable(sender);

                var targetVariable = rw["ApplyToVariable"] as string;

                if (string.IsNullOrEmpty(targetVariable))
                {
                    var newMessage = new MessageBoxCommand();
                    newMessage.v_Message = "User Input question '" + rw["Label"] + "' is missing variables to apply results to! Results for the item will not be tracked.  To fix this, assign a variable in the designer!";
                    newMessage.v_AutoCloseAfter = 10;
                    newMessage.RunCommand(sender);
                }
            }


            //invoke ui for data collection
            var result = engine.tasktEngineUI.Invoke(new Action(() =>
            {

                //get input from user
              var userInputs =  engine.tasktEngineUI.ShowInput(clonedCommand);

                //check if user provided input
                if (userInputs != null)
                {

                    //loop through each input and assign
                    for (int i = 0; i < userInputs.Count; i++)
                    {
                        //get target variable
                        var targetVariable = v_UserInputConfig.Rows[i]["ApplyToVariable"] as string;


                        //if engine is expected to create variables, the user will not expect them to contain start/end markers
                        //ex. {vAge} should not be created, vAge should be created and then called by doing {vAge}
                        if ((!string.IsNullOrEmpty(targetVariable)) && (engine.engineSettings.CreateMissingVariablesDuringExecution))
                        {
                            //remove start markers
                            if (targetVariable.StartsWith(engine.engineSettings.VariableStartMarker))
                            {
                                targetVariable = targetVariable.TrimStart(engine.engineSettings.VariableStartMarker.ToCharArray());
                            }

                            //remove end markers
                            if (targetVariable.EndsWith(engine.engineSettings.VariableEndMarker))
                            {
                                targetVariable = targetVariable.TrimEnd(engine.engineSettings.VariableEndMarker.ToCharArray());
                            }
                        }

                       
                        //store user data in variable
                        if (!string.IsNullOrEmpty(targetVariable))
                        {
                            userInputs[i].StoreInUserVariable(sender, targetVariable);
                        }

                                  
                    }


                }

            }

            ));


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_InputHeader + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Input Commands")]
    [Attributes.ClassAttributes.Description("Allows the entry of data into a web-enabled form")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want a fancy data collection.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'WebBrowser Control' to achieve automation.")]
    public class HTMLInputCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the HTML to be used")]
        [Attributes.PropertyAttributes.InputSpecification("Define the HTML to be displayed")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowHTMLBuilder)]
        public string v_InputHTML { get; set; }


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify if an error should occur on any result other than 'OK'")]
        [Attributes.PropertyAttributes.InputSpecification("Select if this should throw an exception.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error On Close")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Do Not Error On Close")]
        public string v_ErrorOnClose { get; set; }

        public HTMLInputCommand()
        {
            this.CommandName = "HTMLInputCommand";
            this.SelectionName = "Prompt for HTML Input";
            this.CommandEnabled = true;
            //this.v_InputHTML = "<!DOCTYPE html>\r\n\r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title>Please Provide Information</title>\r\n\r\n    <link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css\" integrity=\"sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm\" crossorigin=\"anonymous\">\r\n    <script src=\"https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js\" integrity=\"sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl\" crossorigin=\"anonymous\"></script>\r\n\r\n</head>\r\n<body>\r\n\r\n    <nav class=\"navbar navbar-dark bg-dark\">\r\n        <h1 style=\"color:white\">taskt</h1><small style=\"color:white\">free and open-source process automation</small>\r\n    </nav>\r\n    <br />\r\n\r\n\r\n    <div class=\"container\">\r\n\r\n        <h5><b>Directions:</b> This a sample data collection form that can be presented to a user.  You can add and implement as many fields as you need or choose standard form inputs. Note, each field will require a <b>v_applyToVariable</b> attribute specifying which variable should contain the respective value for the input field.</h5>\r\n\r\n        <hr />\r\n        <form>\r\n            <div class=\"form-row\">\r\n                <div class=\"form-group col-md-6\">\r\n                    <label for=\"inputEmail4\">Email</label>\r\n                    <input type=\"email\" class=\"form-control\" id=\"inputEmail4\" v_applyToVariable=\"vInput\" placeholder=\"Email\">\r\n                </div>\r\n                <div class=\"form-group col-md-6\">\r\n                    <label for=\"inputPassword4\">Password</label>\r\n                    <input type=\"password\" class=\"form-control\" id=\"inputPassword4\" v_applyToVariable=\"vPass\" placeholder=\"Password\">\r\n                </div>\r\n            </div>\r\n            <div class=\"form-group\">\r\n                <label for=\"inputAddress\">Address</label>\r\n                <input type=\"text\" class=\"form-control\" id=\"inputAddress\" v_applyToVariable=\"vAddress\" placeholder=\"1234 Main St\">\r\n            </div>\r\n            <div class=\"form-group\">\r\n                <label for=\"inputAddress2\">Address 2</label>\r\n                <input type=\"text\" class=\"form-control\" id=\"inputAddress2\" v_applyToVariable=\"vAddress2\" placeholder=\"Apartment, studio, or floor\">\r\n            </div>\r\n            <div class=\"form-row\">\r\n                <div class=\"form-group col-md-6\">\r\n                    <label for=\"inputCity\">City</label>\r\n                    <input type=\"text\" class=\"form-control\" id=\"inputCity\" v_applyToVariable=\"vCity\">\r\n                </div>\r\n                <div class=\"form-group col-md-4\">\r\n                    <label for=\"inputState\">State</label>\r\n                    <input type=\"text\" class=\"form-control\" id=\"inputState\" v_applyToVariable=\"vState\">\r\n                </div>\r\n                <div class=\"form-group col-md-2\">\r\n                    <label for=\"inputZip\">Zip</label>\r\n                    <input type=\"text\" class=\"form-control\" id=\"inputZip\" v_applyToVariable=\"vZip\">\r\n                </div>\r\n            </div>\r\n            <div class=\"form-group\">\r\n                <div class=\"form-check\">\r\n                    <input class=\"form-check-input\" type=\"checkbox\" id=\"gridCheck\" v_applyToVariable=\"vCheck\">\r\n                    <label class=\"form-check-label\" for=\"gridCheck\">\r\n                        Check me out\r\n                    </label>\r\n                </div>\r\n            </div>\r\n            <button class=\"btn btn-primary\" onclick=\"window.external.Ok();\">Ok</button>\r\n            <button class=\"btn btn-primary\" onclick=\"window.external.Cancel();\">Close</button>\r\n        </form>\r\n    </div>\r\n\r\n</body>\r\n</html>";
           this.v_InputHTML = "<!DOCTYPE html>\r\n\r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <meta charset=\"utf-8\" />\r\n    <title>Please Provide Information</title>\r\n\r\n    <style type=\"text/css\">\r\n        /*!\n * Bootstrap v4.0.0 (https://getbootstrap.com)\n * Copyright 2011-2018 The Bootstrap Authors\n * Copyright 2011-2018 Twitter, Inc.\n * Licensed under MIT (https://github.com/twbs/bootstrap/blob/master/LICENSE)\n */ :root {\r\n            --blue: #007bff;\r\n            --indigo: #6610f2;\r\n            --purple: #6f42c1;\r\n            --pink: #e83e8c;\r\n            --red: #dc3545;\r\n            --orange: #fd7e14;\r\n            --yellow: #ffc107;\r\n            --green: #28a745;\r\n            --teal: #20c997;\r\n            --cyan: #17a2b8;\r\n            --white: #fff;\r\n            --gray: #6c757d;\r\n            --gray-dark: #343a40;\r\n            --primary: #007bff;\r\n            --secondary: #6c757d;\r\n            --success: #28a745;\r\n            --info: #17a2b8;\r\n            --warning: #ffc107;\r\n            --danger: #dc3545;\r\n            --light: #f8f9fa;\r\n            --dark: #343a40;\r\n            --breakpoint-xs: 0;\r\n            --breakpoint-sm: 576px;\r\n            --breakpoint-md: 768px;\r\n            --breakpoint-lg: 992px;\r\n            --breakpoint-xl: 1200px;\r\n            --font-family-sans-serif: -apple-system,BlinkMacSystemFont,\"Segoe UI\",Roboto,\"Helvetica Neue\",Arial,sans-serif,\"Apple Color Emoji\",\"Segoe UI Emoji\",\"Segoe UI Symbol\";\r\n            --font-family-monospace: SFMono-Regular,Menlo,Monaco,Consolas,\"Liberation Mono\",\"Courier New\",monospace\r\n        }\r\n\r\n        *, ::after, ::before {\r\n            box-sizing: border-box\r\n        }\r\n\r\n        html {\r\n            font-family: sans-serif;\r\n            line-height: 1.15;\r\n            -webkit-text-size-adjust: 100%;\r\n            -ms-text-size-adjust: 100%;\r\n            -ms-overflow-style: scrollbar;\r\n            -webkit-tap-highlight-color: transparent\r\n        }\r\n\r\n        @-ms-viewport {\r\n            width: device-width\r\n        }\r\n\r\n        article, aside, dialog, figcaption, figure, footer, header, hgroup, main, nav, section {\r\n            display: block\r\n        }\r\n\r\n        body {\r\n            margin: 0;\r\n            font-family: -apple-system,BlinkMacSystemFont,\"Segoe UI\",Roboto,\"Helvetica Neue\",Arial,sans-serif,\"Apple Color Emoji\",\"Segoe UI Emoji\",\"Segoe UI Symbol\";\r\n            font-size: 1rem;\r\n            font-weight: 400;\r\n            line-height: 1.5;\r\n            color: #212529;\r\n            text-align: left;\r\n            background-color: #fff\r\n        }\r\n\r\n        [tabindex=\"-1\"]:focus {\r\n            outline: 0 !important\r\n        }\r\n\r\n        hr {\r\n            box-sizing: content-box;\r\n            height: 0;\r\n            overflow: visible\r\n        }\r\n\r\n        h1, h2, h3, h4, h5, h6 {\r\n            margin-top: 0;\r\n            margin-bottom: .5rem\r\n        }\r\n\r\n        p {\r\n            margin-top: 0;\r\n            margin-bottom: 1rem\r\n        }\r\n\r\n        abbr[data-original-title], abbr[title] {\r\n            text-decoration: underline;\r\n            -webkit-text-decoration: underline dotted;\r\n            text-decoration: underline dotted;\r\n            cursor: help;\r\n            border-bottom: 0\r\n        }\r\n\r\n        address {\r\n            margin-bottom: 1rem;\r\n            font-style: normal;\r\n            line-height: inherit\r\n        }\r\n\r\n        dl, ol, ul {\r\n            margin-top: 0;\r\n            margin-bottom: 1rem\r\n        }\r\n\r\n            ol ol, ol ul, ul ol, ul ul {\r\n                margin-bottom: 0\r\n            }\r\n\r\n        dt {\r\n            font-weight: 700\r\n        }\r\n\r\n        dd {\r\n            margin-bottom: .5rem;\r\n            margin-left: 0\r\n        }\r\n\r\n        blockquote {\r\n            margin: 0 0 1rem\r\n        }\r\n\r\n        dfn {\r\n            font-style: italic\r\n        }\r\n\r\n        b, strong {\r\n            font-weight: bolder\r\n        }\r\n\r\n        small {\r\n            font-size: 80%\r\n        }\r\n\r\n        sub, sup {\r\n            position: relative;\r\n            font-size: 75%;\r\n            line-height: 0;\r\n            vertical-align: baseline\r\n        }\r\n\r\n        sub {\r\n            bottom: -.25em\r\n        }\r\n\r\n        sup {\r\n            top: -.5em\r\n        }\r\n\r\n        a {\r\n            color: #007bff;\r\n            text-decoration: none;\r\n            background-color: transparent;\r\n            -webkit-text-decoration-skip: objects\r\n        }\r\n\r\n            a:hover {\r\n                color: #0056b3;\r\n                text-decoration: underline\r\n            }\r\n\r\n            a:not([href]):not([tabindex]) {\r\n                color: inherit;\r\n                text-decoration: none\r\n            }\r\n\r\n                a:not([href]):not([tabindex]):focus, a:not([href]):not([tabindex]):hover {\r\n                    color: inherit;\r\n                    text-decoration: none\r\n                }\r\n\r\n                a:not([href]):not([tabindex]):focus {\r\n                    outline: 0\r\n                }\r\n\r\n        code, kbd, pre, samp {\r\n            font-family: monospace,monospace;\r\n            font-size: 1em\r\n        }\r\n\r\n        pre {\r\n            margin-top: 0;\r\n            margin-bottom: 1rem;\r\n            overflow: auto;\r\n            -ms-overflow-style: scrollbar\r\n        }\r\n\r\n        figure {\r\n            margin: 0 0 1rem\r\n        }\r\n\r\n        img {\r\n            vertical-align: middle;\r\n            border-style: none\r\n        }\r\n\r\n        svg:not(:root) {\r\n            overflow: hidden\r\n        }\r\n\r\n        table {\r\n            border-collapse: collapse\r\n        }\r\n\r\n        caption {\r\n            padding-top: .75rem;\r\n            padding-bottom: .75rem;\r\n            color: #6c757d;\r\n            text-align: left;\r\n            caption-side: bottom\r\n        }\r\n\r\n        th {\r\n            text-align: inherit\r\n        }\r\n\r\n        label {\r\n            display: inline-block;\r\n            margin-bottom: .5rem\r\n        }\r\n\r\n        button {\r\n            border-radius: 0\r\n        }\r\n\r\n            button:focus {\r\n                outline: 1px dotted;\r\n                outline: 5px auto -webkit-focus-ring-color\r\n            }\r\n\r\n        button, input, optgroup, select, textarea {\r\n            margin: 0;\r\n            font-family: inherit;\r\n            font-size: inherit;\r\n            line-height: inherit\r\n        }\r\n\r\n        button, input {\r\n            overflow: visible\r\n        }\r\n\r\n        button, select {\r\n            text-transform: none\r\n        }\r\n\r\n        [type=reset], [type=submit], button, html [type=button] {\r\n            -webkit-appearance: button\r\n        }\r\n\r\n            [type=button]::-moz-focus-inner, [type=reset]::-moz-focus-inner, [type=submit]::-moz-focus-inner, button::-moz-focus-inner {\r\n                padding: 0;\r\n                border-style: none\r\n            }\r\n\r\n        input[type=checkbox], input[type=radio] {\r\n            box-sizing: border-box;\r\n            padding: 0\r\n        }\r\n\r\n        input[type=date], input[type=datetime-local], input[type=month], input[type=time] {\r\n            -webkit-appearance: listbox\r\n        }\r\n\r\n        textarea {\r\n            overflow: auto;\r\n            resize: vertical\r\n        }\r\n\r\n        fieldset {\r\n            min-width: 0;\r\n            padding: 0;\r\n            margin: 0;\r\n            border: 0\r\n        }\r\n\r\n        legend {\r\n            display: block;\r\n            width: 100%;\r\n            max-width: 100%;\r\n            padding: 0;\r\n            margin-bottom: .5rem;\r\n            font-size: 1.5rem;\r\n            line-height: inherit;\r\n            color: inherit;\r\n            white-space: normal\r\n        }\r\n\r\n        progress {\r\n            vertical-align: baseline\r\n        }\r\n\r\n        [type=number]::-webkit-inner-spin-button, [type=number]::-webkit-outer-spin-button {\r\n            height: auto\r\n        }\r\n\r\n        [type=search] {\r\n            outline-offset: -2px;\r\n            -webkit-appearance: none\r\n        }\r\n\r\n            [type=search]::-webkit-search-cancel-button, [type=search]::-webkit-search-decoration {\r\n                -webkit-appearance: none\r\n            }\r\n\r\n        ::-webkit-file-upload-button {\r\n            font: inherit;\r\n            -webkit-appearance: button\r\n        }\r\n\r\n        output {\r\n            display: inline-block\r\n        }\r\n\r\n        summary {\r\n            display: list-item;\r\n            cursor: pointer\r\n        }\r\n\r\n        template {\r\n            display: none\r\n        }\r\n\r\n        [hidden] {\r\n            display: none !important\r\n        }\r\n\r\n        .h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6 {\r\n            margin-bottom: .5rem;\r\n            font-family: inherit;\r\n            font-weight: 500;\r\n            line-height: 1.2;\r\n            color: inherit\r\n        }\r\n\r\n        .h1, h1 {\r\n            font-size: 2.5rem\r\n        }\r\n\r\n        .h2, h2 {\r\n            font-size: 2rem\r\n        }\r\n\r\n        .h3, h3 {\r\n            font-size: 1.75rem\r\n        }\r\n\r\n        .h4, h4 {\r\n            font-size: 1.5rem\r\n        }\r\n\r\n        .h5, h5 {\r\n            font-size: 1.25rem\r\n        }\r\n\r\n        .h6, h6 {\r\n            font-size: 1rem\r\n        }\r\n\r\n        .lead {\r\n            font-size: 1.25rem;\r\n            font-weight: 300\r\n        }\r\n\r\n        .display-1 {\r\n            font-size: 6rem;\r\n            font-weight: 300;\r\n            line-height: 1.2\r\n        }\r\n\r\n        .display-2 {\r\n            font-size: 5.5rem;\r\n            font-weight: 300;\r\n            line-height: 1.2\r\n        }\r\n\r\n        .display-3 {\r\n            font-size: 4.5rem;\r\n            font-weight: 300;\r\n            line-height: 1.2\r\n        }\r\n\r\n        .display-4 {\r\n            font-size: 3.5rem;\r\n            font-weight: 300;\r\n            line-height: 1.2\r\n        }\r\n\r\n        hr {\r\n            margin-top: 1rem;\r\n            margin-bottom: 1rem;\r\n            border: 0;\r\n            border-top: 1px solid rgba(0,0,0,.1)\r\n        }\r\n\r\n        .small, small {\r\n            font-size: 80%;\r\n            font-weight: 400\r\n        }\r\n\r\n        .mark, mark {\r\n            padding: .2em;\r\n            background-color: #fcf8e3\r\n        }\r\n\r\n        .list-unstyled {\r\n            padding-left: 0;\r\n            list-style: none\r\n        }\r\n\r\n        .list-inline {\r\n            padding-left: 0;\r\n            list-style: none\r\n        }\r\n\r\n        .list-inline-item {\r\n            display: inline-block\r\n        }\r\n\r\n            .list-inline-item:not(:last-child) {\r\n                margin-right: .5rem\r\n            }\r\n\r\n        .initialism {\r\n            font-size: 90%;\r\n            text-transform: uppercase\r\n        }\r\n\r\n        .blockquote {\r\n            margin-bottom: 1rem;\r\n            font-size: 1.25rem\r\n        }\r\n\r\n        .blockquote-footer {\r\n            display: block;\r\n            font-size: 80%;\r\n            color: #6c757d\r\n        }\r\n\r\n            .blockquote-footer::before {\r\n                content: \"\\2014 \\00A0\"\r\n            }\r\n\r\n        .img-fluid {\r\n            max-width: 100%;\r\n            height: auto\r\n        }\r\n\r\n        .img-thumbnail {\r\n            padding: .25rem;\r\n            background-color: #fff;\r\n            border: 1px solid #dee2e6;\r\n            border-radius: .25rem;\r\n            max-width: 100%;\r\n            height: auto\r\n        }\r\n\r\n        .figure {\r\n            display: inline-block\r\n        }\r\n\r\n        .figure-img {\r\n            margin-bottom: .5rem;\r\n            line-height: 1\r\n        }\r\n\r\n        .figure-caption {\r\n            font-size: 90%;\r\n            color: #6c757d\r\n        }\r\n\r\n        code, kbd, pre, samp {\r\n            font-family: SFMono-Regular,Menlo,Monaco,Consolas,\"Liberation Mono\",\"Courier New\",monospace\r\n        }\r\n\r\n        code {\r\n            font-size: 87.5%;\r\n            color: #e83e8c;\r\n            word-break: break-word\r\n        }\r\n\r\n        a > code {\r\n            color: inherit\r\n        }\r\n\r\n        kbd {\r\n            padding: .2rem .4rem;\r\n            font-size: 87.5%;\r\n            color: #fff;\r\n            background-color: #212529;\r\n            border-radius: .2rem\r\n        }\r\n\r\n            kbd kbd {\r\n                padding: 0;\r\n                font-size: 100%;\r\n                font-weight: 700\r\n            }\r\n\r\n        pre {\r\n            display: block;\r\n            font-size: 87.5%;\r\n            color: #212529\r\n        }\r\n\r\n            pre code {\r\n                font-size: inherit;\r\n                color: inherit;\r\n                word-break: normal\r\n            }\r\n\r\n        .pre-scrollable {\r\n            max-height: 340px;\r\n            overflow-y: scroll\r\n        }\r\n\r\n        .container {\r\n            width: 100%;\r\n            padding-right: 15px;\r\n            padding-left: 15px;\r\n            margin-right: auto;\r\n            margin-left: auto\r\n        }\r\n\r\n        @media (min-width:576px) {\r\n            .container {\r\n                max-width: 540px\r\n            }\r\n        }\r\n\r\n        @media (min-width:768px) {\r\n            .container {\r\n                max-width: 720px\r\n            }\r\n        }\r\n\r\n        @media (min-width:992px) {\r\n            .container {\r\n                max-width: 960px\r\n            }\r\n        }\r\n\r\n        @media (min-width:1200px) {\r\n            .container {\r\n                max-width: 1140px\r\n            }\r\n        }\r\n\r\n        .container-fluid {\r\n            width: 100%;\r\n            padding-right: 15px;\r\n            padding-left: 15px;\r\n            margin-right: auto;\r\n            margin-left: auto\r\n        }\r\n\r\n        .row {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -ms-flex-wrap: wrap;\r\n            flex-wrap: wrap;\r\n            margin-right: -15px;\r\n            margin-left: -15px\r\n        }\r\n\r\n        .no-gutters {\r\n            margin-right: 0;\r\n            margin-left: 0\r\n        }\r\n\r\n            .no-gutters > .col, .no-gutters > [class*=col-] {\r\n                padding-right: 0;\r\n                padding-left: 0\r\n            }\r\n\r\n        .col, .col-1, .col-10, .col-11, .col-12, .col-2, .col-3, .col-4, .col-5, .col-6, .col-7, .col-8, .col-9, .col-auto, .col-lg, .col-lg-1, .col-lg-10, .col-lg-11, .col-lg-12, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-auto, .col-md, .col-md-1, .col-md-10, .col-md-11, .col-md-12, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-auto, .col-sm, .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-auto, .col-xl, .col-xl-1, .col-xl-10, .col-xl-11, .col-xl-12, .col-xl-2, .col-xl-3, .col-xl-4, .col-xl-5, .col-xl-6, .col-xl-7, .col-xl-8, .col-xl-9, .col-xl-auto {\r\n            position: relative;\r\n            width: 100%;\r\n            min-height: 1px;\r\n            padding-right: 15px;\r\n            padding-left: 15px\r\n        }\r\n\r\n        .col {\r\n            -ms-flex-preferred-size: 0;\r\n            flex-basis: 0;\r\n            -webkit-box-flex: 1;\r\n            -ms-flex-positive: 1;\r\n            flex-grow: 1;\r\n            max-width: 100%\r\n        }\r\n\r\n        .col-auto {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 auto;\r\n            flex: 0 0 auto;\r\n            width: auto;\r\n            max-width: none\r\n        }\r\n\r\n        .col-1 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 8.333333%;\r\n            flex: 0 0 8.333333%;\r\n            max-width: 8.333333%\r\n        }\r\n\r\n        .col-2 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 16.666667%;\r\n            flex: 0 0 16.666667%;\r\n            max-width: 16.666667%\r\n        }\r\n\r\n        .col-3 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 25%;\r\n            flex: 0 0 25%;\r\n            max-width: 25%\r\n        }\r\n\r\n        .col-4 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 33.333333%;\r\n            flex: 0 0 33.333333%;\r\n            max-width: 33.333333%\r\n        }\r\n\r\n        .col-5 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 41.666667%;\r\n            flex: 0 0 41.666667%;\r\n            max-width: 41.666667%\r\n        }\r\n\r\n        .col-6 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 50%;\r\n            flex: 0 0 50%;\r\n            max-width: 50%\r\n        }\r\n\r\n        .col-7 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 58.333333%;\r\n            flex: 0 0 58.333333%;\r\n            max-width: 58.333333%\r\n        }\r\n\r\n        .col-8 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 66.666667%;\r\n            flex: 0 0 66.666667%;\r\n            max-width: 66.666667%\r\n        }\r\n\r\n        .col-9 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 75%;\r\n            flex: 0 0 75%;\r\n            max-width: 75%\r\n        }\r\n\r\n        .col-10 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 83.333333%;\r\n            flex: 0 0 83.333333%;\r\n            max-width: 83.333333%\r\n        }\r\n\r\n        .col-11 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 91.666667%;\r\n            flex: 0 0 91.666667%;\r\n            max-width: 91.666667%\r\n        }\r\n\r\n        .col-12 {\r\n            -webkit-box-flex: 0;\r\n            -ms-flex: 0 0 100%;\r\n            flex: 0 0 100%;\r\n            max-width: 100%\r\n        }\r\n\r\n        .order-first {\r\n            -webkit-box-ordinal-group: 0;\r\n            -ms-flex-order: -1;\r\n            order: -1\r\n        }\r\n\r\n        .order-last {\r\n            -webkit-box-ordinal-group: 14;\r\n            -ms-flex-order: 13;\r\n            order: 13\r\n        }\r\n\r\n        .order-0 {\r\n            -webkit-box-ordinal-group: 1;\r\n            -ms-flex-order: 0;\r\n            order: 0\r\n        }\r\n\r\n        .order-1 {\r\n            -webkit-box-ordinal-group: 2;\r\n            -ms-flex-order: 1;\r\n            order: 1\r\n        }\r\n\r\n        .order-2 {\r\n            -webkit-box-ordinal-group: 3;\r\n            -ms-flex-order: 2;\r\n            order: 2\r\n        }\r\n\r\n        .order-3 {\r\n            -webkit-box-ordinal-group: 4;\r\n            -ms-flex-order: 3;\r\n            order: 3\r\n        }\r\n\r\n        .order-4 {\r\n            -webkit-box-ordinal-group: 5;\r\n            -ms-flex-order: 4;\r\n            order: 4\r\n        }\r\n\r\n        .order-5 {\r\n            -webkit-box-ordinal-group: 6;\r\n            -ms-flex-order: 5;\r\n            order: 5\r\n        }\r\n\r\n        .order-6 {\r\n            -webkit-box-ordinal-group: 7;\r\n            -ms-flex-order: 6;\r\n            order: 6\r\n        }\r\n\r\n        .order-7 {\r\n            -webkit-box-ordinal-group: 8;\r\n            -ms-flex-order: 7;\r\n            order: 7\r\n        }\r\n\r\n        .order-8 {\r\n            -webkit-box-ordinal-group: 9;\r\n            -ms-flex-order: 8;\r\n            order: 8\r\n        }\r\n\r\n        .order-9 {\r\n            -webkit-box-ordinal-group: 10;\r\n            -ms-flex-order: 9;\r\n            order: 9\r\n        }\r\n\r\n        .order-10 {\r\n            -webkit-box-ordinal-group: 11;\r\n            -ms-flex-order: 10;\r\n            order: 10\r\n        }\r\n\r\n        .order-11 {\r\n            -webkit-box-ordinal-group: 12;\r\n            -ms-flex-order: 11;\r\n            order: 11\r\n        }\r\n\r\n        .order-12 {\r\n            -webkit-box-ordinal-group: 13;\r\n            -ms-flex-order: 12;\r\n            order: 12\r\n        }\r\n\r\n        .offset-1 {\r\n            margin-left: 8.333333%\r\n        }\r\n\r\n        .offset-2 {\r\n            margin-left: 16.666667%\r\n        }\r\n\r\n        .offset-3 {\r\n            margin-left: 25%\r\n        }\r\n\r\n        .offset-4 {\r\n            margin-left: 33.333333%\r\n        }\r\n\r\n        .offset-5 {\r\n            margin-left: 41.666667%\r\n        }\r\n\r\n        .offset-6 {\r\n            margin-left: 50%\r\n        }\r\n\r\n        .offset-7 {\r\n            margin-left: 58.333333%\r\n        }\r\n\r\n        .offset-8 {\r\n            margin-left: 66.666667%\r\n        }\r\n\r\n        .offset-9 {\r\n            margin-left: 75%\r\n        }\r\n\r\n        .offset-10 {\r\n            margin-left: 83.333333%\r\n        }\r\n\r\n        .offset-11 {\r\n            margin-left: 91.666667%\r\n        }\r\n\r\n        @media (min-width:576px) {\r\n            .col-sm {\r\n                -ms-flex-preferred-size: 0;\r\n                flex-basis: 0;\r\n                -webkit-box-flex: 1;\r\n                -ms-flex-positive: 1;\r\n                flex-grow: 1;\r\n                max-width: 100%\r\n            }\r\n\r\n            .col-sm-auto {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 auto;\r\n                flex: 0 0 auto;\r\n                width: auto;\r\n                max-width: none\r\n            }\r\n\r\n            .col-sm-1 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 8.333333%;\r\n                flex: 0 0 8.333333%;\r\n                max-width: 8.333333%\r\n            }\r\n\r\n            .col-sm-2 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 16.666667%;\r\n                flex: 0 0 16.666667%;\r\n                max-width: 16.666667%\r\n            }\r\n\r\n            .col-sm-3 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 25%;\r\n                flex: 0 0 25%;\r\n                max-width: 25%\r\n            }\r\n\r\n            .col-sm-4 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 33.333333%;\r\n                flex: 0 0 33.333333%;\r\n                max-width: 33.333333%\r\n            }\r\n\r\n            .col-sm-5 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 41.666667%;\r\n                flex: 0 0 41.666667%;\r\n                max-width: 41.666667%\r\n            }\r\n\r\n            .col-sm-6 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 50%;\r\n                flex: 0 0 50%;\r\n                max-width: 50%\r\n            }\r\n\r\n            .col-sm-7 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 58.333333%;\r\n                flex: 0 0 58.333333%;\r\n                max-width: 58.333333%\r\n            }\r\n\r\n            .col-sm-8 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 66.666667%;\r\n                flex: 0 0 66.666667%;\r\n                max-width: 66.666667%\r\n            }\r\n\r\n            .col-sm-9 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 75%;\r\n                flex: 0 0 75%;\r\n                max-width: 75%\r\n            }\r\n\r\n            .col-sm-10 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 83.333333%;\r\n                flex: 0 0 83.333333%;\r\n                max-width: 83.333333%\r\n            }\r\n\r\n            .col-sm-11 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 91.666667%;\r\n                flex: 0 0 91.666667%;\r\n                max-width: 91.666667%\r\n            }\r\n\r\n            .col-sm-12 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 100%;\r\n                flex: 0 0 100%;\r\n                max-width: 100%\r\n            }\r\n\r\n            .order-sm-first {\r\n                -webkit-box-ordinal-group: 0;\r\n                -ms-flex-order: -1;\r\n                order: -1\r\n            }\r\n\r\n            .order-sm-last {\r\n                -webkit-box-ordinal-group: 14;\r\n                -ms-flex-order: 13;\r\n                order: 13\r\n            }\r\n\r\n            .order-sm-0 {\r\n                -webkit-box-ordinal-group: 1;\r\n                -ms-flex-order: 0;\r\n                order: 0\r\n            }\r\n\r\n            .order-sm-1 {\r\n                -webkit-box-ordinal-group: 2;\r\n                -ms-flex-order: 1;\r\n                order: 1\r\n            }\r\n\r\n            .order-sm-2 {\r\n                -webkit-box-ordinal-group: 3;\r\n                -ms-flex-order: 2;\r\n                order: 2\r\n            }\r\n\r\n            .order-sm-3 {\r\n                -webkit-box-ordinal-group: 4;\r\n                -ms-flex-order: 3;\r\n                order: 3\r\n            }\r\n\r\n            .order-sm-4 {\r\n                -webkit-box-ordinal-group: 5;\r\n                -ms-flex-order: 4;\r\n                order: 4\r\n            }\r\n\r\n            .order-sm-5 {\r\n                -webkit-box-ordinal-group: 6;\r\n                -ms-flex-order: 5;\r\n                order: 5\r\n            }\r\n\r\n            .order-sm-6 {\r\n                -webkit-box-ordinal-group: 7;\r\n                -ms-flex-order: 6;\r\n                order: 6\r\n            }\r\n\r\n            .order-sm-7 {\r\n                -webkit-box-ordinal-group: 8;\r\n                -ms-flex-order: 7;\r\n                order: 7\r\n            }\r\n\r\n            .order-sm-8 {\r\n                -webkit-box-ordinal-group: 9;\r\n                -ms-flex-order: 8;\r\n                order: 8\r\n            }\r\n\r\n            .order-sm-9 {\r\n                -webkit-box-ordinal-group: 10;\r\n                -ms-flex-order: 9;\r\n                order: 9\r\n            }\r\n\r\n            .order-sm-10 {\r\n                -webkit-box-ordinal-group: 11;\r\n                -ms-flex-order: 10;\r\n                order: 10\r\n            }\r\n\r\n            .order-sm-11 {\r\n                -webkit-box-ordinal-group: 12;\r\n                -ms-flex-order: 11;\r\n                order: 11\r\n            }\r\n\r\n            .order-sm-12 {\r\n                -webkit-box-ordinal-group: 13;\r\n                -ms-flex-order: 12;\r\n                order: 12\r\n            }\r\n\r\n            .offset-sm-0 {\r\n                margin-left: 0\r\n            }\r\n\r\n            .offset-sm-1 {\r\n                margin-left: 8.333333%\r\n            }\r\n\r\n            .offset-sm-2 {\r\n                margin-left: 16.666667%\r\n            }\r\n\r\n            .offset-sm-3 {\r\n                margin-left: 25%\r\n            }\r\n\r\n            .offset-sm-4 {\r\n                margin-left: 33.333333%\r\n            }\r\n\r\n            .offset-sm-5 {\r\n                margin-left: 41.666667%\r\n            }\r\n\r\n            .offset-sm-6 {\r\n                margin-left: 50%\r\n            }\r\n\r\n            .offset-sm-7 {\r\n                margin-left: 58.333333%\r\n            }\r\n\r\n            .offset-sm-8 {\r\n                margin-left: 66.666667%\r\n            }\r\n\r\n            .offset-sm-9 {\r\n                margin-left: 75%\r\n            }\r\n\r\n            .offset-sm-10 {\r\n                margin-left: 83.333333%\r\n            }\r\n\r\n            .offset-sm-11 {\r\n                margin-left: 91.666667%\r\n            }\r\n        }\r\n\r\n        @media (min-width:768px) {\r\n            .col-md {\r\n                -ms-flex-preferred-size: 0;\r\n                flex-basis: 0;\r\n                -webkit-box-flex: 1;\r\n                -ms-flex-positive: 1;\r\n                flex-grow: 1;\r\n                max-width: 100%\r\n            }\r\n\r\n            .col-md-auto {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 auto;\r\n                flex: 0 0 auto;\r\n                width: auto;\r\n                max-width: none\r\n            }\r\n\r\n            .col-md-1 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 8.333333%;\r\n                flex: 0 0 8.333333%;\r\n                max-width: 8.333333%\r\n            }\r\n\r\n            .col-md-2 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 16.666667%;\r\n                flex: 0 0 16.666667%;\r\n                max-width: 16.666667%\r\n            }\r\n\r\n            .col-md-3 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 25%;\r\n                flex: 0 0 25%;\r\n                max-width: 25%\r\n            }\r\n\r\n            .col-md-4 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 33.333333%;\r\n                flex: 0 0 33.333333%;\r\n                max-width: 33.333333%\r\n            }\r\n\r\n            .col-md-5 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 41.666667%;\r\n                flex: 0 0 41.666667%;\r\n                max-width: 41.666667%\r\n            }\r\n\r\n            .col-md-6 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 50%;\r\n                flex: 0 0 50%;\r\n                max-width: 50%\r\n            }\r\n\r\n            .col-md-7 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 58.333333%;\r\n                flex: 0 0 58.333333%;\r\n                max-width: 58.333333%\r\n            }\r\n\r\n            .col-md-8 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 66.666667%;\r\n                flex: 0 0 66.666667%;\r\n                max-width: 66.666667%\r\n            }\r\n\r\n            .col-md-9 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 75%;\r\n                flex: 0 0 75%;\r\n                max-width: 75%\r\n            }\r\n\r\n            .col-md-10 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 83.333333%;\r\n                flex: 0 0 83.333333%;\r\n                max-width: 83.333333%\r\n            }\r\n\r\n            .col-md-11 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 91.666667%;\r\n                flex: 0 0 91.666667%;\r\n                max-width: 91.666667%\r\n            }\r\n\r\n            .col-md-12 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 100%;\r\n                flex: 0 0 100%;\r\n                max-width: 100%\r\n            }\r\n\r\n            .order-md-first {\r\n                -webkit-box-ordinal-group: 0;\r\n                -ms-flex-order: -1;\r\n                order: -1\r\n            }\r\n\r\n            .order-md-last {\r\n                -webkit-box-ordinal-group: 14;\r\n                -ms-flex-order: 13;\r\n                order: 13\r\n            }\r\n\r\n            .order-md-0 {\r\n                -webkit-box-ordinal-group: 1;\r\n                -ms-flex-order: 0;\r\n                order: 0\r\n            }\r\n\r\n            .order-md-1 {\r\n                -webkit-box-ordinal-group: 2;\r\n                -ms-flex-order: 1;\r\n                order: 1\r\n            }\r\n\r\n            .order-md-2 {\r\n                -webkit-box-ordinal-group: 3;\r\n                -ms-flex-order: 2;\r\n                order: 2\r\n            }\r\n\r\n            .order-md-3 {\r\n                -webkit-box-ordinal-group: 4;\r\n                -ms-flex-order: 3;\r\n                order: 3\r\n            }\r\n\r\n            .order-md-4 {\r\n                -webkit-box-ordinal-group: 5;\r\n                -ms-flex-order: 4;\r\n                order: 4\r\n            }\r\n\r\n            .order-md-5 {\r\n                -webkit-box-ordinal-group: 6;\r\n                -ms-flex-order: 5;\r\n                order: 5\r\n            }\r\n\r\n            .order-md-6 {\r\n                -webkit-box-ordinal-group: 7;\r\n                -ms-flex-order: 6;\r\n                order: 6\r\n            }\r\n\r\n            .order-md-7 {\r\n                -webkit-box-ordinal-group: 8;\r\n                -ms-flex-order: 7;\r\n                order: 7\r\n            }\r\n\r\n            .order-md-8 {\r\n                -webkit-box-ordinal-group: 9;\r\n                -ms-flex-order: 8;\r\n                order: 8\r\n            }\r\n\r\n            .order-md-9 {\r\n                -webkit-box-ordinal-group: 10;\r\n                -ms-flex-order: 9;\r\n                order: 9\r\n            }\r\n\r\n            .order-md-10 {\r\n                -webkit-box-ordinal-group: 11;\r\n                -ms-flex-order: 10;\r\n                order: 10\r\n            }\r\n\r\n            .order-md-11 {\r\n                -webkit-box-ordinal-group: 12;\r\n                -ms-flex-order: 11;\r\n                order: 11\r\n            }\r\n\r\n            .order-md-12 {\r\n                -webkit-box-ordinal-group: 13;\r\n                -ms-flex-order: 12;\r\n                order: 12\r\n            }\r\n\r\n            .offset-md-0 {\r\n                margin-left: 0\r\n            }\r\n\r\n            .offset-md-1 {\r\n                margin-left: 8.333333%\r\n            }\r\n\r\n            .offset-md-2 {\r\n                margin-left: 16.666667%\r\n            }\r\n\r\n            .offset-md-3 {\r\n                margin-left: 25%\r\n            }\r\n\r\n            .offset-md-4 {\r\n                margin-left: 33.333333%\r\n            }\r\n\r\n            .offset-md-5 {\r\n                margin-left: 41.666667%\r\n            }\r\n\r\n            .offset-md-6 {\r\n                margin-left: 50%\r\n            }\r\n\r\n            .offset-md-7 {\r\n                margin-left: 58.333333%\r\n            }\r\n\r\n            .offset-md-8 {\r\n                margin-left: 66.666667%\r\n            }\r\n\r\n            .offset-md-9 {\r\n                margin-left: 75%\r\n            }\r\n\r\n            .offset-md-10 {\r\n                margin-left: 83.333333%\r\n            }\r\n\r\n            .offset-md-11 {\r\n                margin-left: 91.666667%\r\n            }\r\n        }\r\n\r\n        @media (min-width:992px) {\r\n            .col-lg {\r\n                -ms-flex-preferred-size: 0;\r\n                flex-basis: 0;\r\n                -webkit-box-flex: 1;\r\n                -ms-flex-positive: 1;\r\n                flex-grow: 1;\r\n                max-width: 100%\r\n            }\r\n\r\n            .col-lg-auto {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 auto;\r\n                flex: 0 0 auto;\r\n                width: auto;\r\n                max-width: none\r\n            }\r\n\r\n            .col-lg-1 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 8.333333%;\r\n                flex: 0 0 8.333333%;\r\n                max-width: 8.333333%\r\n            }\r\n\r\n            .col-lg-2 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 16.666667%;\r\n                flex: 0 0 16.666667%;\r\n                max-width: 16.666667%\r\n            }\r\n\r\n            .col-lg-3 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 25%;\r\n                flex: 0 0 25%;\r\n                max-width: 25%\r\n            }\r\n\r\n            .col-lg-4 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 33.333333%;\r\n                flex: 0 0 33.333333%;\r\n                max-width: 33.333333%\r\n            }\r\n\r\n            .col-lg-5 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 41.666667%;\r\n                flex: 0 0 41.666667%;\r\n                max-width: 41.666667%\r\n            }\r\n\r\n            .col-lg-6 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 50%;\r\n                flex: 0 0 50%;\r\n                max-width: 50%\r\n            }\r\n\r\n            .col-lg-7 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 58.333333%;\r\n                flex: 0 0 58.333333%;\r\n                max-width: 58.333333%\r\n            }\r\n\r\n            .col-lg-8 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 66.666667%;\r\n                flex: 0 0 66.666667%;\r\n                max-width: 66.666667%\r\n            }\r\n\r\n            .col-lg-9 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 75%;\r\n                flex: 0 0 75%;\r\n                max-width: 75%\r\n            }\r\n\r\n            .col-lg-10 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 83.333333%;\r\n                flex: 0 0 83.333333%;\r\n                max-width: 83.333333%\r\n            }\r\n\r\n            .col-lg-11 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 91.666667%;\r\n                flex: 0 0 91.666667%;\r\n                max-width: 91.666667%\r\n            }\r\n\r\n            .col-lg-12 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 100%;\r\n                flex: 0 0 100%;\r\n                max-width: 100%\r\n            }\r\n\r\n            .order-lg-first {\r\n                -webkit-box-ordinal-group: 0;\r\n                -ms-flex-order: -1;\r\n                order: -1\r\n            }\r\n\r\n            .order-lg-last {\r\n                -webkit-box-ordinal-group: 14;\r\n                -ms-flex-order: 13;\r\n                order: 13\r\n            }\r\n\r\n            .order-lg-0 {\r\n                -webkit-box-ordinal-group: 1;\r\n                -ms-flex-order: 0;\r\n                order: 0\r\n            }\r\n\r\n            .order-lg-1 {\r\n                -webkit-box-ordinal-group: 2;\r\n                -ms-flex-order: 1;\r\n                order: 1\r\n            }\r\n\r\n            .order-lg-2 {\r\n                -webkit-box-ordinal-group: 3;\r\n                -ms-flex-order: 2;\r\n                order: 2\r\n            }\r\n\r\n            .order-lg-3 {\r\n                -webkit-box-ordinal-group: 4;\r\n                -ms-flex-order: 3;\r\n                order: 3\r\n            }\r\n\r\n            .order-lg-4 {\r\n                -webkit-box-ordinal-group: 5;\r\n                -ms-flex-order: 4;\r\n                order: 4\r\n            }\r\n\r\n            .order-lg-5 {\r\n                -webkit-box-ordinal-group: 6;\r\n                -ms-flex-order: 5;\r\n                order: 5\r\n            }\r\n\r\n            .order-lg-6 {\r\n                -webkit-box-ordinal-group: 7;\r\n                -ms-flex-order: 6;\r\n                order: 6\r\n            }\r\n\r\n            .order-lg-7 {\r\n                -webkit-box-ordinal-group: 8;\r\n                -ms-flex-order: 7;\r\n                order: 7\r\n            }\r\n\r\n            .order-lg-8 {\r\n                -webkit-box-ordinal-group: 9;\r\n                -ms-flex-order: 8;\r\n                order: 8\r\n            }\r\n\r\n            .order-lg-9 {\r\n                -webkit-box-ordinal-group: 10;\r\n                -ms-flex-order: 9;\r\n                order: 9\r\n            }\r\n\r\n            .order-lg-10 {\r\n                -webkit-box-ordinal-group: 11;\r\n                -ms-flex-order: 10;\r\n                order: 10\r\n            }\r\n\r\n            .order-lg-11 {\r\n                -webkit-box-ordinal-group: 12;\r\n                -ms-flex-order: 11;\r\n                order: 11\r\n            }\r\n\r\n            .order-lg-12 {\r\n                -webkit-box-ordinal-group: 13;\r\n                -ms-flex-order: 12;\r\n                order: 12\r\n            }\r\n\r\n            .offset-lg-0 {\r\n                margin-left: 0\r\n            }\r\n\r\n            .offset-lg-1 {\r\n                margin-left: 8.333333%\r\n            }\r\n\r\n            .offset-lg-2 {\r\n                margin-left: 16.666667%\r\n            }\r\n\r\n            .offset-lg-3 {\r\n                margin-left: 25%\r\n            }\r\n\r\n            .offset-lg-4 {\r\n                margin-left: 33.333333%\r\n            }\r\n\r\n            .offset-lg-5 {\r\n                margin-left: 41.666667%\r\n            }\r\n\r\n            .offset-lg-6 {\r\n                margin-left: 50%\r\n            }\r\n\r\n            .offset-lg-7 {\r\n                margin-left: 58.333333%\r\n            }\r\n\r\n            .offset-lg-8 {\r\n                margin-left: 66.666667%\r\n            }\r\n\r\n            .offset-lg-9 {\r\n                margin-left: 75%\r\n            }\r\n\r\n            .offset-lg-10 {\r\n                margin-left: 83.333333%\r\n            }\r\n\r\n            .offset-lg-11 {\r\n                margin-left: 91.666667%\r\n            }\r\n        }\r\n\r\n        @media (min-width:1200px) {\r\n            .col-xl {\r\n                -ms-flex-preferred-size: 0;\r\n                flex-basis: 0;\r\n                -webkit-box-flex: 1;\r\n                -ms-flex-positive: 1;\r\n                flex-grow: 1;\r\n                max-width: 100%\r\n            }\r\n\r\n            .col-xl-auto {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 auto;\r\n                flex: 0 0 auto;\r\n                width: auto;\r\n                max-width: none\r\n            }\r\n\r\n            .col-xl-1 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 8.333333%;\r\n                flex: 0 0 8.333333%;\r\n                max-width: 8.333333%\r\n            }\r\n\r\n            .col-xl-2 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 16.666667%;\r\n                flex: 0 0 16.666667%;\r\n                max-width: 16.666667%\r\n            }\r\n\r\n            .col-xl-3 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 25%;\r\n                flex: 0 0 25%;\r\n                max-width: 25%\r\n            }\r\n\r\n            .col-xl-4 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 33.333333%;\r\n                flex: 0 0 33.333333%;\r\n                max-width: 33.333333%\r\n            }\r\n\r\n            .col-xl-5 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 41.666667%;\r\n                flex: 0 0 41.666667%;\r\n                max-width: 41.666667%\r\n            }\r\n\r\n            .col-xl-6 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 50%;\r\n                flex: 0 0 50%;\r\n                max-width: 50%\r\n            }\r\n\r\n            .col-xl-7 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 58.333333%;\r\n                flex: 0 0 58.333333%;\r\n                max-width: 58.333333%\r\n            }\r\n\r\n            .col-xl-8 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 66.666667%;\r\n                flex: 0 0 66.666667%;\r\n                max-width: 66.666667%\r\n            }\r\n\r\n            .col-xl-9 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 75%;\r\n                flex: 0 0 75%;\r\n                max-width: 75%\r\n            }\r\n\r\n            .col-xl-10 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 83.333333%;\r\n                flex: 0 0 83.333333%;\r\n                max-width: 83.333333%\r\n            }\r\n\r\n            .col-xl-11 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 91.666667%;\r\n                flex: 0 0 91.666667%;\r\n                max-width: 91.666667%\r\n            }\r\n\r\n            .col-xl-12 {\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 100%;\r\n                flex: 0 0 100%;\r\n                max-width: 100%\r\n            }\r\n\r\n            .order-xl-first {\r\n                -webkit-box-ordinal-group: 0;\r\n                -ms-flex-order: -1;\r\n                order: -1\r\n            }\r\n\r\n            .order-xl-last {\r\n                -webkit-box-ordinal-group: 14;\r\n                -ms-flex-order: 13;\r\n                order: 13\r\n            }\r\n\r\n            .order-xl-0 {\r\n                -webkit-box-ordinal-group: 1;\r\n                -ms-flex-order: 0;\r\n                order: 0\r\n            }\r\n\r\n            .order-xl-1 {\r\n                -webkit-box-ordinal-group: 2;\r\n                -ms-flex-order: 1;\r\n                order: 1\r\n            }\r\n\r\n            .order-xl-2 {\r\n                -webkit-box-ordinal-group: 3;\r\n                -ms-flex-order: 2;\r\n                order: 2\r\n            }\r\n\r\n            .order-xl-3 {\r\n                -webkit-box-ordinal-group: 4;\r\n                -ms-flex-order: 3;\r\n                order: 3\r\n            }\r\n\r\n            .order-xl-4 {\r\n                -webkit-box-ordinal-group: 5;\r\n                -ms-flex-order: 4;\r\n                order: 4\r\n            }\r\n\r\n            .order-xl-5 {\r\n                -webkit-box-ordinal-group: 6;\r\n                -ms-flex-order: 5;\r\n                order: 5\r\n            }\r\n\r\n            .order-xl-6 {\r\n                -webkit-box-ordinal-group: 7;\r\n                -ms-flex-order: 6;\r\n                order: 6\r\n            }\r\n\r\n            .order-xl-7 {\r\n                -webkit-box-ordinal-group: 8;\r\n                -ms-flex-order: 7;\r\n                order: 7\r\n            }\r\n\r\n            .order-xl-8 {\r\n                -webkit-box-ordinal-group: 9;\r\n                -ms-flex-order: 8;\r\n                order: 8\r\n            }\r\n\r\n            .order-xl-9 {\r\n                -webkit-box-ordinal-group: 10;\r\n                -ms-flex-order: 9;\r\n                order: 9\r\n            }\r\n\r\n            .order-xl-10 {\r\n                -webkit-box-ordinal-group: 11;\r\n                -ms-flex-order: 10;\r\n                order: 10\r\n            }\r\n\r\n            .order-xl-11 {\r\n                -webkit-box-ordinal-group: 12;\r\n                -ms-flex-order: 11;\r\n                order: 11\r\n            }\r\n\r\n            .order-xl-12 {\r\n                -webkit-box-ordinal-group: 13;\r\n                -ms-flex-order: 12;\r\n                order: 12\r\n            }\r\n\r\n            .offset-xl-0 {\r\n                margin-left: 0\r\n            }\r\n\r\n            .offset-xl-1 {\r\n                margin-left: 8.333333%\r\n            }\r\n\r\n            .offset-xl-2 {\r\n                margin-left: 16.666667%\r\n            }\r\n\r\n            .offset-xl-3 {\r\n                margin-left: 25%\r\n            }\r\n\r\n            .offset-xl-4 {\r\n                margin-left: 33.333333%\r\n            }\r\n\r\n            .offset-xl-5 {\r\n                margin-left: 41.666667%\r\n            }\r\n\r\n            .offset-xl-6 {\r\n                margin-left: 50%\r\n            }\r\n\r\n            .offset-xl-7 {\r\n                margin-left: 58.333333%\r\n            }\r\n\r\n            .offset-xl-8 {\r\n                margin-left: 66.666667%\r\n            }\r\n\r\n            .offset-xl-9 {\r\n                margin-left: 75%\r\n            }\r\n\r\n            .offset-xl-10 {\r\n                margin-left: 83.333333%\r\n            }\r\n\r\n            .offset-xl-11 {\r\n                margin-left: 91.666667%\r\n            }\r\n        }\r\n\r\n        .table {\r\n            width: 100%;\r\n            max-width: 100%;\r\n            margin-bottom: 1rem;\r\n            background-color: transparent\r\n        }\r\n\r\n            .table td, .table th {\r\n                padding: .75rem;\r\n                vertical-align: top;\r\n                border-top: 1px solid #dee2e6\r\n            }\r\n\r\n            .table thead th {\r\n                vertical-align: bottom;\r\n                border-bottom: 2px solid #dee2e6\r\n            }\r\n\r\n            .table tbody + tbody {\r\n                border-top: 2px solid #dee2e6\r\n            }\r\n\r\n            .table .table {\r\n                background-color: #fff\r\n            }\r\n\r\n        .table-sm td, .table-sm th {\r\n            padding: .3rem\r\n        }\r\n\r\n        .table-bordered {\r\n            border: 1px solid #dee2e6\r\n        }\r\n\r\n            .table-bordered td, .table-bordered th {\r\n                border: 1px solid #dee2e6\r\n            }\r\n\r\n            .table-bordered thead td, .table-bordered thead th {\r\n                border-bottom-width: 2px\r\n            }\r\n\r\n        .table-striped tbody tr:nth-of-type(odd) {\r\n            background-color: rgba(0,0,0,.05)\r\n        }\r\n\r\n        .table-hover tbody tr:hover {\r\n            background-color: rgba(0,0,0,.075)\r\n        }\r\n\r\n        .table-primary, .table-primary > td, .table-primary > th {\r\n            background-color: #b8daff\r\n        }\r\n\r\n        .table-hover .table-primary:hover {\r\n            background-color: #9fcdff\r\n        }\r\n\r\n            .table-hover .table-primary:hover > td, .table-hover .table-primary:hover > th {\r\n                background-color: #9fcdff\r\n            }\r\n\r\n        .table-secondary, .table-secondary > td, .table-secondary > th {\r\n            background-color: #d6d8db\r\n        }\r\n\r\n        .table-hover .table-secondary:hover {\r\n            background-color: #c8cbcf\r\n        }\r\n\r\n            .table-hover .table-secondary:hover > td, .table-hover .table-secondary:hover > th {\r\n                background-color: #c8cbcf\r\n            }\r\n\r\n        .table-success, .table-success > td, .table-success > th {\r\n            background-color: #c3e6cb\r\n        }\r\n\r\n        .table-hover .table-success:hover {\r\n            background-color: #b1dfbb\r\n        }\r\n\r\n            .table-hover .table-success:hover > td, .table-hover .table-success:hover > th {\r\n                background-color: #b1dfbb\r\n            }\r\n\r\n        .table-info, .table-info > td, .table-info > th {\r\n            background-color: #bee5eb\r\n        }\r\n\r\n        .table-hover .table-info:hover {\r\n            background-color: #abdde5\r\n        }\r\n\r\n            .table-hover .table-info:hover > td, .table-hover .table-info:hover > th {\r\n                background-color: #abdde5\r\n            }\r\n\r\n        .table-warning, .table-warning > td, .table-warning > th {\r\n            background-color: #ffeeba\r\n        }\r\n\r\n        .table-hover .table-warning:hover {\r\n            background-color: #ffe8a1\r\n        }\r\n\r\n            .table-hover .table-warning:hover > td, .table-hover .table-warning:hover > th {\r\n                background-color: #ffe8a1\r\n            }\r\n\r\n        .table-danger, .table-danger > td, .table-danger > th {\r\n            background-color: #f5c6cb\r\n        }\r\n\r\n        .table-hover .table-danger:hover {\r\n            background-color: #f1b0b7\r\n        }\r\n\r\n            .table-hover .table-danger:hover > td, .table-hover .table-danger:hover > th {\r\n                background-color: #f1b0b7\r\n            }\r\n\r\n        .table-light, .table-light > td, .table-light > th {\r\n            background-color: #fdfdfe\r\n        }\r\n\r\n        .table-hover .table-light:hover {\r\n            background-color: #ececf6\r\n        }\r\n\r\n            .table-hover .table-light:hover > td, .table-hover .table-light:hover > th {\r\n                background-color: #ececf6\r\n            }\r\n\r\n        .table-dark, .table-dark > td, .table-dark > th {\r\n            background-color: #c6c8ca\r\n        }\r\n\r\n        .table-hover .table-dark:hover {\r\n            background-color: #b9bbbe\r\n        }\r\n\r\n            .table-hover .table-dark:hover > td, .table-hover .table-dark:hover > th {\r\n                background-color: #b9bbbe\r\n            }\r\n\r\n        .table-active, .table-active > td, .table-active > th {\r\n            background-color: rgba(0,0,0,.075)\r\n        }\r\n\r\n        .table-hover .table-active:hover {\r\n            background-color: rgba(0,0,0,.075)\r\n        }\r\n\r\n            .table-hover .table-active:hover > td, .table-hover .table-active:hover > th {\r\n                background-color: rgba(0,0,0,.075)\r\n            }\r\n\r\n        .table .thead-dark th {\r\n            color: #fff;\r\n            background-color: #212529;\r\n            border-color: #32383e\r\n        }\r\n\r\n        .table .thead-light th {\r\n            color: #495057;\r\n            background-color: #e9ecef;\r\n            border-color: #dee2e6\r\n        }\r\n\r\n        .table-dark {\r\n            color: #fff;\r\n            background-color: #212529\r\n        }\r\n\r\n            .table-dark td, .table-dark th, .table-dark thead th {\r\n                border-color: #32383e\r\n            }\r\n\r\n            .table-dark.table-bordered {\r\n                border: 0\r\n            }\r\n\r\n            .table-dark.table-striped tbody tr:nth-of-type(odd) {\r\n                background-color: rgba(255,255,255,.05)\r\n            }\r\n\r\n            .table-dark.table-hover tbody tr:hover {\r\n                background-color: rgba(255,255,255,.075)\r\n            }\r\n\r\n        @media (max-width:575.98px) {\r\n            .table-responsive-sm {\r\n                display: block;\r\n                width: 100%;\r\n                overflow-x: auto;\r\n                -webkit-overflow-scrolling: touch;\r\n                -ms-overflow-style: -ms-autohiding-scrollbar\r\n            }\r\n\r\n                .table-responsive-sm > .table-bordered {\r\n                    border: 0\r\n                }\r\n        }\r\n\r\n        @media (max-width:767.98px) {\r\n            .table-responsive-md {\r\n                display: block;\r\n                width: 100%;\r\n                overflow-x: auto;\r\n                -webkit-overflow-scrolling: touch;\r\n                -ms-overflow-style: -ms-autohiding-scrollbar\r\n            }\r\n\r\n                .table-responsive-md > .table-bordered {\r\n                    border: 0\r\n                }\r\n        }\r\n\r\n        @media (max-width:991.98px) {\r\n            .table-responsive-lg {\r\n                display: block;\r\n                width: 100%;\r\n                overflow-x: auto;\r\n                -webkit-overflow-scrolling: touch;\r\n                -ms-overflow-style: -ms-autohiding-scrollbar\r\n            }\r\n\r\n                .table-responsive-lg > .table-bordered {\r\n                    border: 0\r\n                }\r\n        }\r\n\r\n        @media (max-width:1199.98px) {\r\n            .table-responsive-xl {\r\n                display: block;\r\n                width: 100%;\r\n                overflow-x: auto;\r\n                -webkit-overflow-scrolling: touch;\r\n                -ms-overflow-style: -ms-autohiding-scrollbar\r\n            }\r\n\r\n                .table-responsive-xl > .table-bordered {\r\n                    border: 0\r\n                }\r\n        }\r\n\r\n        .table-responsive {\r\n            display: block;\r\n            width: 100%;\r\n            overflow-x: auto;\r\n            -webkit-overflow-scrolling: touch;\r\n            -ms-overflow-style: -ms-autohiding-scrollbar\r\n        }\r\n\r\n            .table-responsive > .table-bordered {\r\n                border: 0\r\n            }\r\n\r\n        .form-control {\r\n            display: block;\r\n            width: 100%;\r\n            padding: .375rem .75rem;\r\n            font-size: 1rem;\r\n            line-height: 1.5;\r\n            color: #495057;\r\n            background-color: #fff;\r\n            background-clip: padding-box;\r\n            border: 1px solid #ced4da;\r\n            border-radius: .25rem;\r\n            transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out\r\n        }\r\n\r\n            .form-control::-ms-expand {\r\n                background-color: transparent;\r\n                border: 0\r\n            }\r\n\r\n            .form-control:focus {\r\n                color: #495057;\r\n                background-color: #fff;\r\n                border-color: #80bdff;\r\n                outline: 0;\r\n                box-shadow: 0 0 0 .2rem rgba(0,123,255,.25)\r\n            }\r\n\r\n            .form-control::-webkit-input-placeholder {\r\n                color: #6c757d;\r\n                opacity: 1\r\n            }\r\n\r\n            .form-control::-moz-placeholder {\r\n                color: #6c757d;\r\n                opacity: 1\r\n            }\r\n\r\n            .form-control:-ms-input-placeholder {\r\n                color: #6c757d;\r\n                opacity: 1\r\n            }\r\n\r\n            .form-control::-ms-input-placeholder {\r\n                color: #6c757d;\r\n                opacity: 1\r\n            }\r\n\r\n            .form-control::placeholder {\r\n                color: #6c757d;\r\n                opacity: 1\r\n            }\r\n\r\n            .form-control:disabled, .form-control[readonly] {\r\n                background-color: #e9ecef;\r\n                opacity: 1\r\n            }\r\n\r\n        select.form-control:not([size]):not([multiple]) {\r\n            height: calc(2.25rem + 2px)\r\n        }\r\n\r\n        select.form-control:focus::-ms-value {\r\n            color: #495057;\r\n            background-color: #fff\r\n        }\r\n\r\n        .form-control-file, .form-control-range {\r\n            display: block;\r\n            width: 100%\r\n        }\r\n\r\n        .col-form-label {\r\n            padding-top: calc(.375rem + 1px);\r\n            padding-bottom: calc(.375rem + 1px);\r\n            margin-bottom: 0;\r\n            font-size: inherit;\r\n            line-height: 1.5\r\n        }\r\n\r\n        .col-form-label-lg {\r\n            padding-top: calc(.5rem + 1px);\r\n            padding-bottom: calc(.5rem + 1px);\r\n            font-size: 1.25rem;\r\n            line-height: 1.5\r\n        }\r\n\r\n        .col-form-label-sm {\r\n            padding-top: calc(.25rem + 1px);\r\n            padding-bottom: calc(.25rem + 1px);\r\n            font-size: .875rem;\r\n            line-height: 1.5\r\n        }\r\n\r\n        .form-control-plaintext {\r\n            display: block;\r\n            width: 100%;\r\n            padding-top: .375rem;\r\n            padding-bottom: .375rem;\r\n            margin-bottom: 0;\r\n            line-height: 1.5;\r\n            background-color: transparent;\r\n            border: solid transparent;\r\n            border-width: 1px 0\r\n        }\r\n\r\n            .form-control-plaintext.form-control-lg, .form-control-plaintext.form-control-sm, .input-group-lg > .form-control-plaintext.form-control, .input-group-lg > .input-group-append > .form-control-plaintext.btn, .input-group-lg > .input-group-append > .form-control-plaintext.input-group-text, .input-group-lg > .input-group-prepend > .form-control-plaintext.btn, .input-group-lg > .input-group-prepend > .form-control-plaintext.input-group-text, .input-group-sm > .form-control-plaintext.form-control, .input-group-sm > .input-group-append > .form-control-plaintext.btn, .input-group-sm > .input-group-append > .form-control-plaintext.input-group-text, .input-group-sm > .input-group-prepend > .form-control-plaintext.btn, .input-group-sm > .input-group-prepend > .form-control-plaintext.input-group-text {\r\n                padding-right: 0;\r\n                padding-left: 0\r\n            }\r\n\r\n        .form-control-sm, .input-group-sm > .form-control, .input-group-sm > .input-group-append > .btn, .input-group-sm > .input-group-append > .input-group-text, .input-group-sm > .input-group-prepend > .btn, .input-group-sm > .input-group-prepend > .input-group-text {\r\n            padding: .25rem .5rem;\r\n            font-size: .875rem;\r\n            line-height: 1.5;\r\n            border-radius: .2rem\r\n        }\r\n\r\n        .input-group-sm > .input-group-append > select.btn:not([size]):not([multiple]), .input-group-sm > .input-group-append > select.input-group-text:not([size]):not([multiple]), .input-group-sm > .input-group-prepend > select.btn:not([size]):not([multiple]), .input-group-sm > .input-group-prepend > select.input-group-text:not([size]):not([multiple]), .input-group-sm > select.form-control:not([size]):not([multiple]), select.form-control-sm:not([size]):not([multiple]) {\r\n            height: calc(1.8125rem + 2px)\r\n        }\r\n\r\n        .form-control-lg, .input-group-lg > .form-control, .input-group-lg > .input-group-append > .btn, .input-group-lg > .input-group-append > .input-group-text, .input-group-lg > .input-group-prepend > .btn, .input-group-lg > .input-group-prepend > .input-group-text {\r\n            padding: .5rem 1rem;\r\n            font-size: 1.25rem;\r\n            line-height: 1.5;\r\n            border-radius: .3rem\r\n        }\r\n\r\n        .input-group-lg > .input-group-append > select.btn:not([size]):not([multiple]), .input-group-lg > .input-group-append > select.input-group-text:not([size]):not([multiple]), .input-group-lg > .input-group-prepend > select.btn:not([size]):not([multiple]), .input-group-lg > .input-group-prepend > select.input-group-text:not([size]):not([multiple]), .input-group-lg > select.form-control:not([size]):not([multiple]), select.form-control-lg:not([size]):not([multiple]) {\r\n            height: calc(2.875rem + 2px)\r\n        }\r\n\r\n        .form-group {\r\n            margin-bottom: 1rem\r\n        }\r\n\r\n        .form-text {\r\n            display: block;\r\n            margin-top: .25rem\r\n        }\r\n\r\n        .form-row {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -ms-flex-wrap: wrap;\r\n            flex-wrap: wrap;\r\n            margin-right: -5px;\r\n            margin-left: -5px\r\n        }\r\n\r\n            .form-row > .col, .form-row > [class*=col-] {\r\n                padding-right: 5px;\r\n                padding-left: 5px\r\n            }\r\n\r\n        .form-check {\r\n            position: relative;\r\n            display: block;\r\n            padding-left: 1.25rem\r\n        }\r\n\r\n        .form-check-input {\r\n            position: absolute;\r\n            margin-top: .3rem;\r\n            margin-left: -1.25rem\r\n        }\r\n\r\n            .form-check-input:disabled ~ .form-check-label {\r\n                color: #6c757d\r\n            }\r\n\r\n        .form-check-label {\r\n            margin-bottom: 0\r\n        }\r\n\r\n        .form-check-inline {\r\n            display: -webkit-inline-box;\r\n            display: -ms-inline-flexbox;\r\n            display: inline-flex;\r\n            -webkit-box-align: center;\r\n            -ms-flex-align: center;\r\n            align-items: center;\r\n            padding-left: 0;\r\n            margin-right: .75rem\r\n        }\r\n\r\n            .form-check-inline .form-check-input {\r\n                position: static;\r\n                margin-top: 0;\r\n                margin-right: .3125rem;\r\n                margin-left: 0\r\n            }\r\n\r\n        .valid-feedback {\r\n            display: none;\r\n            width: 100%;\r\n            margin-top: .25rem;\r\n            font-size: 80%;\r\n            color: #28a745\r\n        }\r\n\r\n        .valid-tooltip {\r\n            position: absolute;\r\n            top: 100%;\r\n            z-index: 5;\r\n            display: none;\r\n            max-width: 100%;\r\n            padding: .5rem;\r\n            margin-top: .1rem;\r\n            font-size: .875rem;\r\n            line-height: 1;\r\n            color: #fff;\r\n            background-color: rgba(40,167,69,.8);\r\n            border-radius: .2rem\r\n        }\r\n\r\n        .custom-select.is-valid, .form-control.is-valid, .was-validated .custom-select:valid, .was-validated .form-control:valid {\r\n            border-color: #28a745\r\n        }\r\n\r\n            .custom-select.is-valid:focus, .form-control.is-valid:focus, .was-validated .custom-select:valid:focus, .was-validated .form-control:valid:focus {\r\n                border-color: #28a745;\r\n                box-shadow: 0 0 0 .2rem rgba(40,167,69,.25)\r\n            }\r\n\r\n            .custom-select.is-valid ~ .valid-feedback, .custom-select.is-valid ~ .valid-tooltip, .form-control.is-valid ~ .valid-feedback, .form-control.is-valid ~ .valid-tooltip, .was-validated .custom-select:valid ~ .valid-feedback, .was-validated .custom-select:valid ~ .valid-tooltip, .was-validated .form-control:valid ~ .valid-feedback, .was-validated .form-control:valid ~ .valid-tooltip {\r\n                display: block\r\n            }\r\n\r\n        .form-check-input.is-valid ~ .form-check-label, .was-validated .form-check-input:valid ~ .form-check-label {\r\n            color: #28a745\r\n        }\r\n\r\n        .form-check-input.is-valid ~ .valid-feedback, .form-check-input.is-valid ~ .valid-tooltip, .was-validated .form-check-input:valid ~ .valid-feedback, .was-validated .form-check-input:valid ~ .valid-tooltip {\r\n            display: block\r\n        }\r\n\r\n        .custom-control-input.is-valid ~ .custom-control-label, .was-validated .custom-control-input:valid ~ .custom-control-label {\r\n            color: #28a745\r\n        }\r\n\r\n            .custom-control-input.is-valid ~ .custom-control-label::before, .was-validated .custom-control-input:valid ~ .custom-control-label::before {\r\n                background-color: #71dd8a\r\n            }\r\n\r\n        .custom-control-input.is-valid ~ .valid-feedback, .custom-control-input.is-valid ~ .valid-tooltip, .was-validated .custom-control-input:valid ~ .valid-feedback, .was-validated .custom-control-input:valid ~ .valid-tooltip {\r\n            display: block\r\n        }\r\n\r\n        .custom-control-input.is-valid:checked ~ .custom-control-label::before, .was-validated .custom-control-input:valid:checked ~ .custom-control-label::before {\r\n            background-color: #34ce57\r\n        }\r\n\r\n        .custom-control-input.is-valid:focus ~ .custom-control-label::before, .was-validated .custom-control-input:valid:focus ~ .custom-control-label::before {\r\n            box-shadow: 0 0 0 1px #fff,0 0 0 .2rem rgba(40,167,69,.25)\r\n        }\r\n\r\n        .custom-file-input.is-valid ~ .custom-file-label, .was-validated .custom-file-input:valid ~ .custom-file-label {\r\n            border-color: #28a745\r\n        }\r\n\r\n            .custom-file-input.is-valid ~ .custom-file-label::before, .was-validated .custom-file-input:valid ~ .custom-file-label::before {\r\n                border-color: inherit\r\n            }\r\n\r\n        .custom-file-input.is-valid ~ .valid-feedback, .custom-file-input.is-valid ~ .valid-tooltip, .was-validated .custom-file-input:valid ~ .valid-feedback, .was-validated .custom-file-input:valid ~ .valid-tooltip {\r\n            display: block\r\n        }\r\n\r\n        .custom-file-input.is-valid:focus ~ .custom-file-label, .was-validated .custom-file-input:valid:focus ~ .custom-file-label {\r\n            box-shadow: 0 0 0 .2rem rgba(40,167,69,.25)\r\n        }\r\n\r\n        .invalid-feedback {\r\n            display: none;\r\n            width: 100%;\r\n            margin-top: .25rem;\r\n            font-size: 80%;\r\n            color: #dc3545\r\n        }\r\n\r\n        .invalid-tooltip {\r\n            position: absolute;\r\n            top: 100%;\r\n            z-index: 5;\r\n            display: none;\r\n            max-width: 100%;\r\n            padding: .5rem;\r\n            margin-top: .1rem;\r\n            font-size: .875rem;\r\n            line-height: 1;\r\n            color: #fff;\r\n            background-color: rgba(220,53,69,.8);\r\n            border-radius: .2rem\r\n        }\r\n\r\n        .custom-select.is-invalid, .form-control.is-invalid, .was-validated .custom-select:invalid, .was-validated .form-control:invalid {\r\n            border-color: #dc3545\r\n        }\r\n\r\n            .custom-select.is-invalid:focus, .form-control.is-invalid:focus, .was-validated .custom-select:invalid:focus, .was-validated .form-control:invalid:focus {\r\n                border-color: #dc3545;\r\n                box-shadow: 0 0 0 .2rem rgba(220,53,69,.25)\r\n            }\r\n\r\n            .custom-select.is-invalid ~ .invalid-feedback, .custom-select.is-invalid ~ .invalid-tooltip, .form-control.is-invalid ~ .invalid-feedback, .form-control.is-invalid ~ .invalid-tooltip, .was-validated .custom-select:invalid ~ .invalid-feedback, .was-validated .custom-select:invalid ~ .invalid-tooltip, .was-validated .form-control:invalid ~ .invalid-feedback, .was-validated .form-control:invalid ~ .invalid-tooltip {\r\n                display: block\r\n            }\r\n\r\n        .form-check-input.is-invalid ~ .form-check-label, .was-validated .form-check-input:invalid ~ .form-check-label {\r\n            color: #dc3545\r\n        }\r\n\r\n        .form-check-input.is-invalid ~ .invalid-feedback, .form-check-input.is-invalid ~ .invalid-tooltip, .was-validated .form-check-input:invalid ~ .invalid-feedback, .was-validated .form-check-input:invalid ~ .invalid-tooltip {\r\n            display: block\r\n        }\r\n\r\n        .custom-control-input.is-invalid ~ .custom-control-label, .was-validated .custom-control-input:invalid ~ .custom-control-label {\r\n            color: #dc3545\r\n        }\r\n\r\n            .custom-control-input.is-invalid ~ .custom-control-label::before, .was-validated .custom-control-input:invalid ~ .custom-control-label::before {\r\n                background-color: #efa2a9\r\n            }\r\n\r\n        .custom-control-input.is-invalid ~ .invalid-feedback, .custom-control-input.is-invalid ~ .invalid-tooltip, .was-validated .custom-control-input:invalid ~ .invalid-feedback, .was-validated .custom-control-input:invalid ~ .invalid-tooltip {\r\n            display: block\r\n        }\r\n\r\n        .custom-control-input.is-invalid:checked ~ .custom-control-label::before, .was-validated .custom-control-input:invalid:checked ~ .custom-control-label::before {\r\n            background-color: #e4606d\r\n        }\r\n\r\n        .custom-control-input.is-invalid:focus ~ .custom-control-label::before, .was-validated .custom-control-input:invalid:focus ~ .custom-control-label::before {\r\n            box-shadow: 0 0 0 1px #fff,0 0 0 .2rem rgba(220,53,69,.25)\r\n        }\r\n\r\n        .custom-file-input.is-invalid ~ .custom-file-label, .was-validated .custom-file-input:invalid ~ .custom-file-label {\r\n            border-color: #dc3545\r\n        }\r\n\r\n            .custom-file-input.is-invalid ~ .custom-file-label::before, .was-validated .custom-file-input:invalid ~ .custom-file-label::before {\r\n                border-color: inherit\r\n            }\r\n\r\n        .custom-file-input.is-invalid ~ .invalid-feedback, .custom-file-input.is-invalid ~ .invalid-tooltip, .was-validated .custom-file-input:invalid ~ .invalid-feedback, .was-validated .custom-file-input:invalid ~ .invalid-tooltip {\r\n            display: block\r\n        }\r\n\r\n        .custom-file-input.is-invalid:focus ~ .custom-file-label, .was-validated .custom-file-input:invalid:focus ~ .custom-file-label {\r\n            box-shadow: 0 0 0 .2rem rgba(220,53,69,.25)\r\n        }\r\n\r\n        .form-inline {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-orient: horizontal;\r\n            -webkit-box-direction: normal;\r\n            -ms-flex-flow: row wrap;\r\n            flex-flow: row wrap;\r\n            -webkit-box-align: center;\r\n            -ms-flex-align: center;\r\n            align-items: center\r\n        }\r\n\r\n            .form-inline .form-check {\r\n                width: 100%\r\n            }\r\n\r\n        @media (min-width:576px) {\r\n            .form-inline label {\r\n                display: -webkit-box;\r\n                display: -ms-flexbox;\r\n                display: flex;\r\n                -webkit-box-align: center;\r\n                -ms-flex-align: center;\r\n                align-items: center;\r\n                -webkit-box-pack: center;\r\n                -ms-flex-pack: center;\r\n                justify-content: center;\r\n                margin-bottom: 0\r\n            }\r\n\r\n            .form-inline .form-group {\r\n                display: -webkit-box;\r\n                display: -ms-flexbox;\r\n                display: flex;\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 0 auto;\r\n                flex: 0 0 auto;\r\n                -webkit-box-orient: horizontal;\r\n                -webkit-box-direction: normal;\r\n                -ms-flex-flow: row wrap;\r\n                flex-flow: row wrap;\r\n                -webkit-box-align: center;\r\n                -ms-flex-align: center;\r\n                align-items: center;\r\n                margin-bottom: 0\r\n            }\r\n\r\n            .form-inline .form-control {\r\n                display: inline-block;\r\n                width: auto;\r\n                vertical-align: middle\r\n            }\r\n\r\n            .form-inline .form-control-plaintext {\r\n                display: inline-block\r\n            }\r\n\r\n            .form-inline .input-group {\r\n                width: auto\r\n            }\r\n\r\n            .form-inline .form-check {\r\n                display: -webkit-box;\r\n                display: -ms-flexbox;\r\n                display: flex;\r\n                -webkit-box-align: center;\r\n                -ms-flex-align: center;\r\n                align-items: center;\r\n                -webkit-box-pack: center;\r\n                -ms-flex-pack: center;\r\n                justify-content: center;\r\n                width: auto;\r\n                padding-left: 0\r\n            }\r\n\r\n            .form-inline .form-check-input {\r\n                position: relative;\r\n                margin-top: 0;\r\n                margin-right: .25rem;\r\n                margin-left: 0\r\n            }\r\n\r\n            .form-inline .custom-control {\r\n                -webkit-box-align: center;\r\n                -ms-flex-align: center;\r\n                align-items: center;\r\n                -webkit-box-pack: center;\r\n                -ms-flex-pack: center;\r\n                justify-content: center\r\n            }\r\n\r\n            .form-inline .custom-control-label {\r\n                margin-bottom: 0\r\n            }\r\n        }\r\n\r\n        .btn {\r\n            display: inline-block;\r\n            font-weight: 400;\r\n            text-align: center;\r\n            white-space: nowrap;\r\n            vertical-align: middle;\r\n            -webkit-user-select: none;\r\n            -moz-user-select: none;\r\n            -ms-user-select: none;\r\n            user-select: none;\r\n            border: 1px solid transparent;\r\n            padding: .375rem .75rem;\r\n            font-size: 1rem;\r\n            line-height: 1.5;\r\n            border-radius: .25rem;\r\n            transition: color .15s ease-in-out,background-color .15s ease-in-out,border-color .15s ease-in-out,box-shadow .15s ease-in-out\r\n        }\r\n\r\n            .btn:focus, .btn:hover {\r\n                text-decoration: none\r\n            }\r\n\r\n            .btn.focus, .btn:focus {\r\n                outline: 0;\r\n                box-shadow: 0 0 0 .2rem rgba(0,123,255,.25)\r\n            }\r\n\r\n            .btn.disabled, .btn:disabled {\r\n                opacity: .65\r\n            }\r\n\r\n            .btn:not(:disabled):not(.disabled) {\r\n                cursor: pointer\r\n            }\r\n\r\n                .btn:not(:disabled):not(.disabled).active, .btn:not(:disabled):not(.disabled):active {\r\n                    background-image: none\r\n                }\r\n\r\n        a.btn.disabled, fieldset:disabled a.btn {\r\n            pointer-events: none\r\n        }\r\n\r\n        .btn-primary {\r\n            color: #fff;\r\n            background-color: #007bff;\r\n            border-color: #007bff\r\n        }\r\n\r\n            .btn-primary:hover {\r\n                color: #fff;\r\n                background-color: #0069d9;\r\n                border-color: #0062cc\r\n            }\r\n\r\n            .btn-primary.focus, .btn-primary:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(0,123,255,.5)\r\n            }\r\n\r\n            .btn-primary.disabled, .btn-primary:disabled {\r\n                color: #fff;\r\n                background-color: #007bff;\r\n                border-color: #007bff\r\n            }\r\n\r\n            .btn-primary:not(:disabled):not(.disabled).active, .btn-primary:not(:disabled):not(.disabled):active, .show > .btn-primary.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #0062cc;\r\n                border-color: #005cbf\r\n            }\r\n\r\n                .btn-primary:not(:disabled):not(.disabled).active:focus, .btn-primary:not(:disabled):not(.disabled):active:focus, .show > .btn-primary.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(0,123,255,.5)\r\n                }\r\n\r\n        .btn-secondary {\r\n            color: #fff;\r\n            background-color: #6c757d;\r\n            border-color: #6c757d\r\n        }\r\n\r\n            .btn-secondary:hover {\r\n                color: #fff;\r\n                background-color: #5a6268;\r\n                border-color: #545b62\r\n            }\r\n\r\n            .btn-secondary.focus, .btn-secondary:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(108,117,125,.5)\r\n            }\r\n\r\n            .btn-secondary.disabled, .btn-secondary:disabled {\r\n                color: #fff;\r\n                background-color: #6c757d;\r\n                border-color: #6c757d\r\n            }\r\n\r\n            .btn-secondary:not(:disabled):not(.disabled).active, .btn-secondary:not(:disabled):not(.disabled):active, .show > .btn-secondary.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #545b62;\r\n                border-color: #4e555b\r\n            }\r\n\r\n                .btn-secondary:not(:disabled):not(.disabled).active:focus, .btn-secondary:not(:disabled):not(.disabled):active:focus, .show > .btn-secondary.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(108,117,125,.5)\r\n                }\r\n\r\n        .btn-success {\r\n            color: #fff;\r\n            background-color: #28a745;\r\n            border-color: #28a745\r\n        }\r\n\r\n            .btn-success:hover {\r\n                color: #fff;\r\n                background-color: #218838;\r\n                border-color: #1e7e34\r\n            }\r\n\r\n            .btn-success.focus, .btn-success:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(40,167,69,.5)\r\n            }\r\n\r\n            .btn-success.disabled, .btn-success:disabled {\r\n                color: #fff;\r\n                background-color: #28a745;\r\n                border-color: #28a745\r\n            }\r\n\r\n            .btn-success:not(:disabled):not(.disabled).active, .btn-success:not(:disabled):not(.disabled):active, .show > .btn-success.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #1e7e34;\r\n                border-color: #1c7430\r\n            }\r\n\r\n                .btn-success:not(:disabled):not(.disabled).active:focus, .btn-success:not(:disabled):not(.disabled):active:focus, .show > .btn-success.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(40,167,69,.5)\r\n                }\r\n\r\n        .btn-info {\r\n            color: #fff;\r\n            background-color: #17a2b8;\r\n            border-color: #17a2b8\r\n        }\r\n\r\n            .btn-info:hover {\r\n                color: #fff;\r\n                background-color: #138496;\r\n                border-color: #117a8b\r\n            }\r\n\r\n            .btn-info.focus, .btn-info:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(23,162,184,.5)\r\n            }\r\n\r\n            .btn-info.disabled, .btn-info:disabled {\r\n                color: #fff;\r\n                background-color: #17a2b8;\r\n                border-color: #17a2b8\r\n            }\r\n\r\n            .btn-info:not(:disabled):not(.disabled).active, .btn-info:not(:disabled):not(.disabled):active, .show > .btn-info.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #117a8b;\r\n                border-color: #10707f\r\n            }\r\n\r\n                .btn-info:not(:disabled):not(.disabled).active:focus, .btn-info:not(:disabled):not(.disabled):active:focus, .show > .btn-info.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(23,162,184,.5)\r\n                }\r\n\r\n        .btn-warning {\r\n            color: #212529;\r\n            background-color: #ffc107;\r\n            border-color: #ffc107\r\n        }\r\n\r\n            .btn-warning:hover {\r\n                color: #212529;\r\n                background-color: #e0a800;\r\n                border-color: #d39e00\r\n            }\r\n\r\n            .btn-warning.focus, .btn-warning:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(255,193,7,.5)\r\n            }\r\n\r\n            .btn-warning.disabled, .btn-warning:disabled {\r\n                color: #212529;\r\n                background-color: #ffc107;\r\n                border-color: #ffc107\r\n            }\r\n\r\n            .btn-warning:not(:disabled):not(.disabled).active, .btn-warning:not(:disabled):not(.disabled):active, .show > .btn-warning.dropdown-toggle {\r\n                color: #212529;\r\n                background-color: #d39e00;\r\n                border-color: #c69500\r\n            }\r\n\r\n                .btn-warning:not(:disabled):not(.disabled).active:focus, .btn-warning:not(:disabled):not(.disabled):active:focus, .show > .btn-warning.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(255,193,7,.5)\r\n                }\r\n\r\n        .btn-danger {\r\n            color: #fff;\r\n            background-color: #dc3545;\r\n            border-color: #dc3545\r\n        }\r\n\r\n            .btn-danger:hover {\r\n                color: #fff;\r\n                background-color: #c82333;\r\n                border-color: #bd2130\r\n            }\r\n\r\n            .btn-danger.focus, .btn-danger:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(220,53,69,.5)\r\n            }\r\n\r\n            .btn-danger.disabled, .btn-danger:disabled {\r\n                color: #fff;\r\n                background-color: #dc3545;\r\n                border-color: #dc3545\r\n            }\r\n\r\n            .btn-danger:not(:disabled):not(.disabled).active, .btn-danger:not(:disabled):not(.disabled):active, .show > .btn-danger.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #bd2130;\r\n                border-color: #b21f2d\r\n            }\r\n\r\n                .btn-danger:not(:disabled):not(.disabled).active:focus, .btn-danger:not(:disabled):not(.disabled):active:focus, .show > .btn-danger.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(220,53,69,.5)\r\n                }\r\n\r\n        .btn-light {\r\n            color: #212529;\r\n            background-color: #f8f9fa;\r\n            border-color: #f8f9fa\r\n        }\r\n\r\n            .btn-light:hover {\r\n                color: #212529;\r\n                background-color: #e2e6ea;\r\n                border-color: #dae0e5\r\n            }\r\n\r\n            .btn-light.focus, .btn-light:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(248,249,250,.5)\r\n            }\r\n\r\n            .btn-light.disabled, .btn-light:disabled {\r\n                color: #212529;\r\n                background-color: #f8f9fa;\r\n                border-color: #f8f9fa\r\n            }\r\n\r\n            .btn-light:not(:disabled):not(.disabled).active, .btn-light:not(:disabled):not(.disabled):active, .show > .btn-light.dropdown-toggle {\r\n                color: #212529;\r\n                background-color: #dae0e5;\r\n                border-color: #d3d9df\r\n            }\r\n\r\n                .btn-light:not(:disabled):not(.disabled).active:focus, .btn-light:not(:disabled):not(.disabled):active:focus, .show > .btn-light.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(248,249,250,.5)\r\n                }\r\n\r\n        .btn-dark {\r\n            color: #fff;\r\n            background-color: #343a40;\r\n            border-color: #343a40\r\n        }\r\n\r\n            .btn-dark:hover {\r\n                color: #fff;\r\n                background-color: #23272b;\r\n                border-color: #1d2124\r\n            }\r\n\r\n            .btn-dark.focus, .btn-dark:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(52,58,64,.5)\r\n            }\r\n\r\n            .btn-dark.disabled, .btn-dark:disabled {\r\n                color: #fff;\r\n                background-color: #343a40;\r\n                border-color: #343a40\r\n            }\r\n\r\n            .btn-dark:not(:disabled):not(.disabled).active, .btn-dark:not(:disabled):not(.disabled):active, .show > .btn-dark.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #1d2124;\r\n                border-color: #171a1d\r\n            }\r\n\r\n                .btn-dark:not(:disabled):not(.disabled).active:focus, .btn-dark:not(:disabled):not(.disabled):active:focus, .show > .btn-dark.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(52,58,64,.5)\r\n                }\r\n\r\n        .btn-outline-primary {\r\n            color: #007bff;\r\n            background-color: transparent;\r\n            background-image: none;\r\n            border-color: #007bff\r\n        }\r\n\r\n            .btn-outline-primary:hover {\r\n                color: #fff;\r\n                background-color: #007bff;\r\n                border-color: #007bff\r\n            }\r\n\r\n            .btn-outline-primary.focus, .btn-outline-primary:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(0,123,255,.5)\r\n            }\r\n\r\n            .btn-outline-primary.disabled, .btn-outline-primary:disabled {\r\n                color: #007bff;\r\n                background-color: transparent\r\n            }\r\n\r\n            .btn-outline-primary:not(:disabled):not(.disabled).active, .btn-outline-primary:not(:disabled):not(.disabled):active, .show > .btn-outline-primary.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #007bff;\r\n                border-color: #007bff\r\n            }\r\n\r\n                .btn-outline-primary:not(:disabled):not(.disabled).active:focus, .btn-outline-primary:not(:disabled):not(.disabled):active:focus, .show > .btn-outline-primary.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(0,123,255,.5)\r\n                }\r\n\r\n        .btn-outline-secondary {\r\n            color: #6c757d;\r\n            background-color: transparent;\r\n            background-image: none;\r\n            border-color: #6c757d\r\n        }\r\n\r\n            .btn-outline-secondary:hover {\r\n                color: #fff;\r\n                background-color: #6c757d;\r\n                border-color: #6c757d\r\n            }\r\n\r\n            .btn-outline-secondary.focus, .btn-outline-secondary:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(108,117,125,.5)\r\n            }\r\n\r\n            .btn-outline-secondary.disabled, .btn-outline-secondary:disabled {\r\n                color: #6c757d;\r\n                background-color: transparent\r\n            }\r\n\r\n            .btn-outline-secondary:not(:disabled):not(.disabled).active, .btn-outline-secondary:not(:disabled):not(.disabled):active, .show > .btn-outline-secondary.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #6c757d;\r\n                border-color: #6c757d\r\n            }\r\n\r\n                .btn-outline-secondary:not(:disabled):not(.disabled).active:focus, .btn-outline-secondary:not(:disabled):not(.disabled):active:focus, .show > .btn-outline-secondary.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(108,117,125,.5)\r\n                }\r\n\r\n        .btn-outline-success {\r\n            color: #28a745;\r\n            background-color: transparent;\r\n            background-image: none;\r\n            border-color: #28a745\r\n        }\r\n\r\n            .btn-outline-success:hover {\r\n                color: #fff;\r\n                background-color: #28a745;\r\n                border-color: #28a745\r\n            }\r\n\r\n            .btn-outline-success.focus, .btn-outline-success:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(40,167,69,.5)\r\n            }\r\n\r\n            .btn-outline-success.disabled, .btn-outline-success:disabled {\r\n                color: #28a745;\r\n                background-color: transparent\r\n            }\r\n\r\n            .btn-outline-success:not(:disabled):not(.disabled).active, .btn-outline-success:not(:disabled):not(.disabled):active, .show > .btn-outline-success.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #28a745;\r\n                border-color: #28a745\r\n            }\r\n\r\n                .btn-outline-success:not(:disabled):not(.disabled).active:focus, .btn-outline-success:not(:disabled):not(.disabled):active:focus, .show > .btn-outline-success.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(40,167,69,.5)\r\n                }\r\n\r\n        .btn-outline-info {\r\n            color: #17a2b8;\r\n            background-color: transparent;\r\n            background-image: none;\r\n            border-color: #17a2b8\r\n        }\r\n\r\n            .btn-outline-info:hover {\r\n                color: #fff;\r\n                background-color: #17a2b8;\r\n                border-color: #17a2b8\r\n            }\r\n\r\n            .btn-outline-info.focus, .btn-outline-info:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(23,162,184,.5)\r\n            }\r\n\r\n            .btn-outline-info.disabled, .btn-outline-info:disabled {\r\n                color: #17a2b8;\r\n                background-color: transparent\r\n            }\r\n\r\n            .btn-outline-info:not(:disabled):not(.disabled).active, .btn-outline-info:not(:disabled):not(.disabled):active, .show > .btn-outline-info.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #17a2b8;\r\n                border-color: #17a2b8\r\n            }\r\n\r\n                .btn-outline-info:not(:disabled):not(.disabled).active:focus, .btn-outline-info:not(:disabled):not(.disabled):active:focus, .show > .btn-outline-info.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(23,162,184,.5)\r\n                }\r\n\r\n        .btn-outline-warning {\r\n            color: #ffc107;\r\n            background-color: transparent;\r\n            background-image: none;\r\n            border-color: #ffc107\r\n        }\r\n\r\n            .btn-outline-warning:hover {\r\n                color: #212529;\r\n                background-color: #ffc107;\r\n                border-color: #ffc107\r\n            }\r\n\r\n            .btn-outline-warning.focus, .btn-outline-warning:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(255,193,7,.5)\r\n            }\r\n\r\n            .btn-outline-warning.disabled, .btn-outline-warning:disabled {\r\n                color: #ffc107;\r\n                background-color: transparent\r\n            }\r\n\r\n            .btn-outline-warning:not(:disabled):not(.disabled).active, .btn-outline-warning:not(:disabled):not(.disabled):active, .show > .btn-outline-warning.dropdown-toggle {\r\n                color: #212529;\r\n                background-color: #ffc107;\r\n                border-color: #ffc107\r\n            }\r\n\r\n                .btn-outline-warning:not(:disabled):not(.disabled).active:focus, .btn-outline-warning:not(:disabled):not(.disabled):active:focus, .show > .btn-outline-warning.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(255,193,7,.5)\r\n                }\r\n\r\n        .btn-outline-danger {\r\n            color: #dc3545;\r\n            background-color: transparent;\r\n            background-image: none;\r\n            border-color: #dc3545\r\n        }\r\n\r\n            .btn-outline-danger:hover {\r\n                color: #fff;\r\n                background-color: #dc3545;\r\n                border-color: #dc3545\r\n            }\r\n\r\n            .btn-outline-danger.focus, .btn-outline-danger:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(220,53,69,.5)\r\n            }\r\n\r\n            .btn-outline-danger.disabled, .btn-outline-danger:disabled {\r\n                color: #dc3545;\r\n                background-color: transparent\r\n            }\r\n\r\n            .btn-outline-danger:not(:disabled):not(.disabled).active, .btn-outline-danger:not(:disabled):not(.disabled):active, .show > .btn-outline-danger.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #dc3545;\r\n                border-color: #dc3545\r\n            }\r\n\r\n                .btn-outline-danger:not(:disabled):not(.disabled).active:focus, .btn-outline-danger:not(:disabled):not(.disabled):active:focus, .show > .btn-outline-danger.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(220,53,69,.5)\r\n                }\r\n\r\n        .btn-outline-light {\r\n            color: #f8f9fa;\r\n            background-color: transparent;\r\n            background-image: none;\r\n            border-color: #f8f9fa\r\n        }\r\n\r\n            .btn-outline-light:hover {\r\n                color: #212529;\r\n                background-color: #f8f9fa;\r\n                border-color: #f8f9fa\r\n            }\r\n\r\n            .btn-outline-light.focus, .btn-outline-light:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(248,249,250,.5)\r\n            }\r\n\r\n            .btn-outline-light.disabled, .btn-outline-light:disabled {\r\n                color: #f8f9fa;\r\n                background-color: transparent\r\n            }\r\n\r\n            .btn-outline-light:not(:disabled):not(.disabled).active, .btn-outline-light:not(:disabled):not(.disabled):active, .show > .btn-outline-light.dropdown-toggle {\r\n                color: #212529;\r\n                background-color: #f8f9fa;\r\n                border-color: #f8f9fa\r\n            }\r\n\r\n                .btn-outline-light:not(:disabled):not(.disabled).active:focus, .btn-outline-light:not(:disabled):not(.disabled):active:focus, .show > .btn-outline-light.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(248,249,250,.5)\r\n                }\r\n\r\n        .btn-outline-dark {\r\n            color: #343a40;\r\n            background-color: transparent;\r\n            background-image: none;\r\n            border-color: #343a40\r\n        }\r\n\r\n            .btn-outline-dark:hover {\r\n                color: #fff;\r\n                background-color: #343a40;\r\n                border-color: #343a40\r\n            }\r\n\r\n            .btn-outline-dark.focus, .btn-outline-dark:focus {\r\n                box-shadow: 0 0 0 .2rem rgba(52,58,64,.5)\r\n            }\r\n\r\n            .btn-outline-dark.disabled, .btn-outline-dark:disabled {\r\n                color: #343a40;\r\n                background-color: transparent\r\n            }\r\n\r\n            .btn-outline-dark:not(:disabled):not(.disabled).active, .btn-outline-dark:not(:disabled):not(.disabled):active, .show > .btn-outline-dark.dropdown-toggle {\r\n                color: #fff;\r\n                background-color: #343a40;\r\n                border-color: #343a40\r\n            }\r\n\r\n                .btn-outline-dark:not(:disabled):not(.disabled).active:focus, .btn-outline-dark:not(:disabled):not(.disabled):active:focus, .show > .btn-outline-dark.dropdown-toggle:focus {\r\n                    box-shadow: 0 0 0 .2rem rgba(52,58,64,.5)\r\n                }\r\n\r\n        .btn-link {\r\n            font-weight: 400;\r\n            color: #007bff;\r\n            background-color: transparent\r\n        }\r\n\r\n            .btn-link:hover {\r\n                color: #0056b3;\r\n                text-decoration: underline;\r\n                background-color: transparent;\r\n                border-color: transparent\r\n            }\r\n\r\n            .btn-link.focus, .btn-link:focus {\r\n                text-decoration: underline;\r\n                border-color: transparent;\r\n                box-shadow: none\r\n            }\r\n\r\n            .btn-link.disabled, .btn-link:disabled {\r\n                color: #6c757d\r\n            }\r\n\r\n        .btn-group-lg > .btn, .btn-lg {\r\n            padding: .5rem 1rem;\r\n            font-size: 1.25rem;\r\n            line-height: 1.5;\r\n            border-radius: .3rem\r\n        }\r\n\r\n        .btn-group-sm > .btn, .btn-sm {\r\n            padding: .25rem .5rem;\r\n            font-size: .875rem;\r\n            line-height: 1.5;\r\n            border-radius: .2rem\r\n        }\r\n\r\n        .btn-block {\r\n            display: block;\r\n            width: 100%\r\n        }\r\n\r\n            .btn-block + .btn-block {\r\n                margin-top: .5rem\r\n            }\r\n\r\n        input[type=button].btn-block, input[type=reset].btn-block, input[type=submit].btn-block {\r\n            width: 100%\r\n        }\r\n\r\n        .fade {\r\n            opacity: 0;\r\n            transition: opacity .15s linear\r\n        }\r\n\r\n            .fade.show {\r\n                opacity: 1\r\n            }\r\n\r\n        .collapse {\r\n            display: none\r\n        }\r\n\r\n            .collapse.show {\r\n                display: block\r\n            }\r\n\r\n        tr.collapse.show {\r\n            display: table-row\r\n        }\r\n\r\n        tbody.collapse.show {\r\n            display: table-row-group\r\n        }\r\n\r\n        .collapsing {\r\n            position: relative;\r\n            height: 0;\r\n            overflow: hidden;\r\n            transition: height .35s ease\r\n        }\r\n\r\n        .dropdown, .dropup {\r\n            position: relative\r\n        }\r\n\r\n        .dropdown-toggle::after {\r\n            display: inline-block;\r\n            width: 0;\r\n            height: 0;\r\n            margin-left: .255em;\r\n            vertical-align: .255em;\r\n            content: \"\";\r\n            border-top: .3em solid;\r\n            border-right: .3em solid transparent;\r\n            border-bottom: 0;\r\n            border-left: .3em solid transparent\r\n        }\r\n\r\n        .dropdown-toggle:empty::after {\r\n            margin-left: 0\r\n        }\r\n\r\n        .dropdown-menu {\r\n            position: absolute;\r\n            top: 100%;\r\n            left: 0;\r\n            z-index: 1000;\r\n            display: none;\r\n            float: left;\r\n            min-width: 10rem;\r\n            padding: .5rem 0;\r\n            margin: .125rem 0 0;\r\n            font-size: 1rem;\r\n            color: #212529;\r\n            text-align: left;\r\n            list-style: none;\r\n            background-color: #fff;\r\n            background-clip: padding-box;\r\n            border: 1px solid rgba(0,0,0,.15);\r\n            border-radius: .25rem\r\n        }\r\n\r\n        .dropup .dropdown-menu {\r\n            margin-top: 0;\r\n            margin-bottom: .125rem\r\n        }\r\n\r\n        .dropup .dropdown-toggle::after {\r\n            display: inline-block;\r\n            width: 0;\r\n            height: 0;\r\n            margin-left: .255em;\r\n            vertical-align: .255em;\r\n            content: \"\";\r\n            border-top: 0;\r\n            border-right: .3em solid transparent;\r\n            border-bottom: .3em solid;\r\n            border-left: .3em solid transparent\r\n        }\r\n\r\n        .dropup .dropdown-toggle:empty::after {\r\n            margin-left: 0\r\n        }\r\n\r\n        .dropright .dropdown-menu {\r\n            margin-top: 0;\r\n            margin-left: .125rem\r\n        }\r\n\r\n        .dropright .dropdown-toggle::after {\r\n            display: inline-block;\r\n            width: 0;\r\n            height: 0;\r\n            margin-left: .255em;\r\n            vertical-align: .255em;\r\n            content: \"\";\r\n            border-top: .3em solid transparent;\r\n            border-bottom: .3em solid transparent;\r\n            border-left: .3em solid\r\n        }\r\n\r\n        .dropright .dropdown-toggle:empty::after {\r\n            margin-left: 0\r\n        }\r\n\r\n        .dropright .dropdown-toggle::after {\r\n            vertical-align: 0\r\n        }\r\n\r\n        .dropleft .dropdown-menu {\r\n            margin-top: 0;\r\n            margin-right: .125rem\r\n        }\r\n\r\n        .dropleft .dropdown-toggle::after {\r\n            display: inline-block;\r\n            width: 0;\r\n            height: 0;\r\n            margin-left: .255em;\r\n            vertical-align: .255em;\r\n            content: \"\"\r\n        }\r\n\r\n        .dropleft .dropdown-toggle::after {\r\n            display: none\r\n        }\r\n\r\n        .dropleft .dropdown-toggle::before {\r\n            display: inline-block;\r\n            width: 0;\r\n            height: 0;\r\n            margin-right: .255em;\r\n            vertical-align: .255em;\r\n            content: \"\";\r\n            border-top: .3em solid transparent;\r\n            border-right: .3em solid;\r\n            border-bottom: .3em solid transparent\r\n        }\r\n\r\n        .dropleft .dropdown-toggle:empty::after {\r\n            margin-left: 0\r\n        }\r\n\r\n        .dropleft .dropdown-toggle::before {\r\n            vertical-align: 0\r\n        }\r\n\r\n        .dropdown-divider {\r\n            height: 0;\r\n            margin: .5rem 0;\r\n            overflow: hidden;\r\n            border-top: 1px solid #e9ecef\r\n        }\r\n\r\n        .dropdown-item {\r\n            display: block;\r\n            width: 100%;\r\n            padding: .25rem 1.5rem;\r\n            clear: both;\r\n            font-weight: 400;\r\n            color: #212529;\r\n            text-align: inherit;\r\n            white-space: nowrap;\r\n            background-color: transparent;\r\n            border: 0\r\n        }\r\n\r\n            .dropdown-item:focus, .dropdown-item:hover {\r\n                color: #16181b;\r\n                text-decoration: none;\r\n                background-color: #f8f9fa\r\n            }\r\n\r\n            .dropdown-item.active, .dropdown-item:active {\r\n                color: #fff;\r\n                text-decoration: none;\r\n                background-color: #007bff\r\n            }\r\n\r\n            .dropdown-item.disabled, .dropdown-item:disabled {\r\n                color: #6c757d;\r\n                background-color: transparent\r\n            }\r\n\r\n        .dropdown-menu.show {\r\n            display: block\r\n        }\r\n\r\n        .dropdown-header {\r\n            display: block;\r\n            padding: .5rem 1.5rem;\r\n            margin-bottom: 0;\r\n            font-size: .875rem;\r\n            color: #6c757d;\r\n            white-space: nowrap\r\n        }\r\n\r\n        .btn-group, .btn-group-vertical {\r\n            position: relative;\r\n            display: -webkit-inline-box;\r\n            display: -ms-inline-flexbox;\r\n            display: inline-flex;\r\n            vertical-align: middle\r\n        }\r\n\r\n            .btn-group-vertical > .btn, .btn-group > .btn {\r\n                position: relative;\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 1 auto;\r\n                flex: 0 1 auto\r\n            }\r\n\r\n                .btn-group-vertical > .btn:hover, .btn-group > .btn:hover {\r\n                    z-index: 1\r\n                }\r\n\r\n                .btn-group-vertical > .btn.active, .btn-group-vertical > .btn:active, .btn-group-vertical > .btn:focus, .btn-group > .btn.active, .btn-group > .btn:active, .btn-group > .btn:focus {\r\n                    z-index: 1\r\n                }\r\n\r\n            .btn-group .btn + .btn, .btn-group .btn + .btn-group, .btn-group .btn-group + .btn, .btn-group .btn-group + .btn-group, .btn-group-vertical .btn + .btn, .btn-group-vertical .btn + .btn-group, .btn-group-vertical .btn-group + .btn, .btn-group-vertical .btn-group + .btn-group {\r\n                margin-left: -1px\r\n            }\r\n\r\n        .btn-toolbar {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -ms-flex-wrap: wrap;\r\n            flex-wrap: wrap;\r\n            -webkit-box-pack: start;\r\n            -ms-flex-pack: start;\r\n            justify-content: flex-start\r\n        }\r\n\r\n            .btn-toolbar .input-group {\r\n                width: auto\r\n            }\r\n\r\n        .btn-group > .btn:first-child {\r\n            margin-left: 0\r\n        }\r\n\r\n        .btn-group > .btn-group:not(:last-child) > .btn, .btn-group > .btn:not(:last-child):not(.dropdown-toggle) {\r\n            border-top-right-radius: 0;\r\n            border-bottom-right-radius: 0\r\n        }\r\n\r\n        .btn-group > .btn-group:not(:first-child) > .btn, .btn-group > .btn:not(:first-child) {\r\n            border-top-left-radius: 0;\r\n            border-bottom-left-radius: 0\r\n        }\r\n\r\n        .dropdown-toggle-split {\r\n            padding-right: .5625rem;\r\n            padding-left: .5625rem\r\n        }\r\n\r\n            .dropdown-toggle-split::after {\r\n                margin-left: 0\r\n            }\r\n\r\n        .btn-group-sm > .btn + .dropdown-toggle-split, .btn-sm + .dropdown-toggle-split {\r\n            padding-right: .375rem;\r\n            padding-left: .375rem\r\n        }\r\n\r\n        .btn-group-lg > .btn + .dropdown-toggle-split, .btn-lg + .dropdown-toggle-split {\r\n            padding-right: .75rem;\r\n            padding-left: .75rem\r\n        }\r\n\r\n        .btn-group-vertical {\r\n            -webkit-box-orient: vertical;\r\n            -webkit-box-direction: normal;\r\n            -ms-flex-direction: column;\r\n            flex-direction: column;\r\n            -webkit-box-align: start;\r\n            -ms-flex-align: start;\r\n            align-items: flex-start;\r\n            -webkit-box-pack: center;\r\n            -ms-flex-pack: center;\r\n            justify-content: center\r\n        }\r\n\r\n            .btn-group-vertical .btn, .btn-group-vertical .btn-group {\r\n                width: 100%\r\n            }\r\n\r\n            .btn-group-vertical > .btn + .btn, .btn-group-vertical > .btn + .btn-group, .btn-group-vertical > .btn-group + .btn, .btn-group-vertical > .btn-group + .btn-group {\r\n                margin-top: -1px;\r\n                margin-left: 0\r\n            }\r\n\r\n            .btn-group-vertical > .btn-group:not(:last-child) > .btn, .btn-group-vertical > .btn:not(:last-child):not(.dropdown-toggle) {\r\n                border-bottom-right-radius: 0;\r\n                border-bottom-left-radius: 0\r\n            }\r\n\r\n            .btn-group-vertical > .btn-group:not(:first-child) > .btn, .btn-group-vertical > .btn:not(:first-child) {\r\n                border-top-left-radius: 0;\r\n                border-top-right-radius: 0\r\n            }\r\n\r\n        .btn-group-toggle > .btn, .btn-group-toggle > .btn-group > .btn {\r\n            margin-bottom: 0\r\n        }\r\n\r\n            .btn-group-toggle > .btn input[type=checkbox], .btn-group-toggle > .btn input[type=radio], .btn-group-toggle > .btn-group > .btn input[type=checkbox], .btn-group-toggle > .btn-group > .btn input[type=radio] {\r\n                position: absolute;\r\n                clip: rect(0,0,0,0);\r\n                pointer-events: none\r\n            }\r\n\r\n        .input-group {\r\n            position: relative;\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -ms-flex-wrap: wrap;\r\n            flex-wrap: wrap;\r\n            -webkit-box-align: stretch;\r\n            -ms-flex-align: stretch;\r\n            align-items: stretch;\r\n            width: 100%\r\n        }\r\n\r\n            .input-group > .custom-file, .input-group > .custom-select, .input-group > .form-control {\r\n                position: relative;\r\n                -webkit-box-flex: 1;\r\n                -ms-flex: 1 1 auto;\r\n                flex: 1 1 auto;\r\n                width: 1%;\r\n                margin-bottom: 0\r\n            }\r\n\r\n                .input-group > .custom-file:focus, .input-group > .custom-select:focus, .input-group > .form-control:focus {\r\n                    z-index: 3\r\n                }\r\n\r\n                .input-group > .custom-file + .custom-file, .input-group > .custom-file + .custom-select, .input-group > .custom-file + .form-control, .input-group > .custom-select + .custom-file, .input-group > .custom-select + .custom-select, .input-group > .custom-select + .form-control, .input-group > .form-control + .custom-file, .input-group > .form-control + .custom-select, .input-group > .form-control + .form-control {\r\n                    margin-left: -1px\r\n                }\r\n\r\n                .input-group > .custom-select:not(:last-child), .input-group > .form-control:not(:last-child) {\r\n                    border-top-right-radius: 0;\r\n                    border-bottom-right-radius: 0\r\n                }\r\n\r\n                .input-group > .custom-select:not(:first-child), .input-group > .form-control:not(:first-child) {\r\n                    border-top-left-radius: 0;\r\n                    border-bottom-left-radius: 0\r\n                }\r\n\r\n            .input-group > .custom-file {\r\n                display: -webkit-box;\r\n                display: -ms-flexbox;\r\n                display: flex;\r\n                -webkit-box-align: center;\r\n                -ms-flex-align: center;\r\n                align-items: center\r\n            }\r\n\r\n                .input-group > .custom-file:not(:last-child) .custom-file-label, .input-group > .custom-file:not(:last-child) .custom-file-label::before {\r\n                    border-top-right-radius: 0;\r\n                    border-bottom-right-radius: 0\r\n                }\r\n\r\n                .input-group > .custom-file:not(:first-child) .custom-file-label, .input-group > .custom-file:not(:first-child) .custom-file-label::before {\r\n                    border-top-left-radius: 0;\r\n                    border-bottom-left-radius: 0\r\n                }\r\n\r\n        .input-group-append, .input-group-prepend {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex\r\n        }\r\n\r\n            .input-group-append .btn, .input-group-prepend .btn {\r\n                position: relative;\r\n                z-index: 2\r\n            }\r\n\r\n                .input-group-append .btn + .btn, .input-group-append .btn + .input-group-text, .input-group-append .input-group-text + .btn, .input-group-append .input-group-text + .input-group-text, .input-group-prepend .btn + .btn, .input-group-prepend .btn + .input-group-text, .input-group-prepend .input-group-text + .btn, .input-group-prepend .input-group-text + .input-group-text {\r\n                    margin-left: -1px\r\n                }\r\n\r\n        .input-group-prepend {\r\n            margin-right: -1px\r\n        }\r\n\r\n        .input-group-append {\r\n            margin-left: -1px\r\n        }\r\n\r\n        .input-group-text {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-align: center;\r\n            -ms-flex-align: center;\r\n            align-items: center;\r\n            padding: .375rem .75rem;\r\n            margin-bottom: 0;\r\n            font-size: 1rem;\r\n            font-weight: 400;\r\n            line-height: 1.5;\r\n            color: #495057;\r\n            text-align: center;\r\n            white-space: nowrap;\r\n            background-color: #e9ecef;\r\n            border: 1px solid #ced4da;\r\n            border-radius: .25rem\r\n        }\r\n\r\n            .input-group-text input[type=checkbox], .input-group-text input[type=radio] {\r\n                margin-top: 0\r\n            }\r\n\r\n        .input-group > .input-group-append:last-child > .btn:not(:last-child):not(.dropdown-toggle), .input-group > .input-group-append:last-child > .input-group-text:not(:last-child), .input-group > .input-group-append:not(:last-child) > .btn, .input-group > .input-group-append:not(:last-child) > .input-group-text, .input-group > .input-group-prepend > .btn, .input-group > .input-group-prepend > .input-group-text {\r\n            border-top-right-radius: 0;\r\n            border-bottom-right-radius: 0\r\n        }\r\n\r\n        .input-group > .input-group-append > .btn, .input-group > .input-group-append > .input-group-text, .input-group > .input-group-prepend:first-child > .btn:not(:first-child), .input-group > .input-group-prepend:first-child > .input-group-text:not(:first-child), .input-group > .input-group-prepend:not(:first-child) > .btn, .input-group > .input-group-prepend:not(:first-child) > .input-group-text {\r\n            border-top-left-radius: 0;\r\n            border-bottom-left-radius: 0\r\n        }\r\n\r\n        .custom-control {\r\n            position: relative;\r\n            display: block;\r\n            min-height: 1.5rem;\r\n            padding-left: 1.5rem\r\n        }\r\n\r\n        .custom-control-inline {\r\n            display: -webkit-inline-box;\r\n            display: -ms-inline-flexbox;\r\n            display: inline-flex;\r\n            margin-right: 1rem\r\n        }\r\n\r\n        .custom-control-input {\r\n            position: absolute;\r\n            z-index: -1;\r\n            opacity: 0\r\n        }\r\n\r\n            .custom-control-input:checked ~ .custom-control-label::before {\r\n                color: #fff;\r\n                background-color: #007bff\r\n            }\r\n\r\n            .custom-control-input:focus ~ .custom-control-label::before {\r\n                box-shadow: 0 0 0 1px #fff,0 0 0 .2rem rgba(0,123,255,.25)\r\n            }\r\n\r\n            .custom-control-input:active ~ .custom-control-label::before {\r\n                color: #fff;\r\n                background-color: #b3d7ff\r\n            }\r\n\r\n            .custom-control-input:disabled ~ .custom-control-label {\r\n                color: #6c757d\r\n            }\r\n\r\n                .custom-control-input:disabled ~ .custom-control-label::before {\r\n                    background-color: #e9ecef\r\n                }\r\n\r\n        .custom-control-label {\r\n            margin-bottom: 0\r\n        }\r\n\r\n            .custom-control-label::before {\r\n                position: absolute;\r\n                top: .25rem;\r\n                left: 0;\r\n                display: block;\r\n                width: 1rem;\r\n                height: 1rem;\r\n                pointer-events: none;\r\n                content: \"\";\r\n                -webkit-user-select: none;\r\n                -moz-user-select: none;\r\n                -ms-user-select: none;\r\n                user-select: none;\r\n                background-color: #dee2e6\r\n            }\r\n\r\n            .custom-control-label::after {\r\n                position: absolute;\r\n                top: .25rem;\r\n                left: 0;\r\n                display: block;\r\n                width: 1rem;\r\n                height: 1rem;\r\n                content: \"\";\r\n                background-repeat: no-repeat;\r\n                background-position: center center;\r\n                background-size: 50% 50%\r\n            }\r\n\r\n        .custom-checkbox .custom-control-label::before {\r\n            border-radius: .25rem\r\n        }\r\n\r\n        .custom-checkbox .custom-control-input:checked ~ .custom-control-label::before {\r\n            background-color: #007bff\r\n        }\r\n\r\n        .custom-checkbox .custom-control-input:checked ~ .custom-control-label::after {\r\n            background-image: url(\"data:image/svg+xml;charset=utf8,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 8 8'%3E%3Cpath fill='%23fff' d='M6.564.75l-3.59 3.612-1.538-1.55L0 4.26 2.974 7.25 8 2.193z'/%3E%3C/svg%3E\")\r\n        }\r\n\r\n        .custom-checkbox .custom-control-input:indeterminate ~ .custom-control-label::before {\r\n            background-color: #007bff\r\n        }\r\n\r\n        .custom-checkbox .custom-control-input:indeterminate ~ .custom-control-label::after {\r\n            background-image: url(\"data:image/svg+xml;charset=utf8,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 4 4'%3E%3Cpath stroke='%23fff' d='M0 2h4'/%3E%3C/svg%3E\")\r\n        }\r\n\r\n        .custom-checkbox .custom-control-input:disabled:checked ~ .custom-control-label::before {\r\n            background-color: rgba(0,123,255,.5)\r\n        }\r\n\r\n        .custom-checkbox .custom-control-input:disabled:indeterminate ~ .custom-control-label::before {\r\n            background-color: rgba(0,123,255,.5)\r\n        }\r\n\r\n        .custom-radio .custom-control-label::before {\r\n            border-radius: 50%\r\n        }\r\n\r\n        .custom-radio .custom-control-input:checked ~ .custom-control-label::before {\r\n            background-color: #007bff\r\n        }\r\n\r\n        .custom-radio .custom-control-input:checked ~ .custom-control-label::after {\r\n            background-image: url(\"data:image/svg+xml;charset=utf8,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='-4 -4 8 8'%3E%3Ccircle r='3' fill='%23fff'/%3E%3C/svg%3E\")\r\n        }\r\n\r\n        .custom-radio .custom-control-input:disabled:checked ~ .custom-control-label::before {\r\n            background-color: rgba(0,123,255,.5)\r\n        }\r\n\r\n        .custom-select {\r\n            display: inline-block;\r\n            width: 100%;\r\n            height: calc(2.25rem + 2px);\r\n            padding: .375rem 1.75rem .375rem .75rem;\r\n            line-height: 1.5;\r\n            color: #495057;\r\n            vertical-align: middle;\r\n            background: #fff url(\"data:image/svg+xml;charset=utf8,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 4 5'%3E%3Cpath fill='%23343a40' d='M2 0L0 2h4zm0 5L0 3h4z'/%3E%3C/svg%3E\") no-repeat right .75rem center;\r\n            background-size: 8px 10px;\r\n            border: 1px solid #ced4da;\r\n            border-radius: .25rem;\r\n            -webkit-appearance: none;\r\n            -moz-appearance: none;\r\n            appearance: none\r\n        }\r\n\r\n            .custom-select:focus {\r\n                border-color: #80bdff;\r\n                outline: 0;\r\n                box-shadow: inset 0 1px 2px rgba(0,0,0,.075),0 0 5px rgba(128,189,255,.5)\r\n            }\r\n\r\n                .custom-select:focus::-ms-value {\r\n                    color: #495057;\r\n                    background-color: #fff\r\n                }\r\n\r\n            .custom-select[multiple], .custom-select[size]:not([size=\"1\"]) {\r\n                height: auto;\r\n                padding-right: .75rem;\r\n                background-image: none\r\n            }\r\n\r\n            .custom-select:disabled {\r\n                color: #6c757d;\r\n                background-color: #e9ecef\r\n            }\r\n\r\n            .custom-select::-ms-expand {\r\n                opacity: 0\r\n            }\r\n\r\n        .custom-select-sm {\r\n            height: calc(1.8125rem + 2px);\r\n            padding-top: .375rem;\r\n            padding-bottom: .375rem;\r\n            font-size: 75%\r\n        }\r\n\r\n        .custom-select-lg {\r\n            height: calc(2.875rem + 2px);\r\n            padding-top: .375rem;\r\n            padding-bottom: .375rem;\r\n            font-size: 125%\r\n        }\r\n\r\n        .custom-file {\r\n            position: relative;\r\n            display: inline-block;\r\n            width: 100%;\r\n            height: calc(2.25rem + 2px);\r\n            margin-bottom: 0\r\n        }\r\n\r\n        .custom-file-input {\r\n            position: relative;\r\n            z-index: 2;\r\n            width: 100%;\r\n            height: calc(2.25rem + 2px);\r\n            margin: 0;\r\n            opacity: 0\r\n        }\r\n\r\n            .custom-file-input:focus ~ .custom-file-control {\r\n                border-color: #80bdff;\r\n                box-shadow: 0 0 0 .2rem rgba(0,123,255,.25)\r\n            }\r\n\r\n                .custom-file-input:focus ~ .custom-file-control::before {\r\n                    border-color: #80bdff\r\n                }\r\n\r\n            .custom-file-input:lang(en) ~ .custom-file-label::after {\r\n                content: \"Browse\"\r\n            }\r\n\r\n        .custom-file-label {\r\n            position: absolute;\r\n            top: 0;\r\n            right: 0;\r\n            left: 0;\r\n            z-index: 1;\r\n            height: calc(2.25rem + 2px);\r\n            padding: .375rem .75rem;\r\n            line-height: 1.5;\r\n            color: #495057;\r\n            background-color: #fff;\r\n            border: 1px solid #ced4da;\r\n            border-radius: .25rem\r\n        }\r\n\r\n            .custom-file-label::after {\r\n                position: absolute;\r\n                top: 0;\r\n                right: 0;\r\n                bottom: 0;\r\n                z-index: 3;\r\n                display: block;\r\n                height: calc(calc(2.25rem + 2px) - 1px * 2);\r\n                padding: .375rem .75rem;\r\n                line-height: 1.5;\r\n                color: #495057;\r\n                content: \"Browse\";\r\n                background-color: #e9ecef;\r\n                border-left: 1px solid #ced4da;\r\n                border-radius: 0 .25rem .25rem 0\r\n            }\r\n\r\n        .nav {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -ms-flex-wrap: wrap;\r\n            flex-wrap: wrap;\r\n            padding-left: 0;\r\n            margin-bottom: 0;\r\n            list-style: none\r\n        }\r\n\r\n        .nav-link {\r\n            display: block;\r\n            padding: .5rem 1rem\r\n        }\r\n\r\n            .nav-link:focus, .nav-link:hover {\r\n                text-decoration: none\r\n            }\r\n\r\n            .nav-link.disabled {\r\n                color: #6c757d\r\n            }\r\n\r\n        .nav-tabs {\r\n            border-bottom: 1px solid #dee2e6\r\n        }\r\n\r\n            .nav-tabs .nav-item {\r\n                margin-bottom: -1px\r\n            }\r\n\r\n            .nav-tabs .nav-link {\r\n                border: 1px solid transparent;\r\n                border-top-left-radius: .25rem;\r\n                border-top-right-radius: .25rem\r\n            }\r\n\r\n                .nav-tabs .nav-link:focus, .nav-tabs .nav-link:hover {\r\n                    border-color: #e9ecef #e9ecef #dee2e6\r\n                }\r\n\r\n                .nav-tabs .nav-link.disabled {\r\n                    color: #6c757d;\r\n                    background-color: transparent;\r\n                    border-color: transparent\r\n                }\r\n\r\n                .nav-tabs .nav-item.show .nav-link, .nav-tabs .nav-link.active {\r\n                    color: #495057;\r\n                    background-color: #fff;\r\n                    border-color: #dee2e6 #dee2e6 #fff\r\n                }\r\n\r\n            .nav-tabs .dropdown-menu {\r\n                margin-top: -1px;\r\n                border-top-left-radius: 0;\r\n                border-top-right-radius: 0\r\n            }\r\n\r\n        .nav-pills .nav-link {\r\n            border-radius: .25rem\r\n        }\r\n\r\n            .nav-pills .nav-link.active, .nav-pills .show > .nav-link {\r\n                color: #fff;\r\n                background-color: #007bff\r\n            }\r\n\r\n        .nav-fill .nav-item {\r\n            -webkit-box-flex: 1;\r\n            -ms-flex: 1 1 auto;\r\n            flex: 1 1 auto;\r\n            text-align: center\r\n        }\r\n\r\n        .nav-justified .nav-item {\r\n            -ms-flex-preferred-size: 0;\r\n            flex-basis: 0;\r\n            -webkit-box-flex: 1;\r\n            -ms-flex-positive: 1;\r\n            flex-grow: 1;\r\n            text-align: center\r\n        }\r\n\r\n        .tab-content > .tab-pane {\r\n            display: none\r\n        }\r\n\r\n        .tab-content > .active {\r\n            display: block\r\n        }\r\n\r\n        .navbar {\r\n            position: relative;\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -ms-flex-wrap: wrap;\r\n            flex-wrap: wrap;\r\n            -webkit-box-align: center;\r\n            -ms-flex-align: center;\r\n            align-items: center;\r\n            -webkit-box-pack: justify;\r\n            -ms-flex-pack: justify;\r\n            justify-content: space-between;\r\n            padding: .5rem 1rem\r\n        }\r\n\r\n            .navbar > .container, .navbar > .container-fluid {\r\n                display: -webkit-box;\r\n                display: -ms-flexbox;\r\n                display: flex;\r\n                -ms-flex-wrap: wrap;\r\n                flex-wrap: wrap;\r\n                -webkit-box-align: center;\r\n                -ms-flex-align: center;\r\n                align-items: center;\r\n                -webkit-box-pack: justify;\r\n                -ms-flex-pack: justify;\r\n                justify-content: space-between\r\n            }\r\n\r\n        .navbar-brand {\r\n            display: inline-block;\r\n            padding-top: .3125rem;\r\n            padding-bottom: .3125rem;\r\n            margin-right: 1rem;\r\n            font-size: 1.25rem;\r\n            line-height: inherit;\r\n            white-space: nowrap\r\n        }\r\n\r\n            .navbar-brand:focus, .navbar-brand:hover {\r\n                text-decoration: none\r\n            }\r\n\r\n        .navbar-nav {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-orient: vertical;\r\n            -webkit-box-direction: normal;\r\n            -ms-flex-direction: column;\r\n            flex-direction: column;\r\n            padding-left: 0;\r\n            margin-bottom: 0;\r\n            list-style: none\r\n        }\r\n\r\n            .navbar-nav .nav-link {\r\n                padding-right: 0;\r\n                padding-left: 0\r\n            }\r\n\r\n            .navbar-nav .dropdown-menu {\r\n                position: static;\r\n                float: none\r\n            }\r\n\r\n        .navbar-text {\r\n            display: inline-block;\r\n            padding-top: .5rem;\r\n            padding-bottom: .5rem\r\n        }\r\n\r\n        .navbar-collapse {\r\n            -ms-flex-preferred-size: 100%;\r\n            flex-basis: 100%;\r\n            -webkit-box-flex: 1;\r\n            -ms-flex-positive: 1;\r\n            flex-grow: 1;\r\n            -webkit-box-align: center;\r\n            -ms-flex-align: center;\r\n            align-items: center\r\n        }\r\n\r\n        .navbar-toggler {\r\n            padding: .25rem .75rem;\r\n            font-size: 1.25rem;\r\n            line-height: 1;\r\n            background-color: transparent;\r\n            border: 1px solid transparent;\r\n            border-radius: .25rem\r\n        }\r\n\r\n            .navbar-toggler:focus, .navbar-toggler:hover {\r\n                text-decoration: none\r\n            }\r\n\r\n            .navbar-toggler:not(:disabled):not(.disabled) {\r\n                cursor: pointer\r\n            }\r\n\r\n        .navbar-toggler-icon {\r\n            display: inline-block;\r\n            width: 1.5em;\r\n            height: 1.5em;\r\n            vertical-align: middle;\r\n            content: \"\";\r\n            background: no-repeat center center;\r\n            background-size: 100% 100%\r\n        }\r\n\r\n        @media (max-width:575.98px) {\r\n            .navbar-expand-sm > .container, .navbar-expand-sm > .container-fluid {\r\n                padding-right: 0;\r\n                padding-left: 0\r\n            }\r\n        }\r\n\r\n        @media (min-width:576px) {\r\n            .navbar-expand-sm {\r\n                -webkit-box-orient: horizontal;\r\n                -webkit-box-direction: normal;\r\n                -ms-flex-flow: row nowrap;\r\n                flex-flow: row nowrap;\r\n                -webkit-box-pack: start;\r\n                -ms-flex-pack: start;\r\n                justify-content: flex-start\r\n            }\r\n\r\n                .navbar-expand-sm .navbar-nav {\r\n                    -webkit-box-orient: horizontal;\r\n                    -webkit-box-direction: normal;\r\n                    -ms-flex-direction: row;\r\n                    flex-direction: row\r\n                }\r\n\r\n                    .navbar-expand-sm .navbar-nav .dropdown-menu {\r\n                        position: absolute\r\n                    }\r\n\r\n                    .navbar-expand-sm .navbar-nav .dropdown-menu-right {\r\n                        right: 0;\r\n                        left: auto\r\n                    }\r\n\r\n                    .navbar-expand-sm .navbar-nav .nav-link {\r\n                        padding-right: .5rem;\r\n                        padding-left: .5rem\r\n                    }\r\n\r\n                .navbar-expand-sm > .container, .navbar-expand-sm > .container-fluid {\r\n                    -ms-flex-wrap: nowrap;\r\n                    flex-wrap: nowrap\r\n                }\r\n\r\n                .navbar-expand-sm .navbar-collapse {\r\n                    display: -webkit-box !important;\r\n                    display: -ms-flexbox !important;\r\n                    display: flex !important;\r\n                    -ms-flex-preferred-size: auto;\r\n                    flex-basis: auto\r\n                }\r\n\r\n                .navbar-expand-sm .navbar-toggler {\r\n                    display: none\r\n                }\r\n\r\n                .navbar-expand-sm .dropup .dropdown-menu {\r\n                    top: auto;\r\n                    bottom: 100%\r\n                }\r\n        }\r\n\r\n        @media (max-width:767.98px) {\r\n            .navbar-expand-md > .container, .navbar-expand-md > .container-fluid {\r\n                padding-right: 0;\r\n                padding-left: 0\r\n            }\r\n        }\r\n\r\n        @media (min-width:768px) {\r\n            .navbar-expand-md {\r\n                -webkit-box-orient: horizontal;\r\n                -webkit-box-direction: normal;\r\n                -ms-flex-flow: row nowrap;\r\n                flex-flow: row nowrap;\r\n                -webkit-box-pack: start;\r\n                -ms-flex-pack: start;\r\n                justify-content: flex-start\r\n            }\r\n\r\n                .navbar-expand-md .navbar-nav {\r\n                    -webkit-box-orient: horizontal;\r\n                    -webkit-box-direction: normal;\r\n                    -ms-flex-direction: row;\r\n                    flex-direction: row\r\n                }\r\n\r\n                    .navbar-expand-md .navbar-nav .dropdown-menu {\r\n                        position: absolute\r\n                    }\r\n\r\n                    .navbar-expand-md .navbar-nav .dropdown-menu-right {\r\n                        right: 0;\r\n                        left: auto\r\n                    }\r\n\r\n                    .navbar-expand-md .navbar-nav .nav-link {\r\n                        padding-right: .5rem;\r\n                        padding-left: .5rem\r\n                    }\r\n\r\n                .navbar-expand-md > .container, .navbar-expand-md > .container-fluid {\r\n                    -ms-flex-wrap: nowrap;\r\n                    flex-wrap: nowrap\r\n                }\r\n\r\n                .navbar-expand-md .navbar-collapse {\r\n                    display: -webkit-box !important;\r\n                    display: -ms-flexbox !important;\r\n                    display: flex !important;\r\n                    -ms-flex-preferred-size: auto;\r\n                    flex-basis: auto\r\n                }\r\n\r\n                .navbar-expand-md .navbar-toggler {\r\n                    display: none\r\n                }\r\n\r\n                .navbar-expand-md .dropup .dropdown-menu {\r\n                    top: auto;\r\n                    bottom: 100%\r\n                }\r\n        }\r\n\r\n        @media (max-width:991.98px) {\r\n            .navbar-expand-lg > .container, .navbar-expand-lg > .container-fluid {\r\n                padding-right: 0;\r\n                padding-left: 0\r\n            }\r\n        }\r\n\r\n        @media (min-width:992px) {\r\n            .navbar-expand-lg {\r\n                -webkit-box-orient: horizontal;\r\n                -webkit-box-direction: normal;\r\n                -ms-flex-flow: row nowrap;\r\n                flex-flow: row nowrap;\r\n                -webkit-box-pack: start;\r\n                -ms-flex-pack: start;\r\n                justify-content: flex-start\r\n            }\r\n\r\n                .navbar-expand-lg .navbar-nav {\r\n                    -webkit-box-orient: horizontal;\r\n                    -webkit-box-direction: normal;\r\n                    -ms-flex-direction: row;\r\n                    flex-direction: row\r\n                }\r\n\r\n                    .navbar-expand-lg .navbar-nav .dropdown-menu {\r\n                        position: absolute\r\n                    }\r\n\r\n                    .navbar-expand-lg .navbar-nav .dropdown-menu-right {\r\n                        right: 0;\r\n                        left: auto\r\n                    }\r\n\r\n                    .navbar-expand-lg .navbar-nav .nav-link {\r\n                        padding-right: .5rem;\r\n                        padding-left: .5rem\r\n                    }\r\n\r\n                .navbar-expand-lg > .container, .navbar-expand-lg > .container-fluid {\r\n                    -ms-flex-wrap: nowrap;\r\n                    flex-wrap: nowrap\r\n                }\r\n\r\n                .navbar-expand-lg .navbar-collapse {\r\n                    display: -webkit-box !important;\r\n                    display: -ms-flexbox !important;\r\n                    display: flex !important;\r\n                    -ms-flex-preferred-size: auto;\r\n                    flex-basis: auto\r\n                }\r\n\r\n                .navbar-expand-lg .navbar-toggler {\r\n                    display: none\r\n                }\r\n\r\n                .navbar-expand-lg .dropup .dropdown-menu {\r\n                    top: auto;\r\n                    bottom: 100%\r\n                }\r\n        }\r\n\r\n        @media (max-width:1199.98px) {\r\n            .navbar-expand-xl > .container, .navbar-expand-xl > .container-fluid {\r\n                padding-right: 0;\r\n                padding-left: 0\r\n            }\r\n        }\r\n\r\n        @media (min-width:1200px) {\r\n            .navbar-expand-xl {\r\n                -webkit-box-orient: horizontal;\r\n                -webkit-box-direction: normal;\r\n                -ms-flex-flow: row nowrap;\r\n                flex-flow: row nowrap;\r\n                -webkit-box-pack: start;\r\n                -ms-flex-pack: start;\r\n                justify-content: flex-start\r\n            }\r\n\r\n                .navbar-expand-xl .navbar-nav {\r\n                    -webkit-box-orient: horizontal;\r\n                    -webkit-box-direction: normal;\r\n                    -ms-flex-direction: row;\r\n                    flex-direction: row\r\n                }\r\n\r\n                    .navbar-expand-xl .navbar-nav .dropdown-menu {\r\n                        position: absolute\r\n                    }\r\n\r\n                    .navbar-expand-xl .navbar-nav .dropdown-menu-right {\r\n                        right: 0;\r\n                        left: auto\r\n                    }\r\n\r\n                    .navbar-expand-xl .navbar-nav .nav-link {\r\n                        padding-right: .5rem;\r\n                        padding-left: .5rem\r\n                    }\r\n\r\n                .navbar-expand-xl > .container, .navbar-expand-xl > .container-fluid {\r\n                    -ms-flex-wrap: nowrap;\r\n                    flex-wrap: nowrap\r\n                }\r\n\r\n                .navbar-expand-xl .navbar-collapse {\r\n                    display: -webkit-box !important;\r\n                    display: -ms-flexbox !important;\r\n                    display: flex !important;\r\n                    -ms-flex-preferred-size: auto;\r\n                    flex-basis: auto\r\n                }\r\n\r\n                .navbar-expand-xl .navbar-toggler {\r\n                    display: none\r\n                }\r\n\r\n                .navbar-expand-xl .dropup .dropdown-menu {\r\n                    top: auto;\r\n                    bottom: 100%\r\n                }\r\n        }\r\n\r\n        .navbar-expand {\r\n            -webkit-box-orient: horizontal;\r\n            -webkit-box-direction: normal;\r\n            -ms-flex-flow: row nowrap;\r\n            flex-flow: row nowrap;\r\n            -webkit-box-pack: start;\r\n            -ms-flex-pack: start;\r\n            justify-content: flex-start\r\n        }\r\n\r\n            .navbar-expand > .container, .navbar-expand > .container-fluid {\r\n                padding-right: 0;\r\n                padding-left: 0\r\n            }\r\n\r\n            .navbar-expand .navbar-nav {\r\n                -webkit-box-orient: horizontal;\r\n                -webkit-box-direction: normal;\r\n                -ms-flex-direction: row;\r\n                flex-direction: row\r\n            }\r\n\r\n                .navbar-expand .navbar-nav .dropdown-menu {\r\n                    position: absolute\r\n                }\r\n\r\n                .navbar-expand .navbar-nav .dropdown-menu-right {\r\n                    right: 0;\r\n                    left: auto\r\n                }\r\n\r\n                .navbar-expand .navbar-nav .nav-link {\r\n                    padding-right: .5rem;\r\n                    padding-left: .5rem\r\n                }\r\n\r\n            .navbar-expand > .container, .navbar-expand > .container-fluid {\r\n                -ms-flex-wrap: nowrap;\r\n                flex-wrap: nowrap\r\n            }\r\n\r\n            .navbar-expand .navbar-collapse {\r\n                display: -webkit-box !important;\r\n                display: -ms-flexbox !important;\r\n                display: flex !important;\r\n                -ms-flex-preferred-size: auto;\r\n                flex-basis: auto\r\n            }\r\n\r\n            .navbar-expand .navbar-toggler {\r\n                display: none\r\n            }\r\n\r\n            .navbar-expand .dropup .dropdown-menu {\r\n                top: auto;\r\n                bottom: 100%\r\n            }\r\n\r\n        .navbar-light .navbar-brand {\r\n            color: rgba(0,0,0,.9)\r\n        }\r\n\r\n            .navbar-light .navbar-brand:focus, .navbar-light .navbar-brand:hover {\r\n                color: rgba(0,0,0,.9)\r\n            }\r\n\r\n        .navbar-light .navbar-nav .nav-link {\r\n            color: rgba(0,0,0,.5)\r\n        }\r\n\r\n            .navbar-light .navbar-nav .nav-link:focus, .navbar-light .navbar-nav .nav-link:hover {\r\n                color: rgba(0,0,0,.7)\r\n            }\r\n\r\n            .navbar-light .navbar-nav .nav-link.disabled {\r\n                color: rgba(0,0,0,.3)\r\n            }\r\n\r\n            .navbar-light .navbar-nav .active > .nav-link, .navbar-light .navbar-nav .nav-link.active, .navbar-light .navbar-nav .nav-link.show, .navbar-light .navbar-nav .show > .nav-link {\r\n                color: rgba(0,0,0,.9)\r\n            }\r\n\r\n        .navbar-light .navbar-toggler {\r\n            color: rgba(0,0,0,.5);\r\n            border-color: rgba(0,0,0,.1)\r\n        }\r\n\r\n        .navbar-light .navbar-toggler-icon {\r\n            background-image: url(\"data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E\")\r\n        }\r\n\r\n        .navbar-light .navbar-text {\r\n            color: rgba(0,0,0,.5)\r\n        }\r\n\r\n            .navbar-light .navbar-text a {\r\n                color: rgba(0,0,0,.9)\r\n            }\r\n\r\n                .navbar-light .navbar-text a:focus, .navbar-light .navbar-text a:hover {\r\n                    color: rgba(0,0,0,.9)\r\n                }\r\n\r\n        .navbar-dark .navbar-brand {\r\n            color: #fff\r\n        }\r\n\r\n            .navbar-dark .navbar-brand:focus, .navbar-dark .navbar-brand:hover {\r\n                color: #fff\r\n            }\r\n\r\n        .navbar-dark .navbar-nav .nav-link {\r\n            color: rgba(255,255,255,.5)\r\n        }\r\n\r\n            .navbar-dark .navbar-nav .nav-link:focus, .navbar-dark .navbar-nav .nav-link:hover {\r\n                color: rgba(255,255,255,.75)\r\n            }\r\n\r\n            .navbar-dark .navbar-nav .nav-link.disabled {\r\n                color: rgba(255,255,255,.25)\r\n            }\r\n\r\n            .navbar-dark .navbar-nav .active > .nav-link, .navbar-dark .navbar-nav .nav-link.active, .navbar-dark .navbar-nav .nav-link.show, .navbar-dark .navbar-nav .show > .nav-link {\r\n                color: #fff\r\n            }\r\n\r\n        .navbar-dark .navbar-toggler {\r\n            color: rgba(255,255,255,.5);\r\n            border-color: rgba(255,255,255,.1)\r\n        }\r\n\r\n        .navbar-dark .navbar-toggler-icon {\r\n            background-image: url(\"data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(255, 255, 255, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E\")\r\n        }\r\n\r\n        .navbar-dark .navbar-text {\r\n            color: rgba(255,255,255,.5)\r\n        }\r\n\r\n            .navbar-dark .navbar-text a {\r\n                color: #fff\r\n            }\r\n\r\n                .navbar-dark .navbar-text a:focus, .navbar-dark .navbar-text a:hover {\r\n                    color: #fff\r\n                }\r\n\r\n        .card {\r\n            position: relative;\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-orient: vertical;\r\n            -webkit-box-direction: normal;\r\n            -ms-flex-direction: column;\r\n            flex-direction: column;\r\n            min-width: 0;\r\n            word-wrap: break-word;\r\n            background-color: #fff;\r\n            background-clip: border-box;\r\n            border: 1px solid rgba(0,0,0,.125);\r\n            border-radius: .25rem\r\n        }\r\n\r\n            .card > hr {\r\n                margin-right: 0;\r\n                margin-left: 0\r\n            }\r\n\r\n            .card > .list-group:first-child .list-group-item:first-child {\r\n                border-top-left-radius: .25rem;\r\n                border-top-right-radius: .25rem\r\n            }\r\n\r\n            .card > .list-group:last-child .list-group-item:last-child {\r\n                border-bottom-right-radius: .25rem;\r\n                border-bottom-left-radius: .25rem\r\n            }\r\n\r\n        .card-body {\r\n            -webkit-box-flex: 1;\r\n            -ms-flex: 1 1 auto;\r\n            flex: 1 1 auto;\r\n            padding: 1.25rem\r\n        }\r\n\r\n        .card-title {\r\n            margin-bottom: .75rem\r\n        }\r\n\r\n        .card-subtitle {\r\n            margin-top: -.375rem;\r\n            margin-bottom: 0\r\n        }\r\n\r\n        .card-text:last-child {\r\n            margin-bottom: 0\r\n        }\r\n\r\n        .card-link:hover {\r\n            text-decoration: none\r\n        }\r\n\r\n        .card-link + .card-link {\r\n            margin-left: 1.25rem\r\n        }\r\n\r\n        .card-header {\r\n            padding: .75rem 1.25rem;\r\n            margin-bottom: 0;\r\n            background-color: rgba(0,0,0,.03);\r\n            border-bottom: 1px solid rgba(0,0,0,.125)\r\n        }\r\n\r\n            .card-header:first-child {\r\n                border-radius: calc(.25rem - 1px) calc(.25rem - 1px) 0 0\r\n            }\r\n\r\n            .card-header + .list-group .list-group-item:first-child {\r\n                border-top: 0\r\n            }\r\n\r\n        .card-footer {\r\n            padding: .75rem 1.25rem;\r\n            background-color: rgba(0,0,0,.03);\r\n            border-top: 1px solid rgba(0,0,0,.125)\r\n        }\r\n\r\n            .card-footer:last-child {\r\n                border-radius: 0 0 calc(.25rem - 1px) calc(.25rem - 1px)\r\n            }\r\n\r\n        .card-header-tabs {\r\n            margin-right: -.625rem;\r\n            margin-bottom: -.75rem;\r\n            margin-left: -.625rem;\r\n            border-bottom: 0\r\n        }\r\n\r\n        .card-header-pills {\r\n            margin-right: -.625rem;\r\n            margin-left: -.625rem\r\n        }\r\n\r\n        .card-img-overlay {\r\n            position: absolute;\r\n            top: 0;\r\n            right: 0;\r\n            bottom: 0;\r\n            left: 0;\r\n            padding: 1.25rem\r\n        }\r\n\r\n        .card-img {\r\n            width: 100%;\r\n            border-radius: calc(.25rem - 1px)\r\n        }\r\n\r\n        .card-img-top {\r\n            width: 100%;\r\n            border-top-left-radius: calc(.25rem - 1px);\r\n            border-top-right-radius: calc(.25rem - 1px)\r\n        }\r\n\r\n        .card-img-bottom {\r\n            width: 100%;\r\n            border-bottom-right-radius: calc(.25rem - 1px);\r\n            border-bottom-left-radius: calc(.25rem - 1px)\r\n        }\r\n\r\n        .card-deck {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-orient: vertical;\r\n            -webkit-box-direction: normal;\r\n            -ms-flex-direction: column;\r\n            flex-direction: column\r\n        }\r\n\r\n            .card-deck .card {\r\n                margin-bottom: 15px\r\n            }\r\n\r\n        @media (min-width:576px) {\r\n            .card-deck {\r\n                -webkit-box-orient: horizontal;\r\n                -webkit-box-direction: normal;\r\n                -ms-flex-flow: row wrap;\r\n                flex-flow: row wrap;\r\n                margin-right: -15px;\r\n                margin-left: -15px\r\n            }\r\n\r\n                .card-deck .card {\r\n                    display: -webkit-box;\r\n                    display: -ms-flexbox;\r\n                    display: flex;\r\n                    -webkit-box-flex: 1;\r\n                    -ms-flex: 1 0 0%;\r\n                    flex: 1 0 0%;\r\n                    -webkit-box-orient: vertical;\r\n                    -webkit-box-direction: normal;\r\n                    -ms-flex-direction: column;\r\n                    flex-direction: column;\r\n                    margin-right: 15px;\r\n                    margin-bottom: 0;\r\n                    margin-left: 15px\r\n                }\r\n        }\r\n\r\n        .card-group {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-orient: vertical;\r\n            -webkit-box-direction: normal;\r\n            -ms-flex-direction: column;\r\n            flex-direction: column\r\n        }\r\n\r\n            .card-group > .card {\r\n                margin-bottom: 15px\r\n            }\r\n\r\n        @media (min-width:576px) {\r\n            .card-group {\r\n                -webkit-box-orient: horizontal;\r\n                -webkit-box-direction: normal;\r\n                -ms-flex-flow: row wrap;\r\n                flex-flow: row wrap\r\n            }\r\n\r\n                .card-group > .card {\r\n                    -webkit-box-flex: 1;\r\n                    -ms-flex: 1 0 0%;\r\n                    flex: 1 0 0%;\r\n                    margin-bottom: 0\r\n                }\r\n\r\n                    .card-group > .card + .card {\r\n                        margin-left: 0;\r\n                        border-left: 0\r\n                    }\r\n\r\n                    .card-group > .card:first-child {\r\n                        border-top-right-radius: 0;\r\n                        border-bottom-right-radius: 0\r\n                    }\r\n\r\n                        .card-group > .card:first-child .card-header, .card-group > .card:first-child .card-img-top {\r\n                            border-top-right-radius: 0\r\n                        }\r\n\r\n                        .card-group > .card:first-child .card-footer, .card-group > .card:first-child .card-img-bottom {\r\n                            border-bottom-right-radius: 0\r\n                        }\r\n\r\n                    .card-group > .card:last-child {\r\n                        border-top-left-radius: 0;\r\n                        border-bottom-left-radius: 0\r\n                    }\r\n\r\n                        .card-group > .card:last-child .card-header, .card-group > .card:last-child .card-img-top {\r\n                            border-top-left-radius: 0\r\n                        }\r\n\r\n                        .card-group > .card:last-child .card-footer, .card-group > .card:last-child .card-img-bottom {\r\n                            border-bottom-left-radius: 0\r\n                        }\r\n\r\n                    .card-group > .card:only-child {\r\n                        border-radius: .25rem\r\n                    }\r\n\r\n                        .card-group > .card:only-child .card-header, .card-group > .card:only-child .card-img-top {\r\n                            border-top-left-radius: .25rem;\r\n                            border-top-right-radius: .25rem\r\n                        }\r\n\r\n                        .card-group > .card:only-child .card-footer, .card-group > .card:only-child .card-img-bottom {\r\n                            border-bottom-right-radius: .25rem;\r\n                            border-bottom-left-radius: .25rem\r\n                        }\r\n\r\n                    .card-group > .card:not(:first-child):not(:last-child):not(:only-child) {\r\n                        border-radius: 0\r\n                    }\r\n\r\n                        .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-footer, .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-header, .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-bottom, .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-top {\r\n                            border-radius: 0\r\n                        }\r\n        }\r\n\r\n        .card-columns .card {\r\n            margin-bottom: .75rem\r\n        }\r\n\r\n        @media (min-width:576px) {\r\n            .card-columns {\r\n                -webkit-column-count: 3;\r\n                -moz-column-count: 3;\r\n                column-count: 3;\r\n                -webkit-column-gap: 1.25rem;\r\n                -moz-column-gap: 1.25rem;\r\n                column-gap: 1.25rem\r\n            }\r\n\r\n                .card-columns .card {\r\n                    display: inline-block;\r\n                    width: 100%\r\n                }\r\n        }\r\n\r\n        .breadcrumb {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -ms-flex-wrap: wrap;\r\n            flex-wrap: wrap;\r\n            padding: .75rem 1rem;\r\n            margin-bottom: 1rem;\r\n            list-style: none;\r\n            background-color: #e9ecef;\r\n            border-radius: .25rem\r\n        }\r\n\r\n        .breadcrumb-item + .breadcrumb-item::before {\r\n            display: inline-block;\r\n            padding-right: .5rem;\r\n            padding-left: .5rem;\r\n            color: #6c757d;\r\n            content: \"/\"\r\n        }\r\n\r\n        .breadcrumb-item + .breadcrumb-item:hover::before {\r\n            text-decoration: underline\r\n        }\r\n\r\n        .breadcrumb-item + .breadcrumb-item:hover::before {\r\n            text-decoration: none\r\n        }\r\n\r\n        .breadcrumb-item.active {\r\n            color: #6c757d\r\n        }\r\n\r\n        .pagination {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            padding-left: 0;\r\n            list-style: none;\r\n            border-radius: .25rem\r\n        }\r\n\r\n        .page-link {\r\n            position: relative;\r\n            display: block;\r\n            padding: .5rem .75rem;\r\n            margin-left: -1px;\r\n            line-height: 1.25;\r\n            color: #007bff;\r\n            background-color: #fff;\r\n            border: 1px solid #dee2e6\r\n        }\r\n\r\n            .page-link:hover {\r\n                color: #0056b3;\r\n                text-decoration: none;\r\n                background-color: #e9ecef;\r\n                border-color: #dee2e6\r\n            }\r\n\r\n            .page-link:focus {\r\n                z-index: 2;\r\n                outline: 0;\r\n                box-shadow: 0 0 0 .2rem rgba(0,123,255,.25)\r\n            }\r\n\r\n            .page-link:not(:disabled):not(.disabled) {\r\n                cursor: pointer\r\n            }\r\n\r\n        .page-item:first-child .page-link {\r\n            margin-left: 0;\r\n            border-top-left-radius: .25rem;\r\n            border-bottom-left-radius: .25rem\r\n        }\r\n\r\n        .page-item:last-child .page-link {\r\n            border-top-right-radius: .25rem;\r\n            border-bottom-right-radius: .25rem\r\n        }\r\n\r\n        .page-item.active .page-link {\r\n            z-index: 1;\r\n            color: #fff;\r\n            background-color: #007bff;\r\n            border-color: #007bff\r\n        }\r\n\r\n        .page-item.disabled .page-link {\r\n            color: #6c757d;\r\n            pointer-events: none;\r\n            cursor: auto;\r\n            background-color: #fff;\r\n            border-color: #dee2e6\r\n        }\r\n\r\n        .pagination-lg .page-link {\r\n            padding: .75rem 1.5rem;\r\n            font-size: 1.25rem;\r\n            line-height: 1.5\r\n        }\r\n\r\n        .pagination-lg .page-item:first-child .page-link {\r\n            border-top-left-radius: .3rem;\r\n            border-bottom-left-radius: .3rem\r\n        }\r\n\r\n        .pagination-lg .page-item:last-child .page-link {\r\n            border-top-right-radius: .3rem;\r\n            border-bottom-right-radius: .3rem\r\n        }\r\n\r\n        .pagination-sm .page-link {\r\n            padding: .25rem .5rem;\r\n            font-size: .875rem;\r\n            line-height: 1.5\r\n        }\r\n\r\n        .pagination-sm .page-item:first-child .page-link {\r\n            border-top-left-radius: .2rem;\r\n            border-bottom-left-radius: .2rem\r\n        }\r\n\r\n        .pagination-sm .page-item:last-child .page-link {\r\n            border-top-right-radius: .2rem;\r\n            border-bottom-right-radius: .2rem\r\n        }\r\n\r\n        .badge {\r\n            display: inline-block;\r\n            padding: .25em .4em;\r\n            font-size: 75%;\r\n            font-weight: 700;\r\n            line-height: 1;\r\n            text-align: center;\r\n            white-space: nowrap;\r\n            vertical-align: baseline;\r\n            border-radius: .25rem\r\n        }\r\n\r\n            .badge:empty {\r\n                display: none\r\n            }\r\n\r\n        .btn .badge {\r\n            position: relative;\r\n            top: -1px\r\n        }\r\n\r\n        .badge-pill {\r\n            padding-right: .6em;\r\n            padding-left: .6em;\r\n            border-radius: 10rem\r\n        }\r\n\r\n        .badge-primary {\r\n            color: #fff;\r\n            background-color: #007bff\r\n        }\r\n\r\n            .badge-primary[href]:focus, .badge-primary[href]:hover {\r\n                color: #fff;\r\n                text-decoration: none;\r\n                background-color: #0062cc\r\n            }\r\n\r\n        .badge-secondary {\r\n            color: #fff;\r\n            background-color: #6c757d\r\n        }\r\n\r\n            .badge-secondary[href]:focus, .badge-secondary[href]:hover {\r\n                color: #fff;\r\n                text-decoration: none;\r\n                background-color: #545b62\r\n            }\r\n\r\n        .badge-success {\r\n            color: #fff;\r\n            background-color: #28a745\r\n        }\r\n\r\n            .badge-success[href]:focus, .badge-success[href]:hover {\r\n                color: #fff;\r\n                text-decoration: none;\r\n                background-color: #1e7e34\r\n            }\r\n\r\n        .badge-info {\r\n            color: #fff;\r\n            background-color: #17a2b8\r\n        }\r\n\r\n            .badge-info[href]:focus, .badge-info[href]:hover {\r\n                color: #fff;\r\n                text-decoration: none;\r\n                background-color: #117a8b\r\n            }\r\n\r\n        .badge-warning {\r\n            color: #212529;\r\n            background-color: #ffc107\r\n        }\r\n\r\n            .badge-warning[href]:focus, .badge-warning[href]:hover {\r\n                color: #212529;\r\n                text-decoration: none;\r\n                background-color: #d39e00\r\n            }\r\n\r\n        .badge-danger {\r\n            color: #fff;\r\n            background-color: #dc3545\r\n        }\r\n\r\n            .badge-danger[href]:focus, .badge-danger[href]:hover {\r\n                color: #fff;\r\n                text-decoration: none;\r\n                background-color: #bd2130\r\n            }\r\n\r\n        .badge-light {\r\n            color: #212529;\r\n            background-color: #f8f9fa\r\n        }\r\n\r\n            .badge-light[href]:focus, .badge-light[href]:hover {\r\n                color: #212529;\r\n                text-decoration: none;\r\n                background-color: #dae0e5\r\n            }\r\n\r\n        .badge-dark {\r\n            color: #fff;\r\n            background-color: #343a40\r\n        }\r\n\r\n            .badge-dark[href]:focus, .badge-dark[href]:hover {\r\n                color: #fff;\r\n                text-decoration: none;\r\n                background-color: #1d2124\r\n            }\r\n\r\n        .jumbotron {\r\n            padding: 2rem 1rem;\r\n            margin-bottom: 2rem;\r\n            background-color: #e9ecef;\r\n            border-radius: .3rem\r\n        }\r\n\r\n        @media (min-width:576px) {\r\n            .jumbotron {\r\n                padding: 4rem 2rem\r\n            }\r\n        }\r\n\r\n        .jumbotron-fluid {\r\n            padding-right: 0;\r\n            padding-left: 0;\r\n            border-radius: 0\r\n        }\r\n\r\n        .alert {\r\n            position: relative;\r\n            padding: .75rem 1.25rem;\r\n            margin-bottom: 1rem;\r\n            border: 1px solid transparent;\r\n            border-radius: .25rem\r\n        }\r\n\r\n        .alert-heading {\r\n            color: inherit\r\n        }\r\n\r\n        .alert-link {\r\n            font-weight: 700\r\n        }\r\n\r\n        .alert-dismissible {\r\n            padding-right: 4rem\r\n        }\r\n\r\n            .alert-dismissible .close {\r\n                position: absolute;\r\n                top: 0;\r\n                right: 0;\r\n                padding: .75rem 1.25rem;\r\n                color: inherit\r\n            }\r\n\r\n        .alert-primary {\r\n            color: #004085;\r\n            background-color: #cce5ff;\r\n            border-color: #b8daff\r\n        }\r\n\r\n            .alert-primary hr {\r\n                border-top-color: #9fcdff\r\n            }\r\n\r\n            .alert-primary .alert-link {\r\n                color: #002752\r\n            }\r\n\r\n        .alert-secondary {\r\n            color: #383d41;\r\n            background-color: #e2e3e5;\r\n            border-color: #d6d8db\r\n        }\r\n\r\n            .alert-secondary hr {\r\n                border-top-color: #c8cbcf\r\n            }\r\n\r\n            .alert-secondary .alert-link {\r\n                color: #202326\r\n            }\r\n\r\n        .alert-success {\r\n            color: #155724;\r\n            background-color: #d4edda;\r\n            border-color: #c3e6cb\r\n        }\r\n\r\n            .alert-success hr {\r\n                border-top-color: #b1dfbb\r\n            }\r\n\r\n            .alert-success .alert-link {\r\n                color: #0b2e13\r\n            }\r\n\r\n        .alert-info {\r\n            color: #0c5460;\r\n            background-color: #d1ecf1;\r\n            border-color: #bee5eb\r\n        }\r\n\r\n            .alert-info hr {\r\n                border-top-color: #abdde5\r\n            }\r\n\r\n            .alert-info .alert-link {\r\n                color: #062c33\r\n            }\r\n\r\n        .alert-warning {\r\n            color: #856404;\r\n            background-color: #fff3cd;\r\n            border-color: #ffeeba\r\n        }\r\n\r\n            .alert-warning hr {\r\n                border-top-color: #ffe8a1\r\n            }\r\n\r\n            .alert-warning .alert-link {\r\n                color: #533f03\r\n            }\r\n\r\n        .alert-danger {\r\n            color: #721c24;\r\n            background-color: #f8d7da;\r\n            border-color: #f5c6cb\r\n        }\r\n\r\n            .alert-danger hr {\r\n                border-top-color: #f1b0b7\r\n            }\r\n\r\n            .alert-danger .alert-link {\r\n                color: #491217\r\n            }\r\n\r\n        .alert-light {\r\n            color: #818182;\r\n            background-color: #fefefe;\r\n            border-color: #fdfdfe\r\n        }\r\n\r\n            .alert-light hr {\r\n                border-top-color: #ececf6\r\n            }\r\n\r\n            .alert-light .alert-link {\r\n                color: #686868\r\n            }\r\n\r\n        .alert-dark {\r\n            color: #1b1e21;\r\n            background-color: #d6d8d9;\r\n            border-color: #c6c8ca\r\n        }\r\n\r\n            .alert-dark hr {\r\n                border-top-color: #b9bbbe\r\n            }\r\n\r\n            .alert-dark .alert-link {\r\n                color: #040505\r\n            }\r\n\r\n        @-webkit-keyframes progress-bar-stripes {\r\n            from {\r\n                background-position: 1rem 0\r\n            }\r\n\r\n            to {\r\n                background-position: 0 0\r\n            }\r\n        }\r\n\r\n        @keyframes progress-bar-stripes {\r\n            from {\r\n                background-position: 1rem 0\r\n            }\r\n\r\n            to {\r\n                background-position: 0 0\r\n            }\r\n        }\r\n\r\n        .progress {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            height: 1rem;\r\n            overflow: hidden;\r\n            font-size: .75rem;\r\n            background-color: #e9ecef;\r\n            border-radius: .25rem\r\n        }\r\n\r\n        .progress-bar {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-orient: vertical;\r\n            -webkit-box-direction: normal;\r\n            -ms-flex-direction: column;\r\n            flex-direction: column;\r\n            -webkit-box-pack: center;\r\n            -ms-flex-pack: center;\r\n            justify-content: center;\r\n            color: #fff;\r\n            text-align: center;\r\n            background-color: #007bff;\r\n            transition: width .6s ease\r\n        }\r\n\r\n        .progress-bar-striped {\r\n            background-image: linear-gradient(45deg,rgba(255,255,255,.15) 25%,transparent 25%,transparent 50%,rgba(255,255,255,.15) 50%,rgba(255,255,255,.15) 75%,transparent 75%,transparent);\r\n            background-size: 1rem 1rem\r\n        }\r\n\r\n        .progress-bar-animated {\r\n            -webkit-animation: progress-bar-stripes 1s linear infinite;\r\n            animation: progress-bar-stripes 1s linear infinite\r\n        }\r\n\r\n        .media {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-align: start;\r\n            -ms-flex-align: start;\r\n            align-items: flex-start\r\n        }\r\n\r\n        .media-body {\r\n            -webkit-box-flex: 1;\r\n            -ms-flex: 1;\r\n            flex: 1\r\n        }\r\n\r\n        .list-group {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-orient: vertical;\r\n            -webkit-box-direction: normal;\r\n            -ms-flex-direction: column;\r\n            flex-direction: column;\r\n            padding-left: 0;\r\n            margin-bottom: 0\r\n        }\r\n\r\n        .list-group-item-action {\r\n            width: 100%;\r\n            color: #495057;\r\n            text-align: inherit\r\n        }\r\n\r\n            .list-group-item-action:focus, .list-group-item-action:hover {\r\n                color: #495057;\r\n                text-decoration: none;\r\n                background-color: #f8f9fa\r\n            }\r\n\r\n            .list-group-item-action:active {\r\n                color: #212529;\r\n                background-color: #e9ecef\r\n            }\r\n\r\n        .list-group-item {\r\n            position: relative;\r\n            display: block;\r\n            padding: .75rem 1.25rem;\r\n            margin-bottom: -1px;\r\n            background-color: #fff;\r\n            border: 1px solid rgba(0,0,0,.125)\r\n        }\r\n\r\n            .list-group-item:first-child {\r\n                border-top-left-radius: .25rem;\r\n                border-top-right-radius: .25rem\r\n            }\r\n\r\n            .list-group-item:last-child {\r\n                margin-bottom: 0;\r\n                border-bottom-right-radius: .25rem;\r\n                border-bottom-left-radius: .25rem\r\n            }\r\n\r\n            .list-group-item:focus, .list-group-item:hover {\r\n                z-index: 1;\r\n                text-decoration: none\r\n            }\r\n\r\n            .list-group-item.disabled, .list-group-item:disabled {\r\n                color: #6c757d;\r\n                background-color: #fff\r\n            }\r\n\r\n            .list-group-item.active {\r\n                z-index: 2;\r\n                color: #fff;\r\n                background-color: #007bff;\r\n                border-color: #007bff\r\n            }\r\n\r\n        .list-group-flush .list-group-item {\r\n            border-right: 0;\r\n            border-left: 0;\r\n            border-radius: 0\r\n        }\r\n\r\n        .list-group-flush:first-child .list-group-item:first-child {\r\n            border-top: 0\r\n        }\r\n\r\n        .list-group-flush:last-child .list-group-item:last-child {\r\n            border-bottom: 0\r\n        }\r\n\r\n        .list-group-item-primary {\r\n            color: #004085;\r\n            background-color: #b8daff\r\n        }\r\n\r\n            .list-group-item-primary.list-group-item-action:focus, .list-group-item-primary.list-group-item-action:hover {\r\n                color: #004085;\r\n                background-color: #9fcdff\r\n            }\r\n\r\n            .list-group-item-primary.list-group-item-action.active {\r\n                color: #fff;\r\n                background-color: #004085;\r\n                border-color: #004085\r\n            }\r\n\r\n        .list-group-item-secondary {\r\n            color: #383d41;\r\n            background-color: #d6d8db\r\n        }\r\n\r\n            .list-group-item-secondary.list-group-item-action:focus, .list-group-item-secondary.list-group-item-action:hover {\r\n                color: #383d41;\r\n                background-color: #c8cbcf\r\n            }\r\n\r\n            .list-group-item-secondary.list-group-item-action.active {\r\n                color: #fff;\r\n                background-color: #383d41;\r\n                border-color: #383d41\r\n            }\r\n\r\n        .list-group-item-success {\r\n            color: #155724;\r\n            background-color: #c3e6cb\r\n        }\r\n\r\n            .list-group-item-success.list-group-item-action:focus, .list-group-item-success.list-group-item-action:hover {\r\n                color: #155724;\r\n                background-color: #b1dfbb\r\n            }\r\n\r\n            .list-group-item-success.list-group-item-action.active {\r\n                color: #fff;\r\n                background-color: #155724;\r\n                border-color: #155724\r\n            }\r\n\r\n        .list-group-item-info {\r\n            color: #0c5460;\r\n            background-color: #bee5eb\r\n        }\r\n\r\n            .list-group-item-info.list-group-item-action:focus, .list-group-item-info.list-group-item-action:hover {\r\n                color: #0c5460;\r\n                background-color: #abdde5\r\n            }\r\n\r\n            .list-group-item-info.list-group-item-action.active {\r\n                color: #fff;\r\n                background-color: #0c5460;\r\n                border-color: #0c5460\r\n            }\r\n\r\n        .list-group-item-warning {\r\n            color: #856404;\r\n            background-color: #ffeeba\r\n        }\r\n\r\n            .list-group-item-warning.list-group-item-action:focus, .list-group-item-warning.list-group-item-action:hover {\r\n                color: #856404;\r\n                background-color: #ffe8a1\r\n            }\r\n\r\n            .list-group-item-warning.list-group-item-action.active {\r\n                color: #fff;\r\n                background-color: #856404;\r\n                border-color: #856404\r\n            }\r\n\r\n        .list-group-item-danger {\r\n            color: #721c24;\r\n            background-color: #f5c6cb\r\n        }\r\n\r\n            .list-group-item-danger.list-group-item-action:focus, .list-group-item-danger.list-group-item-action:hover {\r\n                color: #721c24;\r\n                background-color: #f1b0b7\r\n            }\r\n\r\n            .list-group-item-danger.list-group-item-action.active {\r\n                color: #fff;\r\n                background-color: #721c24;\r\n                border-color: #721c24\r\n            }\r\n\r\n        .list-group-item-light {\r\n            color: #818182;\r\n            background-color: #fdfdfe\r\n        }\r\n\r\n            .list-group-item-light.list-group-item-action:focus, .list-group-item-light.list-group-item-action:hover {\r\n                color: #818182;\r\n                background-color: #ececf6\r\n            }\r\n\r\n            .list-group-item-light.list-group-item-action.active {\r\n                color: #fff;\r\n                background-color: #818182;\r\n                border-color: #818182\r\n            }\r\n\r\n        .list-group-item-dark {\r\n            color: #1b1e21;\r\n            background-color: #c6c8ca\r\n        }\r\n\r\n            .list-group-item-dark.list-group-item-action:focus, .list-group-item-dark.list-group-item-action:hover {\r\n                color: #1b1e21;\r\n                background-color: #b9bbbe\r\n            }\r\n\r\n            .list-group-item-dark.list-group-item-action.active {\r\n                color: #fff;\r\n                background-color: #1b1e21;\r\n                border-color: #1b1e21\r\n            }\r\n\r\n        .close {\r\n            float: right;\r\n            font-size: 1.5rem;\r\n            font-weight: 700;\r\n            line-height: 1;\r\n            color: #000;\r\n            text-shadow: 0 1px 0 #fff;\r\n            opacity: .5\r\n        }\r\n\r\n            .close:focus, .close:hover {\r\n                color: #000;\r\n                text-decoration: none;\r\n                opacity: .75\r\n            }\r\n\r\n            .close:not(:disabled):not(.disabled) {\r\n                cursor: pointer\r\n            }\r\n\r\n        button.close {\r\n            padding: 0;\r\n            background-color: transparent;\r\n            border: 0;\r\n            -webkit-appearance: none\r\n        }\r\n\r\n        .modal-open {\r\n            overflow: hidden\r\n        }\r\n\r\n        .modal {\r\n            position: fixed;\r\n            top: 0;\r\n            right: 0;\r\n            bottom: 0;\r\n            left: 0;\r\n            z-index: 1050;\r\n            display: none;\r\n            overflow: hidden;\r\n            outline: 0\r\n        }\r\n\r\n        .modal-open .modal {\r\n            overflow-x: hidden;\r\n            overflow-y: auto\r\n        }\r\n\r\n        .modal-dialog {\r\n            position: relative;\r\n            width: auto;\r\n            margin: .5rem;\r\n            pointer-events: none\r\n        }\r\n\r\n        .modal.fade .modal-dialog {\r\n            transition: -webkit-transform .3s ease-out;\r\n            transition: transform .3s ease-out;\r\n            transition: transform .3s ease-out,-webkit-transform .3s ease-out;\r\n            -webkit-transform: translate(0,-25%);\r\n            transform: translate(0,-25%)\r\n        }\r\n\r\n        .modal.show .modal-dialog {\r\n            -webkit-transform: translate(0,0);\r\n            transform: translate(0,0)\r\n        }\r\n\r\n        .modal-dialog-centered {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-align: center;\r\n            -ms-flex-align: center;\r\n            align-items: center;\r\n            min-height: calc(100% - (.5rem * 2))\r\n        }\r\n\r\n        .modal-content {\r\n            position: relative;\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-orient: vertical;\r\n            -webkit-box-direction: normal;\r\n            -ms-flex-direction: column;\r\n            flex-direction: column;\r\n            width: 100%;\r\n            pointer-events: auto;\r\n            background-color: #fff;\r\n            background-clip: padding-box;\r\n            border: 1px solid rgba(0,0,0,.2);\r\n            border-radius: .3rem;\r\n            outline: 0\r\n        }\r\n\r\n        .modal-backdrop {\r\n            position: fixed;\r\n            top: 0;\r\n            right: 0;\r\n            bottom: 0;\r\n            left: 0;\r\n            z-index: 1040;\r\n            background-color: #000\r\n        }\r\n\r\n            .modal-backdrop.fade {\r\n                opacity: 0\r\n            }\r\n\r\n            .modal-backdrop.show {\r\n                opacity: .5\r\n            }\r\n\r\n        .modal-header {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-align: start;\r\n            -ms-flex-align: start;\r\n            align-items: flex-start;\r\n            -webkit-box-pack: justify;\r\n            -ms-flex-pack: justify;\r\n            justify-content: space-between;\r\n            padding: 1rem;\r\n            border-bottom: 1px solid #e9ecef;\r\n            border-top-left-radius: .3rem;\r\n            border-top-right-radius: .3rem\r\n        }\r\n\r\n            .modal-header .close {\r\n                padding: 1rem;\r\n                margin: -1rem -1rem -1rem auto\r\n            }\r\n\r\n        .modal-title {\r\n            margin-bottom: 0;\r\n            line-height: 1.5\r\n        }\r\n\r\n        .modal-body {\r\n            position: relative;\r\n            -webkit-box-flex: 1;\r\n            -ms-flex: 1 1 auto;\r\n            flex: 1 1 auto;\r\n            padding: 1rem\r\n        }\r\n\r\n        .modal-footer {\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-align: center;\r\n            -ms-flex-align: center;\r\n            align-items: center;\r\n            -webkit-box-pack: end;\r\n            -ms-flex-pack: end;\r\n            justify-content: flex-end;\r\n            padding: 1rem;\r\n            border-top: 1px solid #e9ecef\r\n        }\r\n\r\n            .modal-footer > :not(:first-child) {\r\n                margin-left: .25rem\r\n            }\r\n\r\n            .modal-footer > :not(:last-child) {\r\n                margin-right: .25rem\r\n            }\r\n\r\n        .modal-scrollbar-measure {\r\n            position: absolute;\r\n            top: -9999px;\r\n            width: 50px;\r\n            height: 50px;\r\n            overflow: scroll\r\n        }\r\n\r\n        @media (min-width:576px) {\r\n            .modal-dialog {\r\n                max-width: 500px;\r\n                margin: 1.75rem auto\r\n            }\r\n\r\n            .modal-dialog-centered {\r\n                min-height: calc(100% - (1.75rem * 2))\r\n            }\r\n\r\n            .modal-sm {\r\n                max-width: 300px\r\n            }\r\n        }\r\n\r\n        @media (min-width:992px) {\r\n            .modal-lg {\r\n                max-width: 800px\r\n            }\r\n        }\r\n\r\n        .tooltip {\r\n            position: absolute;\r\n            z-index: 1070;\r\n            display: block;\r\n            margin: 0;\r\n            font-family: -apple-system,BlinkMacSystemFont,\"Segoe UI\",Roboto,\"Helvetica Neue\",Arial,sans-serif,\"Apple Color Emoji\",\"Segoe UI Emoji\",\"Segoe UI Symbol\";\r\n            font-style: normal;\r\n            font-weight: 400;\r\n            line-height: 1.5;\r\n            text-align: left;\r\n            text-align: start;\r\n            text-decoration: none;\r\n            text-shadow: none;\r\n            text-transform: none;\r\n            letter-spacing: normal;\r\n            word-break: normal;\r\n            word-spacing: normal;\r\n            white-space: normal;\r\n            line-break: auto;\r\n            font-size: .875rem;\r\n            word-wrap: break-word;\r\n            opacity: 0\r\n        }\r\n\r\n            .tooltip.show {\r\n                opacity: .9\r\n            }\r\n\r\n            .tooltip .arrow {\r\n                position: absolute;\r\n                display: block;\r\n                width: .8rem;\r\n                height: .4rem\r\n            }\r\n\r\n                .tooltip .arrow::before {\r\n                    position: absolute;\r\n                    content: \"\";\r\n                    border-color: transparent;\r\n                    border-style: solid\r\n                }\r\n\r\n        .bs-tooltip-auto[x-placement^=top], .bs-tooltip-top {\r\n            padding: .4rem 0\r\n        }\r\n\r\n            .bs-tooltip-auto[x-placement^=top] .arrow, .bs-tooltip-top .arrow {\r\n                bottom: 0\r\n            }\r\n\r\n                .bs-tooltip-auto[x-placement^=top] .arrow::before, .bs-tooltip-top .arrow::before {\r\n                    top: 0;\r\n                    border-width: .4rem .4rem 0;\r\n                    border-top-color: #000\r\n                }\r\n\r\n        .bs-tooltip-auto[x-placement^=right], .bs-tooltip-right {\r\n            padding: 0 .4rem\r\n        }\r\n\r\n            .bs-tooltip-auto[x-placement^=right] .arrow, .bs-tooltip-right .arrow {\r\n                left: 0;\r\n                width: .4rem;\r\n                height: .8rem\r\n            }\r\n\r\n                .bs-tooltip-auto[x-placement^=right] .arrow::before, .bs-tooltip-right .arrow::before {\r\n                    right: 0;\r\n                    border-width: .4rem .4rem .4rem 0;\r\n                    border-right-color: #000\r\n                }\r\n\r\n        .bs-tooltip-auto[x-placement^=bottom], .bs-tooltip-bottom {\r\n            padding: .4rem 0\r\n        }\r\n\r\n            .bs-tooltip-auto[x-placement^=bottom] .arrow, .bs-tooltip-bottom .arrow {\r\n                top: 0\r\n            }\r\n\r\n                .bs-tooltip-auto[x-placement^=bottom] .arrow::before, .bs-tooltip-bottom .arrow::before {\r\n                    bottom: 0;\r\n                    border-width: 0 .4rem .4rem;\r\n                    border-bottom-color: #000\r\n                }\r\n\r\n        .bs-tooltip-auto[x-placement^=left], .bs-tooltip-left {\r\n            padding: 0 .4rem\r\n        }\r\n\r\n            .bs-tooltip-auto[x-placement^=left] .arrow, .bs-tooltip-left .arrow {\r\n                right: 0;\r\n                width: .4rem;\r\n                height: .8rem\r\n            }\r\n\r\n                .bs-tooltip-auto[x-placement^=left] .arrow::before, .bs-tooltip-left .arrow::before {\r\n                    left: 0;\r\n                    border-width: .4rem 0 .4rem .4rem;\r\n                    border-left-color: #000\r\n                }\r\n\r\n        .tooltip-inner {\r\n            max-width: 200px;\r\n            padding: .25rem .5rem;\r\n            color: #fff;\r\n            text-align: center;\r\n            background-color: #000;\r\n            border-radius: .25rem\r\n        }\r\n\r\n        .popover {\r\n            position: absolute;\r\n            top: 0;\r\n            left: 0;\r\n            z-index: 1060;\r\n            display: block;\r\n            max-width: 276px;\r\n            font-family: -apple-system,BlinkMacSystemFont,\"Segoe UI\",Roboto,\"Helvetica Neue\",Arial,sans-serif,\"Apple Color Emoji\",\"Segoe UI Emoji\",\"Segoe UI Symbol\";\r\n            font-style: normal;\r\n            font-weight: 400;\r\n            line-height: 1.5;\r\n            text-align: left;\r\n            text-align: start;\r\n            text-decoration: none;\r\n            text-shadow: none;\r\n            text-transform: none;\r\n            letter-spacing: normal;\r\n            word-break: normal;\r\n            word-spacing: normal;\r\n            white-space: normal;\r\n            line-break: auto;\r\n            font-size: .875rem;\r\n            word-wrap: break-word;\r\n            background-color: #fff;\r\n            background-clip: padding-box;\r\n            border: 1px solid rgba(0,0,0,.2);\r\n            border-radius: .3rem\r\n        }\r\n\r\n            .popover .arrow {\r\n                position: absolute;\r\n                display: block;\r\n                width: 1rem;\r\n                height: .5rem;\r\n                margin: 0 .3rem\r\n            }\r\n\r\n                .popover .arrow::after, .popover .arrow::before {\r\n                    position: absolute;\r\n                    display: block;\r\n                    content: \"\";\r\n                    border-color: transparent;\r\n                    border-style: solid\r\n                }\r\n\r\n        .bs-popover-auto[x-placement^=top], .bs-popover-top {\r\n            margin-bottom: .5rem\r\n        }\r\n\r\n            .bs-popover-auto[x-placement^=top] .arrow, .bs-popover-top .arrow {\r\n                bottom: calc((.5rem + 1px) * -1)\r\n            }\r\n\r\n                .bs-popover-auto[x-placement^=top] .arrow::after, .bs-popover-auto[x-placement^=top] .arrow::before, .bs-popover-top .arrow::after, .bs-popover-top .arrow::before {\r\n                    border-width: .5rem .5rem 0\r\n                }\r\n\r\n                .bs-popover-auto[x-placement^=top] .arrow::before, .bs-popover-top .arrow::before {\r\n                    bottom: 0;\r\n                    border-top-color: rgba(0,0,0,.25)\r\n                }\r\n\r\n                .bs-popover-auto[x-placement^=top] .arrow::after, .bs-popover-top .arrow::after {\r\n                    bottom: 1px;\r\n                    border-top-color: #fff\r\n                }\r\n\r\n        .bs-popover-auto[x-placement^=right], .bs-popover-right {\r\n            margin-left: .5rem\r\n        }\r\n\r\n            .bs-popover-auto[x-placement^=right] .arrow, .bs-popover-right .arrow {\r\n                left: calc((.5rem + 1px) * -1);\r\n                width: .5rem;\r\n                height: 1rem;\r\n                margin: .3rem 0\r\n            }\r\n\r\n                .bs-popover-auto[x-placement^=right] .arrow::after, .bs-popover-auto[x-placement^=right] .arrow::before, .bs-popover-right .arrow::after, .bs-popover-right .arrow::before {\r\n                    border-width: .5rem .5rem .5rem 0\r\n                }\r\n\r\n                .bs-popover-auto[x-placement^=right] .arrow::before, .bs-popover-right .arrow::before {\r\n                    left: 0;\r\n                    border-right-color: rgba(0,0,0,.25)\r\n                }\r\n\r\n                .bs-popover-auto[x-placement^=right] .arrow::after, .bs-popover-right .arrow::after {\r\n                    left: 1px;\r\n                    border-right-color: #fff\r\n                }\r\n\r\n        .bs-popover-auto[x-placement^=bottom], .bs-popover-bottom {\r\n            margin-top: .5rem\r\n        }\r\n\r\n            .bs-popover-auto[x-placement^=bottom] .arrow, .bs-popover-bottom .arrow {\r\n                top: calc((.5rem + 1px) * -1)\r\n            }\r\n\r\n                .bs-popover-auto[x-placement^=bottom] .arrow::after, .bs-popover-auto[x-placement^=bottom] .arrow::before, .bs-popover-bottom .arrow::after, .bs-popover-bottom .arrow::before {\r\n                    border-width: 0 .5rem .5rem .5rem\r\n                }\r\n\r\n                .bs-popover-auto[x-placement^=bottom] .arrow::before, .bs-popover-bottom .arrow::before {\r\n                    top: 0;\r\n                    border-bottom-color: rgba(0,0,0,.25)\r\n                }\r\n\r\n                .bs-popover-auto[x-placement^=bottom] .arrow::after, .bs-popover-bottom .arrow::after {\r\n                    top: 1px;\r\n                    border-bottom-color: #fff\r\n                }\r\n\r\n            .bs-popover-auto[x-placement^=bottom] .popover-header::before, .bs-popover-bottom .popover-header::before {\r\n                position: absolute;\r\n                top: 0;\r\n                left: 50%;\r\n                display: block;\r\n                width: 1rem;\r\n                margin-left: -.5rem;\r\n                content: \"\";\r\n                border-bottom: 1px solid #f7f7f7\r\n            }\r\n\r\n        .bs-popover-auto[x-placement^=left], .bs-popover-left {\r\n            margin-right: .5rem\r\n        }\r\n\r\n            .bs-popover-auto[x-placement^=left] .arrow, .bs-popover-left .arrow {\r\n                right: calc((.5rem + 1px) * -1);\r\n                width: .5rem;\r\n                height: 1rem;\r\n                margin: .3rem 0\r\n            }\r\n\r\n                .bs-popover-auto[x-placement^=left] .arrow::after, .bs-popover-auto[x-placement^=left] .arrow::before, .bs-popover-left .arrow::after, .bs-popover-left .arrow::before {\r\n                    border-width: .5rem 0 .5rem .5rem\r\n                }\r\n\r\n                .bs-popover-auto[x-placement^=left] .arrow::before, .bs-popover-left .arrow::before {\r\n                    right: 0;\r\n                    border-left-color: rgba(0,0,0,.25)\r\n                }\r\n\r\n                .bs-popover-auto[x-placement^=left] .arrow::after, .bs-popover-left .arrow::after {\r\n                    right: 1px;\r\n                    border-left-color: #fff\r\n                }\r\n\r\n        .popover-header {\r\n            padding: .5rem .75rem;\r\n            margin-bottom: 0;\r\n            font-size: 1rem;\r\n            color: inherit;\r\n            background-color: #f7f7f7;\r\n            border-bottom: 1px solid #ebebeb;\r\n            border-top-left-radius: calc(.3rem - 1px);\r\n            border-top-right-radius: calc(.3rem - 1px)\r\n        }\r\n\r\n            .popover-header:empty {\r\n                display: none\r\n            }\r\n\r\n        .popover-body {\r\n            padding: .5rem .75rem;\r\n            color: #212529\r\n        }\r\n\r\n        .carousel {\r\n            position: relative\r\n        }\r\n\r\n        .carousel-inner {\r\n            position: relative;\r\n            width: 100%;\r\n            overflow: hidden\r\n        }\r\n\r\n        .carousel-item {\r\n            position: relative;\r\n            display: none;\r\n            -webkit-box-align: center;\r\n            -ms-flex-align: center;\r\n            align-items: center;\r\n            width: 100%;\r\n            transition: -webkit-transform .6s ease;\r\n            transition: transform .6s ease;\r\n            transition: transform .6s ease,-webkit-transform .6s ease;\r\n            -webkit-backface-visibility: hidden;\r\n            backface-visibility: hidden;\r\n            -webkit-perspective: 1000px;\r\n            perspective: 1000px\r\n        }\r\n\r\n            .carousel-item-next, .carousel-item-prev, .carousel-item.active {\r\n                display: block\r\n            }\r\n\r\n        .carousel-item-next, .carousel-item-prev {\r\n            position: absolute;\r\n            top: 0\r\n        }\r\n\r\n            .carousel-item-next.carousel-item-left, .carousel-item-prev.carousel-item-right {\r\n                -webkit-transform: translateX(0);\r\n                transform: translateX(0)\r\n            }\r\n\r\n        @supports ((-webkit-transform-style:preserve-3d) or (transform-style:preserve-3d)) {\r\n            .carousel-item-next.carousel-item-left, .carousel-item-prev.carousel-item-right {\r\n                -webkit-transform: translate3d(0,0,0);\r\n                transform: translate3d(0,0,0)\r\n            }\r\n        }\r\n\r\n        .active.carousel-item-right, .carousel-item-next {\r\n            -webkit-transform: translateX(100%);\r\n            transform: translateX(100%)\r\n        }\r\n\r\n        @supports ((-webkit-transform-style:preserve-3d) or (transform-style:preserve-3d)) {\r\n            .active.carousel-item-right, .carousel-item-next {\r\n                -webkit-transform: translate3d(100%,0,0);\r\n                transform: translate3d(100%,0,0)\r\n            }\r\n        }\r\n\r\n        .active.carousel-item-left, .carousel-item-prev {\r\n            -webkit-transform: translateX(-100%);\r\n            transform: translateX(-100%)\r\n        }\r\n\r\n        @supports ((-webkit-transform-style:preserve-3d) or (transform-style:preserve-3d)) {\r\n            .active.carousel-item-left, .carousel-item-prev {\r\n                -webkit-transform: translate3d(-100%,0,0);\r\n                transform: translate3d(-100%,0,0)\r\n            }\r\n        }\r\n\r\n        .carousel-control-next, .carousel-control-prev {\r\n            position: absolute;\r\n            top: 0;\r\n            bottom: 0;\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-align: center;\r\n            -ms-flex-align: center;\r\n            align-items: center;\r\n            -webkit-box-pack: center;\r\n            -ms-flex-pack: center;\r\n            justify-content: center;\r\n            width: 15%;\r\n            color: #fff;\r\n            text-align: center;\r\n            opacity: .5\r\n        }\r\n\r\n            .carousel-control-next:focus, .carousel-control-next:hover, .carousel-control-prev:focus, .carousel-control-prev:hover {\r\n                color: #fff;\r\n                text-decoration: none;\r\n                outline: 0;\r\n                opacity: .9\r\n            }\r\n\r\n        .carousel-control-prev {\r\n            left: 0\r\n        }\r\n\r\n        .carousel-control-next {\r\n            right: 0\r\n        }\r\n\r\n        .carousel-control-next-icon, .carousel-control-prev-icon {\r\n            display: inline-block;\r\n            width: 20px;\r\n            height: 20px;\r\n            background: transparent no-repeat center center;\r\n            background-size: 100% 100%\r\n        }\r\n\r\n        .carousel-control-prev-icon {\r\n            background-image: url(\"data:image/svg+xml;charset=utf8,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='%23fff' viewBox='0 0 8 8'%3E%3Cpath d='M5.25 0l-4 4 4 4 1.5-1.5-2.5-2.5 2.5-2.5-1.5-1.5z'/%3E%3C/svg%3E\")\r\n        }\r\n\r\n        .carousel-control-next-icon {\r\n            background-image: url(\"data:image/svg+xml;charset=utf8,%3Csvg xmlns='http://www.w3.org/2000/svg' fill='%23fff' viewBox='0 0 8 8'%3E%3Cpath d='M2.75 0l-1.5 1.5 2.5 2.5-2.5 2.5 1.5 1.5 4-4-4-4z'/%3E%3C/svg%3E\")\r\n        }\r\n\r\n        .carousel-indicators {\r\n            position: absolute;\r\n            right: 0;\r\n            bottom: 10px;\r\n            left: 0;\r\n            z-index: 15;\r\n            display: -webkit-box;\r\n            display: -ms-flexbox;\r\n            display: flex;\r\n            -webkit-box-pack: center;\r\n            -ms-flex-pack: center;\r\n            justify-content: center;\r\n            padding-left: 0;\r\n            margin-right: 15%;\r\n            margin-left: 15%;\r\n            list-style: none\r\n        }\r\n\r\n            .carousel-indicators li {\r\n                position: relative;\r\n                -webkit-box-flex: 0;\r\n                -ms-flex: 0 1 auto;\r\n                flex: 0 1 auto;\r\n                width: 30px;\r\n                height: 3px;\r\n                margin-right: 3px;\r\n                margin-left: 3px;\r\n                text-indent: -999px;\r\n                background-color: rgba(255,255,255,.5)\r\n            }\r\n\r\n                .carousel-indicators li::before {\r\n                    position: absolute;\r\n                    top: -10px;\r\n                    left: 0;\r\n                    display: inline-block;\r\n                    width: 100%;\r\n                    height: 10px;\r\n                    content: \"\"\r\n                }\r\n\r\n                .carousel-indicators li::after {\r\n                    position: absolute;\r\n                    bottom: -10px;\r\n                    left: 0;\r\n                    display: inline-block;\r\n                    width: 100%;\r\n                    height: 10px;\r\n                    content: \"\"\r\n                }\r\n\r\n            .carousel-indicators .active {\r\n                background-color: #fff\r\n            }\r\n\r\n        .carousel-caption {\r\n            position: absolute;\r\n            right: 15%;\r\n            bottom: 20px;\r\n            left: 15%;\r\n            z-index: 10;\r\n            padding-top: 20px;\r\n            padding-bottom: 20px;\r\n            color: #fff;\r\n            text-align: center\r\n        }\r\n\r\n        .align-baseline {\r\n            vertical-align: baseline !important\r\n        }\r\n\r\n        .align-top {\r\n            vertical-align: top !important\r\n        }\r\n\r\n        .align-middle {\r\n            vertical-align: middle !important\r\n        }\r\n\r\n        .align-bottom {\r\n            vertical-align: bottom !important\r\n        }\r\n\r\n        .align-text-bottom {\r\n            vertical-align: text-bottom !important\r\n        }\r\n\r\n        .align-text-top {\r\n            vertical-align: text-top !important\r\n        }\r\n\r\n        .bg-primary {\r\n            background-color: #007bff !important\r\n        }\r\n\r\n        a.bg-primary:focus, a.bg-primary:hover, button.bg-primary:focus, button.bg-primary:hover {\r\n            background-color: #0062cc !important\r\n        }\r\n\r\n        .bg-secondary {\r\n            background-color: #6c757d !important\r\n        }\r\n\r\n        a.bg-secondary:focus, a.bg-secondary:hover, button.bg-secondary:focus, button.bg-secondary:hover {\r\n            background-color: #545b62 !important\r\n        }\r\n\r\n        .bg-success {\r\n            background-color: #28a745 !important\r\n        }\r\n\r\n        a.bg-success:focus, a.bg-success:hover, button.bg-success:focus, button.bg-success:hover {\r\n            background-color: #1e7e34 !important\r\n        }\r\n\r\n        .bg-info {\r\n            background-color: #17a2b8 !important\r\n        }\r\n\r\n        a.bg-info:focus, a.bg-info:hover, button.bg-info:focus, button.bg-info:hover {\r\n            background-color: #117a8b !important\r\n        }\r\n\r\n        .bg-warning {\r\n            background-color: #ffc107 !important\r\n        }\r\n\r\n        a.bg-warning:focus, a.bg-warning:hover, button.bg-warning:focus, button.bg-warning:hover {\r\n            background-color: #d39e00 !important\r\n        }\r\n\r\n        .bg-danger {\r\n            background-color: #dc3545 !important\r\n        }\r\n\r\n        a.bg-danger:focus, a.bg-danger:hover, button.bg-danger:focus, button.bg-danger:hover {\r\n            background-color: #bd2130 !important\r\n        }\r\n\r\n        .bg-light {\r\n            background-color: #f8f9fa !important\r\n        }\r\n\r\n        a.bg-light:focus, a.bg-light:hover, button.bg-light:focus, button.bg-light:hover {\r\n            background-color: #dae0e5 !important\r\n        }\r\n\r\n        .bg-dark {\r\n            background-color: #343a40 !important\r\n        }\r\n\r\n        a.bg-dark:focus, a.bg-dark:hover, button.bg-dark:focus, button.bg-dark:hover {\r\n            background-color: #1d2124 !important\r\n        }\r\n\r\n        .bg-white {\r\n            background-color: #fff !important\r\n        }\r\n\r\n        .bg-transparent {\r\n            background-color: transparent !important\r\n        }\r\n\r\n        .border {\r\n            border: 1px solid #dee2e6 !important\r\n        }\r\n\r\n        .border-top {\r\n            border-top: 1px solid #dee2e6 !important\r\n        }\r\n\r\n        .border-right {\r\n            border-right: 1px solid #dee2e6 !important\r\n        }\r\n\r\n        .border-bottom {\r\n            border-bottom: 1px solid #dee2e6 !important\r\n        }\r\n\r\n        .border-left {\r\n            border-left: 1px solid #dee2e6 !important\r\n        }\r\n\r\n        .border-0 {\r\n            border: 0 !important\r\n        }\r\n\r\n        .border-top-0 {\r\n            border-top: 0 !important\r\n        }\r\n\r\n        .border-right-0 {\r\n            border-right: 0 !important\r\n        }\r\n\r\n        .border-bottom-0 {\r\n            border-bottom: 0 !important\r\n        }\r\n\r\n        .border-left-0 {\r\n            border-left: 0 !important\r\n        }\r\n\r\n        .border-primary {\r\n            border-color: #007bff !important\r\n        }\r\n\r\n        .border-secondary {\r\n            border-color: #6c757d !important\r\n        }\r\n\r\n        .border-success {\r\n            border-color: #28a745 !important\r\n        }\r\n\r\n        .border-info {\r\n            border-color: #17a2b8 !important\r\n        }\r\n\r\n        .border-warning {\r\n            border-color: #ffc107 !important\r\n        }\r\n\r\n        .border-danger {\r\n            border-color: #dc3545 !important\r\n        }\r\n\r\n        .border-light {\r\n            border-color: #f8f9fa !important\r\n        }\r\n\r\n        .border-dark {\r\n            border-color: #343a40 !important\r\n        }\r\n\r\n        .border-white {\r\n            border-color: #fff !important\r\n        }\r\n\r\n        .rounded {\r\n            border-radius: .25rem !important\r\n        }\r\n\r\n        .rounded-top {\r\n            border-top-left-radius: .25rem !important;\r\n            border-top-right-radius: .25rem !important\r\n        }\r\n\r\n        .rounded-right {\r\n            border-top-right-radius: .25rem !important;\r\n            border-bottom-right-radius: .25rem !important\r\n        }\r\n\r\n        .rounded-bottom {\r\n            border-bottom-right-radius: .25rem !important;\r\n            border-bottom-left-radius: .25rem !important\r\n        }\r\n\r\n        .rounded-left {\r\n            border-top-left-radius: .25rem !important;\r\n            border-bottom-left-radius: .25rem !important\r\n        }\r\n\r\n        .rounded-circle {\r\n            border-radius: 50% !important\r\n        }\r\n\r\n        .rounded-0 {\r\n            border-radius: 0 !important\r\n        }\r\n\r\n        .clearfix::after {\r\n            display: block;\r\n            clear: both;\r\n            content: \"\"\r\n        }\r\n\r\n        .d-none {\r\n            display: none !important\r\n        }\r\n\r\n        .d-inline {\r\n            display: inline !important\r\n        }\r\n\r\n        .d-inline-block {\r\n            display: inline-block !important\r\n        }\r\n\r\n        .d-block {\r\n            display: block !important\r\n        }\r\n\r\n        .d-table {\r\n            display: table !important\r\n        }\r\n\r\n        .d-table-row {\r\n            display: table-row !important\r\n        }\r\n\r\n        .d-table-cell {\r\n            display: table-cell !important\r\n        }\r\n\r\n        .d-flex {\r\n            display: -webkit-box !important;\r\n            display: -ms-flexbox !important;\r\n            display: flex !important\r\n        }\r\n\r\n        .d-inline-flex {\r\n            display: -webkit-inline-box !important;\r\n            display: -ms-inline-flexbox !important;\r\n            display: inline-flex !important\r\n        }\r\n\r\n        @media (min-width:576px) {\r\n            .d-sm-none {\r\n                display: none !important\r\n            }\r\n\r\n            .d-sm-inline {\r\n                display: inline !important\r\n            }\r\n\r\n            .d-sm-inline-block {\r\n                display: inline-block !important\r\n            }\r\n\r\n            .d-sm-block {\r\n                display: block !important\r\n            }\r\n\r\n            .d-sm-table {\r\n                display: table !important\r\n            }\r\n\r\n            .d-sm-table-row {\r\n                display: table-row !important\r\n            }\r\n\r\n            .d-sm-table-cell {\r\n                display: table-cell !important\r\n            }\r\n\r\n            .d-sm-flex {\r\n                display: -webkit-box !important;\r\n                display: -ms-flexbox !important;\r\n                display: flex !important\r\n            }\r\n\r\n            .d-sm-inline-flex {\r\n                display: -webkit-inline-box !important;\r\n                display: -ms-inline-flexbox !important;\r\n                display: inline-flex !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:768px) {\r\n            .d-md-none {\r\n                display: none !important\r\n            }\r\n\r\n            .d-md-inline {\r\n                display: inline !important\r\n            }\r\n\r\n            .d-md-inline-block {\r\n                display: inline-block !important\r\n            }\r\n\r\n            .d-md-block {\r\n                display: block !important\r\n            }\r\n\r\n            .d-md-table {\r\n                display: table !important\r\n            }\r\n\r\n            .d-md-table-row {\r\n                display: table-row !important\r\n            }\r\n\r\n            .d-md-table-cell {\r\n                display: table-cell !important\r\n            }\r\n\r\n            .d-md-flex {\r\n                display: -webkit-box !important;\r\n                display: -ms-flexbox !important;\r\n                display: flex !important\r\n            }\r\n\r\n            .d-md-inline-flex {\r\n                display: -webkit-inline-box !important;\r\n                display: -ms-inline-flexbox !important;\r\n                display: inline-flex !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:992px) {\r\n            .d-lg-none {\r\n                display: none !important\r\n            }\r\n\r\n            .d-lg-inline {\r\n                display: inline !important\r\n            }\r\n\r\n            .d-lg-inline-block {\r\n                display: inline-block !important\r\n            }\r\n\r\n            .d-lg-block {\r\n                display: block !important\r\n            }\r\n\r\n            .d-lg-table {\r\n                display: table !important\r\n            }\r\n\r\n            .d-lg-table-row {\r\n                display: table-row !important\r\n            }\r\n\r\n            .d-lg-table-cell {\r\n                display: table-cell !important\r\n            }\r\n\r\n            .d-lg-flex {\r\n                display: -webkit-box !important;\r\n                display: -ms-flexbox !important;\r\n                display: flex !important\r\n            }\r\n\r\n            .d-lg-inline-flex {\r\n                display: -webkit-inline-box !important;\r\n                display: -ms-inline-flexbox !important;\r\n                display: inline-flex !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:1200px) {\r\n            .d-xl-none {\r\n                display: none !important\r\n            }\r\n\r\n            .d-xl-inline {\r\n                display: inline !important\r\n            }\r\n\r\n            .d-xl-inline-block {\r\n                display: inline-block !important\r\n            }\r\n\r\n            .d-xl-block {\r\n                display: block !important\r\n            }\r\n\r\n            .d-xl-table {\r\n                display: table !important\r\n            }\r\n\r\n            .d-xl-table-row {\r\n                display: table-row !important\r\n            }\r\n\r\n            .d-xl-table-cell {\r\n                display: table-cell !important\r\n            }\r\n\r\n            .d-xl-flex {\r\n                display: -webkit-box !important;\r\n                display: -ms-flexbox !important;\r\n                display: flex !important\r\n            }\r\n\r\n            .d-xl-inline-flex {\r\n                display: -webkit-inline-box !important;\r\n                display: -ms-inline-flexbox !important;\r\n                display: inline-flex !important\r\n            }\r\n        }\r\n\r\n        @media print {\r\n            .d-print-none {\r\n                display: none !important\r\n            }\r\n\r\n            .d-print-inline {\r\n                display: inline !important\r\n            }\r\n\r\n            .d-print-inline-block {\r\n                display: inline-block !important\r\n            }\r\n\r\n            .d-print-block {\r\n                display: block !important\r\n            }\r\n\r\n            .d-print-table {\r\n                display: table !important\r\n            }\r\n\r\n            .d-print-table-row {\r\n                display: table-row !important\r\n            }\r\n\r\n            .d-print-table-cell {\r\n                display: table-cell !important\r\n            }\r\n\r\n            .d-print-flex {\r\n                display: -webkit-box !important;\r\n                display: -ms-flexbox !important;\r\n                display: flex !important\r\n            }\r\n\r\n            .d-print-inline-flex {\r\n                display: -webkit-inline-box !important;\r\n                display: -ms-inline-flexbox !important;\r\n                display: inline-flex !important\r\n            }\r\n        }\r\n\r\n        .embed-responsive {\r\n            position: relative;\r\n            display: block;\r\n            width: 100%;\r\n            padding: 0;\r\n            overflow: hidden\r\n        }\r\n\r\n            .embed-responsive::before {\r\n                display: block;\r\n                content: \"\"\r\n            }\r\n\r\n            .embed-responsive .embed-responsive-item, .embed-responsive embed, .embed-responsive iframe, .embed-responsive object, .embed-responsive video {\r\n                position: absolute;\r\n                top: 0;\r\n                bottom: 0;\r\n                left: 0;\r\n                width: 100%;\r\n                height: 100%;\r\n                border: 0\r\n            }\r\n\r\n        .embed-responsive-21by9::before {\r\n            padding-top: 42.857143%\r\n        }\r\n\r\n        .embed-responsive-16by9::before {\r\n            padding-top: 56.25%\r\n        }\r\n\r\n        .embed-responsive-4by3::before {\r\n            padding-top: 75%\r\n        }\r\n\r\n        .embed-responsive-1by1::before {\r\n            padding-top: 100%\r\n        }\r\n\r\n        .flex-row {\r\n            -webkit-box-orient: horizontal !important;\r\n            -webkit-box-direction: normal !important;\r\n            -ms-flex-direction: row !important;\r\n            flex-direction: row !important\r\n        }\r\n\r\n        .flex-column {\r\n            -webkit-box-orient: vertical !important;\r\n            -webkit-box-direction: normal !important;\r\n            -ms-flex-direction: column !important;\r\n            flex-direction: column !important\r\n        }\r\n\r\n        .flex-row-reverse {\r\n            -webkit-box-orient: horizontal !important;\r\n            -webkit-box-direction: reverse !important;\r\n            -ms-flex-direction: row-reverse !important;\r\n            flex-direction: row-reverse !important\r\n        }\r\n\r\n        .flex-column-reverse {\r\n            -webkit-box-orient: vertical !important;\r\n            -webkit-box-direction: reverse !important;\r\n            -ms-flex-direction: column-reverse !important;\r\n            flex-direction: column-reverse !important\r\n        }\r\n\r\n        .flex-wrap {\r\n            -ms-flex-wrap: wrap !important;\r\n            flex-wrap: wrap !important\r\n        }\r\n\r\n        .flex-nowrap {\r\n            -ms-flex-wrap: nowrap !important;\r\n            flex-wrap: nowrap !important\r\n        }\r\n\r\n        .flex-wrap-reverse {\r\n            -ms-flex-wrap: wrap-reverse !important;\r\n            flex-wrap: wrap-reverse !important\r\n        }\r\n\r\n        .justify-content-start {\r\n            -webkit-box-pack: start !important;\r\n            -ms-flex-pack: start !important;\r\n            justify-content: flex-start !important\r\n        }\r\n\r\n        .justify-content-end {\r\n            -webkit-box-pack: end !important;\r\n            -ms-flex-pack: end !important;\r\n            justify-content: flex-end !important\r\n        }\r\n\r\n        .justify-content-center {\r\n            -webkit-box-pack: center !important;\r\n            -ms-flex-pack: center !important;\r\n            justify-content: center !important\r\n        }\r\n\r\n        .justify-content-between {\r\n            -webkit-box-pack: justify !important;\r\n            -ms-flex-pack: justify !important;\r\n            justify-content: space-between !important\r\n        }\r\n\r\n        .justify-content-around {\r\n            -ms-flex-pack: distribute !important;\r\n            justify-content: space-around !important\r\n        }\r\n\r\n        .align-items-start {\r\n            -webkit-box-align: start !important;\r\n            -ms-flex-align: start !important;\r\n            align-items: flex-start !important\r\n        }\r\n\r\n        .align-items-end {\r\n            -webkit-box-align: end !important;\r\n            -ms-flex-align: end !important;\r\n            align-items: flex-end !important\r\n        }\r\n\r\n        .align-items-center {\r\n            -webkit-box-align: center !important;\r\n            -ms-flex-align: center !important;\r\n            align-items: center !important\r\n        }\r\n\r\n        .align-items-baseline {\r\n            -webkit-box-align: baseline !important;\r\n            -ms-flex-align: baseline !important;\r\n            align-items: baseline !important\r\n        }\r\n\r\n        .align-items-stretch {\r\n            -webkit-box-align: stretch !important;\r\n            -ms-flex-align: stretch !important;\r\n            align-items: stretch !important\r\n        }\r\n\r\n        .align-content-start {\r\n            -ms-flex-line-pack: start !important;\r\n            align-content: flex-start !important\r\n        }\r\n\r\n        .align-content-end {\r\n            -ms-flex-line-pack: end !important;\r\n            align-content: flex-end !important\r\n        }\r\n\r\n        .align-content-center {\r\n            -ms-flex-line-pack: center !important;\r\n            align-content: center !important\r\n        }\r\n\r\n        .align-content-between {\r\n            -ms-flex-line-pack: justify !important;\r\n            align-content: space-between !important\r\n        }\r\n\r\n        .align-content-around {\r\n            -ms-flex-line-pack: distribute !important;\r\n            align-content: space-around !important\r\n        }\r\n\r\n        .align-content-stretch {\r\n            -ms-flex-line-pack: stretch !important;\r\n            align-content: stretch !important\r\n        }\r\n\r\n        .align-self-auto {\r\n            -ms-flex-item-align: auto !important;\r\n            align-self: auto !important\r\n        }\r\n\r\n        .align-self-start {\r\n            -ms-flex-item-align: start !important;\r\n            align-self: flex-start !important\r\n        }\r\n\r\n        .align-self-end {\r\n            -ms-flex-item-align: end !important;\r\n            align-self: flex-end !important\r\n        }\r\n\r\n        .align-self-center {\r\n            -ms-flex-item-align: center !important;\r\n            align-self: center !important\r\n        }\r\n\r\n        .align-self-baseline {\r\n            -ms-flex-item-align: baseline !important;\r\n            align-self: baseline !important\r\n        }\r\n\r\n        .align-self-stretch {\r\n            -ms-flex-item-align: stretch !important;\r\n            align-self: stretch !important\r\n        }\r\n\r\n        @media (min-width:576px) {\r\n            .flex-sm-row {\r\n                -webkit-box-orient: horizontal !important;\r\n                -webkit-box-direction: normal !important;\r\n                -ms-flex-direction: row !important;\r\n                flex-direction: row !important\r\n            }\r\n\r\n            .flex-sm-column {\r\n                -webkit-box-orient: vertical !important;\r\n                -webkit-box-direction: normal !important;\r\n                -ms-flex-direction: column !important;\r\n                flex-direction: column !important\r\n            }\r\n\r\n            .flex-sm-row-reverse {\r\n                -webkit-box-orient: horizontal !important;\r\n                -webkit-box-direction: reverse !important;\r\n                -ms-flex-direction: row-reverse !important;\r\n                flex-direction: row-reverse !important\r\n            }\r\n\r\n            .flex-sm-column-reverse {\r\n                -webkit-box-orient: vertical !important;\r\n                -webkit-box-direction: reverse !important;\r\n                -ms-flex-direction: column-reverse !important;\r\n                flex-direction: column-reverse !important\r\n            }\r\n\r\n            .flex-sm-wrap {\r\n                -ms-flex-wrap: wrap !important;\r\n                flex-wrap: wrap !important\r\n            }\r\n\r\n            .flex-sm-nowrap {\r\n                -ms-flex-wrap: nowrap !important;\r\n                flex-wrap: nowrap !important\r\n            }\r\n\r\n            .flex-sm-wrap-reverse {\r\n                -ms-flex-wrap: wrap-reverse !important;\r\n                flex-wrap: wrap-reverse !important\r\n            }\r\n\r\n            .justify-content-sm-start {\r\n                -webkit-box-pack: start !important;\r\n                -ms-flex-pack: start !important;\r\n                justify-content: flex-start !important\r\n            }\r\n\r\n            .justify-content-sm-end {\r\n                -webkit-box-pack: end !important;\r\n                -ms-flex-pack: end !important;\r\n                justify-content: flex-end !important\r\n            }\r\n\r\n            .justify-content-sm-center {\r\n                -webkit-box-pack: center !important;\r\n                -ms-flex-pack: center !important;\r\n                justify-content: center !important\r\n            }\r\n\r\n            .justify-content-sm-between {\r\n                -webkit-box-pack: justify !important;\r\n                -ms-flex-pack: justify !important;\r\n                justify-content: space-between !important\r\n            }\r\n\r\n            .justify-content-sm-around {\r\n                -ms-flex-pack: distribute !important;\r\n                justify-content: space-around !important\r\n            }\r\n\r\n            .align-items-sm-start {\r\n                -webkit-box-align: start !important;\r\n                -ms-flex-align: start !important;\r\n                align-items: flex-start !important\r\n            }\r\n\r\n            .align-items-sm-end {\r\n                -webkit-box-align: end !important;\r\n                -ms-flex-align: end !important;\r\n                align-items: flex-end !important\r\n            }\r\n\r\n            .align-items-sm-center {\r\n                -webkit-box-align: center !important;\r\n                -ms-flex-align: center !important;\r\n                align-items: center !important\r\n            }\r\n\r\n            .align-items-sm-baseline {\r\n                -webkit-box-align: baseline !important;\r\n                -ms-flex-align: baseline !important;\r\n                align-items: baseline !important\r\n            }\r\n\r\n            .align-items-sm-stretch {\r\n                -webkit-box-align: stretch !important;\r\n                -ms-flex-align: stretch !important;\r\n                align-items: stretch !important\r\n            }\r\n\r\n            .align-content-sm-start {\r\n                -ms-flex-line-pack: start !important;\r\n                align-content: flex-start !important\r\n            }\r\n\r\n            .align-content-sm-end {\r\n                -ms-flex-line-pack: end !important;\r\n                align-content: flex-end !important\r\n            }\r\n\r\n            .align-content-sm-center {\r\n                -ms-flex-line-pack: center !important;\r\n                align-content: center !important\r\n            }\r\n\r\n            .align-content-sm-between {\r\n                -ms-flex-line-pack: justify !important;\r\n                align-content: space-between !important\r\n            }\r\n\r\n            .align-content-sm-around {\r\n                -ms-flex-line-pack: distribute !important;\r\n                align-content: space-around !important\r\n            }\r\n\r\n            .align-content-sm-stretch {\r\n                -ms-flex-line-pack: stretch !important;\r\n                align-content: stretch !important\r\n            }\r\n\r\n            .align-self-sm-auto {\r\n                -ms-flex-item-align: auto !important;\r\n                align-self: auto !important\r\n            }\r\n\r\n            .align-self-sm-start {\r\n                -ms-flex-item-align: start !important;\r\n                align-self: flex-start !important\r\n            }\r\n\r\n            .align-self-sm-end {\r\n                -ms-flex-item-align: end !important;\r\n                align-self: flex-end !important\r\n            }\r\n\r\n            .align-self-sm-center {\r\n                -ms-flex-item-align: center !important;\r\n                align-self: center !important\r\n            }\r\n\r\n            .align-self-sm-baseline {\r\n                -ms-flex-item-align: baseline !important;\r\n                align-self: baseline !important\r\n            }\r\n\r\n            .align-self-sm-stretch {\r\n                -ms-flex-item-align: stretch !important;\r\n                align-self: stretch !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:768px) {\r\n            .flex-md-row {\r\n                -webkit-box-orient: horizontal !important;\r\n                -webkit-box-direction: normal !important;\r\n                -ms-flex-direction: row !important;\r\n                flex-direction: row !important\r\n            }\r\n\r\n            .flex-md-column {\r\n                -webkit-box-orient: vertical !important;\r\n                -webkit-box-direction: normal !important;\r\n                -ms-flex-direction: column !important;\r\n                flex-direction: column !important\r\n            }\r\n\r\n            .flex-md-row-reverse {\r\n                -webkit-box-orient: horizontal !important;\r\n                -webkit-box-direction: reverse !important;\r\n                -ms-flex-direction: row-reverse !important;\r\n                flex-direction: row-reverse !important\r\n            }\r\n\r\n            .flex-md-column-reverse {\r\n                -webkit-box-orient: vertical !important;\r\n                -webkit-box-direction: reverse !important;\r\n                -ms-flex-direction: column-reverse !important;\r\n                flex-direction: column-reverse !important\r\n            }\r\n\r\n            .flex-md-wrap {\r\n                -ms-flex-wrap: wrap !important;\r\n                flex-wrap: wrap !important\r\n            }\r\n\r\n            .flex-md-nowrap {\r\n                -ms-flex-wrap: nowrap !important;\r\n                flex-wrap: nowrap !important\r\n            }\r\n\r\n            .flex-md-wrap-reverse {\r\n                -ms-flex-wrap: wrap-reverse !important;\r\n                flex-wrap: wrap-reverse !important\r\n            }\r\n\r\n            .justify-content-md-start {\r\n                -webkit-box-pack: start !important;\r\n                -ms-flex-pack: start !important;\r\n                justify-content: flex-start !important\r\n            }\r\n\r\n            .justify-content-md-end {\r\n                -webkit-box-pack: end !important;\r\n                -ms-flex-pack: end !important;\r\n                justify-content: flex-end !important\r\n            }\r\n\r\n            .justify-content-md-center {\r\n                -webkit-box-pack: center !important;\r\n                -ms-flex-pack: center !important;\r\n                justify-content: center !important\r\n            }\r\n\r\n            .justify-content-md-between {\r\n                -webkit-box-pack: justify !important;\r\n                -ms-flex-pack: justify !important;\r\n                justify-content: space-between !important\r\n            }\r\n\r\n            .justify-content-md-around {\r\n                -ms-flex-pack: distribute !important;\r\n                justify-content: space-around !important\r\n            }\r\n\r\n            .align-items-md-start {\r\n                -webkit-box-align: start !important;\r\n                -ms-flex-align: start !important;\r\n                align-items: flex-start !important\r\n            }\r\n\r\n            .align-items-md-end {\r\n                -webkit-box-align: end !important;\r\n                -ms-flex-align: end !important;\r\n                align-items: flex-end !important\r\n            }\r\n\r\n            .align-items-md-center {\r\n                -webkit-box-align: center !important;\r\n                -ms-flex-align: center !important;\r\n                align-items: center !important\r\n            }\r\n\r\n            .align-items-md-baseline {\r\n                -webkit-box-align: baseline !important;\r\n                -ms-flex-align: baseline !important;\r\n                align-items: baseline !important\r\n            }\r\n\r\n            .align-items-md-stretch {\r\n                -webkit-box-align: stretch !important;\r\n                -ms-flex-align: stretch !important;\r\n                align-items: stretch !important\r\n            }\r\n\r\n            .align-content-md-start {\r\n                -ms-flex-line-pack: start !important;\r\n                align-content: flex-start !important\r\n            }\r\n\r\n            .align-content-md-end {\r\n                -ms-flex-line-pack: end !important;\r\n                align-content: flex-end !important\r\n            }\r\n\r\n            .align-content-md-center {\r\n                -ms-flex-line-pack: center !important;\r\n                align-content: center !important\r\n            }\r\n\r\n            .align-content-md-between {\r\n                -ms-flex-line-pack: justify !important;\r\n                align-content: space-between !important\r\n            }\r\n\r\n            .align-content-md-around {\r\n                -ms-flex-line-pack: distribute !important;\r\n                align-content: space-around !important\r\n            }\r\n\r\n            .align-content-md-stretch {\r\n                -ms-flex-line-pack: stretch !important;\r\n                align-content: stretch !important\r\n            }\r\n\r\n            .align-self-md-auto {\r\n                -ms-flex-item-align: auto !important;\r\n                align-self: auto !important\r\n            }\r\n\r\n            .align-self-md-start {\r\n                -ms-flex-item-align: start !important;\r\n                align-self: flex-start !important\r\n            }\r\n\r\n            .align-self-md-end {\r\n                -ms-flex-item-align: end !important;\r\n                align-self: flex-end !important\r\n            }\r\n\r\n            .align-self-md-center {\r\n                -ms-flex-item-align: center !important;\r\n                align-self: center !important\r\n            }\r\n\r\n            .align-self-md-baseline {\r\n                -ms-flex-item-align: baseline !important;\r\n                align-self: baseline !important\r\n            }\r\n\r\n            .align-self-md-stretch {\r\n                -ms-flex-item-align: stretch !important;\r\n                align-self: stretch !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:992px) {\r\n            .flex-lg-row {\r\n                -webkit-box-orient: horizontal !important;\r\n                -webkit-box-direction: normal !important;\r\n                -ms-flex-direction: row !important;\r\n                flex-direction: row !important\r\n            }\r\n\r\n            .flex-lg-column {\r\n                -webkit-box-orient: vertical !important;\r\n                -webkit-box-direction: normal !important;\r\n                -ms-flex-direction: column !important;\r\n                flex-direction: column !important\r\n            }\r\n\r\n            .flex-lg-row-reverse {\r\n                -webkit-box-orient: horizontal !important;\r\n                -webkit-box-direction: reverse !important;\r\n                -ms-flex-direction: row-reverse !important;\r\n                flex-direction: row-reverse !important\r\n            }\r\n\r\n            .flex-lg-column-reverse {\r\n                -webkit-box-orient: vertical !important;\r\n                -webkit-box-direction: reverse !important;\r\n                -ms-flex-direction: column-reverse !important;\r\n                flex-direction: column-reverse !important\r\n            }\r\n\r\n            .flex-lg-wrap {\r\n                -ms-flex-wrap: wrap !important;\r\n                flex-wrap: wrap !important\r\n            }\r\n\r\n            .flex-lg-nowrap {\r\n                -ms-flex-wrap: nowrap !important;\r\n                flex-wrap: nowrap !important\r\n            }\r\n\r\n            .flex-lg-wrap-reverse {\r\n                -ms-flex-wrap: wrap-reverse !important;\r\n                flex-wrap: wrap-reverse !important\r\n            }\r\n\r\n            .justify-content-lg-start {\r\n                -webkit-box-pack: start !important;\r\n                -ms-flex-pack: start !important;\r\n                justify-content: flex-start !important\r\n            }\r\n\r\n            .justify-content-lg-end {\r\n                -webkit-box-pack: end !important;\r\n                -ms-flex-pack: end !important;\r\n                justify-content: flex-end !important\r\n            }\r\n\r\n            .justify-content-lg-center {\r\n                -webkit-box-pack: center !important;\r\n                -ms-flex-pack: center !important;\r\n                justify-content: center !important\r\n            }\r\n\r\n            .justify-content-lg-between {\r\n                -webkit-box-pack: justify !important;\r\n                -ms-flex-pack: justify !important;\r\n                justify-content: space-between !important\r\n            }\r\n\r\n            .justify-content-lg-around {\r\n                -ms-flex-pack: distribute !important;\r\n                justify-content: space-around !important\r\n            }\r\n\r\n            .align-items-lg-start {\r\n                -webkit-box-align: start !important;\r\n                -ms-flex-align: start !important;\r\n                align-items: flex-start !important\r\n            }\r\n\r\n            .align-items-lg-end {\r\n                -webkit-box-align: end !important;\r\n                -ms-flex-align: end !important;\r\n                align-items: flex-end !important\r\n            }\r\n\r\n            .align-items-lg-center {\r\n                -webkit-box-align: center !important;\r\n                -ms-flex-align: center !important;\r\n                align-items: center !important\r\n            }\r\n\r\n            .align-items-lg-baseline {\r\n                -webkit-box-align: baseline !important;\r\n                -ms-flex-align: baseline !important;\r\n                align-items: baseline !important\r\n            }\r\n\r\n            .align-items-lg-stretch {\r\n                -webkit-box-align: stretch !important;\r\n                -ms-flex-align: stretch !important;\r\n                align-items: stretch !important\r\n            }\r\n\r\n            .align-content-lg-start {\r\n                -ms-flex-line-pack: start !important;\r\n                align-content: flex-start !important\r\n            }\r\n\r\n            .align-content-lg-end {\r\n                -ms-flex-line-pack: end !important;\r\n                align-content: flex-end !important\r\n            }\r\n\r\n            .align-content-lg-center {\r\n                -ms-flex-line-pack: center !important;\r\n                align-content: center !important\r\n            }\r\n\r\n            .align-content-lg-between {\r\n                -ms-flex-line-pack: justify !important;\r\n                align-content: space-between !important\r\n            }\r\n\r\n            .align-content-lg-around {\r\n                -ms-flex-line-pack: distribute !important;\r\n                align-content: space-around !important\r\n            }\r\n\r\n            .align-content-lg-stretch {\r\n                -ms-flex-line-pack: stretch !important;\r\n                align-content: stretch !important\r\n            }\r\n\r\n            .align-self-lg-auto {\r\n                -ms-flex-item-align: auto !important;\r\n                align-self: auto !important\r\n            }\r\n\r\n            .align-self-lg-start {\r\n                -ms-flex-item-align: start !important;\r\n                align-self: flex-start !important\r\n            }\r\n\r\n            .align-self-lg-end {\r\n                -ms-flex-item-align: end !important;\r\n                align-self: flex-end !important\r\n            }\r\n\r\n            .align-self-lg-center {\r\n                -ms-flex-item-align: center !important;\r\n                align-self: center !important\r\n            }\r\n\r\n            .align-self-lg-baseline {\r\n                -ms-flex-item-align: baseline !important;\r\n                align-self: baseline !important\r\n            }\r\n\r\n            .align-self-lg-stretch {\r\n                -ms-flex-item-align: stretch !important;\r\n                align-self: stretch !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:1200px) {\r\n            .flex-xl-row {\r\n                -webkit-box-orient: horizontal !important;\r\n                -webkit-box-direction: normal !important;\r\n                -ms-flex-direction: row !important;\r\n                flex-direction: row !important\r\n            }\r\n\r\n            .flex-xl-column {\r\n                -webkit-box-orient: vertical !important;\r\n                -webkit-box-direction: normal !important;\r\n                -ms-flex-direction: column !important;\r\n                flex-direction: column !important\r\n            }\r\n\r\n            .flex-xl-row-reverse {\r\n                -webkit-box-orient: horizontal !important;\r\n                -webkit-box-direction: reverse !important;\r\n                -ms-flex-direction: row-reverse !important;\r\n                flex-direction: row-reverse !important\r\n            }\r\n\r\n            .flex-xl-column-reverse {\r\n                -webkit-box-orient: vertical !important;\r\n                -webkit-box-direction: reverse !important;\r\n                -ms-flex-direction: column-reverse !important;\r\n                flex-direction: column-reverse !important\r\n            }\r\n\r\n            .flex-xl-wrap {\r\n                -ms-flex-wrap: wrap !important;\r\n                flex-wrap: wrap !important\r\n            }\r\n\r\n            .flex-xl-nowrap {\r\n                -ms-flex-wrap: nowrap !important;\r\n                flex-wrap: nowrap !important\r\n            }\r\n\r\n            .flex-xl-wrap-reverse {\r\n                -ms-flex-wrap: wrap-reverse !important;\r\n                flex-wrap: wrap-reverse !important\r\n            }\r\n\r\n            .justify-content-xl-start {\r\n                -webkit-box-pack: start !important;\r\n                -ms-flex-pack: start !important;\r\n                justify-content: flex-start !important\r\n            }\r\n\r\n            .justify-content-xl-end {\r\n                -webkit-box-pack: end !important;\r\n                -ms-flex-pack: end !important;\r\n                justify-content: flex-end !important\r\n            }\r\n\r\n            .justify-content-xl-center {\r\n                -webkit-box-pack: center !important;\r\n                -ms-flex-pack: center !important;\r\n                justify-content: center !important\r\n            }\r\n\r\n            .justify-content-xl-between {\r\n                -webkit-box-pack: justify !important;\r\n                -ms-flex-pack: justify !important;\r\n                justify-content: space-between !important\r\n            }\r\n\r\n            .justify-content-xl-around {\r\n                -ms-flex-pack: distribute !important;\r\n                justify-content: space-around !important\r\n            }\r\n\r\n            .align-items-xl-start {\r\n                -webkit-box-align: start !important;\r\n                -ms-flex-align: start !important;\r\n                align-items: flex-start !important\r\n            }\r\n\r\n            .align-items-xl-end {\r\n                -webkit-box-align: end !important;\r\n                -ms-flex-align: end !important;\r\n                align-items: flex-end !important\r\n            }\r\n\r\n            .align-items-xl-center {\r\n                -webkit-box-align: center !important;\r\n                -ms-flex-align: center !important;\r\n                align-items: center !important\r\n            }\r\n\r\n            .align-items-xl-baseline {\r\n                -webkit-box-align: baseline !important;\r\n                -ms-flex-align: baseline !important;\r\n                align-items: baseline !important\r\n            }\r\n\r\n            .align-items-xl-stretch {\r\n                -webkit-box-align: stretch !important;\r\n                -ms-flex-align: stretch !important;\r\n                align-items: stretch !important\r\n            }\r\n\r\n            .align-content-xl-start {\r\n                -ms-flex-line-pack: start !important;\r\n                align-content: flex-start !important\r\n            }\r\n\r\n            .align-content-xl-end {\r\n                -ms-flex-line-pack: end !important;\r\n                align-content: flex-end !important\r\n            }\r\n\r\n            .align-content-xl-center {\r\n                -ms-flex-line-pack: center !important;\r\n                align-content: center !important\r\n            }\r\n\r\n            .align-content-xl-between {\r\n                -ms-flex-line-pack: justify !important;\r\n                align-content: space-between !important\r\n            }\r\n\r\n            .align-content-xl-around {\r\n                -ms-flex-line-pack: distribute !important;\r\n                align-content: space-around !important\r\n            }\r\n\r\n            .align-content-xl-stretch {\r\n                -ms-flex-line-pack: stretch !important;\r\n                align-content: stretch !important\r\n            }\r\n\r\n            .align-self-xl-auto {\r\n                -ms-flex-item-align: auto !important;\r\n                align-self: auto !important\r\n            }\r\n\r\n            .align-self-xl-start {\r\n                -ms-flex-item-align: start !important;\r\n                align-self: flex-start !important\r\n            }\r\n\r\n            .align-self-xl-end {\r\n                -ms-flex-item-align: end !important;\r\n                align-self: flex-end !important\r\n            }\r\n\r\n            .align-self-xl-center {\r\n                -ms-flex-item-align: center !important;\r\n                align-self: center !important\r\n            }\r\n\r\n            .align-self-xl-baseline {\r\n                -ms-flex-item-align: baseline !important;\r\n                align-self: baseline !important\r\n            }\r\n\r\n            .align-self-xl-stretch {\r\n                -ms-flex-item-align: stretch !important;\r\n                align-self: stretch !important\r\n            }\r\n        }\r\n\r\n        .float-left {\r\n            float: left !important\r\n        }\r\n\r\n        .float-right {\r\n            float: right !important\r\n        }\r\n\r\n        .float-none {\r\n            float: none !important\r\n        }\r\n\r\n        @media (min-width:576px) {\r\n            .float-sm-left {\r\n                float: left !important\r\n            }\r\n\r\n            .float-sm-right {\r\n                float: right !important\r\n            }\r\n\r\n            .float-sm-none {\r\n                float: none !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:768px) {\r\n            .float-md-left {\r\n                float: left !important\r\n            }\r\n\r\n            .float-md-right {\r\n                float: right !important\r\n            }\r\n\r\n            .float-md-none {\r\n                float: none !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:992px) {\r\n            .float-lg-left {\r\n                float: left !important\r\n            }\r\n\r\n            .float-lg-right {\r\n                float: right !important\r\n            }\r\n\r\n            .float-lg-none {\r\n                float: none !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:1200px) {\r\n            .float-xl-left {\r\n                float: left !important\r\n            }\r\n\r\n            .float-xl-right {\r\n                float: right !important\r\n            }\r\n\r\n            .float-xl-none {\r\n                float: none !important\r\n            }\r\n        }\r\n\r\n        .position-static {\r\n            position: static !important\r\n        }\r\n\r\n        .position-relative {\r\n            position: relative !important\r\n        }\r\n\r\n        .position-absolute {\r\n            position: absolute !important\r\n        }\r\n\r\n        .position-fixed {\r\n            position: fixed !important\r\n        }\r\n\r\n        .position-sticky {\r\n            position: -webkit-sticky !important;\r\n            position: sticky !important\r\n        }\r\n\r\n        .fixed-top {\r\n            position: fixed;\r\n            top: 0;\r\n            right: 0;\r\n            left: 0;\r\n            z-index: 1030\r\n        }\r\n\r\n        .fixed-bottom {\r\n            position: fixed;\r\n            right: 0;\r\n            bottom: 0;\r\n            left: 0;\r\n            z-index: 1030\r\n        }\r\n\r\n        @supports ((position:-webkit-sticky) or (position:sticky)) {\r\n            .sticky-top {\r\n                position: -webkit-sticky;\r\n                position: sticky;\r\n                top: 0;\r\n                z-index: 1020\r\n            }\r\n        }\r\n\r\n        .sr-only {\r\n            position: absolute;\r\n            width: 1px;\r\n            height: 1px;\r\n            padding: 0;\r\n            overflow: hidden;\r\n            clip: rect(0,0,0,0);\r\n            white-space: nowrap;\r\n            -webkit-clip-path: inset(50%);\r\n            clip-path: inset(50%);\r\n            border: 0\r\n        }\r\n\r\n        .sr-only-focusable:active, .sr-only-focusable:focus {\r\n            position: static;\r\n            width: auto;\r\n            height: auto;\r\n            overflow: visible;\r\n            clip: auto;\r\n            white-space: normal;\r\n            -webkit-clip-path: none;\r\n            clip-path: none\r\n        }\r\n\r\n        .w-25 {\r\n            width: 25% !important\r\n        }\r\n\r\n        .w-50 {\r\n            width: 50% !important\r\n        }\r\n\r\n        .w-75 {\r\n            width: 75% !important\r\n        }\r\n\r\n        .w-100 {\r\n            width: 100% !important\r\n        }\r\n\r\n        .h-25 {\r\n            height: 25% !important\r\n        }\r\n\r\n        .h-50 {\r\n            height: 50% !important\r\n        }\r\n\r\n        .h-75 {\r\n            height: 75% !important\r\n        }\r\n\r\n        .h-100 {\r\n            height: 100% !important\r\n        }\r\n\r\n        .mw-100 {\r\n            max-width: 100% !important\r\n        }\r\n\r\n        .mh-100 {\r\n            max-height: 100% !important\r\n        }\r\n\r\n        .m-0 {\r\n            margin: 0 !important\r\n        }\r\n\r\n        .mt-0, .my-0 {\r\n            margin-top: 0 !important\r\n        }\r\n\r\n        .mr-0, .mx-0 {\r\n            margin-right: 0 !important\r\n        }\r\n\r\n        .mb-0, .my-0 {\r\n            margin-bottom: 0 !important\r\n        }\r\n\r\n        .ml-0, .mx-0 {\r\n            margin-left: 0 !important\r\n        }\r\n\r\n        .m-1 {\r\n            margin: .25rem !important\r\n        }\r\n\r\n        .mt-1, .my-1 {\r\n            margin-top: .25rem !important\r\n        }\r\n\r\n        .mr-1, .mx-1 {\r\n            margin-right: .25rem !important\r\n        }\r\n\r\n        .mb-1, .my-1 {\r\n            margin-bottom: .25rem !important\r\n        }\r\n\r\n        .ml-1, .mx-1 {\r\n            margin-left: .25rem !important\r\n        }\r\n\r\n        .m-2 {\r\n            margin: .5rem !important\r\n        }\r\n\r\n        .mt-2, .my-2 {\r\n            margin-top: .5rem !important\r\n        }\r\n\r\n        .mr-2, .mx-2 {\r\n            margin-right: .5rem !important\r\n        }\r\n\r\n        .mb-2, .my-2 {\r\n            margin-bottom: .5rem !important\r\n        }\r\n\r\n        .ml-2, .mx-2 {\r\n            margin-left: .5rem !important\r\n        }\r\n\r\n        .m-3 {\r\n            margin: 1rem !important\r\n        }\r\n\r\n        .mt-3, .my-3 {\r\n            margin-top: 1rem !important\r\n        }\r\n\r\n        .mr-3, .mx-3 {\r\n            margin-right: 1rem !important\r\n        }\r\n\r\n        .mb-3, .my-3 {\r\n            margin-bottom: 1rem !important\r\n        }\r\n\r\n        .ml-3, .mx-3 {\r\n            margin-left: 1rem !important\r\n        }\r\n\r\n        .m-4 {\r\n            margin: 1.5rem !important\r\n        }\r\n\r\n        .mt-4, .my-4 {\r\n            margin-top: 1.5rem !important\r\n        }\r\n\r\n        .mr-4, .mx-4 {\r\n            margin-right: 1.5rem !important\r\n        }\r\n\r\n        .mb-4, .my-4 {\r\n            margin-bottom: 1.5rem !important\r\n        }\r\n\r\n        .ml-4, .mx-4 {\r\n            margin-left: 1.5rem !important\r\n        }\r\n\r\n        .m-5 {\r\n            margin: 3rem !important\r\n        }\r\n\r\n        .mt-5, .my-5 {\r\n            margin-top: 3rem !important\r\n        }\r\n\r\n        .mr-5, .mx-5 {\r\n            margin-right: 3rem !important\r\n        }\r\n\r\n        .mb-5, .my-5 {\r\n            margin-bottom: 3rem !important\r\n        }\r\n\r\n        .ml-5, .mx-5 {\r\n            margin-left: 3rem !important\r\n        }\r\n\r\n        .p-0 {\r\n            padding: 0 !important\r\n        }\r\n\r\n        .pt-0, .py-0 {\r\n            padding-top: 0 !important\r\n        }\r\n\r\n        .pr-0, .px-0 {\r\n            padding-right: 0 !important\r\n        }\r\n\r\n        .pb-0, .py-0 {\r\n            padding-bottom: 0 !important\r\n        }\r\n\r\n        .pl-0, .px-0 {\r\n            padding-left: 0 !important\r\n        }\r\n\r\n        .p-1 {\r\n            padding: .25rem !important\r\n        }\r\n\r\n        .pt-1, .py-1 {\r\n            padding-top: .25rem !important\r\n        }\r\n\r\n        .pr-1, .px-1 {\r\n            padding-right: .25rem !important\r\n        }\r\n\r\n        .pb-1, .py-1 {\r\n            padding-bottom: .25rem !important\r\n        }\r\n\r\n        .pl-1, .px-1 {\r\n            padding-left: .25rem !important\r\n        }\r\n\r\n        .p-2 {\r\n            padding: .5rem !important\r\n        }\r\n\r\n        .pt-2, .py-2 {\r\n            padding-top: .5rem !important\r\n        }\r\n\r\n        .pr-2, .px-2 {\r\n            padding-right: .5rem !important\r\n        }\r\n\r\n        .pb-2, .py-2 {\r\n            padding-bottom: .5rem !important\r\n        }\r\n\r\n        .pl-2, .px-2 {\r\n            padding-left: .5rem !important\r\n        }\r\n\r\n        .p-3 {\r\n            padding: 1rem !important\r\n        }\r\n\r\n        .pt-3, .py-3 {\r\n            padding-top: 1rem !important\r\n        }\r\n\r\n        .pr-3, .px-3 {\r\n            padding-right: 1rem !important\r\n        }\r\n\r\n        .pb-3, .py-3 {\r\n            padding-bottom: 1rem !important\r\n        }\r\n\r\n        .pl-3, .px-3 {\r\n            padding-left: 1rem !important\r\n        }\r\n\r\n        .p-4 {\r\n            padding: 1.5rem !important\r\n        }\r\n\r\n        .pt-4, .py-4 {\r\n            padding-top: 1.5rem !important\r\n        }\r\n\r\n        .pr-4, .px-4 {\r\n            padding-right: 1.5rem !important\r\n        }\r\n\r\n        .pb-4, .py-4 {\r\n            padding-bottom: 1.5rem !important\r\n        }\r\n\r\n        .pl-4, .px-4 {\r\n            padding-left: 1.5rem !important\r\n        }\r\n\r\n        .p-5 {\r\n            padding: 3rem !important\r\n        }\r\n\r\n        .pt-5, .py-5 {\r\n            padding-top: 3rem !important\r\n        }\r\n\r\n        .pr-5, .px-5 {\r\n            padding-right: 3rem !important\r\n        }\r\n\r\n        .pb-5, .py-5 {\r\n            padding-bottom: 3rem !important\r\n        }\r\n\r\n        .pl-5, .px-5 {\r\n            padding-left: 3rem !important\r\n        }\r\n\r\n        .m-auto {\r\n            margin: auto !important\r\n        }\r\n\r\n        .mt-auto, .my-auto {\r\n            margin-top: auto !important\r\n        }\r\n\r\n        .mr-auto, .mx-auto {\r\n            margin-right: auto !important\r\n        }\r\n\r\n        .mb-auto, .my-auto {\r\n            margin-bottom: auto !important\r\n        }\r\n\r\n        .ml-auto, .mx-auto {\r\n            margin-left: auto !important\r\n        }\r\n\r\n        @media (min-width:576px) {\r\n            .m-sm-0 {\r\n                margin: 0 !important\r\n            }\r\n\r\n            .mt-sm-0, .my-sm-0 {\r\n                margin-top: 0 !important\r\n            }\r\n\r\n            .mr-sm-0, .mx-sm-0 {\r\n                margin-right: 0 !important\r\n            }\r\n\r\n            .mb-sm-0, .my-sm-0 {\r\n                margin-bottom: 0 !important\r\n            }\r\n\r\n            .ml-sm-0, .mx-sm-0 {\r\n                margin-left: 0 !important\r\n            }\r\n\r\n            .m-sm-1 {\r\n                margin: .25rem !important\r\n            }\r\n\r\n            .mt-sm-1, .my-sm-1 {\r\n                margin-top: .25rem !important\r\n            }\r\n\r\n            .mr-sm-1, .mx-sm-1 {\r\n                margin-right: .25rem !important\r\n            }\r\n\r\n            .mb-sm-1, .my-sm-1 {\r\n                margin-bottom: .25rem !important\r\n            }\r\n\r\n            .ml-sm-1, .mx-sm-1 {\r\n                margin-left: .25rem !important\r\n            }\r\n\r\n            .m-sm-2 {\r\n                margin: .5rem !important\r\n            }\r\n\r\n            .mt-sm-2, .my-sm-2 {\r\n                margin-top: .5rem !important\r\n            }\r\n\r\n            .mr-sm-2, .mx-sm-2 {\r\n                margin-right: .5rem !important\r\n            }\r\n\r\n            .mb-sm-2, .my-sm-2 {\r\n                margin-bottom: .5rem !important\r\n            }\r\n\r\n            .ml-sm-2, .mx-sm-2 {\r\n                margin-left: .5rem !important\r\n            }\r\n\r\n            .m-sm-3 {\r\n                margin: 1rem !important\r\n            }\r\n\r\n            .mt-sm-3, .my-sm-3 {\r\n                margin-top: 1rem !important\r\n            }\r\n\r\n            .mr-sm-3, .mx-sm-3 {\r\n                margin-right: 1rem !important\r\n            }\r\n\r\n            .mb-sm-3, .my-sm-3 {\r\n                margin-bottom: 1rem !important\r\n            }\r\n\r\n            .ml-sm-3, .mx-sm-3 {\r\n                margin-left: 1rem !important\r\n            }\r\n\r\n            .m-sm-4 {\r\n                margin: 1.5rem !important\r\n            }\r\n\r\n            .mt-sm-4, .my-sm-4 {\r\n                margin-top: 1.5rem !important\r\n            }\r\n\r\n            .mr-sm-4, .mx-sm-4 {\r\n                margin-right: 1.5rem !important\r\n            }\r\n\r\n            .mb-sm-4, .my-sm-4 {\r\n                margin-bottom: 1.5rem !important\r\n            }\r\n\r\n            .ml-sm-4, .mx-sm-4 {\r\n                margin-left: 1.5rem !important\r\n            }\r\n\r\n            .m-sm-5 {\r\n                margin: 3rem !important\r\n            }\r\n\r\n            .mt-sm-5, .my-sm-5 {\r\n                margin-top: 3rem !important\r\n            }\r\n\r\n            .mr-sm-5, .mx-sm-5 {\r\n                margin-right: 3rem !important\r\n            }\r\n\r\n            .mb-sm-5, .my-sm-5 {\r\n                margin-bottom: 3rem !important\r\n            }\r\n\r\n            .ml-sm-5, .mx-sm-5 {\r\n                margin-left: 3rem !important\r\n            }\r\n\r\n            .p-sm-0 {\r\n                padding: 0 !important\r\n            }\r\n\r\n            .pt-sm-0, .py-sm-0 {\r\n                padding-top: 0 !important\r\n            }\r\n\r\n            .pr-sm-0, .px-sm-0 {\r\n                padding-right: 0 !important\r\n            }\r\n\r\n            .pb-sm-0, .py-sm-0 {\r\n                padding-bottom: 0 !important\r\n            }\r\n\r\n            .pl-sm-0, .px-sm-0 {\r\n                padding-left: 0 !important\r\n            }\r\n\r\n            .p-sm-1 {\r\n                padding: .25rem !important\r\n            }\r\n\r\n            .pt-sm-1, .py-sm-1 {\r\n                padding-top: .25rem !important\r\n            }\r\n\r\n            .pr-sm-1, .px-sm-1 {\r\n                padding-right: .25rem !important\r\n            }\r\n\r\n            .pb-sm-1, .py-sm-1 {\r\n                padding-bottom: .25rem !important\r\n            }\r\n\r\n            .pl-sm-1, .px-sm-1 {\r\n                padding-left: .25rem !important\r\n            }\r\n\r\n            .p-sm-2 {\r\n                padding: .5rem !important\r\n            }\r\n\r\n            .pt-sm-2, .py-sm-2 {\r\n                padding-top: .5rem !important\r\n            }\r\n\r\n            .pr-sm-2, .px-sm-2 {\r\n                padding-right: .5rem !important\r\n            }\r\n\r\n            .pb-sm-2, .py-sm-2 {\r\n                padding-bottom: .5rem !important\r\n            }\r\n\r\n            .pl-sm-2, .px-sm-2 {\r\n                padding-left: .5rem !important\r\n            }\r\n\r\n            .p-sm-3 {\r\n                padding: 1rem !important\r\n            }\r\n\r\n            .pt-sm-3, .py-sm-3 {\r\n                padding-top: 1rem !important\r\n            }\r\n\r\n            .pr-sm-3, .px-sm-3 {\r\n                padding-right: 1rem !important\r\n            }\r\n\r\n            .pb-sm-3, .py-sm-3 {\r\n                padding-bottom: 1rem !important\r\n            }\r\n\r\n            .pl-sm-3, .px-sm-3 {\r\n                padding-left: 1rem !important\r\n            }\r\n\r\n            .p-sm-4 {\r\n                padding: 1.5rem !important\r\n            }\r\n\r\n            .pt-sm-4, .py-sm-4 {\r\n                padding-top: 1.5rem !important\r\n            }\r\n\r\n            .pr-sm-4, .px-sm-4 {\r\n                padding-right: 1.5rem !important\r\n            }\r\n\r\n            .pb-sm-4, .py-sm-4 {\r\n                padding-bottom: 1.5rem !important\r\n            }\r\n\r\n            .pl-sm-4, .px-sm-4 {\r\n                padding-left: 1.5rem !important\r\n            }\r\n\r\n            .p-sm-5 {\r\n                padding: 3rem !important\r\n            }\r\n\r\n            .pt-sm-5, .py-sm-5 {\r\n                padding-top: 3rem !important\r\n            }\r\n\r\n            .pr-sm-5, .px-sm-5 {\r\n                padding-right: 3rem !important\r\n            }\r\n\r\n            .pb-sm-5, .py-sm-5 {\r\n                padding-bottom: 3rem !important\r\n            }\r\n\r\n            .pl-sm-5, .px-sm-5 {\r\n                padding-left: 3rem !important\r\n            }\r\n\r\n            .m-sm-auto {\r\n                margin: auto !important\r\n            }\r\n\r\n            .mt-sm-auto, .my-sm-auto {\r\n                margin-top: auto !important\r\n            }\r\n\r\n            .mr-sm-auto, .mx-sm-auto {\r\n                margin-right: auto !important\r\n            }\r\n\r\n            .mb-sm-auto, .my-sm-auto {\r\n                margin-bottom: auto !important\r\n            }\r\n\r\n            .ml-sm-auto, .mx-sm-auto {\r\n                margin-left: auto !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:768px) {\r\n            .m-md-0 {\r\n                margin: 0 !important\r\n            }\r\n\r\n            .mt-md-0, .my-md-0 {\r\n                margin-top: 0 !important\r\n            }\r\n\r\n            .mr-md-0, .mx-md-0 {\r\n                margin-right: 0 !important\r\n            }\r\n\r\n            .mb-md-0, .my-md-0 {\r\n                margin-bottom: 0 !important\r\n            }\r\n\r\n            .ml-md-0, .mx-md-0 {\r\n                margin-left: 0 !important\r\n            }\r\n\r\n            .m-md-1 {\r\n                margin: .25rem !important\r\n            }\r\n\r\n            .mt-md-1, .my-md-1 {\r\n                margin-top: .25rem !important\r\n            }\r\n\r\n            .mr-md-1, .mx-md-1 {\r\n                margin-right: .25rem !important\r\n            }\r\n\r\n            .mb-md-1, .my-md-1 {\r\n                margin-bottom: .25rem !important\r\n            }\r\n\r\n            .ml-md-1, .mx-md-1 {\r\n                margin-left: .25rem !important\r\n            }\r\n\r\n            .m-md-2 {\r\n                margin: .5rem !important\r\n            }\r\n\r\n            .mt-md-2, .my-md-2 {\r\n                margin-top: .5rem !important\r\n            }\r\n\r\n            .mr-md-2, .mx-md-2 {\r\n                margin-right: .5rem !important\r\n            }\r\n\r\n            .mb-md-2, .my-md-2 {\r\n                margin-bottom: .5rem !important\r\n            }\r\n\r\n            .ml-md-2, .mx-md-2 {\r\n                margin-left: .5rem !important\r\n            }\r\n\r\n            .m-md-3 {\r\n                margin: 1rem !important\r\n            }\r\n\r\n            .mt-md-3, .my-md-3 {\r\n                margin-top: 1rem !important\r\n            }\r\n\r\n            .mr-md-3, .mx-md-3 {\r\n                margin-right: 1rem !important\r\n            }\r\n\r\n            .mb-md-3, .my-md-3 {\r\n                margin-bottom: 1rem !important\r\n            }\r\n\r\n            .ml-md-3, .mx-md-3 {\r\n                margin-left: 1rem !important\r\n            }\r\n\r\n            .m-md-4 {\r\n                margin: 1.5rem !important\r\n            }\r\n\r\n            .mt-md-4, .my-md-4 {\r\n                margin-top: 1.5rem !important\r\n            }\r\n\r\n            .mr-md-4, .mx-md-4 {\r\n                margin-right: 1.5rem !important\r\n            }\r\n\r\n            .mb-md-4, .my-md-4 {\r\n                margin-bottom: 1.5rem !important\r\n            }\r\n\r\n            .ml-md-4, .mx-md-4 {\r\n                margin-left: 1.5rem !important\r\n            }\r\n\r\n            .m-md-5 {\r\n                margin: 3rem !important\r\n            }\r\n\r\n            .mt-md-5, .my-md-5 {\r\n                margin-top: 3rem !important\r\n            }\r\n\r\n            .mr-md-5, .mx-md-5 {\r\n                margin-right: 3rem !important\r\n            }\r\n\r\n            .mb-md-5, .my-md-5 {\r\n                margin-bottom: 3rem !important\r\n            }\r\n\r\n            .ml-md-5, .mx-md-5 {\r\n                margin-left: 3rem !important\r\n            }\r\n\r\n            .p-md-0 {\r\n                padding: 0 !important\r\n            }\r\n\r\n            .pt-md-0, .py-md-0 {\r\n                padding-top: 0 !important\r\n            }\r\n\r\n            .pr-md-0, .px-md-0 {\r\n                padding-right: 0 !important\r\n            }\r\n\r\n            .pb-md-0, .py-md-0 {\r\n                padding-bottom: 0 !important\r\n            }\r\n\r\n            .pl-md-0, .px-md-0 {\r\n                padding-left: 0 !important\r\n            }\r\n\r\n            .p-md-1 {\r\n                padding: .25rem !important\r\n            }\r\n\r\n            .pt-md-1, .py-md-1 {\r\n                padding-top: .25rem !important\r\n            }\r\n\r\n            .pr-md-1, .px-md-1 {\r\n                padding-right: .25rem !important\r\n            }\r\n\r\n            .pb-md-1, .py-md-1 {\r\n                padding-bottom: .25rem !important\r\n            }\r\n\r\n            .pl-md-1, .px-md-1 {\r\n                padding-left: .25rem !important\r\n            }\r\n\r\n            .p-md-2 {\r\n                padding: .5rem !important\r\n            }\r\n\r\n            .pt-md-2, .py-md-2 {\r\n                padding-top: .5rem !important\r\n            }\r\n\r\n            .pr-md-2, .px-md-2 {\r\n                padding-right: .5rem !important\r\n            }\r\n\r\n            .pb-md-2, .py-md-2 {\r\n                padding-bottom: .5rem !important\r\n            }\r\n\r\n            .pl-md-2, .px-md-2 {\r\n                padding-left: .5rem !important\r\n            }\r\n\r\n            .p-md-3 {\r\n                padding: 1rem !important\r\n            }\r\n\r\n            .pt-md-3, .py-md-3 {\r\n                padding-top: 1rem !important\r\n            }\r\n\r\n            .pr-md-3, .px-md-3 {\r\n                padding-right: 1rem !important\r\n            }\r\n\r\n            .pb-md-3, .py-md-3 {\r\n                padding-bottom: 1rem !important\r\n            }\r\n\r\n            .pl-md-3, .px-md-3 {\r\n                padding-left: 1rem !important\r\n            }\r\n\r\n            .p-md-4 {\r\n                padding: 1.5rem !important\r\n            }\r\n\r\n            .pt-md-4, .py-md-4 {\r\n                padding-top: 1.5rem !important\r\n            }\r\n\r\n            .pr-md-4, .px-md-4 {\r\n                padding-right: 1.5rem !important\r\n            }\r\n\r\n            .pb-md-4, .py-md-4 {\r\n                padding-bottom: 1.5rem !important\r\n            }\r\n\r\n            .pl-md-4, .px-md-4 {\r\n                padding-left: 1.5rem !important\r\n            }\r\n\r\n            .p-md-5 {\r\n                padding: 3rem !important\r\n            }\r\n\r\n            .pt-md-5, .py-md-5 {\r\n                padding-top: 3rem !important\r\n            }\r\n\r\n            .pr-md-5, .px-md-5 {\r\n                padding-right: 3rem !important\r\n            }\r\n\r\n            .pb-md-5, .py-md-5 {\r\n                padding-bottom: 3rem !important\r\n            }\r\n\r\n            .pl-md-5, .px-md-5 {\r\n                padding-left: 3rem !important\r\n            }\r\n\r\n            .m-md-auto {\r\n                margin: auto !important\r\n            }\r\n\r\n            .mt-md-auto, .my-md-auto {\r\n                margin-top: auto !important\r\n            }\r\n\r\n            .mr-md-auto, .mx-md-auto {\r\n                margin-right: auto !important\r\n            }\r\n\r\n            .mb-md-auto, .my-md-auto {\r\n                margin-bottom: auto !important\r\n            }\r\n\r\n            .ml-md-auto, .mx-md-auto {\r\n                margin-left: auto !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:992px) {\r\n            .m-lg-0 {\r\n                margin: 0 !important\r\n            }\r\n\r\n            .mt-lg-0, .my-lg-0 {\r\n                margin-top: 0 !important\r\n            }\r\n\r\n            .mr-lg-0, .mx-lg-0 {\r\n                margin-right: 0 !important\r\n            }\r\n\r\n            .mb-lg-0, .my-lg-0 {\r\n                margin-bottom: 0 !important\r\n            }\r\n\r\n            .ml-lg-0, .mx-lg-0 {\r\n                margin-left: 0 !important\r\n            }\r\n\r\n            .m-lg-1 {\r\n                margin: .25rem !important\r\n            }\r\n\r\n            .mt-lg-1, .my-lg-1 {\r\n                margin-top: .25rem !important\r\n            }\r\n\r\n            .mr-lg-1, .mx-lg-1 {\r\n                margin-right: .25rem !important\r\n            }\r\n\r\n            .mb-lg-1, .my-lg-1 {\r\n                margin-bottom: .25rem !important\r\n            }\r\n\r\n            .ml-lg-1, .mx-lg-1 {\r\n                margin-left: .25rem !important\r\n            }\r\n\r\n            .m-lg-2 {\r\n                margin: .5rem !important\r\n            }\r\n\r\n            .mt-lg-2, .my-lg-2 {\r\n                margin-top: .5rem !important\r\n            }\r\n\r\n            .mr-lg-2, .mx-lg-2 {\r\n                margin-right: .5rem !important\r\n            }\r\n\r\n            .mb-lg-2, .my-lg-2 {\r\n                margin-bottom: .5rem !important\r\n            }\r\n\r\n            .ml-lg-2, .mx-lg-2 {\r\n                margin-left: .5rem !important\r\n            }\r\n\r\n            .m-lg-3 {\r\n                margin: 1rem !important\r\n            }\r\n\r\n            .mt-lg-3, .my-lg-3 {\r\n                margin-top: 1rem !important\r\n            }\r\n\r\n            .mr-lg-3, .mx-lg-3 {\r\n                margin-right: 1rem !important\r\n            }\r\n\r\n            .mb-lg-3, .my-lg-3 {\r\n                margin-bottom: 1rem !important\r\n            }\r\n\r\n            .ml-lg-3, .mx-lg-3 {\r\n                margin-left: 1rem !important\r\n            }\r\n\r\n            .m-lg-4 {\r\n                margin: 1.5rem !important\r\n            }\r\n\r\n            .mt-lg-4, .my-lg-4 {\r\n                margin-top: 1.5rem !important\r\n            }\r\n\r\n            .mr-lg-4, .mx-lg-4 {\r\n                margin-right: 1.5rem !important\r\n            }\r\n\r\n            .mb-lg-4, .my-lg-4 {\r\n                margin-bottom: 1.5rem !important\r\n            }\r\n\r\n            .ml-lg-4, .mx-lg-4 {\r\n                margin-left: 1.5rem !important\r\n            }\r\n\r\n            .m-lg-5 {\r\n                margin: 3rem !important\r\n            }\r\n\r\n            .mt-lg-5, .my-lg-5 {\r\n                margin-top: 3rem !important\r\n            }\r\n\r\n            .mr-lg-5, .mx-lg-5 {\r\n                margin-right: 3rem !important\r\n            }\r\n\r\n            .mb-lg-5, .my-lg-5 {\r\n                margin-bottom: 3rem !important\r\n            }\r\n\r\n            .ml-lg-5, .mx-lg-5 {\r\n                margin-left: 3rem !important\r\n            }\r\n\r\n            .p-lg-0 {\r\n                padding: 0 !important\r\n            }\r\n\r\n            .pt-lg-0, .py-lg-0 {\r\n                padding-top: 0 !important\r\n            }\r\n\r\n            .pr-lg-0, .px-lg-0 {\r\n                padding-right: 0 !important\r\n            }\r\n\r\n            .pb-lg-0, .py-lg-0 {\r\n                padding-bottom: 0 !important\r\n            }\r\n\r\n            .pl-lg-0, .px-lg-0 {\r\n                padding-left: 0 !important\r\n            }\r\n\r\n            .p-lg-1 {\r\n                padding: .25rem !important\r\n            }\r\n\r\n            .pt-lg-1, .py-lg-1 {\r\n                padding-top: .25rem !important\r\n            }\r\n\r\n            .pr-lg-1, .px-lg-1 {\r\n                padding-right: .25rem !important\r\n            }\r\n\r\n            .pb-lg-1, .py-lg-1 {\r\n                padding-bottom: .25rem !important\r\n            }\r\n\r\n            .pl-lg-1, .px-lg-1 {\r\n                padding-left: .25rem !important\r\n            }\r\n\r\n            .p-lg-2 {\r\n                padding: .5rem !important\r\n            }\r\n\r\n            .pt-lg-2, .py-lg-2 {\r\n                padding-top: .5rem !important\r\n            }\r\n\r\n            .pr-lg-2, .px-lg-2 {\r\n                padding-right: .5rem !important\r\n            }\r\n\r\n            .pb-lg-2, .py-lg-2 {\r\n                padding-bottom: .5rem !important\r\n            }\r\n\r\n            .pl-lg-2, .px-lg-2 {\r\n                padding-left: .5rem !important\r\n            }\r\n\r\n            .p-lg-3 {\r\n                padding: 1rem !important\r\n            }\r\n\r\n            .pt-lg-3, .py-lg-3 {\r\n                padding-top: 1rem !important\r\n            }\r\n\r\n            .pr-lg-3, .px-lg-3 {\r\n                padding-right: 1rem !important\r\n            }\r\n\r\n            .pb-lg-3, .py-lg-3 {\r\n                padding-bottom: 1rem !important\r\n            }\r\n\r\n            .pl-lg-3, .px-lg-3 {\r\n                padding-left: 1rem !important\r\n            }\r\n\r\n            .p-lg-4 {\r\n                padding: 1.5rem !important\r\n            }\r\n\r\n            .pt-lg-4, .py-lg-4 {\r\n                padding-top: 1.5rem !important\r\n            }\r\n\r\n            .pr-lg-4, .px-lg-4 {\r\n                padding-right: 1.5rem !important\r\n            }\r\n\r\n            .pb-lg-4, .py-lg-4 {\r\n                padding-bottom: 1.5rem !important\r\n            }\r\n\r\n            .pl-lg-4, .px-lg-4 {\r\n                padding-left: 1.5rem !important\r\n            }\r\n\r\n            .p-lg-5 {\r\n                padding: 3rem !important\r\n            }\r\n\r\n            .pt-lg-5, .py-lg-5 {\r\n                padding-top: 3rem !important\r\n            }\r\n\r\n            .pr-lg-5, .px-lg-5 {\r\n                padding-right: 3rem !important\r\n            }\r\n\r\n            .pb-lg-5, .py-lg-5 {\r\n                padding-bottom: 3rem !important\r\n            }\r\n\r\n            .pl-lg-5, .px-lg-5 {\r\n                padding-left: 3rem !important\r\n            }\r\n\r\n            .m-lg-auto {\r\n                margin: auto !important\r\n            }\r\n\r\n            .mt-lg-auto, .my-lg-auto {\r\n                margin-top: auto !important\r\n            }\r\n\r\n            .mr-lg-auto, .mx-lg-auto {\r\n                margin-right: auto !important\r\n            }\r\n\r\n            .mb-lg-auto, .my-lg-auto {\r\n                margin-bottom: auto !important\r\n            }\r\n\r\n            .ml-lg-auto, .mx-lg-auto {\r\n                margin-left: auto !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:1200px) {\r\n            .m-xl-0 {\r\n                margin: 0 !important\r\n            }\r\n\r\n            .mt-xl-0, .my-xl-0 {\r\n                margin-top: 0 !important\r\n            }\r\n\r\n            .mr-xl-0, .mx-xl-0 {\r\n                margin-right: 0 !important\r\n            }\r\n\r\n            .mb-xl-0, .my-xl-0 {\r\n                margin-bottom: 0 !important\r\n            }\r\n\r\n            .ml-xl-0, .mx-xl-0 {\r\n                margin-left: 0 !important\r\n            }\r\n\r\n            .m-xl-1 {\r\n                margin: .25rem !important\r\n            }\r\n\r\n            .mt-xl-1, .my-xl-1 {\r\n                margin-top: .25rem !important\r\n            }\r\n\r\n            .mr-xl-1, .mx-xl-1 {\r\n                margin-right: .25rem !important\r\n            }\r\n\r\n            .mb-xl-1, .my-xl-1 {\r\n                margin-bottom: .25rem !important\r\n            }\r\n\r\n            .ml-xl-1, .mx-xl-1 {\r\n                margin-left: .25rem !important\r\n            }\r\n\r\n            .m-xl-2 {\r\n                margin: .5rem !important\r\n            }\r\n\r\n            .mt-xl-2, .my-xl-2 {\r\n                margin-top: .5rem !important\r\n            }\r\n\r\n            .mr-xl-2, .mx-xl-2 {\r\n                margin-right: .5rem !important\r\n            }\r\n\r\n            .mb-xl-2, .my-xl-2 {\r\n                margin-bottom: .5rem !important\r\n            }\r\n\r\n            .ml-xl-2, .mx-xl-2 {\r\n                margin-left: .5rem !important\r\n            }\r\n\r\n            .m-xl-3 {\r\n                margin: 1rem !important\r\n            }\r\n\r\n            .mt-xl-3, .my-xl-3 {\r\n                margin-top: 1rem !important\r\n            }\r\n\r\n            .mr-xl-3, .mx-xl-3 {\r\n                margin-right: 1rem !important\r\n            }\r\n\r\n            .mb-xl-3, .my-xl-3 {\r\n                margin-bottom: 1rem !important\r\n            }\r\n\r\n            .ml-xl-3, .mx-xl-3 {\r\n                margin-left: 1rem !important\r\n            }\r\n\r\n            .m-xl-4 {\r\n                margin: 1.5rem !important\r\n            }\r\n\r\n            .mt-xl-4, .my-xl-4 {\r\n                margin-top: 1.5rem !important\r\n            }\r\n\r\n            .mr-xl-4, .mx-xl-4 {\r\n                margin-right: 1.5rem !important\r\n            }\r\n\r\n            .mb-xl-4, .my-xl-4 {\r\n                margin-bottom: 1.5rem !important\r\n            }\r\n\r\n            .ml-xl-4, .mx-xl-4 {\r\n                margin-left: 1.5rem !important\r\n            }\r\n\r\n            .m-xl-5 {\r\n                margin: 3rem !important\r\n            }\r\n\r\n            .mt-xl-5, .my-xl-5 {\r\n                margin-top: 3rem !important\r\n            }\r\n\r\n            .mr-xl-5, .mx-xl-5 {\r\n                margin-right: 3rem !important\r\n            }\r\n\r\n            .mb-xl-5, .my-xl-5 {\r\n                margin-bottom: 3rem !important\r\n            }\r\n\r\n            .ml-xl-5, .mx-xl-5 {\r\n                margin-left: 3rem !important\r\n            }\r\n\r\n            .p-xl-0 {\r\n                padding: 0 !important\r\n            }\r\n\r\n            .pt-xl-0, .py-xl-0 {\r\n                padding-top: 0 !important\r\n            }\r\n\r\n            .pr-xl-0, .px-xl-0 {\r\n                padding-right: 0 !important\r\n            }\r\n\r\n            .pb-xl-0, .py-xl-0 {\r\n                padding-bottom: 0 !important\r\n            }\r\n\r\n            .pl-xl-0, .px-xl-0 {\r\n                padding-left: 0 !important\r\n            }\r\n\r\n            .p-xl-1 {\r\n                padding: .25rem !important\r\n            }\r\n\r\n            .pt-xl-1, .py-xl-1 {\r\n                padding-top: .25rem !important\r\n            }\r\n\r\n            .pr-xl-1, .px-xl-1 {\r\n                padding-right: .25rem !important\r\n            }\r\n\r\n            .pb-xl-1, .py-xl-1 {\r\n                padding-bottom: .25rem !important\r\n            }\r\n\r\n            .pl-xl-1, .px-xl-1 {\r\n                padding-left: .25rem !important\r\n            }\r\n\r\n            .p-xl-2 {\r\n                padding: .5rem !important\r\n            }\r\n\r\n            .pt-xl-2, .py-xl-2 {\r\n                padding-top: .5rem !important\r\n            }\r\n\r\n            .pr-xl-2, .px-xl-2 {\r\n                padding-right: .5rem !important\r\n            }\r\n\r\n            .pb-xl-2, .py-xl-2 {\r\n                padding-bottom: .5rem !important\r\n            }\r\n\r\n            .pl-xl-2, .px-xl-2 {\r\n                padding-left: .5rem !important\r\n            }\r\n\r\n            .p-xl-3 {\r\n                padding: 1rem !important\r\n            }\r\n\r\n            .pt-xl-3, .py-xl-3 {\r\n                padding-top: 1rem !important\r\n            }\r\n\r\n            .pr-xl-3, .px-xl-3 {\r\n                padding-right: 1rem !important\r\n            }\r\n\r\n            .pb-xl-3, .py-xl-3 {\r\n                padding-bottom: 1rem !important\r\n            }\r\n\r\n            .pl-xl-3, .px-xl-3 {\r\n                padding-left: 1rem !important\r\n            }\r\n\r\n            .p-xl-4 {\r\n                padding: 1.5rem !important\r\n            }\r\n\r\n            .pt-xl-4, .py-xl-4 {\r\n                padding-top: 1.5rem !important\r\n            }\r\n\r\n            .pr-xl-4, .px-xl-4 {\r\n                padding-right: 1.5rem !important\r\n            }\r\n\r\n            .pb-xl-4, .py-xl-4 {\r\n                padding-bottom: 1.5rem !important\r\n            }\r\n\r\n            .pl-xl-4, .px-xl-4 {\r\n                padding-left: 1.5rem !important\r\n            }\r\n\r\n            .p-xl-5 {\r\n                padding: 3rem !important\r\n            }\r\n\r\n            .pt-xl-5, .py-xl-5 {\r\n                padding-top: 3rem !important\r\n            }\r\n\r\n            .pr-xl-5, .px-xl-5 {\r\n                padding-right: 3rem !important\r\n            }\r\n\r\n            .pb-xl-5, .py-xl-5 {\r\n                padding-bottom: 3rem !important\r\n            }\r\n\r\n            .pl-xl-5, .px-xl-5 {\r\n                padding-left: 3rem !important\r\n            }\r\n\r\n            .m-xl-auto {\r\n                margin: auto !important\r\n            }\r\n\r\n            .mt-xl-auto, .my-xl-auto {\r\n                margin-top: auto !important\r\n            }\r\n\r\n            .mr-xl-auto, .mx-xl-auto {\r\n                margin-right: auto !important\r\n            }\r\n\r\n            .mb-xl-auto, .my-xl-auto {\r\n                margin-bottom: auto !important\r\n            }\r\n\r\n            .ml-xl-auto, .mx-xl-auto {\r\n                margin-left: auto !important\r\n            }\r\n        }\r\n\r\n        .text-justify {\r\n            text-align: justify !important\r\n        }\r\n\r\n        .text-nowrap {\r\n            white-space: nowrap !important\r\n        }\r\n\r\n        .text-truncate {\r\n            overflow: hidden;\r\n            text-overflow: ellipsis;\r\n            white-space: nowrap\r\n        }\r\n\r\n        .text-left {\r\n            text-align: left !important\r\n        }\r\n\r\n        .text-right {\r\n            text-align: right !important\r\n        }\r\n\r\n        .text-center {\r\n            text-align: center !important\r\n        }\r\n\r\n        @media (min-width:576px) {\r\n            .text-sm-left {\r\n                text-align: left !important\r\n            }\r\n\r\n            .text-sm-right {\r\n                text-align: right !important\r\n            }\r\n\r\n            .text-sm-center {\r\n                text-align: center !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:768px) {\r\n            .text-md-left {\r\n                text-align: left !important\r\n            }\r\n\r\n            .text-md-right {\r\n                text-align: right !important\r\n            }\r\n\r\n            .text-md-center {\r\n                text-align: center !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:992px) {\r\n            .text-lg-left {\r\n                text-align: left !important\r\n            }\r\n\r\n            .text-lg-right {\r\n                text-align: right !important\r\n            }\r\n\r\n            .text-lg-center {\r\n                text-align: center !important\r\n            }\r\n        }\r\n\r\n        @media (min-width:1200px) {\r\n            .text-xl-left {\r\n                text-align: left !important\r\n            }\r\n\r\n            .text-xl-right {\r\n                text-align: right !important\r\n            }\r\n\r\n            .text-xl-center {\r\n                text-align: center !important\r\n            }\r\n        }\r\n\r\n        .text-lowercase {\r\n            text-transform: lowercase !important\r\n        }\r\n\r\n        .text-uppercase {\r\n            text-transform: uppercase !important\r\n        }\r\n\r\n        .text-capitalize {\r\n            text-transform: capitalize !important\r\n        }\r\n\r\n        .font-weight-light {\r\n            font-weight: 300 !important\r\n        }\r\n\r\n        .font-weight-normal {\r\n            font-weight: 400 !important\r\n        }\r\n\r\n        .font-weight-bold {\r\n            font-weight: 700 !important\r\n        }\r\n\r\n        .font-italic {\r\n            font-style: italic !important\r\n        }\r\n\r\n        .text-white {\r\n            color: #fff !important\r\n        }\r\n\r\n        .text-primary {\r\n            color: #007bff !important\r\n        }\r\n\r\n        a.text-primary:focus, a.text-primary:hover {\r\n            color: #0062cc !important\r\n        }\r\n\r\n        .text-secondary {\r\n            color: #6c757d !important\r\n        }\r\n\r\n        a.text-secondary:focus, a.text-secondary:hover {\r\n            color: #545b62 !important\r\n        }\r\n\r\n        .text-success {\r\n            color: #28a745 !important\r\n        }\r\n\r\n        a.text-success:focus, a.text-success:hover {\r\n            color: #1e7e34 !important\r\n        }\r\n\r\n        .text-info {\r\n            color: #17a2b8 !important\r\n        }\r\n\r\n        a.text-info:focus, a.text-info:hover {\r\n            color: #117a8b !important\r\n        }\r\n\r\n        .text-warning {\r\n            color: #ffc107 !important\r\n        }\r\n\r\n        a.text-warning:focus, a.text-warning:hover {\r\n            color: #d39e00 !important\r\n        }\r\n\r\n        .text-danger {\r\n            color: #dc3545 !important\r\n        }\r\n\r\n        a.text-danger:focus, a.text-danger:hover {\r\n            color: #bd2130 !important\r\n        }\r\n\r\n        .text-light {\r\n            color: #f8f9fa !important\r\n        }\r\n\r\n        a.text-light:focus, a.text-light:hover {\r\n            color: #dae0e5 !important\r\n        }\r\n\r\n        .text-dark {\r\n            color: #343a40 !important\r\n        }\r\n\r\n        a.text-dark:focus, a.text-dark:hover {\r\n            color: #1d2124 !important\r\n        }\r\n\r\n        .text-muted {\r\n            color: #6c757d !important\r\n        }\r\n\r\n        .text-hide {\r\n            font: 0/0 a;\r\n            color: transparent;\r\n            text-shadow: none;\r\n            background-color: transparent;\r\n            border: 0\r\n        }\r\n\r\n        .visible {\r\n            visibility: visible !important\r\n        }\r\n\r\n        .invisible {\r\n            visibility: hidden !important\r\n        }\r\n\r\n        @media print {\r\n            *, ::after, ::before {\r\n                text-shadow: none !important;\r\n                box-shadow: none !important\r\n            }\r\n\r\n            a:not(.btn) {\r\n                text-decoration: underline\r\n            }\r\n\r\n            abbr[title]::after {\r\n                content: \" (\" attr(title) \")\"\r\n            }\r\n\r\n            pre {\r\n                white-space: pre-wrap !important\r\n            }\r\n\r\n            blockquote, pre {\r\n                border: 1px solid #999;\r\n                page-break-inside: avoid\r\n            }\r\n\r\n            thead {\r\n                display: table-header-group\r\n            }\r\n\r\n            img, tr {\r\n                page-break-inside: avoid\r\n            }\r\n\r\n            h2, h3, p {\r\n                orphans: 3;\r\n                widows: 3\r\n            }\r\n\r\n            h2, h3 {\r\n                page-break-after: avoid\r\n            }\r\n\r\n            @page {\r\n                size: a3\r\n            }\r\n\r\n            body {\r\n                min-width: 992px !important\r\n            }\r\n\r\n            .container {\r\n                min-width: 992px !important\r\n            }\r\n\r\n            .navbar {\r\n                display: none\r\n            }\r\n\r\n            .badge {\r\n                border: 1px solid #000\r\n            }\r\n\r\n            .table {\r\n                border-collapse: collapse !important\r\n            }\r\n\r\n                .table td, .table th {\r\n                    background-color: #fff !important\r\n                }\r\n\r\n            .table-bordered td, .table-bordered th {\r\n                border: 1px solid #ddd !important\r\n            }\r\n        }\r\n        /*# sourceMappingURL=bootstrap.min.css.map */\r\n    </style>\r\n    <script type=\"text/javascript\">\r\n        !function (t, e) { \"object\" == typeof exports && \"undefined\" != typeof module ? e(exports, require(\"jquery\"), require(\"popper.js\")) : \"function\" == typeof define && define.amd ? define([\"exports\", \"jquery\", \"popper.js\"], e) : e(t.bootstrap = {}, t.jQuery, t.Popper) }(this, function (t, e, n) { \"use strict\"; function i(t, e) { for (var n = 0; n < e.length; n++) { var i = e[n]; i.enumerable = i.enumerable || !1, i.configurable = !0, \"value\" in i && (i.writable = !0), Object.defineProperty(t, i.key, i) } } function s(t, e, n) { return e && i(t.prototype, e), n && i(t, n), t } function r() { return (r = Object.assign || function (t) { for (var e = 1; e < arguments.length; e++) { var n = arguments[e]; for (var i in n) Object.prototype.hasOwnProperty.call(n, i) && (t[i] = n[i]) } return t }).apply(this, arguments) } e = e && e.hasOwnProperty(\"default\") ? e.default : e, n = n && n.hasOwnProperty(\"default\") ? n.default : n; var o, a, l, h, c, u, f, d, _, g, p, m, v, E, T, y, C, I, A, b, D, S, w, N, O, k, P = function (t) { var e = !1; function n(e) { var n = this, s = !1; return t(this).one(i.TRANSITION_END, function () { s = !0 }), setTimeout(function () { s || i.triggerTransitionEnd(n) }, e), this } var i = { TRANSITION_END: \"bsTransitionEnd\", getUID: function (t) { do { t += ~~(1e6 * Math.random()) } while (document.getElementById(t)); return t }, getSelectorFromElement: function (e) { var n, i = e.getAttribute(\"data-target\"); i && \"#\" !== i || (i = e.getAttribute(\"href\") || \"\"), \"#\" === i.charAt(0) && (n = i, i = n = \"function\" == typeof t.escapeSelector ? t.escapeSelector(n).substr(1) : n.replace(/(:|\\.|\\[|\\]|,|=|@)/g, \"\\\\$1\")); try { return t(document).find(i).length > 0 ? i : null } catch (t) { return null } }, reflow: function (t) { return t.offsetHeight }, triggerTransitionEnd: function (n) { t(n).trigger(e.end) }, supportsTransitionEnd: function () { return Boolean(e) }, isElement: function (t) { return (t[0] || t).nodeType }, typeCheckConfig: function (t, e, n) { for (var s in n) if (Object.prototype.hasOwnProperty.call(n, s)) { var r = n[s], o = e[s], a = o && i.isElement(o) ? \"element\" : (l = o, {}.toString.call(l).match(/\\s([a-zA-Z]+)/)[1].toLowerCase()); if (!new RegExp(r).test(a)) throw new Error(t.toUpperCase() + ': Option \"' + s + '\" provided type \"' + a + '\" but expected type \"' + r + '\".') } var l } }; return e = (\"undefined\" == typeof window || !window.QUnit) && { end: \"transitionend\" }, t.fn.emulateTransitionEnd = n, i.supportsTransitionEnd() && (t.event.special[i.TRANSITION_END] = { bindType: e.end, delegateType: e.end, handle: function (e) { if (t(e.target).is(this)) return e.handleObj.handler.apply(this, arguments) } }), i }(e), L = (a = \"alert\", h = \".\" + (l = \"bs.alert\"), c = (o = e).fn[a], u = { CLOSE: \"close\" + h, CLOSED: \"closed\" + h, CLICK_DATA_API: \"click\" + h + \".data-api\" }, f = \"alert\", d = \"fade\", _ = \"show\", g = function () { function t(t) { this._element = t } var e = t.prototype; return e.close = function (t) { t = t || this._element; var e = this._getRootElement(t); this._triggerCloseEvent(e).isDefaultPrevented() || this._removeElement(e) }, e.dispose = function () { o.removeData(this._element, l), this._element = null }, e._getRootElement = function (t) { var e = P.getSelectorFromElement(t), n = !1; return e && (n = o(e)[0]), n || (n = o(t).closest(\".\" + f)[0]), n }, e._triggerCloseEvent = function (t) { var e = o.Event(u.CLOSE); return o(t).trigger(e), e }, e._removeElement = function (t) { var e = this; o(t).removeClass(_), P.supportsTransitionEnd() && o(t).hasClass(d) ? o(t).one(P.TRANSITION_END, function (n) { return e._destroyElement(t, n) }).emulateTransitionEnd(150) : this._destroyElement(t) }, e._destroyElement = function (t) { o(t).detach().trigger(u.CLOSED).remove() }, t._jQueryInterface = function (e) { return this.each(function () { var n = o(this), i = n.data(l); i || (i = new t(this), n.data(l, i)), \"close\" === e && i[e](this) }) }, t._handleDismiss = function (t) { return function (e) { e && e.preventDefault(), t.close(this) } }, s(t, null, [{ key: \"VERSION\", get: function () { return \"4.0.0\" } }]), t }(), o(document).on(u.CLICK_DATA_API, '[data-dismiss=\"alert\"]', g._handleDismiss(new g)), o.fn[a] = g._jQueryInterface, o.fn[a].Constructor = g, o.fn[a].noConflict = function () { return o.fn[a] = c, g._jQueryInterface }, g), R = (m = \"button\", E = \".\" + (v = \"bs.button\"), T = \".data-api\", y = (p = e).fn[m], C = \"active\", I = \"btn\", A = \"focus\", b = '[data-toggle^=\"button\"]', D = '[data-toggle=\"buttons\"]', S = \"input\", w = \".active\", N = \".btn\", O = { CLICK_DATA_API: \"click\" + E + T, FOCUS_BLUR_DATA_API: \"focus\" + E + T + \" blur\" + E + T }, k = function () { function t(t) { this._element = t } var e = t.prototype; return e.toggle = function () { var t = !0, e = !0, n = p(this._element).closest(D)[0]; if (n) { var i = p(this._element).find(S)[0]; if (i) { if (\"radio\" === i.type) if (i.checked && p(this._element).hasClass(C)) t = !1; else { var s = p(n).find(w)[0]; s && p(s).removeClass(C) } if (t) { if (i.hasAttribute(\"disabled\") || n.hasAttribute(\"disabled\") || i.classList.contains(\"disabled\") || n.classList.contains(\"disabled\")) return; i.checked = !p(this._element).hasClass(C), p(i).trigger(\"change\") } i.focus(), e = !1 } } e && this._element.setAttribute(\"aria-pressed\", !p(this._element).hasClass(C)), t && p(this._element).toggleClass(C) }, e.dispose = function () { p.removeData(this._element, v), this._element = null }, t._jQueryInterface = function (e) { return this.each(function () { var n = p(this).data(v); n || (n = new t(this), p(this).data(v, n)), \"toggle\" === e && n[e]() }) }, s(t, null, [{ key: \"VERSION\", get: function () { return \"4.0.0\" } }]), t }(), p(document).on(O.CLICK_DATA_API, b, function (t) { t.preventDefault(); var e = t.target; p(e).hasClass(I) || (e = p(e).closest(N)), k._jQueryInterface.call(p(e), \"toggle\") }).on(O.FOCUS_BLUR_DATA_API, b, function (t) { var e = p(t.target).closest(N)[0]; p(e).toggleClass(A, /^focus(in)?$/.test(t.type)) }), p.fn[m] = k._jQueryInterface, p.fn[m].Constructor = k, p.fn[m].noConflict = function () { return p.fn[m] = y, k._jQueryInterface }, k), j = function (t) { var e = \"carousel\", n = \"bs.carousel\", i = \".\" + n, o = t.fn[e], a = { interval: 5e3, keyboard: !0, slide: !1, pause: \"hover\", wrap: !0 }, l = { interval: \"(number|boolean)\", keyboard: \"boolean\", slide: \"(boolean|string)\", pause: \"(string|boolean)\", wrap: \"boolean\" }, h = \"next\", c = \"prev\", u = \"left\", f = \"right\", d = { SLIDE: \"slide\" + i, SLID: \"slid\" + i, KEYDOWN: \"keydown\" + i, MOUSEENTER: \"mouseenter\" + i, MOUSELEAVE: \"mouseleave\" + i, TOUCHEND: \"touchend\" + i, LOAD_DATA_API: \"load\" + i + \".data-api\", CLICK_DATA_API: \"click\" + i + \".data-api\" }, _ = \"carousel\", g = \"active\", p = \"slide\", m = \"carousel-item-right\", v = \"carousel-item-left\", E = \"carousel-item-next\", T = \"carousel-item-prev\", y = { ACTIVE: \".active\", ACTIVE_ITEM: \".active.carousel-item\", ITEM: \".carousel-item\", NEXT_PREV: \".carousel-item-next, .carousel-item-prev\", INDICATORS: \".carousel-indicators\", DATA_SLIDE: \"[data-slide], [data-slide-to]\", DATA_RIDE: '[data-ride=\"carousel\"]' }, C = function () { function o(e, n) { this._items = null, this._interval = null, this._activeElement = null, this._isPaused = !1, this._isSliding = !1, this.touchTimeout = null, this._config = this._getConfig(n), this._element = t(e)[0], this._indicatorsElement = t(this._element).find(y.INDICATORS)[0], this._addEventListeners() } var C = o.prototype; return C.next = function () { this._isSliding || this._slide(h) }, C.nextWhenVisible = function () { !document.hidden && t(this._element).is(\":visible\") && \"hidden\" !== t(this._element).css(\"visibility\") && this.next() }, C.prev = function () { this._isSliding || this._slide(c) }, C.pause = function (e) { e || (this._isPaused = !0), t(this._element).find(y.NEXT_PREV)[0] && P.supportsTransitionEnd() && (P.triggerTransitionEnd(this._element), this.cycle(!0)), clearInterval(this._interval), this._interval = null }, C.cycle = function (t) { t || (this._isPaused = !1), this._interval && (clearInterval(this._interval), this._interval = null), this._config.interval && !this._isPaused && (this._interval = setInterval((document.visibilityState ? this.nextWhenVisible : this.next).bind(this), this._config.interval)) }, C.to = function (e) { var n = this; this._activeElement = t(this._element).find(y.ACTIVE_ITEM)[0]; var i = this._getItemIndex(this._activeElement); if (!(e > this._items.length - 1 || e < 0)) if (this._isSliding) t(this._element).one(d.SLID, function () { return n.to(e) }); else { if (i === e) return this.pause(), void this.cycle(); var s = e > i ? h : c; this._slide(s, this._items[e]) } }, C.dispose = function () { t(this._element).off(i), t.removeData(this._element, n), this._items = null, this._config = null, this._element = null, this._interval = null, this._isPaused = null, this._isSliding = null, this._activeElement = null, this._indicatorsElement = null }, C._getConfig = function (t) { return t = r({}, a, t), P.typeCheckConfig(e, t, l), t }, C._addEventListeners = function () { var e = this; this._config.keyboard && t(this._element).on(d.KEYDOWN, function (t) { return e._keydown(t) }), \"hover\" === this._config.pause && (t(this._element).on(d.MOUSEENTER, function (t) { return e.pause(t) }).on(d.MOUSELEAVE, function (t) { return e.cycle(t) }), \"ontouchstart\" in document.documentElement && t(this._element).on(d.TOUCHEND, function () { e.pause(), e.touchTimeout && clearTimeout(e.touchTimeout), e.touchTimeout = setTimeout(function (t) { return e.cycle(t) }, 500 + e._config.interval) })) }, C._keydown = function (t) { if (!/input|textarea/i.test(t.target.tagName)) switch (t.which) { case 37: t.preventDefault(), this.prev(); break; case 39: t.preventDefault(), this.next() } }, C._getItemIndex = function (e) { return this._items = t.makeArray(t(e).parent().find(y.ITEM)), this._items.indexOf(e) }, C._getItemByDirection = function (t, e) { var n = t === h, i = t === c, s = this._getItemIndex(e), r = this._items.length - 1; if ((i && 0 === s || n && s === r) && !this._config.wrap) return e; var o = (s + (t === c ? -1 : 1)) % this._items.length; return -1 === o ? this._items[this._items.length - 1] : this._items[o] }, C._triggerSlideEvent = function (e, n) { var i = this._getItemIndex(e), s = this._getItemIndex(t(this._element).find(y.ACTIVE_ITEM)[0]), r = t.Event(d.SLIDE, { relatedTarget: e, direction: n, from: s, to: i }); return t(this._element).trigger(r), r }, C._setActiveIndicatorElement = function (e) { if (this._indicatorsElement) { t(this._indicatorsElement).find(y.ACTIVE).removeClass(g); var n = this._indicatorsElement.children[this._getItemIndex(e)]; n && t(n).addClass(g) } }, C._slide = function (e, n) { var i, s, r, o = this, a = t(this._element).find(y.ACTIVE_ITEM)[0], l = this._getItemIndex(a), c = n || a && this._getItemByDirection(e, a), _ = this._getItemIndex(c), C = Boolean(this._interval); if (e === h ? (i = v, s = E, r = u) : (i = m, s = T, r = f), c && t(c).hasClass(g)) this._isSliding = !1; else if (!this._triggerSlideEvent(c, r).isDefaultPrevented() && a && c) { this._isSliding = !0, C && this.pause(), this._setActiveIndicatorElement(c); var I = t.Event(d.SLID, { relatedTarget: c, direction: r, from: l, to: _ }); P.supportsTransitionEnd() && t(this._element).hasClass(p) ? (t(c).addClass(s), P.reflow(c), t(a).addClass(i), t(c).addClass(i), t(a).one(P.TRANSITION_END, function () { t(c).removeClass(i + \" \" + s).addClass(g), t(a).removeClass(g + \" \" + s + \" \" + i), o._isSliding = !1, setTimeout(function () { return t(o._element).trigger(I) }, 0) }).emulateTransitionEnd(600)) : (t(a).removeClass(g), t(c).addClass(g), this._isSliding = !1, t(this._element).trigger(I)), C && this.cycle() } }, o._jQueryInterface = function (e) { return this.each(function () { var i = t(this).data(n), s = r({}, a, t(this).data()); \"object\" == typeof e && (s = r({}, s, e)); var l = \"string\" == typeof e ? e : s.slide; if (i || (i = new o(this, s), t(this).data(n, i)), \"number\" == typeof e) i.to(e); else if (\"string\" == typeof l) { if (\"undefined\" == typeof i[l]) throw new TypeError('No method named \"' + l + '\"'); i[l]() } else s.interval && (i.pause(), i.cycle()) }) }, o._dataApiClickHandler = function (e) { var i = P.getSelectorFromElement(this); if (i) { var s = t(i)[0]; if (s && t(s).hasClass(_)) { var a = r({}, t(s).data(), t(this).data()), l = this.getAttribute(\"data-slide-to\"); l && (a.interval = !1), o._jQueryInterface.call(t(s), a), l && t(s).data(n).to(l), e.preventDefault() } } }, s(o, null, [{ key: \"VERSION\", get: function () { return \"4.0.0\" } }, { key: \"Default\", get: function () { return a } }]), o }(); return t(document).on(d.CLICK_DATA_API, y.DATA_SLIDE, C._dataApiClickHandler), t(window).on(d.LOAD_DATA_API, function () { t(y.DATA_RIDE).each(function () { var e = t(this); C._jQueryInterface.call(e, e.data()) }) }), t.fn[e] = C._jQueryInterface, t.fn[e].Constructor = C, t.fn[e].noConflict = function () { return t.fn[e] = o, C._jQueryInterface }, C }(e), H = function (t) { var e = \"collapse\", n = \"bs.collapse\", i = \".\" + n, o = t.fn[e], a = { toggle: !0, parent: \"\" }, l = { toggle: \"boolean\", parent: \"(string|element)\" }, h = { SHOW: \"show\" + i, SHOWN: \"shown\" + i, HIDE: \"hide\" + i, HIDDEN: \"hidden\" + i, CLICK_DATA_API: \"click\" + i + \".data-api\" }, c = \"show\", u = \"collapse\", f = \"collapsing\", d = \"collapsed\", _ = \"width\", g = \"height\", p = { ACTIVES: \".show, .collapsing\", DATA_TOGGLE: '[data-toggle=\"collapse\"]' }, m = function () { function i(e, n) { this._isTransitioning = !1, this._element = e, this._config = this._getConfig(n), this._triggerArray = t.makeArray(t('[data-toggle=\"collapse\"][href=\"#' + e.id + '\"],[data-toggle=\"collapse\"][data-target=\"#' + e.id + '\"]')); for (var i = t(p.DATA_TOGGLE), s = 0; s < i.length; s++) { var r = i[s], o = P.getSelectorFromElement(r); null !== o && t(o).filter(e).length > 0 && (this._selector = o, this._triggerArray.push(r)) } this._parent = this._config.parent ? this._getParent() : null, this._config.parent || this._addAriaAndCollapsedClass(this._element, this._triggerArray), this._config.toggle && this.toggle() } var o = i.prototype; return o.toggle = function () { t(this._element).hasClass(c) ? this.hide() : this.show() }, o.show = function () { var e, s, r = this; if (!this._isTransitioning && !t(this._element).hasClass(c) && (this._parent && 0 === (e = t.makeArray(t(this._parent).find(p.ACTIVES).filter('[data-parent=\"' + this._config.parent + '\"]'))).length && (e = null), !(e && (s = t(e).not(this._selector).data(n)) && s._isTransitioning))) { var o = t.Event(h.SHOW); if (t(this._element).trigger(o), !o.isDefaultPrevented()) { e && (i._jQueryInterface.call(t(e).not(this._selector), \"hide\"), s || t(e).data(n, null)); var a = this._getDimension(); t(this._element).removeClass(u).addClass(f), this._element.style[a] = 0, this._triggerArray.length > 0 && t(this._triggerArray).removeClass(d).attr(\"aria-expanded\", !0), this.setTransitioning(!0); var l = function () { t(r._element).removeClass(f).addClass(u).addClass(c), r._element.style[a] = \"\", r.setTransitioning(!1), t(r._element).trigger(h.SHOWN) }; if (P.supportsTransitionEnd()) { var _ = \"scroll\" + (a[0].toUpperCase() + a.slice(1)); t(this._element).one(P.TRANSITION_END, l).emulateTransitionEnd(600), this._element.style[a] = this._element[_] + \"px\" } else l() } } }, o.hide = function () { var e = this; if (!this._isTransitioning && t(this._element).hasClass(c)) { var n = t.Event(h.HIDE); if (t(this._element).trigger(n), !n.isDefaultPrevented()) { var i = this._getDimension(); if (this._element.style[i] = this._element.getBoundingClientRect()[i] + \"px\", P.reflow(this._element), t(this._element).addClass(f).removeClass(u).removeClass(c), this._triggerArray.length > 0) for (var s = 0; s < this._triggerArray.length; s++) { var r = this._triggerArray[s], o = P.getSelectorFromElement(r); if (null !== o) t(o).hasClass(c) || t(r).addClass(d).attr(\"aria-expanded\", !1) } this.setTransitioning(!0); var a = function () { e.setTransitioning(!1), t(e._element).removeClass(f).addClass(u).trigger(h.HIDDEN) }; this._element.style[i] = \"\", P.supportsTransitionEnd() ? t(this._element).one(P.TRANSITION_END, a).emulateTransitionEnd(600) : a() } } }, o.setTransitioning = function (t) { this._isTransitioning = t }, o.dispose = function () { t.removeData(this._element, n), this._config = null, this._parent = null, this._element = null, this._triggerArray = null, this._isTransitioning = null }, o._getConfig = function (t) { return (t = r({}, a, t)).toggle = Boolean(t.toggle), P.typeCheckConfig(e, t, l), t }, o._getDimension = function () { return t(this._element).hasClass(_) ? _ : g }, o._getParent = function () { var e = this, n = null; P.isElement(this._config.parent) ? (n = this._config.parent, \"undefined\" != typeof this._config.parent.jquery && (n = this._config.parent[0])) : n = t(this._config.parent)[0]; var s = '[data-toggle=\"collapse\"][data-parent=\"' + this._config.parent + '\"]'; return t(n).find(s).each(function (t, n) { e._addAriaAndCollapsedClass(i._getTargetFromElement(n), [n]) }), n }, o._addAriaAndCollapsedClass = function (e, n) { if (e) { var i = t(e).hasClass(c); n.length > 0 && t(n).toggleClass(d, !i).attr(\"aria-expanded\", i) } }, i._getTargetFromElement = function (e) { var n = P.getSelectorFromElement(e); return n ? t(n)[0] : null }, i._jQueryInterface = function (e) { return this.each(function () { var s = t(this), o = s.data(n), l = r({}, a, s.data(), \"object\" == typeof e && e); if (!o && l.toggle && /show|hide/.test(e) && (l.toggle = !1), o || (o = new i(this, l), s.data(n, o)), \"string\" == typeof e) { if (\"undefined\" == typeof o[e]) throw new TypeError('No method named \"' + e + '\"'); o[e]() } }) }, s(i, null, [{ key: \"VERSION\", get: function () { return \"4.0.0\" } }, { key: \"Default\", get: function () { return a } }]), i }(); return t(document).on(h.CLICK_DATA_API, p.DATA_TOGGLE, function (e) { \"A\" === e.currentTarget.tagName && e.preventDefault(); var i = t(this), s = P.getSelectorFromElement(this); t(s).each(function () { var e = t(this), s = e.data(n) ? \"toggle\" : i.data(); m._jQueryInterface.call(e, s) }) }), t.fn[e] = m._jQueryInterface, t.fn[e].Constructor = m, t.fn[e].noConflict = function () { return t.fn[e] = o, m._jQueryInterface }, m }(e), W = function (t) { var e = \"dropdown\", i = \"bs.dropdown\", o = \".\" + i, a = \".data-api\", l = t.fn[e], h = new RegExp(\"38|40|27\"), c = { HIDE: \"hide\" + o, HIDDEN: \"hidden\" + o, SHOW: \"show\" + o, SHOWN: \"shown\" + o, CLICK: \"click\" + o, CLICK_DATA_API: \"click\" + o + a, KEYDOWN_DATA_API: \"keydown\" + o + a, KEYUP_DATA_API: \"keyup\" + o + a }, u = \"disabled\", f = \"show\", d = \"dropup\", _ = \"dropright\", g = \"dropleft\", p = \"dropdown-menu-right\", m = \"dropdown-menu-left\", v = \"position-static\", E = '[data-toggle=\"dropdown\"]', T = \".dropdown form\", y = \".dropdown-menu\", C = \".navbar-nav\", I = \".dropdown-menu .dropdown-item:not(.disabled)\", A = \"top-start\", b = \"top-end\", D = \"bottom-start\", S = \"bottom-end\", w = \"right-start\", N = \"left-start\", O = { offset: 0, flip: !0, boundary: \"scrollParent\" }, k = { offset: \"(number|string|function)\", flip: \"boolean\", boundary: \"(string|element)\" }, L = function () { function a(t, e) { this._element = t, this._popper = null, this._config = this._getConfig(e), this._menu = this._getMenuElement(), this._inNavbar = this._detectNavbar(), this._addEventListeners() } var l = a.prototype; return l.toggle = function () { if (!this._element.disabled && !t(this._element).hasClass(u)) { var e = a._getParentFromElement(this._element), i = t(this._menu).hasClass(f); if (a._clearMenus(), !i) { var s = { relatedTarget: this._element }, r = t.Event(c.SHOW, s); if (t(e).trigger(r), !r.isDefaultPrevented()) { if (!this._inNavbar) { if (\"undefined\" == typeof n) throw new TypeError(\"Bootstrap dropdown require Popper.js (https://popper.js.org)\"); var o = this._element; t(e).hasClass(d) && (t(this._menu).hasClass(m) || t(this._menu).hasClass(p)) && (o = e), \"scrollParent\" !== this._config.boundary && t(e).addClass(v), this._popper = new n(o, this._menu, this._getPopperConfig()) } \"ontouchstart\" in document.documentElement && 0 === t(e).closest(C).length && t(\"body\").children().on(\"mouseover\", null, t.noop), this._element.focus(), this._element.setAttribute(\"aria-expanded\", !0), t(this._menu).toggleClass(f), t(e).toggleClass(f).trigger(t.Event(c.SHOWN, s)) } } } }, l.dispose = function () { t.removeData(this._element, i), t(this._element).off(o), this._element = null, this._menu = null, null !== this._popper && (this._popper.destroy(), this._popper = null) }, l.update = function () { this._inNavbar = this._detectNavbar(), null !== this._popper && this._popper.scheduleUpdate() }, l._addEventListeners = function () { var e = this; t(this._element).on(c.CLICK, function (t) { t.preventDefault(), t.stopPropagation(), e.toggle() }) }, l._getConfig = function (n) { return n = r({}, this.constructor.Default, t(this._element).data(), n), P.typeCheckConfig(e, n, this.constructor.DefaultType), n }, l._getMenuElement = function () { if (!this._menu) { var e = a._getParentFromElement(this._element); this._menu = t(e).find(y)[0] } return this._menu }, l._getPlacement = function () { var e = t(this._element).parent(), n = D; return e.hasClass(d) ? (n = A, t(this._menu).hasClass(p) && (n = b)) : e.hasClass(_) ? n = w : e.hasClass(g) ? n = N : t(this._menu).hasClass(p) && (n = S), n }, l._detectNavbar = function () { return t(this._element).closest(\".navbar\").length > 0 }, l._getPopperConfig = function () { var t = this, e = {}; return \"function\" == typeof this._config.offset ? e.fn = function (e) { return e.offsets = r({}, e.offsets, t._config.offset(e.offsets) || {}), e } : e.offset = this._config.offset, { placement: this._getPlacement(), modifiers: { offset: e, flip: { enabled: this._config.flip }, preventOverflow: { boundariesElement: this._config.boundary } } } }, a._jQueryInterface = function (e) { return this.each(function () { var n = t(this).data(i); if (n || (n = new a(this, \"object\" == typeof e ? e : null), t(this).data(i, n)), \"string\" == typeof e) { if (\"undefined\" == typeof n[e]) throw new TypeError('No method named \"' + e + '\"'); n[e]() } }) }, a._clearMenus = function (e) { if (!e || 3 !== e.which && (\"keyup\" !== e.type || 9 === e.which)) for (var n = t.makeArray(t(E)), s = 0; s < n.length; s++) { var r = a._getParentFromElement(n[s]), o = t(n[s]).data(i), l = { relatedTarget: n[s] }; if (o) { var h = o._menu; if (t(r).hasClass(f) && !(e && (\"click\" === e.type && /input|textarea/i.test(e.target.tagName) || \"keyup\" === e.type && 9 === e.which) && t.contains(r, e.target))) { var u = t.Event(c.HIDE, l); t(r).trigger(u), u.isDefaultPrevented() || (\"ontouchstart\" in document.documentElement && t(\"body\").children().off(\"mouseover\", null, t.noop), n[s].setAttribute(\"aria-expanded\", \"false\"), t(h).removeClass(f), t(r).removeClass(f).trigger(t.Event(c.HIDDEN, l))) } } } }, a._getParentFromElement = function (e) { var n, i = P.getSelectorFromElement(e); return i && (n = t(i)[0]), n || e.parentNode }, a._dataApiKeydownHandler = function (e) { if ((/input|textarea/i.test(e.target.tagName) ? !(32 === e.which || 27 !== e.which && (40 !== e.which && 38 !== e.which || t(e.target).closest(y).length)) : h.test(e.which)) && (e.preventDefault(), e.stopPropagation(), !this.disabled && !t(this).hasClass(u))) { var n = a._getParentFromElement(this), i = t(n).hasClass(f); if ((i || 27 === e.which && 32 === e.which) && (!i || 27 !== e.which && 32 !== e.which)) { var s = t(n).find(I).get(); if (0 !== s.length) { var r = s.indexOf(e.target); 38 === e.which && r > 0 && r-- , 40 === e.which && r < s.length - 1 && r++ , r < 0 && (r = 0), s[r].focus() } } else { if (27 === e.which) { var o = t(n).find(E)[0]; t(o).trigger(\"focus\") } t(this).trigger(\"click\") } } }, s(a, null, [{ key: \"VERSION\", get: function () { return \"4.0.0\" } }, { key: \"Default\", get: function () { return O } }, { key: \"DefaultType\", get: function () { return k } }]), a }(); return t(document).on(c.KEYDOWN_DATA_API, E, L._dataApiKeydownHandler).on(c.KEYDOWN_DATA_API, y, L._dataApiKeydownHandler).on(c.CLICK_DATA_API + \" \" + c.KEYUP_DATA_API, L._clearMenus).on(c.CLICK_DATA_API, E, function (e) { e.preventDefault(), e.stopPropagation(), L._jQueryInterface.call(t(this), \"toggle\") }).on(c.CLICK_DATA_API, T, function (t) { t.stopPropagation() }), t.fn[e] = L._jQueryInterface, t.fn[e].Constructor = L, t.fn[e].noConflict = function () { return t.fn[e] = l, L._jQueryInterface }, L }(e), M = function (t) { var e = \"modal\", n = \"bs.modal\", i = \".\" + n, o = t.fn.modal, a = { backdrop: !0, keyboard: !0, focus: !0, show: !0 }, l = { backdrop: \"(boolean|string)\", keyboard: \"boolean\", focus: \"boolean\", show: \"boolean\" }, h = { HIDE: \"hide\" + i, HIDDEN: \"hidden\" + i, SHOW: \"show\" + i, SHOWN: \"shown\" + i, FOCUSIN: \"focusin\" + i, RESIZE: \"resize\" + i, CLICK_DISMISS: \"click.dismiss\" + i, KEYDOWN_DISMISS: \"keydown.dismiss\" + i, MOUSEUP_DISMISS: \"mouseup.dismiss\" + i, MOUSEDOWN_DISMISS: \"mousedown.dismiss\" + i, CLICK_DATA_API: \"click\" + i + \".data-api\" }, c = \"modal-scrollbar-measure\", u = \"modal-backdrop\", f = \"modal-open\", d = \"fade\", _ = \"show\", g = { DIALOG: \".modal-dialog\", DATA_TOGGLE: '[data-toggle=\"modal\"]', DATA_DISMISS: '[data-dismiss=\"modal\"]', FIXED_CONTENT: \".fixed-top, .fixed-bottom, .is-fixed, .sticky-top\", STICKY_CONTENT: \".sticky-top\", NAVBAR_TOGGLER: \".navbar-toggler\" }, p = function () { function o(e, n) { this._config = this._getConfig(n), this._element = e, this._dialog = t(e).find(g.DIALOG)[0], this._backdrop = null, this._isShown = !1, this._isBodyOverflowing = !1, this._ignoreBackdropClick = !1, this._originalBodyPadding = 0, this._scrollbarWidth = 0 } var p = o.prototype; return p.toggle = function (t) { return this._isShown ? this.hide() : this.show(t) }, p.show = function (e) { var n = this; if (!this._isTransitioning && !this._isShown) { P.supportsTransitionEnd() && t(this._element).hasClass(d) && (this._isTransitioning = !0); var i = t.Event(h.SHOW, { relatedTarget: e }); t(this._element).trigger(i), this._isShown || i.isDefaultPrevented() || (this._isShown = !0, this._checkScrollbar(), this._setScrollbar(), this._adjustDialog(), t(document.body).addClass(f), this._setEscapeEvent(), this._setResizeEvent(), t(this._element).on(h.CLICK_DISMISS, g.DATA_DISMISS, function (t) { return n.hide(t) }), t(this._dialog).on(h.MOUSEDOWN_DISMISS, function () { t(n._element).one(h.MOUSEUP_DISMISS, function (e) { t(e.target).is(n._element) && (n._ignoreBackdropClick = !0) }) }), this._showBackdrop(function () { return n._showElement(e) })) } }, p.hide = function (e) { var n = this; if (e && e.preventDefault(), !this._isTransitioning && this._isShown) { var i = t.Event(h.HIDE); if (t(this._element).trigger(i), this._isShown && !i.isDefaultPrevented()) { this._isShown = !1; var s = P.supportsTransitionEnd() && t(this._element).hasClass(d); s && (this._isTransitioning = !0), this._setEscapeEvent(), this._setResizeEvent(), t(document).off(h.FOCUSIN), t(this._element).removeClass(_), t(this._element).off(h.CLICK_DISMISS), t(this._dialog).off(h.MOUSEDOWN_DISMISS), s ? t(this._element).one(P.TRANSITION_END, function (t) { return n._hideModal(t) }).emulateTransitionEnd(300) : this._hideModal() } } }, p.dispose = function () { t.removeData(this._element, n), t(window, document, this._element, this._backdrop).off(i), this._config = null, this._element = null, this._dialog = null, this._backdrop = null, this._isShown = null, this._isBodyOverflowing = null, this._ignoreBackdropClick = null, this._scrollbarWidth = null }, p.handleUpdate = function () { this._adjustDialog() }, p._getConfig = function (t) { return t = r({}, a, t), P.typeCheckConfig(e, t, l), t }, p._showElement = function (e) { var n = this, i = P.supportsTransitionEnd() && t(this._element).hasClass(d); this._element.parentNode && this._element.parentNode.nodeType === Node.ELEMENT_NODE || document.body.appendChild(this._element), this._element.style.display = \"block\", this._element.removeAttribute(\"aria-hidden\"), this._element.scrollTop = 0, i && P.reflow(this._element), t(this._element).addClass(_), this._config.focus && this._enforceFocus(); var s = t.Event(h.SHOWN, { relatedTarget: e }), r = function () { n._config.focus && n._element.focus(), n._isTransitioning = !1, t(n._element).trigger(s) }; i ? t(this._dialog).one(P.TRANSITION_END, r).emulateTransitionEnd(300) : r() }, p._enforceFocus = function () { var e = this; t(document).off(h.FOCUSIN).on(h.FOCUSIN, function (n) { document !== n.target && e._element !== n.target && 0 === t(e._element).has(n.target).length && e._element.focus() }) }, p._setEscapeEvent = function () { var e = this; this._isShown && this._config.keyboard ? t(this._element).on(h.KEYDOWN_DISMISS, function (t) { 27 === t.which && (t.preventDefault(), e.hide()) }) : this._isShown || t(this._element).off(h.KEYDOWN_DISMISS) }, p._setResizeEvent = function () { var e = this; this._isShown ? t(window).on(h.RESIZE, function (t) { return e.handleUpdate(t) }) : t(window).off(h.RESIZE) }, p._hideModal = function () { var e = this; this._element.style.display = \"none\", this._element.setAttribute(\"aria-hidden\", !0), this._isTransitioning = !1, this._showBackdrop(function () { t(document.body).removeClass(f), e._resetAdjustments(), e._resetScrollbar(), t(e._element).trigger(h.HIDDEN) }) }, p._removeBackdrop = function () { this._backdrop && (t(this._backdrop).remove(), this._backdrop = null) }, p._showBackdrop = function (e) { var n = this, i = t(this._element).hasClass(d) ? d : \"\"; if (this._isShown && this._config.backdrop) { var s = P.supportsTransitionEnd() && i; if (this._backdrop = document.createElement(\"div\"), this._backdrop.className = u, i && t(this._backdrop).addClass(i), t(this._backdrop).appendTo(document.body), t(this._element).on(h.CLICK_DISMISS, function (t) { n._ignoreBackdropClick ? n._ignoreBackdropClick = !1 : t.target === t.currentTarget && (\"static\" === n._config.backdrop ? n._element.focus() : n.hide()) }), s && P.reflow(this._backdrop), t(this._backdrop).addClass(_), !e) return; if (!s) return void e(); t(this._backdrop).one(P.TRANSITION_END, e).emulateTransitionEnd(150) } else if (!this._isShown && this._backdrop) { t(this._backdrop).removeClass(_); var r = function () { n._removeBackdrop(), e && e() }; P.supportsTransitionEnd() && t(this._element).hasClass(d) ? t(this._backdrop).one(P.TRANSITION_END, r).emulateTransitionEnd(150) : r() } else e && e() }, p._adjustDialog = function () { var t = this._element.scrollHeight > document.documentElement.clientHeight; !this._isBodyOverflowing && t && (this._element.style.paddingLeft = this._scrollbarWidth + \"px\"), this._isBodyOverflowing && !t && (this._element.style.paddingRight = this._scrollbarWidth + \"px\") }, p._resetAdjustments = function () { this._element.style.paddingLeft = \"\", this._element.style.paddingRight = \"\" }, p._checkScrollbar = function () { var t = document.body.getBoundingClientRect(); this._isBodyOverflowing = t.left + t.right < window.innerWidth, this._scrollbarWidth = this._getScrollbarWidth() }, p._setScrollbar = function () { var e = this; if (this._isBodyOverflowing) { t(g.FIXED_CONTENT).each(function (n, i) { var s = t(i)[0].style.paddingRight, r = t(i).css(\"padding-right\"); t(i).data(\"padding-right\", s).css(\"padding-right\", parseFloat(r) + e._scrollbarWidth + \"px\") }), t(g.STICKY_CONTENT).each(function (n, i) { var s = t(i)[0].style.marginRight, r = t(i).css(\"margin-right\"); t(i).data(\"margin-right\", s).css(\"margin-right\", parseFloat(r) - e._scrollbarWidth + \"px\") }), t(g.NAVBAR_TOGGLER).each(function (n, i) { var s = t(i)[0].style.marginRight, r = t(i).css(\"margin-right\"); t(i).data(\"margin-right\", s).css(\"margin-right\", parseFloat(r) + e._scrollbarWidth + \"px\") }); var n = document.body.style.paddingRight, i = t(\"body\").css(\"padding-right\"); t(\"body\").data(\"padding-right\", n).css(\"padding-right\", parseFloat(i) + this._scrollbarWidth + \"px\") } }, p._resetScrollbar = function () { t(g.FIXED_CONTENT).each(function (e, n) { var i = t(n).data(\"padding-right\"); \"undefined\" != typeof i && t(n).css(\"padding-right\", i).removeData(\"padding-right\") }), t(g.STICKY_CONTENT + \", \" + g.NAVBAR_TOGGLER).each(function (e, n) { var i = t(n).data(\"margin-right\"); \"undefined\" != typeof i && t(n).css(\"margin-right\", i).removeData(\"margin-right\") }); var e = t(\"body\").data(\"padding-right\"); \"undefined\" != typeof e && t(\"body\").css(\"padding-right\", e).removeData(\"padding-right\") }, p._getScrollbarWidth = function () { var t = document.createElement(\"div\"); t.className = c, document.body.appendChild(t); var e = t.getBoundingClientRect().width - t.clientWidth; return document.body.removeChild(t), e }, o._jQueryInterface = function (e, i) { return this.each(function () { var s = t(this).data(n), a = r({}, o.Default, t(this).data(), \"object\" == typeof e && e); if (s || (s = new o(this, a), t(this).data(n, s)), \"string\" == typeof e) { if (\"undefined\" == typeof s[e]) throw new TypeError('No method named \"' + e + '\"'); s[e](i) } else a.show && s.show(i) }) }, s(o, null, [{ key: \"VERSION\", get: function () { return \"4.0.0\" } }, { key: \"Default\", get: function () { return a } }]), o }(); return t(document).on(h.CLICK_DATA_API, g.DATA_TOGGLE, function (e) { var i, s = this, o = P.getSelectorFromElement(this); o && (i = t(o)[0]); var a = t(i).data(n) ? \"toggle\" : r({}, t(i).data(), t(this).data()); \"A\" !== this.tagName && \"AREA\" !== this.tagName || e.preventDefault(); var l = t(i).one(h.SHOW, function (e) { e.isDefaultPrevented() || l.one(h.HIDDEN, function () { t(s).is(\":visible\") && s.focus() }) }); p._jQueryInterface.call(t(i), a, this) }), t.fn.modal = p._jQueryInterface, t.fn.modal.Constructor = p, t.fn.modal.noConflict = function () { return t.fn.modal = o, p._jQueryInterface }, p }(e), U = function (t) { var e = \"tooltip\", i = \"bs.tooltip\", o = \".\" + i, a = t.fn[e], l = new RegExp(\"(^|\\\\s)bs-tooltip\\\\S+\", \"g\"), h = { animation: \"boolean\", template: \"string\", title: \"(string|element|function)\", trigger: \"string\", delay: \"(number|object)\", html: \"boolean\", selector: \"(string|boolean)\", placement: \"(string|function)\", offset: \"(number|string)\", container: \"(string|element|boolean)\", fallbackPlacement: \"(string|array)\", boundary: \"(string|element)\" }, c = { AUTO: \"auto\", TOP: \"top\", RIGHT: \"right\", BOTTOM: \"bottom\", LEFT: \"left\" }, u = { animation: !0, template: '<div class=\"tooltip\" role=\"tooltip\"><div class=\"arrow\"></div><div class=\"tooltip-inner\"></div></div>', trigger: \"hover focus\", title: \"\", delay: 0, html: !1, selector: !1, placement: \"top\", offset: 0, container: !1, fallbackPlacement: \"flip\", boundary: \"scrollParent\" }, f = \"show\", d = \"out\", _ = { HIDE: \"hide\" + o, HIDDEN: \"hidden\" + o, SHOW: \"show\" + o, SHOWN: \"shown\" + o, INSERTED: \"inserted\" + o, CLICK: \"click\" + o, FOCUSIN: \"focusin\" + o, FOCUSOUT: \"focusout\" + o, MOUSEENTER: \"mouseenter\" + o, MOUSELEAVE: \"mouseleave\" + o }, g = \"fade\", p = \"show\", m = \".tooltip-inner\", v = \".arrow\", E = \"hover\", T = \"focus\", y = \"click\", C = \"manual\", I = function () { function a(t, e) { if (\"undefined\" == typeof n) throw new TypeError(\"Bootstrap tooltips require Popper.js (https://popper.js.org)\"); this._isEnabled = !0, this._timeout = 0, this._hoverState = \"\", this._activeTrigger = {}, this._popper = null, this.element = t, this.config = this._getConfig(e), this.tip = null, this._setListeners() } var I = a.prototype; return I.enable = function () { this._isEnabled = !0 }, I.disable = function () { this._isEnabled = !1 }, I.toggleEnabled = function () { this._isEnabled = !this._isEnabled }, I.toggle = function (e) { if (this._isEnabled) if (e) { var n = this.constructor.DATA_KEY, i = t(e.currentTarget).data(n); i || (i = new this.constructor(e.currentTarget, this._getDelegateConfig()), t(e.currentTarget).data(n, i)), i._activeTrigger.click = !i._activeTrigger.click, i._isWithActiveTrigger() ? i._enter(null, i) : i._leave(null, i) } else { if (t(this.getTipElement()).hasClass(p)) return void this._leave(null, this); this._enter(null, this) } }, I.dispose = function () { clearTimeout(this._timeout), t.removeData(this.element, this.constructor.DATA_KEY), t(this.element).off(this.constructor.EVENT_KEY), t(this.element).closest(\".modal\").off(\"hide.bs.modal\"), this.tip && t(this.tip).remove(), this._isEnabled = null, this._timeout = null, this._hoverState = null, this._activeTrigger = null, null !== this._popper && this._popper.destroy(), this._popper = null, this.element = null, this.config = null, this.tip = null }, I.show = function () { var e = this; if (\"none\" === t(this.element).css(\"display\")) throw new Error(\"Please use show on visible elements\"); var i = t.Event(this.constructor.Event.SHOW); if (this.isWithContent() && this._isEnabled) { t(this.element).trigger(i); var s = t.contains(this.element.ownerDocument.documentElement, this.element); if (i.isDefaultPrevented() || !s) return; var r = this.getTipElement(), o = P.getUID(this.constructor.NAME); r.setAttribute(\"id\", o), this.element.setAttribute(\"aria-describedby\", o), this.setContent(), this.config.animation && t(r).addClass(g); var l = \"function\" == typeof this.config.placement ? this.config.placement.call(this, r, this.element) : this.config.placement, h = this._getAttachment(l); this.addAttachmentClass(h); var c = !1 === this.config.container ? document.body : t(this.config.container); t(r).data(this.constructor.DATA_KEY, this), t.contains(this.element.ownerDocument.documentElement, this.tip) || t(r).appendTo(c), t(this.element).trigger(this.constructor.Event.INSERTED), this._popper = new n(this.element, r, { placement: h, modifiers: { offset: { offset: this.config.offset }, flip: { behavior: this.config.fallbackPlacement }, arrow: { element: v }, preventOverflow: { boundariesElement: this.config.boundary } }, onCreate: function (t) { t.originalPlacement !== t.placement && e._handlePopperPlacementChange(t) }, onUpdate: function (t) { e._handlePopperPlacementChange(t) } }), t(r).addClass(p), \"ontouchstart\" in document.documentElement && t(\"body\").children().on(\"mouseover\", null, t.noop); var u = function () { e.config.animation && e._fixTransition(); var n = e._hoverState; e._hoverState = null, t(e.element).trigger(e.constructor.Event.SHOWN), n === d && e._leave(null, e) }; P.supportsTransitionEnd() && t(this.tip).hasClass(g) ? t(this.tip).one(P.TRANSITION_END, u).emulateTransitionEnd(a._TRANSITION_DURATION) : u() } }, I.hide = function (e) { var n = this, i = this.getTipElement(), s = t.Event(this.constructor.Event.HIDE), r = function () { n._hoverState !== f && i.parentNode && i.parentNode.removeChild(i), n._cleanTipClass(), n.element.removeAttribute(\"aria-describedby\"), t(n.element).trigger(n.constructor.Event.HIDDEN), null !== n._popper && n._popper.destroy(), e && e() }; t(this.element).trigger(s), s.isDefaultPrevented() || (t(i).removeClass(p), \"ontouchstart\" in document.documentElement && t(\"body\").children().off(\"mouseover\", null, t.noop), this._activeTrigger[y] = !1, this._activeTrigger[T] = !1, this._activeTrigger[E] = !1, P.supportsTransitionEnd() && t(this.tip).hasClass(g) ? t(i).one(P.TRANSITION_END, r).emulateTransitionEnd(150) : r(), this._hoverState = \"\") }, I.update = function () { null !== this._popper && this._popper.scheduleUpdate() }, I.isWithContent = function () { return Boolean(this.getTitle()) }, I.addAttachmentClass = function (e) { t(this.getTipElement()).addClass(\"bs-tooltip-\" + e) }, I.getTipElement = function () { return this.tip = this.tip || t(this.config.template)[0], this.tip }, I.setContent = function () { var e = t(this.getTipElement()); this.setElementContent(e.find(m), this.getTitle()), e.removeClass(g + \" \" + p) }, I.setElementContent = function (e, n) { var i = this.config.html; \"object\" == typeof n && (n.nodeType || n.jquery) ? i ? t(n).parent().is(e) || e.empty().append(n) : e.text(t(n).text()) : e[i ? \"html\" : \"text\"](n) }, I.getTitle = function () { var t = this.element.getAttribute(\"data-original-title\"); return t || (t = \"function\" == typeof this.config.title ? this.config.title.call(this.element) : this.config.title), t }, I._getAttachment = function (t) { return c[t.toUpperCase()] }, I._setListeners = function () { var e = this; this.config.trigger.split(\" \").forEach(function (n) { if (\"click\" === n) t(e.element).on(e.constructor.Event.CLICK, e.config.selector, function (t) { return e.toggle(t) }); else if (n !== C) { var i = n === E ? e.constructor.Event.MOUSEENTER : e.constructor.Event.FOCUSIN, s = n === E ? e.constructor.Event.MOUSELEAVE : e.constructor.Event.FOCUSOUT; t(e.element).on(i, e.config.selector, function (t) { return e._enter(t) }).on(s, e.config.selector, function (t) { return e._leave(t) }) } t(e.element).closest(\".modal\").on(\"hide.bs.modal\", function () { return e.hide() }) }), this.config.selector ? this.config = r({}, this.config, { trigger: \"manual\", selector: \"\" }) : this._fixTitle() }, I._fixTitle = function () { var t = typeof this.element.getAttribute(\"data-original-title\"); (this.element.getAttribute(\"title\") || \"string\" !== t) && (this.element.setAttribute(\"data-original-title\", this.element.getAttribute(\"title\") || \"\"), this.element.setAttribute(\"title\", \"\")) }, I._enter = function (e, n) { var i = this.constructor.DATA_KEY; (n = n || t(e.currentTarget).data(i)) || (n = new this.constructor(e.currentTarget, this._getDelegateConfig()), t(e.currentTarget).data(i, n)), e && (n._activeTrigger[\"focusin\" === e.type ? T : E] = !0), t(n.getTipElement()).hasClass(p) || n._hoverState === f ? n._hoverState = f : (clearTimeout(n._timeout), n._hoverState = f, n.config.delay && n.config.delay.show ? n._timeout = setTimeout(function () { n._hoverState === f && n.show() }, n.config.delay.show) : n.show()) }, I._leave = function (e, n) { var i = this.constructor.DATA_KEY; (n = n || t(e.currentTarget).data(i)) || (n = new this.constructor(e.currentTarget, this._getDelegateConfig()), t(e.currentTarget).data(i, n)), e && (n._activeTrigger[\"focusout\" === e.type ? T : E] = !1), n._isWithActiveTrigger() || (clearTimeout(n._timeout), n._hoverState = d, n.config.delay && n.config.delay.hide ? n._timeout = setTimeout(function () { n._hoverState === d && n.hide() }, n.config.delay.hide) : n.hide()) }, I._isWithActiveTrigger = function () { for (var t in this._activeTrigger) if (this._activeTrigger[t]) return !0; return !1 }, I._getConfig = function (n) { return \"number\" == typeof (n = r({}, this.constructor.Default, t(this.element).data(), n)).delay && (n.delay = { show: n.delay, hide: n.delay }), \"number\" == typeof n.title && (n.title = n.title.toString()), \"number\" == typeof n.content && (n.content = n.content.toString()), P.typeCheckConfig(e, n, this.constructor.DefaultType), n }, I._getDelegateConfig = function () { var t = {}; if (this.config) for (var e in this.config) this.constructor.Default[e] !== this.config[e] && (t[e] = this.config[e]); return t }, I._cleanTipClass = function () { var e = t(this.getTipElement()), n = e.attr(\"class\").match(l); null !== n && n.length > 0 && e.removeClass(n.join(\"\")) }, I._handlePopperPlacementChange = function (t) { this._cleanTipClass(), this.addAttachmentClass(this._getAttachment(t.placement)) }, I._fixTransition = function () { var e = this.getTipElement(), n = this.config.animation; null === e.getAttribute(\"x-placement\") && (t(e).removeClass(g), this.config.animation = !1, this.hide(), this.show(), this.config.animation = n) }, a._jQueryInterface = function (e) { return this.each(function () { var n = t(this).data(i), s = \"object\" == typeof e && e; if ((n || !/dispose|hide/.test(e)) && (n || (n = new a(this, s), t(this).data(i, n)), \"string\" == typeof e)) { if (\"undefined\" == typeof n[e]) throw new TypeError('No method named \"' + e + '\"'); n[e]() } }) }, s(a, null, [{ key: \"VERSION\", get: function () { return \"4.0.0\" } }, { key: \"Default\", get: function () { return u } }, { key: \"NAME\", get: function () { return e } }, { key: \"DATA_KEY\", get: function () { return i } }, { key: \"Event\", get: function () { return _ } }, { key: \"EVENT_KEY\", get: function () { return o } }, { key: \"DefaultType\", get: function () { return h } }]), a }(); return t.fn[e] = I._jQueryInterface, t.fn[e].Constructor = I, t.fn[e].noConflict = function () { return t.fn[e] = a, I._jQueryInterface }, I }(e), x = function (t) { var e = \"popover\", n = \"bs.popover\", i = \".\" + n, o = t.fn[e], a = new RegExp(\"(^|\\\\s)bs-popover\\\\S+\", \"g\"), l = r({}, U.Default, { placement: \"right\", trigger: \"click\", content: \"\", template: '<div class=\"popover\" role=\"tooltip\"><div class=\"arrow\"></div><h3 class=\"popover-header\"></h3><div class=\"popover-body\"></div></div>' }), h = r({}, U.DefaultType, { content: \"(string|element|function)\" }), c = \"fade\", u = \"show\", f = \".popover-header\", d = \".popover-body\", _ = { HIDE: \"hide\" + i, HIDDEN: \"hidden\" + i, SHOW: \"show\" + i, SHOWN: \"shown\" + i, INSERTED: \"inserted\" + i, CLICK: \"click\" + i, FOCUSIN: \"focusin\" + i, FOCUSOUT: \"focusout\" + i, MOUSEENTER: \"mouseenter\" + i, MOUSELEAVE: \"mouseleave\" + i }, g = function (r) { var o, g; function p() { return r.apply(this, arguments) || this } g = r, (o = p).prototype = Object.create(g.prototype), o.prototype.constructor = o, o.__proto__ = g; var m = p.prototype; return m.isWithContent = function () { return this.getTitle() || this._getContent() }, m.addAttachmentClass = function (e) { t(this.getTipElement()).addClass(\"bs-popover-\" + e) }, m.getTipElement = function () { return this.tip = this.tip || t(this.config.template)[0], this.tip }, m.setContent = function () { var e = t(this.getTipElement()); this.setElementContent(e.find(f), this.getTitle()); var n = this._getContent(); \"function\" == typeof n && (n = n.call(this.element)), this.setElementContent(e.find(d), n), e.removeClass(c + \" \" + u) }, m._getContent = function () { return this.element.getAttribute(\"data-content\") || this.config.content }, m._cleanTipClass = function () { var e = t(this.getTipElement()), n = e.attr(\"class\").match(a); null !== n && n.length > 0 && e.removeClass(n.join(\"\")) }, p._jQueryInterface = function (e) { return this.each(function () { var i = t(this).data(n), s = \"object\" == typeof e ? e : null; if ((i || !/destroy|hide/.test(e)) && (i || (i = new p(this, s), t(this).data(n, i)), \"string\" == typeof e)) { if (\"undefined\" == typeof i[e]) throw new TypeError('No method named \"' + e + '\"'); i[e]() } }) }, s(p, null, [{ key: \"VERSION\", get: function () { return \"4.0.0\" } }, { key: \"Default\", get: function () { return l } }, { key: \"NAME\", get: function () { return e } }, { key: \"DATA_KEY\", get: function () { return n } }, { key: \"Event\", get: function () { return _ } }, { key: \"EVENT_KEY\", get: function () { return i } }, { key: \"DefaultType\", get: function () { return h } }]), p }(U); return t.fn[e] = g._jQueryInterface, t.fn[e].Constructor = g, t.fn[e].noConflict = function () { return t.fn[e] = o, g._jQueryInterface }, g }(e), K = function (t) { var e = \"scrollspy\", n = \"bs.scrollspy\", i = \".\" + n, o = t.fn[e], a = { offset: 10, method: \"auto\", target: \"\" }, l = { offset: \"number\", method: \"string\", target: \"(string|element)\" }, h = { ACTIVATE: \"activate\" + i, SCROLL: \"scroll\" + i, LOAD_DATA_API: \"load\" + i + \".data-api\" }, c = \"dropdown-item\", u = \"active\", f = { DATA_SPY: '[data-spy=\"scroll\"]', ACTIVE: \".active\", NAV_LIST_GROUP: \".nav, .list-group\", NAV_LINKS: \".nav-link\", NAV_ITEMS: \".nav-item\", LIST_ITEMS: \".list-group-item\", DROPDOWN: \".dropdown\", DROPDOWN_ITEMS: \".dropdown-item\", DROPDOWN_TOGGLE: \".dropdown-toggle\" }, d = \"offset\", _ = \"position\", g = function () { function o(e, n) { var i = this; this._element = e, this._scrollElement = \"BODY\" === e.tagName ? window : e, this._config = this._getConfig(n), this._selector = this._config.target + \" \" + f.NAV_LINKS + \",\" + this._config.target + \" \" + f.LIST_ITEMS + \",\" + this._config.target + \" \" + f.DROPDOWN_ITEMS, this._offsets = [], this._targets = [], this._activeTarget = null, this._scrollHeight = 0, t(this._scrollElement).on(h.SCROLL, function (t) { return i._process(t) }), this.refresh(), this._process() } var g = o.prototype; return g.refresh = function () { var e = this, n = this._scrollElement === this._scrollElement.window ? d : _, i = \"auto\" === this._config.method ? n : this._config.method, s = i === _ ? this._getScrollTop() : 0; this._offsets = [], this._targets = [], this._scrollHeight = this._getScrollHeight(), t.makeArray(t(this._selector)).map(function (e) { var n, r = P.getSelectorFromElement(e); if (r && (n = t(r)[0]), n) { var o = n.getBoundingClientRect(); if (o.width || o.height) return [t(n)[i]().top + s, r] } return null }).filter(function (t) { return t }).sort(function (t, e) { return t[0] - e[0] }).forEach(function (t) { e._offsets.push(t[0]), e._targets.push(t[1]) }) }, g.dispose = function () { t.removeData(this._element, n), t(this._scrollElement).off(i), this._element = null, this._scrollElement = null, this._config = null, this._selector = null, this._offsets = null, this._targets = null, this._activeTarget = null, this._scrollHeight = null }, g._getConfig = function (n) { if (\"string\" != typeof (n = r({}, a, n)).target) { var i = t(n.target).attr(\"id\"); i || (i = P.getUID(e), t(n.target).attr(\"id\", i)), n.target = \"#\" + i } return P.typeCheckConfig(e, n, l), n }, g._getScrollTop = function () { return this._scrollElement === window ? this._scrollElement.pageYOffset : this._scrollElement.scrollTop }, g._getScrollHeight = function () { return this._scrollElement.scrollHeight || Math.max(document.body.scrollHeight, document.documentElement.scrollHeight) }, g._getOffsetHeight = function () { return this._scrollElement === window ? window.innerHeight : this._scrollElement.getBoundingClientRect().height }, g._process = function () { var t = this._getScrollTop() + this._config.offset, e = this._getScrollHeight(), n = this._config.offset + e - this._getOffsetHeight(); if (this._scrollHeight !== e && this.refresh(), t >= n) { var i = this._targets[this._targets.length - 1]; this._activeTarget !== i && this._activate(i) } else { if (this._activeTarget && t < this._offsets[0] && this._offsets[0] > 0) return this._activeTarget = null, void this._clear(); for (var s = this._offsets.length; s--;) { this._activeTarget !== this._targets[s] && t >= this._offsets[s] && (\"undefined\" == typeof this._offsets[s + 1] || t < this._offsets[s + 1]) && this._activate(this._targets[s]) } } }, g._activate = function (e) { this._activeTarget = e, this._clear(); var n = this._selector.split(\",\"); n = n.map(function (t) { return t + '[data-target=\"' + e + '\"],' + t + '[href=\"' + e + '\"]' }); var i = t(n.join(\",\")); i.hasClass(c) ? (i.closest(f.DROPDOWN).find(f.DROPDOWN_TOGGLE).addClass(u), i.addClass(u)) : (i.addClass(u), i.parents(f.NAV_LIST_GROUP).prev(f.NAV_LINKS + \", \" + f.LIST_ITEMS).addClass(u), i.parents(f.NAV_LIST_GROUP).prev(f.NAV_ITEMS).children(f.NAV_LINKS).addClass(u)), t(this._scrollElement).trigger(h.ACTIVATE, { relatedTarget: e }) }, g._clear = function () { t(this._selector).filter(f.ACTIVE).removeClass(u) }, o._jQueryInterface = function (e) { return this.each(function () { var i = t(this).data(n); if (i || (i = new o(this, \"object\" == typeof e && e), t(this).data(n, i)), \"string\" == typeof e) { if (\"undefined\" == typeof i[e]) throw new TypeError('No method named \"' + e + '\"'); i[e]() } }) }, s(o, null, [{ key: \"VERSION\", get: function () { return \"4.0.0\" } }, { key: \"Default\", get: function () { return a } }]), o }(); return t(window).on(h.LOAD_DATA_API, function () { for (var e = t.makeArray(t(f.DATA_SPY)), n = e.length; n--;) { var i = t(e[n]); g._jQueryInterface.call(i, i.data()) } }), t.fn[e] = g._jQueryInterface, t.fn[e].Constructor = g, t.fn[e].noConflict = function () { return t.fn[e] = o, g._jQueryInterface }, g }(e), V = function (t) { var e = \"bs.tab\", n = \".\" + e, i = t.fn.tab, r = { HIDE: \"hide\" + n, HIDDEN: \"hidden\" + n, SHOW: \"show\" + n, SHOWN: \"shown\" + n, CLICK_DATA_API: \"click.bs.tab.data-api\" }, o = \"dropdown-menu\", a = \"active\", l = \"disabled\", h = \"fade\", c = \"show\", u = \".dropdown\", f = \".nav, .list-group\", d = \".active\", _ = \"> li > .active\", g = '[data-toggle=\"tab\"], [data-toggle=\"pill\"], [data-toggle=\"list\"]', p = \".dropdown-toggle\", m = \"> .dropdown-menu .active\", v = function () { function n(t) { this._element = t } var i = n.prototype; return i.show = function () { var e = this; if (!(this._element.parentNode && this._element.parentNode.nodeType === Node.ELEMENT_NODE && t(this._element).hasClass(a) || t(this._element).hasClass(l))) { var n, i, s = t(this._element).closest(f)[0], o = P.getSelectorFromElement(this._element); if (s) { var h = \"UL\" === s.nodeName ? _ : d; i = (i = t.makeArray(t(s).find(h)))[i.length - 1] } var c = t.Event(r.HIDE, { relatedTarget: this._element }), u = t.Event(r.SHOW, { relatedTarget: i }); if (i && t(i).trigger(c), t(this._element).trigger(u), !u.isDefaultPrevented() && !c.isDefaultPrevented()) { o && (n = t(o)[0]), this._activate(this._element, s); var g = function () { var n = t.Event(r.HIDDEN, { relatedTarget: e._element }), s = t.Event(r.SHOWN, { relatedTarget: i }); t(i).trigger(n), t(e._element).trigger(s) }; n ? this._activate(n, n.parentNode, g) : g() } } }, i.dispose = function () { t.removeData(this._element, e), this._element = null }, i._activate = function (e, n, i) { var s = this, r = (\"UL\" === n.nodeName ? t(n).find(_) : t(n).children(d))[0], o = i && P.supportsTransitionEnd() && r && t(r).hasClass(h), a = function () { return s._transitionComplete(e, r, i) }; r && o ? t(r).one(P.TRANSITION_END, a).emulateTransitionEnd(150) : a() }, i._transitionComplete = function (e, n, i) { if (n) { t(n).removeClass(c + \" \" + a); var s = t(n.parentNode).find(m)[0]; s && t(s).removeClass(a), \"tab\" === n.getAttribute(\"role\") && n.setAttribute(\"aria-selected\", !1) } if (t(e).addClass(a), \"tab\" === e.getAttribute(\"role\") && e.setAttribute(\"aria-selected\", !0), P.reflow(e), t(e).addClass(c), e.parentNode && t(e.parentNode).hasClass(o)) { var r = t(e).closest(u)[0]; r && t(r).find(p).addClass(a), e.setAttribute(\"aria-expanded\", !0) } i && i() }, n._jQueryInterface = function (i) { return this.each(function () { var s = t(this), r = s.data(e); if (r || (r = new n(this), s.data(e, r)), \"string\" == typeof i) { if (\"undefined\" == typeof r[i]) throw new TypeError('No method named \"' + i + '\"'); r[i]() } }) }, s(n, null, [{ key: \"VERSION\", get: function () { return \"4.0.0\" } }]), n }(); return t(document).on(r.CLICK_DATA_API, g, function (e) { e.preventDefault(), v._jQueryInterface.call(t(this), \"show\") }), t.fn.tab = v._jQueryInterface, t.fn.tab.Constructor = v, t.fn.tab.noConflict = function () { return t.fn.tab = i, v._jQueryInterface }, v }(e); !function (t) { if (\"undefined\" == typeof t) throw new TypeError(\"Bootstrap's JavaScript requires jQuery. jQuery must be included before Bootstrap's JavaScript.\"); var e = t.fn.jquery.split(\" \")[0].split(\".\"); if (e[0] < 2 && e[1] < 9 || 1 === e[0] && 9 === e[1] && e[2] < 1 || e[0] >= 4) throw new Error(\"Bootstrap's JavaScript requires at least jQuery v1.9.1 but less than v4.0.0\") }(e), t.Util = P, t.Alert = L, t.Button = R, t.Carousel = j, t.Collapse = H, t.Dropdown = W, t.Modal = M, t.Popover = x, t.Scrollspy = K, t.Tab = V, t.Tooltip = U, Object.defineProperty(t, \"__esModule\", { value: !0 }) });\n//# sourceMappingURL=bootstrap.min.js.map\r\n    </script>\r\n\r\n\r\n</head>\r\n<body>\r\n\r\n    <nav class=\"navbar navbar-dark bg-dark\">\r\n        <h1 style=\"color:white\">taskt</h1><small style=\"color:white\">free and open-source process automation</small>\r\n    </nav>\r\n    <br />\r\n\r\n\r\n    <div class=\"container\">\r\n\r\n        <h5><b>Directions:</b> This a sample data collection form that can be presented to a user.  You can add and implement as many fields as you need or choose standard form inputs. Note, each field will require a <b>v_applyToVariable</b> attribute specifying which variable should contain the respective value for the input field.</h5>\r\n\r\n        <hr />\r\n        <form>\r\n            <div class=\"form-row\">\r\n                <div class=\"form-group col-md-6\">\r\n                    <label for=\"inputEmail4\">Email</label>\r\n                    <input type=\"email\" class=\"form-control\" id=\"inputEmail4\" v_applyToVariable=\"vInput\" placeholder=\"Email\">\r\n                </div>\r\n                <div class=\"form-group col-md-6\">\r\n                    <label for=\"inputPassword4\">Password</label>\r\n                    <input type=\"password\" class=\"form-control\" id=\"inputPassword4\" v_applyToVariable=\"vPass\" placeholder=\"Password\">\r\n                </div>\r\n            </div>\r\n            <div class=\"form-group\">\r\n                <label for=\"inputAddress\">Address</label>\r\n                <input type=\"text\" class=\"form-control\" id=\"inputAddress\" v_applyToVariable=\"vAddress\" placeholder=\"1234 Main St\">\r\n            </div>\r\n            <div class=\"form-group\">\r\n                <label for=\"inputAddress2\">Address 2</label>\r\n                <input type=\"text\" class=\"form-control\" id=\"inputAddress2\" v_applyToVariable=\"vAddress2\" placeholder=\"Apartment, studio, or floor\">\r\n            </div>\r\n            <div class=\"form-row\">\r\n                <div class=\"form-group col-md-6\">\r\n                    <label for=\"inputCity\">City</label>\r\n                    <input type=\"text\" class=\"form-control\" id=\"inputCity\" v_applyToVariable=\"vCity\">\r\n                </div>\r\n                <div class=\"form-group col-md-4\">\r\n                    <label for=\"inputState\">State</label>\r\n                    <input type=\"text\" class=\"form-control\" id=\"inputState\" v_applyToVariable=\"vState\">\r\n                </div>\r\n                <div class=\"form-group col-md-2\">\r\n                    <label for=\"inputZip\">Zip</label>\r\n                    <input type=\"text\" class=\"form-control\" id=\"inputZip\" v_applyToVariable=\"vZip\">\r\n                </div>\r\n            </div>\r\n            <div class=\"form-group\">\r\n                <div class=\"form-check\">\r\n                    <input class=\"form-check-input\" type=\"checkbox\" id=\"gridCheck\" v_applyToVariable=\"vCheck\">\r\n                    <label class=\"form-check-label\" for=\"gridCheck\">\r\n                        Check me out\r\n                    </label>\r\n                </div>\r\n            </div>\r\n            <div class=\"form-group\">\r\n                <label for=\"exampleFormControlSelect1\">Example select</label>\r\n                <select class=\"form-control\" id=\"exampleFormControlSelect1\" v_applyToVariable=\"vSelected\">\r\n                    <option>1</option>\r\n                    <option>2</option>\r\n                    <option>3</option>\r\n                    <option>4</option>\r\n                    <option>5</option>\r\n                </select>\r\n            </div>\r\n            <button class=\"btn btn-primary\" onclick=\"window.external.Ok();\">Ok</button>\r\n            <button class=\"btn btn-primary\" onclick=\"window.external.Cancel();\">Close</button>\r\n        </form>\r\n    </div>\r\n\r\n</body>\r\n</html>";
        }

        public override void RunCommand(object sender)
        {


            var engine = (Core.AutomationEngineInstance)sender;


            if (engine.tasktEngineUI == null)
            {
                engine.ReportProgress("HTML UserInput Supported With UI Only");
                System.Windows.Forms.MessageBox.Show("HTML UserInput Supported With UI Only", "UserInput Command", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }
            
            //invoke ui for data collection
            var result = engine.tasktEngineUI.Invoke(new Action(() =>
            {

                //sample for temp testing
                var htmlInput = v_InputHTML.ConvertToUserVariable(sender);

                var variables = engine.tasktEngineUI.ShowHTMLInput(htmlInput);

                //if user selected Ok then process variables
                //null result means user cancelled/closed
                if (variables != null)
                {
                    //store each one into context
                    foreach (var variable in variables)
                    {
                        variable.VariableValue.ToString().StoreInUserVariable(sender, variable.VariableName);
                    }
                }
                else if(v_ErrorOnClose == "Error On Close")
                {
                    throw new Exception("Input Form was closed by the user");
                }


            }

            ));


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Show Form To User]";
        }
    }
    #endregion Input Commands

    #region Database Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("Database Commands")]
    [Attributes.ClassAttributes.Description("This command selects data from a database and applies it against a dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to select data from a database.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'OLEDB' to achieve automation.")]
    public class DatabaseRunQueryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please create a dataset variable name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a custom name that references the dataset.")]
        [Attributes.PropertyAttributes.SampleUsage("**MyData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataSetName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the connection string")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a valid connection string to be used by the database.")]
        [Attributes.PropertyAttributes.SampleUsage(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\myFolder\myAccessFile.accdb;Persist Security Info = False;")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ConnectionString { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please provide the query to run")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the query as text that should be executed.")]
        [Attributes.PropertyAttributes.SampleUsage("**Select * From [table]**")]
        [Attributes.PropertyAttributes.Remarks("")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform a series of commands an endless amount of times.")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform a series of commands a specified amount of times.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    public class BeginNumberOfTimesLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Enter how many times to perform the loop")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the amount of times you would like to perform the encased commands.")]
        [Attributes.PropertyAttributes.SampleUsage("**5** or **10**")]
        [Attributes.PropertyAttributes.Remarks("")]
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

            if (!engine.VariableList.Any(f => f.VariableName == "Loop.CurrentIndex"))
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = "Loop.CurrentIndex", VariableValue = "0" });
            }


            int loopTimes;
            Script.ScriptVariable complexVarible = null;

            var loopParameter = loopCommand.v_LoopParameter.ConvertToUserVariable(sender);

            loopTimes = int.Parse(loopParameter);

            for (int i = 0; i < loopTimes; i++)
            {
                if (complexVarible != null)
                    complexVarible.CurrentPosition = i;

                (i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");

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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to iterate over each item in a list, or a series of items.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    public class BeginListLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please input the list variable to be looped")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a variable which contains a list of items")]
        [Attributes.PropertyAttributes.SampleUsage("[vMyList]")]
        [Attributes.PropertyAttributes.Remarks("Use this command to iterate over the results of the Split command.")]
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


            if (!engine.VariableList.Any(f => f.VariableName == "Loop.CurrentIndex"))
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = "Loop.CurrentIndex", VariableValue = "0" });
            }

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

            dynamic listToLoop;
            if (complexVariable.VariableValue is List<string>)
            {
             listToLoop = (List<string>)complexVariable.VariableValue;
            }
            else if (complexVariable.VariableValue is List<OpenQA.Selenium.IWebElement>)
            {
              listToLoop = (List<OpenQA.Selenium.IWebElement>)complexVariable.VariableValue;
            }
            else
            {
                throw new Exception("Complex Variable List Type<T> Not Supported");
            }


            loopTimes = listToLoop.Count;


            for (int i = 0; i < loopTimes; i++)
            {
                if (complexVariable != null)
                    complexVariable.CurrentPosition = i;

                (i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");

                engine.ReportProgress("Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber);

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;


                    engine.ExecuteCommand(cmd);


                    if (engine.CurrentLoopCancelled)
                        return;
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to iterate over a series of Excel cells.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to loop through a known Excel DataSet")]
    public class BeginExcelDatasetLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Excel DataSet Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a unique dataset name that will be used later to traverse over the data")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
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

            if (!engine.VariableList.Any(f => f.VariableName == "Loop.CurrentIndex"))
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = "Loop.CurrentIndex", VariableValue = "0" });
            }


            DataTable excelTable = (DataTable)dataSetVariable.VariableValue;


            var loopTimes = excelTable.Rows.Count;

            for (int i = 0; i < excelTable.Rows.Count; i++)
            {
                dataSetVariable.CurrentPosition = i;

                (i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");

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
    [Attributes.ClassAttributes.UsesDescription("Use this command to signify the end point of a loop command.")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command to signify that looping should end and commands outside the loop should resume execution.")]
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
    [Attributes.ClassAttributes.Description("This command opens the Excel Application.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to launch a new instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelCreateApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Signifies a unique name that will represemt the application instance.  This unique name allows you to refer to the instance by name in future commands, ensuring that the commands you specify run against the correct application.")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **excelInstance**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
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
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var newExcelSession = new Microsoft.Office.Interop.Excel.Application
            {
                Visible = true
            };

            engine.AddAppInstance(vInstance, newExcelSession);


           
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command opens an Excel Workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to open an existing Excel Workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelOpenWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the workbook file path")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file that should be opened by Excel.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.xlsx or [vFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vFilePath = v_FilePath.ConvertToUserVariable(sender);

           var excelObject = engine.GetAppInstance(vInstance);
            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            excelInstance.Workbooks.Open(vFilePath);

           
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Open from '" + v_FilePath + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command adds a new Excel Workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a new workbook to an Exel Instance")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelAddWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
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
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);


                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.Workbooks.Add();
            
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command moves to a specific cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to move to a new cell from your currently selected cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelGoToCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("A1, B10, [vAddress]")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var excelObject = engine.GetAppInstance(vInstance);

 
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                excelSheet.Range[v_CellLocation].Select();
            
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Go To: '" + v_CellLocation + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command sets the value of a cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelSetCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter text to set")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text value that will be set.")]
        [Attributes.PropertyAttributes.SampleUsage("Hello World or [vText]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_TextToSet { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("A1, B10, [vAddress]")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var excelObject = engine.GetAppInstance(vInstance);

           
                var targetAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);
                var targetText = v_TextToSet.ConvertToUserVariable(sender);

                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                excelSheet.Range[targetAddress].Value = targetText;
            
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set Cell '" + v_ExcelCellAddress + "' to '" + v_TextToSet + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command gets text from a specified Excel Cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a value from a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    public class ExcelGetCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("A1, B10, [vAddress]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ExcelCellAddress { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Assign to Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
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
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);

           

                var targetAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);

                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                var cellValue = (string)excelSheet.Range[targetAddress].Text;
                cellValue.StoreInUserVariable(sender, v_userVariableName);
            
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Get Value From '" + v_ExcelCellAddress + "' and apply to variable '" + v_userVariableName + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command runs a macro.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a run a specific macro in the Excel workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelRunMacroCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the macro name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the name of the macro as it exists in the spreadsheet")]
        [Attributes.PropertyAttributes.SampleUsage("Macro1")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);

           
                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                excelInstance.Run(v_MacroName);
            
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to find the last row in a used range in an Excel Workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to determine how many rows have been used in the Excel Workbook.  You can use this value in a **Number Of Times** Loop to get data.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelGetLastRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter Letter of the Column to check (ex. A, B, C)")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a valid column letter")]
        [Attributes.PropertyAttributes.SampleUsage("A, B, AA, etc.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ColumnLetter { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the row number")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }
        public ExcelGetLastRowCommand()
        {
            this.CommandName = "ExcelGetLastRowCommand";
            this.SelectionName = "Get Last Row Index";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var excelObject = engine.GetAppInstance(vInstance);

            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                var excelSheet = excelInstance.ActiveSheet;
                var lastRow = (int)excelSheet.Cells(excelSheet.Rows.Count, "A").End(Microsoft.Office.Interop.Excel.XlDirection.xlUp).Row;


                lastRow.ToString().StoreInUserVariable(sender, v_applyToVariableName);


            
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Column '" + v_ColumnLetter + "', Apply to '" + v_applyToVariableName + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to close Excel.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an open instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelCloseApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate if the Workbook should be saved")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a TRUE or FALSE value")]
        [Attributes.PropertyAttributes.SampleUsage("'TRUE' or 'FALSE'")]
        [Attributes.PropertyAttributes.Remarks("")]
        public bool v_ExcelSaveOnExit { get; set; }
        public ExcelCloseApplicationCommand()
        {
            this.CommandName = "ExcelCloseApplicationCommand";
            this.SelectionName = "Close Excel Application";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);


            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;


            //check if workbook exists and save
            if (excelInstance.ActiveWorkbook != null)
            {
                excelInstance.ActiveWorkbook.Close(v_ExcelSaveOnExit);
            }

            //close excel
            excelInstance.Quit();

            //remove instance
            engine.RemoveAppInstance(vInstance);

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Save On Close: " + v_ExcelSaveOnExit + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to save an Excel workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to save a workbook to a file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelSaveAsCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the directory of the file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfile.xlsx or [vExcelFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FileName { get; set; }

        public ExcelSaveAsCommand()
        {
            this.CommandName = "ExcelSaveAsCommand";
            this.SelectionName = "Save Workbook As";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Core.AutomationEngineInstance)sender;

            //convert variables
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var fileName = v_FileName.ConvertToUserVariable(engine);

            //get excel app object
            var excelObject = engine.GetAppInstance(vInstance);

            //convert object
            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;

            //overwrite and save
            excelInstance.DisplayAlerts = false;
            excelInstance.ActiveWorkbook.SaveAs(fileName);
            excelInstance.DisplayAlerts = true;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Save To '" + v_FileName + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to save an Excel workbook.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to save changes to a workbook.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelSaveCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        public ExcelSaveCommand()
        {
            this.CommandName = "ExcelSaveCommand";
            this.SelectionName = "Save Workbook";
            this.CommandEnabled = true;
        }
        public override void RunCommand(object sender)
        {
            //get engine context
            var engine = (Core.AutomationEngineInstance)sender;

            //convert variables
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            //get excel app object
            var excelObject = engine.GetAppInstance(vInstance);

            //convert object
            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;

            //save
            excelInstance.ActiveWorkbook.Save();

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to activate a specific worksheet in a workbook")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to switch to a specific worksheet")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelActivateSheetCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the name of the sheet within the Workbook to activate")]
        [Attributes.PropertyAttributes.InputSpecification("Specify the name of the actual sheet")]
        [Attributes.PropertyAttributes.SampleUsage("Sheet1, mySheetName, [vSheet]")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var excelObject = engine.GetAppInstance(vInstance);

            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                string sheetToDelete = v_SheetName.ConvertToUserVariable(sender);
                Microsoft.Office.Interop.Excel.Worksheet workSheet = excelInstance.Sheets[sheetToDelete];
                workSheet.Select();



            
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Sheet Name: " + v_SheetName + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to delete a specified row in Excel")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to delete an entire row from the current sheet.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelDeleteRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the row number to delete")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the number of the row that should be deleted.")]
        [Attributes.PropertyAttributes.SampleUsage("1, 5, [vNumber]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_RowNumber { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate whether the row below will be shifted up to replace the old row.")]
        [Attributes.PropertyAttributes.SampleUsage("Select 'Yes' or 'No'")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);

          


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
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Row Number: " + v_RowNumber + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to delete a specified cell in Excel")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to delete a specific cell from the current sheet.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelDeleteCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the range to delete ex. A1 or A1:C1")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("A1, B10, [vAddress]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Range { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Should the cells below shift upward after deletion?")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate whether the row below will be shifted up to replace the old row.")]
        [Attributes.PropertyAttributes.SampleUsage("Select 'Yes' or 'No'")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);

           

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
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Range: " + v_Range + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.Description("This command gets a range of cells and applies them against a dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to quickly iterate over Excel as a dataset.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'OLEDB' to achieve automation.")]
    public class ExcelCreateDataSetCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please create a DataSet name")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate a unique reference name for later use")]
        [Attributes.PropertyAttributes.SampleUsage("vMyDataset")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataSetName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the workbook file path")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the workbook file")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.xlsx")]
        [Attributes.PropertyAttributes.Remarks("This command does not require Excel to be opened.  A snapshot will be taken of the workbook as it exists at the time this command runs.")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the sheet name")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the specific sheet that should be retrieved.")]
        [Attributes.PropertyAttributes.SampleUsage("Sheet1, mySheet, [vSheet]")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            return base.GetDisplayValue() + " [Get '" + v_SheetName + "' from '" + v_FilePath + "' and apply to '" + v_DataSetName + "']";
        }
    }

    #endregion Excel Commands

    #region Data Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to build a date and apply it to a variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform a date calculation.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class DateCalculationCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the date value or variable (ex. [DateTime.Now]")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify either text or a variable that contains the start date.")]
        [Attributes.PropertyAttributes.SampleUsage("[DateTime.Now] or 1/1/2000")]
        [Attributes.PropertyAttributes.Remarks("You can use known text or variables.")]
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
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary operation")]
        [Attributes.PropertyAttributes.SampleUsage("Select From Add Seconds, Add Minutes, Add Hours, Add Days, Add Years, Subtract Seconds, Subtract Minutes, Subtract Hours, Subtract Days, Subtract Years ")]
        public string v_CalculationMethod { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the increment value")]
        [Attributes.PropertyAttributes.InputSpecification("Enter how many units to increment by")]
        [Attributes.PropertyAttributes.SampleUsage("15, [vIncrement]")]
        [Attributes.PropertyAttributes.Remarks("You can use negative numbers which will do the opposite, ex. Subtract Days and an increment of -5 will Add Days.")]
        public string v_Increment { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Specify String Format")]
        [Attributes.PropertyAttributes.InputSpecification("Specify if a specific string format is required.")]
        [Attributes.PropertyAttributes.SampleUsage("MM/dd/yy, hh:mm, etc.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ToStringFormat { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the date calculation")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to apply specific formatting to text or a variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class FormatDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the value or variable (ex. [DateTime.Now]")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify either text or a variable that contains a date or number requiring formatting")]
        [Attributes.PropertyAttributes.SampleUsage("[DateTime.Now], 1/1/2000, 2500")]
        [Attributes.PropertyAttributes.Remarks("You can use known text or variables.")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the type of data")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Date")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Number")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the source type")]
        [Attributes.PropertyAttributes.SampleUsage("Choose **Date** or **Number**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FormatType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify required output format")]
        [Attributes.PropertyAttributes.InputSpecification("Specify if a specific string format is required.")]
        [Attributes.PropertyAttributes.SampleUsage("MM/dd/yy, hh:mm, C2, D2, etc.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ToStringFormat { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive output")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
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
                formattedString.StoreInUserVariable(sender, v_applyToVariableName);
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select a subset of text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    public class StringSubstringCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable or text to modify")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Start from Position")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the starting position within the string")]
        [Attributes.PropertyAttributes.SampleUsage("0 for beginning, 1 for first character, etc.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public int v_startIndex { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Length (-1 to keep remainder)")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate if only so many characters should be kept")]
        [Attributes.PropertyAttributes.SampleUsage("-1 to keep remainder, 1 for 1 position after start index, etc.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public int v_stringLength { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the changes")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
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


            var variableName = v_userVariableName.ConvertToUserVariable(sender);

            //apply substring
            if (v_stringLength >= 0)
            {
                variableName = variableName.Substring(v_startIndex, v_stringLength);
            }
            else
            {
                variableName = variableName.Substring(v_startIndex);
            }

            variableName.StoreInUserVariable(sender, v_applyToVariableName);
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Substring to '" + v_userVariableName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to split a string")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to split a single text or variable into multiple items")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Split method to achieve automation.")]
    public class StringSplitCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select variable or text to split")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Input Delimiter (ex. [crLF] for new line, [chars] for each char, ',')")]
        [Attributes.PropertyAttributes.InputSpecification("Declare the character that will be used to seperate. [crLF] can be used for line breaks and [chars] can be used to split each digit/letter")]
        [Attributes.PropertyAttributes.SampleUsage("[crLF], [chars], ',' (comma - with no single quote wrapper)")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_splitCharacter { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the list variable which will contain the results")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyConvertToUserVariableName { get; set; }
        public StringSplitCommand()
        {
            this.CommandName = "StringSplitCommand";
            this.SelectionName = "Split Text";
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
            
            var v_receivingVariable = v_applyConvertToUserVariableName.Replace(engine.engineSettings.VariableStartMarker, "").Replace(engine.engineSettings.VariableEndMarker, "");
            //get complex variable from engine and assign
            var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_receivingVariable).FirstOrDefault();

            if (requiredComplexVariable == null)
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_receivingVariable, CurrentPosition = 0 });
                requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_receivingVariable).FirstOrDefault();
            }

            requiredComplexVariable.VariableValue = splitString;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Split '" + v_userVariableName + "' by '" + v_splitCharacter + "' and apply to '" + v_applyConvertToUserVariableName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to replace text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to replace existing text within text or a variable with new text")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Substring method to achieve automation.")]
    public class StringReplaceCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select text or variable to modify")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("indicate the text to be replaced")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the old value of the text that will be replaced")]
        [Attributes.PropertyAttributes.SampleUsage("H")]
        [Attributes.PropertyAttributes.Remarks("H in Hello would be targeted for replacement")]
        public string v_replacementText { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("indicate the replacement value")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the new value after replacement")]
        [Attributes.PropertyAttributes.SampleUsage("J")]
        [Attributes.PropertyAttributes.Remarks("H would be replaced with J to create 'Jello'")]
        public string v_replacementValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the changes")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }
        public StringReplaceCommand()
        {
            this.CommandName = "StringReplaceCommand";
            this.SelectionName = "Replace Text";
            this.CommandEnabled = true;

        }
        public override void RunCommand(object sender)
        {
            //get full text
            string replacementVariable = v_userVariableName.ConvertToUserVariable(sender);

            //get replacement text and value
            string replacementText = v_replacementText.ConvertToUserVariable(sender);
            string replacementValue = v_replacementValue.ConvertToUserVariable(sender);

            //perform replacement
            replacementVariable = replacementVariable.Replace(replacementText, replacementValue);

            //store in variable
            replacementVariable.StoreInUserVariable(sender, v_applyToVariableName);

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Replace '" + v_replacementText + "' with '" + v_replacementValue + "', apply to '" + v_userVariableName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to perform advanced string formatting using RegEx.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform an advanced RegEx extraction from a text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class RegExExtractorCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please supply the value or variable (ex. [vSomeVariable])")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or text value")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello** or **vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Input the RegEx Extractor Pattern")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the RegEx extractor pattern that should be used to extract the text")]
        [Attributes.PropertyAttributes.SampleUsage(@"^([\w\-]+)")]
        [Attributes.PropertyAttributes.Remarks("If an extractor splits each word in a sentence, for example, you will need to specify the associated index of the word that is required.")]
        public string v_RegExExtractor { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Matching Group Index")]
        [Attributes.PropertyAttributes.InputSpecification("Define the index of the result")]
        [Attributes.PropertyAttributes.SampleUsage("1")]
        [Attributes.PropertyAttributes.Remarks("The extractor will split multiple patterns found into multiple indexes.  Test which index is required to retrieve the value or create a better/more define extractor.")]
        public string v_MatchGroupIndex { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the RegEx result")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract a piece of text from a larger text or variable")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class TextExtractorCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the value or variable requiring extraction (ex. [vSomeVariable])")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or text value")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello** or **vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select text extraction type")]
        [Attributes.PropertyAttributes.InputSpecification("Select the type of extraction that is required.")]
        [Attributes.PropertyAttributes.SampleUsage("Select from Before Text, After Text, Between Text")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_TextExtractionType { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Extraction Parameters")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define the required extraction parameters, which is dependent on the type of extraction.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public DataTable v_TextExtractionTable { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the extracted text")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
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
    [Attributes.ClassAttributes.Description("This command allows you to parse a JSON object into a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract data from a JSON object")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ParseJsonCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the value or variable requiring extraction (ex. [vSomeVariable])")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or text value")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello** or **vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Specify a JSON extractor")]
        [Attributes.PropertyAttributes.InputSpecification("Input a JSON token extractor")]
        [Attributes.PropertyAttributes.SampleUsage("Select from Before Text, After Text, Between Text")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_JsonExtractor { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the extracted json")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        public ParseJsonCommand()
        {
            this.CommandName = "ParseJsonCommand";
            this.SelectionName = "Parse JSON";
            this.CommandEnabled = true;
          
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //get variablized input
            var variableInput = v_InputValue.ConvertToUserVariable(sender);

            //get variablized token
            var jsonSearchToken = v_JsonExtractor.ConvertToUserVariable(sender);

            //create objects
            Newtonsoft.Json.Linq.JObject o;
            IEnumerable<Newtonsoft.Json.Linq.JToken> searchResults;
            List<string> resultList = new List<string>();

            //parse json
            try
            {
                 o = Newtonsoft.Json.Linq.JObject.Parse(variableInput);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
            }
 

            //select results
            try
            {
                searchResults = o.SelectTokens(jsonSearchToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
            }
        

            //add results to result list since list<string> is supported
            foreach (var result in searchResults)
            {
                resultList.Add(result.ToString());
            }

            //get variable
            var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();

            //create if var does not exist
            if (requiredComplexVariable == null)
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_applyToVariableName, CurrentPosition = 0 });
                requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();
            }

            //assign value to variable
            requiredComplexVariable.VariableValue = resultList;

        }

      
       
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Result(s) To Variable: " + v_applyToVariableName + "]";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command logs data to files.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to log custom data to a file for debugging or analytical purposes.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class LogDataCommand : ScriptCommand
    {


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select existing log file or enter a custom name.")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Engine Logs")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the file name where logs should be appended to")]
        [Attributes.PropertyAttributes.SampleUsage("Select 'Engine Logs' or specify your own file")]
        [Attributes.PropertyAttributes.Remarks("Date and Time will be automatically appended to the file name.  Logs are all saved in taskt Root\\Logs folder")]
        public string v_LogFile { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the text to log.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the value of the text to be saved.")]
        [Attributes.PropertyAttributes.SampleUsage("Third Step Complete, [vVariable], etc.")]
        [Attributes.PropertyAttributes.Remarks("")]
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
        [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class PDFTextExtractionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the PDF file path")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the applicable file.")]
        [Attributes.PropertyAttributes.SampleUsage(@"C:\temp\myfile.pdf or [vFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the PDF text")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        public PDFTextExtractionCommand()
        {
            this.CommandName = "PDFTextExtractionCommand";
            this.SelectionName = "PDF Extraction";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {

            //get variable path to source file
            var vSourceFilePath = v_FilePath.ConvertToUserVariable(sender);

            //create process interface
            JavaInterface javaInterface = new JavaInterface();

            //get output from process
            var result = javaInterface.ExtractPDFText(vSourceFilePath);

            //apply to variable
            result.StoreInUserVariable(sender, v_applyToVariableName);



        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Extract From '" + v_FilePath + "' and apply result to '" + v_applyToVariableName + "'" ;
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to parse a JSON object into a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract data from a JSON object")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetWordCountCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the value or variable requiring the word count (ex. [vSomeVariable])")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or text value")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello** or **vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputValue { get; set; }


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the extracted json")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }

        public GetWordCountCommand()
        {
            this.CommandName = "GetWordCountCommand";
            this.SelectionName = "Get Word Count";
            this.CommandEnabled = true;

        }

        public override void RunCommand(object sender)
        {
            //get engine
            var engine = (AutomationEngineInstance)sender;

            //get input value
            var stringRequiringCount = v_InputValue.ConvertToUserVariable(sender);

            //count number of words
            var wordCount = stringRequiringCount.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries).Length;

            //store word count into variable
            wordCount.ToString().StoreInUserVariable(sender, v_applyToVariableName);

        }



        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply Result to: '" + v_applyToVariableName + "']";
        }
    }
    #endregion Data Commands

    #region If Commands

    [Serializable]
    [Attributes.ClassAttributes.Group("If Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to evaluate a logical statement to determine if the statement is true.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check if a statement is 'true' or 'false' and subsequently take an action based on either condition. Any 'BeginIf' command must have a following 'EndIf' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command evaluates supplied arguments and if evaluated to true runs sub elements")]
    public class BeginIfCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select type of If Command")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Value")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Variable Has Value")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Variable Is Numeric")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Window Name Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Active Window Name Is")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("File Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Folder Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Web Element Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("GUI Element Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error Occured")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error Did Not Occur")]
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary comparison type.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Value**, **Window Name Exists**, **Active Window Name Is**, **File Exists**, **Folder Exists**, **Web Element Exists**, **Error Occured**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_IfActionType { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Additional Parameters")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select the required comparison parameters.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            else if (v_IfActionType == "Variable Has Value")
            {
                string variableName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                  where rw.Field<string>("Parameter Name") == "Variable Name"
                                  select rw.Field<string>("Parameter Value")).FirstOrDefault());

                var actualVariable = variableName.ConvertToUserVariable(sender).Trim();

                if (!string.IsNullOrEmpty(actualVariable))
                {
                    ifResult = true;
                }
                else
                {
                    ifResult = false;
                }

            }
            else if (v_IfActionType == "Variable Is Numeric")
            {
                string variableName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Variable Name"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                var actualVariable = variableName.ConvertToUserVariable(sender).Trim();

                var numericTest = decimal.TryParse(actualVariable, out decimal parsedResult);

                if (numericTest)
                {
                    ifResult = true;
                }
                else
                {
                    ifResult = false;
                }

            }
            else if (v_IfActionType == "Error Occured")
            {
                //get line number
                string userLineNumber = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Line Number"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                //convert to variable
                string variableLineNumber = userLineNumber.ConvertToUserVariable(sender);

                //convert to int
                int lineNumber = int.Parse(variableLineNumber);

                //determine if error happened
                if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).Count() > 0)
                {

                    var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).FirstOrDefault();
                    error.ErrorMessage.StoreInUserVariable(sender, "Error.Message");
                    error.LineNumber.ToString().StoreInUserVariable(sender, "Error.Line");
                    error.StackTrace.StoreInUserVariable(sender, "Error.StackTrace");

                    ifResult = true;
                }
                else
                {
                    ifResult = false;
                }

            }
            else if (v_IfActionType == "Error Did Not Occur")
            {
                //get line number
                string userLineNumber = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Line Number"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                //convert to variable
                string variableLineNumber = userLineNumber.ConvertToUserVariable(sender);

                //convert to int
                int lineNumber = int.Parse(variableLineNumber);

                //determine if error happened
                if (engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).Count() == 0)
                {
                    ifResult = true;
                }
                else
                {
                    var error = engine.ErrorsOccured.Where(f => f.LineNumber == lineNumber).FirstOrDefault();
                    error.ErrorMessage.StoreInUserVariable(sender, "Error.Message");
                    error.LineNumber.ToString().StoreInUserVariable(sender, "Error.Line");
                    error.StackTrace.StoreInUserVariable(sender, "Error.StackTrace");

                    ifResult = false;
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
            else if (v_IfActionType == "Web Element Exists")
            {
                string parameterName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

                string searchMethod = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Element Search Method"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault());

                SeleniumBrowserElementActionCommand newElementActionCommand = new SeleniumBrowserElementActionCommand();
                newElementActionCommand.v_SeleniumSearchType = searchMethod;
                bool elementExists = newElementActionCommand.ElementExists(sender, searchMethod, parameterName);
                ifResult = elementExists;

            }
            else if (v_IfActionType == "GUI Element Exists")
            {
                string windowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Window Name"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

                string elementSearchParam = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));

                string elementSearchMethod = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                              where rw.Field<string>("Parameter Name") == "Element Search Method"
                                              select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertToUserVariable(sender));


                UIAutomationCommand newUIACommand = new UIAutomationCommand();
                newUIACommand.v_WindowName = windowName;
                newUIACommand.v_UIASearchParameters.Rows.Add(true, elementSearchMethod, elementSearchParam);
                var handle = newUIACommand.SearchForGUIElement(sender, windowName);

                if (handle is null)
                {
                    ifResult = false;
                }
                else
                {
                    ifResult = true;
                }
                

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

                case "Variable Has Value":
                    string variableName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                      where rw.Field<string>("Parameter Name") == "Variable Name"
                                      select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If (Variable " + variableName + " Has Value)";
                case "Variable Is Numeric":
                    string varName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Variable Name"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If (Variable " + varName + " Is Numeric)";

                case "Error Occured":

                    string lineNumber = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Line Number"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If (Error Occured on Line Number " + lineNumber + ")";
                case "Error Did Not Occur":

                    string lineNum = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Line Number"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    return "If (Error Did Not Occur on Line Number " + lineNum + ")";
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


                    return "If Web Element Exists [" + searchMethod + ": " + parameterName + "]";

                case "GUI Element Exists":


                    string guiWindowName = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Window Name"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault());

                    string guiSearch = ((from rw in v_IfActionParameterTable.AsEnumerable()
                                             where rw.Field<string>("Parameter Name") == "Element Search Parameter"
                                             select rw.Field<string>("Parameter Value")).FirstOrDefault());




                    return "If GUI Element Exists [Find " + guiSearch + " Element In " + guiWindowName + "]";


                default:

                    return "If .... ";
            }

        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("If Commands")]
    [Attributes.ClassAttributes.Description("This command signifies the exit point of If actions.  Required for all Begin Ifs.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to signify the exit point of your if scenario")]
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
    [Attributes.ClassAttributes.Description("This command declares the seperation between the actions based on the 'true' or 'false' condition.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to signify the exit point of your if scenario")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert an image into text.  You can then use additional commands to parse the data.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command has a dependency on and implements OneNote OCR to achieve automation.")]
    public class OCRCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Image to OCR")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the image file.")]
        [Attributes.PropertyAttributes.SampleUsage(@"**c:\temp\myimages.png")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FilePath { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply OCR Result To Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to take and save a screenshot.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements User32 CaptureWindow to achieve automation")]
    public class ScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Window name")]
        [Attributes.PropertyAttributes.InputSpecification("Input or Type the name of the window that you want to take a screenshot of.")]
        [Attributes.PropertyAttributes.SampleUsage("**Untitled - Notepad**")]
        [Attributes.PropertyAttributes.Remarks("")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to attempt to locate an image on screen.  You can subsequently take actions such as move the mouse to the location or perform a click.  This command generates a fingerprint from the comparison image and searches for it in on the desktop.")]
    [Attributes.ClassAttributes.ImplementationDescription("TBD")]
    public class ImageRecognitionCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Capture the search image")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowImageRecogitionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Use the tool to capture an image")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("The image will be used as the image to be found on screen.")]
        public string v_ImageCapture { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Offset X Coordinate - Optional")]
        [Attributes.PropertyAttributes.InputSpecification("Specify if an offset is required.")]
        [Attributes.PropertyAttributes.SampleUsage("0 or 100")]
        [Attributes.PropertyAttributes.Remarks("This will move the mouse X pixels to the right of the location of the image")]
        public int v_xOffsetAdjustment { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Offset Y Coordinate - Optional")]
        [Attributes.PropertyAttributes.InputSpecification("Specify if an offset is required.")]
        [Attributes.PropertyAttributes.SampleUsage("0 or 100")]
        [Attributes.PropertyAttributes.Remarks("This will move the mouse X pixels down from the top of the location of the image")]
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
        [Attributes.PropertyAttributes.InputSpecification("Indicate the type of click required")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Left Click**, **Middle Click**, **Right Click**, **Double Left Click**, **Left Down**, **Middle Down**, **Right Down**, **Left Up**, **Middle Up**, **Right Up** ")]
        [Attributes.PropertyAttributes.Remarks("You can simulate custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
        public string v_MouseClick { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Timeout (seconds, 0 for unlimited search time)")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a timeout length if required.")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("Search times become excessive for colors such as white. For best results, capture a large color variance on screen, not just a white block.")]
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
                                    v_XMousePosition = (topLeftX + (v_xOffsetAdjustment)).ToString(),
                                    v_YMousePosition = (topLeftY + (v_xOffsetAdjustment)).ToString(),
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

    #region API Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("API Commands")]
    [Attributes.ClassAttributes.Description("This command downloads the HTML source of a web page for parsing")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to retrieve HTML of a web page without using browser automation.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements System.Web to achieve automation")]
    public class HTTPRequestCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the URL")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a valid URL that you want to collect data from.")]
        [Attributes.PropertyAttributes.SampleUsage("http://mycompany.com/news or [vCompany]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WebRequestURL { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply Result To Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
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
    [Attributes.ClassAttributes.Group("API Commands")]
    [Attributes.ClassAttributes.Description("This command processes an HTML source object")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to parse and extract data from a successful **HTTP Request Command**")]
    [Attributes.ClassAttributes.ImplementationDescription("TBD")]
    public class HTTPQueryResultCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select variable containing HTML")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("XPath Query")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the XPath Query and the item will be extracted.")]
        [Attributes.PropertyAttributes.SampleUsage("@//*[@id=\"aso_search_form_anchor\"]/div/input")]
        [Attributes.PropertyAttributes.Remarks("You can use Chrome Dev Tools to click an element and copy the XPath.")]
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
      

        public ExecuteDLLCommand()
        {
            this.CommandName = "ExecuteDLLCommand";
            this.SelectionName = "Execute DLL";
            this.CommandEnabled = true;

            this.v_MethodParameters = new DataTable();
            this.v_MethodParameters.Columns.Add("Parameter Name");
            this.v_MethodParameters.Columns.Add("Parameter Value");
            this.v_MethodParameters.TableName = DateTime.Now.ToString("MethodParameterTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
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

            //get method
            MethodInfo m = t.GetMethod(methodName);

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
            if (returnType.Namespace != "System")
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

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Call Method '" + v_MethodName + "' in '" + v_ClassName + "']";
        }
    }
    #endregion

    #region Task Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("Task Commands")]
    [Attributes.ClassAttributes.Description("This command stops the current task.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to stop the current running task.")]
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
    [Attributes.ClassAttributes.Description("This command runs tasks.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run another task.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class RunTaskCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select a Task")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the valid path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("c:\\temp\\mytask.xml or [vScriptPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to write data to text files.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class WriteTextFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the text file.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfile.txt or [vTextFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the text to be written")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the text should be written to files.")]
        [Attributes.PropertyAttributes.SampleUsage("**[vText]** or **Hello World!**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_TextToWrite { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select overwrite option")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Append")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Overwrite")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate whether this command should append the text to or overwrite all existing text in the file")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Append** or **Overwrite**")]
        [Attributes.PropertyAttributes.Remarks("")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to read data from text files.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class ReadTextFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the text file.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfile.txt or [vTextFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define where the text should be stored")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command to move a file to a new destination.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class MoveFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate whether to move or copy the file")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Move File")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Copy File")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether you intend to move the file or copy the file.  Moving will remove the file from the original path while Copying will not.")]
        [Attributes.PropertyAttributes.SampleUsage("Select either **Move File** or **Copy File**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfile.txt or [vTextFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the directory to copy to")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the new path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\new path\\ or [vTextFolderPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Create folder if destination does not exist")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether the directory should be created if it does not already exist.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_CreateDirectory { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Delete file if it already exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Specify whether the file should be deleted first if it is already found to exist.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command to detete a file from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class DeleteFileCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfile.txt or [vTextFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
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
    [Attributes.ClassAttributes.Description("This command renames a file at a specified destination")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to rename an existing file.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class RenameFileCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the source file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfile.txt or [vTextFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the new file name (with extension)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify the new file name including the extension.")]
        [Attributes.PropertyAttributes.SampleUsage("newfile.txt or newfile.png")]
        [Attributes.PropertyAttributes.Remarks("Changing the file extension will not automatically convert files.")]
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
    [Attributes.ClassAttributes.UsesDescription("Use this command to wait for a file to exist before proceeding.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class WaitForFileToExistCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the directory of the file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfile.txt or [vTextFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FileName { get; set; }


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate how many seconds to wait for the file to exist")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify how long to wait before an error will occur because the file is not found.")]
        [Attributes.PropertyAttributes.SampleUsage("**10** or **20**")]
        [Attributes.PropertyAttributes.Remarks("")]
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

    #region Engine Commands
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to explicitly add a variable if you are not using **Set Variable* with the setting **Create Missing Variables** at runtime.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify the value of variables.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class AddVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the name of the variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If the variable exists, the value of the old variable will be replaced with the new one")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the input to be set to above variable")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the input that the variable's value should be set to.")]
        [Attributes.PropertyAttributes.SampleUsage("Hello or [vNum]+1")]
        [Attributes.PropertyAttributes.Remarks("You can use variables in input if you encase them within brackets [vName].  You can also perform basic math operations.")]
        public string v_Input { get; set; }
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Do Nothing If Variable Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error If Variable Exists")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Replace If Variable Exists")]
        [Attributes.PropertyAttributes.InputSpecification("Define the action to take if the variable already exists")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_IfExists { get; set; }
        public AddVariableCommand()
        {
            this.CommandName = "AddVariableCommand";
            this.SelectionName = "Add Variable";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Core.AutomationEngineInstance)sender;


            if (!engine.VariableList.Any(f => f.VariableName == v_userVariableName))
            {
                //variable does not exist so add to the list
                try
                {

                    var variableValue = v_Input.ConvertToUserVariable(engine);

                    engine.VariableList.Add(new Script.ScriptVariable
                    {
                        VariableName = v_userVariableName,
                        VariableValue = variableValue
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception("Encountered an error when adding variable '" + v_userVariableName + "': " + ex.ToString());
                }
            }
            else
            {
                //variable exists so decide what to do
                switch (v_IfExists)
                {
                    case "Replace If Variable Exists":
                        v_Input.ConvertToUserVariable(sender).StoreInUserVariable(engine, v_userVariableName);
                        break;
                    case "Error If Variable Exists":
                        throw new Exception("Attempted to create a variable that already exists! Use 'Set Variable' instead or change the Exception Setting in the 'Add Variable' Command.");
                    default:
                        break;
                }
               
            }

         
         

        }

      

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Assign '" + v_Input + "' to New Variable '" + v_userVariableName + "']";
        }
    }
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to modify variables.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify the value of variables.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    public class VariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a variable to modify")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define the input to be set to above variable")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the input that the variable's value should be set to.")]
        [Attributes.PropertyAttributes.SampleUsage("Hello or [vNum]+1")]
        [Attributes.PropertyAttributes.Remarks("You can use variables in input if you encase them within brackets [vName].  You can also perform basic math operations.")]
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
            if ((requiredVariable == null) && (v_userVariableName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_userVariableName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_userVariableName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
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
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command pauses the script for a set amount of time specified in milliseconds.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to pause your script for a specific amount of time.  After the specified time is finished, the script will resume execution.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class PauseCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Amount of time to pause for (in milliseconds).")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a specific amount of time in milliseconds (ex. to specify 8 seconds, one would enter 8000) or specify a variable containing a value.")]
        [Attributes.PropertyAttributes.SampleUsage("**8000** or **[vVariableWaitTime]**")]
        [Attributes.PropertyAttributes.Remarks("")]
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
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command specifies what to do  after an error is encountered.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to define how your script should behave when an error is encountered.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class ErrorHandlingCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Action On Error")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Stop Processing")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Continue Processing")]
        [Attributes.PropertyAttributes.InputSpecification("Select the action you want to take when you come across an error.")]
        [Attributes.PropertyAttributes.SampleUsage("**Stop Processing** to end the script if an error is encountered or **Continue Processing** to continue running the script")]
        [Attributes.PropertyAttributes.Remarks("**If Command** allows you to specify and test if a line number encountered an error. In order to use that functionality, you must specify **Continue Processing**")]
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
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to set delays between execution of commands in a running instance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to change the execution speed between commands.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SetEngineDelayCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Set Delay between commands (in milliseconds).")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a specific amount of time in milliseconds (ex. to specify 8 seconds, one would enter 8000) or specify a variable containing a value.")]
        [Attributes.PropertyAttributes.SampleUsage("**250** or **[vVariableSpeed]**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_EngineSpeed { get; set; }

        public SetEngineDelayCommand()
        {
            this.CommandName = "SetEngineDelayCommand";
            this.SelectionName = "Set Engine Delay";
            this.CommandEnabled = true;
            this.v_EngineSpeed = "250";
        }

      

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set Delay to " + v_EngineSpeed + "ms between commands]";
        }
    }
    #endregion



}







