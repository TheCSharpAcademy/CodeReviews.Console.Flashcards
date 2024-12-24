Flashcards

Console based CRUD application to create stacks of flashcards that can be studied.
Developed using C# , SQL Server and Spectre.Console.

- Database environment managed via Docker(SQL Server container).
- Utilised Azure Data Studio for query testing.

## How to run application

- Configure an app.config using app.config.template.

## Requirements

1. **Flashcards**:

   - Users will be able to create stacks of flashcards for study sessions.

2. **Database Design**:

   - Tables should be linked by foreign key.

3. **Stacks**:

   - Stack names should be unique.
   - On app start, SQL Server database should be created, if one isn’t present.

4. **DTOs**:

   - Uses Data Transfer Objects (DTOs) to display stacks and flashcards.

5. **Error Handling**:

   - Able to handle all possible errors so that the application never crashes.

6. **Database**:

   - Dapper ORM for data access.

7. **Follow DRY Principle**:

   - Avoid code repetition.

8. **Separation of Concerns**:

   - Object-Oriented Programming

9. **Configuration File**:

   - Configuration file will contain database path and connection strings.

## Features

- CRUD operations for stack and flashcards.
- Users can view study session data for a particular stack.
