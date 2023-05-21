using FlashcardsLibrary.Models;

namespace Flashcards.CoreyJordan.DTOs;
public class PackNamesDTO
{
    public int Number { get; set; }
    public string Name { get; set; }

    public PackNamesDTO(int index, PackModel pack)
    {
        Number = index;
        Name = pack.Name;
    }

    public static List<PackNamesDTO> GetPacksDTO(List<PackModel> allPacks)
    {
        List<PackNamesDTO> names = new();
        for (int i = 0; i < allPacks.Count; i++)
        {
            names.Add(new PackNamesDTO(i + 1, allPacks[i]));
        }
        return names;
    }
}