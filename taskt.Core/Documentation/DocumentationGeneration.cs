using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;

namespace taskt.Core.Documentation
{
    /// <summary>
    /// This class generates markdown files for use in the official taskt wiki
    /// </summary>
    public class DocumentationGeneration
    {
        /// <summary>
        /// Returns a path that contains the generated markdown files
        /// </summary>
        /// <returns></returns>
        public string GenerateMarkdownFiles()
        {
            //create directory if required
            var docsFolderName = "docs";
            if (!Directory.Exists(docsFolderName))
            {
                Directory.CreateDirectory(docsFolderName);
            }

            //get all commands   
            var studioPath = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*Studio.exe").First();
            var commandClasses = Assembly.LoadFrom(studioPath).GetTypes()
                      .Where(t => t.Namespace == "taskt.Commands")
                      .Where(t => t.Name != "ScriptCommand")
                      .Where(t => t.IsAbstract == false)
                      .Where(t => t.BaseType.Name == "ScriptCommand")
                      .ToList();

            var cmdAssemblyPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*Commands.dll");
            foreach (var path in cmdAssemblyPaths)
            {
                commandClasses.AddRange(Assembly.LoadFrom(path).GetTypes()
                                 .Where(t => t.Namespace == "taskt.Commands")
                                 .Where(t => t.Name != "ScriptCommand")
                                 .Where(t => t.IsAbstract == false)
                                 .Where(t => t.BaseType.Name == "ScriptCommand")
                                 .ToList());
            }

            var highLevelCommandInfo = new List<CommandMetaData>();
            StringBuilder stringBuilder;
            string fullFileName;

            //loop each command
            foreach (var commandClass in commandClasses)
            {
                //instantiate and pull properties from command class
                ScriptCommand instantiatedCommand = (ScriptCommand)Activator.CreateInstance(commandClass);
                var groupName = GetClassValue(commandClass, typeof(Group));
                var classDescription = GetClassValue(commandClass, typeof(Description));
                var usesDescription = GetClassValue(commandClass, typeof(UsesDescription));
                var commandName = instantiatedCommand.SelectionName;

                stringBuilder = new StringBuilder();

                //create string builder to build markdown document and append data
                stringBuilder.AppendLine("<!--TITLE: " + commandName + " Command -->");
                stringBuilder.AppendLine("<!-- SUBTITLE: a command in the " + groupName + " group. -->");
                stringBuilder.AppendLine("[Go To Automation Commands Overview](/automation-commands)");

                stringBuilder.AppendLine(Environment.NewLine);
                stringBuilder.AppendLine("# " + commandName + " Command");
                stringBuilder.AppendLine(Environment.NewLine);

                //append more
                stringBuilder.AppendLine("## What does this command do?");
                stringBuilder.AppendLine(classDescription);
                stringBuilder.AppendLine(Environment.NewLine);

                //more
                stringBuilder.AppendLine("## When would I want to use this command?");
                stringBuilder.AppendLine(usesDescription);
                stringBuilder.AppendLine(Environment.NewLine);

                //build parameter table based on required user inputs
                stringBuilder.AppendLine("## Command Parameters");
                stringBuilder.AppendLine("| Parameter Question   	| What to input  	|  Sample Data 	| Remarks  	|");
                stringBuilder.AppendLine("| ---                    | ---               | ---           | ---       |");

                //loop each property
                foreach (var prop in commandClass.GetProperties().Where(f => f.Name.StartsWith("v_")).ToList())
                {
                    //pull attributes from property
                    var commandLabel = CleanMarkdownValue(GetPropertyValue(prop, typeof(PropertyDescription)));
                    var helpfulExplanation = CleanMarkdownValue(GetPropertyValue(prop, typeof(InputSpecification)));
                    var sampleUsage = CleanMarkdownValue(GetPropertyValue(prop, typeof(SampleUsage)));
                    var remarks = CleanMarkdownValue(GetPropertyValue(prop, typeof(Remarks)));

                    //append to parameter table
                    stringBuilder.AppendLine("|" + commandLabel + "|" + helpfulExplanation + "|" + sampleUsage + "|" + remarks + "|");
                }

                stringBuilder.AppendLine(Environment.NewLine);
                stringBuilder.AppendLine("## Developer/Additional Reference");
                stringBuilder.AppendLine("Automation Class Name: " + commandClass.Name);
                stringBuilder.AppendLine("Parent Namespace: " + commandClass.Namespace);
                stringBuilder.AppendLine("This page was generated on " + DateTime.Now.ToString("MM/dd/yy hh:mm tt"));
                stringBuilder.AppendLine(Environment.NewLine);
                stringBuilder.AppendLine("## Help");
                stringBuilder.AppendLine("[Open/Report an issue on GitHub](https://github.com/saucepleez/taskt/issues/new)");
                stringBuilder.AppendLine("[Ask a question on Gitter](https://gitter.im/taskt-rpa/Lobby)");

                //create kebob destination and command file name
                var kebobDestination = groupName.Replace(" ", "-").Replace("/", "-").ToLower();
                var kebobFileName = commandName.Replace(" ", "-").Replace("/", "-").ToLower() + "-command.md";

                //create directory if required
                var destinationdirectory = docsFolderName + "\\" + kebobDestination;
                if (!Directory.Exists(destinationdirectory))
                {
                    Directory.CreateDirectory(destinationdirectory);
                }

                //write file
                fullFileName = Path.Combine(destinationdirectory, kebobFileName);
                File.WriteAllText(fullFileName, stringBuilder.ToString());

                //add to high level
                var serverPath = "/automation-commands/" + kebobDestination + "/" + kebobFileName.Replace(".md", "");
                highLevelCommandInfo.Add(
                    new CommandMetaData()
                    {
                        Group = groupName,
                        Description = classDescription,
                        Name = commandName,
                        Location = serverPath
                    });
            }

            stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<!--TITLE: Automation Commands -->");
            stringBuilder.AppendLine("<!-- SUBTITLE: an overview of available commands in taskt. -->");
            stringBuilder.AppendLine("## Automation Commands");
            stringBuilder.AppendLine("| Command Group   	| Command Name 	|  Command Description	|");
            stringBuilder.AppendLine("| ---                | ---           | ---                   |");

            foreach (var cmd in highLevelCommandInfo)
            {
                stringBuilder.AppendLine("|" + cmd.Group + "|[" + cmd.Name + "](" + cmd.Location + ")|" + cmd.Description + "|");
            }

            stringBuilder.AppendLine("This page was generated on " + DateTime.Now.ToString("MM/dd/yy hh:mm tt"));
            stringBuilder.AppendLine(Environment.NewLine);
            stringBuilder.AppendLine("## Help");
            stringBuilder.AppendLine("[Open/Report an issue on GitHub](https://github.com/saucepleez/taskt/issues/new)");
            stringBuilder.AppendLine("[Ask a question on Gitter](https://gitter.im/taskt-rpa/Lobby)");

            //write file
            fullFileName = Path.Combine(docsFolderName, "automation-commands.md");
            File.WriteAllText(fullFileName, stringBuilder.ToString());

            return docsFolderName;
        }

        private string GetPropertyValue(PropertyInfo prop, Type attributeType)
        {
            var attribute = prop.GetCustomAttributes(attributeType, true);

            if (attribute.Length == 0)
            {
                return "Data not specified";
            }
            else
            {
                var attributeFound = attribute[0];

                if (attributeFound is PropertyDescription)
                {
                    var processedAttribute = (PropertyDescription)attributeFound;
                    return processedAttribute.Description;
                }
                else if (attributeFound is InputSpecification)
                {
                    var processedAttribute = (InputSpecification)attributeFound;
                    return processedAttribute.Specification;
                }
                else if (attributeFound is SampleUsage)
                {
                    var processedAttribute = (SampleUsage)attributeFound;
                    return processedAttribute.Usage;
                }
                else if (attributeFound is Remarks)
                {
                    var processedAttribute = (Remarks)attributeFound;
                    return processedAttribute.Remark;
                }
                else
                {
                    return "Attribute not supported";
                }
            }
        }

        private string CleanMarkdownValue(string value)
        {
            Dictionary<string, string> replacementDict = new Dictionary<string, string>
            {
                {"|", "\\|"},
                {"\n\t", "<br>"},
                {"\r\n", "<br>"}
            };
            foreach (var replacementTuple in replacementDict)
            {
                value = value.Replace(replacementTuple.Key, replacementTuple.Value);
            }

            return value;
        }

        private string GetClassValue(Type commandClass, Type attributeType)
        {
            var attribute = commandClass.GetCustomAttributes(attributeType, true);

            if (attribute.Length == 0)
            {
                return "Data not specified";
            }
            else
            {
                var attributeFound = attribute[0];

                if (attributeFound is Group)
                {
                    var processedAttribute = (Group)attributeFound;
                    return processedAttribute.Name;
                }
                else if (attributeFound is Description)
                {
                    var processedAttribute = (Description)attributeFound;
                    return processedAttribute.CommandFunctionalDescription;
                }
                else if (attributeFound is UsesDescription)
                {
                    var processedAttribute = (UsesDescription)attributeFound;
                    return processedAttribute.Description;
                }
            }

            //string groupAttribute = "";
            //if (attribute.Length > 0)
            //{
            //    var attributeFound = (attributeType)attribute[0];
            //    groupAttribute = attributeFound.groupName;
            //}

            return "OK";
        }
    }
}
