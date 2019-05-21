# Section
Section compose chapters and execute it:
* one by one **Sequence section**
* parallel **Parallel section**
* concurentlly, but not more than buffer size. When one task has finisehd another one started **Buffer section**
* concurentlly, but not more than buffer size. When all task have finished a new chunk is started **Window section**

### Section arguments
* SectionName _[mandatory]_ **string** name of section, must be unique accross all sections in runbook
* BufferSize _[mandatory]_ for (Buffer and Window sections) **int** number of chapters which might be executed at the same time

### Section execution flow
**Sequence-Section** registered chapters: [chapter1, chapter2, chapter3, chapter4]

_result (order as registered)_
* chapter1
* -- wait --
* chapter2
* -- wait --
* chapter3
* -- wait --
* chapter4
* -- wait --
  
**Parallel-Section** registered chapters: [chapter1, chapter2, chapter3, chapter4]

_result (race condition, might be any order)_ 
* chapter4
* chapter2
* chapter1
* chapter3
* -- wait --

**Buffer-Section** registered chapters: [chapter1, chapter2, chapter3, chapter4], buffer size 2 

_result (race condition, )_
* chapter4
* chapter1
* -- wait for any --
* chapter3
* chapter2

**Window-Section** registered chapters: [chapter1, chapter2, chapter3, chapter4], buffer size 2 

_result (race condition, )_
* chapter1
* chapter2
* -- wait for all --
* chapter3
* chapter4

Difference between buffer and window section is window waits for all chapters in window. If buffer size is equal 2. Window section wait until both started have finish, but buffer starts new chapter when any chapter finished it's job. Moreover window section take chapters as them were registered. So always buffer equals 2 take firstly chapter1 and chapter2 and secondly chapter3 and chapter4.

### Create new section
```ps
$section = New-SequenceSection -SectionName 'my first section'
$section = New-ParallelSection -SectionName 'my second section'
$section = New-BufferSection -SectionName 'my third section' -BufferSize 2
$section = New-WindowSection -SectionName 'my fourth section' -BufferSize 2
```

Section without at lest one chapter is worthless. At least one chapter has to be added into it. Runbook throws ArgumentException when empty section is added.


```ps
Add-Chapter -Section $section -Chapter $chapter
Add-Chapters -Section $section -Chapters @($chapter1, $chapter2)
```