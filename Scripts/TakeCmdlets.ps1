$configuration = 'Debug'
$src = 'C:\Users\PLMIMAR2\Documents\projects\Runbook\RunbookModule'
$dst = 'C:\Users\PLMIMAR2\Documents\projects\RunbookModule'
$uri = 'https://github.com/mmpyro/Runbook'
cd $dst
Remove-Item -Path "$dst\*" -Recurse -Force

$cmdlets = @()
Get-ChildItem -Path "$src\Cmdlets" -Filter '*Cmdlet.cs'| % {
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


Copy-Item -Path "$src\bin\$configuration\*" -Recurse

New-ModuleManifest -Path "RunbookModule.psd1" -Author 'mmpyro' -RootModule "RunbookModule.dll" -CmdletsToExport $cmdlets -ProjectUri $uri