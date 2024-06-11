# Flashcards
- A console based CRUD application to create flashcards for study.
- Developed using C# and SQL Service with running in Docker container.

# Features

- SQL Server database connection to store, read, update, and delete information.
- Spectre user interface for easier navigation for user.
- User friendly prompt for all functions and actions taken by user.

# How to run the application

- Fork or clone a copy of this repository.
- Change the connection string in "app.config" to your connection string.
- Use of SQLite database connection string is easier and better.
- Open terminal and navigate to directory of the cloned project.
- Type "dotnet run".


# DB functions

## Main Menu

- Users can add, update and delete a stack, add and delete a flashcard, do a study session for a stack
, view study sessions by stack, and get an average score yearly report.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardsMainMenu.png)

### Add a stack

- User enter stack name or press '0' to exit.

	![image](Flashcards.UndercoverDev/Flashcards.UndercoverDev/img/stack%20exist.png)

- If a stack name already exists, user receives an error message and can press any key to try again.

	![image](https://github.com/Fennikko/Images/blob/main/StackError.png)

- App informs user the stack has been added and prompts to press any key to continue
	![image](https://github.com/Fennikko/Images/blob/main/StackAddSuccess.png)

### Delete a stack

- User select a stack to delete from the available stacks, or select back for Main Menu.

	![image](Flashcards.UndercoverDev/Flashcards.UndercoverDev/img/stack%20delete.png)

- User gets a message on screen saying that the stack has been deleted, and they can press any key to continue.
- When a stack is deleted, all flashcards and study sessions for that stack are also deleted from the database.

	![image](https://github.com/Fennikko/Images/blob/main/StackDeleted.png)

### Add a flashcard

- User will be prompted if the flashcard will be for a new stack. If yes, user will be prompted to create a new stack.

	![image](Flashcards.UndercoverDev/Flashcards.UndercoverDev/img/new%20flashcard.png)

- If no, user will select the stack that the flashcard will be added to.

	![image](https://github.com/Fennikko/Images/blob/main/AddFlashcardStackSelect.png)

- Users will be prompted to enter the question and answer for the flashcard, they can also press 0 to return to the main menu.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardFront.png)
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardBack.png)

- Empty values are not allowed, and user can press 0 to return to the main menu.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardEmpty.png)

- The front of flashcards **MUST** be unique, or users gets an error message.

	![image](Flashcards.UndercoverDev/Flashcards.UndercoverDev/img/question%20exist.png)



### Delete a flashcard

- User is prompted to select a stack where flashcard is stored.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardDeletion1.png)

- User is prompted to select the flash card to delete, shown as questions for the flashcards.
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardDeletion2.png)

- User gets message that the flashcard has been deleted.
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardDeletion3.png)


### Study Session

- User is prompted to select a stack to study from.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession.png)

- Users is shown the Id of the flashcard and the front of the flashcard, and  prompted for an answer
- Empty fields are not allowed.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession2.png)

- If user is correct, the flashcard Id and answer of the card is shown to the user.
- User gets a point for correct answer and is prompted to press any key to continue.
- If user is wrong, display message is shown with no point awarded.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession3.png)
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession4.png)

- After all flashcards are answered, user gets the final score and to press any key to continue.
- User is shown that study session has been added, and to press any key to continue.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession5.png)
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardStudySession6.png)


### View Study Sessions by Stack

- User is prompted with list of Stack to select from.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardViewStudySessions.png)

- User is presented with a table with the name of the stack, the session dates, and the scores.
- At the bottom, the average study session score for all sessions for that stack is shown.
- Pressing any key returns user to the main menu.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardViewStudySessions2.png)


### Average Score Yearly Report

- Users is prompted to enter the year for the report.
- If year format is incorrect, user is prompted with error message.

	![image](https://github.com/Fennikko/Images/blob/main/FlashcardsYearlyReport.png)

- If year is correctly entered, user is shown a table with the average study sessions for the year entered.
- If no sessions exist, user is shown an error message with prompt to try again.
- 'Yes' prompts again for year input, 'No' returns user to the main menu.
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardsYearlyReport2.png)
	![image](https://github.com/Fennikko/Images/blob/main/FlashcardsYearlyReport3.png)

	
# Lessons & Skills Learnt

- SQL Server.
- SQL Server installation and commands.
- How to run SQL Server in a Docker container.
- Docker Installations and commands.
- How to make database persist if SQL Server image is deleted.


# Resources
- SQL Server Linux container image with Docker.
- [Microsoft Doc](https://learn.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&tabs=cli&pivots=cs1-bash)
- [YouTube](https://www.youtube.com/watch?v=Z4I35x0fnG8)
- [Spectre Doc](https://spectreconsole.net/)
