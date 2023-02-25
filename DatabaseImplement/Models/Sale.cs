using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DatabaseImplement.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int SalesPointId { get; set; }
        public int? BuyerId { get; set; }
        [ForeignKey("SaleId")]
        public virtual List<SalesData> SalesData { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
