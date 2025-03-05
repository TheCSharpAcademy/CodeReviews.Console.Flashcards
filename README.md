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

![image](https://github.com/user-attachments/assets/0823b2f7-9100-4dde-8ec2-b510548da131)

![image](https://github.com/user-attachments/assets/e0d3d2a5-4ba5-467b-be54-3bbc4ccaf8a9)

![image](https://github.com/user-attachments/assets/78ff8f79-c064-44d9-875d-681aae2880bc)

![image](https://github.com/user-attachments/assets/bb208871-f58c-4ed3-a232-88fb758e9364)

![image](https://github.com/user-attachments/assets/800c520f-4a64-47d2-8e76-89ce86771b49)

## Things I learned

- Setting up LocalDB with SQL Server
- How to use .config files with SQL Server
- Pivoting
- DTO

> Project duration: 14h
