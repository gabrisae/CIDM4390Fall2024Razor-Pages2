using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPagesTestSample.Data;
using Xunit;

public class DataAccessLayerTest
{
    [Fact]
    public async Task DeleteMessageAsync_MessageIsDeleted_WhenMessageIsFound()
    {
        using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
        {
            var seedMessages = new[] { new Message { Text = "Hello" }, new Message { Text = "World" } };
            await db.AddRangeAsync(seedMessages);
            await db.SaveChangesAsync();
            var recId = 1; // Assuming seed messages are assigned ID 1, 2, etc.

            await db.DeleteMessageAsync(recId);

            var actualMessages = await db.Messages.AsNoTracking().ToListAsync();
            Assert.DoesNotContain(actualMessages, m => m.Id == recId);
        }
    }

    [Fact]
    public async Task DeleteMessageAsync_NoMessageIsDeleted_WhenMessageIsNotFound()
    {
        using (var db = new AppDbContext(Utilities.TestDbContextOptions()))
        {
            var seedMessages = new[] { new Message { Text = "Hello" }, new Message { Text = "World" } };
            await db.AddRangeAsync(seedMessages);
            await db.SaveChangesAsync();
            var recId = 999; // Non-existent ID

            await db.DeleteMessageAsync(recId);

            var actualMessages = await db.Messages.AsNoTracking().ToListAsync();
            Assert.Equal(seedMessages.Length, actualMessages.Count);
        }
    }
}
