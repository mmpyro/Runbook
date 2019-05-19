param (
    [Parameter(Mandatory=$True)]
    [string] $ModulePath
)

Import-Module $ModulePath
Import-Module -Name Pester

.\sectionSpec.ps1
.\errorHandling.ps1