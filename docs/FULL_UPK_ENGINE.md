# AERL 1.4.0 — Full UPK Engine

AERL 1.4.0 replaces the legacy header-only cosmetic swapper with a complete local package rebuild pipeline.

Pipeline:
1. Resolve the exact donor and target package AES keys from the cached exact package map.
2. Decrypt the donor UPK header.
3. Parse Rocket League's 64-bit compressed chunk table.
4. Inflate every ZLIB block into a complete uncompressed UE3 package image.
5. Retarget the donor package's internal asset names to the equipped target item.
6. Rebuild the name/import/export table layout and patch 64-bit export serial offsets when header length changes.
7. Recompress all package chunks using the Rocket League ZLIB chunk container format.
8. Rebuild the chunk table and encrypted header.
9. Encrypt with the exact key of the package Rocket League expects to load.
10. Decrypt + decompress the generated output again and validate table/export bounds before the file transaction is committed.

This remains a local file operation. AERL does not open RocketLeague.exe, inject code, disable EAC, patch EAC, modify server inventory, or send altered network state.
