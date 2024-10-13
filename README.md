# Flashcard application
- Console based CRUD application to create and study flashcards
- Developed using C# and SQL Azure Server with Dapper

# Given Requirements:
- [x] When application starts, it creates a SQL database in the server if one is not present
- [x] Creates a database where flashcard stacks are made and studied
- [x] Shows a menu of options
- [x] Allows user to Add and delete a stack, add and delete a flashcard, do a study session, view all study sessions, and get a report on study sessions for the last year.
- [x] Handles all errors so application doesn't crash
- [x] Only exits when user selects Exit

# Features

- Azure SQL database connection
- The program uses a SQL server db conneciton to store and read information.
- If no database exists, or the correct table does not exist, they will be created when the program starts.
- A console based UI using Spectre Console where users can navigate with selecting an option with their keyboard
- Stacks of Flashcards: Create and manage stacks of flashcards.
- Study Sessions: Track study sessions, including dates and scores.
- Data Storage: Stores stacks, flashcards, and study sessions in a SQL Server database.
- Dependency Injection: Implements Dependency Injection (DI) to enhance separation of concerns.

## Requirements

- .NET 8.0 or higher
- SQL Server (or Azure SQL)
- Dapper ORM

# DB functions

## Main Menu

- Users can add and delete a stack, add and delete a flashcard, do a study session, view study sessions by stack, and get an average score yearly report.

### Add a stack

- Stack name is entered, with a way to exit to the main menu by entering 0.
- If a stack name already exists, you receive an error message and can press any key to try again.
- Informs you the stack has been added and prompts to press any key to continue

### Delete a stack

- Users select a stack to delete from the available stacks
- Users get a message on screen telling them that the stack has been deleted, and they can press any key to continue.
- When a stack is deleted, the associated flashcards and study sessions for that stack are also deleted from the database.

### Add a flashcard

- Users will be prompted if the flashcard will be for a new stack. If yes, the user will be prompted to create a new stack.
- If no, they will select the stack that the flashcard will be added to.
- Users will be prompted to enter the front and back of the flashcard, they can also press 0 to return to the main menu.
- Empty values are not allowed, this is handled by prompting the user again, they can also press 0 to return to the main menu.
- The front of flashcards **MUST** be unique, or users will get an error message and will need to try again.



### Delete a flashcard

- Users will be prompted to select a stack to delete the flashcard from.
- Users will then be prompted to select the flash card to delete, these are sorted by the front of the flashcards.
- Users will get a message that the flashcard has been deleted, and that the indexes have been updated.
- When a flash card is deleted, all flashcard Id's will adjust to get rid of any gaps.


### Study Session

- Users will be prompted to select a stack to study from.
- Users will shown the Id of the flashcard and the front of the flashcard, and will be prompted to provide an answer
- If a blank answer is provided, the user will be presented with an error message and be prompted to provide an answer
- Once an answer is provided, the flashscard Id and back of the card will be shown to the user
- If the answer is correct, they will get a point and be prompted to press any key to continue
- If the answer is incorrect, they will not receive a point and will be informed that their answer was incorrect.
- After all flashcards have been answered, the user will get their final score and to press any key to continue.
- User will be shown that the study session has been added, and to press any key to continue.

### View Study Sessions by Stack

- Users will be prompted to select a stack to view all the sessions of.
- Users will then be presented with a table with the name of the stack, the session dates, and the scores.
- At the bottom will show the average study session score for all their sessions for that stack.
- Pressing any key will return user to the main menu.


### Average Score Yearly Report

- Users will be prompted to enter the year for the report.
- If the format is incorrect or blank, the user will receive an error message and be prompted to enter in the correct format.
- After the year is entered, users will be presented with a table with the average study sessions for the year they entered.
- If no sessions exist. The user will be presented with an error message asking if they'd like to try again.
- Selecting yes will prompt them for the year, selecting no will return them to the main menu.

## Configuration

- **Azure SQL Support**: Connects to an Azure SQL database using `appsettings.json`.
- cd FlashcardApp
	1. cd FlashcardApp.Console
	2. touch appsettings.json
	3. add the below code to the appsettings.json file
	```json
	{
  		"ConnectionStrings": {
    		"DefaultConnection": "Server=your-server-name;Database=your-db-name;User Id=your-username;Password=your-password;"
  		}
	}
	```

## Run application

2. cd FlashcardApp
	1. dotnet restore
	2. dotnet build
	3. cd FlashcardApp.Console
	4. dotnet run


## Challenges
- **Circular Dependencies**: I encountered circular dependencies when attempting to separate the menus and service logic. To resolve this, I simplified the design by consolidating the menus into one unified menu. This approach eliminated the dependency issues while maintaining a functional and streamlined architecture.
- **Azure SQL**: Since I am using an M1 Mac, I opted to use Azure SQL instead of a local SQL Server, as the latter is not fully supported on M1 Macs. This was a new experience for me, so I had to learn how to set up and connect to an Azure SQL database. Since mac does not support SQL Server. I created a SQL Server on Docker container and connected to it through Azure Data Studio.

## Lessons Learned

- **Circular Dependencies**: This experience taught me the importance of designing systems that avoid tightly coupling components, which can lead to complications like circular dependencies. By simplifying the design and consolidating the menus, I was able to resolve the issue and create a more maintainable and scalable application.
- **Azure SQL**: Using an M1 Mac presented a unique challenge, as it does not fully support SQL Server. To overcome this, I leveraged Azure SQL, which required learning how to set up and configure the service. Additionally, I explored Docker to run a SQL Server container and connected to it using Azure Data Studio. This experience deepened my understanding of cloud services and containerized databases, while also reinforcing the importance of being flexible and adaptable in choosing tools based on platform constraints.

## Resources
- [MS Docs](https://docs.microsoft.com/en-us/dotnet/)
- [Dapper Documentation](https://www.learndapper.com/)
- [Spectre.Console Library Documentation](https://spectreconsole.net/cli/exceptions)
