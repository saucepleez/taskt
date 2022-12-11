using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using taskt.Core.Automation.Commands;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.UI.CustomControls;

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

            // release
            commandClasses.Clear();
            commandClasses = null;
            highLevelCommandInfo.Clear();
            highLevelCommandInfo = null;
            sortHighLevelCommandInfo.Clear();
            sortHighLevelCommandInfo = null;

            return docsFolderName;
        }

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
            sb.AppendLine("## Command Parameters");

            // get propertyLists
            List<PropertyInfo> propInfos = commandClass.GetProperties().Where(f => f.Name.StartsWith("v_")).ToList();

            // create Parameters List
            int count = 0;
            foreach(var prop in propInfos)
            {
                var commandLabel = CommandControls.GetLabelText(prop.Name, prop, settings);
                sb.AppendLine("- [" + commandLabel + "](#param_" + count +  ")");
                count++;
            }

            sb.AppendLine(Environment.NewLine);

            // add additional parameter info
            count = 0;
            foreach (var prop in propInfos)
            {
                var commandLabel = CommandControls.GetLabelText(prop.Name, prop, settings);

                sb.AppendLine("<a id=\"param_" + count + "\"></a>");
                sb.AppendLine("### " + commandLabel);

                var helpfulExplanation = settings.EngineSettings.replaceEngineKeyword(prop.GetCustomAttribute<InputSpecification>()?.inputSpecification ?? "");
                var sampleUsage = settings.EngineSettings.replaceEngineKeyword(prop.GetCustomAttribute<SampleUsage>()?.sampleUsage ?? "");
                var remarks = settings.EngineSettings.replaceEngineKeyword(prop.GetCustomAttribute<Remarks>()?.remarks ?? "");


                if (helpfulExplanation == "")
                {
                    helpfulExplanation = "(nothing)";
                }
                //if (sampleUsage == "")
                //{
                //    var uiSels = prop.GetCustomAttributes<PropertyUISelectionOption>().ToList();
                //    if (uiSels.Count > 0)
                //    {

                //    }
                //    else
                //    {
                //        sampleUsage = "(nothing)";
                //    }
                //}
                sampleUsage = CorrectionSampleUsage(sampleUsage, prop);

                //var isOpt = prop.GetCustomAttribute<PropertyIsOptional>() ?? new PropertyIsOptional();
                //if (isOpt.isOptional)
                //{
                //    remarks += "<strong>Optional</strong><br>";
                //    if (isOpt.setBlankToValue != "")
                //    {
                //        remarks += "Default Value is " + isOpt.setBlankToValue;
                //    }
                //}
                //if (remarks == "")
                //{
                //    remarks = "(nothing)";
                //}
                remarks = CorrectionRemarks(remarks, prop);

                sb.AppendLine(Environment.NewLine);

                sb.AppendLine("<dl>");
                sb.AppendLine("<dt>What to input</dt><dd>" + ConvertMDToHTML(helpfulExplanation) + "</dd>");
                sb.AppendLine("<dt>Sample Data</dt><dd>" + ConvertMDToHTML(sampleUsage) + "</dd>");
                sb.AppendLine("<dt>Remarks</dt><dd>" + ConvertMDToHTML(remarks) + "</dd>");
                sb.AppendLine("</dl>");

                sb.AppendLine(Environment.NewLine);

                var paramInfos = prop.GetCustomAttributes<PropertyAddtionalParameterInfo>().ToList();
                if (paramInfos.Count > 0)
                {
                    sb.AppendLine("### Addtional Info about &quot;" + commandLabel + "&quot;");
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

                sb.AppendLine(Environment.NewLine);
                count++;
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

        private static string CorrectionSampleUsage(string smp, PropertyInfo propInfo)
        {
            if (smp == "")
            {
                var uiSels = propInfo.GetCustomAttributes<PropertyUISelectionOption>().ToList();
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

        private static string CorrectionRemarks(string rm, PropertyInfo propInfo)
        {
            if (rm == "")
            {
                var isOpt = propInfo.GetCustomAttribute<PropertyIsOptional>() ?? new PropertyIsOptional();
                if (isOpt.isOptional)
                {
                    rm += "**Optional**<br>";
                    if (isOpt.setBlankToValue != "")
                    {
                        rm += "Default Value is **" + isOpt.setBlankToValue + "**";
                    }
                    return rm;
                }
                else
                {
                    return "(nothing)";
                }
            }
            else
            {
                return rm;
            }
        }

        private static string ConvertMDToHTML(string md)
        {
            var html = Markdig.Markdown.ToHtml(md);
            if (html.StartsWith("<p>"))
            {
                html = html.Trim();
                return html.Substring(3, html.Length - 7);
            }
            else
            {
                return html;
            }
        }
     }
}
