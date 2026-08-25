# AERL 1.0 — product and feasibility

## Product

AERL is a Windows Rocket League companion built around local AERL data, normal Windows integration and Rocket League's documented local Stats API. Game-specific functionality is capability-based: when a legitimate integration is unavailable, the app does not replace it with an anti-cheat bypass or protected-memory technique.

## Implemented safe capabilities

| Feature | Implementation | Scope | Status |
|---|---|---:|---|
| Rocket League detection | Epic/Steam paths + Windows process detection | Local OS | Implemented |
| Live match telemetry | Rocket League local Stats API over WebSocket | Local API | Implemented |
| Live HUD | External WPF click-through window | Local display | Implemented |
| Match/player stats | Stats API UpdateState/events | Local API | Implemented |
| Session history | JSON in AERL AppData | Local | Implemented |
| Training tracking | Observes Stats API counters during a user-started session | Local | Implemented |
| Freeplay drill timer | AERL timer | Local | Implemented |
| Replay load/speed/pause/POV | Documented Stats API commands; AERL restricts contextual controls to replay mode | Local API | Implemented |
| Game HUD visibility | Documented Stats API command | Local API | Implemented |
| Garage presets | AERL JSON presets and local preview | Local | Implemented |
| Preset import/export | JSON files | Local | Implemented |
| Plugins | Permission-aware declarative manifest catalog | Local | Implemented |
| Performance | Normal process state/RAM/thread metrics | Local OS | Implemented |
| Mock integration | Simulated detector + live Stats/HUD stream and commands | Local | Implemented |
| Safe Mode | Disables integration/HUD after crash or by setting | Local | Implemented |
| Startup/tray | Windows Run key + NotifyIcon | Local OS | Implemented |
| Updates | Manifest-based version checker | Network only when configured | Implemented |
| Installer | Self-contained app packaged into one Inno Setup EXE | Local OS | Implemented |

## Intentionally unavailable

AERL does not implement:

- Easy Anti-Cheat bypass, disabling, hiding or spoofing;
- prohibited DLL/code injection;
- protected game-memory patching/scraping;
- fake inventory ownership or paid-content server unlocks;
- rank, MMR, XP, credits or drop modification;
- server inventory modification;
- packet manipulation for gameplay advantage;
- effects on another player's client;
- competitive gameplay automation.

These are not unfinished UI placeholders. They are outside AERL's product boundary.

## Garage meaning

AERL Garage presets are local AERL configurations. A typed cosmetic name is stored and previewed by AERL; it does not claim ownership, equip an item on the Rocket League account or change what remote players see.

## Plugin security

The current plugin manager intentionally treats plugins as declarative manifests with explicit permissions. Arbitrary executable community modules are not auto-loaded because an unrestricted module loader would undermine the security model. A future executable SDK should be signed/sandboxed before being enabled.
