
## For help
#DesktopAppConverter.exe -?

## This should only need doing once
#DesktopAppConverter.exe -Setup -BaseImage "C:\DesktopBridge\Windows_InsiderPreview_DAC_15063.wim" -Verbose

## Update files, paths, and version number as necessary
DesktopAppConverter.exe -Installer "C:\Users\matt\Documents\GitHub\UwpDevTidy\Installer\UwpDevTidySetup.exe" -Destination "C:\Users\matt\Documents\GitHub\UwpDevTidy\Installer\appx" -PackageName "UWPDevTidy" -Publisher "CN=mrlacey.com" -Version "1.0.0.0" -MakeAppx -Verbose -Sign -InstallerArguments "/SILENT"
