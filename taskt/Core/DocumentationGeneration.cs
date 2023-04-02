using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using taskt.Core.Automation.Commands;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.UI.CustomControls;
using static taskt.Core.Automation.Commands.PropertyControls;
using Markdig;
using Microsoft.Office.Core;
using taskt.Core.Automation.Engine;

namespace taskt.Core
{
    /// <summary>
    /// This class generates markdown files for use in the official taskt wiki
    /// </summary>
    public static class DocumentationGeneration
     {
        /// <summary>
        /// to sort commands
        /// </summary>
        private class CommandMetaData
        {
            public string Group { get; set; }
            public string SubGroup { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Location { get; set; }
        }

        /// <summary>
        /// for convert md to html, contains table
        /// </summary>
        private static Markdig.MarkdownPipeline mdPipeline = new Markdig.MarkdownPipelineBuilder().UsePipeTables().Build();

        /// <summary>
        /// Returns a path that contains the generated markdown files
        /// </summary>
        /// <returns></returns>
        public static string GenerateMarkdownFiles()
        {
            var en = new CultureInfo("en-US");

            //create directory if required
            var docsFolderName = "docs";
            if (!System.IO.Directory.Exists(docsFolderName))
            {
                System.IO.Directory.CreateDirectory(docsFolderName);
            }

            //get all commands
            var commandClasses = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(t => t.Namespace == "taskt.Core.Automation.Commands")
                        .Where(t => t.Name != "ScriptCommand")
                        .Where(t => t.IsAbstract == false)
                        .Where(t => t.BaseType.Name == "ScriptCommand")
                        .ToList();

            // load settings
            var settings = new ApplicationSettings().GetOrCreateApplicationSettings();
            settings.ClientSettings.ShowSampleUsageInDescription = false;   // output trick
            settings.ClientSettings.ShowDefaultValueInDescription = false;

            // loop each command
            var highLevelCommandInfo = new List<CommandMetaData>();
            foreach (var commandClass in commandClasses)
            {
                highLevelCommandInfo.Add(GenerateMarkdownCommandFile(commandClass, settings, docsFolderName, en));
            }

            // output index
            var sb = new StringBuilder();
            sb.AppendLine("<!--TITLE: Automation Commands -->");
            sb.AppendLine("<!-- SUBTITLE: an overview of available commands in taskt. -->");
            sb.AppendLine("## Automation Commands");

            var sortHighLevelCommandInfo = highLevelCommandInfo
                                            .OrderBy(t => t.Group)
                                            .ThenBy(t => t.SubGroup)
                                            .ThenBy(t => t.Name)
                                            .ToList();

            string oldGroup = "";

            foreach (var cmd in sortHighLevelCommandInfo)
            {
                if (oldGroup != cmd.Group)
                {
                    sb.AppendLine("### " + cmd.Group);
                    sb.AppendLine("| Sub Group   	| Command Name 	|  Command Description	|");
                    sb.AppendLine("| ---                | ---           | ---                   |");
                    oldGroup = cmd.Group;
                }
                sb.AppendLine("|" + cmd.SubGroup + "|[" + cmd.Name + "](" + cmd.Location + ")|" + cmd.Description + "|");
            }

            sb.AppendLine(Environment.NewLine);

            sb.AppendLine("## Help");
            sb.AppendLine("- [Open/Report an issue on GitHub](" + MyURLs.GitIssueURL + ")");
            sb.AppendLine("- [Ask a question on Gitter](" + MyURLs.GitterURL + ")");

            sb.AppendLine(Environment.NewLine);

            sb.AppendLine("This page was generated on " + DateTime.Now.ToString("MM/dd/yy hh:mm tt", en));

            //write file
            string fullFileName = System.IO.Path.Combine(docsFolderName, "automation-commands.md");
            System.IO.File.WriteAllText(fullFileName, sb.ToString());

            return docsFolderName;
        }

        /// <summary>
        /// create and save Command Document file
        /// </summary>
        /// <param name="commandClass"></param>
        /// <param name="settings"></param>
        /// <param name="docsFolderName"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        private static CommandMetaData GenerateMarkdownCommandFile(Type commandClass, ApplicationSettings settings, string docsFolderName, CultureInfo cultureInfo)
        {
            ScriptCommand command = (ScriptCommand)Activator.CreateInstance(commandClass);
            var commandName = command.SelectionName;
            var cmdType = command.GetType();
            var groupName = cmdType.GetCustomAttribute<Automation.Attributes.ClassAttributes.Group>()?.groupName ?? "";
            var subGroupName = cmdType.GetCustomAttribute<Automation.Attributes.ClassAttributes.SubGruop>()?.subGruopName ?? "";

            var sb = new StringBuilder();

            //create string builder to build markdown document and append data
            sb.AppendLine("<!--TITLE: " + commandName + " Command -->");
            sb.AppendLine("<!-- SUBTITLE: a command in the " + groupName + " group. -->");

            // title
            sb.AppendLine("[Go To Automation Commands Overview](/automation-commands.md)");

            sb.AppendLine(Environment.NewLine);

            // bread crumb
            if (subGroupName != "")
            {
                sb.AppendLine(groupName + " &gt; " + subGroupName + " &gt; " + commandName);
            }
            else
            {
                sb.AppendLine(groupName + " &gt; " + commandName);
            }

            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("# " + commandName + " Command");
            sb.AppendLine(Environment.NewLine);

            //append more
            var classDescription = cmdType.GetCustomAttribute<Automation.Attributes.ClassAttributes.Description>()?.commandFunctionalDescription ?? "";
            sb.AppendLine("## What does this command do?");
            sb.AppendLine(classDescription);
            sb.AppendLine(Environment.NewLine);

            //more
            var usesDescription = cmdType.GetCustomAttribute<Automation.Attributes.ClassAttributes.UsesDescription>()?.usesDescription ?? "";
            sb.AppendLine("## When would I want to use this command?");
            sb.AppendLine(usesDescription);
            sb.AppendLine(Environment.NewLine);

            //build parameter table based on required user inputs
            sb.AppendLine("<a id=\"param_list\"></a>");
            sb.AppendLine("## Command Parameters");

            // get propertyLists
            List<PropertyInfo> propInfos = commandClass.GetProperties().Where(f => f.Name.StartsWith("v_")).ToList();

            // create Parameters List
            int maxCount = 0;
            List<PropertyInfo> vPropList = new List<PropertyInfo>();
            foreach(var prop in propInfos)
            {
                var virtualProp = prop.GetVirtualProperty();
                vPropList.Add(virtualProp);
                var commandLabel = CommandControls.GetLabelText(prop.Name, prop, settings, virtualProp);
                sb.AppendLine("- [" + commandLabel + "](#param_" + maxCount +  ")");
                maxCount++;
            }

            sb.AppendLine(Environment.NewLine);

            // add additional parameter info
            int count = 0;
            foreach (var prop in propInfos)
            {
                //var virtualProp = prop.GetVirtualProperty();
                var commandLabel = CommandControls.GetLabelText(prop.Name, prop, settings, vPropList[count]);

                sb.AppendLine("<a id=\"param_" + count + "\"></a>");
                sb.AppendLine("### " + commandLabel);
                
                sb.AppendLine(Environment.NewLine);

                // Input Specification, sample usage, remarks, etc.
                sb.AppendLine("<dl>");

                // what to input
                //var helpfulExplanation = settings.EngineSettings.replaceEngineKeyword(GetCustomAttributeWithVirtual<InputSpecification>(prop, vPropList[count])?.inputSpecification ?? "");
                //if (helpfulExplanation == "")
                //{
                //    helpfulExplanation = "(nothing)";
                //}
                var helpfulExplanation = GetInputSpecification(prop, vPropList[count], settings);
                sb.AppendLine("<dt>What to input</dt><dd>" + ConvertMDToHTML(helpfulExplanation) + "</dd>");

                // value instance type
                (var valueName, var valueDescription) = GetValueInstanceType(prop, vPropList[count]);
                if (valueName != "")
                {
                    sb.AppendLine("<dt>" + valueName + "</dt><dd>" + valueDescription + "</dd>");
                }

                // direction
                var direction = GetCustomAttributeWithVirtual<PropertyParameterDirection>(prop, vPropList[count]);
                if (direction != null)
                {
                    string desc = "";
                    switch (direction.porpose.ToString().ToLower())
                    {
                        case "input":
                            desc = "The Input Parameter for Executing the command";
                            break;
                        case "output":
                            desc = "The Parameter for Storing the Result of command execution";
                            break;
                        case "both":
                            desc = "The Input Parameter for Executing the command And also The Parameter for Storing the Result of command execution";
                            break;
                        default:
                            desc = "Unkown";
                            break;
                    }
                    sb.AppendLine("<dt>Parameter Direction</dt><dd>" + desc + "</dd>");
                }

                // error validation
                var validationRules = GetErrorValidation(prop, vPropList[count]);
                if (validationRules != "")
                {
                    sb.AppendLine("<dt>Error Occurs When the Value is ...</dt><dd>" + ConvertMDToHTML(validationRules) + "</dd>");
                }

                // FilePath setting
                // TODO: move to method
                var filePathSetting = GetCustomAttributeWithVirtual<PropertyFilePathSetting>(prop, vPropList[count]);
                if (filePathSetting != null)
                {
                    string fpDoc = "<dt>File Path Setting</dt><dd><ul>";
                    fpDoc += "<li>Allow URL: " + (filePathSetting.allowURL ? "Yes" : "No") + "</li>";
                    fpDoc += "<li>File Extension and Existance: ";
                    switch (filePathSetting.supportExtension)
                    {
                        case PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension:
                            fpDoc += "Extension <string>Not</string> Required, Existance <string>Not</string> Required";
                            break;
                        case PropertyFilePathSetting.ExtensionBehavior.RequiredExtension:
                            fpDoc += "Extension Required, Existance <string>Not</string> Required";
                            break;
                        case PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists:
                            fpDoc += "Extension Required, Existance Required";
                            break;
                    }
                    fpDoc += "</li>";
                    switch (filePathSetting.supportExtension)
                    {
                        case PropertyFilePathSetting.ExtensionBehavior.RequiredExtension:
                        case PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists:
                            fpDoc += "<li>Support Extensions: " + filePathSetting.extensions + "</li>";
                            break;
                    }
                    fpDoc += "<li>FileCounter Variable Support: ";
                    switch (filePathSetting.supportFileCounter)
                    {
                        case PropertyFilePathSetting.FileCounterBehavior.NoSupport:
                            fpDoc += "No Support";
                            break;
                        case PropertyFilePathSetting.FileCounterBehavior.FirstNotExists:
                            fpDoc += "Number **Not** Found First";
                            break;
                        case PropertyFilePathSetting.FileCounterBehavior.LastExists:
                            fpDoc += "Number Last Found";
                            break;
                    }
                    fpDoc += "</li></ul></dd>";
                    sb.AppendLine(fpDoc);
                }

                var sampleUsage = GetSampleUsageText(prop, vPropList[count], settings);
                sb.AppendLine("<dt>Sample Usage</dt><dd>" + ConvertMDToHTML(sampleUsage) + "</dd>");

                var remarks = GetRemarksText(prop, vPropList[count], settings);
                sb.AppendLine("<dt>Remarks</dt><dd>" + ConvertMDToHTML(remarks) + "</dd>");
                sb.AppendLine("</dl>");

                sb.AppendLine(Environment.NewLine);

                // Additional Parameter Info
                var paramInfos = GetCustomAttributesWithVirtual<PropertyAddtionalParameterInfo>(prop, vPropList[count]);
                if (paramInfos.Count > 0)
                {
                    sb.AppendLine("#### Addtional Info about &quot;" + commandLabel + "&quot;");
                    sb.AppendLine("| Parameter Value(s) | Description   | Sample Data 	| Remarks  	|");
                    sb.AppendLine("| ---             | ---           | ---          | ---       |");

                    foreach (var pInfo in paramInfos)
                    {
                        var searchKey = pInfo.searchKey.Replace("\t", " &amp; ");
                        var desc = pInfo.description;
                        var sample = settings.EngineSettings.replaceEngineKeyword(pInfo.sampleUsage);
                        var rmk = settings.EngineSettings.replaceEngineKeyword(pInfo.remarks);
                        sb.AppendLine("|" + searchKey + "|" + desc + "|" + sample + "|" + rmk + "|");
                    }
                }

                // Detail Sample Usage
                var sampleUsages = GetCustomAttributesWithVirtual<PropertyDetailSampleUsage>(prop, vPropList[count]);
                if (sampleUsages.Count > 0)
                {
                    sb.AppendLine(Environment.NewLine);
                    sb.AppendLine("#### More Detailed Sample Usage(s)");

                    //string sampleUsageTabe = "| Value | Means |\n|---|---|\n";

                    sb.AppendLine("| Value | Means |");
                    sb.AppendLine("|---|---|");
                    
                    foreach(var s in sampleUsages)
                    {
                        sb.AppendLine("| " + ConvertMDToHTML(settings.replaceApplicationKeyword(s.sampleUsage)) + " | " + GetSampleUsageMeansText(s, settings) + " |");
                        //sampleUsageTabe += "| " + ConvertMDToHTML(settings.replaceApplicationKeyword(s.sampleUsage)) + " | " + GetSampleUsageMeansText(s, settings) + " |\n";
                    }
                    //sb.AppendLine(ConvertMDToHTML(sampleUsageTabe));
                }

                sb.AppendLine(Environment.NewLine);
                count++;

                // nav
                sb.AppendLine("<div style=\"font-size: 90%; text-align: center\">");
                sb.AppendLine(Environment.NewLine);
                if (count == 1)
                {
                    sb.AppendLine("prev / [list](#param_list) / [next](#param_" + count + ")");
                }
                else if (count < maxCount)
                {
                    sb.AppendLine("[prev](#param_" + (count - 1) + ") / [list](#param_list) / [next](#param_" + count + ")");
                }
                else
                {
                    sb.AppendLine("[prev](#param_" + (count - 1) + ") / [list](#param_list) / next");
                }
                sb.AppendLine(Environment.NewLine);
                sb.AppendLine("</div>");
                sb.AppendLine(Environment.NewLine);
            }

            sb.AppendLine("## Developer/Additional Reference");
            sb.AppendLine("Automation Class Name: " + commandClass.Name);
            sb.AppendLine("Parent Namespace: " + commandClass.Namespace);
            sb.AppendLine("This page was generated on " + DateTime.Now.ToString("MM/dd/yy hh:mm tt", cultureInfo));

            sb.AppendLine(Environment.NewLine);

            sb.AppendLine("## Help");
            sb.AppendLine("- [Open/Report an issue on GitHub](" + MyURLs.GitIssueURL + ")");
            sb.AppendLine("- [Ask a question on Gitter](" + MyURLs.GitterURL + ")");

            //create kebob destination and command file nmae
            var kebobDestination = groupName.Replace(" ", "-").Replace("/", "-").ToLower();
            var kebobFileName = commandName.Replace(" ", "-").Replace("/", "-").ToLower() + "-command.md";

            //create directory if required
            var destinationdirectory = docsFolderName + "\\" + kebobDestination;
            if (!System.IO.Directory.Exists(destinationdirectory))
            {
                System.IO.Directory.CreateDirectory(destinationdirectory);
            }

            //write file
            var fullFileName = System.IO.Path.Combine(destinationdirectory, kebobFileName);
            System.IO.File.WriteAllText(fullFileName, sb.ToString());

            //add to high level
            var serverPath = "/" + kebobDestination + "/" + kebobFileName;
           
            return new CommandMetaData() { Group = groupName, SubGroup = subGroupName, Description = classDescription, Name = commandName, Location = serverPath };
        }

        private static string GetInputSpecification(PropertyInfo propInfo, PropertyInfo virtualProp, ApplicationSettings settings)
        {
            var attrInput = GetCustomAttributeWithVirtual<InputSpecification>(propInfo, virtualProp);

            if (attrInput != null)
            {
                var txt = settings.EngineSettings.replaceEngineKeyword(attrInput.inputSpecification);
                if (attrInput.autoGenerate)
                {
                    if (txt == "")
                    {
                        var uiOpts = GetCustomAttributesWithVirtual<PropertyUISelectionOption>(propInfo, virtualProp);
                        return ((uiOpts.Count > 0) ? "Select or Enter the One of the Options" : "(nothing)");
                    }
                    else
                    {
                        return "Enter or Select the " + txt;
                    }
                }
                else
                {
                    return ((txt == "") ? "(nothing)" : txt);
                }
            }
            else
            {
                return "(nothing)";
            }
        }

        private static (string dtName, string ddValue) GetValueInstanceType(PropertyInfo propInfo, PropertyInfo virtualPropInfo)
        {
            var uiSels = GetCustomAttributesWithVirtual<PropertyUISelectionOption>(propInfo, virtualPropInfo);
            if (uiSels.Count > 0)
            {
                var sensitive = GetCustomAttributeWithVirtual<PropertySelectionValueSensitive>(propInfo, virtualPropInfo) ?? new PropertySelectionValueSensitive();

                return ("Value", "Selection Values (Case Sensitive: " + (sensitive.caseSensitive ? "Yes" : "No") + ", Whilte-Space Sensitive: " + (sensitive.whiteSpaceSensitive ? "Yes" : "No") + ")");
            }
            var isWin = GetCustomAttributeWithVirtual<PropertyIsWindowNamesList>(propInfo, virtualPropInfo);
            if (isWin != null)
            {
                return ("Value", "Window Names");
            }
            var isVar = GetCustomAttributeWithVirtual<PropertyIsVariablesList>(propInfo, virtualPropInfo);
            var ins = GetCustomAttributeWithVirtual<PropertyInstanceType>(propInfo, virtualPropInfo);

            if ((isVar != null) && (ins != null))
            {
                return ("Value", ins.instanceType.ToString() + " Variable");
            }
            else
            {
                if (isVar != null)
                {
                    return ("Value", "Variables");
                }
                if (ins != null)
                {
                    return ("Instance Type", ins.instanceType.ToString());
                }
            }

            return ("", "");
        }

        private static string GetSampleUsageText(PropertyInfo propInfo, PropertyInfo virtualPropInfo, ApplicationSettings settings)
        {
            var smp = CommandControls.GetSampleUsageText(propInfo, settings, virtualPropInfo, false);

            if (smp == "")
            {
                var uiSels = GetCustomAttributesWithVirtual<PropertyUISelectionOption>(propInfo, virtualPropInfo);
                if (uiSels.Count > 0)
                {
                    foreach(var sel in uiSels)
                    {
                        smp += " **" + sel.uiOption + "** or ";
                    }
                    smp = smp.Trim();
                    return smp.Substring(0, smp.Length - 2);
                }
                else
                {
                    return "(nothing)";
                }
            }
            else
            {
                return smp;
            }
        }

        private static string GetSampleUsageMeansText(PropertyDetailSampleUsage smp, ApplicationSettings settings)
        {
            string ret = "";
            switch (smp.valueType)
            {
                case PropertyDetailSampleUsage.ValueType.Manual:
                    return smp.means;

                case PropertyDetailSampleUsage.ValueType.Value:
                    ret = "Specify " + smp.sampleUsage;
                    break;

                case PropertyDetailSampleUsage.ValueType.VariableValue:
                    string vName = settings.replaceApplicationKeyword(smp.sampleUsage).Replace(settings.EngineSettings.VariableStartMarker, "").Replace(settings.EngineSettings.VariableEndMarker, "");
                    ret = "Specify Value of Variable " + vName;
                    break;

                case PropertyDetailSampleUsage.ValueType.VariableName:
                    string vName2 = settings.replaceApplicationKeyword(smp.sampleUsage).Replace(settings.EngineSettings.VariableStartMarker, "").Replace(settings.EngineSettings.VariableEndMarker, "");
                    ret = "Specify Variable Name " + vName2;
                    break;
            }
            if (smp.target != "")
            {
                ret += " for " + smp.target;
            }
            return ret;
        }

        private static string GetErrorValidation(PropertyInfo propInfo, PropertyInfo virtualPropInfo)
        {
            string validate = "";

            // validation
            var valid = GetCustomAttributeWithVirtual<PropertyValidationRule>(propInfo, virtualPropInfo) ?? new PropertyValidationRule();
            var bet = GetCustomAttributeWithVirtual<PropertyValueRange>(propInfo, virtualPropInfo);
            if (valid.parameterName != "")
            {
                if (valid.errorRule != 0)
                {
                    if (valid.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.Empty))
                    {
                        validate += "- Empty\n";
                    }
                    if (valid.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.LessThanZero))
                    {
                        validate += "- Less than Zero\n";
                    }
                    if (valid.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.GreaterThanZero))
                    {
                        validate += "- Greater than Zero\n";
                    }
                    if (valid.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.EqualsZero))
                    {
                        validate += "- Equals Zero\n";
                    }
                    if (valid.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.NotEqualsZero))
                    {
                        validate += "- **Not** Equals Zero\n";
                    }
                    if (valid.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.NotSelectionOption))
                    {
                        validate += "- **Not** Selection Values\n";
                    }
                    if (valid.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.Between))
                    {
                        if (bet != null)
                        {
                            validate += "- Between " + bet.min + " to " + bet.max + "\n";
                        }
                        else
                        {
                            validate += "- Between *range defined*\n";
                        }
                    }
                    if (valid.IsErrorFlag(PropertyValidationRule.ValidationRuleFlags.NotBetween))
                    {
                        if (bet != null)
                        {
                            validate += "- **Not** Between " + bet.min + " to " + bet.max + "\n";
                        }
                        else
                        {
                            validate += "- **Not** Between *range defined*\n";
                        }
                    }
                }
            }
            return validate;
        }

        private static string GetRemarksText(PropertyInfo propInfo, PropertyInfo virtualPropInfo, ApplicationSettings settings)
        {
            string rm = settings.ClientSettings.replaceClientKeyword(GetCustomAttributeWithVirtual<Remarks>(propInfo, virtualPropInfo)?.remarks ?? "");

            // is optional
            var isOpt = GetCustomAttributeWithVirtual<PropertyIsOptional>(propInfo, virtualPropInfo) ?? new PropertyIsOptional();
            if (isOpt.isOptional)
            {
                if (rm != "")
                {
                    rm += "<br><br>\n";
                }
                rm += "**Optional**<br>";
                if (isOpt.setBlankToValue != "")
                {
                    rm += "Default Value is **" + isOpt.setBlankToValue + "**";
                }
                return rm;
            }
            return (rm != "") ? rm : "(nothing)";
        }

        private static string ConvertMDToHTML(string md)
        {
            var html = Markdig.Markdown.ToHtml(md, mdPipeline).Trim();
            if (html.StartsWith("<table>")) 
            {
                return html.Replace("\n", "");
            }
            else
            {
                return html.Replace("<p>", "").Replace("</p>", "");
            }
        }
     }
}
