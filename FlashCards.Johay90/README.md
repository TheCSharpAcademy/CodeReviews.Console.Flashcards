# Flashcard App

The Flashcard App is a console application that allows users to create and manage flashcard stacks, study those stacks, and view reports on their study sessions. The application uses SQL Server as the database management system.

## Features

- Create, delete, and update flashcard stacks
- Create, delete, and update flashcards within a stack
- Study a selected stack and track study sessions
- View previous study sessions
- Generate monthly reports on study sessions

## Running the Application

1. Build the solution.
2. Run the `App` class.
3. Follow the on-screen instructions to interact with the application.

## Lessons Learned

- Pivoting: The application demonstrates how to pivot data from a database to generate monthly reports.
- Value of DTOs: The use of Data Transfer Objects (DTOs) simplifies the transfer of data between different layers of the application.

## Areas to Improve

- User Input: The `UserInput` class feels messy and could benefit from refactoring, such as moving more code into models and methods, using generic types, and leveraging enums for menu options.
- Console Interface: The console interface could be improved by using a library like Spectre.Console.
- Database Structure: The database structure could be optimized to make pivoting easier and to better handle potential future requirements.
- Database Seeding: Manual database seeding might be preferable for this type of application instead of inserting random data.
- Helper Functionality: Additional helper functionality, such as extension methods, could be added to improve code organization and reusability.
