using System.Security.Cryptography;
using System.Text;

namespace BritBase.Models;

public class ChessGame
{
    public string Id { get; set; } = string.Empty;
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

    public static string CreateStableId(string sourceFile, string eventName, string site, string date, string round, string white, string black, string result, string eco)
    {
        var raw = string.Join("|", sourceFile, eventName, site, date, round, white, black, result, eco);
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    // Helper display title for UI
    public string Title => $"{White} vs {Black} ({Result}) - {Event} ({Date})";
}
