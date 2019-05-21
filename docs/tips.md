# Tips and tricks

Passing argument by reference
```ps
$result = $null
$runbook = New-Runbook -RunbookName 'test'
$section = New-SequenceSection -SectionName 'sequence-section'
$chapter = New-Chapter -Name 'setup' -Arguments @([ref]$result) -Action {
     param(
        [ref] $result
     )
     $result.Value = 2
}
Add-Chapter -Section $section -Chapter $chapter
Add-Section -Section $section -Runbook $runbook
Start-Runbook -Runbook $runbook
```