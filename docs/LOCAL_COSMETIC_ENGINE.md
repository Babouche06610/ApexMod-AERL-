# AERL Local Cosmetic Engine

AERL 1.1 adds a client-side Rocket League asset-swap engine for compatible `.upk` item packages.

## What it does

- Does not inject DLLs into Rocket League.
- Does not edit RocketLeague.exe memory.
- Does not hook the game process.
- Does not disable, patch, spoof, or communicate with Easy Anti-Cheat.
- Reads donor and target `.upk` packages from the user's own Rocket League installation.
- Builds a header-retargeted visual package locally.
- Backs up the real target package under `%APPDATA%\AERL\Backups\Originals`.
- Writes only the target local asset package.
- Tracks SHA-256 hashes and can restore all files.

## Workflow

1. Close Rocket League.
2. Open Garage > Local Cosmetic Swapper.
3. Choose the slot/category.
4. Choose **Item you equip** (the real item you equip in Rocket League).
5. Choose **Item you see** (the visual donor).
6. Click **Apply Locally**.
7. Launch Rocket League normally.
8. To remove the swap, close Rocket League and click **Restore All**.

The catalog/key metadata is downloaded on first use from the public Alphy-Swapper repository and cached locally. AERL does not download Rocket League cosmetic asset packages: donor assets are read from the user's own game installation.

## Compatibility

Not every arbitrary pair is guaranteed compatible. AERL rejects slot mismatches and packages whose internal structure cannot be safely retargeted by the header-only engine.

Rocket League updates or file verification can overwrite modified files. Use **Verify Files** in AERL after an update, then restore/reapply as needed.

## Attribution

The swap workflow was independently implemented in C# after studying the public Alphy/Alphy-Swapper documentation and legacy source. AERL downloads only public metadata files (`items.json` and `keys.txt`) at runtime; it does not download or execute Alphy code.
