using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BritBase.Pages;

public class WhatsNewModel : PageModel
{
    public List<WhatsNewUpdate> Updates { get; private set; } = new List<WhatsNewUpdate>();

    public void OnGet()
    {
        Updates = new List<WhatsNewUpdate>
        {
            new WhatsNewUpdate
            {
                Date = "07/10/26",
                Tournament = "1998 British Championship",
                Winner = "Nigel Short",
                Link = "https://www.saund.co.uk/britbase/pgn/199808bcf-viewer.html",
                Notes = "287 games (from a possible 363), 2 play-off games, 205 Major Open games and 1 other, plus crosstables. <a href=\"https://www.saund.co.uk/britbase/pgn/199808bcf-viewer.html#update\" target=\"_blank\" rel=\"noreferrer\">Latest</a>: added 205 games from the Major Open (which incorporated the Under-18 Championship). The games were read from the bulletin by AI and look to be in good shape but please report any anomalies spotted."
            },
            new WhatsNewUpdate
            {
                Date = "05/10/26",
                Tournament = "1989/90 Hastings",
                Winner = "Sergey Dolmatov",
                Link = "https://www.saund.co.uk/britbase/pgn/198912hast-viewer.html",
                Notes = "All 56 games of the Premier, 97 games from the Challengers, with round numbers and dates and a full crosstable. Plus all 54 games of the Hastings Masters, a Scheveningen-format tournament held immediately following the congress in January 1990 and 22 games from other sections. <a href=\"https://www.saund.co.uk/britbase/pgn/198912hast-viewer.html#update\" target=\"_blank\" rel=\"noreferrer\">Latest</a>: added the game C Howell 1-0 C Costello (Main A, rd 3). Many thanks to Brian Denman for submitting the game."
            },
            new WhatsNewUpdate
            {
                Date = "04/10/26",
                Tournament = "1971 British Championship",
                Winner = "Raymond Keene",
                Link = "https://www.saund.co.uk/britbase/pgn/197108bcf-viewer.html",
                Notes = "All 198 games of the championship proper, plus 51 games from other sections, crosstables, full results of all sections, etc. <a href=\"https://www.saund.co.uk/britbase/pgn/197108bcf-viewer.html#update\" target=\"_blank\" rel=\"noreferrer\">Latest</a>: added the game M Staples 1-0 J G Nicholson (U21, rd 9). Many thanks to Brian Denman for submitting the score."
            },
            new WhatsNewUpdate
            {
                Date = "04/10/26",
                Tournament = "1991 Rochdale Congress",
                Winner = "Jeff Horner",
                Link = "https://www.saund.co.uk/britbase/pgn/199110rochdale-viewer.html",
                Notes = "84 games from the 20th Rochdale Congress, October 1991. Many thanks to Rudy van Kemenade for supplying the games."
            },
            new WhatsNewUpdate
            {
                Date = "03/10/26",
                Tournament = "1998 British Junior Championships",
                Winner = "various",
                Link = "https://www.saund.co.uk/britbase/pgn/199807bcfjunior-viewer.html",
                Notes = "New item. 148 games. For the first time at BritBase, games and crosstables were input and formatted by AI from the <a href=\"https://www.saund.org.uk/pdf.js/web/viewer.html?file=../../britbase/pdfs/1998-british-champ-junior.pdf\" target=\"_blank\" rel=\"noreferrer\">tournament bulletin prepared by Richard O'Brien</a>."
            },
            new WhatsNewUpdate
            {
                Date = "02/10/26",
                Tournament = "2026 Medway Congress",
                Winner = "Can Duruk",
                Link = "https://www.saund.co.uk/britbase/pgn/202609medway-viewer.html",
                Notes = "First uploaded. 52 Open games, 62 U1800 games, 18 U1400 games, kindly input and supplied by Steve Giddins, plus crosstables sourced from chess-results."
            }
        };
    }
}

public class WhatsNewUpdate
{
    public string Date { get; set; } = string.Empty;
    public string Tournament { get; set; } = string.Empty;
    public string Winner { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
