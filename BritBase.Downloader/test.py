import requests

test_url = "https://web.archive.org/web/20230601000000id_/http://www.saund.co.uk/britbase/hastings.htm"

try:
    r = requests.get(test_url, headers={"User-Agent": "Mozilla/5.0"}, timeout=15)
    print(f"Status Code: {r.status_code}")
    print(f"Bytes received: {len(r.content)}")
    print("Sample content from archive:", r.text[:200])
except Exception as e:
    print(f"Error: {e}")