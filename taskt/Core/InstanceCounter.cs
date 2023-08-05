using System;
using System.Collections.Generic;

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
        private Dictionary<string, Dictionary<string, int>> automationElementInstance = new Dictionary<string, Dictionary<string, int>>
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

        public InstanceCounter(ApplicationSettings settings)
        {
            this.appSettings = settings;
        }

        public void addInstance(string instanceName, Automation.Attributes.PropertyAttributes.PropertyInstanceType instanceType, bool isUsed = false)
        {
            Dictionary<string, int> targetDic = decideDictionary(instanceType.instanceType, isUsed);

            if (String.IsNullOrEmpty(instanceName))
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
                instanceName = this.appSettings.EngineSettings.wrapVariableMarker(instanceName);
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

            if (String.IsNullOrEmpty(instanceName))
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
                instanceName = this.appSettings.EngineSettings.wrapVariableMarker(instanceName);
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

        public Dictionary<string, int> getInstanceClone(Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType instanceType, bool isUsed = false)
        {
            Dictionary<string, int> targetDic = decideDictionary(instanceType, isUsed);

            return new Dictionary<string, int>(targetDic);
        }

        private Dictionary<string, int> decideDictionary(Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType instanceType, bool isUsed = false)
        {
            Dictionary<string, Dictionary<string, int>> targetDic;
            switch (instanceType)
            {
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.UIElement:
                    targetDic = automationElementInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Boolean:
                    targetDic = booleanInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Color:
                    targetDic = colorInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataBase:
                    targetDic = databaseInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable:
                    targetDic = dataTableInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DateTime:
                    targetDic = dateTimeInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary:
                    targetDic = dictionaryInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel:
                    targetDic = excelInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.IE:
                    targetDic = ieInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.JSON:
                    targetDic = jsonInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List:
                    targetDic = listInstance; ;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.MailKitEMail:
                    targetDic = mailkitEMailInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.MailKitEMailList:
                    targetDic = mailkitEMailListInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.NLG:
                    targetDic = nlgInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.StopWatch:
                    targetDic = stopWatchInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser:
                    targetDic = webBrowserInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebElement:
                    targetDic = webElementInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word:
                    targetDic = wordInstance;
                    break;
                default:
                    return null;
            }
            return (isUsed) ? targetDic["used"] : targetDic["created"];
        }

        public static Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType GetInstanceType(string instanceType)
        {
            switch (instanceType.ToLower())
            {
                case "automationelement":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.UIElement;
                case "boolean":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Boolean;
                case "color":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Color;
                case "database":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataBase;
                case "datatable":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable;
                case "datetime":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DateTime;
                case "dictionary":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary;
                case "excel":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel;
                case "ie":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.IE;
                case "json":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.JSON;
                case "list":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List;
                case "mailkitemail":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.MailKitEMail;
                case "mailkitemaillist":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.MailKitEMailList;
                case "stopwatch":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.StopWatch;
                case "web browser":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser;
                case "webelement":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebElement;
                case "word":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word;
                case "none":
                default:
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.none;
            }
        }
    }
}
