using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.DTOs;
public class PackNamesDto
{
    public int Number { get; set; }
    public string Name { get; set; }

    public PackNamesDto(int index, PackModel pack)
    {
        Number = index;
        Name = pack.Name;
    }

    public static List<PackNamesDto> GetPacksDto(List<PackModel> allPacks)
    {
        List<PackNamesDto> names = new();
        for (int i = 0; i < allPacks.Count; i++)
        {
            names.Add(new PackNamesDto(i + 1, allPacks[i]));
        }
        return names;
    }
}