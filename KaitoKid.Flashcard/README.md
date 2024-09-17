## Project Summary:
The Flashcard Project uses Entity Framework Core with SSMS to fulfill the given requirements

## Setup Instructions:

1.Set the following environment variables on your local machine:
   - `DB_SERVER`: Your SQL Server instance name
   - `DB_NAME`: Your database name

2. Modify the configuration file to use these environment variables:
   ```xml
   <configuration>
       <appSettings>
           <add key = "FlashcardsDBConnection" value = "Data Source=${DB_SERVER};Initial Catalog=${DB_NAME};Integrated Security=True;" />
       </appSettings >
   </configuration >
   ```

3. Create tables on your SQL Server in following manner:
   
   **Stack Table:**
   ```
   CREATE TABLE Stack (
    StackId INT IDENTITY(1,1) PRIMARY KEY,
    StackName NVARCHAR(50) NOT NULL UNIQUE
   );
   ```
   
   **Flashcard Table:**
   ```
   CREATE TABLE Flashcard (
    CardId INT IDENTITY(1,1) PRIMARY KEY,
    StackId INT NOT NULL,
    StackCardId INT,
    Question NVARCHAR(50) NOT NULL UNIQUE,
    Answer NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Stack_Cards FOREIGN KEY (StackId)
        REFERENCES Stack(StackId)
        ON UPDATE CASCADE
        ON DELETE CASCADE
   );
   ```

   **Study Session Table:**
   ```
   CREATE TABLE StudySession (
    SessionId INT IDENTITY(1,1) PRIMARY KEY,
    StackId INT NOT NULL,
    StackName NVARCHAR(50),
    Date DATETIME NOT NULL DEFAULT GETDATE(),
    Score INT NOT NULL DEFAULT 0,
    Duration VARCHAR(50) NOT NULL,
    CONSTRAINT FK_Stack_StudySession FOREIGN KEY (StackId)
        REFERENCES Stack(StackId)
        ON UPDATE CASCADE
        ON DELETE CASCADE
    );
   ```
   
## Project Workflow:

1. Views
The Views component of the Flashcard Application provides the user interface, allowing users to interact with the application. It includes:
-  Main Menu: The central hub where users can navigate to different parts of the application.
-  Stack Menu: Allows users to manage their flashcard stacks.
-  Flashcard Menu: Enables users to create, view, and study flashcards.
These menus capture user choices and send them to the Services layer for further processing.

2. Services
The Services layer acts as an intermediary between the Views and Repositories. It includes:
-  Stack Service: Handles operations related to flashcard stacks, such as creating, updating, and deleting stacks.
-  Flashcard Service: Manages flashcard-related operations, including adding, updating, and removing flashcards.
-  Study Session Service: Starts study sessions and also allow to view them
These services take user inputs from the Views and pass them to the Repositories for database operations.

3. Repositories
The Repositories layer is responsible for performing CRUD (Create, Read, Update, Delete) operations on the database. It includes:
-  Stack Repository: Manages database operations related to flashcard stacks.
-  Flashcard Repository: Handles CRUD operations for flashcards.
-  Study Session Repository: Insert Study session into the database
Repositories receive data from the Services layer and interact with the DatabaseContext to perform the necessary operations.

4. DatabaseContext
The DatabaseContext is designed to interact with SQL Server using Entity Framework Core. It includes:
-  Model Creation: Defines one-to-many relationships between Stack and Flashcard, and Stack and Study Sessions.
-  CRUD Operations: Facilitates CRUD operations in the Repositories by providing a context for database interactions.
The DatabaseContext ensures that the application can efficiently manage and persist data in the SQL Server database.

