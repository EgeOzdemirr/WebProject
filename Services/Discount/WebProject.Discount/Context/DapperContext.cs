using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebProject.Discount.Entities;

namespace WebProject.Discount.Context
{
    public class DapperContext:DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(_connectionString);
            optionsBuilder.UseSqlServer("Server=DESKTOP-H53IQV2;initial Catalog=WebProjectDiscountDb;integrated Security=True");
        }
        public DbSet<Coupon> Coupons  { get; set; }
        public IDbConnection CreateConnection()=>new SqlConnection(_connectionString);
    }
}
