using DatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseImplement
{
    public class SalesDatabase : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=SalesDatabase;Trusted_Connection=True;MultipleActiveResultSets=True");
            }
            base.OnConfiguring(optionsBuilder);
            
        }
        public virtual DbSet<Buyer> Buyers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProvidedProducts> ProvidedProducts { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SalesData> SalesDatas { get; set; }
        public virtual DbSet<SalesPoint> SalesPoints { get; set; }


    }
}
