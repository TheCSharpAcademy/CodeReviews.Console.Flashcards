# Flashcard Application

-   Console-based CRUD application for creating and studying flashcards.
-   Developed using C# and SQL Azure with Dapper ORM.

## Given Requirements

-   [x] On application start, a SQL database is created in the server if it is not present.
-   [x] Database stores flashcard stacks for creation and study.
-   [x] Displays a menu of options.
-   [x] Allows users to:
    -   Add and delete stacks.
    -   Add and delete flashcards.
    -   Conduct study sessions.
    -   View all study sessions.
    -   Generate a yearly study session report.
-   [x] Gracefully handles errors to prevent crashes.
-   [x] Application exits only when the user selects "Exit".

## Features

-   **Azure SQL Database Connection**: The application connects to an Azure SQL Server database to store and retrieve data.
    -   If no database or correct table exists, they are created at the start of the application.
-   **Console UI**: The UI is implemented using Spectre.Console, enabling users to navigate via keyboard.
-   **Flashcard Stacks**: Create and manage stacks of flashcards.
-   **Study Sessions**: Track study session results, including dates and scores.
-   **Data Storage**: Stores flashcards, stacks, and study sessions in the Azure SQL database.
-   **Dependency Injection**: Implements Dependency Injection (DI) for improved separation of concerns.

## Requirements

-   .NET 8.0 or higher.
-   SQL Server (or Azure SQL).
-   Dapper ORM.

## Application Menu Options

### Add a Stack

-   Prompts the user for a stack name, with an option to return to the main menu by entering `0`.
-   If a stack name already exists, an error message is displayed, and the user is prompted to try again.
-   Confirms that the stack has been successfully added.

### Delete a Stack

-   Allows the user to select a stack to delete.
-   Upon deletion, the stack's associated flashcards and study sessions are also removed from the database.
-   Confirms the stack has been deleted.

### Add a Flashcard

-   Prompts whether the flashcard should belong to a new stack.
    -   If yes, the user is prompted to create a new stack.
    -   If no, the user selects an existing stack.
-   Prompts the user to enter the front and back of the flashcard.
    -   Empty values are not allowed; the user is asked to try again if necessary.
    -   The front of each flashcard must be unique; otherwise, an error message is displayed.

### Delete a Flashcard

-   Prompts the user to select a stack and then choose a flashcard to delete.
-   Confirms the flashcard has been deleted and updates indexes to remove any gaps in flashcard IDs.

### Study Session

-   Prompts the user to select a stack for the study session.
-   Flashcards are displayed with their ID and front side, and the user provides an answer.
-   If the answer is correct, the user earns a point.
-   At the end of the session, the user's score is displayed, and the session is logged.

### View Study Sessions by Stack

-   Displays all study sessions for the selected stack, including session dates and scores.
-   Shows the average score for the selected stack.

### Generate Yearly Report

-   Prompts the user to enter a year to generate a report of average study scores.
-   If no sessions exist for the selected year, the user is prompted to try again.

## Configuration

To set up the Azure SQL connection, add the following to `appsettings.json`:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=your-server-name;Database=your-db-name;User Id=your-username;Password=your-password;"
    }
}
```

## Running the Application

1. Navigate to the project directory:
    ```bash
        cd FlashcardApp
    ```
2. Restore the required packages:
     ```bash
        dotnet restore
    ```
3. Build the project:
     ```bash
        dotnet build
    ```
4. Navigate to the console app directory:
     ```bash
        cd FlashcardApp.Console
    ```
5. Run the application:
     ```bash
        dotnet run
    ```

## Challenges

- **Circular Dependencies**: I encountered circular dependencies when attempting to separate the menus and service logic. To resolve this, I simplified the design by consolidating the menus into one unified menu. This approach eliminated the dependency issues while maintaining a functional and streamlined architecture.
- **Azure SQL on M1 Mac**: Since I am using an M1 Mac, I opted to use Azure SQL instead of a local SQL Server, as the latter is not fully supported on M1 Macs. This was a new experience for me, so I had to learn how to set up and connect to an Azure SQL database. Since mac does not support SQL Server. I created a SQL Server on Docker container and connected to it through Azure Data Studio.

## Lessons Learned

- **Design Principles**: This experience taught me the importance of designing systems that avoid tightly coupling components, which can lead to complications like circular dependencies. By simplifying the design and consolidating the menus, I was able to resolve the issue and create a more maintainable and scalable application.
- **Cloud Services**: Using an M1 Mac presented a unique challenge, as it does not fully support SQL Server. To overcome this, I leveraged Azure SQL, which required learning how to set up and configure the service. Additionally, I explored Docker to run a SQL Server container and connected to it using Azure Data Studio. This experience deepened my understanding of cloud services and containerized databases, while also reinforcing the importance of being flexible and adaptable in choosing tools based on platform constraints. Using Azure SQL and Docker deepened my understanding of cloud services and containerized databases.

## Resources

-   [MS Docs](https://docs.microsoft.com/en-us/dotnet/)
-   [Dapper Documentation](https://www.learndapper.com/)
-   [Spectre.Console Library Documentation](https://spectreconsole.net/cli/exceptions)
