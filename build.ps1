#requires -Version 7.0
[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $Configuration = "Debug"
)

dotnet publish $PSScriptRoot/src

$outputDir = "$PSScriptRoot/out/PSPiTop" 
Remove-Item $outputDir -Force -Recurse -ErrorAction SilentlyContinue
New-Item $outputDir -Force -ItemType Directory

# Copy bin
Get-ChildItem "$PSScriptRoot/src/bin/$Configuration/netcoreapp3.1/publish" -File | Copy-Item -Destination $outputDir

# Copy in psd1
Copy-Item -Path "$PSScriptRoot/src/PSPiTop.psd1" -Destination $outputDir
