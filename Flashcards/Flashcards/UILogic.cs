using ConsoleTableExt;
using Flashcards.Data;

namespace Flashcards;

public class UILogic
{
    DatabaseLogic databaseLogic = new DatabaseLogic();
    Validation validation = new Validation();
    bool validString;
    bool stackExists;

    public void StartStudy(string selectedStack)
    {
        if (selectedStack != "none")
        {
            DateTime dateOfSession = DateTime.Now;
            string formattedDateOfSession = dateOfSession.ToString("yyyy-MM-dd HH:mm");
            int score = 0;

            List<FlashcardsModel> allFlashcards = databaseLogic.CreateFlashCardsList();
            List<FlashcardsModel> filteredFlashcards = allFlashcards.Where(obj => obj.StackName == selectedStack).ToList();

            foreach (FlashcardsModel flashcard in filteredFlashcards)
            {
                string userAnswer = "";

                do
                {
                    Console.WriteLine($"What is the answer to {flashcard.Front}");
                    userAnswer = Console.ReadLine().ToLower().Trim();
                    validation.ValidString(userAnswer, out validString);
                } while (validString == false);

                    string flashcardAnswer = flashcard.Back.ToLower().Trim();

                if (userAnswer == flashcardAnswer)
                {
                    score++;
                    Console.WriteLine("Correct!");
                }
                else
                {
                    Console.WriteLine($"Wrong! The correct answer is {flashcardAnswer}");
                }
                Console.ReadLine();
            }
            databaseLogic.InsertStudySession(selectedStack, formattedDateOfSession, score);
        }
        else
            Console.WriteLine("You have to select a stackName before starting the session!");
        
        Console.ReadLine();
    }

    public void ShowStudySession()
    {
        Console.Clear();
        List<StudySession> studySessions = databaseLogic.CreateStudySessionsList();
        Console.WriteLine("Study Sessions:");

        ConsoleTableBuilder
          .From(studySessions)
          .ExportAndWriteLine();

        Console.ReadLine();
    }

    public void CreateNewStack()
    {
        List<string> stacksList = databaseLogic.CreateStacksList();
        string newStackName;

        do
        {
            Console.WriteLine("Write a name for the new stack");
            newStackName = Console.ReadLine();
            validation.ValidString(newStackName, out validString);
            validation.StackExistsCheck(newStackName, stacksList, out stackExists);
        } while (validString == false || stackExists == true);

        databaseLogic.InsertStack(newStackName);
    }

    public void DeleteStack(ref string selectedStack)
    {
        List<string> stacksList = databaseLogic.CreateStacksList();
        string newStackName;

        do
        {
            Console.WriteLine("Write the name of the stack you want to delete");
            newStackName = Console.ReadLine();
            validation.ValidString(newStackName, out validString);
            validation.StackExistsCheck(newStackName, stacksList, out stackExists);
        } while (validString == false || stackExists == false);

        databaseLogic.DeleteStack(newStackName);
        Console.WriteLine("Stack Deleted!");
        Console.ReadLine();
        selectedStack = "none";
    }

    public void SelectStack(ref string selectedStack)
    {
        List<string> stacksList = databaseLogic.CreateStacksList();
        string stackName;

        do
        {
            Console.WriteLine("Write the name of the stack you want to select!");
            stackName = Console.ReadLine();
            validation.ValidString(stackName, out validString);
            validation.StackExistsCheck(stackName, stacksList, out stackExists);

            if (stackExists == false)
                Console.WriteLine("Stack does not exist!");

        } while (validString == false || stackExists == false);

        selectedStack = stackName;
        int stackID = databaseLogic.GetStackID(stackName);
        Console.WriteLine($"Stack Changed to {stackName}! The stack ID is {stackID}!");
        Console.ReadLine();
    }

    public void ShowStacksList()
    {
        List<string> stacksList = databaseLogic.CreateStacksList();
        Console.WriteLine("StacksName");

        ConsoleTableBuilder
          .From(stacksList)
          .ExportAndWriteLine();

        Console.ReadLine();
    }

    public void CreateFlashcard(string selectedStack)
    {
        List<FlashcardsModel> allFlashcards = databaseLogic.CreateFlashCardsList();
        FlashcardsModel lastFlashcard = allFlashcards[allFlashcards.Count - 1];
        int newFcID = lastFlashcard.FcID + 1;

        string front = "", back = "";

        if(selectedStack == "none")
        {
            Console.WriteLine($"Current selected stack is {selectedStack}.Select a valid stack!");
            Console.ReadLine();
        }
        else if (selectedStack != "none")
        {
            do
            {
                Console.WriteLine("Provide text for the question (front of card):");
                front = Console.ReadLine();
                validation.ValidString(front, out validString);
            } while (validString == false);

            validString = true;

            do
            {
                Console.WriteLine("Provide text for the answer (back of card):");
                back = Console.ReadLine();
                validation.ValidString(back, out validString);
            } while (validString == false);

            int stackID = databaseLogic.GetStackID(selectedStack);
            databaseLogic.InsertFlashcard(newFcID, front, back, selectedStack, stackID);
        }
    }

    public void ShowFilteredFlashcardsList(string selectedStack)
    {
        List<FlashcardsModel> allFlashcards = databaseLogic.CreateFlashCardsList();
        List<FlashcardsModel> filteredFlashcards = allFlashcards.Where(obj => obj.StackName == selectedStack).ToList();

        //this loop will set the ID of the filtered flashcards in numeric crescent order for visualization purposes
        int i = 1;
        foreach (FlashcardsModel flashcard in filteredFlashcards)
        {
            flashcard.FcID = i;
            i++;
        }
        Console.WriteLine("Flashcards Table");

        if(filteredFlashcards.Count == 0)
        {
            Console.WriteLine("No flashcards exist for this stack!");
        }

        ConsoleTableBuilder
         .From(filteredFlashcards)
         .ExportAndWriteLine();

        Console.ReadLine();
    }

    public void ShowAllFlashCardsList()
    {
        List<FlashcardsModel> allFlashcards = databaseLogic.CreateFlashCardsList();

        Console.WriteLine("Flashcards Table");

        ConsoleTableBuilder
         .From(allFlashcards)
         .ExportAndWriteLine();
    }

    public void uiDeleteFlashcard()
    {
        Console.WriteLine("Select the flashcard you wish to delete:");
        int fcID = GetAndValidateFlashcardsID();
        databaseLogic.DeleteFlashcard(fcID);
    }

    public void uiUpdateFlashcard()
    {
        Console.WriteLine("Select the flashcard you wish to update:");
        int fcID = GetAndValidateFlashcardsID();

        Console.WriteLine("Write text for the front of the card:");
        string front = Console.ReadLine();

        Console.WriteLine("Write text for the back of the card:");
        string back = Console.ReadLine();

        databaseLogic.UpdateFlashcard(fcID, front, back);
    }

    public int GetAndValidateFlashcardsID()
    {
        int fcID;
        List<FlashcardsModel> allFlashcards = databaseLogic.CreateFlashCardsList();

        while (true)
        {
            Console.WriteLine("Please write the ID (enter a valid ID):");
            string input = Console.ReadLine();

            if (int.TryParse(input, out fcID) && allFlashcards.Where(obj => obj.FcID == fcID).Any())
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
            }
        }
        return fcID;
    }
}
