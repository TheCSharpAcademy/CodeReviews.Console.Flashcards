# CodingTracker
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
- If no database exists, or the correct table does not exist, they will be created when the program starts.

* A console based UI using Spectre Console where users can navigate with selecting with their keyboard


# DB functions

## Main Menu

- Users can add and delete a stack, add and delete a flashcard, do a study session, view study sessions by stack, and get an average score yearly report.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardsMainMenu.png)

### Add a stack

- Stack name is entered, with a way to exit to the main menu by entering 0.

	![image](https://github.com/Fennikko/Images/blob/main/StackName.png)

- If a stack name already exists, you receive an error message and can press any key to try again.

	![image](https://github.com/Fennikko/Images/blob/main/StackError.png)

- Informs you the stack has been added and prompts to press any key to continue
	![image](https://github.com/Fennikko/Images/blob/main/StackAddSuccess.png)

### Delete a stack

- Users select a stack to delete from the available stacks

	![image](https://github.com/Fennikko/Images/blob/main/StackDeletion.png)

- Users get a message on screen telling them that the stack has been deleted, and they can press any key to continue.
- When a stack is deleted, the associated flashcards and study sessions for that stack are also deleted from the database.

	![image](https://github.com/Fennikko/Images/blob/main/StackDeleted.png)

### Add a flashcard

- Users will be prompted if the flashcard will be for a new stack. If yes, the user will be prompted to create a new stack.
- If no, they will select the stack that the flashcard will be added to.

	![image](https://github.com/Fennikko/Images/blob/main/AddFlashcardStackSelect.png)

- Users will be prompted to enter the front and back of the flashcard, they can also press 0 to return to the main menu.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardFront.png)
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardBack.png)

- Empty values are not allowed, this is handled by prompting the user again, they can also press 0 to return to the main menu.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardEmpty.png)

- The front of flashcards **MUST** be unique, or users will get an error message and will need to try again.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardAddError.png)



### Delete a flashcard

- Users will be prompted to select a stack to delete the flashcard from.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardDeletion1.png)

- Users will then be prompted to select the flash card to delete, these are sorted by the front of the flashcards
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardDeletion2.png)

- Users will get a message that the flashcard has been deleted, and that the indexes have been updated.
- When a flash card is deleted, all flashcard Id's will adjust to get rid of any gaps.
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardDeletion3.png)


### Study Session

- Users will be prompted to select a stack to study from.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession.png)

- Users will shown the Id of the flashcard and the front of the flashcard, and will be prompted to provide an answer
- If a blank answer is provided, the user will be presented with an error message and be prompted to provide an answer

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession2.png)

- Once an answer is provided, the flashscard Id and back of the card will be shown to the user
- If the answer is correct, they will get a point and be prompted to press any key to continue
- If the answer is incorrect, they will not receive a point and will be informed that their answer was incorrect.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession3.png)
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession4.png)

- After all flashcards have been answered, the user will get their final score and to press any key to continue.
- User will be shown that the study session has been added, and to press any key to continue.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession5.png)
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession6.png)


### View Study Sessions by Stack

- Users will be prompted to select a stack to view all the sessions of.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardViewStudySessions.png)

- Users will then be presented with a table with the name of the stack, the session dates, and the scores.
- At the bottom will show the average study session score for all their sessions for that stack.
- Pressing any key will return user to the main menu.
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardViewStudySessions2.png)


### Average Score Yearly Report

- Users will be prompted to enter the year for the report.
- If the format is incorrect or blank, the user will receive an error message and be prompted to enter in the correct format.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardsYearlyReport.png)

- After the year is entered, users will be presented with a table with the average study sessions for the year they entered.
- If no sessions exist, they will be presented with an error message asking if they'd like to try agian.
- Selecting yes will prompt them for the year, selecting no will return them to the main menu.
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardsYearlyReport2.png)
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardsYearlyReport3.png)