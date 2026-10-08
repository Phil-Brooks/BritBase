using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BritBase.Models;
using BritBase.Services;

namespace BritBase.Pages;

public class Decade1980Model : PageModel
{
    private readonly PgnService _pgnService;
    private readonly IWebHostEnvironment _env;

    public Decade1980Model(PgnService pgnService, IWebHostEnvironment env)
    {
        _pgnService = pgnService;
        _env = env;
    }

    public List<TournamentEntry> Tournaments { get; set; } = new();

    public void OnGet()
    {
        Tournaments = LoadTournaments();
    }

    private List<TournamentEntry> LoadTournaments()
    {
        var candidatePaths = new[]
        {
            Path.Combine(_env.ContentRootPath, "Data", "brit80.json"),
            Path.Combine(AppContext.BaseDirectory, "Data", "brit80.json"),
            Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, "Data", "brit80.json")
        };

        foreach (var jsonPath in candidatePaths.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (!System.IO.File.Exists(jsonPath))
            {
                continue;
            }

            try
            {
                var json = System.IO.File.ReadAllText(jsonPath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    continue;
                }

                var parsed = JsonSerializer.Deserialize<List<TournamentEntry>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (parsed is { Count: > 0 })
                {
                    return parsed;
                }
            }
            catch (JsonException)
            {
                // Ignore malformed tournament metadata and continue to the next candidate.
            }
        }

        return new List<TournamentEntry>();
    }

    // HTMX: Load games for a tournament by its exact PGN file or by tournament name
    public IActionResult OnGetTournamentGames(string? pgnFile, string tournamentName, int year)
    {
        List<ChessGame> games = new();

        // 1. If we have the exact PGN file name (e.g. 198004sutton.pgn), filter by it directly!
        if (!string.IsNullOrEmpty(pgnFile))
        {
            games = _pgnService.GetAllGames()
                .Where(g => g.SourceFile.Equals(pgnFile, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // 2. Fallback if not found by filename: filter by Year and Name
        if (!games.Any())
        {
            games = _pgnService.GetAllGames()
                .Where(g => g.Date.StartsWith(year.ToString()) &&
                            (g.Event.Contains(tournamentName, StringComparison.OrdinalIgnoreCase) ||
                             tournamentName.Contains(g.Event, StringComparison.OrdinalIgnoreCase)))
                .Take(100)
                .ToList();
        }

        return Partial("_GameListPartial", games);
    }

    // Direct PGN download handler
    public IActionResult OnGetDownloadPgn(string file)
    {
        var pgnDir = Path.Combine(_env.ContentRootPath, "Data", "pgn");
        var filePath = Path.Combine(pgnDir, file);

        if (!System.IO.File.Exists(filePath))
        {
            var match = Directory.GetFiles(pgnDir, "*.pgn")
                .FirstOrDefault(f => Path.GetFileName(f).Equals(file, StringComparison.OrdinalIgnoreCase));
            if (match != null) filePath = match;
            else return NotFound($"PGN file {file} not found on server.");
        }

        var bytes = System.IO.File.ReadAllBytes(filePath);
        return File(bytes, "application/x-chess-pgn", Path.GetFileName(filePath));
    }
}