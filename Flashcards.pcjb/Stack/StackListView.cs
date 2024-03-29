namespace Flashcards;

using ConsoleTableExt;

class StackListView : BaseView
{
    private readonly StackController controller;
    private readonly List<StackDto> stacks;
    private StackSelectionMode mode = StackSelectionMode.None;

    public StackListView(StackController controller, List<StackDto> stacks)
    {
        this.controller = controller;
        this.stacks = stacks;
    }

    public void SetStackSelectionMode(StackSelectionMode mode)
    {
        this.mode = mode;
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

        if (StackSelectionMode.None != mode)
        {
            Console.WriteLine("Enter stack name and press enter to select a stack.");
        }
        
        Console.WriteLine("Press enter alone to return to menu.");
        var rawInput = Console.ReadLine();
        if (String.IsNullOrEmpty(rawInput))
        {
            controller.ShowMenu();
        }
        else
        {
            controller.SelectStack(rawInput, mode);
        }
    }
}