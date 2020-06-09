using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace taskt.UI.DTOs
{
    public class Method
    {
        public string MethodName { get; set; }
        public List<Parameter> Parameters { get; set; } = new List<Parameter>();
    }
}
