using Microsoft.EntityFrameworkCore;
using WebProject.Comment.Entities;

namespace WebProject.Comment.Context
{
    public class CommentContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=WebProjectCommentDb;User Id=sa;Password=123456aA*;TrustServerCertificate=True");
        }
        public DbSet<UserComment> UserComments { get; set; }
    }
}
