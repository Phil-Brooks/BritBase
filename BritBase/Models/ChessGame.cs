namespace BritBase.Models;

public class ChessGame
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Event { get; set; } = "Unknown Event";
    public string Site { get; set; } = "";
    public string Date { get; set; } = "????.??.??";
    public string Round { get; set; } = "";
    public string White { get; set; } = "Unknown";
    public string Black { get; set; } = "Unknown";
    public string Result { get; set; } = "*";
    public string Eco { get; set; } = "";
    public string Moves { get; set; } = "";
    public string SourceFile { get; set; } = "";

    // Helper display title for UI
    public string Title => $"{White} vs {Black} ({Result}) - {Event} ({Date})";
}