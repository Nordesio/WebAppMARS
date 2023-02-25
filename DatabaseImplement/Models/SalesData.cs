using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DatabaseImplement.Models
{
    public class SalesData
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SaleId { get; set; }
        [Required]
        public int ProductQuantity { get; set; }
        [Required]
        public decimal ProductIdAmount { get; set; }
        public Product Product { get; set; }
        public Sale Sale { get; set; }
    }
}
