# Flash Cards

## Usage


## To be done / Things to improve

1. Implement async programming model.
2. Implement keyboard press to exit the current event, so the user can exit and ongoing method without having to finish it.
3. Add timer restrictions and retries on flashcards and the ability to flip cards to generate another stack.
4. Migrate FK of cards table so it uses stackname instead of stackid. Since the stackname is UNIQUE is more convenient to relation them this way (this makes the 
stackid column useless, so it may be removed)
5. Create a input validation on the questions and answers of the cards. Right now it just checks for length constrains and then adds it as a SqlDBType.VarChar to que insert command.


## References
1. https://stackoverflow.com/questions/18435065/foreign-key-to-non-primary-key
2. https://stackoverflow.com/questions/5891538/listen-for-key-press-in-net-console-app
3. https://learn.microsoft.com/en-us/dotnet/api/system.string.equals?view=net-8.0
4. https://learn.microsoft.com/en-us/dotnet/api/system.stringcomparison?view=net-8.0
5. https://learn.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference?redirectedfrom=MSDN
6. https://learn.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql?view=sql-server-ver16
7. https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/
