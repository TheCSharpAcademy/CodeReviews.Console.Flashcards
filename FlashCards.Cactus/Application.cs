using ConsoleTableExt;

namespace FlashCards.Cactus
{
    public class Application
    {
        public void ManageStacks()
        {
            Console.Clear();

            while (true)
            {
                PrintStackManagementMenu();

                string? op = Console.ReadLine();
                switch (op)
                {
                    case "0":
                        return;
                    case "1":
                        ShowStacks();
                        break;
                    case "2":
                        AddStack();
                        break;
                    case "3":
                        DeleteStack();
                        break;
                    default:
                        break;
                }
                Console.WriteLine("\n");
            }
        }

        private void PrintStackManagementMenu()
        {
            Console.Clear();

            List<string> stackMenu = new List<string>
                {
                    "<0> Back to Main menu.",
                    "<1> Show Stacks.",
                    "<2> Add a new Stack.",
                    "<3> Delete a Stack.",
                };

            ConsoleTableBuilder
            .From(stackMenu)
            .WithTitle("Stack Menu", ConsoleColor.Yellow, ConsoleColor.DarkGray)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);
        }

        private void DeleteStack()
        {
            throw new NotImplementedException();
        }

        private void AddStack()
        {
            throw new NotImplementedException();
        }

        private void ShowStacks()
        {
            throw new NotImplementedException();
        }
    }
}
