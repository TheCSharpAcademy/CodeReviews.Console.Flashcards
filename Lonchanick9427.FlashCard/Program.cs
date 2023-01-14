using Lonchanick9427.FlashCard;
using Lonchanick9427.FlashCard.DB;

void NewStack()
{
    Console.Clear();
    Console.WriteLine();
    showStacks();
    Console.WriteLine("Creating a new Stack");
    Deck aux = new Deck();
    aux.Name = ToolBox.GetStringInput("Nombre");
    aux.Description = ToolBox.GetStringInput("Descripcion");
    DeckDB.Add(aux); 
    Console.Clear() ;
    Console.WriteLine("Done!");
    showStacks();
    Console.ReadLine();
}
List<Deck> showStacks()
{
    List<Deck> l = DeckDB.Get();
    ToolBox.DeckPrettyTable(l);
    return l;
}
void DeleteStack()
{
    List<int> i = GetStackIdList(showStacks());
    Console.WriteLine("Pick any Stack (Id) From the list");
    int aux = ToolBox.GetIntInput("Deck Id");

    while (!(i.IndexOf(aux) >= 0))
    {
        Console.WriteLine("The Stack Id provided does not exist! ");
        aux = ToolBox.GetIntInput("Deck-Id");
    }
    DeckDB.Delete(aux);
    Console.Clear();
    Console.WriteLine("Done! Here the new list!");
    showStacks();
    Console.ReadLine();
}
List<int> GetStackIdList(List<Deck> param)
{
    List<int> i = new();
    foreach (var x in param)
        i.Add(x.Id);

    return i;
}
List<int> GetCardIdList(List<FlashCard> param)
{
    List<int> i = new();
    foreach (var x in param)
        i.Add(x.Id);

    return i;
}
void NewCard()
{
    Console.Clear();
    
    List<int> i = GetStackIdList(showStacks());

    Console.WriteLine("Creating a new Card ");
    FlashCard aux = new();
    aux.Front = ToolBox.GetStringInput("Front-Face");
    aux.Back = ToolBox.GetStringInput("Back-Face");
    aux.Fk = ToolBox.GetIntInput("Deck-Id");

    while (!(i.IndexOf(aux.Fk) >= 0))
    {
        Console.WriteLine("The Stack Id provided does not exist! ");
        aux.Fk = ToolBox.GetIntInput("Deck-Id");
    }

    CardDB.Add(aux);
    Console.WriteLine("Done!");
    Console.ReadLine();
}

void DeleteCard()
{
    Console.Clear();

    List<int> i = GetStackIdList(showStacks());
    Console.WriteLine("Pick any Stack (Id) From the list");
    int id = ToolBox.GetIntInput("Stack Id");
    while (!(i.IndexOf(id) >= 0))
    {
        Console.WriteLine("The Stack Id provided does not exist! ");
        id = ToolBox.GetIntInput("Deck-Id");
    }
    Console.Clear();
    Console.WriteLine($"This cards belong to Stack Id: {id}");
    List<FlashCard> list = CardDB.CardsByStackId(id);
    ToolBox.CardPrettyTable(list);
    Console.Write("Type an Falsh Card Id to delete. ");
    int id2 = ToolBox.GetIntInput("");
    List<int> idList = GetCardIdList(list);
    while (!(idList.IndexOf(id2) >= 0))
    {
        Console.WriteLine("The Card Id provided does not exist! ");
        id2 = ToolBox.GetIntInput("Card-Id");
    }
    if (CardDB.Delete(id2))
        Console.Write("done!");

    Console.ReadLine();
}

while (true)
{
    Console.Clear();
    Console.WriteLine("--- Main Menu ---");
    Console.WriteLine("\t STACK MENU");
    Console.WriteLine("1. Create a new Stack");
    Console.WriteLine("2. Show Stacks");
    Console.WriteLine("3. Delete an Stack");
    Console.WriteLine("4. Update an Stack\n");

    Console.WriteLine("\t CARDS MENU");
    Console.WriteLine("5. Create a new Card");
    Console.WriteLine("6. Delete a card");
    Console.WriteLine("7. Update a Card\n");

    Console.WriteLine("\t STUDY SESSIONS MENU");
    Console.WriteLine("8. Start a New Study Session");
    Console.WriteLine("9. Show all Study Sessions");
    Console.WriteLine("10. Delete a Study Session");
    Console.WriteLine("0. Exit\n");
    Console.Write("Enter your choice: ");

    string input = Console.ReadLine();

    switch (input)
    {
        case "1":
            Console.Clear();
            NewStack();
            break;
        case "2":
            Console.Clear();
            showStacks();
            Console.ReadLine();
            break;
        case "3":
            Console.Clear();
            DeleteStack();
            break;
        case "4":
            Console.WriteLine("Update-stack PENDIENTE");
            break;
        case "5":
            NewCard();
            break;
        case "6":
            DeleteCard();
            //DELETE CARD
            break;
        case "7":
            //UPDATE
            break;
        case "8":
            //START A NEW SESSION STUDY
            break;
        case "9":
            //SHOW SESSION STUDY
            break;
        case "10":
            //DELETE SESSION STUDY
            break;
        case "0":
            return;
        default:
            Console.Clear();
            Console.WriteLine("Invalid input.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            break;
    }
}
