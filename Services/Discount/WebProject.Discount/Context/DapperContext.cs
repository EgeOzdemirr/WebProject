using Npgsql;
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
            optionsBuilder.UseNpgsql(_connectionString);
        }
        public DbSet<Coupon> Coupons  { get; set; }
        public IDbConnection CreateConnection()=>new NpgsqlConnection(_connectionString);
    }
}
