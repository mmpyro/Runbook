# Runbook
Is a composition root of whole module. 

### Section arguments
* RunbookName  _[mandatory]_ **string** name of runbook

Runbook creation
```ps
$runbook = New-Runbook -RunbookName 'runbook'
```

Adding sections to runbook
```ps
Add-Section -Runbook $runbook -Section $section
Add-Sections -Runbook $runbook -Sections @($section1, $section2)
```

Starting runbook
**Start-Runbook** starts runbook execution and waits until it not finish
**Write-Report** write overall report about runbook execution time, each section execution time and chapters execution time. It allows for performance improvements on specific chapter to reduce execution time

```ps
Start-Runbook -Runbook $runbook
Write-Report -Runbook $runbook

# returns runbook status code Success or Fail
$runbook.HasSuccessStatusCode
```