using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.ML.Models
{
   public class ScoringRequest
    {
        public ITransformer Model { get; set; }
        public string Input { get; set; }
    }
}
