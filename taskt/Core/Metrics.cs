﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace taskt.Core
{
    public class Metrics
    {
        public List<ExecutionMetric> ExecutionMetricsSummary()
        {
            //create execution file path
            //var filePath = Path.Combine(IO.Folders.GetFolder(IO.Folders.FolderType.LogFolder), "taskt Execution Summary Logs.txt");
            var filePath = Path.Combine(IO.Folders.GetLogsFolderPath(), "taskt Execution Summary Logs.txt");

            //throw if file doesnt exist
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Execution Summary Log does not exist!");
            }

            //create list for sorting data
            var scriptsFinishedArgs = new List<Automation.Engine.ScriptFinishedEventArgs>();
           
            //get all text from log file
            var logFileLines = File.ReadAllLines(filePath);

            //loop each line from log file
            foreach (var line in logFileLines)
            {
                try
                {
                    //deserialize line
                    var deserializedLine = Newtonsoft.Json.JsonConvert.DeserializeObject(line) as Newtonsoft.Json.Linq.JToken;

                    //convert the logged data json
                    var scriptArgs = Newtonsoft.Json.JsonConvert.DeserializeObject<Automation.Engine.ScriptFinishedEventArgs>(deserializedLine["@mt"].ToString());
    
                    //add to tracking list
                    scriptsFinishedArgs.Add(scriptArgs);
                }
                catch (Exception)
                {
                    //notify user of some failures
                }
            }

            //create list to return
            var executionMetrics = new List<ExecutionMetric>();

            //group by file name and create execution time average
            var groupedTasks = scriptsFinishedArgs.GroupBy(f => f.FileName);
          
            //loop through each group
            foreach (var task in groupedTasks)
            {
                try
                {
                    //select recent 10
                    var filteredItems = task.OrderByDescending(f => f.LoggedOn).Take(10).ToList();

                    //get info around file
                    var fullFileName = task.FirstOrDefault().FileName;
                    string parentFolder = new DirectoryInfo(fullFileName).Parent.Name;
                    string fileName = new FileInfo(fullFileName).Name;

                    //create metric
                    var metric = new ExecutionMetric()
                    {
                        FileName = string.Join("\\", parentFolder, fileName),
                        AverageExecutionTime = new TimeSpan((long)filteredItems.Select(f => f.ExecutionTime.Ticks).Average()),
                        ExecutionData = filteredItems
                    };

                    //add metric to list
                    executionMetrics.Add(metric);
                }
                catch (Exception)
                {
                    //do nothing
                }
            }

            //return metric
            return executionMetrics;
        }

        public void ClearExecutionMetrics()
        {
            //var filePath = Path.Combine(IO.Folders.GetFolder(IO.Folders.FolderType.LogFolder), "taskt Execution Summary Logs.txt");
            var filePath = Path.Combine(IO.Folders.GetLogsFolderPath(), "taskt Execution Summary Logs.txt");
            File.WriteAllText(filePath, string.Empty);
        }
    }

    public class ExecutionMetric
    {
       public string FileName { get; set; }
       public TimeSpan AverageExecutionTime { get; set; }
       public List<Automation.Engine.ScriptFinishedEventArgs> ExecutionData { get; set; }
    }
}
