# FlashCards
- Console based CRUD application to create and study flashcards
- Developed using C# and MSSQL

# Given Requirements:
- [x] When application starts, it creates a MSSQL database if one is not present
- [x] Creates a database where flashcard stacks are made and studied
- [x] Shows a menu of options
- [x] Allows user to Add and delete a stack, add and delete a flashcard, do a study session, view all study sessions, and get a report on study sessions for the last year.
- [x] Handles all errors so application doesn't crash
- [x] Only exits when user selects Exit

# Features

* MSSQL database connection
		
- The program uses a MSSQL db conneciton to store and read information.
- The program utilizes Dapper Micro-ORM for handling queries and mapping into class objects
- If no database exists, or the correct table does not exist, they will be created when the program starts.

* A console based UI using Spectre Console where users can navigate with selecting with their keyboard


# DB functions

## Main Menu

- Users can add and delete a stack, add and delete a flashcard, do a study session, view study sessions by stack, and get an average score yearly report.

	![image](https://github.com/user-attachments/assets/3530de55-a131-4afe-b589-a62f080edafb)

![image](https://github.com/user-attachments/assets/311ec933-6fbd-423b-9829-106651cf3aef)

### Add a stack

- If a stack name already exists, you receive an error message and can press any key to try again.

	![image](https://github.com/user-attachments/assets/c8c67d8a-d6d8-4454-9726-fba798a605f7)


- Informs you the stack has been added and prompts to press any key to continue
	![image](https://github.com/user-attachments/assets/58123004-5369-4065-a5c4-0591d8e0fb1a)



### Add a flashcard
![image](https://github.com/user-attachments/assets/9d6255bf-0dea-4166-9bf3-79693181df07)

Show cards in a stack
![image](https://github.com/user-attachments/assets/dd558765-ac94-47cb-97a3-71a4f0bc456e)

- Users will be prompted if the flashcard will be for a new stack. If yes, the user will be prompted to create a new stack.
- If no, they will select the stack that the flashcard will be added to.

	![image](https://github.com/user-attachments/assets/435d1e93-08d6-45bb-b0f2-050595e58d40)


### Delete a flashcard

- Users will be prompted to select  a card to delete.

	![image](https://github.com/user-attachments/assets/cc0a4c29-f8f5-4ed4-8f94-efbc007f198a)


- Users will then be prompted to select the flash card to delete, these are sorted by the front of the flashcards
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardDeletion2.png)



### Study Session

- Users will be prompted to select a stack to study from.

![image](https://github.com/user-attachments/assets/61434302-b0ff-43fe-8ac0-de3fcf5ffbae)
 

- If the answer is correct, they will get 2 point and be prompted to press any key to continue
- If the answer is incorrect, 1 point will be deducted from their score and will be informed that their answer was incorrect.

	![image](https://github.com/user-attachments/assets/85ebc7fa-37e0-46ae-9668-f850d2e2160f)
![image](https://github.com/user-attachments/assets/8e40649b-1491-4272-8612-c71f3969c070)


- User will be shown that the study session has been added, and to press any key to continue.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession5.png)
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession6.png)


### Average Score Yearly Report

- Users will be prompted to enter the year for the report.
- If the format is incorrect or blank, the user will receive an error message and be prompted to enter in the correct format.
- After the year is entered, users will be presented with a table with the average study sessions for the year they entered.
	![image](https://github.com/user-attachments/assets/f0673045-b5b5-4c41-a633-94108daba5b7)
