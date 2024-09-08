
# Flash-Cards.Lawang

This is the Console Application that replicates the real Flash-Cards.
To make this application .net Console app, Dapper as ORM and SQL-SERVER as database is utilised.

## Requirements

- When the application starts, it should creates a sqlserver database, if one isn’t present.
 
- It should also create a table in the database, where the data is seeded custom data is sedded if the table is empty.(Normally custom data is seeded for testing)
- You need to be able to insert, delete, update and view your flash-cards and stack table.
- You should handle all possible errors so that the application never crashes
- You can only interact with the database using Dapper.

## Features
* Can simply be navigated by pressing the "UP" and "DOWN" keys, moreover the interface is self explainatory.
* SQL SERVER database is used to store and read data from.


![Screenshot from 2024-09-08 13-44-02](https://github.com/user-attachments/assets/00ad2983-f088-4ced-b3ad-11711fe65f0e)



* #### Screen shots:



* Users can Perform CRUD function on the database.
* Go to "Study" to create new Study session

  ![Screenshot from 2024-09-08 13-44-57](https://github.com/user-attachments/assets/e21a8866-48d2-48a9-b3db-019c2908c002)


- Data is presented to user in Table format, using the external library Spectre.Console.
- This app is beautified using Spectre.Console.

![Screenshot from 2024-09-08 13-50-11](https://github.com/user-attachments/assets/d4728aff-be7c-4600-ab2a-8ff5cef9b847)


## Project Summary
#### What challenges did you face and how did you overcome them?

* This project required me to use the Sql-Server database and syntax of this database was some what different from the sqlite database that i previously used.

  * To interact with the database we needed something in .net to interact with database. so, as suggested in Project I started learning about Dapper.
  .
* The requirement for this project was a lot more than the project that i did previously for example, we needed to reference stack table in both flash-card table and Study_session table, moreover we had to make this table interact in such a way than affecting the parent table will cause cascading effect on child tables.

* To follow the Separtion Of Concern principle, I made 3 classes for handling database operation, for each class it handled one table, like wise Visual class to handle the gui of the application, Validation class for checking the validation of the users input and lot more.


* Doing operation on SqlServer database was a hassle so I had to learn a lot of new sql syntax to achieve what was needed

* Pivot was much more tricky than expected but with lot of trial and error I was able to do it.




## 🛠 Skills Learned
#### SQL-SERVER
* Unlike sqlite, sqlserver wasn't a built in database and needed differet server to host this project database, so I learned how to host the database in different server for Sql-server database. 

#### Spectre.Console
* I honed my Spectre.Console skill in this project which i previously learned.

#### SQL
* To interact with database in this project using Dapper I needed to use raw SQL statement, and had to learn and implement it for basic operation. 

* I learned how to add reference to the parent table and pivot the row into the column which was the most tricky part of this project.

* I have to dig deep into the whole world of possiblity in sql language and how the data can be manipulated.


## FAQ

#### How to beautify the table in the project?

Answer I used the Microsoft.Spectre.Console package, which you can get for Nuget package manager. Install it and add Reference to your project. 

For more information u can visit the docs https://spectreconsole.net




## Feedback

If you have any feedback, please reach out to us at depeshgurung44@gmail.com



