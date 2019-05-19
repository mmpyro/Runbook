function TryCatch()
{
    param(
        [ScriptBlock] $Action
    )

    try
    {
        & $Action
    }
    catch
    {
        return $_.Exception
    }
    return $null
}

Describe 'Error handling' {

    It 'Section should not has two chapters with the same name [Range]' {
        #Given
        $section = New-SequenceSection -SectionName 'sequence-section'
        $chapter1 = New-Chapter -Name 'task1' -Action {
             Start-Sleep -Seconds 1
        }
        $chapter2 = New-Chapter -Name 'task1' -Action {
             Start-Sleep -Seconds 1
        }

        #When
        $exception = TryCatch -Action {
             Add-Chapters -Section $section -Chapters @($chapter1, $chapter2)
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Section: $($section.SectionName), Chapters task1 are not unique inside section."
    }

    It 'Section should not has two chapter with the same name' {
        #Given
        $section = New-SequenceSection -SectionName 'sequence-section'
        $chapter1 = New-Chapter -Name 'task1' -Action {
             Start-Sleep -Seconds 1
        }
        $chapter2 = New-Chapter -Name 'task1' -Action {
             Start-Sleep -Seconds 1
        }
        Add-Chapter -Section $section -Chapter $chapter1

        #When
        $exception = TryCatch -Action {
             Add-Chapter -Section $section -Chapter $chapter2
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Section: $($section.SectionName), Chapter task1 already exists in that section."
    }

    It "Runbook doesn't allow for adding empty section" {
        #Given
        $section1 = New-SequenceSection -SectionName 'sequence-section'
        $section2 = New-SequenceSection -SectionName 'sequence-section'
        $runbook = New-Runbook -RunbookName 'runbook'

        #When
        $exception = TryCatch -Action {
            Add-Sections -Runbook $runbook -Sections @($section1, $section2)
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Section cannot be null"
    }

    It "Runbook doesn't allow for section name duplication" {
        #Given
        $chapter1 = New-Chapter -Name 'task1' -Action {}
        $chapter2 = New-Chapter -Name 'task1' -Action {}
        $section1 = New-SequenceSection -SectionName 'sequence-section'
        $section2 = New-SequenceSection -SectionName 'sequence-section'
        Add-Chapter -Section $section1 -Chapter $chapter1
        Add-Chapter -Section $section2 -Chapter $chapter2
        $runbook = New-Runbook -RunbookName 'runbook'
        Add-Section -Runbook $runbook -Section $section1

        #When
        $exception = TryCatch -Action {
            Add-Section -Runbook $runbook -Section $section2
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Section: sequence-section already exists in that runbook."
    }

    It "Chapter shouldn't be created with empty Name" {
        #Given

        #When
        $exception = TryCatch -Action {
            $chapter1 = New-Chapter -Name ' ' -Action {} -NumberOfRetries 1
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "ChapterName cannot be null or empty"
    }

     It "Chapter shouldn't be created with NumberOfRetries lower than one" {
        #Given

        #When
        $exception = TryCatch -Action {
            $chapter1 = New-Chapter -Name 'task1' -Action {} -NumberOfRetries 0
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "NumberOfRetires cannot be lower than 1"
    }

     It "Sequence-Section shouldn't be created with empty Name" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-SequenceSection -SectionName '  '
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "SectionName cannot be null or empty"
    }

      It "Parallel-Section shouldn't be created with empty Name" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-ParallelSection -SectionName '  '
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "SectionName cannot be null or empty"
    }

      It "Buffer-Section shouldn't be created with empty Name" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-BufferSection -SectionName '  ' -BufferSize 2
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "SectionName cannot be null or empty"
    }


      It "Buffer-Section shouldn't be created with BufferSize lower than 1" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-BufferSection -SectionName 'section' -BufferSize 0
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "BufferSize cannot be lower than 1"
    }

      It "Window-Section shouldn't be created with empty Name" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-WindowSection -SectionName '  ' -BufferSize 2
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "SectionName cannot be null or empty"
    }

    It "Window-Section shouldn't be created with BufferSize lower than 1" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-WindowSection -SectionName 'section' -BufferSize 0
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "BufferSize cannot be lower than 1"
    }

    It "IncrementalDelayStrategy shouldn't be created with milliseconds parameter lower than 0" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-IncrementalDelayRetryStrategy -MilliSeconds -1
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Milliseconds has to be greater or equal zero"
    }

    It "IncrementalDelayStrategy shouldn't be created with seconds parameter lower than 0" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-IncrementalDelayRetryStrategy -Seconds -1
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Seconds has to be greater or equal zero"
    }

    It "IncrementalDelayStrategy shouldn't be created with minutes parameter lower than 0" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-IncrementalDelayRetryStrategy -Minutes -1
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Minutes has to be greater or equal zero"
    }

     It "IncrementalDelayStrategy shouldn't be created with hours parameter lower than 0" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-IncrementalDelayRetryStrategy -Hours -1
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Hours has to be greater or equal zero"
    }

    It "DelayStrategy shouldn't be created with milliseconds parameter lower than 0" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-DelayRetryStrategy -MilliSeconds -1
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Milliseconds has to be greater or equal zero"
    }

    It "DelayStrategy shouldn't be created with seconds parameter lower than 0" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-DelayRetryStrategy -Seconds -1
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Seconds has to be greater or equal zero"
    }

    It "DelayStrategy shouldn't be created with minutes parameter lower than 0" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-DelayRetryStrategy -Minutes -1
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Minutes has to be greater or equal zero"
    }

     It "DelayStrategy shouldn't be created with hours parameter lower than 0" {
        #Given

        #When
        $exception = TryCatch -Action {
            $section = New-DelayRetryStrategy -Hours -1
        }

        #Then
        $exception| Should -Not -BeNullOrEmpty
        $exception.GetType()| Should -BeLike '*ArgumentException'
        $exception.Message|Should -Be "Hours has to be greater or equal zero"
    }
}