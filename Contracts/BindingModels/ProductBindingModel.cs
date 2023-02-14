using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.BindingModels
{
    public class ProductBindingModel
    {
        public int? Id { get; set; }
        
        public string Name { get; set; }
        public float Price { get; set; }
    }
}
