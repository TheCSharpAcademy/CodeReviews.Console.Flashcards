# CodingTracker
- Console based CRUD application to create and study flashcards
- Developed using C# and MSSQL

## Running the Application

Ensure you have .NET 8 installed on your machine.

Ensure you have a SQL Server database running on your machine somehow and
can connect to the server instance via a connection string similar to the
one found in `Flashcards.Bkohler93/Flashcards.Console/example.App.Config`.

Copy `Flashcards.Bkohler93/Flashcards.Console/example.App.Config` and
paste the file as `App.config` with your SQL Server connection string in
the `sqlConnectionString` appSetting.

Run the application within `Flashcards.Bkholer93/Flashcards.Console` in
Visual Studio or by using the CLI command `dotnet run`.

## Given Requirements
- [x] When application starts, it creates a MSSQL database if one is not present
- [x] Creates a database where flashcard stacks are made and studied
- [x] Shows a menu of options
- [x] Allows user to Add and delete a stack, add and delete a flashcard, do a study session, view all study sessions, and get a report
on study sessions for the last year.
- [x] Handles all errors so application doesn't crash
- [x] Only exits when user selects Exit

## Features

* MSSQL database connection
		
- The program uses a MSSQL db conneciton to store and read information.
- If no database exists, or the correct table does not exist, they will
be created when the program starts.

* A console based UI using Spectre Console where users can navigate with selecting with their keyboard

