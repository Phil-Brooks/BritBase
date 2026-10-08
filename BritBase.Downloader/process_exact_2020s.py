import json
import os
import re

import requests
from bs4 import BeautifulSoup

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
JSON_OUT = os.path.abspath(os.path.join(SCRIPT_DIR, "..", "BritBase", "Data", "brit2020.json"))
PGN_DIR = os.path.abspath(os.path.join(SCRIPT_DIR, "..", "BritBase", "Data", "pgn"))
PAGE_URL = "https://www.saund.org.uk/britbase/brit2020.htm"
BASE_URL = "https://www.saund.org.uk/britbase/"


def download_pgn(filename: str, viewer_hint: str = ""):
    if not filename:
        return

    dest = os.path.join(PGN_DIR, filename)
    os.makedirs(PGN_DIR, exist_ok=True)
    if os.path.exists(dest) and os.path.getsize(dest) > 0:
        return

    viewer_file = viewer_hint or filename.replace(".pgn", "-viewer.html")
    url = f"{BASE_URL}pgn/{filename}"
    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0 Safari/537.36",
        "Referer": f"{BASE_URL}pgn/{viewer_file}",
    }

    response = requests.get(url, headers=headers, timeout=30)
    if response.status_code == 200 and response.content:
        with open(dest, "wb") as f:
            f.write(response.content)
        print(f"Downloaded {filename} ({len(response.content)} bytes)")
    else:
        print(f"Failed to download {filename}: HTTP {response.status_code}")


def parse_page():
    response = requests.get(PAGE_URL, timeout=30)
    response.raise_for_status()
    soup = BeautifulSoup(response.text, "html.parser")

    table = None
    for candidate in soup.select("table"):
        row_count = len(candidate.find_all("tr"))
        if row_count > 20:
            table = candidate
            break

    if table is None:
        raise RuntimeError("Could not find the 2020s tournament table")

    current_year = None
    tournaments = []

    for row in table.find_all("tr"):
        cells = row.find_all(["td", "th"])
        if len(cells) < 4:
            continue

        texts = [c.get_text(" ", strip=True) for c in cells]
        first = texts[0].strip() if texts else ""

        if re.fullmatch(r"20(20|21|22|23|24|25|26)", first):
            current_year = int(first)
            continue

        if not first or "winner" in first.lower() or "date posted" in first.lower():
            continue

        if current_year is None:
            continue

        name = texts[0].strip()
        winner = texts[1].strip() if len(texts) > 1 else ""
        start_date = texts[2].strip() if len(texts) > 2 else ""
        end_date = texts[3].strip() if len(texts) > 3 else ""
        games_count = texts[4].strip() if len(texts) > 4 else ""
        date_updated = texts[5].strip() if len(texts) > 5 else ""

        pgn_file = ""
        viewer_hint = ""
        for link in row.select("a[href]"):
            href = link.get("href", "").strip()
            if not href:
                continue
            if href.lower().endswith(".pgn"):
                pgn_file = os.path.basename(href)
                break
            if href.lower().endswith("-viewer.html"):
                viewer_hint = os.path.basename(href)
                pgn_file = os.path.basename(href).replace("-viewer.html", ".pgn")
                break

        tournaments.append({
            "Year": current_year,
            "Name": name,
            "Winner": winner,
            "StartDate": start_date,
            "EndDate": end_date,
            "GamesCount": games_count,
            "DateUpdated": date_updated,
            "PgnFileName": pgn_file,
        })

    return tournaments


def main():
    tournaments = parse_page()
    os.makedirs(os.path.dirname(JSON_OUT), exist_ok=True)
    unique_pgns = sorted({t["PgnFileName"] for t in tournaments if t.get("PgnFileName")})
    for pgn in unique_pgns:
        download_pgn(pgn)

    with open(JSON_OUT, "w", encoding="utf-8") as f:
        json.dump(tournaments, f, indent=2, ensure_ascii=False)

    print(f"Saved {len(tournaments)} tournaments to {JSON_OUT}")
    print(f"Attempted downloads for {len(unique_pgns)} PGN files in {PGN_DIR}")


if __name__ == "__main__":
    main()
