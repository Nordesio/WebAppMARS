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
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public int SalesPointId { get; set; }
        public int BuyerId { get; set; }
        public Dictionary<int, float> SalesData { get; set; }
        public float TotalAmount { get; set; }
    }
}
