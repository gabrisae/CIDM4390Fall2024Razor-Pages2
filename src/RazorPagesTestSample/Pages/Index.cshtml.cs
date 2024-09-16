using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RazorPagesTestSample.Data;
using System.Linq;
using System.Threading.Tasks;


public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public IList<Message> Messages { get; set; } = new List<Message>();
    public string AverageWordsMessage { get; set; }

    public async Task OnGetAsync()
    {
        Messages = await _db.Messages.ToListAsync();
    }

    public async Task<IActionResult> OnPostAddMessageAsync(string text)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(text))
        {
            return Page();
        }

        var message = new Message { Text = text };
        _db.Messages.Add(message);
        await _db.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteMessageAsync(int id)
    {
        var message = await _db.Messages.FindAsync(id);
        if (message != null)
        {
            _db.Messages.Remove(message);
            await _db.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAllMessagesAsync()
    {
        _db.Messages.RemoveRange(_db.Messages);
        await _db.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAnalyzeMessagesAsync()
    {
        Messages = await _db.Messages.ToListAsync();
        if (!Messages.Any())
        {
            AverageWordsMessage = "No messages to analyze.";
            return Page();
        }

        var averageWords = Messages.Average(m => m.Text.Split().Length);
        AverageWordsMessage = $"Average number of words per message: {averageWords:F2}";
        return Page();
    }
}
