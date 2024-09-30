# Flashcards

This Flashcards application is a C# project that introduces SQL Server for database management. The application allows users to create and manage stacks of flashcards. Users can study the stacks, track their progress, and manage their study sessions, which are stored in a database. The database consists of two linked tables (`Stacks` and `Flashcards`) with a foreign key relationship.

## Introduction

This project introduces Data Transfer Objects (DTOs) to simplify data manipulation between the database and the user interface. The focus of the project is to provide a smooth and user-friendly experience for managing flashcards and study sessions.

## Features

1. **Manage Stacks**: Users can create stacks of flashcards, update and delete them, with each stack having a unique name.

2. **Manage Flashcards**: Flashcards can be added, updated, and deleted. The flashcards within a stack are displayed with auto-numbered IDs starting from 1.

3. **Auto-numbered Flashcards without Gaps**: When displaying flashcards, IDs should be numbered sequentially starting from 1, with no gaps.

4. **Study Sessions**: Users can initiate study sessions, which are stored in the database. Study sessions track the date and score.

5. **Hard Deletes**: Deleting a stack should permanently remove it and allow the name to be reused without conflicts.

### Requirements

- **SQL Server**: Use SQL Server for managing data.

### How to Run the Project

- Configure the database connection string in the `appsettings.json` or use an alternative configuration method to connect to the SQL Server database.

### Database Design

- Create two tables: `Stacks` and `Flashcards`. The `Flashcards` table should have a foreign key reference to the `Stacks` table.

### Known Issues

1. **Unique Stack Names**: Each stack must have a unique name, and names should be reusable after deletion.

   - **Problem**: When a stack with a unique name was deleted, an error occurred while trying to reuse the name.
   - **Solution**: The issue arose from executing the `INSERT` command twice. Once corrected, the problem went away. Ensuring hard delete functionality (via `DELETE` SQL command) ensures stack names can be reused after deletion.

2. **SQL Syntax Issues**: 

   - **Problem**: Error "Incorrect syntax near the keyword 'IF'," is due to the use of the `IF NOT EXISTS` syntax, which is not supported in SQL Server when creating tables. This syntax is commonly found in other databases like SQLite, but SQL Server requires a different approach.

   - **Solution**: In SQL Server, you'll need to check if the table exists before trying to create it, so I used a conditional statement in my SQL command instead.

3. **Database Connection Issues**: 

   - **Problem**: Incorrect server connection led to failure in connecting to the database.
   - **Solution**: Ensured the correct connection string was used to connect to the `mssqllocaldb` instance.

### Lessons Learned

- In .NET Core and .NET 5/6/7/8, `app.config` is replaced by `appsettings.json`, and configuration is handled via the `IConfiguration` interface from the `Microsoft.Extensions.Configuration` namespace. `ConfigurationManager` is not typically used directly in modern .NET Core projects.
