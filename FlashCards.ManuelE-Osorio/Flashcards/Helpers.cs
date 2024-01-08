namespace Flashcards;

class Helpers
{
    public static void ClearConsole()
    {
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Clear();
    }

    public static void DBPopulate()
    {
        string stacksPath = "StacksData.csv";
        string cardsPath = "FlashCardsData.csv";
        string studysessionsPath = "StudySessionsData.csv";

        if(!DBController.TableIsNotEmpty(DBController.stacksTableName))
        {
            List<Stacks> stacks = File.ReadAllLines(stacksPath)
                .Skip(1)
                .Select(Stacks.FromCsv)
                .ToList();
            
            foreach (Stacks stack in stacks)
            {
                DBController.InsertNewStack(stack);
            }
        }

        if(!DBController.TableIsNotEmpty(DBController.flashcardsTableName))
        {
            List<Cards> cards = File.ReadAllLines(cardsPath)
                .Skip(1)
                .Select(Cards.FromCsv)
                .ToList();

            foreach (Cards card in cards)
            {
                DBController.InsertNewCard(card);
            }
        }

        if(!DBController.TableIsNotEmpty(DBController.studysessionsTableName))
        {
            List<StudySession> studySessions = File.ReadAllLines(studysessionsPath)
                .Skip(1)
                .Select(StudySession.FromCSV)
                .ToList();
            foreach(StudySession studySession in studySessions)
            {
                DBController.InsertNewStudySession(studySession);
            }
        }
    }
}