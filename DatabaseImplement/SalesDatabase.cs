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
                optionsBuilder.UseSqlServer(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=SalesDatabase;Integrated Security=True;
MultipleActiveResultSets=True;");
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
