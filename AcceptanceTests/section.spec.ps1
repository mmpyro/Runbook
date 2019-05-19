param (
    [Parameter(Mandatory=$True)]
    [string] $ModulePath
)

Import-Module $ModulePath
Import-Module -Name Pester

Describe 'Runbook with all sections types' {
    It 'Sequence section should pass without errors' {
        #Given
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

        #When
        Start-Runbook -Runbook $runbook

        #Then
        $runbook.HasSuccessStatusCode| Should -Be $true
        $result| Should -Be 2
    }

    It 'Sequence section should run tasks one by one' {
        #Given
        $runbook = New-Runbook -RunbookName 'test'
        $section = New-SequenceSection -SectionName 'sequence-section'
        $chapter1 = New-Chapter -Name 'task1' -Action {
             Start-Sleep -Seconds 1
        }
        $chapter2 = New-Chapter -Name 'task2' -Action {
             Start-Sleep -Seconds 1
        }
        Add-Chapters -Section $section -Chapters @($chapter1, $chapter2)
        Add-Section -Section $section -Runbook $runbook

        #When
        Start-Runbook -Runbook $runbook

        #Then
        $runbook.HasSuccessStatusCode| Should -Be $true
        $executionTime = Get-ExecutionTime -Runbook $runbook 
        $executionTime| Should -Not -BeNullOrEmpty
        $executionTime.TotalSeconds|Should -BeGreaterOrEqual 2
        $executionTime.TotalSeconds|Should -BeLessThan 3
    }

    It 'Parallel section should run tasks parallel' {
        #Given
        $runbook = New-Runbook -RunbookName 'test'
        $section = New-ParallelSection -SectionName 'parallel-section'
        $chapter1 = New-Chapter -Name 'task1' -Action {
             Start-Sleep -Seconds 1
        }
        $chapter2 = New-Chapter -Name 'task2' -Action {
             Start-Sleep -Seconds 1
        }
        Add-Chapters -Section $section -Chapters @($chapter1, $chapter2)
        Add-Section -Runbook $runbook -Section $section

        #When
        Start-Runbook -Runbook $runbook

        #Then
        $runbook.HasSuccessStatusCode| Should -Be $true
        $executionTime = Get-ExecutionTime -Runbook $runbook
        $executionTime| Should -Not -BeNullOrEmpty
        $executionTime.TotalSeconds|Should -BeGreaterOrEqual 1
        $executionTime.TotalSeconds|Should -BeLessThan 2
    }

    It 'Windows section should run task in window size' {
        #Given
        $runbook = New-Runbook -RunbookName 'test'
        $section = New-WindowSection -SectionName 'window-section' -BufferSize 2
        $chapter1 = New-Chapter -Name 'setup1' -Action {
            Start-Sleep -Seconds 1
        }
        $chapter2 = New-Chapter -Name 'setup2' -Action {
            Start-Sleep -Seconds 1
        }
        $chapter3 = New-Chapter -Name 'setup3' -Action {
            Start-Sleep -Seconds 1
        }
        $chapter4 = New-Chapter -Name 'setup4' -Action {
            Start-Sleep -Seconds 1
        }
        Add-Chapters -Section $section -Chapters @($chapter1, $chapter2, $chapter3, $chapter4)
        Add-Section -Section $section -Runbook $runbook

        #When
        Start-Runbook -Runbook $runbook


        #Then
        $runbook.HasSuccessStatusCode| Should -Be $true
        $executionTime = Get-ExecutionTime -Runbook $runbook
        $executionTime| Should -Not -BeNullOrEmpty
        $executionTime.TotalSeconds|Should -BeGreaterOrEqual 2
        $executionTime.TotalSeconds|Should -BeLessThan 3
    }

    It 'Buffer section should run task in chunks when any finish next should be started' {
        #Given
        $runbook = New-Runbook -RunbookName 'test'
        $section = New-BufferSection -SectionName 'buffer-section' -BufferSize 2
        $chapter1 = New-Chapter -Name 'setup1' -Action {
            Start-Sleep -Seconds 1
        }
        $chapter2 = New-Chapter -Name 'setup2' -Action {
            Start-Sleep -Seconds 1
        }
        $chapter3 = New-Chapter -Name 'setup3' -Action {
            Start-Sleep -Seconds 1
        }
        $chapter4 = New-Chapter -Name 'setup4' -Action {
            Start-Sleep -Seconds 1
        }
        Add-Chapters -Section $section -Chapters @($chapter1, $chapter2, $chapter3, $chapter4)
        Add-Section -Section $section -Runbook $runbook

        #When
        Start-Runbook -Runbook $runbook


        #Then
        $runbook.HasSuccessStatusCode| Should -Be $true
        $executionTime = Get-ExecutionTime -Runbook $runbook
        $executionTime| Should -Not -BeNullOrEmpty
        $executionTime.TotalSeconds|Should -BeGreaterOrEqual 2
        $executionTime.TotalSeconds|Should -BeLessThan 3
    }
}