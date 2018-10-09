using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using taskt.Core.AutomationCommands.Attributes;
namespace taskt.Core
{
 
 public class DocumentationGeneration
    {
     public void GenerateMarkdownFiles()
     {

            //get all commands
            var commandClasses = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == "taskt.Core.AutomationCommands")
                      .Where(t => t.Name != "ScriptCommand")
                      .Where(t => t.IsAbstract == false)
                      .Where(t => t.BaseType.Name == "ScriptCommand")
                      .ToList();

            //loop each command
            foreach (var commandClass in commandClasses)
            {
                //instantiate and pull properties from command class
                Core.AutomationCommands.ScriptCommand instantiatedCommand = (Core.AutomationCommands.ScriptCommand)Activator.CreateInstance(commandClass);
                var groupName = GetClassValue(commandClass, typeof(Core.AutomationCommands.Attributes.ClassAttributes.Group));
                var classDescription = GetClassValue(commandClass, typeof(Core.AutomationCommands.Attributes.ClassAttributes.Description));
                var usesDescription = GetClassValue(commandClass, typeof(Core.AutomationCommands.Attributes.ClassAttributes.UsesDescription));
                var commandName = instantiatedCommand.SelectionName;

                //create string builder to build markdown document and append data
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<!--TITLE: " + commandName + " Command -->");
                sb.AppendLine("<!-- SUBTITLE: a command in the " + groupName + " group -->");
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
                    var commandLabel = GetPropertyValue(prop, typeof(Core.AutomationCommands.Attributes.PropertyAttributes.PropertyDescription));
                    var helpfulExplanation = GetPropertyValue(prop, typeof(Core.AutomationCommands.Attributes.PropertyAttributes.HelpfulExplanation));
                    var sampleUsage = GetPropertyValue(prop, typeof(Core.AutomationCommands.Attributes.PropertyAttributes.SampleUsage));
                    var remarks = GetPropertyValue(prop, typeof(Core.AutomationCommands.Attributes.PropertyAttributes.Remarks));

                    //append to parameter table
                    sb.AppendLine("|" + commandLabel + "|" + helpfulExplanation + "|" + sampleUsage + "|" + remarks + "|");

                }

                sb.AppendLine(Environment.NewLine);

                //create directory if required
                var docsFolderName = "docs";
                if (!System.IO.Directory.Exists(docsFolderName))
                {
                    System.IO.Directory.CreateDirectory(docsFolderName);
                }

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


            }



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

                if (attributeFound is Core.AutomationCommands.Attributes.PropertyAttributes.PropertyDescription)
                {
                    var processedAttribute = (Core.AutomationCommands.Attributes.PropertyAttributes.PropertyDescription)attributeFound;
                    return processedAttribute.propertyDescription;
                }
                else if (attributeFound is Core.AutomationCommands.Attributes.PropertyAttributes.HelpfulExplanation)
                {
                    var processedAttribute = (Core.AutomationCommands.Attributes.PropertyAttributes.HelpfulExplanation)attributeFound;
                    return processedAttribute.helpfulExplanation;
                }
                else if (attributeFound is Core.AutomationCommands.Attributes.PropertyAttributes.SampleUsage)
                {
                    var processedAttribute = (Core.AutomationCommands.Attributes.PropertyAttributes.SampleUsage)attributeFound;
                    return processedAttribute.sampleUsage;
                }
                else if (attributeFound is Core.AutomationCommands.Attributes.PropertyAttributes.Remarks)
                {
                    var processedAttribute = (Core.AutomationCommands.Attributes.PropertyAttributes.Remarks)attributeFound;
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


                if (attributeFound is Core.AutomationCommands.Attributes.ClassAttributes.Group)
                {
                    var processedAttribute = (Core.AutomationCommands.Attributes.ClassAttributes.Group)attributeFound;
                    return processedAttribute.groupName;
                }
                else if (attributeFound is Core.AutomationCommands.Attributes.ClassAttributes.Description)
                {
                    var processedAttribute = (Core.AutomationCommands.Attributes.ClassAttributes.Description)attributeFound;
                    return processedAttribute.commandFunctionalDescription;
                }
                else if (attributeFound is Core.AutomationCommands.Attributes.ClassAttributes.UsesDescription)
                {
                    var processedAttribute = (Core.AutomationCommands.Attributes.ClassAttributes.UsesDescription)attributeFound;
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
}
