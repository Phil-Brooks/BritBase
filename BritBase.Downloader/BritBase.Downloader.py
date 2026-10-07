import os
import io
import time
import zipfile
import requests
from urllib.parse import urljoin
from bs4 import BeautifulSoup
from tqdm import tqdm

# Destination folder in your ASP.NET Core project
SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
OUTPUT_DIR = os.path.abspath(os.path.join(SCRIPT_DIR, "..", "BritBase", "Data", "pgn"))

# Internet Archive raw snapshot prefix (2023 snapshot of BritBase)
ARCHIVE_PREFIX = "https://web.archive.org/web/20230601000000id_/"

# Main BritBase archive hubs to scrape
HUB_URLS = [
    "http://www.saund.co.uk/britbase/hastings.htm",
    "http://www.saund.co.uk/britbase/britchamps.html",
    "http://www.saund.co.uk/britbase/varsity.html",
    "http://www.saund.co.uk/britbase/players.html",
    "http://www.saund.co.uk/britbase/1920-1929.html",
    "http://www.saund.co.uk/britbase/1930-1939.html",
    "http://www.saund.co.uk/britbase/1940-1949.html",
    "http://www.saund.co.uk/britbase/1950-1959.html",
    "http://www.saund.co.uk/britbase/1960-1969.html",
    "http://www.saund.co.uk/britbase/1970-1979.html",
    "http://www.saund.co.uk/britbase/1980-1989.html",
    "http://www.saund.co.uk/britbase/1990-1999.html",
    "http://www.saund.co.uk/britbase/2000-2009.html",
    "http://www.saund.co.uk/britbase/2010-2019.html",
]

HEADERS = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:128.0) Gecko/20100101 Firefox/128.0"
}


def ensure_output_dir():
    os.makedirs(OUTPUT_DIR, exist_ok=True)
    print(f"[*] Target PGN directory: {OUTPUT_DIR}\n")


def make_archive_url(original_url):
    """Wraps original url in Wayback Machine raw snapshot format."""
    # Remove any existing archive.org wrapper if present
    if "web.archive.org" in original_url:
        return original_url
    return f"{ARCHIVE_PREFIX}{original_url}"


def download_file(url, session):
    filename = url.split("/")[-1].split("?")[0]
    if not filename:
        return None

    dest_check = os.path.join(OUTPUT_DIR, filename)
    if os.path.exists(dest_check) and os.path.getsize(dest_check) > 0:
        print(f"  [skip] Already downloaded: {filename}")
        return None

    archive_url = make_archive_url(url)

    try:
        response = session.get(archive_url, stream=True, timeout=30)
        if response.status_code != 200:
            print(f"  [!] HTTP {response.status_code} for {filename}")
            return None
    except Exception as e:
        print(f"  [!] Failed to download {filename}: {e}")
        return None

    total_size = int(response.headers.get("content-length", 0))
    chunks = []

    with tqdm(total=total_size, unit="B", unit_scale=True, desc=f"  {filename[:25]}") as pbar:
        for chunk in response.iter_content(chunk_size=8192):
            if chunk:
                chunks.append(chunk)
                pbar.update(len(chunk))

    return filename, b"".join(chunks)


def save_pgn_or_extract_zip(filename, data):
    lower = filename.lower()

    if lower.endswith(".pgn"):
        dest = os.path.join(OUTPUT_DIR, filename)
        with open(dest, "wb") as f:
            f.write(data)
        print(f"    [+] Saved PGN: {filename}")

    elif lower.endswith(".zip"):
        try:
            with zipfile.ZipFile(io.BytesIO(data)) as z:
                count = 0
                for member in z.namelist():
                    if member.lower().endswith(".pgn"):
                        base = os.path.basename(member)
                        if base:
                            dest = os.path.join(OUTPUT_DIR, base)
                            with open(dest, "wb") as f:
                                f.write(z.read(member))
                            count += 1
                print(f"    [+] Extracted {count} PGN(s) from {filename}")
        except zipfile.BadZipFile:
            print(f"    [!] Not a valid zip: {filename}")


def scrape_hub_page(hub_url, session):
    """Scrapes a hub page via archive.org and discovers all direct and viewer PGN/ZIP files."""
    archive_hub = make_archive_url(hub_url)
    downloads = set()
    viewer_pages = set()

    try:
        resp = session.get(archive_hub, timeout=20)
        if resp.status_code != 200:
            return downloads
    except Exception as e:
        print(f"    [!] Error accessing {hub_url}: {e}")
        return downloads

    soup = BeautifulSoup(resp.text, "html.parser")

    for a in soup.find_all("a", href=True):
        href = a["href"].strip()

        # Clean off any Wayback machine internal URL parts if present
        if "/http" in href:
            href = "http" + href.split("/http", 1)[1]

        full_url = urljoin(hub_url, href)
        clean_url = full_url.split("?")[0].lower()

        if clean_url.endswith((".pgn", ".zip")):
            downloads.add(full_url)
        elif "viewer.htm" in clean_url:
            viewer_pages.add(full_url)

    # Scrape any viewer pages found (e.g. British Championships)
    for v_url in viewer_pages:
        try:
            v_archive = make_archive_url(v_url)
            v_resp = session.get(v_archive, timeout=10)
            if v_resp.status_code == 200:
                v_soup = BeautifulSoup(v_resp.text, "html.parser")
                for va in v_soup.find_all("a", href=True):
                    va_href = va["href"].strip()
                    if "/http" in va_href:
                        va_href = "http" + va_href.split("/http", 1)[1]
                    va_full = urljoin(v_url, va_href)
                    if va_full.split("?")[0].lower().endswith((".pgn", ".zip")):
                        downloads.add(va_full)
        except Exception:
            pass

    return downloads


def main():
    ensure_output_dir()
    session = requests.Session()
    session.headers.update(HEADERS)

    print("Step 1: Discovering files via Internet Archive mirror...")
    all_links = set()

    for hub in HUB_PAGES:
        hub_name = hub.split("/")[-1]
        print(f"  Scanning: {hub_name} ...", end=" ", flush=True)
        found = scrape_hub_page(hub, session)
        print(f"found {len(found)} file(s)")
        all_links.update(found)

    print(f"\nTotal unique archives discovered: {len(all_links)}")

    if not all_links:
        print("[!] No archives found.")
        return

    print("\nStep 2: Downloading and extracting archives into Data/pgn...")
    for link in sorted(all_links):
        result = download_file(link, session)
        if result:
            fname, content = result
            save_pgn_or_extract_zip(fname, content)
        time.sleep(0.1)  # Be polite to archive.org

    pgn_files = [f for f in os.listdir(OUTPUT_DIR) if f.lower().endswith(".pgn")]
    print(f"\n[DONE] Finished! You now have {len(pgn_files)} PGN files in {OUTPUT_DIR}")


if __name__ == "__main__":
    HUB_PAGES = HUB_URLS
    main()