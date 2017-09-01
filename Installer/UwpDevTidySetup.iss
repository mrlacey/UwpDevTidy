; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{00FBB580-7152-4E36-AEE0-595716146CA6}
AppName=UWP Dev Tidy
AppVersion=1.0
;AppVerName=My Program 1.5
AppPublisher=Matt Lacey
AppPublisherURL=https://github.com/mrlacey/UwpDevTidy
AppSupportURL=https://github.com/mrlacey/UwpDevTidy
AppUpdatesURL=https://github.com/mrlacey/UwpDevTidy
DefaultDirName={pf}\UWP Dev Tidy
DisableProgramGroupPage=yes
OutputDir=C:\Users\matt\Documents\GitHub\UwpDevTidy\Installer
OutputBaseFilename=UwpDevTidySetup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\matt\Documents\GitHub\UwpDevTidy\UWPDevTidy\bin\Release\UWPDevTidy.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\matt\Documents\GitHub\UwpDevTidy\UWPDevTidy\bin\Release\CommandLine.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\matt\Documents\GitHub\UwpDevTidy\UWPDevTidy\bin\Release\System.Management.Automation.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\matt\Documents\GitHub\UwpDevTidy\UWPDevTidy\bin\Release\UWPDevTidy.exe.config"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{commonprograms}\My Program"; Filename: "{app}\UWPDevTidy.exe"
Name: "{commondesktop}\My Program"; Filename: "{app}\UWPDevTidy.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\UWPDevTidy.exe"; Description: "{cm:LaunchProgram,My Program}"; Flags: nowait postinstall skipifsilent
