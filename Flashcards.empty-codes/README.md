## Flashcards

### Introduction
This Flashcards application is a C# project that introduces SQL Server for database management. The application allows users to create and manage stacks of flashcards. Users can study the stacks, track their progress, and manage their study sessions, which are stored in a database. The database consists of two linked tables (`Stacks` and `Flashcards`) with a foreign key relationship.

This project introduces Data Transfer Objects (DTOs) to simplify data manipulation between the database and the user interface. The focus of the project is to provide a smooth and user-friendly experience for managing flashcards and study sessions.

![MainMenu](Screenshots/main.PNG)
![StackMenu](Screenshots/stacks.PNG)
![StudySessionMenu](Screenshots/session.PNG)

### Requirements
- **SQL Server**: Use SQL Server for managing data.
- **Tables**:
  - **Stacks Table**: Stores different stacks, each with a unique name.
  - **Flashcards Table**: Each flashcard belongs to a stack and contains a question and answer.
- **Unique Stack Names**: Each stack must have a unique name, and names should be reusable after deletion.
- **DTOs**: Use DTOs to display flashcards without exposing their database IDs.
- **Auto-numbered Flashcards**: When displaying flashcards, IDs should be numbered sequentially starting from 1, with no gaps.
- **Study Sessions**: Store the date and score of each study session in the database. The study session should be linked to a specific stack, and if a stack is deleted, its related study sessions should be deleted as well.
- **Hard Deletes**: Deleting a stack should permanently remove it and allow the name to be reused without conflicts.

### Features
- **Manage Stacks**: Users can create stacks of flashcards, update and delete them with each stack having a unique name.
- **Manage Flashcards**: Flashcards can be added, updated, and deleted. The flashcards within a stack are displayed with auto-numbered IDs starting from 1.
- **Study Sessions**: Users can initiate study sessions, which are stored in the database. Study sessions track the date and score.
- **View Study Sessions**: Users can view all study sessions linked to a stack.

![StudySession](Screenshots/study.PNG)
![StudySessionReports](Screenshots/data.PNG)

### How to Run the Project
1. **Prerequisites**:
   - Install SQL Server.
   - Install Visual Studio with .NET and C# support.
   - Configure the database connection string in the `appsettings.json` or use an alternative configuration method to connect to the SQL Server database.
   
2. **Database Setup**:
   - Create two tables: `Stacks` and `Flashcards`. The `Flashcards` table should have a foreign key reference to the `Stacks` table.
   - Use the provided SQL scripts or manually create the tables in SQL Server.

3. **Running the Application**:
   - Compile and run the application in Visual Studio.
   - Follow the menu options to create, manage, and study stacks of flashcards.
   
### Challenges Faced & Lessons Learned

#### 1. **Unique Stack Names**
   - **Problem**: When a stack with a unique name was deleted, an error occurred while trying to reuse the name.
     - **Details**: Violation of UNIQUE KEY constraint 'UQ_Stacks_F73C0D30A3FA20E9'. Cannot insert duplicate key in object 'dbo.Stacks'.
   - **Solution**: The issue arose from executing the `INSERT` command twice. Once corrected, the problem went away. Ensuring hard delete functionality (via `DELETE` SQL command) ensures stack names can be reused after deletion.

#### 2. **Auto-numbered Flashcards without Gaps**
   - **Problem**: The `FlashcardId` column used an `IDENTITY` constraint, which does not support renumbering after a flashcard is deleted.
   - **Solution**: Implemented a "fake ID" system that assigns sequential numbers to flashcards when displayed. The `FlashcardId` is still used for updates and deletions by retrieving the flashcard via its question text.

#### 3. **Database Connection Issues**
   - **Problem**: Incorrect server connection led to failure in connecting to the database.
   - **Solution**: Ensured the correct connection string was used to connect to the `mssqllocaldb` instance.

#### 4. **SQL Syntax Issues**
   - **Problem**: Error "Incorrect syntax near the keyword 'IF'," is due to the use of the `IF NOT EXISTS` syntax, which is not supported in SQL Server when creating tables. This syntax is commonly found in other databases like SQLite, but SQL Server requires a different approach.
   - **Solution**: In SQL Server, you'll need to check if the table exists before trying to create it, so I used a conditional statement in my SQL command instead.

#### 5. **Config Issues**
   - **Lesson**: In .NET Core and .NET 5/6/7/8, `app.config` is replaced by `appsettings.json`, and configuration is handled via the `IConfiguration` interface from the `Microsoft.Extensions.Configuration` namespace. `ConfigurationManager` is not typically used directly in modern .NET Core projects.

### Areas for Improvement
- **Error Handling**: Add more detailed error handling and logging for database operations.
- **Pivot Reports**: Although the initial plan included pivot reports for viewing study sessions per stack, this feature was removed for simplicity. It could be reintroduced in the future.
- **Advanced Study Features**: Consider adding features like timed study sessions, flashcard categories, or user-specific settings.

### Conclusion
I was introduced to DTOs and the `IDENTITY` constraints for the first time. I also learned about pivoting tables, although I did not implement the reports feature.