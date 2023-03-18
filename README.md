# Console FlashCards

A CRUD based console application that allows the user to study with flashcards. Developed using C#/.NET and SQL Server.

## Features

### SQL Server connection
- The program uses SQL Server to store and read information
- If no database exists a new one is initialized

### Console Based UI
- Most UI elements make use of the [ConsoleTableExt Library](https://github.com/minhhungit/ConsoleTableExt) to write the content in neat tables

### View
- All Ids (flashcard's and stack's) are presented to the user sequentially starting from 1.
  - If one Id's entry is deleted, the rest are updated to fill the gap.
  
### Study Sessions
- Study sessions allow the user to study a stack and all it's cards.

<img src ="https://user-images.githubusercontent.com/64802476/226097108-883c1816-0f2c-4d07-9fdb-d64df13d3dbd.png" width=45%> <img src ="https://user-images.githubusercontent.com/64802476/226097120-2d20a3d6-74c8-410b-ac1b-f2a93283757c.png" width=45%>

## Challenges
- SQL Server 
  - Setting up was tricky, for whatever reason my installation didn't install with the default parametres, so I wasted a lot of time troubleshooting.
  - Syntax - I had already worked with SQLite so the switch wasn't hard, just had to get used to it.
  - Learning to use SQL Server Management Studio - Testing queries was extremely useful and time-saving.
  
- Organization
  - I think I did a good job separating concerns from the very start.
    - I could still go further, but in projects of this scale (and that I know I don't want to expand) seems like wasted effort.

## Lessons learned
- Became familiar with SQL Server while continuining to learn SQL
- Cemented previous knowledge in C#/.NET

## Areas to improve
- Keep learning SQL
- Make better use of KanBan boards. They're a powerfull tool for organization, but I end up forgetting I have one set up, so I don't use as much as I would like.

## Resources
- The [C#Academy Flashcard's project](https://www.thecsharpacademy.com/project/14) was the project guide.
- I reused a lot of code from my [previous project](https://github.com/ThePortugueseMan/ThePortugueseMan.CodingTracker).
- The [C#Academy discord community](https://discord.com/invite/JVnwYdM79C) that are always ready to help, and came in clutch with my SQL Server troubles (shout out to ghoulam!)
- The [C#Academy coding coventions](https://thecsharpacademy.com/article/58) to help clean up the code and stick to the coding conventions.
- Various resources from all over the web.
