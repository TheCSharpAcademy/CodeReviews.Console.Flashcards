## FlashCards
This is a simple C# console application designed to help users study and memorize information using flashcards. Users can create custom flashcard Stacks, view questions, and test their knowledge with multiple-choice style questions. 

## Key Features:

- Create custom flashcard Stacks.
  - Users are able to Create, Read, Update and Delete Stacks and Cards.
- Test knowledge with multiple-choice questions.
  - Provides randomised Questions to the user Based on Stack selection.
- Track study Score (Reporting)
  - Instance per month and avergae score per month presented using Pivot table
- Simple and user-friendly console interface.
  - Spectre.Console Library used to create User Interface.
- Performs operations against SQL Server Database.
  - Perform CRUD operations on stacks, Cards and Study Session Tables.
  - Automatically Create and Populate Database if not present.
  - Using Linked tables to manage data (if stack deleted, cards and history removed).
  - Using Pivot tables to seperate report data into Month Columns per stack.

  ## Application Setup

  ### Pre-requisites
  - A local SQL Server Database must be present and running.
  - The Local SQL Server name must be copied and Inserted into the App.config
  - 

'''
        <configuration>
        
        		<appSettings>
        			<add key ="ConnectionString" value="Server=(LocalDB)\[Your Local DB Name];Database=FlashCardsDB;Trusted_Connection=True;" />
        			<add key ="DBCreationString" value="Server=(LocalDB)\[Your Local DB Name];Database=;Trusted_Connection=True;" />
        		</appSettings>
        		
        	</configuration>
''''


