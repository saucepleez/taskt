using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.Core.ML.Models
{
    public class MultiClassInput
    {
        [ColumnName("Statement")]
        public string Statement { get; set; }
        [ColumnName("Label")]
        public string Label { get; set; }
    }
}
