using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    public class InstanceCounter
    {
        private Dictionary<string, int> databaseInstance = new Dictionary<string, int>();
        private Dictionary<string, int> excelInstance = new Dictionary<string, int>();
        private Dictionary<string, int> ieInstance = new Dictionary<string, int>();
        private Dictionary<string, int> webBrowserInstance = new Dictionary<string, int>();
        private Dictionary<string, int> stopWatchInstance = new Dictionary<string, int>();
        private Dictionary<string, int> wordInstance = new Dictionary<string, int>();
        private Dictionary<string, int> nlgInstance = new Dictionary<string, int>();
        private Dictionary<string, int> dictionaryInstance = new Dictionary<string, int>();
        private Dictionary<string, int> dataTableInstance = new Dictionary<string, int>();

        public InstanceCounter()
        {

        }

        private void addInstance(string instanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType instanceType)
        {
            Dictionary<string, int> targetDic = decideDictionary(instanceType);

            instanceName = instanceName.Trim();
            if (instanceName.Length == 0)
            {
                return;
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
        
        public void addInstance(InstanceNameType nameType)
        {
            this.addInstance(nameType.Name, nameType.InstanceType);
        }

        private void removeInstance(string instanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType instanceType)
        {
            Dictionary<string, int> targetDic = decideDictionary(instanceType);

            instanceName = instanceName.Trim();
            if (instanceName.Length == 0)
            {
                return;
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

        public string[] getInstances(Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType instanceType)
        {
            Dictionary<string, int> targetDic = decideDictionary(instanceType);

            return targetDic.Keys.ToArray();
        }

        private Dictionary<string, int> decideDictionary(Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType instanceType)
        {
            switch (instanceType)
            {
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataBase:
                    return databaseInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable:
                    return dataTableInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary:
                    return dictionaryInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel:
                    return excelInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.IE:
                    return ieInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.NLG:
                    return nlgInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.StopWatch:
                    return stopWatchInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser:
                    return webBrowserInstance;
                    break;
                case Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word:
                    return wordInstance;
                    break;
                default:
                    return null;
                    break;
            }
        }

        public void addInstance(Core.Automation.Commands.ScriptCommand command)
        {
            if (command is Core.Automation.Commands.DatabaseDefineConnectionCommand)
            {
                this.addInstance(((Core.Automation.Commands.DatabaseDefineConnectionCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataBase);
            }
            else if (command is Core.Automation.Commands.ExcelCreateApplicationCommand)
            {
                this.addInstance(((Core.Automation.Commands.ExcelCreateApplicationCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel);
            }
            else if (command is Core.Automation.Commands.IEBrowserCreateCommand)
            {
                this.addInstance(((Core.Automation.Commands.IEBrowserCreateCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.IE);
            }
            else if (command is Core.Automation.Commands.NLGCreateInstanceCommand)
            {
                this.addInstance(((Core.Automation.Commands.NLGCreateInstanceCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.NLG);
            }
            else if (command is Core.Automation.Commands.StopwatchCommand)
            {
                this.addInstance(((Core.Automation.Commands.StopwatchCommand)command).v_StopwatchName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.StopWatch);
            }
            else if (command is Core.Automation.Commands.SeleniumBrowserCreateCommand)
            {
                this.addInstance(((Core.Automation.Commands.SeleniumBrowserCreateCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);
            }
            else if (command is Core.Automation.Commands.WordCreateApplicationCommand)
            {
                this.addInstance(((Core.Automation.Commands.WordCreateApplicationCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word);
            }
            else if (command is Core.Automation.Commands.CreateDictionaryCommand)
            {
                this.addInstance(((Core.Automation.Commands.CreateDictionaryCommand)command).v_DictionaryName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary);
            }
            else if (command is Core.Automation.Commands.LoadDictionaryCommand)
            {
                this.addInstance(((Core.Automation.Commands.LoadDictionaryCommand)command).v_DictionaryName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary);
            }
            else if (command is Core.Automation.Commands.CreateDataTableCommand)
            {
                this.addInstance(((Core.Automation.Commands.CreateDataTableCommand)command).v_DataTableName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
            }
            else if (command is Core.Automation.Commands.FilterDataTableCommand)
            {
                this.addInstance(((Core.Automation.Commands.FilterDataTableCommand)command).v_DataTableName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
                this.addInstance(((Core.Automation.Commands.FilterDataTableCommand)command).v_OutputDTName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
            }
            else if (command is Core.Automation.Commands.LoadDataTableCommand)
            {
                this.addInstance(((Core.Automation.Commands.LoadDataTableCommand)command).v_DataSetName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
            }
        }

        public void removeInstance(Core.Automation.Commands.ScriptCommand command)
        {
            if (command is Core.Automation.Commands.DatabaseDefineConnectionCommand)
            {
                this.removeInstance(((Core.Automation.Commands.DatabaseDefineConnectionCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataBase);
            }
            else if (command is Core.Automation.Commands.ExcelCreateApplicationCommand)
            {
                this.removeInstance(((Core.Automation.Commands.ExcelCreateApplicationCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel);
            }
            else if (command is Core.Automation.Commands.IEBrowserCreateCommand)
            {
                this.removeInstance(((Core.Automation.Commands.IEBrowserCreateCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.IE);
            }
            else if (command is Core.Automation.Commands.NLGCreateInstanceCommand)
            {
                this.removeInstance(((Core.Automation.Commands.NLGCreateInstanceCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.NLG);
            }
            else if (command is Core.Automation.Commands.StopwatchCommand)
            {
                this.removeInstance(((Core.Automation.Commands.StopwatchCommand)command).v_StopwatchName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.StopWatch);
            }
            else if (command is Core.Automation.Commands.SeleniumBrowserCreateCommand)
            {
                this.removeInstance(((Core.Automation.Commands.SeleniumBrowserCreateCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);
            }
            else if (command is Core.Automation.Commands.WordCreateApplicationCommand)
            {
                this.removeInstance(((Core.Automation.Commands.WordCreateApplicationCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word);
            }
            else if (command is Core.Automation.Commands.CreateDictionaryCommand)
            {
                this.removeInstance(((Core.Automation.Commands.CreateDictionaryCommand)command).v_DictionaryName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary);
            }
            else if (command is Core.Automation.Commands.LoadDictionaryCommand)
            {
                this.removeInstance(((Core.Automation.Commands.LoadDictionaryCommand)command).v_DictionaryName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary);
            }
            else if (command is Core.Automation.Commands.CreateDataTableCommand)
            {
                this.removeInstance(((Core.Automation.Commands.CreateDataTableCommand)command).v_DataTableName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
            }
            else if (command is Core.Automation.Commands.FilterDataTableCommand)
            {
                this.removeInstance(((Core.Automation.Commands.FilterDataTableCommand)command).v_DataTableName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
                this.removeInstance(((Core.Automation.Commands.FilterDataTableCommand)command).v_OutputDTName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
            }
            else if (command is Core.Automation.Commands.LoadDataTableCommand)
            {
                this.removeInstance(((Core.Automation.Commands.LoadDataTableCommand)command).v_DataSetName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
            }
        }

        public InstanceNameType getInstanceNameType(Core.Automation.Commands.ScriptCommand command)
        {
            if (command is Core.Automation.Commands.DatabaseDefineConnectionCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.DatabaseDefineConnectionCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataBase);
            }
            else if (command is Core.Automation.Commands.ExcelCreateApplicationCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.ExcelCreateApplicationCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel);
            }
            else if (command is Core.Automation.Commands.IEBrowserCreateCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.IEBrowserCreateCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.IE);
            }
            else if (command is Core.Automation.Commands.NLGCreateInstanceCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.NLGCreateInstanceCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.NLG);
            }
            else if (command is Core.Automation.Commands.StopwatchCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.StopwatchCommand)command).v_StopwatchName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.StopWatch);
            }
            else if (command is Core.Automation.Commands.SeleniumBrowserCreateCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.SeleniumBrowserCreateCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.WebBrowser);
            }
            else if (command is Core.Automation.Commands.WordCreateApplicationCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.WordCreateApplicationCommand)command).v_InstanceName, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Word);
            }
            else if (command is Core.Automation.Commands.CreateDictionaryCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.CreateDictionaryCommand)command).v_DictionaryName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary);
            }
            else if (command is Core.Automation.Commands.LoadDictionaryCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.LoadDictionaryCommand)command).v_DictionaryName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Dictionary);
            }
            else if (command is Core.Automation.Commands.CreateDataTableCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.CreateDataTableCommand)command).v_DataTableName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
            }
            else if (command is Core.Automation.Commands.FilterDataTableCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.CreateDataTableCommand)command).v_DataTableName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
            }
            else if (command is Core.Automation.Commands.LoadDataTableCommand)
            {
                return new InstanceNameType(((Core.Automation.Commands.LoadDataTableCommand)command).v_DataSetName, Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.DataTable);
            }
            else
            {
                return null;
            }
        }
    }

    public class InstanceNameType
    {
        public string Name { get; private set; }
        public Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType InstanceType { get; private set; }

        public InstanceNameType(string name, Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType type)
        {
            this.Name = name;
            this.InstanceType = type;
        }
    }
}
