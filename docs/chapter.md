# Chapter
It's a basic task which needs to be executed. For chapter might be assigned retry strategy which define how chapter should be retried. 

### Chapter arguments
* Name _[mandatory]_ **string** name of chapter
* Action _[mandatory]_ **ScriptBlock**  task code to be executed
* Arguments **array** arguments used by script block. Default **empty array**.
* NumberOfRetries **int** number of retrying attempts. Default **0**.
* IgnoreErrorStream **boolean** if true error stream is ignored during checking chapter execution status. Default **false**.
* RetryStrategy **IRetryStrategy** retry strategy for chapter. Default **ImmediateRetryStrategy**.

### Basic chapter
```ps
$chapter = New-Chapter -Name 'chapter1' -Arguments @('Hello World') -Action {
    param(
        [string] $message
    )
    Write-Host $message
}
```

### Chapter with retries
```ps
$delayRetry = New-DelayRetryStrategy -Seconds 10
$chapter = New-Chapter -Name 'chapter1' -Arguments @('Hello World') -NumberOfRetries 2 -RetryStrategy $delayRetry -Action {
    param(
        [string] $message
    )
    Write-Host $message
}
```

### Scope
To chapter might be added additional **ScriptBlock** which allows for adding common powershell code accross chapters. For doing that use Add-ToScope command.

```ps
Add-ToScope -Chapter $chapter -Scope $scope
```

```ps
$scope = {
    function Add($a, $b) {
        return $a+$b
    }
    $sum = Add 4 5
}

$chapter = New-Chapter -Name 'chapter1' -Action {

    Write-Host $sum
}

Add-ToScope -Chapter $chapter -Scope $scope
$section = New-SequenceSection -SectionName 'section'
$runbook = New-Runbook -RunbookName 'runbook'

Add-Chapter -Section $section -Chapter $chapter
Add-Section -Runbook $runbook -Section $section
Start-Runbook -Runbook $runbook
Write-Report -Runbook $runbook
```