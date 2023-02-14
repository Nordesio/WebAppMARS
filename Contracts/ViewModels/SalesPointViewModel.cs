using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ViewModels
{
    public class SalesPointViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        public virtual Dictionary<int, int> ProvidedProducts { get; set; }
    }
}
