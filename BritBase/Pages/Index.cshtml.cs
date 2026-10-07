using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BritBase.Models;
using BritBase.Services;

namespace BritBase.Pages;

public class IndexModel : PageModel
{
    private readonly PgnService _pgnService;

    public IndexModel(PgnService pgnService)
    {
        _pgnService = pgnService;
    }

    public IReadOnlyList<ChessGame> Games { get; set; } = new List<ChessGame>();

    [BindProperty(SupportsGet = true)]
    public string? Query { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Year { get; set; }

    public void OnGet()
    {
        Games = _pgnService.Search(Query, Year).ToList();
    }

    // HTMX handler for search filtering
    public IActionResult OnGetSearch(string? query, string? year)
    {
        var filtered = _pgnService.Search(query, year).ToList();
        return Partial("_GameListPartial", filtered);
    }

    public IActionResult OnGetLoadGame(string id)
    {
        var game = _pgnService.GetGameById(id);
        if (game == null)
        {
            return Content("<div class='alert alert-danger'>Game not found.</div>");
        }

        return Partial("_GameReplayerPartial", game);
    }
    // Filter by tournament / fixture (Hastings, Varsity, etc.)
    public IActionResult OnGetFilterCategory(string category)
    {
        var filtered = _pgnService.Search(category, null, limit: 150).ToList();
        return Partial("_GameListPartial", filtered);
    }

    // Filter by decade prefix (e.g. "193" matches all 1930s games, "192" matches 1920s)
    public IActionResult OnGetFilterDecade(string decade)
    {
        var filtered = _pgnService.GetAllGames()
            .Where(g => g.Date.StartsWith(decade, StringComparison.OrdinalIgnoreCase))
            .Take(150)
            .ToList();

        return Partial("_GameListPartial", filtered);
    }
}