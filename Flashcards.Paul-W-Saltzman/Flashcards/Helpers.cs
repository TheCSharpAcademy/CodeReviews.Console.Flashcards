using ConsoleTableExt;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace Flashcards
{
    internal static class Helpers
    {

        internal static void setEnvironmentVariables()
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr GetStdHandle(int nStdHandle);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern int SetConsoleOutputCP(uint wCodePageID);

            // Set the console font to Lucida Console (or any font that supports Unicode)
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            // Set the console output code page to UTF-8
            SetConsoleOutputCP(65001);

        }

        internal static string Sanitize(string toSanitize)
        {
            //the assignment is to work with sql direct with the database this is my very humble attempt to stop the most obvious sql injections attacks 
            // This will not capture everything.
            toSanitize = toSanitize.Trim();
            toSanitize = toSanitize.ToLower();
            int originalLength = toSanitize.Length;
            toSanitize = toSanitize.Replace("'", "''");
            toSanitize = toSanitize.Trim();
            string sanitized = toSanitize;
            int finalLength = sanitized.Length;
            if (originalLength != finalLength)
            { Console.WriteLine("Your input has been sanitized."); }
            else { }
            return sanitized;
        }

        internal static bool CompareStrings(string string1, string string2)
        {
            bool match = false;
            string1 = string1.Trim().ToLower();
            string2 = string2.Trim().ToLower();
            string1 = Regex.Replace(string1, @"\s", "");
            string2 = Regex.Replace(string2, @"\s", "");
            if (string1 == string2) { match = true; }
            return match;
        }
        internal static void ShowTable(DataTable dataTable, String title)
        {
            
            ConsoleTableBuilder
            .From(dataTable)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithCharMapDefinition(
                    CharMapDefinition.FramePipDefinition,
                    new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                    })
                .WithTitle(title, ConsoleColor.Green, ConsoleColor.Black, TextAligntment.Center)
                .ExportAndWriteLine(TableAligntment.Center);
        }

        internal static void ViewCard(DTO_StackAndCard card, bool front)
        {
            string cardText = "";
            string cardSide = "";
            if (front)
            {
                if (card.CardFront.IsNullOrEmpty()) { }
                else
                {
                    cardText = card.CardFront;
                    cardSide = "front";
                }
            }
            else if (!front)
            {
                if (card.CardBack.IsNullOrEmpty()) { }
                else
                {
                    cardText = card.CardBack;
                    cardSide = "back";
                }
            }

            String numberLine = $"{card.CardNumberInStack}";
            int numberLineCount = 49 - numberLine.Length;
            int textLineCount1 = (50 - cardText.Length) / 2;
            int textLineCount2 = textLineCount1;
            int sideLineCount1 = (50 - cardSide.Length) / 2;
            int sideLineCount2 = sideLineCount1;
            while (textLineCount1 + textLineCount2 + cardText.Length < 50)
            {
                textLineCount2++;
            }
            while (sideLineCount1 + sideLineCount2 + cardSide.Length < 50)
            {
                sideLineCount2++;
            }
            string numberLinePad = new string(' ', numberLineCount);
            string textLinePad1 = new string(' ', textLineCount1);
            string textLinePad2 = new string(' ', textLineCount2);
            string sideLinePad1 = new string('_', sideLineCount1);
            string sideLinePad2 = new string('_', sideLineCount2);


            Console.WriteLine(" __________________________________________________ ");
            Console.WriteLine($"| {numberLine}{numberLinePad}|");
            Console.WriteLine("|                                                  |");
            Console.WriteLine($"|{textLinePad1}{cardText}{textLinePad2}|");
            Console.WriteLine("|                                                  |");
            Console.WriteLine($"|{sideLinePad1}{cardSide}{sideLinePad2}|");



        }

        internal static void ViewCard(Card card, bool front)
        {
            string cardText = "";
            string cardSide = "";
            if (front)
            {
                if (card.Front.IsNullOrEmpty()) { }
                else
                {
                    cardText = card.Front;
                    cardSide = "front";
                }
            }
            else if (!front)
            {
                if (card.Back.IsNullOrEmpty()) { }
                else
                {
                    cardText = card.Back;
                    cardSide = "back";
                }
            }

            String numberLine = $"{card.NoInStack}";
            int numberLineCount = 49 - numberLine.Length;
            int textLineCount1 = (50 - cardText.Length) / 2;
            int textLineCount2 = textLineCount1;
            int sideLineCount1 = (50 - cardSide.Length) / 2;
            int sideLineCount2 = sideLineCount1;
            while (textLineCount1 + textLineCount2 + cardText.Length < 50)
            {
                textLineCount2++;
            }
            while (sideLineCount1 + sideLineCount2 + cardSide.Length < 50)
            {
                sideLineCount2++;
            }
            string numberLinePad = new string(' ', numberLineCount);
            string textLinePad1 = new string(' ', textLineCount1);
            string textLinePad2 = new string(' ', textLineCount2);
            string sideLinePad1 = new string('_', sideLineCount1);
            string sideLinePad2 = new string('_', sideLineCount2);


            Console.WriteLine(" __________________________________________________ ");
            Console.WriteLine($"| {numberLine}{numberLinePad}|");
            Console.WriteLine("|                                                  |");
            Console.WriteLine($"|{textLinePad1}{cardText}{textLinePad2}|");
            Console.WriteLine("|                                                  |");
            Console.WriteLine($"|{sideLinePad1}{cardSide}{sideLinePad2}|");

        }
    }
}
