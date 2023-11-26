using ConsoleTableExt;
using Flashcards.Core;
using Flashcards.DataAccess;
using TCSAHelper.Console;
using static TCSAHelper.Console.Utils;

namespace Flashcards.UI;

internal static class StudySessionMenu
{
    public static Screen Get(IDataAccess dataAccess)
    {
        const int headerHeight = 1;
        // Actual footer height varies, but using a constant simplifies things (including for the user).
        const int footerHeight = Screen.FooterPadding + Screen.FooterSeparatorHeight + 3;
        const string promptText = "\nSelect a Stack: ";
        const int constantListOverhead = 3;
        const int perItemHeight = 2;
        const int promptHeight = 2;
        PaginationResult? paginationResult = null;
        int previouslyUsableHeight = -1;
        int skip = 0;

        int stackCount = dataAccess.CountStacksAsync().Result;

        Screen screen = new(header: (_, usableHeight) =>
        {
            if (usableHeight != previouslyUsableHeight)
            {
                // Reset pagination when the window size changes.
                previouslyUsableHeight = usableHeight;
                skip = 0;
            }
            int heightAvailableToBody = usableHeight - (headerHeight + footerHeight);
            paginationResult = DeterminePagination(heightAvailableToBody, stackCount, heightPerItem: perItemHeight, perPageListHeightOverhead: constantListOverhead + promptHeight, skippedItems: skip);
            if (paginationResult.TotalPages > 1)
            {
                return $"Study a Stack (page {paginationResult.CurrentPage}/{paginationResult.TotalPages})";
            }
            else
            {
                return "Study a Stack";
            }
        }, body: (usableWidth, _) =>
        {
            if (paginationResult!.TotalPages > 0)
            {
                var take = paginationResult.ItemsPerPage;
                return GetStackList(dataAccess, skip, take, usableWidth) + promptText;
            }
            else if (stackCount > 0)
            {
                // Note that this may actually not be true due to reserving space for PageUp and PageDown hints.
                // TODO: Consider whether always printing the PageUp and PageDown hints, to not annoy the user by refusing to print items when there is space.
                return "Window is too small to list any stacks.";
            }
            else
            {
                return "No stacks stored.";
            }
        }, footer: (_, _) =>
        {
            var footerText = "";
            if (paginationResult!.CurrentPage > 1)
            {
                footerText += "[PgUp] to go to the previous page,\n";
            }
            if (paginationResult.CurrentPage < paginationResult.TotalPages)
            {
                footerText += "[PgDown] to go to the next page,\n";
            }
            if (footerText.Length > 0)
            {
                footerText += "or ";
            }
            return $"Press {footerText}[Esc] to go back.";
        });

        void PromptHandler(string userInput)
        {
            var stackName = StackManagement.CreateSortName(userInput);
            var stack = dataAccess.GetStackListItemBySortNameAsync(stackName).Result;
            if (stack != null)
            {
                StudyStackScreen.Get(dataAccess, stack.Id, stack.ViewName).Show();
            }
            else
            {
                Console.Beep();
            }
        }

        screen.AddAction(ConsoleKey.PageUp, () =>
        {
            if (paginationResult!.CurrentPage > 1)
            {
                skip -= paginationResult.ItemsPerPage;
            }
        });
        screen.AddAction(ConsoleKey.PageDown, () =>
        {
            if (paginationResult!.CurrentPage < paginationResult.TotalPages)
            {
                skip += paginationResult.ItemsPerPage;
            }
        });
        screen.AddAction(ConsoleKey.Escape, screen.ExitScreen);

        if (stackCount > 0)
        {
            screen.SetPromptAction(PromptHandler);
        }

        return screen;
    }

    private static string GetStackList(IDataAccess dataAccess, int skip, int take, int usableWidth)
    {
        int minListWidth = $"| name | cards | {Program.DateTimeFormat} |".Length;
        int nameWidth = Math.Max(usableWidth - minListWidth, 4);
        var tableData = dataAccess.GetStackListAsync(take, skip).Result
            .ConvertAll(si => new List<object> { TCSAHelper.General.Utils.LimitWidth(si.ViewName, nameWidth), si.Cards, si.LastStudied?.ToLocalTime().ToString(Program.DateTimeFormat) ?? "(never)" })
            ?? new List<List<object>>();
        return ConsoleTableBuilder.From(tableData)
            .AddColumn("name").AddColumn("cards").AddColumn("last studied")
            .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
            .Export().ToString();
    }
}
