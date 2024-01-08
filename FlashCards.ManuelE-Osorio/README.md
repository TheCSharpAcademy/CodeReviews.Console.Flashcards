# Flash Cards

## Usage

### First Time Running

The user has to configure the app.config file with the appropiate connection string for SQL Sever. There are three .csv files included for testing purposes, they can be imported if the tables are empty and the user has selected the PopulateDB as true in the app.config file.

### Stacks Menu

The user is able to create, edit and delete stacks in the database. Before using the Cards Menu and running a Study Session the user has to selected the working stack they want to use.

### Cards Menu

The user is able to view, create, edit and delete the cards in the database for the selected stack. Currently there are no major restrictions on the answer and questions of the cards, just a 300 character length limitation.

### Study Session

The user is able to start a study session for the selected stack. Currently the only parameter the user is able to modify is the quantity of cards that will be displayed on the study session.

### Study Session Data

The user is able to access all the records of the previous study sessions and check monthy average scores by deck.


## To be done / Things to improve

1. Implement async programming model.
2. Implement keyboard press to exit the current event, so the user can exit and ongoing method without having to finish it.
3. Add timer restrictions and retries on flashcards and the ability to flip cards to generate another stack.
4. Create a input validation on the questions and answers of the cards. Right now it just checks for length constrains and then adds it as a SqlDBType.NVarChar to que insert command.
5. Add an option to import cards from a .csv based on the Helpers.PopulateDB() method.

## References
1. https://stackoverflow.com/questions/18435065/foreign-key-to-non-primary-key
2. https://stackoverflow.com/questions/5891538/listen-for-key-press-in-net-console-app
3. https://learn.microsoft.com/en-us/dotnet/api/system.string.equals?view=net-8.0
4. https://learn.microsoft.com/en-us/dotnet/api/system.stringcomparison?view=net-8.0
5. https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference?redirectedfrom=MSDN
6. https://learn.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql?view=sql-server-ver16
7. https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/
8. https://stackoverflow.com/questions/26790477/read-csv-to-list-of-objects
9. https://learn.microsoft.com/en-us/sql/t-sql/data-types/nchar-and-nvarchar-transact-sql?view=sql-server-ver16


