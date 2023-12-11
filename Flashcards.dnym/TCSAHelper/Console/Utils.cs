namespace TCSAHelper.Console;
public static class Utils
{
    public class PaginationResult
    {
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

    private static readonly PaginationResult _paginationErrorResult = new() { ItemsPerPage = 0, CurrentPage = 0, TotalPages = 0 };

    public static PaginationResult DeterminePagination(int heightAvailableToList, int listLength, int heightPerItem = 1, int perPageListHeightOverhead = 0, int skippedItems = 0)
    {
        if (heightAvailableToList < 1) return _paginationErrorResult;
        if (listLength < 0) throw new ArgumentException("List length must be a non-negative number.");
        if (heightPerItem < 1) throw new ArgumentException("Height per item must be a positive number.");
        if (perPageListHeightOverhead < 0) throw new ArgumentException("Per page list height overhead must be a non-negative number.");
        if (skippedItems < 0) throw new ArgumentException("Skipped items must be a non-negative number.");

        if (heightAvailableToList < (perPageListHeightOverhead + heightPerItem))
        {
            // If there's not enough room for even one item...
            return new PaginationResult
            {
                ItemsPerPage = 0,
                CurrentPage = 0,
                TotalPages = 0
            };
        }

        int itemsPerPage = (heightAvailableToList - perPageListHeightOverhead) / heightPerItem;
        int totalPages = (int)Math.Ceiling((double)listLength / itemsPerPage);
        int currentPage = (int)Math.Ceiling((double)skippedItems / itemsPerPage) + 1;

        return new PaginationResult
        {
            ItemsPerPage = itemsPerPage,
            CurrentPage = currentPage,
            TotalPages = totalPages
        };
    }

    internal static void ClearRestOfLine(char c = ' ')
    {
        var currentLine = System.Console.CursorTop;
        var currentColumn = System.Console.CursorLeft;
        System.Console.Write(new string(c, System.Console.WindowWidth - currentColumn));
        System.Console.SetCursorPosition(currentColumn, currentLine);
    }
}
