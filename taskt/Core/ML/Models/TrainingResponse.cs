using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.ML.Models
{
   public class TrainingResponse
    {   
            public TrainingRequest Request { get; set; }
            public DateTime Started { get; set; }
            public DateTime Finished { get; set; }
            public string Result { get; set; }
            public string ModelName { get; set; }
            public string ModelData { get; set; }
    }
}
