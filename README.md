# Console FlashCards
Test
A console application that allows the user to study flashcards. Developed using C#/.NET and MSSQL.

## Features

### SQL Server connection
- The program uses MSSQL Server to store and read information
- If no database exists, a new one is initialized

### Console Based UI
- UI uses the [Spectre.Console](https://github.com/spectreconsole/spectre.console) library for menus, input, and data presentation.
  
### Study Sessions
- Study sessions allow the user to study a stack and all its cards. Sessions are scored and the score, stack studied, and date/time of session are stored for later review.


## Resources/Credits
- The [C#Academy Flashcard's project](https://www.thecsharpacademy.com/project/14) was the project guide.
- Seed data was taken from [forser's implementation of this project](https://github.com/TheCSharpAcademy/CodeReviews.Console.Flashcards/tree/main/FlashCards.Forser).
