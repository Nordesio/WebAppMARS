using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DatabaseImplement.Models
{
    public class SalesPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("SalesPointId")]
        public List<ProvidedProducts> ProvidedProducts { get; set; }
        [ForeignKey("SalesPointId")]
        public List<Sale> Sales { get; set; }

    }
}
