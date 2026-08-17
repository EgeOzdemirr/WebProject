using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Order.Domain.Entities;

namespace WebProject.Order.Persistence.Context
{
    public class OrderContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=WebProjectOrderDb;User Id=sa;Password=123456aA*;TrustServerCertificate=True");

        }
        public DbSet<Address> Adresses { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Ordering> Orderings { get; set; }

    }
}
