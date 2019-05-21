Welcome to the Runbook wiki!
To install module from PowerShell Gallery use:
```ps
Install-Module -Name RunBookModule
```
RunBookModule was design for controlling how task should be running (sequence, parallel, using buffer, using window). The main purpose was allow users for easy setup whole workflow which is spitted by single task (chapter) and assign them to sections which controls that flow. Moreover each chapter adds error handling for itself which specify retry strategy and number of retries.

RunBookModule contains three main parts:
* [chapter](docs/chapter.md), it's a basic task which is responsible for single operation
* [section](docs/section.md), (sequence, parallel, buffer, window) which compose chapters and runs it in a specific way
* [runbook](docs/runbook.md), composition root of all chapters and sections

Retry strategies:
* ImmediateRetryStrategy, runs retry immediately after fails
* DelayRetryStrategy, runs retry after specified time parameters 
* IncrementalDelayRetryStrategy, increase delay time after each retry for the same chapter

### Quickstart
```ps
Import-Module RunBookModule


$chapter = New-Chapter -Name 'my first chapter' -Arguments @('Hello World') -Action {
    param(
        [string] $Message
    )
    Write-Host $Message
}

$section = New-SequenceSection -SectionName 'section'
$runbook = New-Runbook -RunbookName 'runbook'

Add-Chapter -Section $section -Chapter $chapter
Add-Section -Runbook $runbook -Section $section
Start-Runbook -Runbook $runbook
Write-Report -Runbook $runbook
```

* [Tips and tricks](docs/tips.md)
* [Example](docs/example.md)