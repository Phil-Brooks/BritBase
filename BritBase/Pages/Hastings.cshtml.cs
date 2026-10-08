using BritBase.Models;
using BritBase.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BritBase.Pages;

public class HastingsModel : PageModel
{
    private readonly IWebHostEnvironment _env;
    private readonly PgnService _pgnService;

    public HastingsModel(IWebHostEnvironment env, PgnService pgnService)
    {
        _env = env;
        _pgnService = pgnService;
        NormalizeLocalLinks();
    }

    public List<HastingsTournament> Tournaments { get; } = new()
    {
        new() { Number = 1, Year = "1920/21", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/192012hast-viewer.html", Winner = "F Yates", PremierGames = "10/12", Venue = "Hastings CC", PgnUrl = "https://www.saund.co.uk/britbase/pgn/192012hast.pgn" },
        new() { Number = 2, Year = "1921/22", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/192112hast-viewer.html", Winner = "B Kostich", PremierGames = "16/28", Venue = "Hastings Town Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/192112hast.pgn" },
        new() { Number = 3, Year = "1922/23", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/192212hast-viewer.html", Winner = "A Rubinstein", PremierGames = "29/45", Venue = "Hastings Town Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/192212hast.pgn" },
        new() { Number = 4, Year = "1923/24", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/192312hast-viewer.html", Winner = "M Euwe", PremierGames = "45/45", Venue = "Hastings Town Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/192312hast.pgn" },
        new() { Number = 5, Year = "1924/25", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/192412hast-viewer.html", Winner = "G Maroczy, S Tartakower", PremierGames = "59/59", Venue = "Hastings Town Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/192412hast.pgn" },
        new() { Number = 6, Year = "1925/26", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/192512hast-viewer.html", Winner = "A Alekhine, M Vidmar", PremierGames = "27/45", Venue = "Hastings Town Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/192512hast.pgn" },
        new() { Number = 7, Year = "1926/27", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/192612hast-viewer.html", Winner = "S Tartakower", PremierGames = "45/45", Venue = "Hastings Town Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/192612hast.pgn" },
        new() { Number = 8, Year = "1927/28", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/192712hast-viewer.html", Winner = "S Tartakower", PremierGames = "20/45", Venue = "Hastings Town Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/192712hast.pgn" },
        new() { Number = 9, Year = "1928/29", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/192812hast-viewer.html", Winner = "E Colle, F Marshall, A Takacs", PremierGames = "45/45", Venue = "Hastings Town Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/192812hast.pgn" },
        new() { Number = 10, Year = "1929/30", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/192912hast-viewer.html", Winner = "J Capablanca", PremierGames = "25/45", Venue = "Hastings Town Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/192912hast.pgn" },
        new() { Number = 11, Year = "1930/31", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/193012hast-viewer.html", Winner = "M Euwe", PremierGames = "33/45", Venue = "Waverley Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/193012hast.pgn" },
        new() { Number = 12, Year = "1931/32", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/193112hast-viewer.html", Winner = "S Flohr", PremierGames = "27/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/193112hast.pgn" },
        new() { Number = 13, Year = "1932/33", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/193212hast-viewer.html", Winner = "S Flohr", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/193212hast.pgn" },
        new() { Number = 14, Year = "1933/34", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/193312hast-viewer.html", Winner = "S Flohr", PremierGames = "44/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/193312hast.pgn" },
        new() { Number = 15, Year = "1934/35", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/193412hast-viewer.html", Winner = "G Thomas, M Euwe, S Flohr", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/193412hast.pgn" },
        new() { Number = 16, Year = "1935/36", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/193512hast-viewer.html", Winner = "R Fine", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/193512hast.pgn" },
        new() { Number = 17, Year = "1936/37", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/193612hast-viewer.html", Winner = "A Alekhine", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/193612hast.pgn" },
        new() { Number = 18, Year = "1937/38", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/193712hast-viewer.html", Winner = "S Reshevsky", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/193712hast.pgn" },
        new() { Number = 19, Year = "1938/39", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/193812hast-viewer.html", Winner = "L Szabo", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/193812hast.pgn" },
        new() { Number = 20, Year = "1939/40", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/193912hast-viewer.html", Winner = "F Parr", PremierGames = "10/28", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/193912hast.pgn" },
        new() { Number = 21, Year = "1945/46", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/194512hast-viewer.html", Winner = "S Tartakower", PremierGames = "66/66", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/194512hast.pgn" },
        new() { Number = 22, Year = "1946/47", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/194612hast-viewer.html", Winner = "C H O'D Alexander", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/194612hast.pgn" },
        new() { Number = 23, Year = "1947/48", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/194712hast-viewer.html", Winner = "L Szabo", PremierGames = "22/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/194712hast.pgn" },
        new() { Number = 24, Year = "1948/49", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/194812hast-viewer.html", Winner = "N Rossolimo", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/194812hast.pgn" },
        new() { Number = 25, Year = "1949/50", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/194912hast-viewer.html", Winner = "L Szabo", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/194912hast.pgn" },
        new() { Number = 26, Year = "1950/51", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/195012hast-viewer.html", Winner = "W Unzicker", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/195012hast.pgn" },
        new() { Number = 27, Year = "1951/52", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/195112hast-viewer.html", Winner = "S Gligoric", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/195112hast.pgn" },
        new() { Number = 28, Year = "1952/53", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/195212hast-viewer.html", Winner = "H Golombek, A Medina, J Penrose, D A Yanofsky", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/195212hast.pgn" },
        new() { Number = 29, Year = "1953/54", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/195312hast-viewer.html", Winner = "C H O'D Alexander, D Bronstein", PremierGames = "45/45", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/195312hast.pgn" },
        new() { Number = 30, Year = "1954/55", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/195412hast-viewer.html", Winner = "P Keres, V Smyslov", PremierGames = "45/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/195412hast.pgn" },
        new() { Number = 31, Year = "1955/56", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/195512hast-viewer.html", Winner = "V Korchnoi, F Olafsson", PremierGames = "45/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/195512hast.pgn" },
        new() { Number = 32, Year = "1956/57", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/195612hast-viewer.html", Winner = "S Gligoric, B Larsen", PremierGames = "45/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/195612hast.pgn" },
        new() { Number = 33, Year = "1957/58", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/195712hast-viewer.html", Winner = "P Keres", PremierGames = "45/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/195712hast.pgn" },
        new() { Number = 34, Year = "1958/59", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/195812hast-viewer.html", Winner = "W Uhlmann", PremierGames = "14+6/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/195812hast.pgn" },
        new() { Number = 35, Year = "1959/60", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/195912hast-viewer.html", Winner = "S Gligoric", PremierGames = "45/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/195912hast.pgn" },
        new() { Number = 36, Year = "1960/61", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/196012hast-viewer.html", Winner = "S Gligoric", PremierGames = "45/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/196012hast.pgn" },
        new() { Number = 37, Year = "1961/62", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/196112hast-viewer.html", Winner = "M Botvinnik", PremierGames = "45/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/196112hast.pgn" },
        new() { Number = 38, Year = "1962/63", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/196212hast-viewer.html", Winner = "S Gligoric, A Kotov", PremierGames = "45/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/196212hast.pgn" },
        new() { Number = 39, Year = "1963/64", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/196312hast-viewer.html", Winner = "M Tal", PremierGames = "45/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/196312hast.pgn" },
        new() { Number = 40, Year = "1964/65", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/196412hast-viewer.html", Winner = "P Keres", PremierGames = "45/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/196412hast.pgn" },
        new() { Number = 41, Year = "1965/66", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/196512hast-viewer.html", Winner = "B Spassky, W Uhlmann", PremierGames = "45/45", Venue = "Sun Lounge, St Leonards", PgnUrl = "https://www.saund.co.uk/britbase/pgn/196512hast.pgn" },
        new() { Number = 42, Year = "1966/67", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/196612hast-viewer.html", Winner = "M Botvinnik", PremierGames = "45/45", Venue = "Falaise Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/196612hast.pgn" },
        new() { Number = 43, Year = "1967/68", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/196712hast-viewer.html", Winner = "F Gheorghiu, V Hort, L Stein, A Suetin", PremierGames = "45/45", Venue = "Falaise Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/196712hast.pgn" },
        new() { Number = 44, Year = "1968/69", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/196812hast-viewer.html", Winner = "V Smyslov", PremierGames = "66/66", Venue = "Falaise Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/196812hast.pgn" },
        new() { Number = 45, Year = "1969/70", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/196912hast-viewer.html", Winner = "L Portisch", PremierGames = "45/45", Venue = "Falaise Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/196912hast.pgn" },
        new() { Number = 46, Year = "1970/71", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/197012hast-viewer.html", Winner = "L Portisch", PremierGames = "45/45", Venue = "Falaise Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/197012hast.pgn" },
        new() { Number = 47, Year = "1971/72", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/197112hast-viewer.html", Winner = "A Karpov, V Korchnoi", PremierGames = "120/120", Venue = "Falaise Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/197112hast.pgn" },
        new() { Number = 48, Year = "1972/73", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/197212hast-viewer.html", Winner = "Bent Larsen", PremierGames = "120/120", Venue = "Falaise Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/197212hast.pgn" },
        new() { Number = 49, Year = "1973/74", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/197312hast-viewer.html", Winner = "G Kuzmin, L Szabo, M Tal, J Timman", PremierGames = "120/120", Venue = "Falaise Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/197312hast.pgn" },
        new() { Number = 50, Year = "1974/75", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/197412hast-viewer.html", Winner = "Vlastimil Hort", PremierGames = "120/120", Venue = "Falaise Hall", PgnUrl = "https://www.saund.co.uk/britbase/pgn/197412hast.pgn" },
        new() { Number = 51, Year = "1975/76", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/197512hast-viewer.html", Winner = "D Bronstein, V Hort, W Uhlmann", PremierGames = "120/120", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/197512hast.pgn" },
        new() { Number = 52, Year = "1976/77", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/197612hast-viewer.html", Winner = "Oleg Romanishin", PremierGames = "105/105", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/197612hast.pgn" },
        new() { Number = 53, Year = "1977/78", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/197712hast-viewer.html", Winner = "Roman Dzindzihashvili", PremierGames = "105/105", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/197712hast.pgn" },
        new() { Number = 54, Year = "1978/79", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/197812hast-viewer.html", Winner = "Ulf Andersson", PremierGames = "105/105", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/197812hast.pgn" },
        new() { Number = 55, Year = "1979/80", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/197912hast-viewer.html", Winner = "Ulf Andersson, John Nunn", PremierGames = "120/120", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/197912hast.pgn" },
        new() { Number = 56, Year = "1980/81", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/198012hast-viewer.html", Winner = "Ulf Andersson", PremierGames = "120/120", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/198012hast.pgn" },
        new() { Number = 57, Year = "1981/82", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/198112hast-viewer.html", Winner = "V Kupreychik", PremierGames = "91/91", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/198112hast.pgn" },
        new() { Number = 58, Year = "1982/83", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/198212hast-viewer.html", Winner = "R Vaganian", PremierGames = "91/91", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/198212hast.pgn" },
        new() { Number = 59, Year = "1983/84", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/198312hast-viewer.html", Winner = "L Karlsson, J Speelman", PremierGames = "91/91", Venue = "White Rock Pavilion", PgnUrl = "https://www.saund.co.uk/britbase/pgn/198312hast.pgn" },
        new() { Number = 60, Year = "1984/85", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/198412hast-viewer.html", Winner = "E Sveshnikov", PremierGames = "91/91", Venue = "Queen's Hotel", PgnUrl = string.Empty },
        new() { Number = 61, Year = "1985/86", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/198512hast-viewer.html", Winner = "M Petursson", PremierGames = "91/91", Venue = "Queen's Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/198512hast.pgn" },
        new() { Number = 62, Year = "1986/87", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/198612hast-viewer.html", Winner = "M Chandler, B Larsen, S Lputian, J Speelman", PremierGames = "91/91", Venue = "Queen's Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/198612hast.pgn" },
        new() { Number = 63, Year = "1987/88", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/198712hast-viewer.html", Winner = "N Short", PremierGames = "56/56", Venue = "Queen's Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/198712hast.pgn" },
        new() { Number = 64, Year = "1988/89", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/198812hast-viewer.html", Winner = "N Short", PremierGames = "51/51", Venue = "Queen's Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/198812hast.pgn" },
        new() { Number = 65, Year = "1989/90", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/198912hast-viewer.html", Winner = "S Dolmatov", PremierGames = "56/56", Venue = "Cinque Ports Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/198912hast.pgn" },
        new() { Number = 66, Year = "1990/91", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/199012hast-viewer.html", Winner = "E Bareev", PremierGames = "56/56", Venue = "Cinque Ports Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/199012hast.pgn" },
        new() { Number = 67, Year = "1991/92", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/199112hast-viewer.html", Winner = "E Bareev", PremierGames = "56/56", Venue = "Cinque Ports Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/199112hast.pgn" },
        new() { Number = 68, Year = "1992/93", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/199212hast-viewer.html", Winner = "J Polgar, E Bareev", PremierGames = "56/56", Venue = "Cinque Ports Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/199212hast.pgn" },
        new() { Number = 69, Year = "1993/94", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/199312hast-viewer.html", Winner = "J Nunn", PremierGames = "45/45", Venue = "Cinque Ports Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/199312hast.pgn" },
        new() { Number = 70, Year = "1994/95", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/199412hast-viewer.html", Winner = "T Luther", PremierGames = "45/45", Venue = "Cinque Ports Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/199412hast.pgn" },
        new() { Number = 71, Year = "1995/96", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/199512hast-viewer.html", Winner = "S Conquest, A Khalifman, B Lalic", PremierGames = "45/45", Venue = "Cinque Ports Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/199512hast.pgn" },
        new() { Number = 72, Year = "1996/97", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/199612hast-viewer.html", Winner = "M Hebden, J Nunn, E Rozentalis", PremierGames = "45/45", Venue = "Cinque Ports Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/199612hast.pgn" },
        new() { Number = 73, Year = "1997/98", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/199712hast-viewer.html", Winner = "M Sadler", PremierGames = "45/45", Venue = "Cinque Ports Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/199712hast.pgn" },
        new() { Number = 74, Year = "1998/99", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/199812hast-viewer.html", Winner = "I Sokolov", PremierGames = "45/45", Venue = "Cinque Ports Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/199812hast.pgn" },
        new() { Number = 75, Year = "1999/00", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/199912hast-viewer.html", Winner = "E Sutovsky", PremierGames = "45/45", Venue = "Cinque Ports Hotel", PgnUrl = "https://www.saund.co.uk/britbase/pgn/199912hast.pgn" },
        new() { Number = 76, Year = "2000/01", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/200012hast-viewer.html", Winner = "S Conquest, K Sasikiran", PremierGames = "45/45", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/200012hast.pgn" },
        new() { Number = 77, Year = "2001/02", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/200112hast-viewer.html", Winner = "A Barsov, P Harikrishna, K Sasikiran", PremierGames = "45/45", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/200112hast.pgn" },
        new() { Number = 78, Year = "2002/03", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/200212hast-viewer.html", Winner = "P H Nielsen", PremierGames = "45/45", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/200212hast.pgn" },
        new() { Number = 79, Year = "2003/04", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/200312hast-viewer.html", Winner = "V Kotronias, J Rowson", PremierGames = "45/45", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/200312hast.pgn" },
        new() { Number = 80, Year = "2004/05", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/200412hast-viewer.html", Winner = "V Belov", PremierGames = "ko/open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/200412hast.pgn" },
        new() { Number = 81, Year = "2005/06", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/200512hast-viewer.html", Winner = "Y Neverov", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/200512hast.pgn" },
        new() { Number = 82, Year = "2006/07", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/200612hast-viewer.html", Winner = "M Gagunashvili, Y Neverov", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/200612hast.pgn" },
        new() { Number = 83, Year = "2007/08", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/200712hast-viewer.html", Winner = "V Malakhatko, N Mamedov, V Neverov", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/200712hast.pgn" },
        new() { Number = 84, Year = "2008/09", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/200812hast-viewer.html", Winner = "Igor Kurnosov", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/200812hast.pgn" },
        new() { Number = 85, Year = "2009/10", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/200912hast-viewer.html", Winner = "A Istratescu, D Howell, R Edouard, M Hebden", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/200912hast.pgn" },
        new() { Number = 86, Year = "2010/11", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/201012hast-viewer.html", Winner = "A Das, Deep Sengupta", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/201012hast.pgn" },
        new() { Number = 87, Year = "2011/12", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/201112hast-viewer.html", Winner = "Wang Yue", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/201112hast.pgn" },
        new() { Number = 88, Year = "2012/13", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/201212hast-viewer.html", Winner = "G Jones", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/201212hast.pgn" },
        new() { Number = 89, Year = "2013/14", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/201312hast-viewer.html", Winner = "Qun Ma, M Mchedlishvili, I Khenkin, J Vakhidov, J Sarkar, M Hebden, J Radovanovic", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/201312hast.pgn" },
        new() { Number = 90, Year = "2014/15", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/201412hast-viewer.html", Winner = "Zhao Jun", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/201412hast.pgn" },
        new() { Number = 91, Year = "2015/16", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/201512hast-viewer.html", Winner = "Jahongir Vakhidov, Aleksander Mista", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/201512hast.pgn" },
        new() { Number = 92, Year = "2016/17", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/201612hast-viewer.html", Winner = "Deep Sengupta", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/201612hast.pgn" },
        new() { Number = 93, Year = "2017/18", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/201712hast-viewer.html", Winner = "Lou Kiping, Deep Sengupta", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/201712hast.pgn" },
        new() { Number = 94, Year = "2018/19", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/201812hast-viewer.html", Winner = "D Gormally, M Petrov, O Korneev, C Murphy, A Cherniaev, S Sulskis", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/201812hast.pgn" },
        new() { Number = 95, Year = "2019/20", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/201912hast-viewer.html", Winner = "Magesh Chandran Panchanathan", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/201912hast.pgn" },
        new() { Number = 0, Year = "2020/21", ViewerUrl = string.Empty, Winner = "not held", PremierGames = string.Empty, Venue = string.Empty, PgnUrl = string.Empty },
        new() { Number = 0, Year = "2021/22", ViewerUrl = string.Empty, Winner = "not held", PremierGames = string.Empty, Venue = string.Empty, PgnUrl = string.Empty },
        new() { Number = 96, Year = "2022/23", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/202212hast-viewer.html", Winner = "Sarunas Sulskis", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/202212hast.pgn" },
        new() { Number = 97, Year = "2023/24", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/202312hast-viewer.html", Winner = "Abhijeet Gupta", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/202312hast.pgn" },
        new() { Number = 98, Year = "2024/25", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/202412hast-viewer.html", Winner = "Haowen Xue", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/202412hast.pgn" },
        new() { Number = 99, Year = "2025/26", ViewerUrl = "https://www.saund.co.uk/britbase/pgn/202512hast-viewer.html", Winner = "Alexander Kovchan, Alex Golding", PremierGames = "open", Venue = "Horntye Park", PgnUrl = "https://www.saund.co.uk/britbase/pgn/202512hast.pgn" }
    };

    private void NormalizeLocalLinks()
    {
        foreach (var item in Tournaments)
        {
            var resolvedPgnFile = ResolvePgnFileName(item.PgnUrl, item.ViewerUrl);
            if (!string.IsNullOrWhiteSpace(resolvedPgnFile))
            {
                item.PgnFileName = resolvedPgnFile;
            }

            if (!string.IsNullOrWhiteSpace(item.PgnUrl) && item.PgnUrl.Contains("saund.co.uk", StringComparison.OrdinalIgnoreCase))
            {
                item.PgnUrl = ToDownloadUrl(item.PgnUrl);
            }

            if (!string.IsNullOrWhiteSpace(item.ViewerUrl) && item.ViewerUrl.Contains("saund.co.uk", StringComparison.OrdinalIgnoreCase))
            {
                item.ViewerUrl = ToViewUrl(item.ViewerUrl, item.Year);
            }
        }
    }

    private static string? ResolvePgnFileName(string? pgnUrl, string? viewerUrl)
    {
        var source = !string.IsNullOrWhiteSpace(pgnUrl) ? pgnUrl : viewerUrl;
        if (string.IsNullOrWhiteSpace(source))
        {
            return null;
        }

        var fileName = Path.GetFileName(new Uri(source).LocalPath);
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return null;
        }

        return fileName.Replace("-viewer.html", ".pgn", StringComparison.OrdinalIgnoreCase);
    }

    private static string ToDownloadUrl(string originalUrl)
    {
        var fileName = Path.GetFileName(new Uri(originalUrl).LocalPath);
        return $"/Hastings?handler=DownloadPgn&file={Uri.EscapeDataString(fileName)}";
    }

    private static string ToViewUrl(string originalUrl, string tournamentYear)
    {
        var fileName = Path.GetFileName(new Uri(originalUrl).LocalPath)
            .Replace("-viewer.html", ".pgn", StringComparison.OrdinalIgnoreCase);
        return $"/Hastings?handler=TournamentGames&pgnFile={Uri.EscapeDataString(fileName)}&tournamentName={Uri.EscapeDataString(tournamentYear)}";
    }

    public IActionResult OnGetTournamentGames(string? pgnFile, string? tournamentName)
    {
        var games = new List<ChessGame>();

        if (!string.IsNullOrWhiteSpace(pgnFile))
        {
            games = _pgnService.GetAllGames()
                .Where(g => g.SourceFile.Equals(pgnFile, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (!games.Any() && !string.IsNullOrWhiteSpace(tournamentName))
        {
            var normalizedName = tournamentName.Trim();
            var year = normalizedName.Contains('/') ? normalizedName.Split('/')[0] : normalizedName;

            games = _pgnService.GetAllGames()
                .Where(g => g.Date.StartsWith(year, StringComparison.OrdinalIgnoreCase) ||
                            g.Event.Contains(normalizedName, StringComparison.OrdinalIgnoreCase) ||
                            normalizedName.Contains(g.Event, StringComparison.OrdinalIgnoreCase))
                .Take(100)
                .ToList();
        }

        return Partial("_GameListPartial", games);
    }

    public IActionResult OnGetViewPgn(string file)
    {
        var filePath = ResolvePgnPath(file);
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return NotFound();
        }

        var bytes = System.IO.File.ReadAllBytes(filePath);
        return File(bytes, "text/plain", Path.GetFileName(filePath));
    }

    public IActionResult OnGetDownloadPgn(string file)
    {
        var filePath = ResolvePgnPath(file);
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return NotFound();
        }

        var bytes = System.IO.File.ReadAllBytes(filePath);
        return File(bytes, "application/x-chess-pgn", Path.GetFileName(filePath));
    }

    private string? ResolvePgnPath(string? file)
    {
        if (string.IsNullOrWhiteSpace(file))
        {
            return null;
        }

        var expected = Path.GetFileName(file);
        var candidate = Path.Combine(_env.ContentRootPath, "Data", "pgn", expected);
        if (System.IO.File.Exists(candidate))
        {
            return candidate;
        }

        return null;
    }
}

public class HastingsTournament
{
    public int Number { get; set; }
    public string Year { get; set; } = string.Empty;
    public string ViewerUrl { get; set; } = string.Empty;
    public string PgnFileName { get; set; } = string.Empty;
    public string Winner { get; set; } = string.Empty;
    public string PremierGames { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;
    public string PgnUrl { get; set; } = string.Empty;
}
