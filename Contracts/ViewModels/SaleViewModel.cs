using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.ViewModels
{
    public class SaleViewModel
    {
        public int Id { get; set; }
        [DisplayName("Дата")]
        public DateTime Date { get; set; }
        [DisplayName("Время")]
        public DateTime Time { get; set; }
        public int SalesPointId { get; set; }
        public int BuyerId { get; set; }
        public Dictionary<int, float> SalesData { get; set; }
        [DisplayName("Общая цена")]
        public float TotalAmount { get; set; }
    }
}
