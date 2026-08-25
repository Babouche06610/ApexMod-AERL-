# AERL single-EXE release

The public AERL release is designed to be distributed as exactly one file:

`AERL_Setup_1.2.2.exe`

The setup wizard:

- supports English and French;
- lets the user choose the installation directory;
- defaults to `%LOCALAPPDATA%\Programs\AERL` so administrator rights are normally unnecessary;
- extracts the complete published AERL application;
- prepares `%APPDATA%\AERL\Config`, `Presets`, `Plugins`, `Cache`, `Sessions`, `Logs`, and `Backups`;
- optionally creates a desktop shortcut;
- optionally configures launch with Windows;
- creates a Start Menu shortcut;
- registers a normal Windows uninstaller;
- can launch AERL at the end of setup.

## Build the public installer

Requirements on the release machine:

1. Windows 10/11 x64
2. .NET 8 SDK
3. Inno Setup 6

Then double-click `build-release.bat` or run:

```powershell
.\release.ps1
```

Output:

`dist\AERL_Setup_1.2.2.exe`

Only that EXE needs to be distributed to users.
