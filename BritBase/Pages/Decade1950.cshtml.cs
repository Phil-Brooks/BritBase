using System.Text.Json;
using BritBase.Models;
using BritBase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BritBase.Pages;

public class Decade1950Model : PageModel
{
    private readonly PgnService _pgnService;
    private readonly IWebHostEnvironment _env;

    public Decade1950Model(PgnService pgnService, IWebHostEnvironment env)
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
            Path.Combine(_env.ContentRootPath, "Data", "brit50.json"),
            Path.Combine(AppContext.BaseDirectory, "Data", "brit50.json"),
            Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, "Data", "brit50.json")
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
            }
        }

        return new List<TournamentEntry>();
    }

    public IActionResult OnGetTournamentGames(string? pgnFile, string tournamentName, int year)
    {
        List<ChessGame> games = new();

        if (!string.IsNullOrEmpty(pgnFile))
        {
            games = _pgnService.GetAllGames()
                .Where(g => g.SourceFile.Equals(pgnFile, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

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
