using AutoMapper;

namespace Flashcards
{
    internal class State
    {
        internal static string? selectedStack;
        internal static int selectedStackID;

        internal static Dictionary<int, int> idLookup = new Dictionary<int, int>();
        internal static Dictionary<int, int> idLookup2 = new Dictionary<int, int>();
        internal static IMapper mapper = AutoMapperConfig.InitializeAutoMapper();
    }
}
