using Flashcards.Model;

namespace Flashcards.Helpers {
    internal class HelpersAndValidation {

        internal static void InsertSeparator() {
            Console.WriteLine("\n-------------------------------------------\n");
        }

        internal static void DealWithError( Exception ex ) {
            Console.WriteLine(ex);
            Console.WriteLine("\n------------\n");
            Console.WriteLine("Something Went wrong! Check what you typed, you might have typed something incorrectly, or check the error log!");
        }

        internal static List<string> GetNames( List<Stack> stacks ) {
            List<string> names = new List<string>();

            foreach (Stack stack in stacks) {
                names.Add(stack.Name);
            }

            return names;
        }
    }
}
