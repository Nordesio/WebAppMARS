using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Contracts.ViewModels
{
    public class BuyerViewModel
    {
        public int Id { get; set; }
        [DisplayName("ФИО")]
        public string Name { get; set; }
        public Dictionary<int, int> SalesIds { get; set; }
    }
}
