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

<img src ="https://user-images.githubusercontent.com/64802476/226097108-883c1816-0f2c-4d07-9fdb-d64df13d3dbd.png" width=35%> <img src ="https://user-images.githubusercontent.com/64802476/226097120-2d20a3d6-74c8-410b-ac1b-f2a93283757c.png" width=35%>
