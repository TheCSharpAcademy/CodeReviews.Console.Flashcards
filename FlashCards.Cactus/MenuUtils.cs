using ConsoleTableExt;

namespace FlashCards.Cactus
{
    public class MenuUtils
    {
        public static void PrintMenu(string name, List<String> menu)
        {
            Console.Clear();

            ConsoleTableBuilder
            .From(menu)
            .WithTitle(name, ConsoleColor.Yellow, ConsoleColor.DarkGray)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);
        }
    }
}
