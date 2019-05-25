param(
 [string] $Configuration = 'Debug',
 [string] $Src,
 [string] $Dst,
 [string] $ModuleVersion = '1.0.0'
)

$uri = 'https://github.com/mmpyro/Runbook'
$dotnetVersion = '4.6'
$psVersion = '5.0'
$description = @"
Runbook is module which allows for composing tasks (called chapters) into four type of sections: Sequence,Buffer,Window and Parallel. This allow for running set of tasks and controlling it's execution.
"@

if( -not (Test-Path -Path $Dst))
{
    New-Item -Path $Dst -ItemType Directory
}
cd $Dst
Remove-Item -Path "$Dst\*" -Recurse -Force

$cmdlets = @()
Get-ChildItem -Path "$Src\Cmdlets" -Filter '*Cmdlet.cs'| % {
    Get-Content -Path $_.FullName|Select-String -Pattern '.*\[Cmdlet\(VerbsCommon.*'|% {
       $trimed = $_.ToString().Trim().Substring(20)
       $splited = $trimed.Remove($trimed.Length-2, 2).Replace('"','').Split(',')
       $cmdlets += "$($splited[0].Trim())-$($splited[1].Trim())"
    }

    Get-Content -Path $_.FullName|Select-String -Pattern '.*\[Cmdlet\((").*'|% {
       $trimed = $_.ToString().Trim().Substring(9)
       $splited = $trimed.Remove($trimed.Length-3, 3).Replace('"','').Split(',')
       $cmdlets += "$($splited[0].Trim())-$($splited[1].Trim())"
    }
}


Copy-Item -Path "$Src\bin\$Configuration\*" -Recurse
$assemblies = Get-ChildItem -Path $Dst -Filter "*.dll"|Select -ExpandProperty Name

Write-Host "Cmdlets: " $cmdlets

New-ModuleManifest -Path "RunbookModule.psd1" -Author 'mmpyro' -RootModule "RunbookModule.dll" -Description $description -ModuleVersion $ModuleVersion `
 -CmdletsToExport $cmdlets -ProjectUri $uri -DotNetFrameworkVersion $dotnetVersion -PowerShellVersion $psVersion -RequiredAssemblies $assemblies