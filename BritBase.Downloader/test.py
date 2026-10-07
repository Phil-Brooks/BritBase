import requests
from bs4 import BeautifulSoup

url = "https://web.archive.org/web/20230601000000id_/http://www.saund.co.uk/britbase/brit80.htm"
headers = {"User-Agent": "Mozilla/5.0"}
resp = requests.get(url, headers=headers)
soup = BeautifulSoup(resp.text, "html.parser")

# Check for <pre> tags
pres = soup.find_all("pre")
print(f"Number of <pre> tags: {len(pres)}")
if pres:
    for idx, p in enumerate(pres):
        print(f"PRE {idx} (first 300 chars):")
        print(p.get_text()[:300])
        print("-" * 40)

# Check all links on the page that mention 'pgn' or '1980'
print("\nLinks containing 'pgn' or '198':")
for a in soup.find_all("a", href=True):
    href = a["href"]
    text = a.get_text(strip=True)
    if any(k in href.lower() for k in ["pgn", "198", "sutton"]):
        print(f"  href: {href} | text: {text}")