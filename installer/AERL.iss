#define MyAppName "AERL"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Babouche (AMDG)"
#define MyAppExeName "AERL.exe"

[Setup]
AppId={{A7C4E2F7-2A2B-4E6A-A91E-8A25CF5D0E8D}
AppName={#MyAppName}
AppVersion=1.0.0
AppVerName=AERL 1.0.0 Public Beta
AppPublisher={#MyAppPublisher}
VersionInfoCompany=Babouche (AMDG)
VersionInfoDescription=AERL Rocket League Mod Setup
VersionInfoProductName=AERL
VersionInfoProductVersion={#MyAppVersion}
VersionInfoCopyright=Copyright © 2026 Babouche (AMDG)
DefaultDirName={localappdata}\Programs\AERL
DefaultGroupName=AERL
DisableProgramGroupPage=yes
AllowNoIcons=yes
PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
OutputDir=..\dist
OutputBaseFilename=AERL_Setup_1.0.0_Public_Beta
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
SetupIconFile=..\src\AERL.App\Assets\Branding\aerl.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
SetupLogging=yes
Uninstallable=yes
UninstallDisplayName=AERL
CloseApplications=yes
RestartApplications=no
UsePreviousAppDir=yes
UsePreviousTasks=yes
CreateAppDir=yes
DirExistsWarning=no
ShowLanguageDialog=auto

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "startup"; Description: "Launch AERL with Windows"; GroupDescription: "Startup"; Flags: unchecked

[Files]
Source: "..\artifacts\app\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Dirs]
Name: "{userappdata}\AERL\Config"
Name: "{userappdata}\AERL\Presets"
Name: "{userappdata}\AERL\Plugins"
Name: "{userappdata}\AERL\Cache"
Name: "{userappdata}\AERL\Sessions"
Name: "{userappdata}\AERL\Logs"
Name: "{userappdata}\AERL\Backups"

[Icons]
Name: "{autoprograms}\AERL"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"
Name: "{autodesktop}\AERL"; Filename: "{app}\{#MyAppExeName}"; WorkingDir: "{app}"; Tasks: desktopicon

[Registry]
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "AERL"; ValueData: """{app}\{#MyAppExeName}"" --startup"; Tasks: startup; Flags: uninsdeletevalue

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Launch AERL"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{app}"

[Code]
procedure CurStepChanged(CurStep: TSetupStep);
begin
  if (CurStep = ssPostInstall) and (not WizardIsTaskSelected('startup')) then
    RegDeleteValue(HKCU, 'Software\Microsoft\Windows\CurrentVersion\Run', 'AERL');
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then
  begin
    if MsgBox('Keep your AERL settings, presets, plugins, sessions and logs?', mbConfirmation, MB_YESNO) = IDNO then
      DelTree(ExpandConstant('{userappdata}\AERL'), True, True, True);
  end;
end;

function InitializeSetup(): Boolean;
begin
  Result := True;
end;
