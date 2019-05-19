param (
    [Parameter(Mandatory=$True)]
    [string] $modulePath = 'C:\Users\PLMIMAR2\Documents\projects\RunbookModule\RunBookModule.psd1'
)

Import-Module $modulePath
Import-Module -Name Pester

.\sectionSpec.ps1
.\errorHandling.ps1