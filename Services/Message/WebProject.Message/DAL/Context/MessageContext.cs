using Microsoft.EntityFrameworkCore;
using WebProject.Message.DAL.Entities;

namespace WebProject.Message.DAL.Context
{
    public class MessageContext:DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options):base(options)
        {
            
        }
        public DbSet<UserMessage> UserMessages { get; set; }
    }
}
