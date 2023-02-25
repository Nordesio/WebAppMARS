using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ViewModels
{
    public class ProductViewModel
    {
        
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Цена")]
        public decimal Price { get; set; }
    }
}
