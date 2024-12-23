using System.Data.Common;
using System.Security.Cryptography.X509Certificates;
using Azure.Core.GeoJson;
using Microsoft.Data.SqlClient;

public static class Logic
{
    //Create Flash Card
    public static FlashCardModel CreateFlashCard(string Name, string Definition, int Id = 0, int StackId = 0)
    {
        FlashCardModel FlashCard = new FlashCardModel();
        FlashCard.Name = Name;
        FlashCard.Definition = Definition;
        FlashCard.Id = Id;
        FlashCard.StackId = StackId;
        
        return  FlashCard;
    }
    public static FlashCardModel SaveFlashCard(FlashCardModel model)
    {
        //int stackID = DBController.QueryStackID(DBController.ConnectDB(),model.Name);
        DBController.InsertFlashCard(DBController.ConnectDB(),model.Name,model.Definition,model.StackId);
        return model;
    }


    // //Delete Flash Cards
    // public static void DeleteFlashCard(int Id)
    // {
        
    //     return;
    // }
    //update FlashCard
    // public static FlashCardModel UpdateFlashCard(string Name, string Definition, int Id)
    // {
    //     //Get FlashCard from DB

    //     //Set FlashCard
    //     FlashCardModel FlashCard = new FlashCardModel();
    //     FlashCard.Name = Name;
    //     FlashCard.Definition = Definition;
    //     FlashCard.Id = Id;

    //     //Save Flash Card to db
    //     return  FlashCard;
    // }

    //update FlashCard
    public static void UpdateFlashCard()
    {
        //view all flash cards
        List<FlashCardDto> flashCardDtos = DBController.ViewAllFlashCards(DBController.ConnectDB());
        Console.WriteLine("Enter the ID of the Flashcard you want to edit");
        int position = 0;
        int flashcardIDInt = 0;
        while (true)
        {
            string flashcardID = Console.ReadLine().Trim();
            bool validID = int.TryParse(flashcardID, out position);
            if (validID)
            {
                flashcardIDInt = flashCardDtos.Where(x => x.Position == position).First().Id;
                break;
            }
        }
        Console.WriteLine("Enter the new Flashcard name");
        string name = Console.ReadLine();
        Console.WriteLine("Enter the new Flashcard definition");
        string definition = Console.ReadLine();
        Console.WriteLine("Enter the new stack");
        string stackName = "";
        SqlConnection connection = DBController.ConnectDB();
        while(true)
        {
            stackName = Console.ReadLine();
            if (Logic.StackExists(stackName))
            {
                break;
            }
            else
            {
                Console.WriteLine("This stack does not exist. Do you want to create this?");
                {
                    if (Console.ReadLine().ToUpper().Trim()=="Y")
                    {
                        DBController.InsertStack(connection, stackName);
                    }
                    else
                        Console.WriteLine("Please enter a valid stack.");
                }
            }
        }
        int stackID = DBController.QueryStackID(connection,stackName);
        DBController.UpdateFlashCard(connection,flashcardIDInt,name,definition, stackID);
    }
    
    //Delete FlashCard
    public static void DeleteFlashCard()
    {
        //  view all flash cards
        List<FlashCardDto> FlashCardDto = DBController.ViewAllFlashCards(DBController.ConnectDB());
        int position = 0;
        int flashcardID = 0;
        while (true)
        {
            Console.WriteLine("Enter the ID of the flashcard to delete");
            bool validInt = int.TryParse(Console.ReadLine().Trim(), out position);
            if (validInt)
                {
                    flashcardID = FlashCardDto.Where(x => x.Position == position).First().Id;
                    break;
                }
        }
        DBController.DeleteFlashCard(DBController.ConnectDB(), flashcardID);
    }

    //Check if FlashCard Exists
    public static bool StackExists(string Stack)
    {
        //check if stack is in the stack table
        //return false if not
        List<string> stackList  = DBController.QueryStacks(DBController.ConnectDB());
        if (stackList.Contains(Stack.ToUpper().Trim()))
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    //Pick random stack of cards
    public static List<FlashCardModel> CreateQuizCards(List<FlashCardModel> stack, int cardNumber = 0)
    {
        List<FlashCardModel> result = new List<FlashCardModel>();
        List<int> ids = new List<int>();
        Random rand = new Random();
        if (cardNumber > stack.Count())
        {
            cardNumber = stack.Count();
        }
        else if (cardNumber == 0)
        {
            cardNumber = stack.Count();
        }
        
        for (int i = 0; i<cardNumber; i++)
        {
            while(true)
            {
                
                int id = rand.Next(cardNumber);
                if (!ids.Contains(id))
                {
                    ids.Add(id);
                    break;
                }
                else if (i==cardNumber)
                {
                    break;
                }
                
            }
        }
        
        foreach (int i in ids)
        {
            result.Add(stack[i]);
        }

        return result;
    }
    
    public static SessionModel StudySession(List<FlashCardModel> quiz)
    {
        SessionModel session = new SessionModel();
        
        decimal correct = 0;
        session.FlashCards = quiz;
        session.StackId = quiz[0].StackId;
        session.Date = DateTime.Now.ToString("MM/DD/yyyy h:mm:ss");
        foreach (FlashCardModel card in quiz)
        {
            Console.WriteLine($"What is the definiation of {card.Name}?");
            string answer = Console.ReadLine().ToUpper().Trim();       
            
            if (answer == card.Definition.ToUpper().Trim())
            {
                Console.WriteLine("Correct Answer!");
                correct+=1;
            }
            else
            {
                Console.WriteLine("Wrong Answer!");
            }
        }
        session.Score = $"{correct}/{quiz.Count()}";
        return session;
    }



    //write quiz results to database

    //read quiz results
}