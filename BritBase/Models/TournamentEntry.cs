namespace BritBase.Models;

public class TournamentEntry
{
    public int Year { get; set; }
    public string Name { get; set; } = "";
    public string Winner { get; set; } = "";
    public string StartDate { get; set; } = "";
    public string EndDate { get; set; } = "";
    public string GamesCount { get; set; } = "";
    public string DateUpdated { get; set; } = "";
    public string? PgnFileName { get; set; } // Matches downloaded files in Data/pgn
}