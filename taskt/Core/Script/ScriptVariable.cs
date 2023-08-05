using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using OpenQA.Selenium;

namespace taskt.Core.Script
{
    [Serializable]
    public class ScriptVariable
    {
        /// <summary>
        /// name that will be used to identify the variable
        /// </summary>
        public string VariableName { get; set; }
        /// <summary>
        /// index/position tracking for complex variables (list)
        /// </summary>
        [XmlIgnore]
        public int CurrentPosition = 0;
        /// <summary>
        /// value of the variable or current index
        /// </summary>
        public object VariableValue { get; set; }
        /// <summary>
        /// retrieve value of the variable
        /// </summary>
        public string GetDisplayValue(string requiredProperty = "")
        {

            if (VariableValue is string)
            {
                switch (requiredProperty)
                {
                    case "type":
                    case "Type":
                    case "TYPE":
                        return "BASIC";
                    default:
                        return (string)VariableValue;
                }

            }
            else if (VariableValue is DataTable dataTable)
            {
                return GetDisplayValue(dataTable, requiredProperty);
            }
            else if (VariableValue is Dictionary<string, string> trgDic)
            {
                return GetDisplayValue(trgDic, requiredProperty);
            }
            else if (VariableValue is List<string> requiredValue)
            {
                return GetDisplayValue(requiredValue, requiredProperty);
            }
            else if (VariableValue is System.Windows.Automation.AutomationElement elem)
            {
                return GetDisplayValue(elem, requiredProperty);
            }
            else if (VariableValue is DateTime dt)
            {
                return GetDisplayValue(dt, requiredProperty);
            }
            else if (VariableValue is System.Drawing.Color co)
            {
                return GetDisplayValue(co, requiredProperty);
            }
            else if (VariableValue is MimeKit.MimeMessage mail)
            {
                return GetDisplayValue(mail, requiredProperty);
            }
            else if (VariableValue is List<MimeKit.MimeMessage> list)
            {
                return GetDisplayValue(list, requiredProperty);
            }
            else if (VariableValue is IWebElement webElem)
            {
                return GetDisplayValue(webElem, requiredProperty);
            }
            else
            {
                return "UNKNOWN";
            }
        }
        private string GetDisplayValue(DataTable myDT, string requiredProperty = "")
        {
            switch (requiredProperty)
            {
                case "rows":
                case "Rows":
                case "ROWS":
                    return myDT.Rows.ToString();
                case "cols":
                case "Cols":
                case "COLS":
                case "columns":
                case "Columns":
                case "COLUMNS":
                    return myDT.Columns.ToString();
                case "type":
                case "Type":
                case "TYPE":
                    return "DATATABLE";
                case "index":
                case "Index":
                case "INDEX":
                    return CurrentPosition.ToString();
                default:
                    if (requiredProperty == "")
                    {
                        var dataRow = myDT.Rows[CurrentPosition];
                        return Newtonsoft.Json.JsonConvert.SerializeObject(dataRow.ItemArray);
                    }
                    else
                    {
                        if (int.TryParse(requiredProperty, out int idx))
                        {
                            return myDT.Rows[CurrentPosition][idx].ToString();

                        }
                        else
                        {
                            return myDT.Rows[CurrentPosition][requiredProperty].ToString();
                        }
                    }
            }
        }

        private string GetDisplayValue(Dictionary<string, string> myDic, string requiredProperty)
        {
            Dictionary<string, string> trgDic = (Dictionary<string, string>)VariableValue;
            switch (requiredProperty)
            {
                case "count":
                case "Count":
                case "COUNT":
                    return trgDic.Values.Count.ToString();
                case "type":
                case "Type":
                case "TYPE":
                    return "DICTIONARY";
                case "index":
                case "Index":
                case "INDEX":
                    return CurrentPosition.ToString();
                default:
                    if (requiredProperty == "")
                    {
                        return (trgDic.Values.ToArray())[CurrentPosition];
                    }
                    else
                    {
                        if (int.TryParse(requiredProperty, out int idx))
                        {
                            return (trgDic.Values.ToArray())[idx];
                        }
                        else
                        {
                            return trgDic[requiredProperty];
                        }
                    }
            }
        }

        private string GetDisplayValue(List<string> myList, string requiredProperty)
        {
            switch (requiredProperty)
            {
                case "count":
                case "Count":
                case "COUNT":
                    return myList.Count.ToString();
                case "index":
                case "Index":
                case "INDEX":
                    return CurrentPosition.ToString();
                case "tojson":
                case "ToJson":
                case "toJson":
                case "TOJSON":
                    return Newtonsoft.Json.JsonConvert.SerializeObject(myList);
                case "topipe":
                case "ToPipe":
                case "toPipe":
                case "TOPIPE":
                    return String.Join("|", myList);
                case "first":
                case "First":
                case "FIRST":
                    return myList.FirstOrDefault();
                case "last":
                case "Last":
                case "LAST":
                    return myList.Last();
                case "type":
                case "Type":
                case "TYPE":
                    return "LIST";
                default:
                    return myList[CurrentPosition];
            }
        }
        private string GetDisplayValue(System.Windows.Automation.AutomationElement element, string requiredProperty)
        {
            switch (requiredProperty)
            {
                case "type":
                case "Type":
                case "TYPE":
                    return "AUTOMATIONELEMENT";
                default:
                    return "Name: " + element.Current.Name + ", LocalizedControlType: " + element.Current.LocalizedControlType + ", ControlType: " + taskt.Core.Automation.Commands.UIElementControls.GetControlTypeText(element.Current.ControlType);
            }
        }
        private string GetDisplayValue(DateTime dt, string requiredProperty)
        {
            switch (requiredProperty)
            {
                case "type":
                case "Type":
                case "TYPE":
                    return "DATETIME";
                default:
                    return dt.ToString();
            }
        }

        private string GetDisplayValue(System.Drawing.Color co, string requiredProperty)
        {
            switch (requiredProperty)
            {
                case "type":
                case "Type":
                case "TYPE":
                    return "COLOR";
                case "red":
                case "Red":
                case "RED":
                    return co.R.ToString();
                case "green":
                case "Green":
                case "GREEN":
                    return co.G.ToString();
                case "blue":
                case "Blue":
                case "BLUE":
                    return co.B.ToString();
                case "alpha":
                case "Alpha":
                case "ALPHA":
                    return co.A.ToString();
                case "hex":
                case "Hex":
                case "HEX":
                    return String.Format("{0:X2}{1:X2}{2:X2}", co.R, co.G, co.B);
                default:
                    return co.ToString();
            }
        }

        private string GetDisplayValue(MimeKit.MimeMessage mail, string requiredProperty)
        {
            switch (requiredProperty)
            {
                case "type":
                case "Type":
                case "TYPE":
                    return "MAILKIT_EMAIL";
                case "subject":
                case "Subject":
                case "SUBJECT":
                    return mail.Subject;
                case "body":
                case "Body":
                case "BODY":
                    return mail.TextBody;
                case "to":
                case "To":
                case "TO":
                    return mail.To.ToString();
                case "cc":
                case "Cc":
                case "CC":
                    return mail.Cc.ToString();
                case "bcc":
                case "Bcc":
                case "BCC":
                    return mail.Bcc.ToString();
                case "from":
                case "From":
                case "FROM":
                    return mail.From.ToString();
                case "id":
                case "Id":
                case "ID":
                    return mail.MessageId;
                default:
                    string mes = "";
                    if (mail.TextBody != null)
                    {
                        mes = mail.TextBody;
                    }
                    else if (mail.HtmlBody != null)
                    {
                        mes = mail.HtmlBody;
                    }
                    return "Subject: " + mail.Subject + ", Message: " + ((mes.Length > 100) ? mes.Substring(0, 100) : mes);
            }
        }
        private string GetDisplayValue(List<MimeKit.MimeMessage> mails, string requiredProperty)
        {
            switch (requiredProperty)
            {
                case "count":
                case "Count":
                case "COUNT":
                    return mails.Count.ToString();
                case "index":
                case "Index":
                case "INDEX":
                    return CurrentPosition.ToString();
                case "type":
                case "Type":
                case "TYPE":
                    return "MAILKIT_EMAILLIST";
                default:
                    return "Index: " + CurrentPosition + ", " + GetDisplayValue(mails[CurrentPosition], "");
            }
        }

        private string GetDisplayValue(IWebElement elem, string requiredProperty)
        {
            switch(requiredProperty)
            {
                case "type":
                case "Type":
                case "TYPE":
                    return "WEBELEMENT";
                default:
                    return elem.ToString();
            }
        }
    }
}
