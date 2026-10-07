import os
import re
import json
import requests
from urllib.parse import urljoin
from bs4 import BeautifulSoup

# Paths
SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
PGN_DIR = os.path.abspath(os.path.join(SCRIPT_DIR, "..", "BritBase", "Data", "pgn"))
JSON_OUT = os.path.abspath(os.path.join(SCRIPT_DIR, "..", "BritBase", "Data", "brit80.json"))

# Target page via Wayback Machine mirror
PAGE_URL = "https://web.archive.org/web/20230601000000id_/http://www.saund.co.uk/britbase/brit80.htm"
BASE_HTTP = "http://www.saund.co.uk/britbase/"
ARCHIVE_PREFIX = "https://web.archive.org/web/20230601000000id_/"

HEADERS = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:128.0) Gecko/20100101 Firefox/128.0"
}


def download_missing_pgn(pgn_url, filename):
    dest = os.path.join(PGN_DIR, filename)
    if os.path.exists(dest) and os.path.getsize(dest) > 0:
        return True  # already got it

    arch_url = f"{ARCHIVE_PREFIX}{pgn_url}" if not pgn_url.startswith("https://web.archive.org") else pgn_url
    print(f"  [+] Downloading missing PGN: {filename} from {arch_url} ...")
    try:
        r = requests.get(arch_url, headers=HEADERS, timeout=20)
        if r.status_code == 200 and len(r.content) > 0:
            with open(dest, "wb") as f:
                f.write(r.content)
            print(f"      Saved {filename} ({len(r.content)} bytes)")
            return True
        else:
            print(f"      [!] Failed with HTTP {r.status_code}")
    except Exception as e:
        print(f"      [!] Error: {e}")
    return False


def main():
    os.makedirs(PGN_DIR, exist_ok=True)
    print(f"Fetching brit80.htm from archive mirror...")
    resp = requests.get(PAGE_URL, headers=HEADERS, timeout=25)
    resp.raise_for_status()

    soup = BeautifulSoup(resp.text, "html.parser")
    tables = soup.find_all("table")

    tournaments = []
    current_year = 1980

    for table in tables:
        for row in table.find_all("tr"):
            cols = row.find_all(["td", "th"])
            if len(cols) < 5:
                continue

            col_texts = [c.get_text(strip=True) for c in cols]
            c0 = col_texts[0]

            # Check if this row is a year header (e.g. 1980, 1981, 1982...)
            year_match = re.match(r"^(198\d)", c0)
            if year_match and ("winner" in col_texts[1].lower() or len(cols) == 6):
                current_year = int(year_match.group(1))
                continue

            # Skip header rows
            if "winner" in col_texts[1].lower() or "tournament" in c0.lower():
                continue

            name = col_texts[0]
            winner = col_texts[1] if len(cols) > 1 else ""
            start_date = col_texts[2] if len(cols) > 2 else ""
            end_date = col_texts[3] if len(cols) > 3 else ""
            games_count = col_texts[4] if len(cols) > 4 else ""
            date_updated = col_texts[5] if len(cols) > 5 else ""

            # Extract the actual link from the first column or the PGN column
            pgn_filename = ""
            viewer_url = ""

            for col_idx in [0, 4]:
                a_tag = cols[col_idx].find("a", href=True)
                if a_tag:
                    href = a_tag["href"].strip()
                    # Strip any internal web.archive.org prefix
                    if "/http" in href:
                        href = "http" + href.split("/http", 1)[1]
                    clean_href = urljoin(BASE_HTTP, href)

                    if clean_href.lower().endswith(".pgn"):
                        pgn_filename = os.path.basename(clean_href)
                        download_missing_pgn(clean_href, pgn_filename)
                    elif "-viewer.htm" in clean_href.lower() or "viewer.htm" in clean_href.lower():
                        viewer_url = clean_href
                        # Derive the PGN file name (e.g. 198004sutton-viewer.html -> 198004sutton.pgn)
                        guessed_pgn = re.sub(r"-?viewer\.html?$", ".pgn", clean_href, flags=re.I)
                        pgn_filename = os.path.basename(guessed_pgn)
                        # Try to download it if missing
                        download_missing_pgn(guessed_pgn, pgn_filename)

            entry = {
                "Year": current_year,
                "Name": name,
                "Winner": winner,
                "StartDate": start_date,
                "EndDate": end_date,
                "GamesCount": games_count,
                "DateUpdated": date_updated,
                "PgnFileName": pgn_filename,
                "ViewerUrl": viewer_url
            }
            tournaments.append(entry)

    # Save to JSON
    with open(JSON_OUT, "w", encoding="utf-8") as f:
        json.dump(tournaments, f, indent=2, ensure_ascii=False)

    print(f"\n[DONE] Extracted {len(tournaments)} tournaments from 1980-1989!")
    print(f"Saved metadata to: {JSON_OUT}")


if __name__ == "__main__":
    main()