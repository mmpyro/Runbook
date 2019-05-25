# Example

### Runbook for copy set of databases on Azure Sql Server with elastic pool.
```ps
Import-Module RunBookModule -RequiredVersion 1.0.2

$resourceGroup = ''
$sqlServerName = ''
$datbaseName = ''
$elasticPoolName = ''
$clientId = ''
$password = ''
$tenantId = ''

$scope = {
    function Login-AzureRm() {
        param(
            [string] $ClientId,
            [string] $Password,
            [string] $TenantId
        )
        Import-Module AzureRM
        $secpasswd = ConvertTo-SecureString $Password -AsPlainText -Force
        $creds = New-Object System.Management.Automation.PSCredential ($ClientId, $secpasswd)
        Login-AzureRmAccount -ServicePrincipal -Tenant $TenantId -Credential $creds
    }
}

$copyChapters = @()
 @(1..5)|% { $chapter = New-Chapter -Name "CopyDb_$($_)" -Arguments @($_, $resourceGroup, $sqlServerName, $datbaseName, $elasticPoolName, $clientId, $password, $tenantId) -Action {
        param(
            [int] $Index,
            [string] $ResourceGroup,
            [string] $SqlServerName,
            [string] $DatabseName,
            [string] $ElasticPoolName,
            [string] $ClientId,
            [string] $Password,
            [string] $TenantId
        )

        Login-AzureRm -ClientId $ClientId -Password $Password -TenantId $TenantId
        Write-Host "Copy DB Copy_$($Index)"
        New-AzureRmSqlDatabaseCopy -ResourceGroupName $ResourceGroup `
          -ServerName $SqlServerName `
          -DatabaseName $DatabseName `
          -CopyResourceGroupName $ResourceGroup `
          -CopyServerName $SqlServerName `
          -CopyDatabaseName "Copy_$($Index)" `
          -ElasticPoolName $ElasticPoolName
    }
    Add-ToScope -Chaper $chapter -Scope $scope
    $copyChapters += $chapter
}

$deleteChapters = @()
 @(1..5)|% { $chapter = New-Chapter -Name "CopyDb_$($_)" -Arguments @($_, $resourceGroup, $sqlServerName, $datbaseName, $elasticPoolName, $clientId, $password, $tenantId) -Action {
        param(
            [int] $Index,
            [string] $ResourceGroup,
            [string] $SqlServerName,
            [string] $DatabseName,
            [string] $ClientId,
            [string] $Password,
            [string] $TenantId
        )

        Login-AzureRm -ClientId $ClientId -Password $Password -TenantId $TenantId
        Write-Host "Delete DB Copy_$($Index)"
        Remove-AzureRmSqlDatabase -ResourceGroupName $ResourceGroup `
          -ServerName $SqlServerName `
          -DatabaseName "Copy_$($Index)" `
    }
    Add-ToScope -Chaper $chapter -Scope $scope
    $deleteChapters += $chapter
}

$copyDbSection = New-ParallelSection -SectionName 'Copy databases'
$deleteDbSection = New-ParallelSection -SectionName 'Delete databases'
$runbook = New-Runbook -RunbookName 'Performance test'
Add-Chapters -Section $copyDbSection -Chapters $copyChapters
Add-Chapters -Section $deleteDbSection -Chapters $deleteChapters
Add-Section -Runbook $runbook -Section $copyDbSection
Add-Section -Runbook $runbook -Section $deleteDbSection

Start-Runbook -Runbook $runbook
Write-Report -Runbook $runbook
```