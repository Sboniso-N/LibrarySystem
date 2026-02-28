// Services/IRecommendationService.cs
using LibrarySystem.Data;
using LibrarySystem.Models;

public interface IRecommendationService
{
    Task<List<Book>> GetRecommendationsAsync(string genre, string preferredAuthor = "");
}

public class RecommendationService : IRecommendationService
{
    private readonly LibraryDbContext _context;

    public RecommendationService(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetRecommendationsAsync(string genre, string preferredAuthor = "")
    {
        var query = _context.Books.Where(b => b.Genre.Contains(genre) && b.IsAvailable);

        if (!string.IsNullOrEmpty(preferredAuthor))
        {
            query = query.Where(b => b.Author.Contains(preferredAuthor));
        }

        return await query.Take(5).ToListAsync();
    }
}