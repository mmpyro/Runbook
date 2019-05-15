Import-Module 'C:\Users\PLMIMAR2\Documents\projects\RunbookModule\RunBookModule.psd1'
Import-Module -Name Pester

Describe 'Runbook' {
    It 'Sequence section should pass without errors' {
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
        $runbook.HasSuccessStatusCode()| Should -Be $true
        $result| Should -Be 2
    }

    It 'Sequence section should run tasks one by one' {
        $runbook = New-Runbook -RunbookName 'test'
        $section = New-SequenceSection -SectionName 'sequence-section'
        $chapter1 = New-Chapter -Name 'task1' -Action {
             Start-Sleep -Seconds 1
        }
        $chapter2 = New-Chapter -Name 'task2' -Action {
             Start-Sleep -Seconds 1
        }
        $section.Add($chapter1)
        $section.Add($chapter2)
        $runbook.Add($section)
        $runbook.Invoke()
        $report = $runbook.OverallReport()
        $runbook.HasSuccessStatusCode()| Should -Be $true
        $report.ExecutionTime.TotalSeconds|Should -BeGreaterOrEqual 2
        $report.ExecutionTime.TotalSeconds|Should -BeLessThan 3
    }

    It 'Parallel section should run tasks parallel' {
        $runbook = New-Runbook -RunbookName 'test'
        $section = New-ParallelSection -SectionName 'parallel-section'
        $chapter1 = New-Chapter -Name 'task1' -Action {
             Start-Sleep -Seconds 1
        }
        $chapter2 = New-Chapter -Name 'task2' -Action {
             Start-Sleep -Seconds 1
        }
        $section.Add($chapter1)
        $section.Add($chapter2)
        $runbook.Add($section)
        $runbook.Invoke()
        $report = $runbook.OverallReport()
        $runbook.HasSuccessStatusCode()| Should -Be $true
        $report.ExecutionTime.TotalSeconds|Should -BeGreaterOrEqual 1
        $report.ExecutionTime.TotalSeconds|Should -BeLessThan 2
    }
}