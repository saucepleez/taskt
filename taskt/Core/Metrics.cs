using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core
{
    public class Metrics
    {
        public List<ExecutionMetric> ExecutionMetricsSummary()
        {
            //create execution file path
            var filePath = System.IO.Path.Combine(Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.LogFolder), "taskt Execution Summary Logs.txt");

            //throw if file doesnt exist
            if (!System.IO.File.Exists(filePath))
            {
                throw new System.IO.FileNotFoundException("Execution Summary Log does not exist!");
            }

            //create list for sorting data
            var scriptsFinishedArgs = new List<Core.Automation.Engine.ScriptFinishedEventArgs>();
           
            //get all text from log file
            var logFileLines = System.IO.File.ReadAllLines(filePath);

            //loop each line from log file
            foreach (var line in logFileLines)
            {
                try
                {
                    //deserialize line
                    var deserializedLine = Newtonsoft.Json.JsonConvert.DeserializeObject(line) as Newtonsoft.Json.Linq.JToken;

                    //convert the logged data json
                    var scriptArgs = Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Automation.Engine.ScriptFinishedEventArgs>(deserializedLine["@mt"].ToString());
    
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
            var groupedTasks = scriptsFinishedArgs
                .GroupBy(f => f.FileName);
          





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
                    string fileName = new System.IO.FileInfo(fullFileName).Name;

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
            var filePath = System.IO.Path.Combine(Core.IO.Folders.GetFolder(Core.IO.Folders.FolderType.LogFolder), "taskt Execution Summary Logs.txt");
            System.IO.File.WriteAllText(filePath, string.Empty);
        }
    }

    public class ExecutionMetric
    {
       public string FileName { get; set; }
       public TimeSpan AverageExecutionTime { get; set; }
       public List<Core.Automation.Engine.ScriptFinishedEventArgs> ExecutionData { get; set; }
        
    }
}
