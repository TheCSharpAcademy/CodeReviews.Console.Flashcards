namespace Flashcards;

using ConsoleTableExt;

class StackListView : BaseView
{
    private readonly StackController controller;
    private readonly List<StackDto> stacks;

    public StackListView(StackController controller, List<StackDto> stacks)
    {
        this.controller = controller;
        this.stacks = stacks;
    }

    public override void Body()
    {
        Console. WriteLine("Stacks");
        if (stacks != null && stacks.Count > 0)
        {
            ConsoleTableBuilder.From(stacks).ExportAndWriteLine();
        }
        else
        {
            Console.WriteLine("No stacks found.");
        }

        Console.WriteLine("Enter stack name and press enter to select a stack.");
        Console.WriteLine("Press enter alone to return to main menu.");
        var rawInput = Console.ReadLine();
        if (String.IsNullOrEmpty(rawInput))
        {
            controller.BackToMainMenu();
        }
        else
        {
            controller.SelectStack(rawInput);
        }
    }
}