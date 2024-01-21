using System;
using System.Collections.Generic;
using taskt.Core.Automation.Commands;
using static taskt.Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType;

namespace taskt.Core
{
    public class InstanceCounter
    {
        private ApplicationSettings appSettings = null;

        // instance
        private Dictionary<string, Dictionary<string, int>> databaseInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> excelInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> ieInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> webBrowserInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> stopWatchInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> wordInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> nlgInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        // variable type
        private Dictionary<string, Dictionary<string, int>> dictionaryInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> dataTableInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> jsonInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> listInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> booleanInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> dateTimeInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> uiElementInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> colorInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> mailkitEMailInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> mailkitEMailListInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> webElementInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> windowHandleInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };
        private Dictionary<string, Dictionary<string, int>> numericInstance = new Dictionary<string, Dictionary<string, int>>
        {
            { "created", new Dictionary<string, int>() },
            { "used", new Dictionary<string, int>() }
        };

        public InstanceCounter(ApplicationSettings settings)
        {
            this.appSettings = settings;
        }

        public void addInstance(string instanceName, Automation.Attributes.PropertyAttributes.PropertyInstanceType instanceType, bool isUsed = false)
        {
            Dictionary<string, int> targetDic = decideDictionary(instanceType.instanceType, isUsed);

            if (string.IsNullOrEmpty(instanceName))
            {
                instanceName = "";
            }
            instanceName = instanceName.Trim();
            if (instanceName.Length == 0)
            {
                return;
            }

            if ((instanceType.autoWrapVariableMarker) && !(this.appSettings.EngineSettings.isWrappedVariableMarker(instanceName)))
            {
                //instanceName = this.appSettings.EngineSettings.wrapVariableMarker(instanceName);
                instanceName = VariableNameControls.GetWrappedVariableName(instanceName, appSettings);
            }
            
            if (targetDic.ContainsKey(instanceName))
            {
                targetDic[instanceName] = targetDic[instanceName] + 1;
            }
            else
            {
                targetDic.Add(instanceName, 1);
            }
        }

        public void removeInstance(string instanceName, Automation.Attributes.PropertyAttributes.PropertyInstanceType instanceType, bool isUsed = false)
        {
            Dictionary<string, int> targetDic = decideDictionary(instanceType.instanceType, isUsed);

            if (string.IsNullOrEmpty(instanceName))
            {
                instanceName = "";
            }
            instanceName = instanceName.Trim();
            if (instanceName.Length == 0)
            {
                return;
            }

            if ((instanceType.autoWrapVariableMarker) && !(this.appSettings.EngineSettings.isWrappedVariableMarker(instanceName)))
            {
                //instanceName = this.appSettings.EngineSettings.wrapVariableMarker(instanceName);
                instanceName = VariableNameControls.GetWrappedVariableName(instanceName, appSettings);
            }

            if (targetDic.ContainsKey(instanceName))
            {
                if (targetDic[instanceName] > 1)
                {
                    targetDic[instanceName] = targetDic[instanceName] - 1;
                }
                else
                {
                    targetDic.Remove(instanceName);
                }
            }
        }

        public Dictionary<string, int> getInstanceClone(InstanceType instanceType, bool isUsed = false)
        {
            Dictionary<string, int> targetDic = decideDictionary(instanceType, isUsed);

            return new Dictionary<string, int>(targetDic);
        }

        private Dictionary<string, int> decideDictionary(InstanceType instanceType, bool isUsed = false)
        {
            Dictionary<string, Dictionary<string, int>> targetDic;
            switch (instanceType)
            {
                case InstanceType.Boolean:
                    targetDic = booleanInstance;
                    break;
                case InstanceType.Color:
                    targetDic = colorInstance;
                    break;
                case InstanceType.DataBase:
                    targetDic = databaseInstance;
                    break;
                case InstanceType.DataTable:
                    targetDic = dataTableInstance;
                    break;
                case InstanceType.DateTime:
                    targetDic = dateTimeInstance;
                    break;
                case InstanceType.Dictionary:
                    targetDic = dictionaryInstance;
                    break;
                case InstanceType.Excel:
                    targetDic = excelInstance;
                    break;
                case InstanceType.IE:
                    targetDic = ieInstance;
                    break;
                case InstanceType.JSON:
                    targetDic = jsonInstance;
                    break;
                case InstanceType.List:
                    targetDic = listInstance; ;
                    break;
                case InstanceType.MailKitEMail:
                    targetDic = mailkitEMailInstance;
                    break;
                case InstanceType.MailKitEMailList:
                    targetDic = mailkitEMailListInstance;
                    break;
                case InstanceType.NLG:
                    targetDic = nlgInstance;
                    break;
                case InstanceType.Numeric:
                    targetDic = numericInstance;
                    break;
                case InstanceType.StopWatch:
                    targetDic = stopWatchInstance;
                    break;
                case InstanceType.UIElement:
                    targetDic = uiElementInstance;
                    break;
                case InstanceType.WebBrowser:
                    targetDic = webBrowserInstance;
                    break;
                case InstanceType.WebElement:
                    targetDic = webElementInstance;
                    break;
                case InstanceType.WindowHandle:
                    targetDic = windowHandleInstance;
                    break;
                case InstanceType.Word:
                    targetDic = wordInstance;
                    break;
                default:
                    return null;
            }
            return (isUsed) ? targetDic["used"] : targetDic["created"];
        }

        public static InstanceType GetInstanceType(string instanceType)
        {
            switch (instanceType.ToLower())
            {
                case "boolean":
                    return InstanceType.Boolean;
                case "color":
                    return InstanceType.Color;
                case "database":
                    return InstanceType.DataBase;
                case "datatable":
                    return InstanceType.DataTable;
                case "datetime":
                    return InstanceType.DateTime;
                case "dictionary":
                    return InstanceType.Dictionary;
                case "excel":
                    return InstanceType.Excel;
                case "ie":
                    return InstanceType.IE;
                case "json":
                    return InstanceType.JSON;
                case "list":
                    return InstanceType.List;
                case "mailkitemail":
                    return InstanceType.MailKitEMail;
                case "mailkitemaillist":
                    return InstanceType.MailKitEMailList;
                case "numeric":
                    return InstanceType.Numeric;
                case "stopwatch":
                    return InstanceType.StopWatch;
                case "uielement":
                    return InstanceType.UIElement;
                case "web browser":
                    return InstanceType.WebBrowser;
                case "webelement":
                    return InstanceType.WebElement;
                case "windowhandle":
                    return InstanceType.WindowHandle;
                case "word":
                    return InstanceType.Word;
                case "none":
                default:
                    return InstanceType.none;
            }
        }
    }
}
