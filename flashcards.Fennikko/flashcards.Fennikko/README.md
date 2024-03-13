# CodingTracker
Console based CRUD application to create and study flashcards
Developed using C# and MSSQL

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

- Users can Exit the application, create an Auto Session, Manual Session, Get all Session History, Get Session History with a filter.

	![image](https://github.com/Fennikko/Images/blob/main/Main%20Menu.png)

### Add a stack

- Stack name is entered, with a way to exit to the main menu by entering 0.

	![image](https://github.com/Fennikko/Images/blob/main/Auto%20Session.png)

- If a stack name already exists, you receive an error message and can press any key to try again.

	![image](https://github.com/Fennikko/Images/blob/main/Auto%20Session.png)

- Informs you the stack has been added and prompts to press any key to continue
	![image](https://github.com/Fennikko/Images/blob/main/Auto%20Session.png)

### Delete a stack

- Users enter a start time and end time, you can also type 0 to return to the Main Menu.

	![image](https://github.com/Fennikko/Images/blob/main/Manual%20Session.png)

- Date and time are checked to make sure they are in the correct format, you can also type 0 to return to the Main Menu.

	![image](https://github.com/Fennikko/Images/blob/main/Manual%20Session%20Format.png)

### Session History

- This populates a table with all of your logged coding sessions, It also calculates your avarage session time.

	![image](https://github.com/Fennikko/Images/blob/main/Session%20History%20No%20Filter.png)

### Session History By Filter

- This populates session history by days,weeks,months, and years. It will also calculate your average session time by this filter.

	![image](https://github.com/Fennikko/Images/blob/main/Session%20History%20Filter.png)

	![image](https://github.com/Fennikko/Images/blob/main/Session%20History%20Filter%202.png)

	![image](https://github.com/Fennikko/Images/blob/main/Session%20History%20Filter%203.png)