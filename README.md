# Flashcards

Console-based CRUD application that manages studying with flashcards. Developed using C#, SQL Server, Dapper and Spectre.Console.

## Features

- Create, edit and delete stacks of flashcards.
- Create study sessions to practice card front -> card back, or vice-versa.
- Visualize all of your data in neat tables.
- View reports that exhibit total study sessions and average score divided per month.
- Enter '.' to return from any input field.
- SQL Server database created on startup, according to .config file.
- SQL queries are protected with Dapper's parameterized queries.
- Application handles loss of database.
- Option to fill database with random data, to facilitate testing the application.
- Organized code that follows OOP, DRY and DTO principles.
- All application text is organized into a static `ApplicationTexts` class to facilitate text reusability and localization.

## Images

<!-- ![image](https://github.com/user-attachments/assets/43877317-3183-4872-8537-4758ca3d8c24)

![image](https://github.com/user-attachments/assets/0c80f961-d406-4c68-b408-e172b5433df7)

![image](https://github.com/user-attachments/assets/5b129d33-3571-408f-8493-739879652bff)

![image](https://github.com/user-attachments/assets/40488f8a-fcee-463a-84b4-b5f793633877)

![image](https://github.com/user-attachments/assets/3881e0f9-d05e-4dbb-aa49-6773de060905) -->

## Things I learned

- Setting up LocalDB with SQL Server
- How to use .config files with SQL Server
- Pivoting
- DTO

> Project duration: 14h