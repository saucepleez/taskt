using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.ML.Models
{
    public class TrainingRequest
    {
        public List<MultiClassInput> Dataset { get; set; } = new List<MultiClassInput>();
        public bool LoadAfterTraining { get; set; } = false;
        public bool ReturnFileData { get; set; } = false;
        public string ModelLocation { get; set; }
    }
}
