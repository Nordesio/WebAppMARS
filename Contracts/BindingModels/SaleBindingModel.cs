using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.BindingModels
{
    public class SaleBindingModel
    {
        public int? Id { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int SalesPointId { get; set; }
        public int? BuyerId { get; set; }
        public Dictionary<int, (string, int, decimal)> SalesData { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
