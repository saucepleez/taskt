using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using taskt.Core.Automation.Attributes;
namespace taskt.Core
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

            //loop each command
            foreach (var commandClass in commandClasses)
            {
                //instantiate and pull properties from command class
                Core.Automation.Commands.ScriptCommand instantiatedCommand = (Core.Automation.Commands.ScriptCommand)Activator.CreateInstance(commandClass);
                var groupName = GetClassValue(commandClass, typeof(Core.Automation.Attributes.ClassAttributes.Group));
                var classDescription = GetClassValue(commandClass, typeof(Core.Automation.Attributes.ClassAttributes.Description));
                var usesDescription = GetClassValue(commandClass, typeof(Core.Automation.Attributes.ClassAttributes.UsesDescription));
                var commandName = instantiatedCommand.SelectionName;

                sb = new StringBuilder();

                //create string builder to build markdown document and append data
                sb.AppendLine("<!--TITLE: " + commandName + " Command -->");
                sb.AppendLine("<!-- SUBTITLE: a command in the " + groupName + " group. -->");

                sb.AppendLine("[Go To Automation Commands Overview](/automation-commands)");

                sb.AppendLine(Environment.NewLine);
                sb.AppendLine("# " + commandName + " Command");
                sb.AppendLine(Environment.NewLine);

                //append more
                sb.AppendLine("## What does this command do?");
                sb.AppendLine(classDescription);
                sb.AppendLine(Environment.NewLine);

                //more
                sb.AppendLine("## When would I want to use this command?");
                sb.AppendLine(usesDescription);
                sb.AppendLine(Environment.NewLine);

                //build parameter table based on required user inputs
                sb.AppendLine("## Command Parameters");
                sb.AppendLine("| Parameter Question   	| What to input  	|  Sample Data 	| Remarks  	|");
                sb.AppendLine("| ---                    | ---               | ---           | ---       |");

                //loop each property
                foreach (var prop in commandClass.GetProperties().Where(f => f.Name.StartsWith("v_")).ToList())
                {

                    //pull attributes from property
                    var commandLabel = GetPropertyValue(prop, typeof(Core.Automation.Attributes.PropertyAttributes.PropertyDescription));
                    var helpfulExplanation = GetPropertyValue(prop, typeof(Core.Automation.Attributes.PropertyAttributes.InputSpecification));
                    var sampleUsage = GetPropertyValue(prop, typeof(Core.Automation.Attributes.PropertyAttributes.SampleUsage));
                    var remarks = GetPropertyValue(prop, typeof(Core.Automation.Attributes.PropertyAttributes.Remarks));

                    //append to parameter table
                    sb.AppendLine("|" + commandLabel + "|" + helpfulExplanation + "|" + sampleUsage + "|" + remarks + "|");

                }

                sb.AppendLine(Environment.NewLine);




                sb.AppendLine("## Developer/Additional Reference");
                sb.AppendLine("Automation Class Name: " + commandClass.Name);
                sb.AppendLine("Parent Namespace: " + commandClass.Namespace);
                sb.AppendLine("This page was generated on " + DateTime.Now.ToString("MM/dd/yy hh:mm tt"));


                sb.AppendLine(Environment.NewLine);

                sb.AppendLine("## Help");
                sb.AppendLine("[Open/Report an issue on GitHub](https://github.com/saucepleez/taskt/issues/new)");
                sb.AppendLine("[Ask a question on Gitter](https://gitter.im/taskt-rpa/Lobby)");





              

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
                fullFileName = System.IO.Path.Combine(destinationdirectory, kebobFileName);
                System.IO.File.WriteAllText(fullFileName, sb.ToString());

                //add to high level
                var serverPath = "/automation-commands/" + kebobDestination + "/" + kebobFileName.Replace(".md", "");
                highLevelCommandInfo.Add(new CommandMetaData() { Group = groupName, Description = classDescription, Name = commandName, Location = serverPath });

            }

            sb = new StringBuilder();
            sb.AppendLine("<!--TITLE: Automation Commands -->");
            sb.AppendLine("<!-- SUBTITLE: an overview of available commands in taskt. -->");
            sb.AppendLine("## Automation Commands");
            sb.AppendLine("| Command Group   	| Command Name 	|  Command Description	|");
            sb.AppendLine("| ---                | ---           | ---                   |");


            foreach (var cmd in highLevelCommandInfo)
            {
                sb.AppendLine("|" + cmd.Group + "|[" + cmd.Name + "](" + cmd.Location + ")|" + cmd.Description + "|");
            }

            sb.AppendLine("This page was generated on " + DateTime.Now.ToString("MM/dd/yy hh:mm tt"));

            sb.AppendLine(Environment.NewLine);


            sb.AppendLine("## Help");
            sb.AppendLine("[Open/Report an issue on GitHub](https://github.com/saucepleez/taskt/issues/new)");
            sb.AppendLine("[Ask a question on Gitter](https://gitter.im/taskt-rpa/Lobby)");

            //write file
            fullFileName = System.IO.Path.Combine(docsFolderName, "automation-commands.md");
            System.IO.File.WriteAllText(fullFileName, sb.ToString());


            return docsFolderName;

        }
        public string GetPropertyValue(PropertyInfo prop, Type attributeType)
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
        public string GetClassValue(Type commandClass, Type attributeType)
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

            //string groupAttribute = "";
            //if (attribute.Length > 0)
            //{
            //    var attributeFound = (attributeType)attribute[0];
            //    groupAttribute = attributeFound.groupName;
            //}

            return "OK";

        }

    }

    public class CommandMetaData
    {
        public string Group { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }

}
