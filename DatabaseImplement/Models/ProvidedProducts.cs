using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DatabaseImplement.Models
{
    public class ProvidedProducts
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Required]
        public int ProductQuntity { get; set; }
        public virtual Product Produdct {get;set;}


    }
}
