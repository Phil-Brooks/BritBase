import os
import re
import json
import requests

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
PGN_DIR = os.path.abspath(os.path.join(SCRIPT_DIR, "..", "BritBase", "Data", "pgn"))
JSON_OUT = os.path.abspath(os.path.join(SCRIPT_DIR, "..", "BritBase", "Data", "brit80.json"))

ARCHIVE_BASE = "https://web.archive.org/web/20260401000000id_/http://www.saund.co.uk/britbase/pgn/"
HEADERS = {
    "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Firefox/128.0",
    "Referer": "http://www.saund.co.uk/britbase/brit80.htm"
}

# The exact data pasted directly from your prompt
RAW_DATA = """
1980
SCCU Champ'ship 	Peter Large, David Cummings 	1980.04.04 	1980.04.07 	17 	2025.10.25
Surrey Open 	John Nunn, Gyula Sax 	1980.04.04 	1980.04.07 	12^ 	2025.10.25
Phillips & Drew Kings 	Miles Korchnoi Andersson 	1980.04.10 	1980.04.25 	91 	2020.08.25
Phillips & Drew Kts 	vd Sterren, Botterill 	1980.04.11 	1980.04.25 	105^ 	2020.08.25
Lloyds Bank Masters 	Florin Gheorghiu 	1980.08.20 	1980.08.28 	132 	2021.02.28
Lloyds Bank Juniors 	  	1980.08.20 	1980.08.28 	3^ 	2021.02.28
GBR-ch Brighton 	Nunn, Hartston 	1980.08.04 	1980.08.15 	220 	2019.03.30
GBR-ch other 	  	1980.08.04 	1980.08.15 	15^ 	2019.03.30
GBR-ch Play-Off 	Nunn bt Hartston 	1981.02.13 	1981.02.17 	6^ 	2019.03.28
Manchester Bened. 	John Nunn 	1980.09.03 	1980.09.11 	104 	2024.02.18
Guernsey Open 6th 	five-way tie 	1980.10.19 	1980.10.25 	14 	2025.10.08
Jersey International 	Ian D Wells 	1980.10.26 	1980.11.01 	53 	2026.05.08
Brighton 	Chandler, Speelman 	1980.12.10 	1980.12.18 	45 	2020.08.19
Welsh Champ 1981 	A Howard Williams 	1980.12.28 	1981.01.03 	28 	2020.08.28
Hastings Premier 	Ulf Andersson 	1980.12.29 	1981.01.15 	120 	2022.07.04
Hastings Challengers 	M Rivas 	1980.12.29 	1981.01.08 	51+9^ 	2022.12.07
1981
ARC Young Masters 1st 	Murray Chandler 	1981.02.27 	1981.03.01 	138 	2022.08.01
Botvinnik Simul 	Botvinnik 4½-3½ 	1981.04.08 	  	4+1 	2024.06.24
West of England Ch 	Peter H Clarke 	1981.04.16 	1981.04.19 	34 	2024.07.19
West of England other 	  	1981.04.16 	1981.04.19 	6^ 	2024.07.19
SCCU Champ'ship 	John Pigott 	1981.04.17 	1981.04.20 	6 	2025.12.07
Surrey Open 	M Franklin, B Jacobs 	1981.04.17 	1981.04.20 	7^ 	2025.12.07
Staffordshire Open 	John Carleton 	1981.05.23 	1981.05.27 	87 	2023.12.18
GBR-ch Morecambe 	Paul Littlewood 	1981.08.03 	1981.08.14 	286 	2019.06.20
GBR-ch other 	  	1981.08.03 	1981.08.14 	4^ 	2022.12.20
Lloyds Bank Masters 	Keene, Seirawan, Miles 	1981.08.25 	1981.09.02 	129+7 	2025.08.02
Berks & Bucks C'gress 	W Hartston, S Taulbut 	1981.08.28 	1981.08.31 	10 	2026.06.15
Manchester Bened. 	Tony Miles 	1981.09.04 	1981.09.12 	116 	2020.09.06
Guernsey Open 7th 	five-way tie 	1981.10.18 	1981.10.24 	14 	2025.10.08
Lewisham Intern'l 	Mark Hebden 	1981.11.24 	1981.12.03 	173 	2023.07.16
Regency Ramsgate 	John Fedorowicz 	1981.12.05 	1981.12.13 	249 	2021.09.05
Brighton 	Murray Chandler 	1981.12.13 	1981.12.21 	45 	2020.09.06
Hastings Premier 	Viktor Kupreichik 	1981.12.28 	1982.01.12 	91 	2025.01.04
Hastings Challengers 	Jim Plaskett 	1981.12.28 	1982.01.07 	10^ 	2025.08.02
Hastings other 	  	1981.12.28 	1982.01.07 	6^ 	Hastings
Welsh Champ 1982 	A Howard Williams 	1981.12 	1982.01 	28 	2020.09.06
1982
Kings Head Inter'l 	Jonathan Tisdall 	1982.01.31 	1982.02.07 	45 	2020.10.14
ARC Young Masters 2nd 	Speelman, Britton, Martin 	1982.02.26 	1982.02.28 	9 	2025.10.13
Sweden v England 	England (by 9½-6½) 	1982.03.13 	1982.03.14 	16+1 	2025.12.24
Karpov simul, 25 bds 	Karpov 16, ENG Juniors 9 	1982.04.13 	bulletin 	5 	2026.04.04
London GLC Kings 	Andersson, Karpov 	1982.04.15 	1982.04.30 	91 	2026.01.11
London GLC Kts 	William Watson 	1982.04.15 	1982.04.30 	120^ 	2026.01.11
Robert Silk YM 	William Watson 	1982.07.13 	1982.07.22 	45 	2007.05.02
GBR-ch Torquay 	Tony Miles 	1982.08.02 	1982.08.13 	308 	2020.09.18
GBR-ch other 	  	1982.08.02 	1982.08.13 	2^ 	2021.08.19
Lloyds Bank Masters 	Tony Miles 	1982.08.25 	1982.09.02 	149 	2022.12.30
Berks & Bucks C'gress 	William Hartston 	1982.08.27 	1982.08.30 	10 	2026.06.15
Manchester Benedict. 	Miles, Kudrin 	1982.09.08 	1982.09.16 	111+6 	2020.11.05
Guernsey Open 8th 	Jim Plaskett 	1982.10.17 	1982.10.23 	15 	2025.10.08
Lewisham Int'l 	Jim Plaskett 	1982.11.22 	1982.12.01 	88 	2023.01.24
Regency Ramsgate 	Mark Hebden 	1982.12.04 	1982.12.12 	92 	2021.10.13
Brighton Kt Flight 	3-way-tie 	1982.12.13 	1982.12.21 	55 	2022.12.24
Hastings Premier 	Rafael Vaganian 	1982.12.28 	1983.01.12 	91 	2025.01.09
Hastings Challengers 	Andrew D Martin 	1982.12.28 	1983.01.12 	4^ 	2026.01.21
Hastings other 	  	1982.12.28 	1983.01.12 	0 	See Hastings Page
1983
ARC Young Masters 3rd 	Kosten, Nunn, P Littlewood, Short 	1983.02.25 	1983.02.27 	15 	2025.10.13
Midland Indiv Champ 	Geoff Lawton 	1983.03.25 	1983.03.27 	30 	2023.07.08
BBC TV Master Game 	Tony Miles 	1983.04 	  	(25) 	 
Charlton 	Hebden, Plaskett, etc 	1983.04.05 	1983.04.15 	96+17 	1998.11.15
Lloyds Bank Masters 	Yuri Razuvaev 	1983.08.24 	1983.09.01 	142+1 	2026.05.11
GBR-ch Southport 	Jonathan Mestel 	1983.08.08 	1983.08.19 	319 	2023.01.13
GBR-ch others 	  	1983.08.08 	1983.08.19 	35^ 	2024.09.14
Manchester Benedict. 	Jim Plaskett 	1983.09.07 	1983.09.15 	103/225 	2022.07.10
Guernsey Open 9th 	Bruno Carlier 	1983.10.16 	1983.10.22 	14 	2025.10.08
BBC TV Master Game 	Anatoly Karpov 	1983.11 	  	0 	 
London Candidates 	Kasparov, Korchnoi 	1983.11.21 	1983.12.16 	(11) 	 
London Candidates 	Smyslov, Ribli 	1983.11.21 	1983.12.16 	(11) 	 
Lewisham Int'l 	Jim Plaskett 	1983.11.24 	1983.12.02 	101 	2022.04.19
Lewisham others 	  	1983.11.24 	1983.12.02 	7^ 	2021.08.11
Regency Ramsgate 	Mark Hebden 	1983.12.03 	1983.12.11 	100/222 	2021.10.14
Brighton 	John Nunn 	1983.12.12 	1983.12.20 	45 	2022.07.11
Welsh Champ 1984 	John G Cooper 	1983.12.27 	1984.01.02 	28 	2026.01.26
Hastings Premier 	L Karlsson, J Speelman 	1983.12.28 	1984.01.12 	91 	2025.01.02
Hastings Challengers 	G Flear, K Berg 	1983.12.28 	1984.01.06 	19^ 	2026.05.07
Hastings other 	  	1983.12.28 	1984.01.08 	7^ 	See Hastings Page
1984
Midland Individ Ch 	Mark Hebden 	1984.02.17 	1984.02.19 	26 	2022.08.23
ARC Young Masters 4th 	Flear, P Littlewood, Speelman 	1984.02.24 	1984.02.26 	13 	2025.10.13
Oakham Masters 	Niaz Murshed 	1984.04.08 	1984.04.17 	180 	2023.08.08
West of England Ch 	Gary W Lane 	1984.04.19 	1984.04.23 	37+2 	2024.07.19
Phillips & D/GLC Kings 	Anatoly Karpov 	1984.04.26 	1984.05.11 	91 	2022.07.11
Phillips & D/GLC Kts 	D Johansen, P Large 	1984.04.26 	1984.05.11 	111^ 	2022.07.11
USSR-World Match 	USSR 	1984.06.24 	1984.06.29 	(40) 	 
Oxford International 	Gert Ligterink 	1984.06.25 	1984.07.03 	45 	2021.06.19
SCO Centenary, Troon 	Lev Psakhis 	1984.07.12 	1984.07.20 	45 	2024.02.18
SCO Open Ch'ship 	  	1984.07.14 	1984.07.20 	15^ 	2024.02.18
Robert Silk YM 	William Hartston 	1984.07.15 	1984.07.23 	45 	2022.07.11
Robert Silk Lady M 	N Hoiberg 	1984.07.15 	1984.07.23 	45^ 	2022.07.11
Glorney Cup ENG 	England 	1984.07.15 	1984.07.18 	0 	 
GBR-ch Brighton 	Nigel Short 	1984.07.30 	1984.08.10 	347 	2020.09.17
GBR-ch others 	  	1984.07.30 	1984.08.10 	33^ 	2024.07.12
Lloyds Bank Under-21 	Viswanathan Anand 	1984.08.20 	1984.08.21 	0 	 
Lloyds Bank Masters 	John Nunn 	1984.08.22 	1984.08.30 	184 	2021.07.05
Lewisham Int'l 	M Hebden, G de Boer 	1984.09.04 	1984.09.14 	112 	2021.08.19
NatWest YM 	4-way tie 	1984.09 	1984.10.06 	45 	2024.02.19
Chequers Tournament 	Gavin Crawley 	1984.10? 	  	(45) 	 
Guernsey Open 10th 	Bruno Carlier, Mark Hebden 	1984.10.14 	1984.10.20 	8 	2025.10.08
Regency Ramsgate 	4-way tie 	1984.12.08 	1984.12.16 	212 	2021.08.19
Brighton Zonal 	Jonathan Speelman 	1984.12.12 	1984.12.20 	45 	2022.07.12
Welsh Champ 1985 	John G Cooper 	1984.12.27 	1985.01.02 	28 	2022.07.11
Hastings Premier 	Evgeny Sveshnikov 	1984.12.29 	1985.01.13 	91 	2025.06.19
Hastings Challengers 	Ed Formanek 	1984.12.29 	1985.01.07 	12^ 	2025.06.19
Hastings other 	  	1984.12.29 	1985.01.07 	  	See Hastings Page
1985
Plymouth Under-18 	Neil McDonald 	1985.01.02 	1985.01.06 	0 	 
ARC Young Masters 5th 	4-way tie 	1985.02.08 	1985.02.10 	18 	2023.05.21
Midland Individ Ch 	Chris Baker, Simon Small 	1985.02.15 	1985.02.17 	45 	2022.08.22
Commonwealth Ch 	Spraggett, Thipsay 	1985.02.16 	1985.02.26 	199 	2023.08.22
West of England Ch 	Gary W Lane 	1985.04.04 	1985.04.08 	40+1 	2024.07.20
West of England other 	  	1985.04.04 	1985.04.08 	8^ 	2024.07.20
Chequers Masters 	Hebden, Condie, McNab 	1985.04.06 	1985.04.17 	45 	2022.07.12
Jersey Open 	Erik O M C Teichmann 	1985.05.04 	1985.05.10 	0 	 
Glorney Cup, NED 	  	1985.07.15 	1985.07.19 	0 	 
GBR-ch Edinburgh 	Jonathan Speelman 	1985.07.29 	1985.08.09 	418 	2023.02.20
GBR-ch others 	  	1985.07.29 	1985.08.09 	19^ 	2024.10.19
NatWest Team 	England 	1985.08.11 	1985.08.20 	54 	2023.02.28
Lloyds Bank Masters 	Alexander Belyavsky 	1985.08.21 	1985.08.29 	185 	2023.02.20
Berks & Bucks C'gress 	S Taulbut, D Cummings 	1985.08.24 	1985.08.26 	11 	2026.06.15
NatWest YM 	David Norwood 	1985.09.07 	1985.09.15 	45 	2022.07.12
London Cand.Res. 	Speelman, Gavrikov, vd Wiel 	1985.09.04 	1985.09.17 	(12) 	 
Guernsey Open 11th 	S Conquest, J Hodgson 	1985.10.13 	1985.10.19 	97 	2025.10.08
Guernsey Holiday 	Wim Badenhop 	1985.10.13 	1985.10.18 	9^ 	2025.10.08
Brighton 	Andrew Whiteley 	1985.12.12 	1985.12.18 	45 	2020.10.19
Hastings Premier 	Margeir Petursson 	1985.12.28 	1986.01.12 	91 	2026.01.29
Hastings Challengers 	Peter Large 	1985.12.28 	1986.01.06 	13^ 	2025.10.07
Hastings other 	  	1985.12.28 	1986.01.06 	0 	See Hastings Page
1986
ARC Young Masters 6th 	James Plaskett 	1986.02.21 	1986.02.23 	3 	2025.10.13
Midland Indiv Ch 	K Arkell, C Baker, G Lawton 	1986.03.07 	1986.03.09 	55 	2016.01.18
GLC Challenge 	Glenn Flear 	1986.03.11 	1986.03.27 	91 	2022.07.12
GLC Masters 	J Levitt, N McDonald 	1986.03.11 	1986.03.27 	120^ 	2022.07.12
Oakham Masters 	Robert Kuczynski 	1986.04.02 	1986.04.10 	189 	2022.07.12
Chequers Invitation 	K Arkell, N McDonald 	1986.05.19 	1986.05.30 	(45) 	 
Speelman-Alburt m 	  	1986.05 	  	(8) 	 
GBR-ch Southamp'n 	Speelman, etc 	1986.07.28 	1986.08.08 	341 	2020.09.13
GBR-ch other 	  	1986.07.28 	1986.08.08 	1^ 	2023.06.26
GBR-ch Play-Off 	Jonathan Speelman 	1986.12.11 	1986.12.17 	8^ 	2020.09.13
Commonwealth Ch 	Hjartarson, Prasad 	1986.08.11 	1986.08.19 	29 	2023.07.02
Lloyds Bank Masters 	Simen Agdestein 	1986.08.20 	1986.08.28 	145 	2023.06.26
NatWest YM's 	Pedersen, Norwood 	1986.08.30 	1986.09.07 	45 	2022.07.12
Guernsey Open 12th 	B Carlier, R Harris, JM Hodgson 	1986.10.12 	1986.10.18 	103 	2025.12.16
Guernsey Holiday 	H Enevoldsen 	1986.10.12 	1986.10.18 	9^ 	2025.12.16
Hastings Premier 	M Chandler, etc 	1986.12.29 	1987.01.13 	91 	2020.08.20
Hastings Challengers 	Nigel Davies 	1986.12.29 	1987.01 	6^ 	Hastings
1987
Nott'm Chessforce 	Keith Arkell 	1987.01.20 	1987.01.29 	45+PDF 	2016.01.18
Bath Zonal 	Jonathan Speelman 	1987.02.14 	1987.02.24 	55 	2026.04.09
Bath Women's Zonal 	Susan Arkell (»Lalic) 	1987.02.19 	1987.02.24 	15+2^ 	2026.04.09
ARC Young Masters 7th 	Nigel Davies 	1987.02.27 	1987.03.01 	15 	2025.10.13
West of England Ch 	Michael Adams 	1987.04.16 	1987.04.20 	0 	2022.02.08
GBR-ch Swansea 	Nigel Short 	1987.08.03 	1987.08.14 	321 	2020.09.13
Lloyds Bank Masters 	Michael Wilder 	1987.08.22 	1987.08.31 	213 	2023.08.20
Chess for Peace 	Julian Hodgson 	1987.09.01 	1987.09.11 	167 	2023.08.14
NatWest YM 	Gary Lane 	1987.10.03 	1987.10.11 	45 	2023.04.30
Guernsey Open 13th 	G Ballon, B Carlier 	1987.10.18 	1987.10.24 	105 	2025.12.16
Guernsey Holiday 	A Molet 	1987.10.18 	1987.10.24 	9^ 	2025.12.16
Hastings Premier 	Nigel Short 	1987.12.29 	1988.01.14 	56 	2025.01.08
Hastings Challengers 	Tony Kosten 	1987.12.29 	1988.01.07 	33^ 	2025.01.08
Hastings others 	Hastings 	1987.12.29 	1988.01.07 	21^ 	2026.07.14
1988
ARC Young Masters 8th 	John Fedorowicz 	1988.02.26 	1988.02.28 	5 	2025.10.13
Midland Indiv Ch 	Graham Waddingham 	1988.03.18 	1988.03.20 	69 	2022.11.29
Oakham Masters 	James Howell 	1988.03.28 	1988.04.05 	233+PDF 	2026.01.26
Watson, Farley & W 	Paul Motwani 	1988.05.27 	1988.06.07 	66 	2026.09.02
Haringey Masters 	Julian Hodgson, J Murey 	1988.07.21 	1988.07.29 	81 	2020.10.21
GBR-ch Blackpool 	Jonathan Mestel 	1988.08.01 	1988.08.12 	305 	2023.04.30
GBR-ch other 	  	1988.08.01 	1988.08.12 	1^ 	2023.04.30
Lloyds Bank Masters 	Gary Lane 	1988.08.20 	1988.08.29 	206 	2025.08.07
NatWest YM 	David Norwood, etc 	1988.08.31 	1988.09.09 	45 	2023.04.30
Guernsey Open 14th 	Marinus Kuijf 	1988.10.16 	1988.10.22 	107 	2025.08.25
Duncan Lawrie 	Judit Polgar 	1988.10.22 	1988.10.30 	45 	2025.08.25
Hastings Premier 	Nigel Short 	1988.12.29 	1989.01.14 	51 	2023.05.23
Hastings Challengers 	Judit Polgar 	1988.12.28 	1989.01.06 	111^ 	2025.01.15
Hastings other 	  	1988.12.28 	1989.01.06 	9^ 	2025.10.12
1989
Barnsdale Young M's 	Mihai Suba 	1989.02.24 	1989.02.26 	47/230 	2026.08.11
Midland Indiv Ch 	Andrew Ledger 	1989.03.10 	1989.03.12 	69 	2022.11.28
Edinburgh Open 	Julian Hodgson 	1989.03.24 	1989.04.01 	216 	2025.08.03
Watson, Farley & W 	Bent Larsen 	1989.05.19 	1989.06.01 	91 	2025.08.03
Northumberland Open 	David J Walker 	1989.05.26 	1989.05.29 	106 	2025.10.19
Park Hall International 	M Adams (2nd GM norm) 	1989.06.18 	1989.06.26 	45 	2025.08.04
British Rapidplay Ch 	John Nunn 	1989.07.01 	1989.07.02 	14 	2025.08.05
Haringey Masters 	M Adams (final GM norm) 	1989.07.11 	1989.07.21 	81 	2025.08.03
Scottish Championship 	Mark L Condie 	1989.07.13 	1989.07.21 	63 	2025.08.05
GBR-ch Plymouth 	Michael Adams 	1989.07.31 	1989.08.11 	315 	2024.08.29
GBR other 	  	1989.07.31 	1989.08.11 	2+1^ 	2026.04.25
Lloyds Bank Masters 	Zurab Azmaiparashvili 	1989.08.19 	1989.08.28 	223 	2021.07.25
NatWest Young GMs 	Dibyendu Barua 	1989.08.30 	1989.09.08 	45 	2023.04.30
Young England v Polgars 	Drawn (rapidplay) 	1989.11.12 	1989.11.13 	18 	2025.09.14
Hastings Premier 	Sergei Dolmatov 	1989.12.28 	1990.01.14 	56 	2025.01.16
Hastings Challengers 	Tony Kosten 	1989.12.28 	1990.01.07 	97^ 	2025.01.16
Hastings Masters 	(Scheveningen format) 	1990.01 	1990.01 	54^ 	2025.01.16
Hastings other 	See Hastings Page 	1989.12.28 	1990.01 	22^ 	2026.10.05
"""

# Map tournament name or date to the exact BritBase PGN file on the server
# (e.g. SCCU -> 198004sutton.pgn, Phillips & Drew -> 198004phillipsdrew.pgn, etc.)
PGN_MAP = {
    # 1980
    ("SCCU Champ'ship", 1980): "198004sutton.pgn",
    ("Surrey Open", 1980): "198004sutton.pgn", # games included with sutton
    ("Phillips & Drew Kings", 1980): "198004phillipsdrew.pgn",
    ("Phillips & Drew Kts", 1980): "198004phillipsdrew.pgn",
    ("Lloyds Bank Masters", 1980): "198008lloyds.pgn",
    ("Lloyds Bank Juniors", 1980): "198008lloyds.pgn",
    ("GBR-ch Brighton", 1980): "198008bcf.pgn",
    ("GBR-ch other", 1980): "198008bcf.pgn",
    ("GBR-ch Play-Off", 1980): "198008bcf.pgn",
    ("Manchester Bened.", 1980): "198009benedictine.pgn",
    ("Guernsey Open 6th", 1980): "198010guernsey.pgn",
    ("Jersey International", 1980): "198010jersey.pgn",
    ("Brighton", 1980): "198012brighton.pgn",
    ("Welsh Champ 1981", 1980): "198012wales.pgn",
    ("Hastings Premier", 1980): "198012hast.pgn",
    ("Hastings Challengers", 1980): "198012hast.pgn",

    # 1981
    ("ARC Young Masters 1st", 1981): "198102arcyoungmasters.pgn",
    ("West of England Ch", 1981): "198104wecu.pgn",
    ("Staffordshire Open", 1981): "198105staffs.pgn",
    ("GBR-ch Morecambe", 1981): "198108bcf.pgn",
    ("GBR-ch other", 1981): "198108bcf.pgn",
    ("Lloyds Bank Masters", 1981): "198108lloyds.pgn",
    ("Manchester Bened.", 1981): "198109benedictine.pgn",
    ("Lewisham Intern'l", 1981): "198111lewisham.pgn",
    ("Regency Ramsgate", 1981): "198112ramsgate.pgn",
    ("Brighton", 1981): "198112brighton.pgn",
    ("Hastings Premier", 1981): "198112hast.pgn",
    ("Hastings Challengers", 1981): "198112hast.pgn",
    ("Welsh Champ 1982", 1981): "198112wales.pgn",

    # 1982
    ("Kings Head Inter'l", 1982): "198201kingshead.pgn",
    ("London GLC Kings", 1982): "198204londonpdglc.pgn",
    ("London GLC Kts", 1982): "198204londonpdglc.pgn",
    ("Robert Silk YM", 1982): "198207robertsilk.pgn",
    ("GBR-ch Torquay", 1982): "198208bcf.pgn",
    ("GBR-ch other", 1982): "198208bcf.pgn",
    ("Lloyds Bank Masters", 1982): "198208lloyds.pgn",
    ("Manchester Benedict.", 1982): "198209benedictine.pgn",
    ("Lewisham Int'l", 1982): "198211lewisham.pgn",
    ("Regency Ramsgate", 1982): "198212ramsgate.pgn",
    ("Brighton Kt Flight", 1982): "198212brighton.pgn",
    ("Hastings Premier", 1982): "198212hast.pgn",
    ("Hastings Challengers", 1982): "198212hast.pgn",

    # 1983
    ("Midland Indiv Champ", 1983): "198303mccu.pgn",
    ("GBR-ch Southport", 1983): "198308bcf.pgn",
    ("GBR-ch others", 1983): "198308bcf.pgn",
    ("Lloyds Bank Masters", 1983): "198308lloyds.pgn",
    ("Manchester Benedict.", 1983): "198309benedictine.pgn",
    ("Lewisham Int'l", 1983): "198311lewisham.pgn",
    ("Regency Ramsgate", 1983): "198312ramsgate.pgn",
    ("Brighton", 1983): "198312brighton.pgn",
    ("Hastings Premier", 1983): "198312hast.pgn",

    # 1984
    ("Midland Individ Ch", 1984): "198402mccu.pgn",
    ("Phillips & D/GLC Kings", 1984): "198404londonpdglc.pgn",
    ("Phillips & D/GLC Kts", 1984): "198404londonpdglc.pgn",
    ("Oxford International", 1984): "198406oxford.pgn",
    ("Robert Silk YM", 1984): "198407robertsilk.pgn",
    ("GBR-ch Brighton", 1984): "198407bcf.pgn",
    ("GBR-ch others", 1984): "198407bcf.pgn",
    ("Lloyds Bank Masters", 1984): "198408lloyds.pgn",
    ("Lewisham Int'l", 1984): "198409lewisham.pgn",
    ("NatWest YM", 1984): "198409londonnwym.pgn",
    ("Regency Ramsgate", 1984): "198412ramsgate.pgn",
    ("Brighton Zonal", 1984): "198412brightonzonal.pgn",
    ("Welsh Champ 1985", 1984): "198412wlschamp.pgn",
    ("Hastings Premier", 1984): "198412hast.pgn",

    # 1985
    ("Midland Individ Ch", 1985): "198502mccu.pgn",
    ("Commonwealth Ch", 1985): "198502commonwealth.pgn",
    ("Chequers Masters", 1985): "198504chequers.pgn",
    ("GBR-ch Edinburgh", 1985): "198507bcf.pgn",
    ("GBR-ch others", 1985): "198507bcf.pgn",
    ("Lloyds Bank Masters", 1985): "198508lloyds.pgn",
    ("NatWest YM", 1985): "198509londonnwym.pgn",
    ("Brighton", 1985): "198512brighton.pgn",
    ("Hastings Premier", 1985): "198512hast.pgn",

    # 1986
    ("Midland Indiv Ch", 1986): "198603mccu.pgn",
    ("GLC Challenge", 1986): "198603glcchallenge.pgn",
    ("Oakham Masters", 1986): "198604oakham.pgn",
    ("GBR-ch Southamp'n", 1986): "198607bcf.pgn",
    ("GBR-ch other", 1986): "198607bcf.pgn",
    ("GBR-ch Play-Off", 1986): "198607bcf.pgn",
    ("Lloyds Bank Masters", 1986): "198608lloyds.pgn",
    ("NatWest YM's", 1986): "198608londonnwym.pgn",
    ("Hastings Premier", 1986): "198612hast.pgn",

    # 1987
    ("Nott'm Chessforce", 1987): "198701nott.pgn",
    ("GBR-ch Swansea", 1987): "198708bcf.pgn",
    ("Lloyds Bank Masters", 1987): "198708lloyds.pgn",
    ("Hastings Premier", 1987): "198712hast.pgn",

    # 1988
    ("Midland Indiv Ch", 1988): "198803mccu.pgn",
    ("Oakham Masters", 1988): "198803oakham.pgn",
    ("Haringey Masters", 1988): "198807haringey.pgn",
    ("GBR-ch Blackpool", 1988): "198808bcf.pgn",
    ("GBR-ch other", 1988): "198808bcf.pgn",
    ("Lloyds Bank Masters", 1988): "198808lloyds.pgn",
    ("Hastings Premier", 1988): "198812hast.pgn",

    # 1989
    ("Barnsdale Young M's", 1989): "198902barnsdale.pgn",
    ("Midland Indiv Ch", 1989): "198903mccu.pgn",
    ("GBR-ch Plymouth", 1989): "198907bcf.pgn",
    ("GBR other", 1989): "198907bcf.pgn",
    ("Lloyds Bank Masters", 1989): "198908lloyds.pgn",
    ("Hastings Premier", 1989): "198912hast.pgn",
}


def download_file_if_missing(filename):
    if not filename:
        return
    dest = os.path.join(PGN_DIR, filename)
    if os.path.exists(dest) and os.path.getsize(dest) > 0:
        return

    print(f"[*] Downloading missing PGN: {filename} ...")
    url = f"{ARCHIVE_BASE}{filename}"
    try:
        r = requests.get(url, headers=HEADERS, timeout=20)
        if r.status_code == 200 and len(r.content) > 0:
            with open(dest, "wb") as f:
                f.write(r.content)
            print(f"    [+] Saved {filename} ({len(r.content)} bytes)")
        else:
            print(f"    [!] HTTP {r.status_code} for {filename}")
    except Exception as e:
        print(f"    [!] Error downloading {filename}: {e}")


def main():
    os.makedirs(PGN_DIR, exist_ok=True)
    tournaments = []
    current_year = 1980

    for line in RAW_DATA.strip().splitlines():
        line = line.strip()
        if not line:
            continue

        if re.match(r"^198\d$", line):
            current_year = int(line)
            continue

        parts = [p.strip() for p in line.split("\t") if p.strip()]
        if len(parts) < 3:
            continue

        name = parts[0]
        winner = parts[1] if len(parts) > 1 else ""
        start_date = parts[2] if len(parts) > 2 else ""
        end_date = parts[3] if len(parts) > 3 else ""
        games_count = parts[4] if len(parts) > 4 else ""
        date_updated = parts[5] if len(parts) > 5 else ""

        # Map to accurate PGN file
        pgn_file = PGN_MAP.get((name, current_year), "")

        # If not in custom map, check if there's a date-based guess
        if not pgn_file and start_date:
            date_clean = start_date.replace(".", "")[:6] # e.g. 198004
            # Look in local PGN directory for matching prefix
            matches = [f for f in os.listdir(PGN_DIR) if f.startswith(date_clean)]
            if matches:
                pgn_file = matches[0]

        entry = {
            "Year": current_year,
            "Name": name,
            "Winner": winner,
            "StartDate": start_date,
            "EndDate": end_date,
            "GamesCount": games_count,
            "DateUpdated": date_updated,
            "PgnFileName": pgn_file
        }
        tournaments.append(entry)

        # Download if we have a filename and it's missing locally
        if pgn_file:
            download_file_if_missing(pgn_file)

    # Save to brit80.json
    with open(JSON_OUT, "w", encoding="utf-8") as f:
        json.dump(tournaments, f, indent=2, ensure_ascii=False)

    print(f"\n[DONE] Successfully processed {len(tournaments)} tournaments from 1980-1989!")
    print(f"Saved metadata to: {JSON_OUT}")


if __name__ == "__main__":
    main()