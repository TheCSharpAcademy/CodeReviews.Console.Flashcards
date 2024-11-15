# ConsoleFlashcards


## Given Requirements:
- [x] This is an application where you will create Stacks of Flashcards.
- [x] There must be two different tables for stacks and flashcards. The tables should be linked by a foreign key.
- [x] Stacks should have an unique name.
- [x] Every flashcard needs to be part of a stack. If a stack is deleted, the same happens with the flashcard.
- [x] Usage of DTOs to show the flashcards to the user without the Id of the stack it belongs to.
- [x] When showing a stack to the user, the flashcard Ids always starts with 1 without gaps between them.
- [x] "Study Session" area, where you can study the stacks. All study sessions are stored, with date and score.
- [x] The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
- [x] To contain a call to the Study table so you can see all your study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.

## Features
* SQL Server database connection with Entity Framework.
> [!IMPORTANT]
> After downloading the project, you should check appsetting.json and write your own path to connect the db.
> 
> ![image](https://github.com/TwilightSaw/CodeReviews.Console.Flashcards/blob/main/images/appsettings.png)


 > [!IMPORTANT]
 > Also you should do starting migrations to create db with all necessary tables, simply write ```dotnet ef database update``` in CLI.
 > 
 > ![image](https://github.com/TwilightSaw/CodeReviews.Console.Flashcards/blob/main/images/migrations.png)

* A console based UI where you can navigate by user input.
  
   ![image](https://github.com/TwilightSaw/CodeReviews.Console.Flashcards/blob/main/images/UI.png)
  
* CRUD DB functions
  - From the menu you can Create, Read, Update or Delete entries to manipulate your Stacks and Flashcards.

* Study Area
  - After creating your Stack and Flashcards inside, you can start a Study Session, where you can earn points by answering your Flashcards.

* Table that contains all your previous Study Sessions.
  
   ![image](https://github.com/TwilightSaw/CodeReviews.Console.Flashcards/blob/main/images/table.png)
    
* Study Session report, where you can see your sessions amount per month and average amount of points you earned per month for every existing Stack for desired year.
  
   ![image](https://github.com/TwilightSaw/CodeReviews.Console.Flashcards/blob/main/images/report.png)

## Challenges and Learned Lessons
- Using Entity Framework for the first was a hard task, but it's a really convinient tool when you finally get used to it.
- I wasn't really sure how to create my first linked tables, but maybe due to EF it wasn't such a problem as I thought, but it is really amusing to watch how these all are connected.
- The same I can say about Host and Dependency Injection container, it may seem unnecessary to use this in a such small project, but I think it's better to get along with it as soon as possible.
- SQL Server was a little bit tricky, partially due to his syntaxis style.
- Doing Study Session report was an interesting task, final SQL query was really big but it's amazing that it can do such manipulations.
## Areas to Improve
- Do more maintainable and clear code.
- Deepen my SQL and EF knowledge.
## Resources Used
- C# Academy guidelines and roadmap.
- ChatGPT for new information as EF, Host, Dependency Injection, DTO etc..
- Spectre.Console documentation.
- SQL Server YouTube guides.
- Various StackOverflow articles.
