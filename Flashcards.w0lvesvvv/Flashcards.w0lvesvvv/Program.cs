using Flashcards.w0lvesvvv.DTOs;
using Flashcards.w0lvesvvv.Models;
using Flashcards.w0lvesvvv.Persistance;
using Flashcards.w0lvesvvv.Utils;

DataBaseManager.CreateDatabase();
DataBaseManager.CreateTables();

Dictionary<CardDTO, int> orderedCarts = new();

do
{
    string option = DisplayMenu();

    switch (option)
    {
        case "1":
            Study();
            break;
        case "2":
            GetStudies();
            break;
        case "3":
            DisplayStacks();
            break;
        case "4":
            InsertStack();
            break;
        case "5":
            UpdateStack();
            break;
        case "6":
            DeleteStack();
            break;
        case "7":
            DisplayCards(out string cardStackName);
            break;
        case "8":
            InsertCard();
            break;
        case "9":
            UpdateCard();
            break;
        case "10":
            DeleteCard();
            break;
        case "0":
            Environment.Exit(0);
            break;
    }

} while (true);


#region Menu
string DisplayMenu()
{
    ConsoleUtils.SetColor(ConsoleColor.Green);
    Console.WriteLine();
    Console.WriteLine("|==============================|");
    Console.WriteLine("|           FlashCard          |");
    Console.WriteLine("|==============================|");
    Console.WriteLine("|   1 - Start study session    |");
    Console.WriteLine("|   2 - Show study sessions    |");
    Console.WriteLine("|   3 - Show stacks            |");
    Console.WriteLine("|   4 - Insert stack           |");
    Console.WriteLine("|   5 - Update stack           |");
    Console.WriteLine("|   6 - Delete stack           |");
    Console.WriteLine("|   7 - Show stack cards       |");
    Console.WriteLine("|   8 - Insert card            |");
    Console.WriteLine("|   9 - Update card            |");
    Console.WriteLine("|  10 - Delete card            |");
    Console.WriteLine("|   0 - Exit                   |");
    Console.WriteLine("|==============================|");

    Console.WriteLine();
    return UserInput.RequestString("Option selected: ");
}
#endregion

#region Stack
bool DisplayStacks()
{

    List<Stack> listStacks = DataBaseManager.GetStacks();

    if (listStacks != null && listStacks.Any())
    {
        Console.WriteLine();
        ConsoleUtils.SetColor(ConsoleColor.Yellow);
        foreach (var stack in listStacks)
        {
            Console.WriteLine($"- {stack.StackName}");
        }
        return true;
    }
    else
    {
        Console.Clear();
        ConsoleUtils.DisplayMessage("There aren't stacks registered. Try creating one first.", messageColor: ConsoleColor.Red);
        return false;
    }
}

void InsertStack()
{
    Console.WriteLine();
    string stackName = UserInput.RequestString("Stack name: ");

    if (string.IsNullOrWhiteSpace(stackName)) return;

    if (stackName.Length > 250)
    {
        Console.Clear();
        ConsoleUtils.DisplayMessage("Stack name max length 250.", messageColor: ConsoleColor.Red);
    }

    if (DataBaseManager.InsertStack(stackName))
    {
        Console.Clear();
        ConsoleUtils.DisplayMessage("Stack created.", messageColor: ConsoleColor.Yellow);
    }
    else
    {
        Console.WriteLine();
        ConsoleUtils.DisplayMessage("Stack name is already in use.", messageColor: ConsoleColor.Red);
    }
}

void UpdateStack()
{
    if (!DisplayStacks()) return;
    Console.WriteLine();

    string oldName = UserInput.RequestString("Introduce stack name to update: ");
    if (string.IsNullOrWhiteSpace(oldName)) return;

    string newName = UserInput.RequestString("Introduce new stack name: ");
    if (string.IsNullOrWhiteSpace(newName)) return;

    Console.Clear();
    if (!DataBaseManager.UpdateStack(oldName, newName))
    {
        ConsoleUtils.DisplayMessage($"There isn't any stack named {oldName}", messageColor: ConsoleColor.Red); return;
    }

    ConsoleUtils.DisplayMessage("Stack updated.", messageColor: ConsoleColor.Yellow);
}

void DeleteStack()
{
    if (!DisplayStacks()) return;
    Console.WriteLine();

    string stackName = UserInput.RequestString("Introduce stack name to delete: ");
    if (string.IsNullOrWhiteSpace(stackName)) return;

    Console.Clear();

    if (!DataBaseManager.DeleteStack(stackName))
    {
        ConsoleUtils.DisplayMessage($"There isn't any stack named {stackName}", messageColor: ConsoleColor.Red); return;
    }

    ConsoleUtils.DisplayMessage("Stack deleted.", messageColor: ConsoleColor.Yellow);

}
#endregion

#region Card
bool DisplayCards(out string cardStackName)
{
    cardStackName = string.Empty;
    if (!DisplayStacks()) return false;
    Console.WriteLine();

    cardStackName = UserInput.RequestString("Introduce stack name: ");
    if (string.IsNullOrWhiteSpace(cardStackName)) return false;

    List<Card> listCards = DataBaseManager.GetCards(cardStackName);

    if (listCards == null || !listCards.Any())
    {
        Console.Clear();
        ConsoleUtils.DisplayMessage("There aren't study sessions registered. Try creating one first.", messageColor: ConsoleColor.Red);
        return false;
    }

    Console.WriteLine();
    ConsoleUtils.SetColor(ConsoleColor.Yellow);
    int counter = 1;

    orderedCarts = new();
    foreach (Card card in listCards)
    {
        orderedCarts.Add(new CardDTO { CardId = counter++, CardQuestion = card.CardQuestion, CardAnswer = card.CardAnswer }, card.CardId);
    }
    TableBuilder.BuildTable(orderedCarts.Keys.ToList());
    return true;
}

void InsertCard()
{
    if (!DisplayStacks()) return;
    Console.WriteLine();

    string cardStackName = UserInput.RequestString("Introduce stack name: ");
    if (string.IsNullOrWhiteSpace(cardStackName)) return;

    string cardQuestion = UserInput.RequestString("Introduce question: ");

    string cardAnswer = UserInput.RequestString("Introduce answer: ");

    var result = DataBaseManager.InsertCard(cardStackName, cardQuestion, cardAnswer);

    switch (result)
    {
        case 1:
            Console.Clear();
            ConsoleUtils.SetColor(ConsoleColor.Yellow);
            Console.WriteLine("Card created.");
            break;
        case -1:
            ConsoleUtils.SetColor(ConsoleColor.Red);
            Console.WriteLine("Stack name doesn't exist.");
            break;
        case -2:
            ConsoleUtils.SetColor(ConsoleColor.Red);
            Console.WriteLine("Card question already created.");
            break;
        default:
            ConsoleUtils.SetColor(ConsoleColor.Red);
            Console.WriteLine("Server error.");
            break;
    }
}

void UpdateCard()
{
    if (!DisplayCards(out string cardStackName)) return;
    Console.WriteLine();

    int? cardId = UserInput.RequestNumber("Introduce card id to update: ");
    if (cardId == null) return;

    CardDTO? card = orderedCarts.Keys.FirstOrDefault(x => x.CardId == cardId);
    if (card != null && orderedCarts.TryGetValue(card, out int id)) { cardId = id; }

    string cardQuestion = UserInput.RequestString("Introduce new question: ");
    if (string.IsNullOrWhiteSpace(cardQuestion)) return;

    string cardAnswer = UserInput.RequestString("Introduce new answer: ");
    if (string.IsNullOrWhiteSpace(cardAnswer)) return;

    Console.Clear();
    if (!DataBaseManager.UpdateCard(cardId.Value, cardStackName, cardQuestion, cardAnswer))
    {
        ConsoleUtils.DisplayMessage($"There is already one card with that question in the stack {cardStackName}", messageColor: ConsoleColor.Red); return;
    }

    ConsoleUtils.DisplayMessage("Card updated.", messageColor: ConsoleColor.Yellow);
}

void DeleteCard()
{
    if (!DisplayCards(out string cardStackName)) return;
    Console.WriteLine();

    int? cardId = UserInput.RequestNumber("Introduce card id to update: ");
    if (cardId == null) return;

    CardDTO? card = orderedCarts.Keys.FirstOrDefault(x => x.CardId == cardId);
    if (card != null && orderedCarts.TryGetValue(card, out int id)) { cardId = id; }

    Console.Clear();

    if (!DataBaseManager.DeleteCard(cardId.Value))
    {
        ConsoleUtils.DisplayMessage($"There isn't any card with that id on the stack {cardStackName}", messageColor: ConsoleColor.Red); return;
    }

    ConsoleUtils.DisplayMessage("Card deleted.", messageColor: ConsoleColor.Yellow);

}
#endregion

#region StudySession
void Study()
{
    if (!DisplayStacks()) return;
    Console.WriteLine();

    string stackName = UserInput.RequestString("Introduce stack name: ");
    if (string.IsNullOrWhiteSpace(stackName)) return;

    List<Card> listCards = DataBaseManager.GetCards(stackName);

    if (listCards == null || !listCards.Any())
    {
        Console.Clear();
        ConsoleUtils.DisplayMessage("There aren't cards registered. Try creating one first.", messageColor: ConsoleColor.Red);
        return;
    }

    Console.Clear();
    int points = 0;
    List<CardErrorDTO> listErrors = new();

    foreach (var card in listCards)
    {
        ConsoleUtils.DisplayMessage($"Question: {card.CardQuestion}");
        var answer = UserInput.RequestString("Answer: ");
        if (answer.Equals(card.CardAnswer))
        {
            points++;
        }
        else
        {
            listErrors.Add(new CardErrorDTO
            {
                CardQuestion = card.CardQuestion,
                CardAnswer = card.CardAnswer,
                CardWrongAnswer = answer
            });
        }
    }

    StudySession session = new StudySession
    {
        StudySessionDate = DateTime.Now,
        StudySessionPoints = points,
        StudySessionMaxPoints = listCards.Count
    };

    if (session.StudySessionPoints < session.StudySessionMaxPoints)
    {
        Console.WriteLine();
        ConsoleUtils.DisplayMessage("Errors:", messageColor: ConsoleColor.Yellow);
        TableBuilder.BuildTable(listErrors);
    }

    Console.WriteLine();
    ConsoleUtils.DisplayMessage($"Score: {session.StudySessionPoints}/{session.StudySessionMaxPoints}", messageColor: ConsoleColor.Yellow);

    DataBaseManager.InsertStudySession(session, stackName);
}

void GetStudies()
{
    if (!DisplayStacks()) return;
    Console.WriteLine();

    string stackName = UserInput.RequestString("Introduce stack name: ");
    if (string.IsNullOrWhiteSpace(stackName)) return;

    List<StudySession> listSessions = DataBaseManager.GetStudySessions(stackName);

    if (listSessions == null || !listSessions.Any())
    {
        Console.Clear();
        ConsoleUtils.DisplayMessage("There aren't sessions registered. Try creating one first.", messageColor: ConsoleColor.Red);
        return;
    }

    Console.WriteLine();
    ConsoleUtils.SetColor(ConsoleColor.Yellow);
    TableBuilder.BuildTable(listSessions.Select(session => new StudySessionDTO { StudySessionDate = session.StudySessionDate, StudySessionPoints = session.StudySessionPoints, StudySessionMaxPoints = session.StudySessionMaxPoints }).ToList());
}
#endregion
