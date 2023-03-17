using ConsoleTableExt;
using ObjectsLibrary;

namespace Screens;

internal class StacksMenu
{
    AskInputs.AskInput askInput = new();
    SettingsLibrary.Settings settings = new();
    DbCommandsLibrary.DbCommands dbCmd = new();
    public void View()
    {
        bool exitMenu = false;
        List<object> optionsString = new List<object> {
            "1 - View Stacks",
            "2 - Add new Stack",
            "3 - Update Stack",
            "4 - Delete Stack",
            "0 - Return"
        };
        while (!exitMenu)
        {
            Console.Clear();
            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Stacks")
                .ExportAndWriteLine();
            Console.Write("\n");
            switch (askInput.PositiveNumber("Please select a valid option"))
            {
                case 0: exitMenu = true; continue;
                case 1: ViewStacks(); break;
                case 2: AddStack(); break;
                case 3: UpdateStack(); break;
                case 4: DeleteStack(); break;
                default: break;
            }
            askInput.AnyKeyToContinue();
        }
        return;
    }

    private void ViewStacks()
    {
        Console.Clear();
        List<Stack> stackList = dbCmd.Return.AllStacks();

        DisplayStackList(stackList, "VIEW");

        return;
    }

    private void AddStack()
    {
        string stackName = 
            askInput.AlphasNumbersSpecialUpToLimit(settings.stackNameCharLimit, "Write the name of the stack.");
        Stack newStack = new Stack { Name = stackName };

        if (stackName == "0") return;
        if(dbCmd.Check.StackByName(stackName)) { Console.WriteLine("Stack already exists..."); return; }

        newStack.ViewId = dbCmd.Return.LastViewId(settings.stacksTableName) + 1;
        if (newStack.ViewId == 0+1 ) newStack.ViewId = 1;
        
        if (dbCmd.Insert.IntoTable(newStack)) Console.WriteLine("New stack added successfully!");
        else Console.WriteLine("Couldn't add stack...");
        
        return;
    }

    private void UpdateStack()
    {
        Console.Clear();
        int index;

        List<Stack> stackList = dbCmd.Return.AllStacks();
        DisplayStackList(stackList, "UPDATE");

        do
        {
            index = askInput.PositiveNumber("Write the index of the stack you want to update, or 0 to return");
        }
        while ((index != 0) && (dbCmd.Return.StackByIndex(index) == null));
        if (index == 0) return;

        Stack updatedStack = new Stack {
            Name = askInput.AlphasNumbersSpecialUpToLimit(settings.stackNameCharLimit, "Write the new name of the stack.")
        };

        if (dbCmd.Update.StackByIndex(index,updatedStack)) Console.WriteLine("Stack updated successfully!");
        else Console.WriteLine("Couldn't update stack...");
        return;
    }

    private void DeleteStack()
    {
        Console.Clear();
        int index;
        int viewId;

        DisplayStackList(dbCmd.Return.AllStacks(), "DELETE");

        do
        {
            viewId = askInput.PositiveNumber("Write the index of the stack you want to delete, or 0 to return");
            index = dbCmd.Return.IdFromViewId(settings.stacksTableName, viewId);
        }
        while((viewId != 0) && !dbCmd.Check.StackByIndex(index));

        if (index == 0) return;

        if (dbCmd.Delete.StackByViewId(viewId)) Console.WriteLine("Stack deleted successfully!");
        else Console.WriteLine("Couldn't delete stack...");
        return;
    }

    public void DisplayStackList(List<Stack> stacksToDisplay, string title)
    {
        var tableDataDisplay = new List<List<object>>();

        if (stacksToDisplay is not null)
        {
            foreach (Stack stack in stacksToDisplay)
            {
                tableDataDisplay.Add(
                    new List<object>
                    {
                        stack.ViewId,
                        stack.Name
                    });
            }
        }
        else
        {
            tableDataDisplay.Add(new List<object> { "", ""});
            title = "EMPTY";
        }

        ConsoleTableBuilder.From(tableDataDisplay)
            .WithTitle(title)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithColumn("Id", "Name")
            .ExportAndWriteLine();
        return;
    }
}
