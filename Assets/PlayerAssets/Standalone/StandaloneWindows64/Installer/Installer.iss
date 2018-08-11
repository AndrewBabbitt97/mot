#define MyAppName "Mist of Time"
#define MyAppVersion "1.0.0.0"
#define MyAppPublisher "Ryuusoft"
#define MyAppExeName "motboot.exe"

[Setup]
ArchitecturesInstallIn64BitMode=x64
AppId={{2DAC08AF-A75D-4722-9DF0-18F0C369B737}
AppName={#MyAppName} (x64)
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={pf}\{#MyAppPublisher}\{#MyAppName}
DefaultGroupName={#MyAppPublisher}\{#MyAppName} (x64)
OutputBaseFilename=Installer
SetupIconFile=Icon.ico
Compression=lzma
SolidCompression=yes
UninstallDisplayName={#MyAppName} (x64)
UninstallDisplayIcon={app}\motboot.exe
OutputDir=..\..\..\..\..\obj\StandaloneWindows64\Installer

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Dirs]
Name: "{app}"; Permissions: everyone-full

[Files]
Source: "..\..\..\..\..\obj\StandaloneWindows64\Player\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\{#MyAppName} (x64)"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName} (x64)"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{app}"

