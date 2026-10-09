using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BritBase.Pages;

public class PlayersModel : PageModel
{
    public IReadOnlyList<PlayerEntry> Players { get; } =
    [
        new("Aitken, James M", "Aitken, James M", "1908", "1983", "2,281", "Most games input by Geoff Chandler", "12.01.2026"),
        new("Atkins, Henry E", "Atkins, Henry E", "1872", "1955", "389", "", "07.12.2025"),
        new("Barden, Leonard W", "Barden, Leonard W", "1929", "", "480", "includes 23 part-games, 68 stubs", "01.07.2026"),
        new("Basman, Michael J", "Basman, Michael J", "1946", "2022", "1,444", "includes 15 part-games, 33 stubs, 3 problems", "12.04.2026"),
        new("Bennett, Gerald H", "Bennett, Gerald H", "1949", "2025", "131", "", "28.01.2026"),
        new("Berry, Stephen H", "Berry, Stephen H", "1951", "2019", "432", "", "17.11.2025"),
        new("Cafferty, Bernard", "Cafferty, Bernard", "1934", "", "2,051", "includes 53 stubs, byes & default wins", "04.03.2026"),
        new("Chandler, Murray", "Chandler, Murray", "1960", "", "2,436", "", "19.10.2025"),
        new("Collins, Richard A", "Collins, Richard A", "1944", "2020", "2,039", "", "31.01.2026"),
        new("Coy, G Philip S", "Coy, G Philip S", "1917", "2019", "80", "Mostly games played between 1934 and 1940", "26.03.2024"),
        new("Emerson, Roger", "Emerson, Roger", "1949", "", "605", "Games played between 1965 and 1982", "05.08.2024"),
        new("Franklin, Michael J", "Franklin, Michael J", "1931", "2023", "749", "includes 38 stubs, byes & default wins", "04.03.2026"),
        new("Gibbs, Peter Campbell", "Gibbs, Peter Campbell", "1934", "2023", "371", "Includes 4 part-games", "19.06.2026"),
        new("Griffiths, Peter C", "Griffiths, Peter C", "1946", "", "1,027", "", "02.06.2026"),
        new("Ives, Ronald Wilson", "Ives, Ronald Wilson", "1923", "1964", "80", "Plus links to Yorkshire Evening Post columns", "29.09.2024"),
        new("Littlewood, Paul E", "Littlewood, Paul E", "1956", "", "1,743", "Games from 1960s to 2025", "26.11.2025"),
        new("Mabbs, David J", "Mabbs, David J", "1940", "", "303", "Games from 1958-1964, 1969-present", "26.10.2025"),
        new("Mansfield, Comins", "Mansfield, Comins", "1896", "1984", "44", "", "21.05.2024"),
        new("Menchik, Vera", "Menchik, Vera", "1906", "1944", "584", "total excluding stubs = 454 games/15 part-g's", "09.12.2025"),
        new("Miles, Anthony J", "Miles, Anthony J", "1955", "2001", "984", "Collection of games to the end of 1979", "18.07.2026"),
        new("Nicholson, John G", "Nicholson, John G", "1953", "", "100", "A personal collection of games by JG Nicholson", "24.08.2024"),
        new("Parr, Frank", "Parr, Frank", "1918", "2003", "348", "excluding stubs", "23.12.2023"),
        new("Pratten, Wilfred H", "Pratten, Wilfred H", "1908", "1985", "575", "includes 5 stubs, 3 part-games", "22.02.2026"),
        new("Rosselson, Leon", "Rosselson, Leon", "1934", "", "358", "", "11.09.2026"),
        new("Russ, Victor John Anthony", "Russ, Victor John Anthony", "1905", "1985", "687", "", "04.03.2026"),
        new("Sandercock, (Edward) Barry", "Sandercock, (Edward) Barry", "1930", "2023", "7", "died 21 February 2023 - English Chess Forum", "22.02.2023"),
        new("Shoosmith, Herbert W", "Shoosmith, Herbert W", "1877", "1912", "144", "127 complete games, 2 part-games, 15 stubs", "13.09.2023"),
        new("Short, Nigel", "Short, Nigel", "1965", "", "529", "Games from 1974 to Hastings 1983/84. Includes 19 stubs.", "07.10.2026"),
        new("Sultan Khan", "Sultan Khan", "1903", "1966", "241", "241 games including 30 stubs.", "21.02.2025"),
        new("Thomas, Sir George Alan", "Thomas, Sir George Alan", "1881", "1972", "1,251", "Includes 18 part-games & 163 stubs", "02.08.2026"),
        new("Wells, Ian D", "Wells, Ian D", "1964", "1982", "333", "Includes 7 stub games", "01.04.2026")
    ];

    public sealed record PlayerEntry(string Name, string Query, string Born, string Died, string Games, string Notes, string Updated);
}
