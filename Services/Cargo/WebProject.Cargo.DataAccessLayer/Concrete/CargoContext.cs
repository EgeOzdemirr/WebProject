using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Cargo.EntityLayer.Concrete;

namespace WebProject.Cargo.DataAccessLayer.Concrete
{
    public class CargoContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=WebProjectCargoDb;User Id=sa;Password=123456aA*;TrustServerCertificate=True");
        }
        public DbSet<CargoCompany> CargoCompanies { get; set; }
        public DbSet<CargoDetail> CargoDetails { get; set; }
        public DbSet<CargoCustomer> CargoCustomers { get; set; }
        public DbSet<CargoOperation> CargoOperations { get; set; }
    }
}
