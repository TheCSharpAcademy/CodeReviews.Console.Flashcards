A C# Console Application to mimic real life Flashcards for studying.

The app creates SQL databases (locally) which contain tables for Flashcard stacks ("dbo.Stacks"), Flashcards ("dbo.Flashcards") and Study Sessions ("dbo.StudySessions").
Each of these names are configurable via the App.config file, and the program will automatically reference these names throughout.

Users can create Flashcard Stacks, add, update or delete Flashcards in that stack, and then begin studying said Stack. Flashcards can be deleted manually, but both Study Sessions and Flashcards are linked
to their parent Flashcard Stack. Once a Flashcard Stack is deleted, so too are the flashcards and any Study Session records attached to them, but this is not bidirectional.

Once the user has completed a Study Session, they can view a leaderboard to see their scores reflected over time.

Main Menu:

![alt text](https://github.com/geicoandsonic/CodeReviews.Console.Flashcards/blob/main/FlashcardsMainMenu.PNG)

Managing Flashcard Stacks Menu:

![alt text](https://github.com/geicoandsonic/CodeReviews.Console.Flashcards/blob/main/ManageFlashcardStacks.PNG)

Study Session Menu: 

![alt text](https://github.com/geicoandsonic/CodeReviews.Console.Flashcards/blob/main/StudySession.PNG)

Example of a Study Session Score: 

![alt text](https://github.com/geicoandsonic/CodeReviews.Console.Flashcards/blob/main/StudySessionScore.PNG)

Study Session Scoreboard:

![alt text](https://github.com/geicoandsonic/CodeReviews.Console.Flashcards/blob/main/StudySessionScoreboard.PNG)

Specific Study Sessions (and invalid ID example):

![alt text](https://github.com/geicoandsonic/CodeReviews.Console.Flashcards/blob/main/ValidVsInvalidID.PNG)

Thank you for taking a look at my project. Please feel free to take a look at other projects of mine.
