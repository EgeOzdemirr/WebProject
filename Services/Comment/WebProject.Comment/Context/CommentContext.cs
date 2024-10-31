using Microsoft.EntityFrameworkCore;
using WebProject.Comment.Entities;

namespace WebProject.Comment.Context
{
    public class CommentContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1442;initial " +
                "Catalog=WebProjectCommentDb;User=sa;Password=123456aA*");
        }
        public DbSet<UserComment> UserComments { get; set; }
    }
}
