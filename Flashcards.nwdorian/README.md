# Flashcards
Console based application for flashcards studying system.

Created with C# / .NET 8 and SQL Server LocalDb
## Features
* **Console UI**
  * ![Menu navigation](https://github.com/nwdorian/Flashcards/assets/118033138/277ed88b-018c-495c-912f-daab0b9bebfc)
  * users can select menu options by navigating with arrow keys
  * menus and data previews are displayed by [Spectre.Console](https://github.com/spectreconsole/spectre.console)
* **Functionality**
  *  manage flashcard stacks or individual flashcards
  *  studying sessions area for learning
  *  table reports of completed studying sessions with date and score
* **Database**
  * the application uses LocalDb to store and retrieve data
  * data access is done trough [Dapper](https://github.com/DapperLib/Dapper) micro-ORM
  * database is created and seeded on startup if it doesn't exist
  * database diagram
  * ![diagram](https://github.com/nwdorian/Flashcards/assets/118033138/0be3e07e-5539-4013-b20a-dd2e66ebe4c0)
  * CRUD operations for stacks and flashcards with checks for existing records
  * when a stack is deleted, all linked flashcards and studying sessions cascade delete
## Lessons learned
* working with SQL Server LocalDb
* using Data Transfer Objects (DTOs)
* async / await
* repository pattern
