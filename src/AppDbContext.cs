using Microsoft.EntityFrameworkCore;

namespace RazorPagesTestSample.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Message> Messages { get; set; }

        // Seeding the database with default messages
        public static void Seed(AppDbContext context)
        {
            if (context.Messages.Any()) return; 

            context.Messages.AddRange(
                new Message { Text = "First message" },
                new Message { Text = "Second message" },
                new Message { Text = "Third message" }
            );
            context.SaveChanges();
        }
        public async Task DeleteMessageAsync(int id)
{
    var message = await Messages.FindAsync(id);
    if (message != null)
    {
        Messages.Remove(message);
        await SaveChangesAsync();
    }
}
    }
}
