using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        public InstanceCounter(ApplicationSettings settings)
        {
            this.appSettings = settings;
        }

        public void addInstance(string instanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType instanceType, bool isUsed = false)
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

        public void removeInstance(string instanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType instanceType, bool isUsed = false)
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

        public Dictionary<string, int> getInstanceClone(Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType instanceType, bool isUsed = false)
        {
            Dictionary<string, int> targetDic = decideDictionary(instanceType, isUsed);

            return new Dictionary<string, int>(targetDic);
        }

        private Dictionary<string, int> decideDictionary(Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType instanceType, bool isUsed = false)
        {
            Dictionary<string, Dictionary<string, int>> targetDic;
            switch (instanceType)
            {
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.AutomationElement:
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
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.NLG:
                    targetDic = nlgInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.StopWatch:
                    targetDic = stopWatchInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser:
                    targetDic = webBrowserInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word:
                    targetDic = wordInstance;
                    break;
                default:
                    return null;
                    break;
            }
            return (isUsed) ? targetDic["used"] : targetDic["created"];
        }

        //public void addInstance(Core.Automation.Commands.ScriptCommand command)
        //{
        //    //if (command is Core.Automation.Commands.DatabaseDefineConnectionCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.DatabaseDefineConnectionCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataBase);
        //    //}
        //    //else if (command is Core.Automation.Commands.ExcelCreateApplicationCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.ExcelCreateApplicationCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel);
        //    //}
        //    //else if (command is Core.Automation.Commands.IEBrowserCreateCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.IEBrowserCreateCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.IE);
        //    //}
        //    //else if (command is Core.Automation.Commands.NLGCreateInstanceCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.NLGCreateInstanceCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.NLG);
        //    //}
        //    //else if (command is Core.Automation.Commands.StopwatchCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.StopwatchCommand)command).v_StopwatchName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.StopWatch);
        //    //}
        //    //else if (command is Core.Automation.Commands.SeleniumBrowserCreateCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.SeleniumBrowserCreateCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);
        //    //}
        //    //else if (command is Core.Automation.Commands.WordCreateApplicationCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.WordCreateApplicationCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word);
        //    //}
        //    //else if (command is Core.Automation.Commands.CreateDictionaryCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.CreateDictionaryCommand)command).v_DictionaryName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary);
        //    //}
        //    //else if (command is Core.Automation.Commands.LoadDictionaryCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.LoadDictionaryCommand)command).v_DictionaryName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary);
        //    //}
        //    //else if (command is Core.Automation.Commands.CreateDataTableCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.CreateDataTableCommand)command).v_DataTableName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
        //    //}
        //    //else if (command is Core.Automation.Commands.FilterDataTableCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.FilterDataTableCommand)command).v_DataTableName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
        //    //    this.addInstance(((Core.Automation.Commands.FilterDataTableCommand)command).v_OutputDTName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
        //    //}
        //    //else if (command is Core.Automation.Commands.LoadDataTableCommand)
        //    //{
        //    //    this.addInstance(((Core.Automation.Commands.LoadDataTableCommand)command).v_DataSetName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
        //    //}

        //    //Type cmdType = command.GetType();
        //    var props = command.GetType().GetProperties();
        //    foreach(var prop in props)
        //    {
        //        if (prop.Name.StartsWith("v_") && (prop.Name != "v_Comment"))
        //        {
        //            if (prop.GetValue(command) != null)
        //            {
        //                string insValue = prop.GetValue(command).ToString();
        //                var insType = (Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType));
        //                var direction = (Core.Automation.Attributes.PropertyAttributes.PropertyParameterDirection)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyParameterDirection));
        //                if ((insType != null) && (direction != null) &&
        //                        (insType.instanceType != Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.none))
        //                {
        //                    this.addInstance(insValue, insType, (direction.porpose != Automation.Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output));
        //                    this.addInstance(insValue, insType, true);
        //                }
        //                else if ((insType != null) && (insType.instanceType != Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.none))
        //                {
        //                    this.addInstance(insValue, insType, true);
        //                }
        //            }
        //        }
        //    }
        //}

        //public void removeInstance(Core.Automation.Commands.ScriptCommand command)
        //{
        //    //if (command is Core.Automation.Commands.DatabaseDefineConnectionCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.DatabaseDefineConnectionCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataBase);
        //    //}
        //    //else if (command is Core.Automation.Commands.ExcelCreateApplicationCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.ExcelCreateApplicationCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel);
        //    //}
        //    //else if (command is Core.Automation.Commands.IEBrowserCreateCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.IEBrowserCreateCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.IE);
        //    //}
        //    //else if (command is Core.Automation.Commands.NLGCreateInstanceCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.NLGCreateInstanceCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.NLG);
        //    //}
        //    //else if (command is Core.Automation.Commands.StopwatchCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.StopwatchCommand)command).v_StopwatchName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.StopWatch);
        //    //}
        //    //else if (command is Core.Automation.Commands.SeleniumBrowserCreateCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.SeleniumBrowserCreateCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);
        //    //}
        //    //else if (command is Core.Automation.Commands.WordCreateApplicationCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.WordCreateApplicationCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word);
        //    //}
        //    //else if (command is Core.Automation.Commands.CreateDictionaryCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.CreateDictionaryCommand)command).v_DictionaryName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary);
        //    //}
        //    //else if (command is Core.Automation.Commands.LoadDictionaryCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.LoadDictionaryCommand)command).v_DictionaryName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary);
        //    //}
        //    //else if (command is Core.Automation.Commands.CreateDataTableCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.CreateDataTableCommand)command).v_DataTableName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
        //    //}
        //    //else if (command is Core.Automation.Commands.FilterDataTableCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.FilterDataTableCommand)command).v_DataTableName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
        //    //    this.removeInstance(((Core.Automation.Commands.FilterDataTableCommand)command).v_OutputDTName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
        //    //}
        //    //else if (command is Core.Automation.Commands.LoadDataTableCommand)
        //    //{
        //    //    this.removeInstance(((Core.Automation.Commands.LoadDataTableCommand)command).v_DataSetName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
        //    //}
        //    var props = command.GetType().GetProperties();
        //    foreach (var prop in props)
        //    {
        //        if (prop.Name.StartsWith("v_") && (prop.Name != "v_Comment"))
        //        {
        //            if (prop.GetValue(command) != null)
        //            {
        //                string insValue = prop.GetValue(command).ToString();
        //                var insType = (Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType));
        //                var direction = (Core.Automation.Attributes.PropertyAttributes.PropertyParameterDirection)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyParameterDirection));
        //                if ((insType != null) && (direction != null) &&
        //                        (insType.instanceType != Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.none))
        //                {
        //                    this.removeInstance(insValue, insType, (direction.porpose != Automation.Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output));
        //                    this.removeInstance(insValue, insType, true);
        //                }
        //                else if ((insType != null) && (insType.instanceType != Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.none))
        //                {
        //                    this.removeInstance(insValue, insType, true);
        //                }
        //            }
        //        }
        //    }
        //}

        public static Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType GetInstanceType(string instanceType)
        {
            switch (instanceType.ToLower())
            {
                case "automationelement":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.AutomationElement;
                    break;
                case "boolean":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Boolean;
                    break;
                case "color":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Color;
                    break;
                case "database":
                    return Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataBase;
                    break;
                case "datatable":
                    return Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable;
                    break;
                case "datetime":
                    return Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DateTime;
                    break;
                case "dictionary":
                    return Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary;
                    break;
                case "excel":
                    return Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel;
                    break;
                case "ie":
                    return Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.IE;
                    break;
                case "json":
                    return Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.JSON;
                    break;
                case "list":
                    return Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List;
                    break;
                case "stopwatch":
                    return Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.StopWatch;
                    break;
                case "web browser":
                    return Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser;
                    break;
                case "word":
                    return Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word;
                    break;
                case "none":
                default:
                    return Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.none;
                    break;
            }
        }
    }
}
