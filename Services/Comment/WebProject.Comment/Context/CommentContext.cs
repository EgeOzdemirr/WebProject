using Microsoft.EntityFrameworkCore;
using WebProject.Comment.Entities;

namespace WebProject.Comment.Context
{
    public class CommentContext:DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options)
        {
        }
        public DbSet<UserComment> UserComments { get; set; }
    }
}
