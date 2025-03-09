# Flashcards

**Flashcards** is a console application written in C# that I made in order to grasp the basics of working with SQL Server. The user can create thematic decks, add flashcards to them, and practice added vocabulary using the *Study* functionality.

# Tech stack
- C#
- SQL Server 2022
- Dapper
- [Spectre.Console](https://github.com/spectreconsole/spectre.console)

# Features
- Adding, deleting and editing flashcard stacks (to group flashcards thematically; every flashcard has to be assigned to a stack)
- Adding, deleting and editing flashcards; every flashcard needs to have a term and a definition
- Study session functionality - the user is asked to provide definition of every term from selected flashcard stack; the application stores results of study sessions
- Data is stored in SQL Server LocalDB. The database and tables are created automatically.

# Running the app

To run this app make sure you have installed:
- .NET 8.0
- SQL Server 2022 (with LocalDB)

1. Clone the repository:
    ```
    git clone https://github.com/afilipkowski/Flashcards
    cd Flashcards/Flashcards
    ```
2. Build the project:
    ```
    dotnet build
    ```
3. Run the application:
    ```
    dotnet run
    ```

