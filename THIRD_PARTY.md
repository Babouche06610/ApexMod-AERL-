# Third-party references

AERL is an independent project. Its local cosmetic workflow was informed by publicly available documentation and legacy source from **Alphy** / **Alphy-Swapper** by AC-Storm-YT.

Runtime metadata sources used by AERL:

- `items.json` — Alphy-Swapper public repository
- `keys.txt` — Alphy-Swapper public repository

AERL downloads these data files only when the local cosmetic module is first initialized. It does **not** download or execute Alphy binaries or Python code.

Alphy/Alphy-Swapper remain third-party projects with their own authorship and licensing. AERL is not affiliated with Epic Games, Psyonix, Alphy, or BakkesMod.


## VelocityRL Alpha Compatibility Engine
AERL 1.6.3 optionally routes Alpha Reward cosmetic package generation through the upstream VelocityRL Python swap engine.
Upstream: https://github.com/bitsfdb/VelocityRL
License: GPL-3.0 (see AlphaEngine/LICENSE_VELOCITYRL.txt in built distributions).
AERL invokes it as a separate local helper process; no injection or memory patching is performed.
