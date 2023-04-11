# Flashcards
CRUD based console application allowing the user to create and manage stacks of flashcards, and to use them during study sessions.
Developed using C#/.NET and SQL Server.

# Features
## SQL Server connection
- The program uses SQL Server to store and read information.
- Automaticaly creates a database and the proper tables at startup if they don't exist.

## Console Based UI
- Clean table representation made posible thanks to [ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt).

## Study Sessions
- Study sessions allow the user to study the stacks of flashcards he made.
- Automatically records the date and score of each study session.

# Challenges
- SQL Server is quite similar to SQLite, so the transition wasn't too bad, the setup was the hardest part.
- I learned a lot on the separation of concerns, and how poor design choices can be a headache the longer you work with them. But that also taught me how to deal with them, and how to make better design choices.

# Future
- I'd like to learn more about proper intelligent design in the future. To learn more basic principles that can facilitate building application in the beginning so that early choices don't end up slowing everything down.
- I also want to rebuild some general part of this application to be very solid, so that I can reuse them in other applications of mine.

# Resources
- Microsoft documentation
- [ConsoleTableExt](https://github.com/minhhungit/ConsoleTableExt)
- [The C# Academy](https://www.thecsharpacademy.com/) for general guidance
- StackOverflow
