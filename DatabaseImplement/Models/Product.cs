using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DatabaseImplement.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("ProductId")]
        public virtual List<ProvidedProducts> ProvidedProducts { get; set; }
        [ForeignKey("ProductId")]
        public virtual List<SalesData> SalesDatas { get; set; }

    }
}
