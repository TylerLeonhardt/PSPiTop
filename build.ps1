#requires -Version 7.0

dotnet publish $PSScriptRoot/src

$outputDir = "$PSScriptRoot/out/PSPiTop" 
Remove-Item $outputDir -Force -Recurse -ErrorAction SilentlyContinue
New-Item $outputDir

Get-ChildItem "$PSScriptRoot/src/bin/$Configuration/netcoreapp3.1/publish" -File
| Copy-Item $outputDir

Get-ChildItem "$PSScriptRoot/src/PSPiTop.psd1"
| Copy-Item $outputDir
