using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DatabaseImplement.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public virtual List<int> SalesIds { get; set; }
    }
}
