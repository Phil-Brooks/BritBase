import json
import os
import re

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
JSON_OUT = os.path.abspath(os.path.join(SCRIPT_DIR, "..", "BritBase", "Data", "brit30.json"))
PGN_DIR = os.path.abspath(os.path.join(SCRIPT_DIR, "..", "BritBase", "Data", "pgn"))


def extract_pgn_metadata(path: str):
    with open(path, "r", encoding="utf-8", errors="ignore") as f:
        text = f.read()

    event_match = re.search(r'\[Event "([^"]+)"\]', text)
    site_match = re.search(r'\[Site "([^"]+)"\]', text)
    date_match = re.search(r'\[Date "(\d{4}\.\d{2}\.\d{2})"\]', text)
    source_date_match = re.search(r'\[SourceDate "(\d{4}\.\d{2}\.\d{2})"\]', text)
    source_quality_match = re.search(r'\[SourceQuality "([^"]+)"\]', text)

    return {
        "Event": event_match.group(1) if event_match else os.path.splitext(os.path.basename(path))[0],
        "Site": site_match.group(1) if site_match else "",
        "Date": date_match.group(1) if date_match else "",
        "SourceDate": source_date_match.group(1) if source_date_match else "",
        "SourceQuality": source_quality_match.group(1) if source_quality_match else "",
    }


def count_games(path: str):
    with open(path, "r", encoding="utf-8", errors="ignore") as f:
        text = f.read()
    return len(re.findall(r'\n\s*\[Event "', text))


def main():
    records = []
    for name in sorted(os.listdir(PGN_DIR)):
        if not name.lower().endswith('.pgn'):
            continue
        if not re.match(r'193\d', name):
            continue

        full_path = os.path.join(PGN_DIR, name)
        meta = extract_pgn_metadata(full_path)
        year = meta["Date"][:4] if meta["Date"] else name[:4]
        games_count = count_games(full_path)

        records.append({
            "Year": int(year),
            "Name": meta["Event"],
            "Winner": "",
            "StartDate": meta["Date"],
            "EndDate": "",
            "GamesCount": str(games_count),
            "DateUpdated": meta["SourceDate"] or meta["SourceQuality"] or "",
            "PgnFileName": name,
        })

    os.makedirs(os.path.dirname(JSON_OUT), exist_ok=True)
    with open(JSON_OUT, 'w', encoding='utf-8') as f:
        json.dump(records, f, indent=2, ensure_ascii=False)

    print(f"Saved {len(records)} tournaments to {JSON_OUT}")


if __name__ == "__main__":
    main()
