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
        Tournaments = Get1980sData();
    }

    // HTMX: Load games for a tournament into the game replayer / list
    public IActionResult OnGetTournamentGames(string tournamentName, int year)
    {
        var games = _pgnService.GetAllGames()
            .Where(g => g.Date.StartsWith(year.ToString()) &&
                        (g.Event.Contains(tournamentName, StringComparison.OrdinalIgnoreCase) ||
                         tournamentName.Contains(g.Event, StringComparison.OrdinalIgnoreCase)))
            .Take(100)
            .ToList();

        if (!games.Any())
        {
            // Fallback: search by tournament name across the whole database
            games = _pgnService.Search(tournamentName, year.ToString(), limit: 100).ToList();
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
            // Search case-insensitively
            var match = Directory.GetFiles(pgnDir, "*.pgn")
                .FirstOrDefault(f => Path.GetFileName(f).Equals(file, StringComparison.OrdinalIgnoreCase));
            if (match != null) filePath = match;
            else return NotFound();
        }

        var bytes = System.IO.File.ReadAllBytes(filePath);
        return File(bytes, "application/x-chess-pgn", Path.GetFileName(filePath));
    }

    private static List<TournamentEntry> Get1980sData() => new()
    {
        // 1980
        new() { Year = 1980, Name = "SCCU Champ'ship", Winner = "Peter Large, David Cummings", StartDate = "1980.04.04", EndDate = "1980.04.07", GamesCount = "17", DateUpdated = "2025.10.25", PgnFileName = "198004sccu.pgn" },
        new() { Year = 1980, Name = "Surrey Open", Winner = "John Nunn, Gyula Sax", StartDate = "1980.04.04", EndDate = "1980.04.07", GamesCount = "12^", DateUpdated = "2025.10.25" },
        new() { Year = 1980, Name = "Phillips & Drew Kings", Winner = "Miles Korchnoi Andersson", StartDate = "1980.04.10", EndDate = "1980.04.25", GamesCount = "91", DateUpdated = "2020.08.25", PgnFileName = "198004p&dkings.pgn" },
        new() { Year = 1980, Name = "Phillips & Drew Kts", Winner = "vd Sterren, Botterill", StartDate = "1980.04.11", EndDate = "1980.04.25", GamesCount = "105^", DateUpdated = "2020.08.25" },
        new() { Year = 1980, Name = "Lloyds Bank Masters", Winner = "Florin Gheorghiu", StartDate = "1980.08.20", EndDate = "1980.08.28", GamesCount = "132", DateUpdated = "2021.02.28", PgnFileName = "198008lloyds.pgn" },
        new() { Year = 1980, Name = "GBR-ch Brighton", Winner = "Nunn, Hartston", StartDate = "1980.08.04", EndDate = "1980.08.15", GamesCount = "220", DateUpdated = "2019.03.30", PgnFileName = "198008bcf.pgn" },
        new() { Year = 1980, Name = "Manchester Bened.", Winner = "John Nunn", StartDate = "1980.09.03", EndDate = "1980.09.11", GamesCount = "104", DateUpdated = "2024.02.18", PgnFileName = "198009manchester.pgn" },
        new() { Year = 1980, Name = "Guernsey Open 6th", Winner = "five-way tie", StartDate = "1980.10.19", EndDate = "1980.10.25", GamesCount = "14", DateUpdated = "2025.10.08", PgnFileName = "198010guernsey.pgn" },
        new() { Year = 1980, Name = "Jersey International", Winner = "Ian D Wells", StartDate = "1980.10.26", EndDate = "1980.11.01", GamesCount = "53", DateUpdated = "2026.05.08", PgnFileName = "198010jersey.pgn" },
        new() { Year = 1980, Name = "Brighton", Winner = "Chandler, Speelman", StartDate = "1980.12.10", EndDate = "1980.12.18", GamesCount = "45", DateUpdated = "2020.08.19", PgnFileName = "198012brighton.pgn" },
        new() { Year = 1980, Name = "Welsh Champ 1981", Winner = "A Howard Williams", StartDate = "1980.12.28", EndDate = "1981.01.03", GamesCount = "28", DateUpdated = "2020.08.28", PgnFileName = "198012wlschamp.pgn" },
        new() { Year = 1980, Name = "Hastings Premier", Winner = "Ulf Andersson", StartDate = "1980.12.29", EndDate = "1981.01.15", GamesCount = "120", DateUpdated = "2022.07.04", PgnFileName = "198012hast.pgn" },

        // 1981
        new() { Year = 1981, Name = "ARC Young Masters 1st", Winner = "Murray Chandler", StartDate = "1981.02.27", EndDate = "1981.03.01", GamesCount = "138", DateUpdated = "2022.08.01" },
        new() { Year = 1981, Name = "Botvinnik Simul", Winner = "Botvinnik 4½-3½", StartDate = "1981.04.08", EndDate = "", GamesCount = "4+1", DateUpdated = "2024.06.24" },
        new() { Year = 1981, Name = "West of England Ch", Winner = "Peter H Clarke", StartDate = "1981.04.16", EndDate = "1981.04.19", GamesCount = "34", DateUpdated = "2024.07.19" },
        new() { Year = 1981, Name = "Staffordshire Open", Winner = "John Carleton", StartDate = "1981.05.23", EndDate = "1981.05.27", GamesCount = "87", DateUpdated = "2023.12.18" },
        new() { Year = 1981, Name = "GBR-ch Morecambe", Winner = "Paul Littlewood", StartDate = "1981.08.03", EndDate = "1981.08.14", GamesCount = "286", DateUpdated = "2019.06.20", PgnFileName = "198108bcf.pgn" },
        new() { Year = 1981, Name = "Lloyds Bank Masters", Winner = "Keene, Seirawan, Miles", StartDate = "1981.08.25", EndDate = "1981.09.02", GamesCount = "129+7", DateUpdated = "2025.08.02", PgnFileName = "198108lloyds.pgn" },
        new() { Year = 1981, Name = "Manchester Bened.", Winner = "Tony Miles", StartDate = "1981.09.04", EndDate = "1981.09.12", GamesCount = "116", DateUpdated = "2020.09.06", PgnFileName = "198109manchester.pgn" },
        new() { Year = 1981, Name = "Hastings Premier", Winner = "Viktor Kupreichik", StartDate = "1981.12.28", EndDate = "1982.01.12", GamesCount = "91", DateUpdated = "2025.01.04", PgnFileName = "198112hast.pgn" },

        // 1982
        new() { Year = 1982, Name = "London GLC Kings", Winner = "Andersson, Karpov", StartDate = "1982.04.15", EndDate = "1982.04.30", GamesCount = "91", DateUpdated = "2026.01.11" },
        new() { Year = 1982, Name = "GBR-ch Torquay", Winner = "Tony Miles", StartDate = "1982.08.02", EndDate = "1982.08.13", GamesCount = "308", DateUpdated = "2020.09.18", PgnFileName = "198208bcf.pgn" },
        new() { Year = 1982, Name = "Lloyds Bank Masters", Winner = "Tony Miles", StartDate = "1982.08.25", EndDate = "1982.09.02", GamesCount = "149", DateUpdated = "2022.12.30", PgnFileName = "198208lloyds.pgn" },
        new() { Year = 1982, Name = "Hastings Premier", Winner = "Rafael Vaganian", StartDate = "1982.12.28", EndDate = "1983.01.12", GamesCount = "91", DateUpdated = "2025.01.09", PgnFileName = "198212hast.pgn" },

        // 1983
        new() { Year = 1983, Name = "ARC Young Masters 3rd", Winner = "Kosten, Nunn, Short", StartDate = "1983.02.25", EndDate = "1983.02.27", GamesCount = "15", DateUpdated = "2025.10.13" },
        new() { Year = 1983, Name = "GBR-ch Southport", Winner = "Jonathan Mestel", StartDate = "1983.08.08", EndDate = "1983.08.19", GamesCount = "319", DateUpdated = "2023.01.13", PgnFileName = "198308bcf.pgn" },
        new() { Year = 1983, Name = "Hastings Premier", Winner = "L Karlsson, J Speelman", StartDate = "1983.12.28", EndDate = "1984.01.12", GamesCount = "91", DateUpdated = "2025.01.02", PgnFileName = "198312hast.pgn" },

        // 1984
        new() { Year = 1984, Name = "Oakham Masters", Winner = "Niaz Murshed", StartDate = "1984.04.08", EndDate = "1984.04.17", GamesCount = "180", DateUpdated = "2023.08.08" },
        new() { Year = 1984, Name = "Phillips & D/GLC Kings", Winner = "Anatoly Karpov", StartDate = "1984.04.26", EndDate = "1984.05.11", GamesCount = "91", DateUpdated = "2022.07.11" },
        new() { Year = 1984, Name = "GBR-ch Brighton", Winner = "Nigel Short", StartDate = "1984.07.30", EndDate = "1984.08.10", GamesCount = "347", DateUpdated = "2020.09.17", PgnFileName = "198407bcf.pgn" },
        new() { Year = 1984, Name = "Hastings Premier", Winner = "Evgeny Sveshnikov", StartDate = "1984.12.29", EndDate = "1985.01.13", GamesCount = "91", DateUpdated = "2025.06.19", PgnFileName = "198412hast.pgn" },

        // 1985
        new() { Year = 1985, Name = "GBR-ch Edinburgh", Winner = "Jonathan Speelman", StartDate = "1985.07.29", EndDate = "1985.08.09", GamesCount = "418", DateUpdated = "2023.02.20", PgnFileName = "198507bcf.pgn" },
        new() { Year = 1985, Name = "Lloyds Bank Masters", Winner = "Alexander Belyavsky", StartDate = "1985.08.21", EndDate = "1985.08.29", GamesCount = "185", DateUpdated = "2023.02.20", PgnFileName = "198508lloyds.pgn" },
        new() { Year = 1985, Name = "Hastings Premier", Winner = "Margeir Petursson", StartDate = "1985.12.28", EndDate = "1986.01.12", GamesCount = "91", DateUpdated = "2026.01.29", PgnFileName = "198512hast.pgn" },

        // 1986
        new() { Year = 1986, Name = "GBR-ch Southamp'n", Winner = "Jonathan Speelman", StartDate = "1986.07.28", EndDate = "1986.08.08", GamesCount = "341", DateUpdated = "2020.09.13", PgnFileName = "198607bcf.pgn" },
        new() { Year = 1986, Name = "Hastings Premier", Winner = "M Chandler, etc", StartDate = "1986.12.29", EndDate = "1987.01.13", GamesCount = "91", DateUpdated = "2020.08.20", PgnFileName = "198612hast.pgn" },

        // 1987
        new() { Year = 1987, Name = "GBR-ch Swansea", Winner = "Nigel Short", StartDate = "1987.08.03", EndDate = "1987.08.14", GamesCount = "321", DateUpdated = "2020.09.13", PgnFileName = "198708bcf.pgn" },
        new() { Year = 1987, Name = "Hastings Premier", Winner = "Nigel Short", StartDate = "1987.12.29", EndDate = "1988.01.14", GamesCount = "56", DateUpdated = "2025.01.08", PgnFileName = "198712hast.pgn" },

        // 1988
        new() { Year = 1988, Name = "GBR-ch Blackpool", Winner = "Jonathan Mestel", StartDate = "1988.08.01", EndDate = "1988.08.12", GamesCount = "305", DateUpdated = "2023.04.30", PgnFileName = "198808bcf.pgn" },
        new() { Year = 1988, Name = "Hastings Premier", Winner = "Nigel Short", StartDate = "1988.12.29", EndDate = "1989.01.14", GamesCount = "51", DateUpdated = "2023.05.23", PgnFileName = "198812hast.pgn" },

        // 1989
        new() { Year = 1989, Name = "GBR-ch Plymouth", Winner = "Michael Adams", StartDate = "1989.07.31", EndDate = "1989.08.11", GamesCount = "315", DateUpdated = "2024.08.29", PgnFileName = "198907bcf.pgn" },
        new() { Year = 1989, Name = "Hastings Premier", Winner = "Sergei Dolmatov", StartDate = "1989.12.28", EndDate = "1990.01.14", GamesCount = "56", DateUpdated = "2025.01.16", PgnFileName = "198912hast.pgn" },
    };
}