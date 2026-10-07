import requests
from bs4 import BeautifulSoup

url = "https://www.saund.co.uk/britbase/brit80.htm"
headers = {"User-Agent": "Mozilla/5.0"}
resp = requests.get(url, headers=headers)
soup = BeautifulSoup(resp.text, "html.parser")

print("Searching for 1982 Phillips & Drew / GLC links:")
for a in soup.find_all("a", href=True):
    text = a.get_text(strip=True)
    href = a["href"]
    if any(k in href.lower() or k in text.lower() for k in ["198204", "phillips", "glc"]):
        print(f"  text: '{text}' -> href: '{href}'")