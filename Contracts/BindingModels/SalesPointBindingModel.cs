using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.BindingModels
{
    public class SalesPointBindingModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public Dictionary<int, (string, int)> ProvidedProducts { get; set; }
    }
}
