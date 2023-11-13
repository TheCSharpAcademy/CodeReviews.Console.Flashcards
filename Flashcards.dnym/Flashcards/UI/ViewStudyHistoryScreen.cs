using ConsoleTableExt;
using Flashcards.DataAccess;
using Flashcards.DataAccess.DTOs;
using TCSAHelper.Console;
using static TCSAHelper.Console.Utils;

namespace Flashcards.UI;

internal static class ViewStudyHistoryScreen
{
    public static Screen Get(IDataAccess dataAccess, HistoryListItem history)
    {
        const int headerHeight = 1;
        const int footerHeight = Screen.FooterPadding + Screen.FooterSeparatorHeight + 3;
        const int constantListOverhead = 3;
        const int perItemHeight = 2;
        PaginationResult? paginationResult = null;
        int previouslyUsableHeight = -1;
        int skip = 0;

        int resultsCount = history.CardsStudied;

        var screen = new Screen(header: (_, usableHeight) =>
        {
            if (usableHeight != previouslyUsableHeight)
            {
                previouslyUsableHeight = usableHeight;
                skip = 0;
            }
            int heightAvailableToBody = usableHeight - (headerHeight + footerHeight);
            paginationResult = DeterminePagination(heightAvailableToBody, resultsCount, heightPerItem: perItemHeight, perPageListHeightOverhead: constantListOverhead, skippedItems: skip);
            if (paginationResult.TotalPages > 1)
            {
                return $"Study Session for {history.StackViewName} @ {history.StartedAt.ToLocalTime().ToString(Program.DateTimeFormat)} (page {paginationResult.CurrentPage}/{paginationResult.TotalPages})";
            }
            else
            {
                return $"Study Session for {history.StackViewName} @ {history.StartedAt.ToLocalTime().ToString(Program.DateTimeFormat)}";
            }
        }, body: (usableWidth, _) =>
        {
            if (paginationResult!.TotalPages > 0)
            {
                var take = paginationResult!.ItemsPerPage;
                return GetStudyResultsList(dataAccess, history.Id, skip, take, usableWidth);
            }
            else if (resultsCount > 0)
            {
                return "Window is too small to list any results.";
            }
            else
            {
                return "No flashcards answered for this session.";
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

        return screen;
    }

    private static string GetStudyResultsList(IDataAccess dataAccess, int historyId, int skip, int take, int usableWidth)
    {
        int minListWidth = $"| no. | front | {Program.DateTimeFormat} | correct |".Length;
        int frontWidth = Math.Max(usableWidth - minListWidth, 5);
        var tableData = dataAccess.GetStudyResults(historyId, take, skip).Result
            .ConvertAll(sr => new List<object> { sr.Ordinal, TCSAHelper.General.Utils.LimitWidth(sr.Front, frontWidth), sr.AnsweredAt.ToLocalTime().ToString(Program.DateTimeFormat), sr.WasCorrect ? "yes" : "no" })
            ?? new List<List<object>>();
        return ConsoleTableBuilder.From(tableData)
            .AddColumn("no.").AddColumn("front").AddColumn("answered at").AddColumn("correct")
            .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
            .Export().ToString();
    }
}
