# Flashcards Project by C# Academy

## Project Overview

This project allows users to create and manage stacks of flashcards for studying. It includes features for creating, organizing, and reviewing flashcards, as well as tracking study sessions.

Project Link: [Flashcards Project](https://www.thecsharpacademy.com/project/14/flashcards)

## Project Requirements

- Users can create stacks of flashcards.

- Two database tables are required: one for stacks and another for flashcards, linked via a foreign key.

- Stacks must have unique names.

- Every flashcard must belong to a stack. Deleting a stack should also delete its associated flashcards.

- Use DTOs to display flashcards without exposing the stack ID.

- Flashcard IDs within a stack should always be sequential, starting from 1. If a flashcard is deleted, IDs should be restructured accordingly.

- Implement a "Study Session" feature where users can review flashcards. Study sessions should be recorded with a timestamp and score.

- Study sessions should be linked to stacks. If a stack is deleted, its study sessions should also be removed.

- Users should be able to view their study history. Study session records should only allow insert operations—no updates or deletions.

## Additional Challenges

- Implement a reporting system to display:

  - The number of study sessions per month, per stack.

  - The average score per month, per stack.

- Create an autofill function that loads default data from a JSON file into the database if required tables do not exist.

## Lessons Learned

**1. Database Setup & SQL Exploration:**

  - Installed SQL Server and SSMS, explored SQL operations.

  - Initialized LocalDB for development and tested CRUD operations.

  - Used SSMS throughout the project to validate SQL queries before implementation.

**2. Project Initialization & Design:**

  - Started with a blank project and connected it to SQL Server.

  - Found minimal differences between SQL Server and SQLite in basic operations.

  - Sketched a rough design of classes and models before coding, which evolved over time but reinforced the importance of planning before implementation.

**3. Feature Implementation:**

  - Developed repositories and services for stacks, flashcards, and study sessions.

  - This served as a refresher on database handling after working on a non-SQL-based project.

**4. AutoFill Functionality:**

  - Implemented an autofill feature using a JSON file to store default data.

  - While not the most efficient method, it provided a valuable learning experience in JSON serialization and deserialization.

**5. SQL Query Optimization & Pivoting:**

  - Faced challenges when implementing the reporting system, particularly with SQL table pivoting.

  - Learned to nest SQL queries, an essential skill for advanced data manipulation.

## Areas for Improvement

The project structure feels well-organized, but further refinements may be needed as I gain more experience.

## Main Resources Used

Various SQL Server and SSMS tutorials

[W3Schools SQL Reference](https://www.w3schools.com/SQL)

[JSON Format Checker](https://jsonchecker.com/)

[Spectre.Console](https://spectreconsole.net/)

[Dapper ORM Guide](https://www.learndapper.com/)

## Packages Used
| Package | Version |
|---------|---------|
| Dapper | 2.1.66 |
| Microsoft.Data.SqlClient | 6.0.1 |
| Microsoft.Extensions.DependencyInjection | 9.0.3 |
| Spectre.Console | 0.49.1 |







