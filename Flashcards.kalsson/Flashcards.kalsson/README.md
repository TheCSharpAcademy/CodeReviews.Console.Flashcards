# FlashcardsApp

## Description

FlashcardsApp is a C# console application for managing stacks of flashcards and study sessions. The application uses SQL Server for data storage and Dapper for data access. Spectre.Console is used for the user interface, providing a rich console experience.

## Features

- Manage stacks of flashcards.
- Add, update, delete, and view flashcards within stacks.
- Conduct study sessions and record scores.
- Display all study sessions.

## Requirements

- .NET 8
- SQL Server LocalDB
- Visual Studio, JetBrains Rider, or any C# compatible IDE

## Install necessary NuGet packages:

* Dapper
* Spectre.Console
* Microsoft.Data.SqlClient

You can install these packages via the NuGet Package Manager.

## Ensure your appsettings.json is configured:

* Open appsettings.json and ensure it contains the following:

"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=FlashcardsDb;Trusted_Connection=True;"

## Run the application:
When you run the application for the first time, it will automatically create the necessary database and 
tables if they do not already exist.

## Usage
Run the application. You will be presented with a menu to choose from various options:

1. Show all stacks
2. Add a stack
3. Update a stack
4. Delete a stack
5. Show all flashcards
6. Add a flashcard
7. Update a flashcard
8. Delete a flashcard
9. Show all study sessions
10. Add a study session
11. Exit

## Managing Stacks
* Show all stacks: Displays all stacks with continuous IDs.
* Add a stack: Prompts for a stack name and adds it to the database.
* Update a stack: Prompts for a stack display ID and new name, then updates the stack.
* Delete a stack: Prompts for a stack display ID and deletes the stack.

## Managing Flashcards
* Show all flashcards: Prompts for a stack name and displays all flashcards in the stack with continuous IDs.
* Add a flashcard: Prompts for a stack name, question, and answer, then adds the flashcard.
* Update a flashcard: Prompts for a stack name, flashcard display ID, new question, and new answer, then updates the flashcard.
* Delete a flashcard: Prompts for a stack name and flashcard display ID, then deletes the flashcard.

## Study Sessions
* Show all study sessions: Displays all study sessions.
* Add a study session: Prompts for a stack name and score, then records the study session.