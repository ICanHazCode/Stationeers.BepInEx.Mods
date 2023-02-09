# Self-elevate the script if required
if (-Not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] 'Administrator')) {
 if ([int](Get-CimInstance -Class Win32_OperatingSystem | Select-Object -ExpandProperty BuildNumber) -ge 6000) {
  $CommandLine = "-File `"" + $MyInvocation.MyCommand.Path + "`" " + $MyInvocation.UnboundArguments
  Start-Process -FilePath PowerShell.exe -Verb Runas -WorkingDirectory $PSScriptRoot -ArgumentList $CommandLine 
  Exit
 }
}

"This script sets directory links in BepInEx to the steam workshop for Stationeers."
"This will allow BepInEx-enabled workshop items to be run by just Subscribing from the Steam workshop."
Start-Sleep -Seconds 3

try{
cd $PSScriptRoot
$CurrentDir=$PSScriptRoot
$patchers=Join-Path -Path $CurrentDir -ChildPath "patchers\workshop"
$plugins=Join-Path -Path $CurrentDir -ChildPath "plugins\workshop"

cd ..\..\..\workshop\content\544550
$Workshop=pwd
New-Item -ItemType SymbolicLink -Force -Path $patchers -Target $Workshop
New-Item -ItemType SymbolicLink -Force -Path $plugins -Target $Workshop
}
catch{
"There were errors in the script!"
Write($Error)
Pause
Exit
}
"Completed link setup."
Start-Sleep -Seconds 5
Remove-Item $script:MyInvocation.MyCommand.Path -Force
