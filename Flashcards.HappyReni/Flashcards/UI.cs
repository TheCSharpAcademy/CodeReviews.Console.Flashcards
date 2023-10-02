using ConsoleTableExt;

namespace Flashcards
{
    internal class UI
    {
        public UI() {}

        public SELECTOR MainMenu()
        {
            Console.Clear();
            Write("Flashcards");
            Write("".PadRight(24, '='));
            Write("1. Create a Stack");
            Write("2. Manage a Stack");
            Write("3. View all the study sessions.");
            Write("0. Exit\n");
            var selector = (SELECTOR)GetInput("Select ").val;

            return selector;
        }
        public string CreateStack()
        {
            Console.Clear();
            Write("Create a stack");
            Write("".PadRight(24, '='));
            var name = GetInput("Type a name of stack.").str;
            return name;
        }
        public int ManageStack(string name)
        {
            Console.Clear();
            Write($"Current Stack : {name}");
            Write("Choose the number of action you want.\n");
            Write("1. View all flashcards in stack.");
            Write("2. Put a new flashcard into stack.");
            Write("3. Edit a flashcard.");
            Write("4. Delete a flashcard.");
            Write("5. Study current stack.");
            Write("6. Change current stack.");
            Write("7. Delete current stack.");
            Write("0. Return to main menu\n");
            Write("".PadRight(24, '='));

            return GetInput("Select").val;
        }
        public void Write(string text)
        {
            Console.WriteLine(text);
        }
        public void Write(int text)
        {
            Console.WriteLine(text);
        }
        public void MakeTable(List<List<object>> data, string type)
        {
            Console.Clear();
            if (type == "stack")
            {
                ConsoleTableBuilder
                .From(data)
                .WithTitle("Stacks", ConsoleColor.Green)
                .WithColumn("ID", "Name")
                .ExportAndWriteLine();
                Write("".PadRight(24, '='));
            }
            else if(type == "Flashcards")
            {
                ConsoleTableBuilder
                .From(data)
                .WithTitle("Flashcards", ConsoleColor.Green)
                .WithColumn("Front", "Back")
                .ExportAndWriteLine();
                Write("".PadRight(24, '='));
            }
            else
            {
                ConsoleTableBuilder
                .From(data)
                .WithTitle("Sessions", ConsoleColor.Green)
                .WithColumn("StackID", "StartTime", "EndTime", "Score", "Question Count")
                .ExportAndWriteLine();
                Write("".PadRight(24, '='));
            }
            return;
        }

        public SELECTOR GoToMainMenu(string message = "")
        {
            WaitForInput(message);
            return MainMenu();
        }
        public (bool res, string str, int val) GetInput(string message)
        {
            // This function returns string input too in case you need it
            int number;
            Write(message);
            Console.Write(">> ");
            string str = Console.ReadLine();
            var res = int.TryParse(str, out number);

            number = res ? number : (int)SELECTOR.INVALID_SELECT;
            str = str == null ? "" : str;

            return (res, str, number);
        }
        public void WaitForInput(string message = "")
        {
            Write(message);
            Console.ReadKey();
        }
    }
}
