# AERL 1.1.4

AERL is a Windows companion for Rocket League focused on safe local customization, the documented local Stats API, an external HUD, training/session tracking, presets, replay tooling and quality-of-life features.

## Implemented now

- Premium WPF shell, onboarding, sidebar and command palette (`Ctrl+K`)
- English / French live UI switching
- Epic Games / Steam installation + process detection
- Complete Mock Mode: game detection + simulated live match stream, HUD, stats and replay commands for testing without Rocket League
- Official Rocket League Stats API WebSocket client with automatic reconnect
- Safe configuration of `TAStatsAPI.ini` / `DefaultStatsAPI.ini` with backup
- Live score, clock, teams, players, goals, shots, saves, assists, demos, arena and ball speed
- External top-most click-through HUD
- Official replay commands used by AERL: LoadReplay, SetGameSpeed, SetMatchPaused and ChangePOV
- Official SetHUDVisibility command
- Local Garage presets plus an experimental local cosmetic swap engine for compatible Rocket League `.upk`/`.bnk` packages
- Item-to-item local swaps: choose the item actually equipped and the item AERL should render locally
- Vanilla backup, SHA-256 verification, Restore Selected/Restore All, update mismatch protection, and custom package import
- Preset import/export + active preset
- Training session tracker + persistent history
- Freeplay/drill timer
- Local match/session history
- Lightweight Windows process monitoring
- Permission-aware declarative plugin catalog with enable/disable state
- Windows startup, tray icon, Safe Mode and crash recovery
- Config/log/data separation under `%APPDATA%\AERL`
- Update-manifest checker
- Dependency-free smoke-test project
- Self-contained Windows x64 publish + one-file Inno Setup installer

## Safety boundary

The local cosmetic engine uses file-level package editing only; it does not inject into Rocket League or hook game memory. AERL requires Rocket League to be closed while applying/restoring files. It is a third-party modification and is not guaranteed or endorsed by Epic/Psyonix; future game or anti-cheat updates can break compatibility.

AERL does not disable Easy Anti-Cheat, hide from it, inject prohibited code, patch protected game memory, spoof inventory ownership, alter ranks/MMR/XP/credits, modify server-side inventory, manipulate packets for an advantage or automate competitive gameplay.

If Rocket League does not expose a capability through a legitimate accessible method, AERL keeps that capability local to AERL or leaves it unavailable instead of bypassing game security.

## Build the public installer

Developer PC requirements:

1. Windows 10/11 x64
2. .NET 8 SDK
3. Inno Setup 6

Double-click:

`build-release.bat`

The release pipeline:

1. checks the .NET SDK;
2. runs AERL smoke tests;
3. publishes the self-contained Windows x64 app;
4. builds the setup with Inno Setup;
5. prints the SHA-256 of the final installer.

Successful public output:

`dist\AERL_Setup_1.1.4.exe`

That setup EXE is the only file end users need. When opened, it lets them choose the installation directory and extracts the complete AERL application there.

## Rocket League Stats API

Open **Settings → Official Stats API → Configure Rocket League**. AERL detects the Rocket League install, creates a backup of the relevant Stats API INI when possible, and enables WebSocket telemetry with the configured port/rate. Restart Rocket League after changing that configuration.

If you want to test AERL before launching Rocket League, enable **Developer Mock Mode** in Settings and save. The Stats/HUD pages immediately receive simulated match data.

## User data

AERL keeps user-generated data separate from the application binaries:

`%APPDATA%\AERL\Config`
`%APPDATA%\AERL\Presets`
`%APPDATA%\AERL\Plugins`
`%APPDATA%\AERL\Cache`
`%APPDATA%\AERL\Sessions`
`%APPDATA%\AERL\Logs`
`%APPDATA%\AERL\Backups`

The uninstaller asks whether these personal AERL files should be kept or removed.


## Local cosmetic data

On first use of the Garage swapper, AERL downloads the public `items.json` and `keys.txt` metadata from the Alphy-Swapper repository into `%APPDATA%\AERL\Cache\RuntimeData`. It downloads metadata only, never executable code. Existing cache is reused offline.
