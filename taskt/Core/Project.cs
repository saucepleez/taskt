using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace taskt.Core
{
    public class Project
    {
        [JsonProperty]
        private string ProjectName { get; set; }
        [JsonProperty]
        private string Main { get; set; }
        [JsonProperty]
        private List<string> ScriptPaths { get; set; }

        public Project()
        {

        }
        public Project(string projectName)
        {
            ScriptPaths = new List<string>();
            ProjectName = projectName;
            Main = "Main.xml";
        }

        public string GetProjectName()
        {
            return ProjectName;
        }

        public void SaveProject(string scriptPath, Script.Script script)
        {
            //Looks through sequential parent directories to find one that matches the script's ProjectName and contains a Main.xml
            string projectPath;
            string dirName;
            string mainScriptPath; 
            do
            {
                projectPath = Path.GetDirectoryName(scriptPath);
                DirectoryInfo dirInfo = new DirectoryInfo(projectPath);
                dirName = dirInfo.Name;
                mainScriptPath = Path.Combine(projectPath, "Main.xml");
                scriptPath = projectPath;

            } while (dirName != script.ProjectName || !File.Exists(mainScriptPath));

            //If requirements are met, a project.json is created/updated
            if (dirName == script.ProjectName && File.Exists(mainScriptPath))
            {
                List<string> updatedScriptPaths = new List<string>();
                List<string> scriptFullPaths = Directory.GetFiles(projectPath, ".", SearchOption.AllDirectories).ToList();
                foreach (string scriptFullPath in scriptFullPaths)
                {
                    string relativeScriptPath = scriptFullPath.Replace(projectPath, "");
                    if (relativeScriptPath.Contains(".xml"))
                        updatedScriptPaths.Add(relativeScriptPath);
                }
                ScriptPaths = updatedScriptPaths;
                
                string projectJSONFilePath = Path.Combine(projectPath, "project.json");
                File.WriteAllText(projectJSONFilePath, JsonConvert.SerializeObject(this));
            }
            else
            {
                throw new Exception("Project Directory Not Found");
            }
        }

        public static Project OpenProject(string mainFilePath)
        {
            //Gets project path and project.json from main script
            string projectPath = Path.GetDirectoryName(mainFilePath);
            string projectJSONPath = Path.Combine(projectPath, "project.json");

            //Loads project from project.json
            Project openProject = new Project();
            if (File.Exists(projectJSONPath))
            {
                string projectJSONString = File.ReadAllText(projectJSONPath);
                openProject = JsonConvert.DeserializeObject<Project>(projectJSONString);

                //checks if any scripts have been deleted
                List<string> updatedScriptPaths = new List<string>();
                List<string> scriptFullPaths = Directory.GetFiles(projectPath, ".", SearchOption.AllDirectories).ToList();
                foreach (string scriptFullPath in scriptFullPaths)
                {
                    string relativeScriptPath = scriptFullPath.Replace(projectPath, "");
                    if (relativeScriptPath.Contains(".xml"))
                        updatedScriptPaths.Add(relativeScriptPath);
                }
                openProject.ScriptPaths = updatedScriptPaths;

                //updates project.json
                File.WriteAllText(projectJSONPath, JsonConvert.SerializeObject(openProject));
                return openProject;
            }
            else
            {
                throw new Exception("project.json Not Found");
            }
        }
    }
}
