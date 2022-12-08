using ADODB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using taskt.Core.Automation.Attributes;
using taskt.Core.Automation.Commands;

namespace taskt.Core
{
     /// <summary>
     /// This class generates markdown files for use in the official taskt wiki
     /// </summary>
     public static class DocumentationGeneration
     {
        private class CommandMetaData
        {
            public string Group { get; set; }
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
            var en = new System.Globalization.CultureInfo("en-US");

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


            var highLevelCommandInfo = new List<CommandMetaData>();
            StringBuilder sb;
            string fullFileName;

            var settings = new Core.ApplicationSettings().GetOrCreateApplicationSettings();
            settings.ClientSettings.ShowSampleUsageInDescription = false;   // output trick

            //loop each command
            foreach (var commandClass in commandClasses)
            {
                highLevelCommandInfo.Add(GenerateMarkdownCommandFile(commandClass, settings, docsFolderName, en));
            }

            sb = new StringBuilder();
            sb.AppendLine("<!--TITLE: Automation Commands -->");
            sb.AppendLine("<!-- SUBTITLE: an overview of available commands in taskt. -->");
            sb.AppendLine("## Automation Commands");

            var sortHighLevelCommandInfo = highLevelCommandInfo
                                            .OrderBy(t => t.Group)
                                            .ThenBy(t => t.Name)
                                            .ToList();

            string oldGroup = "";

            foreach (var cmd in sortHighLevelCommandInfo)
            {
                if (oldGroup != cmd.Group)
                {
                    sb.AppendLine("### " + cmd.Group);
                    sb.AppendLine("| Command Group   	| Command Name 	|  Command Description	|");
                    sb.AppendLine("| ---                | ---           | ---                   |");
                    oldGroup = cmd.Group;
                }
                sb.AppendLine("|" + cmd.Group + "|[" + cmd.Name + "](" + cmd.Location + ")|" + cmd.Description + "|");
            }

            sb.AppendLine("This page was generated on " + DateTime.Now.ToString("MM/dd/yy hh:mm tt", en));

            sb.AppendLine(Environment.NewLine);


            sb.AppendLine("## Help");
            sb.AppendLine("- [Open/Report an issue on GitHub](" + MyURLs.GitIssueURL + ")");
            sb.AppendLine("- [Ask a question on Gitter](" + MyURLs.GitterURL + ")");

            //write file
            fullFileName = System.IO.Path.Combine(docsFolderName, "automation-commands.md");
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
            var groupName = cmdType.GetCustomAttribute<taskt.Core.Automation.Attributes.ClassAttributes.Group>()?.groupName ?? "";
            var subGroupName = cmdType.GetCustomAttribute<taskt.Core.Automation.Attributes.ClassAttributes.SubGruop>()?.subGruopName ?? "";

            var sb = new StringBuilder();

            //create string builder to build markdown document and append data
            sb.AppendLine("<!--TITLE: " + commandName + " Command -->");
            sb.AppendLine("<!-- SUBTITLE: a command in the " + groupName + " group. -->");

            sb.AppendLine("[Go To Automation Commands Overview](/automation-commands.md)");

            sb.AppendLine(Environment.NewLine);
            sb.AppendLine("# " + commandName + " Command");
            sb.AppendLine(Environment.NewLine);

            //append more
            var classDescription = cmdType.GetCustomAttribute<taskt.Core.Automation.Attributes.ClassAttributes.Description>()?.commandFunctionalDescription ?? "";
            sb.AppendLine("## What does this command do?");
            sb.AppendLine(classDescription);
            sb.AppendLine(Environment.NewLine);

            //more
            var usesDescription = cmdType.GetCustomAttribute<taskt.Core.Automation.Attributes.ClassAttributes.UsesDescription>()?.usesDescription ?? "";
            sb.AppendLine("## When would I want to use this command?");
            sb.AppendLine(usesDescription);
            sb.AppendLine(Environment.NewLine);

            //build parameter table based on required user inputs
            sb.AppendLine("## Command Parameters");
            sb.AppendLine("| Parameter Question   	| What to input  	|  Sample Data 	| Remarks  	|");
            sb.AppendLine("| ---                    | ---               | ---           | ---       |");

            // get propertyLists
            List<PropertyInfo> propInfos = commandClass.GetProperties().Where(f => f.Name.StartsWith("v_")).ToList();

            //loop each property
            foreach (var prop in propInfos)
            {
                var commandLabel = UI.CustomControls.CommandControls.GetLabelText(prop.Name, prop, settings);

                var helpfulExplanation = settings.EngineSettings.replaceEngineKeyword(GetPropertyValue(prop, typeof(Core.Automation.Attributes.PropertyAttributes.InputSpecification)));
                var sampleUsage = settings.EngineSettings.replaceEngineKeyword(GetPropertyValue(prop, typeof(Core.Automation.Attributes.PropertyAttributes.SampleUsage)));

                var remarks = settings.EngineSettings.replaceEngineKeyword(GetPropertyValue(prop, typeof(Core.Automation.Attributes.PropertyAttributes.Remarks)));
                remarks = remarks.Replace("\n", "<br>");

                //append to parameter table
                sb.AppendLine("|" + commandLabel + "|" + helpfulExplanation + "|" + sampleUsage + "|" + remarks + "|");
            }

            sb.AppendLine(Environment.NewLine);

            // add additional parameter info
            foreach (var prop in propInfos)
            {
                var commandLabel = settings.EngineSettings.replaceEngineKeyword(GetPropertyValue(prop, typeof(Core.Automation.Attributes.PropertyAttributes.PropertyDescription)));
                var paramInfos = prop.GetCustomAttributes(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyAddtionalParameterInfo), true);
                if (paramInfos.Length > 0)
                {
                    sb.AppendLine("### Addtional Info about &quot;" + commandLabel + "&quot;");
                    sb.AppendLine("| Parameter Value(s) | Description   | Sample Data 	| Remarks  	|");
                    sb.AppendLine("| ---             | ---           | ---          | ---       |");

                    foreach (Core.Automation.Attributes.PropertyAttributes.PropertyAddtionalParameterInfo pInfo in paramInfos)
                    {
                        var searchKey = pInfo.searchKey.Replace("\t", " &amp; ");
                        var desc = pInfo.description;
                        var sample = settings.EngineSettings.replaceEngineKeyword(pInfo.sampleUsage);
                        var remarks = settings.EngineSettings.replaceEngineKeyword(pInfo.remarks);
                        sb.AppendLine("|" + searchKey + "|" + desc + "|" + sample + "|" + remarks + "|");
                    }
                }

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
            //var serverPath = "/automation-commands/" + kebobDestination + "/" + kebobFileName.Replace(".md", "");
            var serverPath = "/" + kebobDestination + "/" + kebobFileName;
           
            return new CommandMetaData() { Group = groupName, Description = classDescription, Name = commandName, Location = serverPath };
        }

        public static string GetPropertyValue(PropertyInfo prop, Type attributeType)
        {
            var attribute = prop.GetCustomAttributes(attributeType, true);

            if (attribute.Length == 0)
            {
                return "Data not specified";
            }
            else
            {
                var attributeFound = attribute[0];

                if (attributeFound is Core.Automation.Attributes.PropertyAttributes.PropertyDescription)
                {
                    var processedAttribute = (Core.Automation.Attributes.PropertyAttributes.PropertyDescription)attributeFound;
                    return processedAttribute.propertyDescription;
                }
                else if (attributeFound is Core.Automation.Attributes.PropertyAttributes.InputSpecification)
                {
                    var processedAttribute = (Core.Automation.Attributes.PropertyAttributes.InputSpecification)attributeFound;
                    return processedAttribute.inputSpecification;
                }
                else if (attributeFound is Core.Automation.Attributes.PropertyAttributes.SampleUsage)
                {
                    var processedAttribute = (Core.Automation.Attributes.PropertyAttributes.SampleUsage)attributeFound;
                    return processedAttribute.sampleUsage;
                }
                else if (attributeFound is Core.Automation.Attributes.PropertyAttributes.Remarks)
                {
                    var processedAttribute = (Core.Automation.Attributes.PropertyAttributes.Remarks)attributeFound;
                    return processedAttribute.remarks;
                }
                else
                {
                    return "Attribute not supported";
                }
            }

        }
        public static string GetClassValue(Type commandClass, Type attributeType)
        {
            var attribute = commandClass.GetCustomAttributes(attributeType, true);

            if (attribute.Length == 0)
            {
                return "Data not specified";
            }
            else
            {
                var attributeFound = attribute[0];


                if (attributeFound is Core.Automation.Attributes.ClassAttributes.Group)
                {
                    var processedAttribute = (Core.Automation.Attributes.ClassAttributes.Group)attributeFound;
                    return processedAttribute.groupName;
                }
                else if (attributeFound is Core.Automation.Attributes.ClassAttributes.Description)
                {
                    var processedAttribute = (Core.Automation.Attributes.ClassAttributes.Description)attributeFound;
                    return processedAttribute.commandFunctionalDescription;
                }
                else if (attributeFound is Core.Automation.Attributes.ClassAttributes.UsesDescription)
                {
                    var processedAttribute = (Core.Automation.Attributes.ClassAttributes.UsesDescription)attributeFound;
                    return processedAttribute.usesDescription;
                }
            }
            return "OK";
        }
     }
}
