namespace FlashcardsLibrary.DTOs;
public class PackOverviewDTO
{
    public int Id { get; set; }
    public string Name { get; set; }

    public PackOverviewDTO(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
